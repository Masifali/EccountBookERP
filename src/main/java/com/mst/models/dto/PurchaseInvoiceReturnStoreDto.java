package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 346 "Purchase Invoice Return Store" ({@code PurchaseInvoiceReturn_Store.cs}, DocumentTypeId 145)
 * — what the page posts on Save / Update.
 *
 * Field names are the JSON keys the page sends (Jackson reads public fields by their exact name).
 * The grid rows carry the desktop grid's own column names (dtGrid, Form_Load:502-541) with one
 * exception: the "Tax%" column travels as {@code TaxPercent}.
 *
 * Several header values are posted as the TEXT of their box because the desktop validates the text,
 * not the number (Insert():2764/2770 compare {@code txtExchangeRate.Text} / {@code txtFcyAmount.Text}
 * with "" and "0"), and converts it with Conversion.ToDouble / ToDecimal / ToInt afterwards.
 *
 * Everything the server owns — organization, company, branch, financial year, users, dates of
 * entry/modification, the document number, ScreenName — is filled by the service from the session.
 */
public class PurchaseInvoiceReturnStoreDto {

    public Integer Id = 0;                    // RecId
    public String  DocDate;                   // DocDate picker, "yyyy-MM-dd"
    public String  DueDate;                   // duedate picker: "yyyy-MM-dd" or "yyyy-MM-ddTHH:mm:ss" (as read back)
    public String  DueDaysText;               // txtduedays.Text
    public Integer SupplierCustomerId = 0;    // CmbSupplierName
    public String  SupplierReferenceNo;       // txtSupplierReference
    public String  ManualBillNo;              // txtbillno
    public Integer PaymentTermId = 0;         // CmbPaymentTerm
    public Integer DeliveryTermId = 0;        // combdeliverytrm.Value (validated)
    public String  DeliveryTerm;              // combdeliverytrm.Text (saved)
    public String  RemarksHeader;             // txtremarks
    public Integer TaxAccountId = 0;          // CmbTaxAccount
    public String  BillAmountText;            // txtBillAmount.Text
    public Integer CurrencyId = 0;            // cmbCurrency
    public String  ExchangeRateText;          // txtExchangeRate.Text
    public String  InvoiceQtyText;            // txtInvoiceQty.Text
    public String  FcyAmountText;             // txtFcyAmount.Text
    public String  SupplierGLIdText;          // txtSupplierGLId.Text (hidden) — only used by the GL-grid check

    public List<Row> rows = new ArrayList<>();
    public List<Expense> expenses = new ArrayList<>();
    public List<Journal> journals = new ArrayList<>();
    public List<Freight> freights = new ArrayList<>();

    /** One row of grd (dtGrid). */
    public static class Row {
        public Integer Id = 0;
        public Integer InvGdnId = 0;
        public Integer InvGdnDetailId = 0;
        public Integer GdnNo = 0;
        public Integer RefDocumentTypeId = 0;
        public Integer RefDocId = 0;
        public Integer RefDocSubId = 0;
        public Integer ItemId = 0;
        public String  ItemCode;
        public String  Item;
        public Integer WarehouseId = 0;
        public String  Warehouse;
        public Integer RackId = 0;
        public String  RackName;
        public Integer ItemConditionId = 0;
        public String  ItemCondition;
        public Integer JobLotId = 0;
        public String  JobLot;
        public Integer PackUOMId = 0;
        public String  PackUOM;
        public Double  PackEquivalent = 0d;
        public Double  ItemQty = 0d;
        public Double  Rate = 0d;
        public Double  ItemAmount = 0d;
        public Double  FcyAmount = 0d;
        public Integer TaxTypeId = 0;
        public String  TaxTypeText;           // the TaxTypeId cell's display text (r.Cells["TaxTypeId"].Text)
        public Double  TaxPercent = 0d;       // "Tax%"
        public Double  TaxAmount = 0d;
        public Double  BillAmount = 0d;
        public Double  Expense = 0d;
        public Double  Journal = 0d;
        public Double  Freight = 0d;
        public String  GpDate;                // null = DBNull (Conversion.ToDateTime → 1900-01-01)
        public Integer GpNo = 0;
        public String  VehicleNo;
        public Integer CityId = 0;
        public String  CityName;
        public String  Remarks;
        public Integer ReasonId = 0;
        public Double  BalQty = 0d;
    }

    /** One row of grdInvExp (dtInvExp). */
    public static class Expense {
        public Integer Id = 0;
        public Integer ItemId = 0;
        public Double  Qty = 0d;
        public Double  Rate = 0d;
        public Double  Amount = 0d;
        public String  Remarks;
    }

    /** One row of grdGLedger (dtGrdGL). */
    public static class Journal {
        public Integer AccountId = 0;
        public Integer GlAccountId = 0;
        public String  Remarks;
        public Double  Percentage = 0d;
        public Double  Qty = 0d;
        public Double  Rate = 0d;
        public Double  Debit = 0d;
        public Double  Credit = 0d;
    }

    /** One row of grdFreight (dtFreight). */
    public static class Freight {
        public Integer Id = 0;
        public Integer GdnId = 0;
        public Integer Transporter = 0;
        public Integer GlAccountId = 0;
        public Double  Freight = 0d;
        public Double  Debit = 0d;
        public String  Remarks;
    }

    /** frmDefineReasons — Save / Update. */
    public static class Reason {
        public Integer ReasonId = 0;
        public Integer RefDocumentTypeId = 0;
        public String  Reason;
    }
}
