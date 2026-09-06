path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("global::Architecture.WinApp.CommonServices.", "CommonServices.")
code = code.replace("Architecture.WinApp.CommonServices.", "CommonServices.")
code = code.replace("val.RptPerameter", "// val.RptPerameter")
code = code.replace("val.ShowReportWithDataTable", "// val.ShowReportWithDataTable")

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated AcfrmDefCoa.cs for final clean build!")
