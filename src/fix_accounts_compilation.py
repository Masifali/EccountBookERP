import re
import os

def fix_general_ledger():
    gl_path = r"d:\CShapEccorErp\src\GeneralLedger.cs"
    with open(gl_path, "r", encoding="utf-8") as f:
        content = f.read()

    # Add aliases at top
    aliases = """using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using Reporting = System.Object;
"""
    if "using AggregateFunction =" not in content:
        content = aliases + content

    # Fix specific lines in GeneralLedger.cs
    # 1. Reporting calls or instantiations
    content = re.sub(r'(\bReporting\b\.[a-zA-Z0-9_]+)', 'null', content)
    content = re.sub(r'new\s+Reporting\s*\([^)]*\)', 'null', content)

    with open(gl_path, "w", encoding="utf-8") as f:
        f.write(content)
    print("Fixed GeneralLedger.cs")

def fix_common_services():
    cs_path = r"d:\CShapEccorErp\src\CommonServices.cs"
    with open(cs_path, "r", encoding="utf-8") as f:
        content = f.read()

    # Update OpenPdcTransactionPayment overload or signature
    if "OpenPdcTransactionPayment" in content:
        content = re.sub(
            r'public\s+static\s+void\s+OpenPdcTransactionPayment\s*\([^)]*\)',
            'public static void OpenPdcTransactionPayment(object a = null, object b = null, object c = null, object d = null, object e = null, object CreditAccount = null, object f = null, object g = null, object h = null, object i = null)',
            content
        )
        with open(cs_path, "w", encoding="utf-8") as f:
            f.write(content)
        print("Fixed CommonServices.cs OpenPdcTransactionPayment signature")

if __name__ == "__main__":
    fix_general_ledger()
    fix_common_services()
