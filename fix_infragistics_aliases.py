import os
import glob

src_dir = r"d:\CShapEccorErp\src"
cs_files = glob.glob(os.path.join(src_dir, "**", "*.cs"), recursive=True)

alias_lines = """using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
"""

fixed_count = 0

for filepath in cs_files:
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()

    modified = False

    if "Infragistics" in content:
        if "using Appearance = Infragistics.Win.Appearance;" not in content:
            if "namespace Architecture.WinApp\n{" in content:
                content = content.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\n" + alias_lines)
                modified = True
            elif "namespace Architecture.WinApp\r\n{" in content:
                content = content.replace("namespace Architecture.WinApp\r\n{", "namespace Architecture.WinApp\r\n{\n" + alias_lines)
                modified = True

    if modified:
        with open(filepath, "w", encoding="utf-8") as f:
            f.write(content)
        fixed_count += 1

print(f"Added Infragistics aliases to {fixed_count} C# files!")
