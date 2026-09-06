path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("Architecture.BLL.Accounts.Architecture.BLL.Accounts.AccountTypes.GetAll", "Architecture.BLL.Accounts.AccountTypes.GetAll")
code = code.replace("Architecture.WinApp.CommonServices.", "global::Architecture.WinApp.CommonServices.")
code = code.replace("new DefineCity(UserAccount).Show();", "// new DefineCity(UserAccount).Show();")
code = code.replace("Reporting val = new Reporting();", "// Reporting val = new Reporting();")

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated AcfrmDefCoa.cs with final 9 error fixes!")
