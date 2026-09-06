namespace Architecture.Model.ModernHistoryModels;

public class Puchase_Sale_InvoiceHistoryDetail
{
	public int InvoiceId { get; set; }

	public int GatePassNumber { get; set; }

	public int Gdn_Grn_Number { get; set; }

	public int GatePassId { get; set; }

	public int Gdn_Grn_Id { get; set; }

	public int PurchaseGLAC { get; set; }

	public int SaleGLAC { get; set; }

	public int WarehouseId { get; set; }

	public string WarehouseName { get; set; }

	public int Job_LotId { get; set; }

	public string JobLotName { get; set; }

	public string CropYear { get; set; }

	public int ItemId { get; set; }

	public string ItemName { get; set; }

	public string PackingType { get; set; }

	public double ItemQty { get; set; }

	public string PackUom { get; set; }

	public double GrossWeight { get; set; }

	public double BagWeight { get; set; }

	public double WeightCut { get; set; }

	public double NetWeight { get; set; }

	public double StockWeight { get; set; }

	public double Rate { get; set; }

	public string RateUom { get; set; }

	public double ItemAmount { get; set; }

	public double RateCutAmount { get; set; }

	public double FreightAmount { get; set; }

	public double ExpenseAmount { get; set; }

	public double BillAmount { get; set; }

	public bool IsDetailExpanded { get; set; }
}
