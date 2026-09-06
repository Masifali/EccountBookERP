path = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    lines = f.readlines()

new_lines = lines[:431]
new_lines.append("\n}\n}\n")
with open(path, "w", encoding="utf-8") as f:
    f.writelines(new_lines)
print(f"Truncated CommonServices.cs to line 431 cleanly!")
