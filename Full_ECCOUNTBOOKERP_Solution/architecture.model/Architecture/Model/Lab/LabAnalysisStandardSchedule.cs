using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class LabAnalysisStandardSchedule
{
	public int LabAnalysisStandardScheduleId { get; set; }

	public int LabAnalysisGroupId { get; set; }

	public int LabAnalysisItemId { get; set; }

	public decimal MinValue { get; set; }

	public decimal MaxValue { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public bool Active { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public int? ModifyUserId { get; set; }

	public DateTime? ModifyDate { get; set; }

	public DateTime EffectiveDateFrom { get; set; }

	public DateTime EffectiveDateTo { get; set; }

	public int ActionId { get; set; }

	public string LabAnalysisStandardScheduleName { get; set; }

	public virtual string LabAnalysisItemName { get; set; }

	public virtual string LabAnalysisItemCode { get; set; }

	public virtual string LabAnalysisGroupDescription { get; set; }

	public virtual string LabAnalysisGroupCode { get; set; }

	public virtual List<LabAnalysisStandardSchedule> LabAnalysisStandardScheduleList { get; set; }

	public LabAnalysisStandardSchedule()
	{
		LabAnalysisStandardScheduleList = new List<LabAnalysisStandardSchedule>();
	}
}
