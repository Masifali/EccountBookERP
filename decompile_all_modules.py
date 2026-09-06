import os
import glob
import subprocess

DLL_DIR = r"d:\CShapEccorErp\Decompressed_DLLs"
OUT_ROOT = r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution"
ILSPY_DLL = r"d:\CShapEccorErp\ilspycmd8_tool\tools\net6.0\any\ilspycmd.dll"
DOTNET = r"C:\Program Files\dotnet\dotnet.exe"
REF_DIR = r"C:\Program Files (x86)\SCS\Rice_Erp_EccountBook"

if not os.path.exists(OUT_ROOT):
    os.makedirs(OUT_ROOT, exist_ok=True)

# Select core Architecture assemblies to decompile into projects
target_files = [
    "architecture.bll.dll",
    "architecture.dal.dll",
    "architecture.model.dll",
    "architecture.common.dll",
    "architecture.reports.exe",
    "architecture.psdsms.exe",
    "architecture.webcamera.dll",
    "architecture.webhelper.dll",
    "architecture.zktfingerprint.dll"
]

for tf in target_files:
    full_path = os.path.join(DLL_DIR, tf)
    if not os.path.exists(full_path):
        print(f"Skipping {tf} (not found)")
        continue

    proj_name = os.path.splitext(tf)[0]
    out_dir = os.path.join(OUT_ROOT, proj_name)
    os.makedirs(out_dir, exist_ok=True)

    cmd = [
        DOTNET, "--roll-forward", "Major", ILSPY_DLL,
        "-p", "--nested-directories", "-usepdb",
        "-r", DLL_DIR,
        "-r", REF_DIR,
        "-o", out_dir,
        full_path
    ]

    print(f"Decompiling {tf} into {out_dir}...")
    res = subprocess.run(cmd, capture_output=True, text=True)
    if res.returncode == 0:
        print(f"Successfully decompiled {tf}!")
    else:
        print(f"Decompiled {tf} with code {res.returncode}")
        print("Stdout:", res.stdout[:300])
        print("Stderr:", res.stderr[:300])

print(f"All core modules decompiled into {OUT_ROOT}!")
