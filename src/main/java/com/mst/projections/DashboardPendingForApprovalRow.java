package com.mst.projections;

import java.sql.Timestamp;

/**
 * Row of Sp_Inventory_ReadDashboardPendingForApproval (single branch).
 * Columns A.* come from the big UNION ALL of per-document-type counts; the rest come from
 * the procedure parameters echoed back and from InventoryApprovedParams R (LEFT JOIN on TypeId).
 */
public interface DashboardPendingForApprovalRow {
    Integer getTypeID();
    String getDescription();
    Long getCount();
    Integer getActionId();
    Integer getSortNo();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getBranchesId();
    Integer getFinancialYearId();
    Integer getEntryUser();
    Timestamp getPostDate();
    String getActivity();
    Integer getRecordId();
    Integer getDocumentTypeId();
    String getReqType();
    String getApprovedFilter();
    String getCompanyName();
    String getCompanyAddress();
    String getReportName();
    String getVoucherReportName();
    String getApiEndPoint();
    String getApprovedEndPoint();
    String getPrintEndPoint();
    String getPrintEndPointVoucher();
    String getComboBindApiEndPoint();
}
