import os
import re
import glob

SRC_DIR = r"d:\CShapEccorErp\src"

def fix_file(filepath):
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()

    modified = False

    # Fix file-scoped namespace: namespace Architecture.WinApp;
    match_ns = re.search(r'^\s*namespace\s+([A-Za-z0-9_\.]+)\s*;', content, flags=re.MULTILINE)
    if match_ns:
        ns_name = match_ns.group(1)
        # Remove the file-scoped line
        content_no_ns = re.sub(r'^\s*namespace\s+[A-Za-z0-9_\.]+\s*;\r?\n', '', content, flags=re.MULTILINE)
        content = f"namespace {ns_name}\n{{\n{content_no_ns}\n}}\n"
        modified = True

    # Replace $ string interpolation $"..." with string.Format or string concatenation
    # Simple regex for $"text {var} text" -> string.Format("text {0} text", var)
    # Or replacing simple $"..." expressions where MSBuild C# 5 compiler complains
    def replace_interpolated(m):
        raw = m.group(0)
        str_body = m.group(1)
        # extract expressions in {expr}
        exprs = re.findall(r'\{([^}]+)\}', str_body)
        if not exprs:
            return f'"{str_body}"'
        fmt_str = str_body
        for idx, expr in enumerate(exprs):
            fmt_str = fmt_str.replace("{" + expr + "}", "{" + str(idx) + "}")
        args_str = ", ".join(exprs)
        return f'string.Format("{fmt_str}", {args_str})'

    new_content = re.sub(r'\$"([^"\r\n]*)"', replace_interpolated, content)
    if new_content != content:
        content = new_content
        modified = True

    if modified:
        with open(filepath, "w", encoding="utf-8") as f:
            f.write(content)
        print(f"Fixed {os.path.basename(filepath)}")

all_cs = glob.glob(os.path.join(SRC_DIR, "**", "*.cs"), recursive=True)
for cs in all_cs:
    fix_file(cs)

print("Finished fixing all C# files in src!")
