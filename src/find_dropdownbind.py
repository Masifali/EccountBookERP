import glob
import subprocess

dlls = glob.glob(r"d:\CShapEccorErp\src\bin\Debug\*.dll")
for d in dlls:
    cmd = f'[System.Reflection.Assembly]::LoadFrom("{d}").GetTypes() | % {{ $_.FullName }} | ? {{ $_ -like "*DropDownBind*" }}'
    res = subprocess.run(["powershell", "-Command", cmd], capture_output=True, text=True)
    if res.stdout.strip():
        print(f"FOUND IN: {d} -> {res.stdout.strip()}")
