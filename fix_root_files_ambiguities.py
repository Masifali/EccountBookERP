import os
import glob
import re

src_dir = r"d:\CShapEccorErp\src"

files = [f for f in glob.glob(os.path.join(src_dir, "*.cs"))]

for path in files:
    with open(path, "r", encoding="utf-8", errors="ignore") as f:
        code = f.read()

    orig_code = code

    code = code.replace("private List<FinancialYear> Finlst = new List<FinancialYear>();", "private List<Architecture.Model.FinancialYear> Finlst = new List<Architecture.Model.FinancialYear>();")
    
    # Inject type aliases after usings if missing
    aliases = []
    if "using UserAccount = Architecture.Model.UserAccount;" not in code and ("UserAccount" in code):
        aliases.append("using UserAccount = Architecture.Model.UserAccount;")
    if "using FinancialYear = Architecture.Model.FinancialYear;" not in code and ("FinancialYear" in code):
        aliases.append("using FinancialYear = Architecture.Model.FinancialYear;")
    if "using City = Architecture.Model.City;" not in code and ("City" in code):
        aliases.append("using City = Architecture.Model.City;")
    if "using SupCustCreditLimitSchedule = Architecture.Model.Inventory.SupCustCreditLimitSchedule;" not in code and ("SupCustCreditLimitSchedule" in code):
        aliases.append("using SupCustCreditLimitSchedule = Architecture.Model.Inventory.SupCustCreditLimitSchedule;")

    if aliases:
        alias_block = "\n".join(aliases) + "\n"
        # Insert inside namespace if namespace exists, else at top
        if "namespace Architecture.WinApp" in code:
            code = code.replace("namespace Architecture.WinApp\n{", "namespace Architecture.WinApp\n{\n" + alias_block)
        elif "namespace " in code:
            code = re.sub(r'(namespace\s+[A-Za-z0-9_\.]+\s*\n\{)', r'\1\n' + alias_block, code, count=1)
        else:
            code = alias_block + code

    if code != orig_code:
        with open(path, "w", encoding="utf-8") as f:
            f.write(code)
        print(f"Updated ambiguities in {os.path.basename(path)}")

print("Done updating root C# ambiguities.")
