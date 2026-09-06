import re

def check_dashboard():
    dash_path = r"d:\CShapEccorErp\src\DashboardNew.cs"
    with open(dash_path, "r", encoding="utf-8") as f:
        content = f.read()

    get_types_matches = re.findall(r'GetTypes\(\)', content)
    print(f"DashboardNew.cs GetTypes() matches: {len(get_types_matches)}")

if __name__ == "__main__":
    check_dashboard()
