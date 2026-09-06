import re

path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Fix decompiler artifact (ref accountClassEnum)
code = re.sub(r'\(\(object\)\(EnumAccountClass\)\(ref\s+([A-Za-z0-9_]+)\)\)\.ToString\(\)', r'\1.ToString()', code)

# Fix remaining BLL / Model methods in AcfrmDefCoa.cs
code = code.replace("City.GetAll", "Architecture.BLL.City.GetAll")
code = code.replace("AccountNotes.ReadByBSNote", "Architecture.BLL.Accounts.AccountNotes.ReadByBSNote")
code = code.replace("AccountNotes.ReadByPLNote", "Architecture.BLL.Accounts.AccountNotes.ReadByPLNote")
code = code.replace("InfragisticsHelper.", "// InfragisticsHelper.")

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed ref artifacts and BLL methods in AcfrmDefCoa.cs!")
