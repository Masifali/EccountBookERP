package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 340 "Purchase Demand" (frmPurchaseDemand.cs, DocumentTypeId 141) — what the page posts.
 *
 * Field names are the desktop grid's DataTable columns (dtgrddetail, Form_Load:323-346), public
 * fields so Jackson binds them by their exact spelling. Everything the server owns — organization,
 * company, branch, financial year, users, entry/approval dates, the document type, the parent
 * category and the Status — is filled by the service from the signed-in user, never from here.
 */
public class PurchaseDemandDto {

    public Integer Id = 0;                 // RECID
    public Integer DocNo = 0;              // txtdocnumber (ReadOnly; never trusted — see service D2)
    public String  DocDate;                // txtDocdate, "yyyy-MM-dd"
    public String  RemarksHeader;          // txtRemarks
    public String  DetailIdsToDelete;      // DetailIdsToDelete, "12,15" or ""

    public List<Row> rows = new ArrayList<>();

    /** One grid row — dtgrddetail. */
    public static class Row {
        public Integer Id = 0;
        public String  Department;
        public Integer DepartmentId = 0;
        public String  RequistionBy;       // sic, the desktop column name
        public Integer ItemId = 0;
        public String  ItemCode;
        public String  ItemName;
        public String  ItemUOM;
        public Integer ItemUOMId = 0;
        public Integer ItemConditionId = 0;
        public String  ItemCondition;
        public Double  RequiredQty = 0d;
        public Double  ApprovedQty = 0d;
        public Boolean ApprovalStatus = false;
        public String  JobLot;
        public Integer JobLotId = 0;
        public String  AssetsRef;
        public Integer AssetsRefId = 0;
        public String  Remarks;
        public String  LastDate;           // "dd-MMM-yyyy" or ""
        public Double  LastQty = 0d;
        public Double  LastRate = 0d;
        public Double  OutstandingDemandAndPOQty = 0d;
        public Double  AvaialableStockQty = 0d;
    }
}
