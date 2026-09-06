import os, glob, re

java_dir = r"d:\CShapEccorErp\EccountBookERP\src\main\java"

count = 0
for root, dirs, files in os.walk(java_dir):
    for f in files:
        if f.endswith(".java"):
            fpath = os.path.join(root, f)
            with open(fpath, "r", encoding="utf-8", errors="ignore") as file:
                content = file.read()
            
            # Replace import com.mst.services.I... with import com.mst.serviceInterface.I...
            new_content = re.sub(r'import\s+com\.mst\.services\.(I[A-Za-z0-9_]+);', r'import com.mst.serviceInterface.\1;', content)
            
            if new_content != content:
                with open(fpath, "w", encoding="utf-8") as file:
                    file.write(new_content)
                count += 1
                print(f"Updated imports in: {f}")

print(f"Updated serviceInterface imports in {count} java files!")
