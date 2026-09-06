path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# 1. Fix double global:: prefix
while "global::Architecture.Common.global::Architecture.Common." in code:
    code = code.replace("global::Architecture.Common.global::Architecture.Common.", "global::Architecture.Common.")

# 2. Comment out dashboardNew.Show(); and Hide(); if DashboardNew is commented out
code = code.replace("dashboardNew.Show();\n\t\t\tHide();", "// dashboardNew.Show();\n\t\t\t// Hide();")
code = code.replace("dashboardNew.Show();", "// dashboardNew.Show();")

# 3. Fix C# 8 using HttpClient
code = code.replace("using HttpClient client = new HttpClient();", "using (HttpClient client = new HttpClient()) {")
code = code.replace("string token = await response.Content.ReadAsStringAsync();", "string token = await response.Content.ReadAsStringAsync();\n\t\t}")

with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

print("Fixed final 3 LoginNew.cs errors!")
