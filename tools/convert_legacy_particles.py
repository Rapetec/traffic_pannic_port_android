#!/usr/bin/env python3
"""Cambia en escenas y prefabs los componentes de partículas de Unity 3 (EllipsoidParticleEmitter
!u!15, ParticleAnimator !u!12, ParticleRenderer !u!26), que Unity 2019 ya no carga, por el
componente Scripts/Port/ParticleEmitter.cs con los mismos parámetros.
Uso: convert_legacy_particles.py <Assets> <guid del script ParticleEmitter>"""
import glob, os, re, sys

assets, guid = sys.argv[1], sys.argv[2]
DOC = re.compile(r'^--- !u!(\d+) &(\d+)[^\n]*\n', re.M)

def split_docs(s):
    heads = list(DOC.finditer(s))
    pre = s[:heads[0].start()] if heads else s
    docs = []
    for i, h in enumerate(heads):
        end = heads[i + 1].start() if i + 1 < len(heads) else len(s)
        docs.append([h.group(1), h.group(2), s[h.start():end]])
    return pre, docs

def val(body, key):
    m = re.search(r'^  ' + re.escape(key) + r': (.*)$', body, re.M)
    return m.group(1).strip() if m else None

def color(rgba):
    v = int(rgba)
    return '{r: %g, g: %g, b: %g, a: %g}' % ((v & 255) / 255.0, ((v >> 8) & 255) / 255.0,
                                              ((v >> 16) & 255) / 255.0, (v >> 24) / 255.0)

def convert(path):
    s = open(path, encoding='utf-8').read()
    pre, docs = split_docs(s)
    by_go = {}
    for d in docs:
        if d[0] in ('15', '12', '26'):
            go = re.search(r'm_GameObject: \{fileID: (\d+)\}', d[2]).group(1)
            by_go.setdefault(go, {})[d[0]] = d
    if not by_go:
        return 0
    out = []
    for d in docs:
        cid, fid, body = d
        if cid in ('12', '26'):
            continue
        if cid == '1' and fid in by_go:
            comps = by_go[fid]
            em = comps.get('15')
            for k, c in comps.items():
                if k == '15':
                    body = body.replace('  - 15: {fileID: %s}' % c[1], '  - 114: {fileID: %s}' % c[1])
                else:
                    body = body.replace('  - %s: {fileID: %s}\n' % (k, c[1]), '')
        if cid == '15':
            go = re.search(r'm_GameObject: \{fileID: (\d+)\}', body).group(1)
            an = by_go[go].get('12', [None, None, ''])[2]
            rn = by_go[go].get('26', [None, None, ''])[2]
            prefab = val(body, 'm_PrefabInternal') or '{fileID: 0}'
            parent = val(body, 'm_PrefabParentObject') or '{fileID: 0}'
            cols = re.findall(r'colorAnimation\[\d\]:\n\s+serializedVersion: \d+\n\s+rgba: (\d+)', an)
            mat = re.search(r'm_Materials:\n  - (\{[^}]*\})', rn)
            f = [
                ('m_Emit', val(body, 'm_Emit') or '1'),
                ('minSize', val(body, 'minSize')), ('maxSize', val(body, 'maxSize')),
                ('minEnergy', val(body, 'minEnergy')), ('maxEnergy', val(body, 'maxEnergy')),
                ('minEmission', val(body, 'minEmission')), ('maxEmission', val(body, 'maxEmission')),
                ('worldVelocity', val(body, 'worldVelocity')), ('localVelocity', val(body, 'localVelocity')),
                ('rndVelocity', val(body, 'rndVelocity')), ('emitterVelocityScale', val(body, 'emitterVelocityScale')),
                ('angularVelocity', val(body, 'angularVelocity')), ('rndAngularVelocity', val(body, 'rndAngularVelocity')),
                ('rndRotation', val(body, 'rndRotation')), ('useWorldSpace', val(body, 'Simulate in Worldspace?')),
                ('oneShot', val(body, 'm_OneShot')), ('ellipsoid', val(body, 'm_Ellipsoid')),
                ('minEmitterRange', val(body, 'm_MinEmitterRange')),
                ('doesAnimateColor', val(an, 'Does Animate Color?') or '1'),
                ('sizeGrow', val(an, 'sizeGrow') or '0'), ('rndForce', val(an, 'rndForce') or '{x: 0, y: 0, z: 0}'),
                ('force', val(an, 'force') or '{x: 0, y: 0, z: 0}'), ('damping', val(an, 'damping') or '1'),
                ('autodestruct', val(an, 'autodestruct') or '0'),
                ('material', mat.group(1) if mat else '{fileID: 0}'),
                ('maxParticleSize', val(rn, 'm_MaxParticleSize') or '0.25'),
            ]
            lines = ['--- !u!114 &%s' % fid, 'MonoBehaviour:', '  m_ObjectHideFlags: 0',
                     '  m_PrefabParentObject: %s' % parent, '  m_PrefabInternal: %s' % prefab,
                     '  m_GameObject: {fileID: %s}' % go, '  m_Enabled: %s' % (val(body, 'm_Enabled') or '1'),
                     '  m_EditorHideFlags: 0', '  m_Script: {fileID: 11500000, guid: %s, type: 3}' % guid,
                     '  m_Name: ', '  m_EditorClassIdentifier: ']
            lines += ['  %s: %s' % (k, v) for k, v in f if v is not None]
            lines.append('  colorAnimation:')
            lines += ['  - ' + color(c) for c in (cols or ['4294967295'] * 5)]
            body = '\n'.join(lines) + '\n'
        out.append(body)
    open(path, 'w', encoding='utf-8').write(pre + ''.join(out))
    return len(by_go)

total = 0
for p in glob.glob(os.path.join(assets, '**', '*.prefab'), recursive=True) + \
         glob.glob(os.path.join(assets, '**', '*.unity'), recursive=True):
    n = convert(p)
    if n:
        print('%s: %d emisores' % (os.path.relpath(p, assets), n))
        total += n
print('total:', total)
