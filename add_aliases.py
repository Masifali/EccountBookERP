import os

files = [
    r"d:\CShapEccorErp\src\DashboardNew.cs",
    r"d:\CShapEccorErp\src\EnterLicenseKey.cs",
    r"d:\CShapEccorErp\src\LicenseKey.cs",
]

aliases = """using UserAccount = Architecture.Model.UserAccount;
using tblUserRights = Architecture.Model.tblUserRights;
using UserAccountAllocation = Architecture.Model.UserAccountAllocation;
"""

for path in files:
    with open(path, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()

    if "using UserAccount =" not in content:
        # insert after namespace Architecture.WinApp {
        content = content.replace("namespace Architecture.WinApp\n{\n", "namespace Architecture.WinApp\n{\n" + aliases)
        with open(path, "w", encoding="utf-8") as f:
            f.write(content)
        print(f"Added type aliases to {os.path.basename(path)}")

