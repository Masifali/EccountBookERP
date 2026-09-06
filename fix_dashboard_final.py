import re

# Fix LoginNew.cs
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    l_code = f.read()

if "using OTPAthentication = Architecture.WinApp.OTPAthentication;" not in l_code:
    l_code = l_code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\nusing OTPAthentication = Architecture.WinApp.OTPAthentication;\n")

with open(path_login, "w", encoding="utf-8") as f:
    f.write(l_code)

# Fix DashboardNew.cs
path_dash = r"d:\CShapEccorErp\src\DashboardNew.cs"
with open(path_dash, "r", encoding="utf-8", errors="ignore") as f:
    d_code = f.read()

d_code = d_code.replace("ApplicationsAllocateToUser.GetApplicationsAllocateToCompany()", "global::Architecture.BLL.ApplicationsAllocateToUser.GetApplicationsAllocateToCompany()")
d_code = d_code.replace("Company comp = Company.GetByID(UserAccount.CompanyId);", "global::Architecture.BLL.Company comp = global::Architecture.BLL.Company.GetByID(UserAccount.CompanyId);")
d_code = d_code.replace("Company comp = global::Architecture.BLL.Company.GetByID(UserAccount.CompanyId);", "global::Architecture.BLL.Company comp = global::Architecture.BLL.Company.GetByID(UserAccount.CompanyId);")

d_code = d_code.replace("CommonServices.", "global::Architecture.Common.CommonServices.")
d_code = d_code.replace("global::Architecture.Common.global::Architecture.Common.CommonServices.", "global::Architecture.Common.CommonServices.")

d_code = d_code.replace("new SqlLogin().ShowDialog();", "// new SqlLogin().ShowDialog();")

with open(path_dash, "w", encoding="utf-8") as f:
    f.write(d_code)

print("Applied final fixes to LoginNew.cs and DashboardNew.cs!")
