import os, glob, re

repo_dir = r"d:\CShapEccorErp\EccountBookERP\src\main\java"

for root, dirs, files in os.walk(repo_dir):
    for f in files:
        if f.endswith(".java"):
            fpath = os.path.join(root, f)
            with open(fpath, "r", encoding="utf-8", errors="ignore") as file:
                content = file.read()
            
            # Match IExtendedRepository<Type1, Type2> with missing 3rd argument
            new_content = re.sub(r'IExtendedRepository<([^,>]+),\s*([^,>]+)>', r'IExtendedRepository<\1, \2, Integer>', content)
            if new_content != content:
                with open(fpath, "w", encoding="utf-8") as file:
                    file.write(new_content)
                print(f"Updated: {f}")

print("All IExtendedRepository declarations updated to 3 generic parameters!")
