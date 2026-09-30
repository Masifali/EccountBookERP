package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Save payload of the two desktop Day Book entry forms:
 *
 * <ul>
 *   <li>screen 15 "Day Book" — {@code Architecture.WinApp.Account_Definition.DayBook},
 *       DocumentTypeId 9: a Receipt grid (credit lines) and a Payment grid (debit lines) against
 *       one Cash Account, balanced by a cash line the server builds.</li>
 *   <li>screen 24 "Day Book (Off Set)" — {@code Architecture.WinApp.Account_Definition.frmDayBook},
 *       DocumentTypeId 8: one grid of credit/debit pairs entered together.</li>
 * </ul>
 *
 * The client posts the grid rows the operator built; the server turns them into VoucherDetail
 * lines exactly as each form's own Insert() / btnsave_Click does. Tenancy, users, dates of entry
 * and the document type are never taken from here.
 */
public class DayBookVoucherDto {

    /** 0 = Save, the VoucherHead id being edited = Update (the form's RecId). */
    public Integer id = 0;
    public String voucherCode;          // txtDocNo.Text
    public String voucherDate;          // txtDocDate.Value, "yyyy-MM-dd" or "yyyy-MM-dd HH:mm:ss"
    public Integer cashAccountId = 0;   // cmbCashAccount.Value
    public String remarks;              // txtHeaderRemarks (15) / txtVoucherRemarks (24)

    // ---------------------------------------------------------------- screen 15 (DayBook.cs)
    public List<Line> receipts = new ArrayList<>();   // ReceiptGrid -> CreditAmount lines
    public List<Line> payments = new ArrayList<>();   // PaymentGrid -> DebitAmount lines

    /** One row of ReceiptGrid / PaymentGrid (dtCredit / dtDedit columns). */
    public static class Line {
        public Integer accountId = 0;
        public Integer subsidiaryAccountId = 0;
        public Integer subsidiaryAccountTypeId = 0;
        public Double amount = 0d;
        public Integer pageNo = 0;
        public String chequeDate;
        public String chequeRef;
        public String remarks;
        public Integer lineId = 0;
    }

    // ---------------------------------------------------------------- screen 24 (frmDayBook.cs)
    public Integer currencyId = 0;          // cmbCurrency.Value
    public String exchangeRate;             // txtExchangeRate.Text
    public String fcyAmount;                // txtFcyAmount.Text
    public Integer temporaryAccountId = 0;  // CmbTemporaryAccount.Value
    public List<Row> rows = new ArrayList<>();
    /** The operator already answered Yes to VoucherExistWithSameAmountInSameDate. */
    public Boolean duplicateAcknowledged = Boolean.FALSE;

    /** One row of grdDetail (the form's "table" DataTable, in its column order). */
    public static class Row {
        public Integer subCode = 0;
        public Integer accountId = 0;
        public Integer againstAccountId = 0;
        public Integer subsidiaryAccountId = 0;
        public Integer subsidiaryAccountTypeId = 0;
        public String remarks;
        public String cheqNo;
        public Double totalAmountDr = 0d;
        public Double totalAmountCr = 0d;
        public Double amountDr = 0d;
        public Double amountCr = 0d;
        public Integer lineId = 0;
        public Integer cheqId = 0;
        public Double debitFcyAmount = 0d;
        public Double creditFcyAmount = 0d;
    }
}
