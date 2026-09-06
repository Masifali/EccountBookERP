import glob
import os

src_dir = r"d:\CShapEccorErp\src"
cs_files = glob.glob(os.path.join(src_dir, "**", "*.cs"), recursive=True)

for filepath in cs_files:
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    
    if "using Architecture.WinApp.Common;" not in code:
        if "namespace Architecture.WinApp" in code:
            code = code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\nusing Architecture.WinApp.Common;\n")
        elif "namespace Architecture.WinApp;" in code:
            code = code.replace("namespace Architecture.WinApp;", "namespace Architecture.WinApp;\nusing Architecture.WinApp.Common;\n")
    
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(code)

print("Added using Architecture.WinApp.Common to all C# files!")
