path_dash = r"d:\CShapEccorErp\src\DashboardNew.cs"
with open(path_dash, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("using WagesRefDocumentsStatus = global::Architecture.Model.ContractorWages.WagesRefDocumentsStatus;", "using WagesRefDocumentsStatus = global::Architecture.BLL.ContractorWages.WagesRefDocumentsStatus;")
if "using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;" not in code:
    code = code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\nusing DropDownBind = global::Architecture.WinApp.Common.DropDownBind;\nusing DDL = global::Architecture.WinApp.Common.DropDownBind;")

with open(path_dash, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed DashboardNew.cs aliases!")
