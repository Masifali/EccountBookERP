path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    lines = f.readlines()

for idx, line in enumerate(lines):
    if "Grid Record" in line or "Record Not Found" in line or "btnsave" in line or "BranchesBindInGrid" in line:
        print(f"Line {idx+1}: {line.strip()}")
