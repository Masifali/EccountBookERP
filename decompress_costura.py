import os
import glob
import zlib

SRC_DIR = r"d:\CShapEccorErp\EccountBook_Original_Src"
DLL_OUT_DIR = r"d:\CShapEccorErp\Decompressed_DLLs"

if not os.path.exists(DLL_OUT_DIR):
    os.makedirs(DLL_OUT_DIR, exist_ok=True)

compressed_files = glob.glob(os.path.join(SRC_DIR, "**", "*.compressed"), recursive=True)
print(f"Found {len(compressed_files)} compressed assembly files.")

decompressed_count = 0

for cfile in compressed_files:
    fname = os.path.basename(cfile)
    # Format: costura.architecture.bll.dll.compressed -> Architecture.BLL.dll
    clean_name = fname
    if clean_name.startswith("costura."):
        clean_name = clean_name[8:]
    if clean_name.endswith(".compressed"):
        clean_name = clean_name[:-11]

    target_path = os.path.join(DLL_OUT_DIR, clean_name)

    try:
        with open(cfile, "rb") as f:
            data = f.read()

        # Costura uses raw DeflateStream (wbits = -15)
        decompressed_data = zlib.decompress(data, -15)

        with open(target_path, "wb") as out_f:
            out_f.write(decompressed_data)

        decompressed_count += 1
        print(f"Decompressed: {clean_name} ({len(decompressed_data)} bytes)")
    except Exception as e:
        print(f"Error decompressing {fname}: {e}")

print(f"Successfully decompressed {decompressed_count} DLLs/EXEs to {DLL_OUT_DIR}!")
