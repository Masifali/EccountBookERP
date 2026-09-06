import os

target_base = r"d:\CShapEccorErp\EccountingERP\src\main\java\com\mst"

folders = [
    "configuration",
    "constants",
    "controllers",
    "datacontract",
    "email",
    "exception",
    "generate/password",
    "jasperReport",
    "kanta",
    "messages",
    "models",
    "projections",
    "quartz",
    "reports",
    "repositories",
    "security",
    "serviceInterface",
    "services",
    "sideBar",
    "smsclient",
    "specs",
    "tempuri",
    "utilities"
]

for folder in folders:
    folder_path = os.path.join(target_base, folder)
    os.makedirs(folder_path, exist_ok=True)
    print(f"Created/Verified folder: {folder_path}")

print("All 23 empty folders created under d:\\CShapEccorErp\\EccountingERP\\src\\main\\java\\com\\mst!")
