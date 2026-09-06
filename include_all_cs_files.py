path_b = r"d:\CShapEccorErp\build_minimal_login_csproj.py"
with open(path_b, "r", encoding="utf-8") as f:
    code = f.read()

# Replace cs_files definition to include all cs files in src recursively
code = code.replace(
    'cs_files = [os.path.join(SRC_DIR, f) for f in login_essential_files if os.path.exists(os.path.join(SRC_DIR, f))]',
    'cs_files = glob.glob(os.path.join(SRC_DIR, "**", "*.cs"), recursive=True)'
)

with open(path_b, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated build_minimal_login_csproj.py to include ALL 83 C# files!")
