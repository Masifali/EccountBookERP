package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screens 28 / 29 / 30 / 31 - Cash Payment, Bank Payment, Cash Receipt and Bank Receipt Voucher.
 *
 * Desktop forms (ScreenDefinition / FavoriteScreens TargetUrl, and
 * CommonServices.EditMethodFromLinked, which opens DocumentTypeId 1/2 in
 * Architecture.WinApp.Account_Definition.PaymentVoucherNew with Tag "frmCashPaymentVoucher" /
 * "frmBankPaymentVoucher" and 3/4 in Architecture.WinApp.Account_Definition.ReceiptsVoucherNew with
 * Tag "frmCashReceiptVoucher" / "frmBankReceiptVoucher"). NOT the VouchersWithTax variants - those
 * are the separate "...Tax" screens.
 *
 * This is what the page sends: the header controls and the grid ROWS the operator added (the
 * desktop's DataTable "table"). The ledger lines (vd / vd2 / vd3 / vd4) are derived server-side
 * from the desktop's Insert(), so a caller can never choose which side of an entry is debited.
 * The procedure-shaped objects written by DesktopVoucherWriter are ContraVoucherDto.Head / Detail /
 * CostCentre, which mirror Architecture.Model.Accounts.VoucherHead / VoucherDetail /
 * voucherCostCenterDetail field for field (see that class).
 */
public class PaymentReceiptVoucherDto {

    /** 0 = Save (Insert), the loaded voucher's Id = Update (RecId). */
    public Integer Id = 0;

    public Integer VoucherCode = 0;          // txtvoucherno
    public String  VoucherDate;              // voucherdatetime, yyyy-MM-dd
    public Integer ProjectId = 0;            // CmbProjectId ("Cost Center" on CPV/BPV, "Project" on CRV/BRV)
    /** CPV/BPV CmbCreditAccount, CRV/BRV CmbDebitAccount - the cash/bank account. */
    public Integer RefAccountId = 0;
    public String  Remarks;                  // txtremarksmain
    public Integer MultiCurrencyId = 0;      // cmbCurrency
    public Double  ExchangeCurrencyRate = 0d;// txtExchangeRate
    public Double  FcAmount = 0d;            // txtFcyAmount

    // ---- WHT (ChkBoxWthHolding / CmbTaxType / txtTaxPercent / txtTaxAmount / CmbAgainstAc / CmbWithHoldingAc)
    public Boolean IncludeWHT = Boolean.FALSE;
    public Integer TaxTypeId = 0;
    public String  TaxTypeName;              // CmbTaxType.Text - used in the desktop's tax-row Comments
    public String  TaxPercent;               // txtTaxPercent.Text, as shown (Comments embed the text)
    public Double  TaxAmount = 0d;           // txtTaxAmount
    /** CmbAgainstAc -> vh.RefDocNoId. */
    public Integer AgainstAcId = 0;
    /** CmbWithHoldingAc -> vh.AgainstAccountId. */
    public Integer WithHoldingAcId = 0;
    /** CPV/BPV only: radioButton1 ("Included Tax") -> vh.InclusiveTax. */
    public Boolean InclusiveTax = Boolean.FALSE;

    // ---- BRV header cheque controls (ReceiptsVoucherNew: CmbCheqNo / CheqDate / txtPayTitle)
    public String  ChequeNo;
    public String  ChequeDate;
    public String  PayTitle;

    public List<Row> rows = new ArrayList<>();

    /**
     * Desktop Yes/No prompts the operator already answered Yes to - "whtMismatch",
     * "negativeBalance", "duplicate", "glBalance". The server still evaluates every check; an entry
     * here only stands for the Yes click. An outright refusal (PreventNegativeBalanceEntry) cannot be
     * acknowledged.
     */
    public List<String> acknowledged = new ArrayList<>();

    /** One row of grd (Add_Click_1). */
    public static class Row {
        public Integer PaymentTypeId = 0;     // CmbPaymentType
        public Integer AccountId = 0;         // CPV/BPV CmbAccountDetail, CRV/BRV combactitle
        public Integer JobLotId = 0;          // CmbJobLot
        public String  Remarks;               // txtremarks
        public Double  Amount = 0d;           // txtamount
        public Integer BranchId = 0;          // cmbBranchName (0 when the Branch feature is off)
        public Integer CostCenterId = 0;      // CmbCostCenter (booking office only)
        // BPV only
        public Integer FinancialInstrumentId = 0; // CmbFinancialInstrument
        public String  ChequeDate;                // CheqDate (CPV rows carry the hidden picker's date too)
        public Integer ChequeId = 0;              // CmbCheqNo.Value (cheque-book leaf id)
        public String  ChequeNo;                  // CmbCheqNo.Text
        public String  PayeeTitle;                // txtPayTitle
        public Integer ChequeTypeId = 0;          // cmbChequeType
    }
}
