import os
import glob
import shutil

src_dir = r"d:\CShapEccorErp\src"

# List all items in src_dir
subdirs = [os.path.join(src_dir, d) for d in os.listdir(src_dir) if os.path.isdir(os.path.join(src_dir, d)) and d != "resx" and d != "bin" and d != "obj" and d != "Properties"]

print(f"Removing {len(subdirs)} extra subdirectories from src/...")
for d in subdirs:
    shutil.rmtree(d, ignore_errors=True)

print("Done cleaning extra subdirectories from src/.")
