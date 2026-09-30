package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory (model 0710) - the 36 non-virtual
 * properties SetProc sends to Sp_genEmployeeHistory_Insert (36 parameters, all declared).
 * Description / HeaderCoverLetterText / FooterCoverLetterText stay null (the form never sets them),
 * so they are omitted from the EXEC exactly as AddWithValue(null) omits them.
 */
public class GenEmployeeHistory extends DesktopModel {
    public int ActionTypeId;
    public boolean Active;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime FromDate;
    public LocalDateTime ProbationEnding;
    public LocalDateTime ToDate;
    public LocalDateTime WithEffectFrom;
    public BigDecimal TotalSalary = BigDecimal.ZERO;
    public int PayableAcId;
    public int ExpenseAccountId;
    public int LoanAcId;
    public int DepartmentId;
    public int EmployeeGroupId;
    public int DesignationId;
    public int EmployeeHistoryLineId;
    public int JobId;
    public int SectionId;
    public int ShiftId;
    public int StoreId;
    public int AlteredById;
    public int BranchId;
    public int CreatedById;
    public int EmployeeHistoryId;
    public int EmployeeId;
    public int LocationId;
    public int PartyLocationId;
    public int ReportToEmployeeId;
    public int UserLogId;
    public String Description;
    public String FooterCoverLetterText;
    public String HeaderCoverLetterText;
    public int OrganizationId;
    public int CompanyId;
    public int ProjectId;
    public int EmployeeTypeProfileId;
}
