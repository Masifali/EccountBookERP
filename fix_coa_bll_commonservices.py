path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

aliases = "using BLLCommonServices = global::Architecture.BLL.CommonServices;\n"
if "using BLLCommonServices =" not in code:
    code = code.replace("namespace Architecture.WinApp\n{\n", "namespace Architecture.WinApp\n{\n" + aliases)

code = code.replace("CommonServices.CoaAllocationGetForComboServiceBind", "BLLCommonServices.CoaAllocationGetForComboServiceBind")
code = code.replace("CommonServices.CompanyServiceBind", "BLLCommonServices.CompanyServiceBind")
code = code.replace("CommonServices.SetRightsValueInRightsObject", "BLLCommonServices.SetRightsValueInRightsObject")
code = code.replace("CommonServices.GetERPFeatureById", "BLLCommonServices.GetERPFeatureById")
code = code.replace("CommonServices.GetConfigurationByOrgCompandConfigDescription", "BLLCommonServices.GetConfigurationByOrgCompandConfigDescription")

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated CommonServices calls in AcfrmDefCoa.cs!")
