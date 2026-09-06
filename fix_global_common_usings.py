import glob
import os

src_dir = r"d:\CShapEccorErp\src"
cs_files = glob.glob(os.path.join(src_dir, "**", "*.cs"), recursive=True)

header_usings = """using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;
using Janus.Windows.GridEX;
"""

for filepath in cs_files:
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    
    if "using Janus.Windows.GridEX;" not in code:
        if "namespace Architecture.WinApp\n{" in code:
            code = code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\n" + header_usings)
        elif "namespace Architecture.WinApp;" in code:
            code = code.replace("namespace Architecture.WinApp;", "namespace Architecture.WinApp;\n" + header_usings)
    
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(code)

print("Added using Janus.Windows.GridEX to all C# files!")
