package com.mst.models.hrm.payroll;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.PayrollManagement.PayrollDetail (model 0691) - the 36 non-virtual
 * properties hrm.Sp_PayrollDetail_Insert declares (checked). The four virtual display properties
 * (EmployeeName, EmployeeNo, AccountCode, AccountId) are transient.
 */
public class PayrollDetail extends DesktopModel {
    public int ActionTypeId;               // byte
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public BigDecimal AdvanceAmount = BigDecimal.ZERO;
    public BigDecimal ArrearAmount = BigDecimal.ZERO;
    public BigDecimal EmployeePayrollSalary = BigDecimal.ZERO;
    public BigDecimal EmployeePerDaySalary = BigDecimal.ZERO;
    public BigDecimal EOBIAmount = BigDecimal.ZERO;
    public BigDecimal FoodAmount = BigDecimal.ZERO;
    public BigDecimal FuelAmount = BigDecimal.ZERO;
    public BigDecimal GrossSalary = BigDecimal.ZERO;
    public BigDecimal IncomeTaxAmount = BigDecimal.ZERO;
    public BigDecimal LateDeductionAmount = BigDecimal.ZERO;
    public BigDecimal LeaveAmount = BigDecimal.ZERO;
    public BigDecimal LoanAmount = BigDecimal.ZERO;
    public BigDecimal MedicalAmount = BigDecimal.ZERO;
    public BigDecimal MiscAddAmount = BigDecimal.ZERO;
    public BigDecimal MiscLessAmount = BigDecimal.ZERO;
    public BigDecimal MobileAmount = BigDecimal.ZERO;
    public BigDecimal NetSalary = BigDecimal.ZERO;
    public BigDecimal OTAmount = BigDecimal.ZERO;
    public BigDecimal PFAmount = BigDecimal.ZERO;
    public BigDecimal TotalAmount = BigDecimal.ZERO;
    public BigDecimal TravellingAmount = BigDecimal.ZERO;
    public BigDecimal AddLessAmount = BigDecimal.ZERO;
    public BigDecimal SalaryWithoutAddLess = BigDecimal.ZERO;
    public int EmployeeDays;
    public int LoanNOS;
    public long AlteredById;
    public long CreatedById;
    public long EmployeeHistoryId;
    public long EmployeeId;
    public long InwardId;
    public long PayrollDetailId;
    public long PayrollId;
    public long UserLogId;

    public transient String EmployeeName;
    public transient int EmployeeNo;
    public transient String AccountCode;
    public transient int AccountId;
}
