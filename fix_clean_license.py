path_lic = r"d:\CShapEccorErp\src\License.cs"
with open(path_lic, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Fix namespace
code = code.replace("namespace Architecture.WinApp.Common;", "namespace Architecture.WinApp.Common\n{") + "\n}\n"

# Fix using declarations
code = code.replace("using Aes encryptor = Aes.Create();", "using (Aes encryptor = Aes.Create())")
code = code.replace("using MemoryStream ms = new MemoryStream();", "using (MemoryStream ms = new MemoryStream())")

# Fix BLL ActivityLog and CLV method calls
code = code.replace("ActivityLog.GetDate(", "global::Architecture.BLL.Authentication.ActivityLog.GetDate(")
code = code.replace("ActivityLog.Save(", "global::Architecture.BLL.Authentication.ActivityLog.Save(")
code = code.replace("CLV = CLV.GetLicense(", "CLV = global::Architecture.BLL.CLV.GetLicense(")

with open(path_lic, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed clean License.cs!")
