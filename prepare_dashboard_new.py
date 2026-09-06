import os
import re

# 1. Update DashboardNew.cs
path_dash = r"d:\CShapEccorErp\src\DashboardNew.cs"
with open(path_dash, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Add aliases to namespace top
aliases = """
using tblUserRights = global::Architecture.Model.tblUserRights;
using WagesRefDocumentsStatus = global::Architecture.Model.ContractorWages.WagesRefDocumentsStatus;
"""
if "using tblUserRights = global::Architecture.Model.tblUserRights;" not in code:
    code = code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\n" + aliases)

# Remove unused sub-namespace usings
sub_usings = [
    "using Architecture.Reports;",
    "using Architecture.WinApp.Account_Definition;",
    "using Architecture.WinApp.Audit_Dashboard;",
    "using Architecture.WinApp.Common;",
    "using Architecture.WinApp.Configurations;",
    "using Architecture.WinApp.Dashboard;",
    "using Architecture.WinApp.Helper;",
    "using Architecture.WinApp.Inventory_Reports;",
    "using Architecture.WinApp.PreBookingAndDelivery;",
    "using Architecture.WinApp.SQL;",
    "using Architecture.WinApp.Themes_ERP;"
]
for u in sub_usings:
    code = code.replace(u, "// " + u)

with open(path_dash, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated DashboardNew.cs!")

# 2. Uncomment DashboardNew in LoginNew.cs
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    l_code = f.read()

l_code = l_code.replace("// DashboardNew dashboardNew = new DashboardNew(clsGlobalVariables.UserAccount);", "DashboardNew dashboardNew = new DashboardNew(clsGlobalVariables.UserAccount);")
l_code = l_code.replace("// // dashboardNew.Show();", "dashboardNew.Show();")
l_code = l_code.replace("// Hide();", "Hide();")

with open(path_login, "w", encoding="utf-8") as f:
    f.write(l_code)

print("Uncommented DashboardNew in LoginNew.cs!")

# 3. Add DashboardNew.cs to build_minimal_login_csproj.py
path_b = r"d:\CShapEccorErp\build_minimal_login_csproj.py"
with open(path_b, "r", encoding="utf-8") as f:
    b_code = f.read()

if '"DashboardNew.cs",' not in b_code:
    b_code = b_code.replace('"LoginNew.cs",', '"LoginNew.cs",\n    "DashboardNew.cs",')
    with open(path_b, "w", encoding="utf-8") as f:
        f.write(b_code)

print("Added DashboardNew.cs to build_minimal_login_csproj.py!")
