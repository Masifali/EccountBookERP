path = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    lines = f.readlines()

# Find line with `public static void Print` or `public static void DisplayDynamicReport`
cut_index = -1
for idx, line in enumerate(lines):
    if "public static void Print" in line or "public static void DisplayDynamicReport" in line:
        cut_index = idx
        break

if cut_index != -1:
    new_lines = lines[:cut_index]
    new_lines.append("\n}\n}\n")
    with open(path, "w", encoding="utf-8") as f:
        f.writelines(new_lines)
    print(f"Truncated CommonServices.cs at line {cut_index}")

path_ctrl = r"d:\CShapEccorErp\src\CtrlGrdBar.cs"
with open(path_ctrl, "r", encoding="utf-8", errors="ignore") as f:
    ctrl_code = f.read()

if "using GridLayout = Architecture.Model.GridLayout;" not in ctrl_code:
    ctrl_code = ctrl_code.replace("using Janus.Windows.GridEX.Export;", "using Janus.Windows.GridEX.Export;\nusing GridLayout = Architecture.Model.GridLayout;")
    with open(path_ctrl, "w", encoding="utf-8") as f:
        f.write(ctrl_code)
    print("Fixed CtrlGrdBar.cs GridLayout alias")
