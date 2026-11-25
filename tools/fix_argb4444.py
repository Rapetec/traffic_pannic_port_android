#!/usr/bin/env python3
"""Corrige los colores de las texturas de 16 bits con alfa (formato 2, "ARGB4444").

En el Android de Unity 3.5 esas texturas guardan cada píxel como RGBA4444: rojo en los 4 bits
altos y alfa en los 4 bajos. AssetRipper las lee como ARGB4444, así que los canales salen
rotados: lo rojo se ve azul, lo amarillo morado, y el alfa sale del azul. Afecta a 117
texturas: los atlas de la interfaz ("16A", "16A 2"), la grúa, los iconos de logros…

El script vuelve a decodificar cada una desde el APK y sobrescribe el PNG exportado. Antes
comprueba que el PNG exportado sea exactamente la versión rotada, para no tocar nada que no
corresponda.

Uso: python3 fix_argb4444.py <APK>/assets/bin/Data <Proyecto>/Assets
"""
import os
import sys

import numpy as np
import UnityPy
from PIL import Image


def main():
    data_dir, assets = sys.argv[1], sys.argv[2]
    pngs = {}
    for root, _, files in os.walk(assets):
        for f in files:
            if f.endswith('.png'):
                pngs.setdefault(f[:-4], []).append(os.path.join(root, f))

    env = UnityPy.load(data_dir)
    fixed = 0
    for o in env.objects:
        if o.type.name != 'Texture2D':
            continue
        try:
            t = o.read()
        except Exception:
            continue  # texturas internas de Unity
        if int(t.m_TextureFormat) != 2:
            continue
        paths = pngs.get(t.m_Name, [])
        if len(paths) != 1:
            raise SystemExit('%s: %d PNG con ese nombre' % (t.m_Name, len(paths)))
        w, h = t.m_Width, t.m_Height
        v = np.frombuffer(bytes(t.image_data)[:w * h * 2], dtype='<u2').reshape(h, w)
        n = [((v >> s) & 15).astype(np.uint8) * 17 for s in (12, 8, 4, 0)]
        good = np.stack(n, -1)[::-1]            # R G B A, de arriba abajo
        wrong = np.stack([n[1], n[2], n[3], n[0]], -1)[::-1]  # lo que exportó AssetRipper
        cur = np.asarray(Image.open(paths[0]).convert('RGBA'))
        if np.array_equal(cur, good):
            continue  # ya corregida
        if not np.array_equal(cur, wrong):
            raise SystemExit('%s: el PNG no es la versión rotada; no lo toco' % paths[0])
        Image.fromarray(good, 'RGBA').save(paths[0])
        fixed += 1
    print('Texturas corregidas: %d' % fixed)


if __name__ == '__main__':
    main()
