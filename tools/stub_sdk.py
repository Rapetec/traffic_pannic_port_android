#!/usr/bin/env python3
"""Vacía las clases puente de los SDK de terceros (prime31, Chartboost, OpenFeint, NADS...):
mantiene sus declaraciones públicas para que el juego compile, pero cada método deja de llamar a
Java o a la red y devuelve el valor por defecto. Uso: stub_sdk.py <archivo.cs>..."""
import re, sys

SIG = re.compile(r'^(\t+)((?:public|private|protected|internal|static|virtual|override|extern|unsafe|\s)+)'
                 r'([\w.<>\[\], ]+?\s+)?(\w+)\s*\(([^)]*)\)\s*$')

def stub(path):
    lines = open(path, encoding='utf-8').read().split('\n')
    out, i, n = [], 0, 0
    cls = None
    while i < len(lines):
        ln = lines[i]
        mc = re.match(r'^\s*(?:public |internal |private )?(?:static |sealed |abstract )*class (\w+)', ln)
        if mc: cls = mc.group(1)
        m = SIG.match(ln)
        if m and i + 1 < len(lines) and lines[i + 1] == m.group(1) + '{':
            ind, ret, name = m.group(1), (m.group(3) or '').strip(), m.group(4)
            j = i + 2
            while j < len(lines) and lines[j] != ind + '}':
                j += 1
            params = m.group(5)
            outs = re.findall(r'\bout\s+[\w.<>\[\]]+\s+(\w+)', params)
            body = [ind + '\t%s = default(%s);' % (o, re.search(r'\bout\s+([\w.<>\[\]]+)\s+' + o, params).group(1)) for o in outs]
            if ret and ret != 'void' and name != cls:
                body.append(ind + '\treturn default(%s);' % ret)
            out += [ln, ind + '{'] + body + [ind + '}']
            i = j + 1
            n += 1
            continue
        out.append(ln)
        i += 1
    s = '\n'.join(out)
    # declaraciones nativas de iOS que ya nadie llama, y campos que creaban clases Java al construirse
    s = re.sub(r'\n\t+\[DllImport\("__Internal"\)\]\n\t+[^\n]*\bextern\b[^\n]*;', '', s)
    s = re.sub(r'(\bAndroidJava(?:Class|Object)\s+\w+)\s*=\s*new AndroidJava(?:Class|Object)\([^;]*\);', r'\1;', s)
    s = s.replace('\n', '\n// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)\n', 1) if n else s
    open(path, 'w', encoding='utf-8').write(s)
    print('%s: %d métodos vaciados' % (path, n))

for p in sys.argv[1:]:
    stub(p)
