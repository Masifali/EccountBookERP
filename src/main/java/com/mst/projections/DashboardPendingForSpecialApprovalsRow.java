package com.mst.projections;

/**
 * Row of usp_ReadDashboardPendingForSpecialApprovals (single branch).
 * ReasonForSpecialApproval comes from SpecialApprovalReason SR (LEFT JOIN on ReasonId = SR.Id).
 */
public interface DashboardPendingForSpecialApprovalsRow {
    Integer getTypeID();
    String getDescription();
    Long getCount();
    Integer getActionId();
    Integer getReasonId();
    String getReasonForSpecialApproval();
}
