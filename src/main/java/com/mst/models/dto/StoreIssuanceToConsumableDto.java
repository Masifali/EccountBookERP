package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * What the Store Issuance To Consumable Store page (screen 347, frmStoreIssuanceToCosumableStore,
 * DocumentTypeId 1616) posts to /api/store/issuance-to-consumable-store/{precheck,save}.
 *
 * Public fields, named exactly as the JS sends them (Jackson maps public fields by their own name,
 * so "Id" stays "Id"). Organization, company, user, financial year, document type, dates of entry
 * and the Doc No are taken on the server from the session / generator / stored header, never from
 * this body.
 */
public class StoreIssuanceToConsumableDto {

    /** RECID — 0 for Save, the opened header id for Update. */
    public Integer Id = 0;
    public String  DocDate;               // txtDocdate "yyyy-MM-dd"
    public String  Remarks;               // txtRemarks
    public Integer BranchesId = 0;        // cmbBranch.Value
    public String  BranchText;            // cmbBranch.Text (FormValidation tests the TEXT, :607)
    public Integer ProjectsId = 0;        // cmbProject.Value
    public String  ProjectText;           // cmbProject.Text (:613)

    /** grdDetail rows in display order (dtgrddetail). */
    public List<Row> rows = new ArrayList<>();

    /** lstRemoveRecord — every row removed with "X" since the page was opened (never cleared, D4). */
    public List<Row> removed = new ArrayList<>();

    /** dtgrddetail columns (frmStoreIssuanceToCosumableStore.cs:198-217). */
    public static class Row {
        public Integer Id = 0;
        public Integer DepartmentRequestId = 0;
        public Integer DepartmentRequestDetailId = 0;
        public Integer DepartmentRequestNo = 0;
        public Integer LocationId = 0;
        public Integer DepartmentFromId = 0;
        public String  DepartmentFrom;
        public Integer DepartmentToId = 0;
        public String  DepartmentTo;
        public Integer WorkOrderId = 0;
        public String  WorkOrder;
        public Integer ItemId = 0;
        public String  Item;
        public String  ItemCode;
        public Integer UnitId = 0;
        public String  Unit;
        public Integer AssetId = 0;
        public String  Asset;
        public Double  IssueQty;
        public String  Remarks;
    }
}
