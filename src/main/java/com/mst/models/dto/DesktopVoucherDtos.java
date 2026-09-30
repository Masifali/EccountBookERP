package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * What the pages of screens 19 (VoucherEntry) and 46 (ExpenseVoucher) post: the header fields the
 * operator typed and the GRID ROWS exactly as the form's DataTable holds them. The ledger lines
 * (and, for the Expense Voucher, the credit mirror of every row) are derived on the server from
 * the form's own Insert(), never accepted from the client.
 *
 * Public fields, no accessors: Jackson binds each key by the field's own spelling, so the page
 * posts exactly these names.
 */
public final class DesktopVoucherDtos {

    private DesktopVoucherDtos() { }

    /** Screen 19 - Architecture.WinApp.Account_Definition.VoucherEntry. */
    public static class JournalEntry {
        public Integer id = 0;                  // RecId
        public Integer voucherCode = 0;         // txtdocno
        public String  voucherDate;             // DocDate, yyyy-MM-dd
        public Integer projectId = 0;           // CmbProjectId
        public String  remarks;                 // txtVoucherremarks
        public Integer multiCurrencyId = 0;     // cmbCurrency
        public Double  exchangeCurrencyRate = 0d;
        public Double  fcAmount = 0d;           // txtFcyAmount
        /** The form's DataTable "table", in ITS order (insertion order). */
        public List<JournalRow> rows = new ArrayList<>();
        /** The operator answered Yes to the same-amount-same-date warning. */
        public Boolean duplicateAcknowledged = Boolean.FALSE;
    }

    /** One row of VoucherEntry's "table" (columns added at DayBookVoucher_Load :1451). */
    public static class JournalRow {
        public String  accountCode;
        public Integer accountId = 0;
        public String  accountTitle;
        public Integer againstAccountId = 0;
        public Integer subsidiaryAccountId = 0;
        public String  subsidiaryAccount;
        public Integer subsidiaryAccountTypeId = 0;
        public String  remarks;
        public Integer jobLotId = 0;
        public String  jobLot;
        public String  cheqNo;
        public Double  amountDr = 0d;
        public Double  amountCr = 0d;
        public Integer lineId = 0;
        public Integer branchId = 0;
        public Integer costCenterId = 0;
        public Double  fcyAmount = 0d;
        public Integer sortIndex = 0;
    }

    /** Screen 46 - Architecture.WinApp.Account_Definition.ExpenseVoucher. */
    public static class Expense {
        public Integer id = 0;                  // RecId
        public Integer voucherCode = 0;         // txtVoucherCode
        public String  voucherDate;             // datVoucherDate
        public Integer projectId = 0;           // CmbProjectId
        public Integer refAccountId = 0;        // CmbRefAccountId ("Credit Account")
        public String  refAccountTitle;         // CmbRefAccountId.Text, for the balance message
        public String  chequeDate;              // datChequeDate
        public Integer cheqId = 0;              // CmbChequeNo.Value
        public String  chequeNo;                // CmbChequeNo.Text
        public String  payTitle;                // txtPayTitle
        public String  remarks;                 // txtRemarks
        public Integer multiCurrencyId = 0;
        public Double  exchangeCurrencyRate = 0d;
        public Double  fcAmount = 0d;
        /** grd's DataTable "table", in grid order. */
        public List<ExpenseRow> rows = new ArrayList<>();
        /** grdCostCenterDetail rows (dtGridCostCenterDetail). */
        public List<CostCenterBreakup> costCenters = new ArrayList<>();
        public Boolean duplicateAcknowledged = Boolean.FALSE;
        public Boolean negativeBalanceAcknowledged = Boolean.FALSE;
    }

    /** One row of ExpenseVoucher's "table" (columns added at AcfrmPaymentVoucher_Load :484). */
    public static class ExpenseRow {
        public String  accountCode;
        public Integer accountId = 0;
        public String  accountTitle;
        public Integer subsidiaryAccountId = 0;
        public String  subsidiaryAccount;
        public Integer subsidiaryAccountTypeId = 0;
        public Integer jobLotId = 0;
        public String  remarks;
        public Double  amount = 0d;
        public Double  fcyAmount = 0d;
        public Integer lineId = 0;
        public Integer branchId = 0;
        public Integer costCenterId = 0;
    }

    /** Screen 855 - Architecture.WinApp.Account_Definition.VouchersWithTax.ContraVoucher. */
    public static class ContraTax {
        public Integer id = 0;                  // RecId
        public Integer voucherCode = 0;         // txtvoucherno
        public String  voucherDate;             // voucherdatetime
        public Integer projectId = 0;           // CmbProjectId
        public Integer refAccountId = 0;        // CmbCreditAccount
        public String  remarks;                 // txtremarksmain
        public Integer multiCurrencyId = 0;     // cmbCurrency ("Tcy Code")
        public Double  exchangeCurrencyRate = 0d;
        public Double  fcAmount = 0d;           // txtFcyAmount ("Tcy Amount")
        public Boolean customAccounts = Boolean.FALSE;   // chkCustomAccounts
        public Integer locationTypeId = 0;      // cmbLocationType
        /** grd's DataTable "table" (:1123), in grid order. */
        public List<ContraTaxRow> rows = new ArrayList<>();
        public Boolean duplicateAcknowledged = Boolean.FALSE;
        public Boolean negativeBalanceAcknowledged = Boolean.FALSE;
    }

    public static class ContraTaxRow {
        public Integer accountTypeId = 0;
        public String  accountCode;
        public Integer accountId = 0;
        public Integer glCurrencyId = 0;
        public Integer jobLotId = 0;
        public String  remarks;
        public Integer tcyCodeId = 0;
        public Double  tcyExchangeRate = 0d;
        public Double  fcyAmount = 0d;
        public Double  amount = 0d;
        public Integer referenceAccountId = 0;
        public Integer branchId = 0;
        public Integer costCenterId = 0;
        public Integer chequeId = 0;
        public String  chequeDate;
        public String  chequeNo;
        public String  payTitle;
    }

    /** One row of dtGridCostCenterDetail (:500). */
    public static class CostCenterBreakup {
        public Integer id = 0;
        public Integer lineId = 0;
        public Integer parentId = 0;
        public String  debitAccount;
        public Integer costCenterId = 0;
        public String  costCenter;
        public Double  percent = 0d;
        public Double  amount = 0d;
    }
}
