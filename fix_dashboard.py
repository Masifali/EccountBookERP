import re

def fix_file_usings(filepath):
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()

    # Remove ambiguous usings if both BLL and Model are present, or qualify
    code = code.replace("using Architecture.BLL;\n", "")
    code = code.replace("using Architecture.Reports;\n", "")

    # Replace UserAccount in parameter or field with Architecture.Model.UserAccount
    code = re.sub(r'\bUserAccount\b', 'Architecture.Model.UserAccount', code)
    code = re.sub(r'\btblUserRights\b', 'Architecture.Model.tblUserRights', code)

    with open(filepath, "w", encoding="utf-8") as f:
        f.write(code)
    print(f"Fixed {filepath}")

fix_file_usings(r"d:\CShapEccorErp\src\DashboardNew.cs")
fix_file_usings(r"d:\CShapEccorErp\src\EnterLicenseKey.cs")
fix_file_usings(r"d:\CShapEccorErp\src\LicenseKey.cs")
