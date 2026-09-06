import os
import re

# 1. Fix LoginNew.cs appearance and AutoCompleteMode casts
path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Replace using Appearance = Infragistics.Win.Appearance; with global namespace
code = code.replace("using Appearance = Infragistics.Win.Appearance;", "")
code = code.replace("using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;", "")
code = re.sub(r'\(Appearance\)resources\.GetObject\(', '(Infragistics.Win.Appearance)resources.GetObject(', code)
code = re.sub(r'AutoCompleteMode\s*=\s*\((?:System\.Windows\.Forms\.)?AutoCompleteMode\)', 'AutoCompleteMode = (Infragistics.Win.AutoCompleteMode)', code)

with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

# 2. Fix global::Architecture in all C# files
for root, dirs, files in os.walk(r"d:\CShapEccorErp\src"):
    for file in files:
        if file.endswith(".cs"):
            p = os.path.join(root, file)
            with open(p, "r", encoding="utf-8", errors="ignore") as f:
                c = f.read()
            orig = c
            # Replace Architecture.BLL with global::Architecture.BLL
            c = re.sub(r'(?<!global::)\bArchitecture\.BLL\b', 'global::Architecture.BLL', c)
            c = re.sub(r'(?<!global::)\bArchitecture\.Model\b', 'global::Architecture.Model', c)
            c = re.sub(r'(?<!global::)\bArchitecture\.Common\b', 'global::Architecture.Common', c)
            if c != orig:
                with open(p, "w", encoding="utf-8") as f:
                    f.write(c)

print("Applied global:: prefix to Architecture namespaces across all src C# files!")
