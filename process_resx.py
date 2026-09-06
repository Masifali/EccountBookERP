import os
import glob
import shutil

original_dir = r"d:\CShapEccorErp\EccountBook_Original_Src"
dest_resx_dir = r"d:\CShapEccorErp\src\resx"

os.makedirs(dest_resx_dir, exist_ok=True)

resx_files = glob.glob(os.path.join(original_dir, "**", "*.resx"), recursive=True)
print(f"Copying {len(resx_files)} resx files to {dest_resx_dir}...")

copied_count = 0
for resx_path in resx_files:
    fname = os.path.basename(resx_path)
    dest_path = os.path.join(dest_resx_dir, fname)
    shutil.copy2(resx_path, dest_path)
    copied_count += 1

print(f"Successfully copied {copied_count} resx files.")
