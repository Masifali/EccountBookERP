using System;
using System.Collections.Generic;

namespace Architecture.Model.FeedMill.PricingSchedule;

public class ItemPricingSchedule
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double ItemRate { get; set; }

	public int ApprovedUserId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PricingCustomGroupId { get; set; }

	public int RateUomId { get; set; }

	public virtual List<ItemPricingSchedule> ScheduleList { get; set; }
}
