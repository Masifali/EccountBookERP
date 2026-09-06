import os, glob

services_dir = r"d:\CShapEccorErp\EccountBookERP\src\main\java\com\mst\services"
count = 0

for root, dirs, files in os.walk(services_dir):
    for f in files:
        if f.endswith(".java"):
            fpath = os.path.join(root, f)
            with open(fpath, "r", encoding="utf-8", errors="ignore") as file:
                content = file.read()
            
            if "import com.mst.serviceInterface.*;" not in content and "import com.mst.serviceInterface." not in content:
                content = content.replace("package com.mst.services;", "package com.mst.services;\n\nimport com.mst.serviceInterface.*;")
                with open(fpath, "w", encoding="utf-8") as file:
                    file.write(content)
                count += 1

print(f"Added serviceInterface import to {count} service classes!")
