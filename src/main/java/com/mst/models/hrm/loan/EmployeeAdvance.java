package com.mst.models.hrm.loan;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.LoanManagement.EmployeeAdvance (model 0695) - every property, as
 * GenericProvider.SetProc sends it to hrm.Sp_EmployeeAdvance_Insert / hrm.Sp_EmployeeAdvance_Update
 * (both declare all 26; @ApprovalRemarks, which the model lacks, is left to the procedure's NULL default
 * exactly as the desktop leaves it).
 */
public class EmployeeAdvance extends DesktopModel {
    public int ActionTypeId;
    public boolean IsApproved;
    public LocalDateTime AlteredOn;
    public LocalDateTime AppliedOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime DocDate;
    public BigDecimal AdvanceAmount = BigDecimal.ZERO;
    public BigDecimal ApprovedAmount = BigDecimal.ZERO;
    public BigDecimal PreviousApprovedAmount = BigDecimal.ZERO;
    public BigDecimal PreviousAdvanceAmount = BigDecimal.ZERO;
    public int DocNo;
    public int BranchId;
    public int CompanyId;
    public int EmployeeAdvanceId;
    public int OrganizationId;
    public int PayrollMonth;
    public int PayrollYear;
    public int ProjectId;
    public long AlteredById;
    public long ApprovedById;
    public long CreatedById;
    public long EmployeeId;
    public long LocationId;
    public long UserLogId;
    public String Reason;
}
