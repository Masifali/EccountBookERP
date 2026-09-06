import re

def fix_csproj():
    csproj_path = r"d:\CShapEccorErp\src\Architecture.WinApp.csproj"
    with open(csproj_path, "r", encoding="utf-8") as f:
        content = f.read()

    # Remove Camera / EDSDK / ActionSource / Observable related Compile items
    patterns = [
        r'<Compile Include="CameraController\.cs" />\s*',
        r'<Compile Include="CameraEvent\.cs" />\s*',
        r'<Compile Include="CameraEventListener\.cs" />\s*',
        r'<Compile Include="CameraModel\.cs" />\s*',
        r'<Compile Include="CameraSetting\.cs" />\s*',
        r'<Compile Include="ActionSource\.cs" />\s*',
        r'<Compile Include="ActionListener\.cs" />\s*',
        r'<Compile Include="Observable\.cs" />\s*',
        r'<Compile Include="IObserver\.cs" />\s*',
        r'<Compile Include="EDSDK\.cs" />\s*',
        r'<Compile Include="ActionEvent\.cs" />\s*',
        r'<Compile Include="CommandProcessor\.cs" />\s*'
    ]

    for p in patterns:
        content = re.sub(p, '', content, flags=re.IGNORECASE)

    with open(csproj_path, "w", encoding="utf-8") as f:
        f.write(content)
    print("Removed camera items from csproj")

if __name__ == "__main__":
    fix_csproj()
