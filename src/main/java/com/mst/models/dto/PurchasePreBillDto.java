package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 961 "Store Purchase Pre Bill" (frmPurchasePreBill.cs, DocumentTypeId 147) — what the page
 * posts to /api/store/purchase-pre-bill/save.
 *
 * Field names are the desktop control / grid column keys, spelled exactly as the page's JSON
 * (public fields: Jackson uses the field name as-is). Everything the server owns — organization,
 * company, branch, financial year, users, Doc No, entry dates, the header Bill Amount — is filled
 * by the service from the signed-in user, the stored document or its own arithmetic, never from here.
 */
public class PurchasePreBillDto {

    /** false: run the checks the desktop runs BEFORE its "Are you sure to Save/Update" prompt and
     *  answer with the prompt text; true: the operator said Yes — build and save. */
    public Boolean confirmed = Boolean.FALSE;

    public Integer Id = 0;                    // RecId (0 = new; btnsave_Click forces 0)
    public String  DocDate;                   // txtDocDate, "yyyy-MM-dd"
    public Integer BillToPartyId = 0;         // CmbBillToParty.Value
    public Integer VendorSupplierId = 0;      // CmbVendorSupplier.Value
    public Integer ReferencePartyId = 0;      // CmbRefParty.Value
    public Integer DeliveryTermId = 0;        // CmbDeliveryTerm.Value
    public Integer CityId = 0;                // CmbCityName.Value
    public String  VendorBillDate;            // txtVendorBillDate, "yyyy-MM-dd"
    public String  VendorBillNo;              // txtVendorBillNo
    public String  VehicleNo;                 // txtVehicleNo
    public String  BiltyNo;                   // txtBiltyNo
    public String  RemarksHeader;             // txtRemarksMain
    public String  DiscountAmount;            // txtDiscountAmountFooter (text — Conversion.ToDouble)

    public List<Row> rows = new ArrayList<>();          // grd (dtDetailGrid)
    public List<Row> removed = new ArrayList<>();       // lstRemoveRecord (rows deleted from an opened bill)
    public List<Expense> expenses = new ArrayList<>();  // grdInvExp (dtExpGrid)

    /** frmPurchasePreBill_Helper.InitializeDetailtable — the grid's columns. */
    public static class Row {
        public Integer Id = 0;                        // PurchasePreBillDetailId
        public Integer PurchaseDemandHeaderId = 0;
        public Integer PurchaseDemandDetailId = 0;
        public Integer PurchaseDemandNo = 0;
        public Integer ItemId = 0;
        public Integer UomId = 0;
        public Integer ItemConditionId = 0;
        public Double  DemandQty = 0d;
        public Double  PurchasedQty = 0d;
        public Double  BalanceQty = 0d;
        public Double  ThisQty = 0d;
        public Double  Rate = 0d;
        public Double  ItemAmountWithoutDiscount = 0d;
        public Double  DiscountAmount = 0d;
        public Double  ItemAmount = 0d;
        public Double  ExpenseAmount = 0d;
        public Double  ItemNetAmount = 0d;
        public String  Remarks;
    }

    /** dtExpGrid: Id, ItemId, Qty, Rate, Amount, Remarks. */
    public static class Expense {
        public Integer Id = 0;
        public Integer ItemId = 0;
        public Double  Qty = 0d;
        public Double  Rate = 0d;
        public Double  Amount = 0d;
        public String  Remarks;
    }
}
