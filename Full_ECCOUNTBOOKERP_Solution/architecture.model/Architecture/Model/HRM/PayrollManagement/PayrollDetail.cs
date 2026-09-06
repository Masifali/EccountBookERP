using System;

namespace Architecture.Model.HRM.PayrollManagement;

public class PayrollDetail
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal AdvanceAmount { get; set; }

	public decimal ArrearAmount { get; set; }

	public decimal EmployeePayrollSalary { get; set; }

	public decimal EmployeePerDaySalary { get; set; }

	public decimal EOBIAmount { get; set; }

	public decimal FoodAmount { get; set; }

	public decimal FuelAmount { get; set; }

	public decimal GrossSalary { get; set; }

	public decimal IncomeTaxAmount { get; set; }

	public decimal LateDeductionAmount { get; set; }

	public decimal LeaveAmount { get; set; }

	public decimal LoanAmount { get; set; }

	public decimal MedicalAmount { get; set; }

	public decimal MiscAddAmount { get; set; }

	public decimal MiscLessAmount { get; set; }

	public decimal MobileAmount { get; set; }

	public decimal NetSalary { get; set; }

	public decimal OTAmount { get; set; }

	public decimal PFAmount { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal TravellingAmount { get; set; }

	public decimal AddLessAmount { get; set; }

	public decimal SalaryWithoutAddLess { get; set; }

	public int EmployeeDays { get; set; }

	public int LoanNOS { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeHistoryId { get; set; }

	public long EmployeeId { get; set; }

	public long InwardId { get; set; }

	public long PayrollDetailId { get; set; }

	public long PayrollId { get; set; }

	public long UserLogId { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual int EmployeeNo { get; set; }

	public virtual string AccountCode { get; set; }

	public virtual int AccountId { get; set; }
}
