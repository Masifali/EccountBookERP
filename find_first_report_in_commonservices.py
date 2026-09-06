path = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    lines = f.readlines()

for idx, line in enumerate(lines):
    if "new Reporting()" in line or "Reporting val" in line:
        print(f"First Reporting call found at line {idx}: {line.strip()}")
        # Find method declaration start above
        for sub in range(idx, max(0, idx - 30), -1):
            if "public static" in lines[sub]:
                print(f"Method declaration start at line {sub}: {lines[sub].strip()}")
                break
        break
