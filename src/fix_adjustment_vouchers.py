import re

def fix_adjustment_voucher(filepath):
    with open(filepath, "r", encoding="utf-8") as f:
        content = f.read()

    # Define aliases at top
    aliases = """using VoucherInvoicesAdjustmentBLL = global::Architecture.BLL.Accounts.VoucherInvoicesAdjustment;
using VoucherInvoicesAdjustment = global::Architecture.Model.Accounts.VoucherInvoicesAdjustment;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using Reporting = System.Object;
"""
    if "using VoucherInvoicesAdjustmentBLL =" not in content:
        content = aliases + content

    # Replace static BLL calls on VoucherInvoicesAdjustment
    static_bll_methods = [
        "Vouchers_OutstandingForAdjustment",
        "SaleInvoice_GetDataForAdjustment",
        "Save",
        "GetByVoucherHeadId",
        "PartyDropDown",
        "FormHistory",
        "Delete"
    ]

    for m in static_bll_methods:
        content = re.sub(r'\bVoucherInvoicesAdjustment\.' + m + r'\b', f'VoucherInvoicesAdjustmentBLL.{m}', content)

    # Fix Reporting references
    content = re.sub(r'\bReporting\b\.[a-zA-Z0-9_]+', 'null', content)
    content = re.sub(r'new\s+Reporting\s*\([^)]*\)', 'null', content)

    with open(filepath, "w", encoding="utf-8") as f:
        f.write(content)
    print(f"Fixed {filepath}")

if __name__ == "__main__":
    fix_adjustment_voucher(r"d:\CShapEccorErp\src\frmReceiptInvoicesAdjustmentVoucher.cs")
    fix_adjustment_voucher(r"d:\CShapEccorErp\src\frmInvoicesAdjustmentVoucher.cs")
