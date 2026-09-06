import os

# 1. Fix frmMessageBoxOk.cs
path_msg = r"d:\CShapEccorErp\src\frmMessageBoxOk.cs"
if os.path.exists(path_msg):
    with open(path_msg, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    code = code.replace("using Architecture.Model;", "using Architecture.Model;\nusing UserAccountAllocation = Architecture.Model.UserAccountAllocation;\nusing Appearance = Infragistics.Win.Appearance;\nusing AutoCompleteMode = Infragistics.Win.AutoCompleteMode;")
    code = code.replace("UserAccountAllocation.GetAllCompaniesByUserId", "Architecture.BLL.UserAccountAllocation.GetAllCompaniesByUserId")
    with open(path_msg, "w", encoding="utf-8") as f:
        f.write(code)
    print("Fixed frmMessageBoxOk.cs")

# 2. Fix OTPAthentication.cs
path_otp = r"d:\CShapEccorErp\src\OTPAthentication.cs"
if os.path.exists(path_otp):
    with open(path_otp, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    code = code.replace("UserAccount.setAuthenticationCode", "Architecture.BLL.UserAccount.setAuthenticationCode")
    with open(path_otp, "w", encoding="utf-8") as f:
        f.write(code)
    print("Fixed OTPAthentication.cs")

# 3. Fix LicenseKey.cs
path_lk = r"d:\CShapEccorErp\src\LicenseKey.cs"
if os.path.exists(path_lk):
    with open(path_lk, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    code = code.replace("using Architecture.Model;", "using Architecture.Model;\nusing CLV = Architecture.Model.CLV;\nusing UserAccount = Architecture.Model.UserAccount;")
    code = code.replace("CLV.SaveLicense", "Architecture.BLL.CLV.SaveLicense")
    code = code.replace("UserAccount.GetLicensCompanyAndExpiryDate", "Architecture.BLL.UserAccount.GetLicensCompanyAndExpiryDate")
    with open(path_lk, "w", encoding="utf-8") as f:
        f.write(code)
    print("Fixed LicenseKey.cs")

# 4. Fix EnterLicenseKey.cs
path_elk = r"d:\CShapEccorErp\src\EnterLicenseKey.cs"
if os.path.exists(path_elk):
    with open(path_elk, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    code = code.replace("using Architecture.Model;", "using Architecture.Model;\nusing CLV = Architecture.Model.CLV;\nusing UserAccount = Architecture.Model.UserAccount;")
    code = code.replace("CLV.SaveLicense", "Architecture.BLL.CLV.SaveLicense")
    with open(path_elk, "w", encoding="utf-8") as f:
        f.write(code)
    print("Fixed EnterLicenseKey.cs")

# 5. Fix License.cs
path_lic = r"d:\CShapEccorErp\src\License.cs"
if os.path.exists(path_lic):
    with open(path_lic, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    code = code.replace("ActivityLog.GetDate", "Architecture.BLL.ActivityLog.GetDate")
    code = code.replace("ActivityLog.Save", "Architecture.BLL.ActivityLog.Save")
    code = code.replace("CLV.GetLicense", "Architecture.BLL.CLV.GetLicense")
    code = code.replace("using SqlCommand command =", "using (SqlCommand command =")
    code = code.replace("using SqlDataReader reader =", "using (SqlDataReader reader =")
    with open(path_lic, "w", encoding="utf-8") as f:
        f.write(code)
    print("Fixed License.cs")

# 6. Fix DashboardNew.cs
path_dash = r"d:\CShapEccorErp\src\DashboardNew.cs"
if os.path.exists(path_dash):
    with open(path_dash, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()
    if "using ButtonDisplayStyle =" not in code:
        code = code.replace("using Infragistics.Win.UltraWinGrid;", "using Infragistics.Win.UltraWinGrid;\nusing Appearance = Infragistics.Win.Appearance;\nusing ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle;")
    with open(path_dash, "w", encoding="utf-8") as f:
        f.write(code)
    print("Fixed DashboardNew.cs")

