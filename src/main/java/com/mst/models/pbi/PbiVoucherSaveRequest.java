package com.mst.models.pbi;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import java.util.ArrayList;
import java.util.List;

/**
 * What the Payment By Invoice Voucher page posts on Save / Update - the controls
 * PaymentByInvoiceVoucherNew.Save_Click / Update_Click read, nothing more. Tenancy (Organization,
 * Company of the voucher, user, financial year) is never taken from here.
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class PbiVoucherSaveRequest {

    /** 0 on Save; the loaded voucher's id on Update (the form's static Id). */
    public Integer Id = 0;

    public Integer CompanyId = 0;          // combcomp (validated only - vh.CompanyId is UserAccount.CompanyId)
    public Integer BranchId = 0;           // combbranch
    public Integer ProjectId = 0;          // combproject
    public Integer DocumentTypeId = 0;     // combvtype: 1 CPV, 2 BPV
    public Integer VoucherCode = 0;        // txtvoucherno (ReadOnly)
    public String  VoucherDate;            // voucherdatetime, yyyy-MM-dd
    public Integer RefAccountId = 0;       // combcreditac
    public String  ChequeDate;             // CheqDate, yyyy-MM-dd
    public Integer CheqId = 0;             // CmbCheqNo.Value
    public String  ChequeNo;               // CmbCheqNo.Text
    public String  PayTitle;               // txtPayTitle
    public String  Remarks;                // txtremarksmain

    public Boolean IncludeWHT = Boolean.FALSE;   // ChkBoxWthHolding
    public Integer WhtAgainstAccountId = 0;      // CmbAgainstAc  -> vh.RefDocNoId
    public Integer WhtAccountId = 0;             // CmbWithHoldingAc -> vh.AgainstAccountId
    public Integer TaxTypeId = 0;                // CmbTaxType
    public String  TaxPercent;                   // txtTaxPercent.Text (read only on the form)

    public List<Row> rows = new ArrayList<>();

    /** One row of grd (the form's DataTable `table`). */
    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Row {
        public Integer RefDocumentTypeId = 0;
        public Integer InvoiceId = 0;
        public String  InvoiceNo;
        public Integer AccountId = 0;
        public Integer JobLotId = 0;
        public String  Remarks;
        public Double  WHT = 0d;
        public Double  InvoiceAmount = 0d;
        public Double  TotalPaidAmount = 0d;
        public Double  BalanceAmount = 0d;
        public Double  Amount = 0d;
    }
}
