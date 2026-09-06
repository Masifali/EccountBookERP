import os
import re

def clean_file(filepath):
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        lines = f.readlines()

    seen_aliases = set()
    new_lines = []

    for line in lines:
        match = re.match(r'^\s*using\s+([A-Za-z0-9_]+)\s*=\s*', line)
        if match:
            alias = match.group(1)
            if alias in seen_aliases:
                continue  # Skip duplicate alias definition
            seen_aliases.add(alias)
        new_lines.append(line)

    with open(filepath, "w", encoding="utf-8") as f:
        f.writelines(new_lines)
    print(f"Cleaned aliases in {os.path.basename(filepath)}")

def main():
    target_files = [
        "ExpenseVoucher.cs",
        "ExpenseVoucherNew.cs",
        "ContraVoucher.cs",
        "DayBook.cs",
        "AccountsPrematurePaymentsReceipts.cs",
        "Acfrmfcbankreceipt.cs",
        "Acfrmfcbankreceipt_DebitCurrency.cs",
        "frmInvoicesAdjustmentVoucher.cs",
        "frmReceiptInvoicesAdjustmentVoucher.cs",
        "AcfrmDefPdcManagment.cs",
        "GeneralLedger.cs",
        "TrialBalance.cs",
        "Payables.cs",
        "Receivables.cs",
        "VoucherReport.cs"
    ]
    src_dir = r"d:\CShapEccorErp\src"
    for tf in target_files:
        fpath = os.path.join(src_dir, tf)
        if os.path.exists(fpath):
            clean_file(fpath)

if __name__ == "__main__":
    main()
