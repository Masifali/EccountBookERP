import re

def add_assembly_info():
    csproj_path = r"d:\CShapEccorErp\src\Architecture.WinApp.csproj"
    with open(csproj_path, "r", encoding="utf-8") as f:
        content = f.read()

    if "AssemblyInfo.cs" not in content:
        # Add AssemblyInfo.cs to Compile items
        content = re.sub(
            r'(<Compile Include="[^"]+" />)',
            r'<Compile Include="AssemblyInfo.cs" />\n    \1',
            content,
            count=1
        )
        with open(csproj_path, "w", encoding="utf-8") as f:
            f.write(content)
        print("Added AssemblyInfo.cs to csproj")
    else:
        print("AssemblyInfo.cs already in csproj")

if __name__ == "__main__":
    add_assembly_info()
