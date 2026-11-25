#!/usr/bin/env bash
# Compila los scripts contra un UnityEngine.dll (el 3.5 del APK o el recortado a la API de 2019)
# para comprobar sintaxis y API. check_ignore.txt: errores falsos (APIs que sí existen en 2019).
set -u
A=${1:-/tmp/tp/proj/ExportedProject/Assets}; M=${2:-/tmp/tp/apk/assets/bin/Data/Managed}; O=${3:-/tmp/tp/chk}
mkdir -p "$O"
R="-nowarn:0618,0414,0219,0169,0649,0162,0108,0114,1701,1702 -r:$M/UnityEngine.dll"
mcs -target:library $R -out:$O/fp.dll $(find "$A/Plugins/Assembly-CSharp-firstpass" -name "*.cs") $(dirname "$0")/checkstubs/*.cs 2>&1 | grep -E "error" | grep -v "Location of" | grep -vFf "$(dirname "$0")/check_ignore.txt"
[ -f $O/fp.dll ] || exit 1
mcs -target:library $R -r:$O/fp.dll -r:$M/Assembly-UnityScript-firstpass.dll -out:$O/main.dll $(find "$A/Scripts" -name "*.cs" ! -path "*/Scripts/Port/*") $(dirname "$0")/checkstubs/main/*.cs 2>&1 | grep -E "error" | grep -v "Location of" | grep -vFf "$(dirname "$0")/check_ignore.txt"
ls -la $O/*.dll
