namespace Architecture.Model.HRM.PolicyManagment;

public class hrmLeaveQuotaDetail
{
	public int LeaveQuotaDetailId { get; set; }

	public int LeaveQuotaId { get; set; }

	public int LeaveTypeProfileId { get; set; }

	public int QuotaValue { get; set; }

	public virtual string ProfileTypeName { get; set; }

	public virtual string ProfileName { get; set; }
}
