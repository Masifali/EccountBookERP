import os
import glob

original_dir = r"d:\CShapEccorErp\EccountBook_Original_Src"
resx_files = glob.glob(os.path.join(original_dir, "**", "*.resx"), recursive=True)
print(f"Found {len(resx_files)} resx files")
for r in resx_files[:20]:
    print(os.path.basename(r))
