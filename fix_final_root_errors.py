import os

# 1. Update build_csproj.py to include System.Speech
path_build = r"d:\CShapEccorErp\build_csproj.py"
with open(path_build, "r", encoding="utf-8") as f:
    code = f.read()
if '<Reference Include="System.Speech" />' not in code:
    code = code.replace('<Reference Include="System" />', '<Reference Include="System" />\n    <Reference Include="System.Speech" />')
    with open(path_build, "w", encoding="utf-8") as f:
        f.write(code)
    print("Added System.Speech reference")

# 2. Fix AcfrmDefCoa.cs
path_coa = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
if os.path.exists(path_coa):
    with open(path_coa, "r", encoding="utf-8", errors="ignore") as f:
        c = f.read()
    if "using ChartofAccount = Architecture.Model.Accounts.ChartofAccount;" not in c:
        c = c.replace("using Architecture.Model.Accounts;", "using Architecture.Model.Accounts;\nusing ChartofAccount = Architecture.Model.Accounts.ChartofAccount;")
        with open(path_coa, "w", encoding="utf-8") as f:
            f.write(c)
        print("Fixed AcfrmDefCoa.cs")

# 3. Fix AcfrmOpeningBalance.cs
path_op = r"d:\CShapEccorErp\src\AcfrmOpeningBalance.cs"
if os.path.exists(path_op):
    with open(path_op, "r", encoding="utf-8", errors="ignore") as f:
        c = f.read()
    if "using AccountsOpeningBalances = Architecture.Model.Accounts.AccountsOpeningBalances;" not in c:
        c = c.replace("using Architecture.Model.Accounts;", "using Architecture.Model.Accounts;\nusing AccountsOpeningBalances = Architecture.Model.Accounts.AccountsOpeningBalances;")
        with open(path_op, "w", encoding="utf-8") as f:
            f.write(c)
        print("Fixed AcfrmOpeningBalance.cs")
