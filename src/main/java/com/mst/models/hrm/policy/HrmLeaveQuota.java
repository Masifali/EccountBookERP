package com.mst.models.hrm.policy;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.List;

/**
 * Architecture.Model.HRM.PolicyManagment.hrmLeaveQuota (model 0675) -> sp_hrmLeaveQuota_Insert / Sp_hrmLeaveQuota_Update (14 params).
 */
public class HrmLeaveQuota extends DesktopModel {
    public boolean IsActive;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime FromDate;
    public LocalDateTime ToDate;
    public int ActionTypeId;
    public int AlteredById;
    public int CompanyId;
    public int CreatedById;
    public int LeaveQuotaId;
    public int LocationId;
    public int OrganizationId;
    public long UserLogId;
    public String Description;
    public transient List<HrmLeaveQuotaDetail> HrmLeaveQuotaDetailList;
}
