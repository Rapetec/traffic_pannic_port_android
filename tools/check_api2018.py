#!/usr/bin/env python3
"""Comprueba qué miembros de UnityEngine que usa el juego (lista de monodis) no existen o
dan error de obsoleto en el código de referencia de Unity 2018.4 (UnityCsReference)."""
import re, os, sys, collections
ucs, refs, types = sys.argv[1], sys.argv[2], sys.argv[3]
files = {}
for top in ('Runtime', 'Modules'):
    for dp, _, fs in os.walk(os.path.join(ucs, top)):
        for f in fs:
            if f.endswith('.cs'):
                files[os.path.join(dp, f)] = open(os.path.join(dp, f), errors='ignore').read()
bytype = collections.defaultdict(list)
for p, s in files.items():
    for m in re.finditer(r'\b(?:class|struct|enum|interface)\s+(\w+)', s):
        bytype[m.group(1)].append(p)
for t in sorted(l.strip() for l in open(types)):
    if t not in bytype: print('TIPO NO EXISTE:', t)
OPS = {'.ctor', '.cctor'}
for r in sorted(l.strip() for l in open(refs)):
    typ, _, mem = r.rpartition('.')
    if mem in OPS or mem.startswith('op_') or typ not in bytype: continue
    name = re.sub(r'^(get_|set_|add_|remove_)', '', mem)
    found = []
    for p in bytype[typ]:
        s = files[p]
        for m in re.finditer(r'\b' + re.escape(name) + r'\b\s*(\(|\{|=>|<|;|=)', s):
            line_start = s.rfind('\n', 0, m.start())
            line = s[line_start:m.end()]
            if not re.search(r'\b(public|protected|extern)\b', line): continue
            pre = s[max(0, line_start - 600):line_start]
            attrs = pre.split(';')[-1].split('}')[-1]
            found.append(bool(re.search(r'Obsolete\(".*?"\s*,\s*true\s*\)', attrs)))
    if not found: print('NO EXISTE:', r)
    elif all(found): print('OBSOLETO (error):', r)
