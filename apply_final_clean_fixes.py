import os
import re

# 1. Fix LoginNew.cs appearance cast
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("(Appearance)resources.GetObject(", "(Infragistics.Win.Appearance)resources.GetObject(")
with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)
print("Fixed LoginNew.cs appearance cast")

# 2. Fix double BLL qualifications in frmMessageBoxOk.cs, License.cs, OTPAthentication.cs
for fn in ["frmMessageBoxOk.cs", "License.cs", "OTPAthentication.cs", "EnterLicenseKey.cs"]:
    p = os.path.join(r"d:\CShapEccorErp\src", fn)
    if os.path.exists(p):
        with open(p, "r", encoding="utf-8", errors="ignore") as f:
            c = f.read()
        c = c.replace("Architecture.BLL.Architecture.BLL.", "Architecture.BLL.")
        c = c.replace("new DashboardNew(", "// new DashboardNew(")
        with open(p, "w", encoding="utf-8") as f:
            f.write(c)
        print(f"Cleaned {fn}")

# 3. Fix CommonServices.cs aliases & formatting syntax
path_cs = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_cs, "r", encoding="utf-8", errors="ignore") as f:
    c = f.read()

c = c.replace("using Architecture.Model.VPS;", "using Architecture.Model.VPS;\nusing InvSaleInvoice = Architecture.BLL.Inventory.InvSaleInvoice;\nusing VoucherHead = Architecture.Model.Accounts.VoucherHead;")

# Replace problematic C# 8 format expressions in CommonServices.cs
c = re.sub(r'string\.Format\("[^"]*",\s*Math\.Floor\([^)]+\):#,##0\)', 'string.Format("{0:#,##0}", Math.Floor(result))', c)

with open(path_cs, "w", encoding="utf-8") as f:
    f.write(c)

print("Cleaned CommonServices.cs")
