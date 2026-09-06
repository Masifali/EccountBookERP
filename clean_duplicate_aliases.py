import os

def remove_dup_lines(path, duplicates):
    if not os.path.exists(path):
        return
    with open(path, "r", encoding="utf-8", errors="ignore") as f:
        lines = f.readlines()
    new_lines = []
    seen = set()
    for line in lines:
        stripped = line.strip()
        if stripped in duplicates:
            if stripped in seen:
                continue
            seen.add(stripped)
        new_lines.append(line)
    with open(path, "w", encoding="utf-8") as f:
        f.writelines(new_lines)
    print(f"Cleaned {os.path.basename(path)}")

remove_dup_lines(r"d:\CShapEccorErp\src\EnterLicenseKey.cs", {"using UserAccount = Architecture.Model.UserAccount;"})
remove_dup_lines(r"d:\CShapEccorErp\src\LicenseKey.cs", {"using UserAccount = Architecture.Model.UserAccount;"})
remove_dup_lines(r"d:\CShapEccorErp\src\frmMessageBoxOk.cs", {"using Appearance = Infragistics.Win.Appearance;", "using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;"})
