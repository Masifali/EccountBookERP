path = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    lines = f.readlines()

cut_index = -1
for idx, line in enumerate(lines):
    if "public static void GdnReturnable_Slip_1203" in line:
        cut_index = idx
        break

if cut_index != -1:
    new_lines = lines[:cut_index]
    new_lines.append("\n}\n}\n")
    with open(path, "w", encoding="utf-8") as f:
        f.writelines(new_lines)
    print(f"Truncated CommonServices.cs at line {cut_index}")
