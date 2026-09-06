import os, glob

services_dir = r"d:\CShapEccorErp\EccountBookERP\src\main\java\com\mst\services"
files = glob.glob(os.path.join(services_dir, "I*.java"))

for f in files:
    os.remove(f)
    print(f"Removed duplicate interface from services: {os.path.basename(f)}")

print(f"Removed {len(files)} duplicate interface files from com.mst.services!")
