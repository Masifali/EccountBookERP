package com.mst.models.hrm.dto;

/** One dthead row of frmEmployeeAllowance (Id, EmployeeId, EmployeeBenefitId, AppliedOn, TotalAmount, IsForPayroll, Remarks). */
public class PayrollAllowanceRowDto {
    public Object id;
    public Object employeeId;
    public Object employeeBenefitId;
    public String appliedOn;
    public Object totalAmount;
    public Object isForPayroll;
    public String remarks;
}
