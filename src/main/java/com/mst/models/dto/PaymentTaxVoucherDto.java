package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screens 850 PaymentVoucherWithTax, 851 frmBankPaymentVoucherTax and 852 frmCashPaymentVoucherTax -
 * desktop class Architecture.WinApp.Account_Definition.VouchersWithTax.PaymentVoucherNew.
 *
 * What the page posts: the header controls and the grid ROWS the operator added (the desktop's DataTable
 * "table"). The ledger lines (vd / vd2 / WHT vd3-vd4 / SRB vd5-vd6 / discount vd7-vd8) are derived on the
 * server from the form's own Insert(), so a caller can never choose which side of an entry is debited,
 * nor submit its own totals - Total() / TaxAmountProportion() are recomputed there.
 */
public class PaymentTaxVoucherDto {
    /** 0 = Save, the loaded voucher's Id = Update (RecId). */
    public Integer Id = 0;
    public Integer VoucherCode = 0;            // txtvoucherno
    public String  VoucherDate;                // voucherdatetime, yyyy-MM-dd
    /** CmbVoucherType.Value: 1 = Cash Payment Voucher, 2 = Bank Payment Voucher. */
    public Integer DocumentTypeId = 0;
    public Integer ProjectId = 0;              // CmbProjectId ("Cost Center")
    public Integer LocationTypeId = 0;         // cmbLocationType
    public Integer RefAccountId = 0;           // CmbCreditAccount
    public String  Remarks;                    // txtremarksmain
    public Integer MultiCurrencyId = 0;        // cmbCurrency
    public Double  ExchangeCurrencyRate = 0d;  // txtExchangeRate
    public Double  FcAmount = 0d;              // txtFcyAmount
    public Boolean CustomAccounts = Boolean.FALSE;   // chkCustomAccounts
    public Boolean InclusiveTax = Boolean.FALSE;     // radioButton1 ("Included Tax"); RadExcluded is the default
    // ---- WHT
    public Boolean IncludeWHT = Boolean.FALSE;       // ChkBoxWthHolding
    public Integer TaxTypeId = 0;                    // CmbTaxType
    public String  TaxTypeName;                      // CmbTaxType.Text (tax-row Comments)
    public String  TaxPercent;                       // txtTaxPercent.Text
    public Integer AgainstAcId = 0;                  // CmbAgainstAc  -> vh.RefDocNoId   ("WHT Debit Ac")
    public Integer WithHoldingAcId = 0;              // CmbWithHoldingAc -> vh.AgainstAccountId ("WHT Credit Account")
    // ---- SRB tax (Total() adds it to the payable)
    public Integer SrbAccountId = 0;                 // CmbSrbTaxAccount
    public String  SrbAccountName;                   // CmbSrbTaxAccount.Text
    public Double  SrbAmount = 0d;                   // txtSrbTaxAmount
    // ---- Discount
    public Integer DiscountAccountId = 0;            // CmbDiscountAccount
    public Double  DiscountPercent = 0d;             // txtDiscPercent
    public Double  DiscountAmount = 0d;              // txtDiscountAmount
    /** txtDiscountAmount.Text exactly as shown (the discount line's Comments embed it). */
    public String  DiscountAmountText;
    public List<Row> rows = new ArrayList<>();
    /**
     * Desktop Yes/No prompts the operator already answered Yes to - "whtMismatch", "negativeBalance",
     * "duplicate", "glBalance". The server still evaluates every check; an entry only stands for the
     * Yes click. An outright refusal (PreventNegativeBalanceEntry) cannot be acknowledged.
     */
    public List<String> acknowledged = new ArrayList<>();

    /** One row of grd (Add_Click_1 -> table.Rows.Add(...)). */
    public static class Row {
        public Integer PaymentTypeId = 0;          // CmbPaymentType
        public Integer AccountId = 0;              // CmbAccountDetail
        public Integer SubsidiaryAccountId = 0;    // CmbSubsidiaryAccount (or the account itself, type 4)
        public Integer SubsidiaryAccountTypeId = 0;
        public Integer JobLotId = 0;               // CmbJobLot
        public String  Remarks;                    // txtremarks
        public Integer TcyCodeId = 0;              // cmbTcyCodeDetail
        public Double  TcyExchangeRate = 0d;       // txtTcyExchangeRateDetail
        public Double  FcyAmount = 0d;             // txtFcyAmountDetail ("Tcy Amount")
        public Double  Amount = 0d;                // txtamount ("Debit Amount")
        public Integer ReferenceAccountId = 0;     // CmbReferenceAccount
        public Integer FinancialInstrumentId = 0;  // CmbFinancialInstrument (bank)
        public String  ChequeDate;                 // CheqDate (cash rows carry the hidden picker's date too)
        public Integer ChequeId = 0;               // CmbCheqNo.Value (cheque-book leaf id)
        public String  ChequeNo;                   // CmbCheqNo.Text
        public String  PayeeTitle;                 // txtPayTitle
        public Integer ChequeTypeId = 0;           // cmbChequeType
        public Integer BranchId = 0;               // cmbBranchName (0 when the Branch feature is off)
        public Integer CostCenterId = 0;           // CmbCostCenter (booking office only)
    }
}
