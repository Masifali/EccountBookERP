package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 323 "Purchase Invoice Store Management" (PurchaseInvoiceStoreManagement.cs,
 * DocumentTypeId 64) — what the page posts to /api/store/purchase-invoice-store-management/save.
 *
 * Field names are the desktop controls / grid column keys, spelled exactly as the page's JSON
 * (public fields: Jackson uses the field name as-is). Everything the server owns — organization,
 * company, branch, financial year, users, Doc No, Branch Sr No, Invoice Tax No, entry dates — is
 * filled by the service from the signed-in user or the stored header, never from here.
 */
public class PurchaseInvoiceStoreMgmtDto {

    /** false: run the checks the desktop runs BEFORE its "Are you sure to Save/Update" prompt
     *  and answer with the prompt text; true: the operator said Yes — build and save. */
    public Boolean confirmed = Boolean.FALSE;

    public Integer Id = 0;                      // form Id (0 = new; btnSave forces 0)
    public String  DocDate;                     // DocDate picker, "yyyy-MM-dd"
    public Integer SupplierCustomerId = 0;      // cmbsuppliername.Value
    public String  ManualBillNo;                // txtbillno
    public Integer PaymentTermsId = 0;          // CmbPaymentTerm.Value
    public String  DueDays;                     // txtDueDays (text — Conversion.ToInt)
    public String  DueDate;                     // DueDate picker, "yyyy-MM-dd"
    public String  RemarksHeader;               // txtRemarks
    public Integer TaxAccountId = 0;            // CmbTaxAccount.Value
    public Integer DiscountAccountId = 0;       // CmbDiscountAccount.Value
    public String  DiscountAmount;              // txtBillDiscountAmountFooter (text)
    public String  BillAmount;                  // txtBillAmount (text, ReadOnly, computed by BillAmount())
    public Integer GrnBaseDocumentTypeId = 0;   // form field GrnBaseDocumentTypeId (set by Load GRN / ReadById)

    public List<Row> rows = new ArrayList<>();          // grd (dtGrid)
    public List<Freight> freight = new ArrayList<>();   // grdFreight (dtFreight)
    public List<Journal> journal = new ArrayList<>();   // grdGLedger (dtGrdGL)
    public List<Expense> expenses = new ArrayList<>();  // grdInvExp (dtExpGrid)

    /** frmPurchaseInvoiceStoreManagement_Helper.InitializeDetailtable — the grid's columns. */
    public static class Row {
        public Integer Id = 0;
        public Integer PurchaseOrderDocumentTypeId = 0;
        public Integer PurchaseOrderId = 0;
        public Integer PurchaseOrderNo = 0;
        public Integer PurchaseDemandDocumentTypeId = 0;
        public Integer PurchaseDemandId = 0;
        public Integer PurchaseDemandNo = 0;
        public Integer PreBillDocumentTypeId = 0;
        public Integer PreBillId = 0;
        public Integer PreBillNo = 0;
        public Integer DeliveryChallanDocumentTypeId = 0;
        public Integer DeliveryChallanId = 0;
        public Integer DeliveryChallanNo = 0;
        public Integer InvGrnDocumentTypeId = 0;
        public Integer InvGrnId = 0;
        public Integer InvGrnDetailId = 0;
        public Integer GrnNo = 0;
        public Integer WarehouseId = 0;
        public String  WareHouseName;
        public Integer ItemId = 0;
        public String  ItemName;
        public Integer ItemUomId = 0;
        public String  UOMCodeItem;
        public Integer ItemConditionId = 0;
        public String  ItemCondition;
        public Double  ItemQty = 0d;
        public Double  Rate = 0d;
        public Double  ItemAmountWithoutDiscount = 0d;
        public Double  DiscountAmount = 0d;
        public Double  ItemAmount = 0d;
        public Integer TaxNameId = 0;
        public String  TaxName;
        public Double  TaxPercent = 0d;
        public Double  TaxAmount = 0d;
        public Double  BillAmount = 0d;
        public Double  Freights = 0d;
        public Double  ExpenseAmount = 0d;
        public Integer PoAttachments = 0;
        public Integer DemandAttachments = 0;
        public Integer PreBillAttachments = 0;
        public Integer DeliveryChallanAttachments = 0;
        public Integer GrnAttachments = 0;
        public Integer RackId = 0;
        public String  RackName;
    }

    /** dtFreight: InvGrnId, Transporter, Freight (caption "Credit"), Debit. */
    public static class Freight {
        public Integer InvGrnId = 0;
        public Integer Transporter = 0;
        public Double  Freight = 0d;
        public Double  Debit = 0d;
    }

    /** dtGrdGL: AccountId, Remarks, Percentage, Qty, Rate, Debit, Credit. */
    public static class Journal {
        public Integer AccountId = 0;
        public String  Remarks;
        public Double  Percentage = 0d;
        public Double  Qty = 0d;
        public Double  Rate = 0d;
        public Double  Debit = 0d;
        public Double  Credit = 0d;
    }

    /** dtExpGrid: Id, ItemId, Qty, Rate, Amount, Remarks (+ the combo cell's display text). */
    public static class Expense {
        public Integer Id = 0;
        public Integer ItemId = 0;
        public String  ItemText;                // r4.Cells["ItemId"].Text — the combo's display text
        public Double  Qty = 0d;
        public Double  Rate = 0d;
        public Double  Amount = 0d;
        public String  Remarks;
    }
}
