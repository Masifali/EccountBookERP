package com.mst.models.hrm.leave;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Architecture.Model.HRM.LeaveManagement.EmployeeLeaveOpening (model 0698) - every non-virtual property,
 * as GenericProvider.SetProc sends it to [hrm].[Sp_EmployeeLeaveOpening_Insert] / _Update
 * (11 parameters each, checked against the proc; nothing missing, nothing extra).
 * UserLogId is never set by frmEmployeeLeaveOpening.Insert(), so 0 is sent (CLR default).
 */
public class EmployeeLeaveOpening extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int LeaveQuotaId;
    public int LeaveTypeProfileId;
    public long AlteredById;
    public long CreatedById;
    public long EmployeeLeaveOpeningId;
    public long UserLogId;
    public long OrganizationId;
    public long CompanyId;

    /** virtual List&lt;EmployeeLeaveQuota&gt; - not a parameter. */
    public transient List<EmployeeLeaveQuota> EmployeeLeaveQuotasList = new ArrayList<>();
}
