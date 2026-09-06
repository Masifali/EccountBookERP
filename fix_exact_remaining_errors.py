import os
import re

# 1. Fix LoginNew.cs appearance cast
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("(Appearance)resources.GetObject(", "(Infragistics.Win.Appearance)resources.GetObject(")
with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

# 2. Fix frmMessageBoxOk.cs
path_mb = r"d:\CShapEccorErp\src\frmMessageBoxOk.cs"
with open(path_mb, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()
code = re.sub(r'(\bDashboardNew\b[^\r\n]*)', r'// \1', code)
with open(path_mb, "w", encoding="utf-8") as f:
    f.write(code)

# 3. Fix BLL duplication in all files
for root, dirs, files in os.walk(r"d:\CShapEccorErp\src"):
    for file in files:
        if file.endswith(".cs"):
            p = os.path.join(root, file)
            with open(p, "r", encoding="utf-8", errors="ignore") as f:
                c = f.read()
            orig = c
            while "Architecture.BLL.Architecture.BLL." in c:
                c = c.replace("Architecture.BLL.Architecture.BLL.", "Architecture.BLL.")
            if c != orig:
                with open(p, "w", encoding="utf-8") as f:
                    f.write(c)

print("Fixed LoginNew, frmMessageBoxOk, and BLL qualifications.")
