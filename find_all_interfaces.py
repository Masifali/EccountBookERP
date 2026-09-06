import os

master_dir = r"D:\EccountBookERP\src\main\java\com\mst"

for root, dirs, files in os.walk(master_dir):
    for f in files:
        if f.startswith("I") and "Service" in f and f.endswith(".java"):
            rel_path = os.path.relpath(os.path.join(root, f), master_dir)
            print(f"{f} -> {rel_path}")
