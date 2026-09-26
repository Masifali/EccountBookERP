package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 338 "Department Request" (frmDepartmentRequest.cs, DocumentTypeId 450) — what the page posts.
 *
 * Field names are the desktop's (dtdetail columns, InitializeComponentCustom:282, and the
 * DepartmentRequestDetail model). Public fields so Jackson binds the exact casing the JS sends.
 * Organization, company, branch, financial year, users, Doc No and entry dates are never taken from
 * here — the service fills them from the session and the stored header.
 */
public class DepartmentRequestDto {

    public Integer Id = 0;                  // RecId
    public String  DocDate;                 // "yyyy-MM-dd" (DocDate picker)
    public Integer FromDepartmentId = 0;    // cmbDepartmentFrom
    public Integer ToDepartmentId = 0;      // cmbDepartmentTo
    public String  RemarksHeader;           // txtremarks

    /** grdDetail rows, in grid order. */
    public List<Row> rows = new ArrayList<>();

    /** lstRemoveRecord — stored rows deleted from the grid on an opened document. */
    public List<Row> removed = new ArrayList<>();

    public static class Row {
        public Integer Id = 0;
        public Integer ItemId = 0;
        public Integer ItemConditionId = 0;
        public Integer ItemUOMId = 0;
        public Integer AssetId = 0;
        public Double  RequestedQty = 0d;
    }
}
