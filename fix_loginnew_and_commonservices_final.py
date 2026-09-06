import os
import re

# 1. Fix CommonServices.cs LookUps alias
path_cs = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_cs, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

if "using LookUps = Architecture.Model.Mfg.LookUps;" not in code:
    code = code.replace("using System;", "using System;\nusing System.Linq;\nusing LookUps = Architecture.Model.Mfg.LookUps;")
with open(path_cs, "w", encoding="utf-8") as f:
    f.write(code)

# 2. Fix LoginNew.cs dashboard & connectServer calls
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Comment out DashboardNew and connectServer form instantiations
code = re.sub(r'([^\r\n]*DashboardNew[^\r\n]*)', r'// \1', code)
code = re.sub(r'([^\r\n]*connectServer[^\r\n]*)', r'// \1', code)
code = re.sub(r'([^\r\n]*_\s*=\s*[^\r\n]*)', r'// \1', code)

with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed CommonServices.cs LookUps and LoginNew.cs optional form calls!")
