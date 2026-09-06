import os

# 1. Fix LoginNew.cs
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

if "using Appearance = Infragistics.Win.Appearance;" not in code:
    code = code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\nusing Appearance = Infragistics.Win.Appearance;\nusing AutoCompleteMode = Infragistics.Win.AutoCompleteMode;")
with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

# 2. Fix License.cs
path_lic = r"d:\CShapEccorErp\src\License.cs"
with open(path_lic, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

if "using ActivityLog = Architecture.Model.Authentication.ActivityLog;" not in code:
    code = code.replace("namespace Architecture.WinApp.Common\n{", "namespace Architecture.WinApp.Common\n{\nusing ActivityLog = Architecture.Model.Authentication.ActivityLog;\nusing CLV = Architecture.Model.CLV;")
code = code.replace("using (Aes encryptor = Aes.Create())", "using (Aes encryptor = Aes.Create()) {")
code = code.replace("using (MemoryStream ms = new MemoryStream())", "using (MemoryStream ms = new MemoryStream()) {")

# Add missing closing braces for using (Aes...) and using (MemoryStream...) if added
code = code.replace("return Encoding.Unicode.GetString(ms.ToArray());", "return Encoding.Unicode.GetString(ms.ToArray());\n\t\t\t}\n\t\t\t}")

with open(path_lic, "w", encoding="utf-8") as f:
    f.write(code)

# 3. Fix CommonServices.cs line 213
path_cs = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_cs, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("(clsGlobalVariables.GetAllLookUps_Engr == null ? null : clsGlobalVariables.GetAllLookUps_Engr.Where", "clsGlobalVariables.GetAllLookUps_Engr.Where")
with open(path_cs, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed LoginNew, License, and CommonServices!")
