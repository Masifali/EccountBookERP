import os
import re

def clean_file(filepath):
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        lines = f.readlines()

    seen_aliases = set()
    new_lines = []

    for line in lines:
        match = re.match(r'^\s*using\s+([A-Za-z0-9_]+)\s*=', line)
        if match:
            alias = match.group(1)
            if alias in seen_aliases:
                continue # Skip duplicate alias
            seen_aliases.add(alias)
        new_lines.append(line)

    with open(filepath, "w", encoding="utf-8") as f:
        f.writelines(new_lines)

def main():
    src_dir = r"d:\CShapEccorErp\src"
    for root, dirs, files in os.walk(src_dir):
        for file in files:
            if file.endswith(".cs"):
                clean_file(os.path.join(root, file))

if __name__ == "__main__":
    main()
