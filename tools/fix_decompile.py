#!/usr/bin/env python3
"""Arregla artefactos del decompilador en el C# exportado por AssetRipper.

Quita los puentes de interfaz «virtual» (accesores y métodos) que genera el compilador
(`virtual GameObject IUIObject.get_gameObject() { return base.gameObject; }`): en el código
original las cubrían los miembros heredados de MonoBehaviour (gameObject, transform, name).
Uso: python3 fix_decompile.py <carpeta Assets>
"""
import os, re, sys

PAT = re.compile(r'^(\s*)virtual [\w.<>\[\]]+ I\w+\.\w+\([^)]*\)\s*\n\1\{\n(?:.*\n)*?\1\}\n', re.M)

RND = re.compile(r'(?<![\w.])(\(|\s|=|-|\+|\*|,)Random\.(?=Range|value|seed|insideUnit|onUnitSphere|rotation)')

def main(root):
    n = 0
    for dp, _, fs in os.walk(root):
        for f in fs:
            if not f.endswith('.cs'):
                continue
            p = os.path.join(dp, f)
            s = open(p, encoding='utf-8-sig').read()
            s2, k = PAT.subn('', s)
            # System.Random y UnityEngine.Random a la vez: el original usaba el de Unity
            if 'using System;' in s2 and 'using UnityEngine;' in s2:
                s3, k2 = RND.subn(r'\1UnityEngine.Random.', s2)
                s2, k = s3, k + k2
            if k:
                open(p, 'w', encoding='utf-8').write(s2)
                n += k
                print('%s: %d' % (os.path.relpath(p, root), k))
    print('total:', n)

if __name__ == '__main__':
    main(sys.argv[1])
