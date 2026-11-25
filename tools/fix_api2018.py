#!/usr/bin/env python3
"""Adapta el C# del juego a Unity 2018.4 usando los errores de compilar contra el
UnityEngine.dll recortado (make_ue2018_check.py): cada `x.renderer`, `x.collider`... que el
compilador marca en un Component/GameObject pasa a `x.GetComponent<Renderer>()`.
Repite hasta que no quedan errores de ese tipo.
Uso: fix_api2018.py <Assets> <dir con UnityEngine recortado> <dir temporal>"""
import re, subprocess, sys, os, collections
assets, ue, tmp = sys.argv[1:4]
CHECK = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'check35.sh')
TYPES = {'renderer': 'Renderer', 'collider': 'Collider', 'rigidbody': 'Rigidbody', 'audio': 'AudioSource',
         'animation': 'Animation', 'camera': 'Camera', 'light': 'Light', 'guiText': 'GUIText',
         'guiTexture': 'GUITexture', 'hingeJoint': 'HingeJoint', 'constantForce': 'ConstantForce'}
ERR = re.compile(r'^(.*\.cs)\((\d+),(\d+)\): error CS(?:1061|0117): .*definition for `(\w+)\'')
for it in range(20):
    out = subprocess.run([CHECK, assets, ue, tmp], capture_output=True, text=True).stdout
    todo = collections.defaultdict(set)
    for ln in out.splitlines():
        m = ERR.match(ln)
        if m and m.group(4) in TYPES:
            todo[m.group(1)].add((int(m.group(2)), int(m.group(3)), m.group(4)))
    if not todo:
        break
    n = 0
    for path, locs in todo.items():
        lines = open(path, encoding='utf-8').read().split('\n')
        for (l, c, mem) in sorted(locs, key=lambda x: (x[0], -x[1])):
            s = lines[l - 1]
            # busca la aparición de .mem más cercana a la columna indicada
            best = None
            for mm in re.finditer(r'\.' + mem + r'\b(?!\s*\()', s):
                d = abs(mm.start() - (c - 1))
                if best is None or d < best[0]: best = (d, mm)
            if not best: continue
            mm = best[1]
            lines[l - 1] = s[:mm.start()] + '.GetComponent<%s>()' % TYPES[mem] + s[mm.end():]
            n += 1
        open(path, 'w', encoding='utf-8').write('\n'.join(lines))
    print('pasada %d: %d cambios' % (it + 1, n))
print(out[-3000:])
