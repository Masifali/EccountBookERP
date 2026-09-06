using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class ItemPricingScheduleForRice
{
	public DateTime EffectiveDate { get; set; }

	public virtual string ItemName { get; set; }

	public double ItemRate { get; set; }

	public virtual string RateUom { get; set; }

	public DateTime ApprovedDate { get; set; }

	public bool IsApproved { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int CropYearId { get; set; }

	public int ModifyUserId { get; set; }

	public int DocumentTypeId { get; set; }

	public int OrganizationId { get; set; }

	public int RateUomId { get; set; }

	public bool IsActive { get; set; }

	public virtual List<ItemPricingScheduleForRice> ItemPricingScheduleForRicelist { get; set; }
}
