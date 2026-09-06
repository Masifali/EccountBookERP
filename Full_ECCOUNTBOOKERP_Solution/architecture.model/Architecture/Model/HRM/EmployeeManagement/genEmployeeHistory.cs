using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genEmployeeHistory
{
	public int ActionTypeId { get; set; }

	public bool Active { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ProbationEnding { get; set; }

	public DateTime ToDate { get; set; }

	public DateTime WithEffectFrom { get; set; }

	public decimal TotalSalary { get; set; }

	public int PayableAcId { get; set; }

	public int ExpenseAccountId { get; set; }

	public int LoanAcId { get; set; }

	public int DepartmentId { get; set; }

	public int EmployeeGroupId { get; set; }

	public int DesignationId { get; set; }

	public int EmployeeHistoryLineId { get; set; }

	public int JobId { get; set; }

	public int SectionId { get; set; }

	public int ShiftId { get; set; }

	public int StoreId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CreatedById { get; set; }

	public int EmployeeHistoryId { get; set; }

	public int EmployeeId { get; set; }

	public int LocationId { get; set; }

	public int PartyLocationId { get; set; }

	public int ReportToEmployeeId { get; set; }

	public int UserLogId { get; set; }

	public string Description { get; set; }

	public string FooterCoverLetterText { get; set; }

	public string HeaderCoverLetterText { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int ProjectId { get; set; }

	public int EmployeeTypeProfileId { get; set; }

	public virtual string Location { get; set; }

	public virtual string ReportedBranch { get; set; }

	public virtual string Section { get; set; }

	public virtual string Store { get; set; }

	public virtual string Designation { get; set; }

	public virtual string ShiftName { get; set; }

	public virtual string ReportedPerson { get; set; }

	public virtual string DepartmentName { get; set; }

	public virtual string FistName { get; set; }

	public virtual string MiddleName { get; set; }

	public virtual string LastName { get; set; }

	public virtual string EmployeeType { get; set; }
}
