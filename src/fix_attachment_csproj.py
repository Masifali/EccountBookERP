import re

def fix_csproj_and_test():
    csproj_path = r"d:\CShapEccorErp\src\Architecture.WinApp.csproj"
    with open(csproj_path, "r", encoding="utf-8") as f:
        content = f.read()

    patterns = [
        r'<Compile Include="Attachment\.cs" />\s*',
        r'<Compile Include="AttachmentForDetailRows\.cs" />\s*',
        r'<Compile Include="Command\.cs" />\s*',
    ]

    for p in patterns:
        content = re.sub(p, '', content, flags=re.IGNORECASE)

    with open(csproj_path, "w", encoding="utf-8") as f:
        f.write(content)
    print("Removed Attachment and Command from csproj")

if __name__ == "__main__":
    fix_csproj_and_test()
