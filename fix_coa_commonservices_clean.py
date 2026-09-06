path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("using BLLCommonServices = global::Architecture.BLL.CommonServices;\n", "")
code = code.replace("BLLCommonServices.", "CommonServices.")

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Replaced BLLCommonServices with CommonServices in AcfrmDefCoa.cs!")
