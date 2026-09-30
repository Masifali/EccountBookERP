package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.EmployeeSalary (model 0703) - 14 properties, as SetProc
 * sends them to hrm.Sp_EmployeeSalary_Insert (14 parameters, all declared).
 */
public class EmployeeSalary extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public BigDecimal Amount = BigDecimal.ZERO;
    public BigDecimal SalaryTypePercent = BigDecimal.ZERO;
    public int SalaryTypeProfileId;
    public long AlteredById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeHistoryId;
    public long EmployeeSalaryId;
    public long OrganizationId;
    public long UserLogId;
    public long EmployeeId;
}
