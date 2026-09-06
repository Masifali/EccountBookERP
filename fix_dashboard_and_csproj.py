import os

# 1. Update build_csproj.py to include System.Configuration reference
path_build = r"d:\CShapEccorErp\build_csproj.py"
with open(path_build, "r", encoding="utf-8") as f:
    build_code = f.read()

if '<Reference Include="System.Configuration" />' not in build_code:
    build_code = build_code.replace('<Reference Include="System" />', '<Reference Include="System" />\n    <Reference Include="System.Configuration" />')
    with open(path_build, "w", encoding="utf-8") as f:
        f.write(build_code)
    print("Updated build_csproj.py with System.Configuration")

# 2. Fix DashboardNew.cs BLL call
path_dash = r"d:\CShapEccorErp\src\DashboardNew.cs"
with open(path_dash, "r", encoding="utf-8") as f:
    dash_code = f.read()

dash_code = dash_code.replace("ThemeSettings.GetThemeSettingsByScreenList", "Architecture.BLL.Themes_ERP.ThemeSettings.GetThemeSettingsByScreenList")

with open(path_dash, "w", encoding="utf-8") as f:
    f.write(dash_code)

print("Updated DashboardNew.cs")
