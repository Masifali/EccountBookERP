package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screens 853 frmCashReceiptVoucherTax and 854 frmBankReceiptVoucherTax -
 * desktop class Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew.
 *
 * What the page posts: the header controls and the grid ROWS the operator added (the desktop's DataTable
 * "table"). The ledger lines (vd / vd2 per row, WHT vd3-vd4, discount vd5-vd6) are derived on the server from
 * the form's own Insert(), so a caller can never choose which side of an entry is debited, nor submit its own
 * totals - Total() and the WHT amount are recomputed there.
 */
public class ReceiptTaxVoucherDto {
    /** 0 = Save, the loaded voucher's Id = Update (RecId). */
    public Integer Id = 0;
    public Integer VoucherCode = 0;            // txtvoucherno
    public String  VoucherDate;                // voucherdatetime, yyyy-MM-dd
    public Integer ProjectId = 0;              // CmbProjectId ("Cost Center")
    public Integer LocationTypeId = 0;         // cmbLocationType
    public Integer RefAccountId = 0;           // CmbDebitAccount (cash / bank account)
    public String  Remarks;                    // txtremarksmain
    public Integer MultiCurrencyId = 0;        // cmbCurrency
    public Double  ExchangeCurrencyRate = 0d;  // txtExchangeRate
    public Double  FcAmount = 0d;              // txtFcyAmount
    public Boolean CustomAccounts = Boolean.FALSE;   // chkCustomAccounts
    // ---- WHT
    public Boolean IncludeWHT = Boolean.FALSE;       // ChkBoxWthHolding
    public Integer TaxTypeId = 0;                    // CmbTaxType
    public String  TaxPercent;                       // txtTaxPercent.Text
    public Integer AgainstAcId = 0;                  // CmbAgainstAc -> vh.RefDocNoId   ("WHT Credit Ac")
    public Integer WithHoldingAcId = 0;              // CmbWithHoldingAc -> vh.AgainstAccountId ("WHT Debit Ac")
    // ---- Discount
    public Integer DiscountAccountId = 0;            // CmbDiscountAccount
    public Double  DiscountPercent = 0d;             // txtDiscPercent
    public Double  DiscountAmount = 0d;              // txtDiscountAmount
    /** txtDiscountAmount.Text exactly as shown (the discount line's Comments embed it). */
    public String  DiscountAmountText;
    public List<Row> rows = new ArrayList<>();
    /**
     * Desktop Yes/No prompts the operator already answered Yes to - only "whtMismatch" applies to a receipt.
     * The server still evaluates the check; an entry only stands for the Yes click.
     */
    public List<String> acknowledged = new ArrayList<>();

    /** One row of grd (Add_Click_1 -> table.Rows.Add(...)). */
    public static class Row {
        public Integer PaymentTypeId = 0;          // CmbPaymentType
        public Integer AccountId = 0;              // combactitle ("Credit Account")
        public Integer SubsidiaryAccountId = 0;    // CmbSubsidiaryAccount (or the account itself, type 4)
        public Integer SubsidiaryAccountTypeId = 0;
        public Integer JobLotId = 0;               // CmbJobLot
        public String  Remarks;                    // txtremarks
        public Integer TcyCodeId = 0;              // cmbTcyCodeDetail
        public Double  TcyExchangeRate = 0d;       // txtTcyExchangeRateDetail
        public Double  FcyAmount = 0d;             // txtFcyAmountDetail ("Tcy Amount")
        public Double  Amount = 0d;                // txtamount ("Credit Amount")
        public Integer ReferenceAccountId = 0;     // CmbReferenceAccount
        public String  ChequeDate;                 // CheqDate
        public String  ChequeNo;                   // CmbCheqNo.Text (plain text on a receipt)
        public String  PayeeTitle;                 // txtPayTitle
        public Integer BranchId = 0;               // cmbBranchName (0 when the Branch feature is off)
        public Integer CostCenterId = 0;           // CmbCostCenter (booking office only)
    }
}
