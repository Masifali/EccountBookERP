using System;

namespace Architecture.Model.Mfg;

public class ProductionInputDetail
{
	public DateTime timeOut { get; set; }

	public DateTime timeStart { get; set; }

	public decimal BomQty { get; set; }

	public decimal Qty { get; set; }

	public decimal Weight { get; set; }

	public int ActionTypeId { get; set; }

	public int BomHeaderId { get; set; }

	public int BomInputDetailId { get; set; }

	public int HeatCode { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int LineId { get; set; }

	public int ProductionHeaderId { get; set; }

	public int productionInputDetailId { get; set; }

	public int SortNo { get; set; }

	public string RemarksDetail { get; set; }

	public decimal Amount { get; set; }

	public decimal ItemRate { get; set; }

	public int RateUomId { get; set; }

	public int WareHouseId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackUom { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual double RateEquivalent { get; set; }

	public virtual string BomNo { get; set; }
}
