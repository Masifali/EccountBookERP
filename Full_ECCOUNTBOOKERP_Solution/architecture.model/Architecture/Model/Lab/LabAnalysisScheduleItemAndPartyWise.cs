using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class LabAnalysisScheduleItemAndPartyWise
{
	public int LabAnalysisScheduleItemAndPartyWiseId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int BrandItemId { get; set; }

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

	public int MainId { get; set; }

	public int ActionId { get; set; }

	public string ScheduleName { get; set; }

	public virtual string PartyName { get; set; }

	public virtual string BrandItemName { get; set; }

	public virtual string BrandItemCode { get; set; }

	public virtual string LabAnalysisItemName { get; set; }

	public virtual string LabAnalysisItemCode { get; set; }

	public virtual string LabAnalysisGroupDescription { get; set; }

	public virtual string LabAnalysisGroupCode { get; set; }

	public virtual List<LabAnalysisScheduleItemAndPartyWise> LabAnalysisScheduleItemAndPartyWiseList { get; set; }

	public LabAnalysisScheduleItemAndPartyWise()
	{
		LabAnalysisScheduleItemAndPartyWiseList = new List<LabAnalysisScheduleItemAndPartyWise>();
	}
}
