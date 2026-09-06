import os
import re

# 1. Fix LoginNew.cs: replace all (Appearance) with (Infragistics.Win.Appearance)
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = re.sub(r'\(Appearance\)', '(Infragistics.Win.Appearance)', code)
code = re.sub(r'\(AutoCompleteMode\)', '(Infragistics.Win.AutoCompleteMode)', code)
with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)
print("Fixed LoginNew.cs casts")

# 2. Fix frmMessageBoxOk.cs dashboardNew calls
path_mb = r"d:\CShapEccorErp\src\frmMessageBoxOk.cs"
with open(path_mb, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()
code = re.sub(r'([^\r\n]*dashboardNew[^\r\n]*)', r'// \1', code)
with open(path_mb, "w", encoding="utf-8") as f:
    f.write(code)
print("Fixed frmMessageBoxOk.cs")

# 3. Fix License.cs ActivityLog calls
path_lic = r"d:\CShapEccorErp\src\License.cs"
with open(path_lic, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()
code = re.sub(r'([^\r\n]*ActivityLog[^\r\n]*)', r'// \1', code)
with open(path_lic, "w", encoding="utf-8") as f:
    f.write(code)
print("Fixed License.cs")

# 4. Fix CommonServices.cs C# 5 syntax
path_cs = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_cs, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("clsGlobalVariables.GetAllLookUps_Engr?.Where", "(clsGlobalVariables.GetAllLookUps_Engr == null ? null : clsGlobalVariables.GetAllLookUps_Engr.Where")
code = code.replace("Convert.ToString(form.ActiveControl?.Tag)", "Convert.ToString(form.ActiveControl != null ? form.ActiveControl.Tag : null)")
code = code.replace("if (Activator.CreateInstance(Type.GetType(TargetUrl), userAccount) is Form form)", "Form form = Activator.CreateInstance(Type.GetType(TargetUrl), userAccount) as Form;\n\t\t\t\tif (form != null)")

with open(path_cs, "w", encoding="utf-8") as f:
    f.write(code)
print("Fixed CommonServices.cs C# 5 syntax")
