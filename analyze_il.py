import os
import glob
import re
from collections import Counter

il_dir = r"d:\CShapEccorErp\il_by_namespace"
files = glob.glob(os.path.join(il_dir, "*.txt"))

print(f"Total txt files: {len(files)}")

class_names = []
method_count = 0
opcodes = Counter()
string_literals = set()
field_references = set()
method_calls = set()

class_header_re = re.compile(r"^CLASS\s+(.+)$")
method_header_re = re.compile(r"^;\s*([^\s(]+)(?:\s*\((.*)\))?")
il_inst_re = re.compile(r"^\s*IL_[0-9a-fA-F]+:\s*([^\s]+)(?:\s+(.*))?$")

for f in files:
    with open(f, "r", encoding="utf-8", errors="ignore") as fp:
        for line in fp:
            line_str = line.strip()
            cm = class_header_re.match(line_str)
            if cm:
                class_names.append(cm.group(1))
                continue
            mm = method_header_re.match(line_str)
            if mm:
                method_count += 1
                continue
            im = il_inst_re.match(line_str)
            if im:
                op = im.group(1)
                operand = im.group(2) or ""
                opcodes[op] += 1
                if op == "ldstr":
                    string_literals.add(operand)
                elif op in ("ldfld", "stfld", "ldsfld", "stsfld"):
                    field_references.add(operand)
                elif op in ("call", "callvirt", "newobj"):
                    method_calls.add(operand)

print(f"Total classes found: {len(class_names)}")
print(f"Total methods found: {method_count}")
print(f"Top 20 opcodes: {opcodes.most_common(20)}")
print(f"Sample field references (first 10): {list(field_references)[:10]}")
print(f"Sample method calls (first 10): {list(method_calls)[:10]}")
