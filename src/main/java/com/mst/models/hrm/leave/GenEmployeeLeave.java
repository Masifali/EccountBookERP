package com.mst.models.hrm.leave;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Architecture.Model.HRM.LeaveManagement.genEmployeeLeave (model 0700) - the 21 non-virtual properties
 * Sp_genEmployeeLeave_Insert / Sp_genEmployeeLeave_Update declare (checked: same 21).
 * LocationName / EmployeeName / LeaveType and the two child lists are virtual - not parameters.
 * Remarks is left null by frmEmployeeCPLRequest (AddWithValue(null) sends nothing).
 */
public class GenEmployeeLeave extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime FromDate;
    public LocalDateTime ToDate;
    public BigDecimal NoOfDays = BigDecimal.ZERO;
    public int DocMovementId;
    public int LeaveTypeProfileId;
    public long AlteredById;
    public long ApprovedById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeId;
    public long EmployeeLeaveId;
    public long EmployeeLeaveQuotaId;
    public long LocationId;
    public long OrganizationId;
    public long UserLogId;
    public String Reason;
    public String Remarks;

    public transient List<GenEmployeeLeaveDetail> genEmployeeLeaveDetailList = new ArrayList<>();
    public transient List<CPLEmployeeLeaveDetail> CPLEmployeeLeaveDetailslist = new ArrayList<>();
}
