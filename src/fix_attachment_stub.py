import re

def fix_attachment_file():
    att_path = r"d:\CShapEccorErp\src\Attachment.cs"
    with open(att_path, "r", encoding="utf-8") as f:
        content = f.read()

    # Comment out EDSDKLib using and camera fields/interfaces
    content = re.sub(r'using EDSDKLib;', '// using EDSDKLib;', content)
    content = re.sub(r'private CameraController controller;', '// private CameraController controller;', content)
    content = re.sub(r'private ActionSource actionSource;', '// private ActionSource actionSource;', content)
    content = re.sub(r'private ActionListener actionListener;', '// private ActionListener actionListener;', content)
    content = re.sub(r': Form, IObserver<CameraEvent>', ': Form', content)
    content = re.sub(r'public void Update\(Observable sender, CameraEvent e\)', '// public void Update(Observable sender, CameraEvent e)', content)

    with open(att_path, "w", encoding="utf-8") as f:
        f.write(content)
    print("Fixed Attachment.cs")

    csproj_path = r"d:\CShapEccorErp\src\Architecture.WinApp.csproj"
    with open(csproj_path, "r", encoding="utf-8") as f:
        c_content = f.read()

    if "Attachment.cs" not in c_content:
        c_content = re.sub(
            r'(<Compile Include="[^"]+" />)',
            r'<Compile Include="Attachment.cs" />\n    <Compile Include="AttachmentForDetailRows.cs" />\n    \1',
            c_content,
            count=1
        )
        with open(csproj_path, "w", encoding="utf-8") as f:
            f.write(c_content)
        print("Added Attachment.cs back to csproj")

if __name__ == "__main__":
    fix_attachment_file()
