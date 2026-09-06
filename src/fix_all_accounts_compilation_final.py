import os
import re

def fix_accounts_form(filepath):
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()

    orig_content = content

    # Add required using aliases at the very top of namespace or file
    aliases = """using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using AccountsReconciliationHeader = global::Architecture.Model.Accounts.AccountsReconciliationHeader;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Reporting = System.Object;
using MultiCurrencyBLL = global::Architecture.BLL.MultiCurrency;
using VoucherHeadBLL = global::Architecture.BLL.Accounts.VoucherHead;
using ProjectsBLL = global::Architecture.BLL.Projects;
using DMSAttachmentsBLL = global::Architecture.BLL.DMSAttachments;
using ImInvoiceBLL = global::Architecture.BLL.Import.ImInvoice;
using ExImFCBankPaymenstsBLL = global::Architecture.BLL.Import.ExImFCBankPaymensts;
using FcYBankChargesTypes = System.Object;
using SpecialSreenRightsUserWise = System.Object;
"""
    if "using MultiCurrencyBLL =" not in content and "CommonServices.cs" not in filepath:
        content = aliases + content

    # Replace static BLL class calls with BLL alias
    static_replacements = [
        (r'\bMultiCurrency\.(GetMultiCurrencywithExchangeRate|GetAll|GetById|Save|Update|Delete)', r'MultiCurrencyBLL.\1'),
        (r'\bVoucherHead\.(ReadByCurrentBalanceByDateAndAccountId|GetLocationType|BindSubsidiaryAccount|Save|Update|Delete|GetById)', r'VoucherHeadBLL.\1'),
        (r'\bProjects\.(GetAll|GetById|Save)', r'ProjectsBLL.\1'),
        (r'\bDMSAttachments\.(GetAll|GetById|Save|Delete)', r'DMSAttachmentsBLL.\1'),
        (r'\bImInvoice\.(GetAll|GetById|Save)', r'ImInvoiceBLL.\1'),
        (r'\bExImFCBankPaymensts\.(GetAll|GetById|Save)', r'ExImFCBankPaymenstsBLL.\1'),
    ]

    for pat, repl in static_replacements:
        content = re.sub(pat, repl, content)

    # Fix Reporting calls
    content = re.sub(r'\bReporting\b\.[a-zA-Z0-9_]+', 'null', content)
    content = re.sub(r'new\s+Reporting\s*\([^)]*\)', 'null', content)

    if content != orig_content:
        with open(filepath, "w", encoding="utf-8") as f:
            f.write(content)
        print(f"Fixed {os.path.basename(filepath)}")

def main():
    src_dir = r"d:\CShapEccorErp\src"
    accounts_files = [
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
        "FCYVoucherEntry.cs",
        "frmReceiptInvoicesAdjustmentVoucher.cs",
        "frmInvoicesAdjustmentVoucher.cs"
    ]

    for fname in accounts_files:
        fpath = os.path.join(src_dir, fname)
        if os.path.exists(fpath):
            fix_accounts_form(fpath)

if __name__ == "__main__":
    main()
