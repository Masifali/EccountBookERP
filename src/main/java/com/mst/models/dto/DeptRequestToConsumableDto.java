package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 349 "Department Request To Consumable Store" (DepartmentRequestToConsumableStore.cs,
 * DocumentTypeId 1615) — what the page posts to /api/store/department-request-to-consumable/save.
 *
 * Field names are the desktop's (the DepartmentRequest model and the dtdetail columns built in
 * EccountBook_Load, Form.cs:264). Public fields so Jackson binds exactly the casing the JS sends.
 * Organization, company, financial year, users, Doc No and entry dates are never taken from here —
 * the service fills them from the session and the stored header.
 */
public class DeptRequestToConsumableDto {

    public Integer Id = 0;                  // RecId
    public String  DocDate;                 // "yyyy-MM-dd" (DocDate picker)
    public Integer ProjectId = 0;           // cmbProject
    public Integer WorkOrderId = 0;         // cmbworkno — written on every detail row (Form.cs:761)
    public Integer FromDepartmentId = 0;    // cmbDepartmentFrom
    public Integer ToDepartmentId = 0;      // cmbDepartmentTo
    public String  RemarksHeader;           // txtremarks

    /** grdDetail rows, in grid order. */
    public List<Row> rows = new ArrayList<>();

    /** lstRemoveRecord — stored rows deleted from the grid on an opened document (ActionTypeId 3). */
    public List<Row> removed = new ArrayList<>();

    public static class Row {
        public Integer Id = 0;
        public Integer ItemId = 0;
        public Integer ItemUOMId = 0;
        public Integer AssetId = 0;
        public Double  RequestedQty = 0d;
        /** Only for removed rows: cmbworkno.Value at the moment the row was deleted (Form.cs:1186). */
        public Integer WorkOrderId = 0;
    }
}
