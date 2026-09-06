using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class ItemPricingSchedule
{
	public int Id { get; set; }

	public string ScheduleDesc { get; set; }

	public int ItemId { get; set; }

	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ChangedPriceLookUpId { get; set; }

	public int PriceTypeId { get; set; }

	public double ItemPrice { get; set; }

	public double PackingRate { get; set; }

	public double Margin { get; set; }

	public int PackingUom { get; set; }

	public int RateUomId { get; set; }

	public int CropYearId { get; set; }

	public bool IsActive { get; set; }

	public virtual string ItemIds { get; set; }

	public virtual string ItemName { get; set; }

	public virtual int FormulaForPriceTypeId { get; set; }

	public virtual int FormulaTypeId { get; set; }

	public virtual double Value { get; set; }

	public virtual double ItemRetailPrice { get; set; }

	public virtual double ItemWholeSalePrice { get; set; }

	public virtual double ItemPurchasePrice { get; set; }

	public virtual double ItemCostPrice { get; set; }

	public virtual double ItemFactoryPrice { get; set; }

	public virtual List<ItemPricingSchedule> ItemPricingScheduleslist { get; set; }

	public virtual List<SalesPricingScheduleBreakup> SalesPricingScheduleBreakupsList { get; set; }
}
