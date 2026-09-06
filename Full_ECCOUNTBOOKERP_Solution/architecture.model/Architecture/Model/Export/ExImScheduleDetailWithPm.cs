using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImScheduleDetailWithPm
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double BrandInnerQty { get; set; }

	public double BrandOuterQty { get; set; }

	public double PmInnerQty { get; set; }

	public double PmOuterQty { get; set; }

	public int ActionTypeId { get; set; }

	public int BrandId { get; set; }

	public int BrandPackingTypeId { get; set; }

	public int BrandUomId { get; set; }

	public int ContractId { get; set; }

	public int ContractScheduleId { get; set; }

	public int Id { get; set; }

	public int PMItemId { get; set; }

	public int PmItemUomId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public string Remarks { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandCode { get; set; }

	public virtual string BrandUomCode { get; set; }

	public virtual string BrandUomEquivalent { get; set; }

	public virtual string BrandQtyEquivalent { get; set; }

	public virtual string BrandPackingType { get; set; }

	public virtual string PmItemName { get; set; }

	public virtual string PmItemCode { get; set; }

	public virtual string PmUomCode { get; set; }

	public virtual string PmUomEquivalent { get; set; }

	public virtual List<ExImScheduleDetailWithPm> ExImScheduleDetailWithPmList { get; set; }
}
