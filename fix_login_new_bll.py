import re

path = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Replace static BLL calls:
code = re.sub(r'\bUserAccount\.Login\b', 'Architecture.BLL.UserAccount.Login', code)
code = re.sub(r'\bUserAccount\.LicenseCheck\b', 'Architecture.BLL.UserAccount.LicenseCheck', code)
code = re.sub(r'\bUserAccountAllocation\.GetAllCompaniesByUserId\b', 'Architecture.BLL.UserAccountAllocation.GetAllCompaniesByUserId', code)
code = re.sub(r'\bBranchesAllocationToUser\.GetBranchsAllocatedToUser\b', 'Architecture.BLL.BranchesAllocationToUser.GetBranchsAllocatedToUser', code)
code = re.sub(r'\bApplicationsAllocateToUser\.ApplicationsAllocateToUser_ByUser\b', 'Architecture.BLL.ApplicationsAllocateToUser.ApplicationsAllocateToUser_ByUser', code)
code = re.sub(r'\bFinancialYear\.GetFinancialYearlist\b', 'Architecture.BLL.FinancialYear.GetFinancialYearlist', code)
code = re.sub(r'\bCommonServices\.GetEncryptedUniqueDeviceId\b', 'Architecture.Common.CommonServices.GetEncryptedUniqueDeviceId', code)

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated LoginNew.cs BLL static methods successfully!")
