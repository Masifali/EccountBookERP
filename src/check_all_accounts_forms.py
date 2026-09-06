import os
import re
import xml.etree.ElementTree as ET

def check_forms():
    csproj_path = r"d:\CShapEccorErp\src\Architecture.WinApp.csproj"
    with open(csproj_path, "r", encoding="utf-8") as f:
        csproj_content = f.read()

    # Find all compile items in csproj
    compiled_files = set(re.findall(r'<Compile Include="([^"]+)"', csproj_content, re.IGNORECASE))
    print(f"Total compiled files in csproj: {len(compiled_files)}")

    # Check specific forms mentioned in screenshot / dashboard
    target_forms = [
        "frmPartyPaymentVoucher.cs",
        "ExpenseVoucher.cs",
        "ContraVoucher.cs",
        "JournalVoucher.cs",
        "DayBook.cs",
        "Acfrmfcbankreceipt.cs",
        "BalanceSheet.cs",
        "GeneralLedger.cs",
        "ProfitLoss.cs",
        "TrialBalance.cs",
        "Payables.cs",
        "Receivables.cs",
        "AccountsReconciliation.cs",
        "AcfrmDefPdcManagment.cs",
        "PostDatedCheqPaymentVouchers.cs",
        "VoucherEntry.cs",
        "VoucherReport.cs",
        "JournalVoucher_New.cs",
        "ExpenseVoucherNew.cs",
        "PaymentVoucherNew.cs",
        "ReceiptsVoucherNew.cs",
        "PaymentByInvoiceVoucherNew.cs",
        "ReceiptByInvoiceAccount.cs",
        "PaymentByInvoiceAccount.cs",
        "AccountsPrematurePaymentsReceipts.cs",
        "frmDayBook.cs",
        "Acfrmfcbankreceipt_DebitCurrency.cs",
        "AcfrmFcyPaymentVoucher.cs",
        "FCYVoucherEntry.cs"
    ]

    print("\n--- Check target forms in csproj ---")
    missing_in_csproj = []
    for tf in target_forms:
        if tf in compiled_files:
            print(f"[COMPILED] {tf}")
        else:
            print(f"[MISSING ] {tf}")
            missing_in_csproj.append(tf)

    print(f"\nTotal missing in csproj: {len(missing_in_csproj)}")

if __name__ == "__main__":
    check_forms()
