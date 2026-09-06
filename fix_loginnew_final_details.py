path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("global::Architecture.Common.CommonServices", "global::Architecture.WinApp.Common.CommonServices")
code = code.replace("new global::Architecture.BLL.UserAccountAllocation", "new global::Architecture.Model.UserAccountAllocation")

with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed LoginNew.cs namespace details!")
