namespace Architecture.Model.HRM.PolicyManagment;

public class hrmPolicyDetail
{
	public int AdvanceAfterDay { get; set; }

	public bool AllowMultiShiftToEmployeeOnDay { get; set; }

	public int CPLApplyMinHours { get; set; }

	public bool FoodEqually { get; set; }

	public bool FoodOnSalary { get; set; }

	public bool IsMonthEndPayroll { get; set; }

	public bool IsSeparteOTPayroll { get; set; }

	public bool LateArrivalWithGraceTime { get; set; }

	public int LateNoOfDay { get; set; }

	public int NoOfHalfDay { get; set; }

	public int NoOfShortHoursInDay { get; set; }

	public int NoOfShortLeave { get; set; }

	public int NoOfShortLeaveInMonth { get; set; }

	public int OTApplyAfterMin { get; set; }

	public int PayrollCutOffDay { get; set; }

	public int ShortNoOfHours { get; set; }

	public decimal AdvanceLimitPercent { get; set; }

	public decimal FoodShareAmount { get; set; }

	public decimal FoodSharePercent { get; set; }

	public decimal OTFactorRate { get; set; }

	public int HeaderId { get; set; }

	public int PolicyId { get; set; }

	public int EmployeeDutyHour { get; set; }

	public string Description { get; set; }
}
