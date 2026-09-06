import os
import re

def transpile_il_file(filepath):
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        lines = f.readlines()

    current_class = None
    methods = []
    current_method = None

    class_re = re.compile(r"^====================================================================================================\s*CLASS\s+(.+)$")
    method_re = re.compile(r"^;\s*([^\s(]+)(?:\s*\((.*)\))?")
    il_re = re.compile(r"^\s*IL_[0-9a-fA-F]+:\s*([^\s]+)(?:\s+(.*))?$")

    for line in lines:
        line_str = line.strip()
        cm = class_re.match(line_str)
        if cm:
            current_class = cm.group(1)
            continue
        
        mm = method_re.match(line_str)
        if mm:
            mname = mm.group(1)
            mhdr = mm.group(2) or ""
            current_method = {
                "full_name": mname,
                "header": mhdr,
                "instructions": []
            }
            methods.append(current_method)
            continue

        im = il_re.match(line_str)
        if im and current_method:
            current_method["instructions"].append((im.group(1), im.group(2) or ""))

    return current_class, methods

# Test transpiling BarcodeGenerate.txt
cls, methods = transpile_il_file(r"d:\CShapEccorErp\il_by_namespace\Architecture.WinApp.BarcodeGenerate.txt")
print(f"Class: {cls}")
print(f"Methods count: {len(methods)}")
for m in methods[:5]:
    print(f"  Method: {m['full_name']} ({len(m['instructions'])} instructions)")
