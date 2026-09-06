import re
import os

def convert_props(path):
    if not os.path.exists(path):
        return
    with open(path, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()

    def fix_prop(m):
        prop_decl = m.group(1)
        expr = m.group(2)
        return f"{prop_decl}\n\t{{\n\t\tget {{ return {expr}; }}\n\t}}"

    # Match internal/public static/instance properties with expression body `=>`
    new_content = re.sub(r'(\b(?:public|internal)\s+(?:static\s+)?[A-Za-z0-9_\.]+\s+[A-Za-z0-9_]+\b)\s*=>\s*([^;\r\n]+);', fix_prop, content)

    with open(path, "w", encoding="utf-8") as f:
        f.write(new_content)
    print(f"Fixed {os.path.basename(path)}")

convert_props(r"d:\CShapEccorErp\src\Properties\Resources.cs")
convert_props(r"d:\CShapEccorErp\src\Properties\Settings.cs")
