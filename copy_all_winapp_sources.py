import os
import shutil

src_orig = r"D:\CShapEccorErp\EccountBook_Original_Src\Architecture\WinApp"
dest_src = r"D:\CShapEccorErp\src"

print(f"Copying all files from {src_orig} to {dest_src}...")

copied_count = 0
for root, dirs, files in os.walk(src_orig):
    rel_dir = os.path.relpath(root, src_orig)
    target_dir = os.path.join(dest_src, rel_dir) if rel_dir != "." else dest_src
    os.makedirs(target_dir, exist_ok=True)
    
    for f in files:
        src_file = os.path.join(root, f)
        dest_file = os.path.join(target_dir, f)
        # Copy file
        shutil.copy2(src_file, dest_file)
        copied_count += 1

print(f"Successfully copied {copied_count} files into {dest_src}!")
