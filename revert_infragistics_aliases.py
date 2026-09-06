import os
import glob

src_dir = r"d:\CShapEccorErp\src"
cs_files = glob.glob(os.path.join(src_dir, "**", "*.cs"), recursive=True)

alias_lines = "using Appearance = Infragistics.Win.Appearance;\nusing AutoCompleteMode = Infragistics.Win.AutoCompleteMode;\n"

reverted_count = 0

for filepath in cs_files:
    if os.path.basename(filepath) in ["frmMenue.cs", "DashboardNew.cs", "CommonServices.cs"]:
        continue
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()

    if alias_lines in content:
        content = content.replace(alias_lines, "")
        with open(filepath, "w", encoding="utf-8") as f:
            f.write(content)
        reverted_count += 1

print(f"Reverted alias lines in {reverted_count} C# files!")
