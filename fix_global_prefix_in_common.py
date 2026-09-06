path_common = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_common, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("Architecture.Common.clsGlobalVariables", "global::Architecture.Common.clsGlobalVariables")
code = code.replace("Architecture.BLL.Accounts.COAAllocation", "global::Architecture.BLL.Accounts.COAAllocation")

with open(path_common, "w", encoding="utf-8") as f:
    f.write(code)

print("Added global:: prefix in CommonServices.cs!")
