import subprocess
import os
import re

def main():
    cmd = r'& "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" "d:\CShapEccorErp\src\Architecture.WinApp.csproj" /t:Build /p:Configuration=Debug /fl /flp:logfile=build.log;errorsonly'
    res = subprocess.run(["powershell", "-Command", cmd], capture_output=True, text=True, cwd=r"d:\CShapEccorErp\src")
    
    with open(r"d:\CShapEccorErp\src\build.log", "r", encoding="utf-8", errors="ignore") as f:
        log_content = f.read()

    errors = re.findall(r"(D:\\CShapEccorErp\\src\\[^(]+\((\d+),\d+\): error CS\d+: .+)", log_content)
    print(f"Total error lines parsed: {len(errors)}")
    
    # Print unique error messages
    unique_msgs = set()
    for err, line in errors[:30]:
        print(err)

if __name__ == "__main__":
    main()
