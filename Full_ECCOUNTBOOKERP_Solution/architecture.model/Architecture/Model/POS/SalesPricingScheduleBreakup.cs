using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class SalesPricingScheduleBreakup
{
	public bool RateChangeable { get; set; }

	public DateTime BrandEffectedDate { get; set; }

	public DateTime SemiEffectedDate { get; set; }

	public decimal BrandRate { get; set; }

	public decimal Expenses { get; set; }

	public decimal MarginPercent { get; set; }

	public decimal PackingRate { get; set; }

	public decimal Percent { get; set; }

	public decimal PercentRate { get; set; }

	public decimal RateUom { get; set; }

	public decimal SemiRate { get; set; }

	public int BrandId { get; set; }

	public int CompanyId { get; set; }

	public int DiscountTypeId { get; set; }

	public int Id { get; set; }

	public int CropYearId { get; set; }

	public int BrandCropYearId { get; set; }

	public int ItemId { get; set; }

	public int OrganizationId { get; set; }

	public string ScheduleDescription { get; set; }

	public virtual List<SalesPricingScheduleBreakup> SalesPricingScheduleBreakupsList { get; set; }
}
