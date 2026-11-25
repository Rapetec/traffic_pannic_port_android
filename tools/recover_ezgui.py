#!/usr/bin/env python3
"""Recupera los datos de PackedSprite y EZScreenPlacement que AssetRipper no supo leer.

AssetRipper 1.1.1 lee los MonoBehaviour con las reglas de serialización de Unity 4.5+, y en
dos scripts de EZ GUI eso no cuadra con lo que guardó Unity 3.5:
  * PackedSprite: tiene un campo `public SPRITE_FRAME staticFrameInfo` (un struct). Unity 3.5
    no serializaba structs propios, así que en el binario no está y AssetRipper se desalinea.
  * EZScreenPlacement: el binario lleva 4 bytes más al final de lo que AssetRipper espera.
AssetRipper deja esos componentes vacíos (solo cabecera), así que en el port todos los
PackedSprite salen sin textura ni tamaño y todos los EZScreenPlacement colocan su objeto en la
esquina superior izquierda, pegado a la cámara. Entre ellos, los botones del menú.

Este script lee los bytes originales con UnityPy, los decodifica con el formato de Unity 3.5
(cada bool ocupa 4 bytes, sin structs) y reescribe esos componentes en las escenas exportadas.
Los fileID de AssetRipper coinciden con los pathID originales, así que la correspondencia es
directa.

Uso: python3 recover_ezgui.py <APK>/assets/bin/Data <Proyecto>/Assets
"""
import math
import os
import re
import struct
import sys

import UnityPy


class R:
    def __init__(self, b):
        self.b = b
        self.o = 0

    def i32(self):
        v = struct.unpack_from('<i', self.b, self.o)[0]
        self.o += 4
        return v

    def f32(self):
        v = struct.unpack_from('<f', self.b, self.o)[0]
        self.o += 4
        return v

    def boolean(self):
        v = self.b[self.o]
        self.o += 4  # Unity 3.5: bool + alineado a 4
        return 1 if v else 0

    def string(self):
        n = self.i32()
        s = self.b[self.o:self.o + n].decode('utf-8')
        self.o += (n + 3) & ~3
        return s

    def pptr(self):
        return (self.i32(), self.i32())

    def vec(self, n):
        return [self.f32() for _ in range(n)]

    def array(self, fn):
        return [fn() for _ in range(self.i32())]


def read_frame(r):
    return {'uvs': r.vec(4), 'scaleFactor': r.vec(2), 'topLeftOffset': r.vec(2), 'bottomRightOffset': r.vec(2)}


def read_packed_sprite(r):
    d = {}
    # SpriteRoot
    d['managed'] = r.boolean()
    d['manager'] = r.pptr()
    d['drawLayer'] = r.i32()
    d['persistent'] = r.boolean()
    d['plane'] = r.i32()
    d['winding'] = r.i32()
    d['width'] = r.f32()
    d['height'] = r.f32()
    d['bleedCompensation'] = r.vec(2)
    d['anchor'] = r.i32()
    d['pixelPerfect'] = r.boolean()
    d['autoResize'] = r.boolean()
    d['billboarded'] = r.boolean()
    d['offset'] = r.vec(3)
    d['color'] = r.vec(4)
    d['renderCamera'] = r.pptr()
    d['pixelsPerUV'] = r.vec(2)
    d['hideAtStart'] = r.boolean()
    d['ignoreClipping'] = r.boolean()
    # SpriteBase
    d['playAnimOnStart'] = r.boolean()
    d['crossfadeFrames'] = r.boolean()
    d['defaultAnim'] = r.i32()
    # AutoSpriteBase
    d['doNotTrimImages'] = r.boolean()

    def uvanim():
        return {'name': r.string(), 'loopCycles': r.i32(), 'loopReverse': r.boolean(),
                'framerate': r.f32(), 'index': r.i32(), 'onAnimEnd': r.i32()}
    d['animations'] = r.array(uvanim)
    # PackedSprite
    d['staticTexPath'] = r.string()
    d['staticTexGUID'] = r.string()
    d['_ser_stat_frame_info'] = read_frame(r)

    def texanim():
        return {'name': r.string(), 'loopCycles': r.i32(), 'loopReverse': r.boolean(),
                'framerate': r.f32(), 'onAnimEnd': r.i32(),
                'framePaths': r.array(r.string), 'frameGUIDs': r.array(r.string),
                'spriteFrames': r.array(lambda: read_frame(r))}
    d['textureAnimations'] = r.array(texanim)
    return d


def read_screen_placement(r):
    d = {}
    d['renderCamera'] = r.pptr()
    d['screenPos'] = r.vec(3)
    d['ignoreZ'] = r.boolean()
    d['relativeTo'] = {'horizontal': r.i32(), 'vertical': r.i32()}
    d['relativeObject'] = r.pptr()
    d['alwaysRecursive'] = r.boolean()
    d['allowTransformDrag'] = r.boolean()
    r.i32()  # 4 bytes de más que no corresponden a ningún campo del script
    return d


READERS = {'PackedSprite': read_packed_sprite, 'EZScreenPlacement': read_screen_placement}


# ---------------------------------------------------------------- YAML
def fnum(x):
    if math.isnan(x):
        return 'NaN'
    if math.isinf(x):
        return 'Infinity' if x > 0 else '-Infinity'
    s = format(x, '.9g')
    return s


def fstr(s):
    if s == '':
        return ''
    if re.search(r"[:#\[\]{}',&*!|>%@`\"]", s) or s != s.strip() or s[0] in '-?':
        return "'" + s.replace("'", "''") + "'"
    return s


def vec(v, names='xyzw'):
    return '{' + ', '.join('%s: %s' % (names[i], fnum(x)) for i, x in enumerate(v)) + '}'


def color(v):
    return '{' + ', '.join('%s: %s' % (n, fnum(x)) for n, x in zip('rgba', v)) + '}'


def pptr(p, external_ok=False):
    fid, pid = p
    if fid != 0:
        # Referencia a otro archivo: no aparece en estos dos scripts
        raise ValueError('referencia externa %r' % (p,))
    return '{fileID: %d}' % pid


def frame_lines(f, ind):
    u = f['uvs']
    return [
        ind + 'uvs:',
        ind + '  serializedVersion: 2',
        ind + '  x: ' + fnum(u[0]),
        ind + '  y: ' + fnum(u[1]),
        ind + '  width: ' + fnum(u[2]),
        ind + '  height: ' + fnum(u[3]),
        ind + 'scaleFactor: ' + vec(f['scaleFactor']),
        ind + 'topLeftOffset: ' + vec(f['topLeftOffset']),
        ind + 'bottomRightOffset: ' + vec(f['bottomRightOffset']),
    ]


def list_block(key, items, ind, fn):
    if not items:
        return [ind + key + ': []']
    out = [ind + key + ':']
    for it in items:
        sub = fn(it, ind + '  ')
        sub[0] = ind + '- ' + sub[0][len(ind) + 2:]
        out += sub
    return out


def packed_sprite_yaml(d):
    o = []
    o.append('  managed: %d' % d['managed'])
    o.append('  manager: ' + pptr(d['manager']))
    o.append('  drawLayer: %d' % d['drawLayer'])
    o.append('  persistent: %d' % d['persistent'])
    o.append('  plane: %d' % d['plane'])
    o.append('  winding: %d' % d['winding'])
    o.append('  width: ' + fnum(d['width']))
    o.append('  height: ' + fnum(d['height']))
    o.append('  bleedCompensation: ' + vec(d['bleedCompensation']))
    o.append('  anchor: %d' % d['anchor'])
    o.append('  pixelPerfect: %d' % d['pixelPerfect'])
    o.append('  autoResize: %d' % d['autoResize'])
    o.append('  billboarded: %d' % d['billboarded'])
    o.append('  offset: ' + vec(d['offset']))
    o.append('  color: ' + color(d['color']))
    o.append('  renderCamera: ' + pptr(d['renderCamera']))
    o.append('  pixelsPerUV: ' + vec(d['pixelsPerUV']))
    o.append('  hideAtStart: %d' % d['hideAtStart'])
    o.append('  ignoreClipping: %d' % d['ignoreClipping'])
    o.append('  playAnimOnStart: %d' % d['playAnimOnStart'])
    o.append('  crossfadeFrames: %d' % d['crossfadeFrames'])
    o.append('  defaultAnim: %d' % d['defaultAnim'])
    o.append('  doNotTrimImages: %d' % d['doNotTrimImages'])

    def uva(a, ind):
        return [ind + 'name: ' + fstr(a['name']),
                ind + 'loopCycles: %d' % a['loopCycles'],
                ind + 'loopReverse: %d' % a['loopReverse'],
                ind + 'framerate: ' + fnum(a['framerate']),
                ind + 'index: %d' % a['index'],
                ind + 'onAnimEnd: %d' % a['onAnimEnd']]
    o += list_block('animations', d['animations'], '  ', uva)
    o.append('  staticTexPath: ' + fstr(d['staticTexPath']))
    o.append('  staticTexGUID: ' + fstr(d['staticTexGUID']))
    o.append('  _ser_stat_frame_info:')
    o += frame_lines(d['_ser_stat_frame_info'], '    ')
    o.append('  staticFrameInfo:')
    o += frame_lines(d['_ser_stat_frame_info'], '    ')

    def strs(key, items, ind):
        if not items:
            return [ind + key + ': []']
        return [ind + key + ':'] + [ind + '- ' + fstr(s) for s in items]

    def ta(a, ind):
        out = [ind + 'name: ' + fstr(a['name']),
               ind + 'loopCycles: %d' % a['loopCycles'],
               ind + 'loopReverse: %d' % a['loopReverse'],
               ind + 'framerate: ' + fnum(a['framerate']),
               ind + 'onAnimEnd: %d' % a['onAnimEnd']]
        out += strs('framePaths', a['framePaths'], ind)
        out += strs('frameGUIDs', a['frameGUIDs'], ind)
        out += list_block('spriteFrames', a['spriteFrames'], ind, frame_lines)
        return out
    o += list_block('textureAnimations', d['textureAnimations'], '  ', ta)
    return o


def screen_placement_yaml(d):
    return [
        '  renderCamera: ' + pptr(d['renderCamera']),
        '  screenPos: ' + vec(d['screenPos']),
        '  ignoreZ: %d' % d['ignoreZ'],
        '  relativeTo:',
        '    horizontal: %d' % d['relativeTo']['horizontal'],
        '    vertical: %d' % d['relativeTo']['vertical'],
        '  relativeObject: ' + pptr(d['relativeObject']),
        '  alwaysRecursive: %d' % d['alwaysRecursive'],
        '  allowTransformDrag: %d' % d['allowTransformDrag'],
    ]


WRITERS = {'PackedSprite': packed_sprite_yaml, 'EZScreenPlacement': screen_placement_yaml}


def main():
    data_dir, assets = sys.argv[1], sys.argv[2]
    guids = {}
    for name in READERS:
        meta = os.path.join(assets, 'Plugins', 'Assembly-CSharp-firstpass', name + '.cs.meta')
        guids[re.search(r'guid: (\w+)', open(meta).read()).group(1)] = name

    env = UnityPy.load(data_dir)
    scripts = {}
    for fname, sf in env.files.items():
        if not hasattr(sf, 'objects'):
            continue
        for pid, o in sf.objects.items():
            if o.type.name == 'MonoScript':
                try:
                    scripts[(os.path.basename(fname).lower(), pid)] = o.read().m_ClassName
                except Exception:
                    pass

    # Datos originales: {pathID: (script, campos)} por archivo
    recovered = {}
    for fname, sf in env.files.items():
        if not hasattr(sf, 'objects'):
            continue
        base = os.path.basename(fname)
        for pid, o in sf.objects.items():
            if o.type.name != 'MonoBehaviour':
                continue
            raw = o.get_raw_data()
            sfid, spid = struct.unpack_from('<ii', raw, 12)
            ext = base if sfid == 0 else os.path.basename(sf.externals[sfid - 1].path)
            cls = scripts.get((ext.lower(), spid))
            if cls not in READERS:
                continue
            nl = struct.unpack_from('<i', raw, 20)[0]
            r = R(raw)
            r.o = 24 + ((nl + 3) & ~3)
            d = READERS[cls](r)
            if r.o != len(raw):
                raise SystemExit('%s/%d (%s): leídos %d de %d bytes' % (base, pid, cls, r.o, len(raw)))
            recovered.setdefault(base, {})[pid] = (cls, d)
    total = sum(len(v) for v in recovered.values())
    print('Leídos del APK: %d componentes' % total)

    # Escenas exportadas: buscar los componentes vacíos y rellenarlos
    done = 0
    scene_dir = os.path.join(assets, 'Scenes')
    for scene in sorted(os.listdir(scene_dir)):
        if not scene.endswith('.unity'):
            continue
        path = os.path.join(scene_dir, scene)
        text = open(path, encoding='utf-8').read()
        docs = re.split(r'(?m)^(?=--- )', text)
        targets = {}
        for i, doc in enumerate(docs):
            m = re.match(r'--- !u!114 &(\d+)', doc)
            g = re.search(r'm_Script: \{fileID: 11500000, guid: (\w+)', doc)
            if m and g and g.group(1) in guids:
                targets[int(m.group(1))] = (i, guids[g.group(1)])
        if not targets:
            continue
        # El archivo original es el que tiene esos mismos pathID con el mismo script
        src = None
        for base, objs in recovered.items():
            if all(pid in objs and objs[pid][0] == cls for pid, (_, cls) in targets.items()):
                src = objs
                break
        if src is None:
            raise SystemExit('%s: no encuentro su archivo de origen en el APK' % scene)
        n = 0
        for pid, (i, cls) in targets.items():
            doc = docs[i]
            head = doc[:doc.index('  m_Name:')]
            head += '  m_Name: \n'
            body = WRITERS[cls](src[pid][1])
            docs[i] = head + '\n'.join(body) + '\n'
            n += 1
        open(path, 'w', encoding='utf-8').write(''.join(docs))
        print('%s: %d componentes recuperados' % (scene, n))
        done += n
    if done != total:
        print('Aviso: había %d en el APK y se han escrito %d' % (total, done))


if __name__ == '__main__':
    main()
