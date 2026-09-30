package com.mst.models.hrm.leave;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.LeaveManagement.EmployeeLeaveQuota (model 0699) - the 14 properties
 * [hrm].[Sp_EmployeeLeaveQuota_Insert] declares (checked: same 14). Avail / CPLQuota /
 * EmployeeLeaveQuotaId are never set by the opening form, so 0 is sent as the desktop sends it.
 */
public class EmployeeLeaveQuota extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public BigDecimal Assigned = BigDecimal.ZERO;
    public BigDecimal Avail = BigDecimal.ZERO;
    public BigDecimal Availed = BigDecimal.ZERO;
    public BigDecimal CPLQuota = BigDecimal.ZERO;
    public int LeaveQuotaDetailId;
    public long AlteredById;
    public long CreatedById;
    public long EmployeeId;
    public long EmployeeLeaveOpeningId;
    public long EmployeeLeaveQuotaId;
    public long UserLogId;
}
