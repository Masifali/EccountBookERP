using System;

namespace Architecture.Model.Inventory;

public class FIFOStockEvaluation
{
	public int Id { get; set; }

	public DateTime DocDate { get; set; }

	public int ItemId { get; set; }

	public int WarehouseId { get; set; }

	public int ItemUomId { get; set; }

	public int RateUomId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public double QtyIn { get; set; }

	public double QtyOut { get; set; }

	public double BalQty { get; set; }

	public double WeightIn { get; set; }

	public double WeightOut { get; set; }

	public double BalWeight { get; set; }

	public double AmountIn { get; set; }

	public double AmountOut { get; set; }

	public double BalAmount { get; set; }

	public double AvgRate { get; set; }

	public double ReserveWeight { get; set; }

	public double ReserveQty { get; set; }

	public double NetBalQty { get; set; }

	public double NetBalWeight { get; set; }
}
