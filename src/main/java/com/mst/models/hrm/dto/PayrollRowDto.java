package com.mst.models.hrm.dto;

/**
 * One grdDetails row of frmPayrollPosting (dtdetail's columns, camelCase). Values stay as the grid
 * holds them (Object) so the service applies the desktop's Conversion.ToInt / ToDecimal to each.
 */
public class PayrollRowDto {
    public Object id;
    public Object employeeId;
    public Object employeeHistoryId;
    public Object grossSalary;
    public Object employeeDays;
    public Object employeePerDaySalary;
    public Object employeePayrollSalary;
    public Object lateDeductionAmount;
    public Object incomeTaxAmount;
    public Object leaveAmount;
    public Object advanceAmount;
    public Object loanAmount;
    public Object loanNos;
    public Object pfAmount;
    public Object eobiAmount;
    public Object miscLessAmount;
    public Object totalLessAmount;
    public Object otAmount;
    public Object travellingAmount;
    public Object medicalAmount;
    public Object mobileAmount;
    public Object fuelAmount;
    public Object foodAmount;
    public Object miscAddAmount;
    public Object arearAmount;
    public Object salary;
    public Object addLessAmount;
    public Object netSalary;
}
