package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit (model 0719) - 14 properties, as SetProc
 * sends them to Sp_hrmEmployeeBenefit_Insert (14 parameters, all declared).
 */
public class HrmEmployeeBenefit extends DesktopModel {
    public int EmployeeBenefitId;
    public int EmployeeId;
    public int EmployeeHistoryId;
    public int BenefitId;
    public int ApprovalEmployeeId;
    public String Description;
    public LocalDateTime CreatedOn;
    public int CreatedById;
    public int AlteredById;
    public LocalDateTime AlteredOn;
    public int ActionTypeId;
    public int UserLogId;
    public int OrganizationId;
    public int CompanyId;
}
