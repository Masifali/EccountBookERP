path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("ChartofAccount.Save(coa);", "Architecture.BLL.Accounts.ChartofAccount.Save(coa);")
code = code.replace("new // frmExcelSheetUpload(UserAccount).Show();", "// new frmExcelSheetUpload(UserAccount).Show();")
code = code.replace("Width = // Constants.", "Width = 100; // Constants.")

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed syntax and BLL calls in AcfrmDefCoa.cs!")
