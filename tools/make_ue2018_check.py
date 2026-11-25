#!/usr/bin/env python3
"""Genera, a partir del UnityEngine.dll 3.5 desensamblado (monodis), una versión sin las APIs
que Unity 2018.4 ya no admite. Compilar el juego contra ella marca, con su tipo, cada sitio que
hay que adaptar. Uso: make_ue2018_check.py ue.il salida.il"""
import re, sys
REMOVE = {
    'Component': ['renderer', 'collider', 'rigidbody', 'audio', 'animation', 'camera', 'light', 'guiText',
                  'guiTexture', 'hingeJoint', 'particleEmitter', 'networkView', 'constantForce', 'active'],
    'GameObject': ['renderer', 'collider', 'rigidbody', 'audio', 'animation', 'camera', 'light', 'guiText',
                   'guiTexture', 'hingeJoint', 'particleEmitter', 'networkView', 'constantForce'],
    'Camera': ['mainCamera'],
}
REMOVE_METHODS = {'Application': ['CaptureScreenshot']}
REMOVE_TYPES = ['iPhoneKeyboard', 'iPhoneKeyboardType', 'ParticleEmitter', 'ParticleAnimator', 'ParticleRenderer',
                'iPhoneSettings', 'iPhoneInput', 'iPhoneUtils', 'iPhoneGeneration']
# Miembros que Unity 2019 sí tiene y 3.5 no (para que la comprobación no dé errores falsos)
ADD = {
    'GameObject': ['.method public hidebysig instance void SetActive(bool v) cil managed { ret }',
                   '.method public hidebysig specialname instance bool get_activeSelf() cil managed { ldc.i4.0 ret }',
                   '.property instance bool activeSelf() { .get instance bool UnityEngine.GameObject::get_activeSelf() }',
                   '.method public hidebysig specialname instance bool get_activeInHierarchy() cil managed { ldc.i4.0 ret }',
                   '.property instance bool activeInHierarchy() { .get instance bool UnityEngine.GameObject::get_activeInHierarchy() }'],
    'TouchScreenKeyboard': ['.method public static hidebysig specialname bool get_hideInput() cil managed { ldc.i4.0 ret }',
                            '.method public static hidebysig specialname void set_hideInput(bool v) cil managed { ret }',
                            '.property bool hideInput() { .get bool UnityEngine.TouchScreenKeyboard::get_hideInput() .set void UnityEngine.TouchScreenKeyboard::set_hideInput(bool) }',
                            '.method public hidebysig specialname instance void set_text(string v) cil managed { ret }',
                            '.method public hidebysig specialname instance bool get_active() cil managed { ldc.i4.0 ret }',
                            '.method public hidebysig specialname instance void set_active(bool v) cil managed { ret }',
                            '.property instance bool active() { .get instance bool UnityEngine.TouchScreenKeyboard::get_active() .set instance void UnityEngine.TouchScreenKeyboard::set_active(bool) }'],
}
src, out = sys.argv[1], sys.argv[2]
lines = open(src).read().split('\n')
cls = None
res = []
for ln in lines:
    m = re.match(r'\s*\.class .*\s(\w+)\s*$', ln)
    if m:
        cls = m.group(1)
        if cls in REMOVE_TYPES: ln = ln[:m.start(1)] + 'X__' + cls
    if cls in REMOVE:
        for p in REMOVE[cls]:
            ln = re.sub(r'\b(get|set)_%s\b(?=\s*\()' % p, r'\1_X__%s' % p, ln)
            ln = re.sub(r'(\.property .*)\b%s\b(?=\s*\()' % p, r'\1X__%s' % p, ln)
    if cls in REMOVE_METHODS:
        for p in REMOVE_METHODS[cls]:
            ln = re.sub(r'(?<![:.\w])%s(?=\s*\()' % p, r'X__%s' % p, ln)
    res.append(ln)
    if m and cls in ADD:
        res.append('__ADD__' + cls)
s = '\n'.join(res)
# inserta tras la llave de apertura de la clase
for c, items in ADD.items():
    s = re.sub(r'__ADD__%s\n((?:.*\n)*?\s*\{\n)' % c, lambda m: m.group(1) + '\n'.join(items) + '\n', s, count=1)
# llamadas internas a lo renombrado
for c, ps in REMOVE.items():
    for p in ps:
        s = re.sub(r'(UnityEngine\.%s::)(get|set)_%s\(' % (c, p), r'\1\2_X__%s(' % p, s)
for c, ps in REMOVE_METHODS.items():
    for p in ps:
        s = re.sub(r'(UnityEngine\.%s::)%s\(' % (c, p), r'\1X__%s(' % p, s)
for t in REMOVE_TYPES:
    s = re.sub(r'\bUnityEngine\.%s\b' % t, 'UnityEngine.X__%s' % t, s)
s = s.replace('.get instance default string UnityEngine.TouchScreenKeyboard::get_text ()',
              '.get instance default string UnityEngine.TouchScreenKeyboard::get_text ()\n\t\t.set instance void UnityEngine.TouchScreenKeyboard::set_text(string)')
open(out, 'w').write(s)
