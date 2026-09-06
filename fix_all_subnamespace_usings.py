import glob
import os
import re

src_dir = r"d:\CShapEccorErp\src"
cs_files = glob.glob(os.path.join(src_dir, "**", "*.cs"), recursive=True)

patterns = [
    r"using\s+Architecture\.WinApp\.[a-zA-Z0-9_]+;",
    r"using\s+Architecture\.Reports;",
    r"using\s+Architecture\.Reports\.[a-zA-Z0-9_]+;",
    r"using\s+static\s+Architecture\.WinApp\.[a-zA-Z0-9_]+;"
]

fixed_count = 0
for filepath in cs_files:
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    
    new_code = code
    for pat in patterns:
        new_code = re.sub(pat, r"// \g<0>", new_code)
    
    if new_code != code:
        with open(filepath, "w", encoding="utf-8") as f:
            f.write(new_code)
        fixed_count += 1

print(f"Cleaned sub-namespace usings in {fixed_count} C# files!")
