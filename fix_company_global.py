path_common = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_common, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("Architecture.Common.", "global::Architecture.Common.")
code = code.replace("Architecture.BLL.", "global::Architecture.BLL.")
code = code.replace("Architecture.Model.", "global::Architecture.Model.")
code = code.replace("global::global::", "global::")

with open(path_common, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated CompanyServiceBind with global:: prefixes in CommonServices.cs!")
