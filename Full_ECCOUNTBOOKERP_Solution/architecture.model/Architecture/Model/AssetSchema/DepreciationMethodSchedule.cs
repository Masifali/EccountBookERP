using System;

namespace Architecture.Model.AssetSchema;

public class DepreciationMethodSchedule
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApproveDate { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime effectiveDateFrom { get; set; }

	public DateTime effectiveDateTo { get; set; }

	public DateTime LastModifyDate { get; set; }

	public int ApproveBy { get; set; }

	public int companyId { get; set; }

	public int CreatedBy { get; set; }

	public int depreciationMethodScheduleId { get; set; }

	public int LastModifyBy { get; set; }

	public int organizationId { get; set; }

	public int sortNo { get; set; }

	public string depreciatonMethodName { get; set; }
}
