using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class ItemDiscounting
{
	public DateTime EntryDate { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime FromTime { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public DateTime StatusChangedDate { get; set; }

	public DateTime ToDate { get; set; }

	public DateTime ToTime { get; set; }

	public double DiscountValue { get; set; }

	public double RangeFrom { get; set; }

	public double RangeTo { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DiscountDocNo { get; set; }

	public int MasterDiscountTypeId { get; set; }

	public int drmLookUpId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int XGroupId { get; set; }

	public int YGroupId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int PriceTypeId { get; set; }

	public int WarehouseId { get; set; }

	public int TargetTypeId { get; set; }

	public string DiscountDescription { get; set; }

	public string DiscountName { get; set; }

	public string DiscountStatus { get; set; }

	public virtual List<ItemDiscounting> ItemDiscountingScheduleslist { get; set; }
}
