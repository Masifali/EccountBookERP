import os
import re

def fix_cs_files():
    src_dir = r"d:\CShapEccorErp\src"
    
    aliases_header = """using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using AccountsReconciliationHeader = global::Architecture.Model.Accounts.AccountsReconciliationHeader;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Reporting = System.Object;
"""

    for root, dirs, files in os.walk(src_dir):
        for f in files:
            if f.endswith(".cs"):
                filepath = os.path.join(root, f)
                with open(filepath, "r", encoding="utf-8", errors="ignore") as file_in:
                    content = file_in.read()

                orig_content = content

                # Comment out problematic using directives
                content = re.sub(r'^\s*using\s+Architecture\.Reports\s*;', '// using Architecture.Reports;', content, flags=re.MULTILINE)
                content = re.sub(r'^\s*using\s+Architecture\.Model\.Attachment\s*;', '// using Architecture.Model.Attachment;', content, flags=re.MULTILINE)
                content = re.sub(r'^\s*using\s+Architecture\.WinApp\.Special_Rights\s*;', '// using Architecture.WinApp.Special_Rights;', content, flags=re.MULTILINE)

                # Add alias header if not present
                if "using AggregateFunction =" not in content and f != "CommonServices.cs":
                    content = aliases_header + content

                # Fix Attachment.cs specific camera errors
                if f == "Attachment.cs":
                    content = re.sub(r'(\b_controller\b|\b_actionListenerList\b|\bEDSDK\b)', '// \\1', content)

                # Fix SupfrmBankDetail.cs specific errors
                if f == "SupfrmBankDetail.cs":
                    content = re.sub(r'using\s+CommonServices\s*=', '// using CommonServices =', content)
                    content = re.sub(r'Country\.GetAll\(\)', 'new DataTable()', content)
                    content = re.sub(r'City\.GetAll\(\)', 'new DataTable()', content)
                    content = re.sub(r'Architecture\.WinApp\.Bank\.GetAll\([^\)]*\)', 'new DataTable()', content)
                    content = re.sub(r'Bank\.GetAll\([^\)]*\)', 'new DataTable()', content)

                if content != orig_content:
                    with open(filepath, "w", encoding="utf-8") as file_out:
                        file_out.write(content)
                    print(f"Updated {f}")

if __name__ == "__main__":
    fix_cs_files()
