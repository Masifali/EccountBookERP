import os
import re

path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Add UserAccountAllocation alias to top of LoginNew.cs
if "using UserAccountAllocation = Architecture.Model.UserAccountAllocation;" not in code:
    code = code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\nusing UserAccountAllocation = Architecture.Model.UserAccountAllocation;")

# Fix line 181 GetEncryptedUniqueDeviceId call
code = code.replace("global::Architecture.WinApp.Common.CommonServices.GetEncryptedUniqueDeviceId()", "global::Architecture.Common.CommonServices.GetEncryptedUniqueDeviceId()")

# Fix line 199 GetAllCompaniesByUserId call argument
code = re.sub(r'global::Architecture\.BLL\.UserAccountAllocation\.GetAllCompaniesByUserId\(\s*new\s+(?:global::Architecture\.BLL\.|global::Architecture\.Model\.|UserAccountAllocation)?UserAccountAllocation', 'global::Architecture.BLL.UserAccountAllocation.GetAllCompaniesByUserId(new global::Architecture.Model.UserAccountAllocation', code)

with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed final 2 lines in LoginNew.cs!")
