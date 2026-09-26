package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * What the Store Issuance pages post (screens 322 frmGSIssuance and 321 StoreIssuanceDirect).
 *
 * Only what the operator can actually see or type travels from the browser. Organization,
 * company, branch, financial year, document type, users, dates of entry, approval and every
 * other server-owned column are filled by the service from the signed-in user, whatever the body
 * says, exactly where the desktop's Insert() fills them from UserAccount / clsGlobalVariables.
 *
 * Field names follow the desktop grid's column keys (dtgrddetail on 322, "table" on 321), so a
 * reader can hold this class next to the form and see the same names.
 */
public class StoreIssuanceDto {

    /** RECID — 0 for a new document, the header id when updating. */
    public Integer Id = 0;
    public Integer DocNo = 0;              // txtDocNo (ReadOnly on the desktop; the server never trusts it - D7)
    public Integer BranchSrNo = 0;         // txtBranchSrNo
    public String  DocDate;                // txtDocdate, "yyyy-MM-dd"
    public String  Remarks;                // txtRemarks
    public String  ManualNo;               // txtManualNo (321 only)

    /** The live grid, in display order. */
    public List<Row> rows = new ArrayList<>();

    /** lstRemoveRecord — saved rows the operator deleted with "X" (ActionTypeId 3). */
    public List<Row> removed = new ArrayList<>();

    /**
     * One grid row. 322 uses the loader columns; 321 uses the entry-bar columns. A field a screen
     * does not have simply stays at its default, which is what the desktop's Conversion.ToInt /
     * ToDouble of a missing cell yields too.
     */
    public static class Row {
        public Integer Id = 0;

        // ---- 322 linkage (from the two loaders)
        public Integer DepartmentRequestId = 0;
        public Integer DepartmentRequestDetailId = 0;
        public Integer DepartmentRequestNo = 0;
        public Integer DoDocumentTypeId = 0;
        public Integer DoId = 0;
        public Integer DoDetailId = 0;
        public Integer DoNo = 0;
        public Integer SupplierCustomerId = 0;
        public String  SupplierName;
        public Integer BagTypeId = 0;
        public Double  Equivalent = 0d;
        public Double  RowBalanceQty = 0d;
        public Double  BalanceStock = 0d;

        // ---- shared
        public Integer ItemId = 0;
        public String  ItemCode;
        public String  ItemName;
        public Integer WarehouseId = 0;        // 322 "LocationId", 321 "WareHouseId"
        public String  WarehouseName;
        public Integer RackId = 0;
        public String  RackName;
        public Integer ItemConditionId = 0;
        public String  ItemCondition;
        public Integer UnitId = 0;             // 322 "UnitId", 321 "PackUomId"
        public String  Unit;
        public Double  IssueQty = 0d;
        public Double  ItemRate = 0d;
        public Integer DepartmentId = 0;
        public Integer AssetId = 0;
        public String  Remarks;

        // ---- 321 only
        public Integer IssuanceTypeId = 0;     // CmbCategory1 (InvLookup type 20)
        public Integer ContractScheduleId = 0; // CmbInvoiceNo
        public String  ContractScheduleNo;
        public Integer DrAcId = 0;             // cmbDrAc
    }
}
