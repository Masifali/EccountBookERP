package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmLeaveQuotaDetail (model 0676) -> Sp_hrmLeaveQuotaDetail_Insert (4 params).
 * The virtual ProfileTypeName / ProfileName are read-only columns.
 */
public class HrmLeaveQuotaDetail extends DesktopModel {
    public int LeaveQuotaDetailId;
    public int LeaveQuotaId;
    public int LeaveTypeProfileId;
    public int QuotaValue;
    public transient String ProfileTypeName;
    public transient String ProfileName;
}
