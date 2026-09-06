import re

def list_files():
    csproj_path = r"d:\CShapEccorErp\src\Architecture.WinApp.csproj"
    with open(csproj_path, "r", encoding="utf-8") as f:
        content = f.read()

    files = re.findall(r'<Compile Include="([^"]+)"', content, re.IGNORECASE)
    print(f"Total compiled files in csproj: {len(files)}")
    for f in sorted(files):
        print(f)

if __name__ == "__main__":
    list_files()
