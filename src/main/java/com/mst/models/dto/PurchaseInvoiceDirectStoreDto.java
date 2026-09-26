package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 334 "Purchase Invoice Direct Store" ({@code frmPurchaseInvoiceDirectStore.cs},
 * DocumentTypeId 61) — what the page posts.
 *
 * Field names are the desktop's own DataTable column keys: {@code dtGrid} (Load:402-431) for
 * {@link Row}, {@code dtFreight} (Load:446-449) for {@link FreightRow}, {@code dtGrdGL}
 * (Load:435-442) for {@link JournalRow}. Only what the operator can see or type travels; every
 * server-owned column (organization, company, branch, financial year, users, entry / modify /
 * due dates, Doc No, Branch Sr No, Bill Amount, the per-row Freights and BillAmount) is filled by
 * the service, exactly where Insert():1201 fills it from UserAccount / clsGlobalVariables or
 * recomputes it with BillAmount():1621 and FreightProportion():2326.
 */
public class PurchaseInvoiceDirectStoreDto {

    /** Id — 0 for a new invoice, the header id when updating. */
    public Integer Id = 0;
    public String  DocDate;                 // DocDate picker, "yyyy-MM-dd"
    public Integer SupplierCustomerId = 0;  // comsupplier
    public String  ManualBillNo;            // txtbillno
    public String  RemarksHeader;           // txtremarks

    /** grd — the detail grid, in display order. */
    public List<Row> rows = new ArrayList<>();
    /** grdFreight — "Freight (Charge To Product)". */
    public List<FreightRow> freight = new ArrayList<>();
    /** grdGLedger — "Supplier Add/Less". */
    public List<JournalRow> journal = new ArrayList<>();

    /** One dtGrid row (Load:402). */
    public static class Row {
        public Integer Id = 0;
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
        public Integer UOMId = 0;
        public String  UOM;
        public Double  ItemQty = 0d;
        public Double  Rate = 0d;
        public Integer RateUOMId = 0;
        public String  RateUOM;
        public Double  ItemAmount = 0d;
        public Integer TaxNameId = 0;
        public String  TaxName;
        public Double  TaxPercent = 0d;
        public Double  TaxAmount = 0d;
        public Double  BillAmount = 0d;       // recomputed by the server (BillProportion:2360)
        public Double  Freights = 0d;         // recomputed by the server (FreightProportion:2326)
        public String  RemarksDetail;
        public String  GpNo;                  // untyped DataColumn on the desktop
        public String  VehicleNo;
        public Integer BranchId = 0;
        public String  BranchName;
    }

    /** One dtFreight row (Load:446). Transporter holds the value-list value (see the service). */
    public static class FreightRow {
        public String  Transporter;
        public Double  Freight = 0d;          // caption "Credit"
        public String  Remarks;
        public Integer GlAccountId = 0;
    }

    /** One dtGrdGL row (Load:435). AccountId holds the value-list value. */
    public static class JournalRow {
        public String  AccountId;
        public String  Remarks;
        public String  Percentage;
        public String  Qty;
        public String  Rate;
        public Double  Debit = 0d;
        public Double  Credit = 0d;
        public Integer GlAccountId = 0;
    }
}
