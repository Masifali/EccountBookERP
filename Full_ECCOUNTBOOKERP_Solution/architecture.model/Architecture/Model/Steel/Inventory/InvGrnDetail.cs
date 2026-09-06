namespace Architecture.Model.Steel.Inventory;

public class InvGrnDetail
{
	public decimal AdLsWeight { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal ItemQty { get; set; }

	public decimal NetBillWeight { get; set; }

	public decimal StockWeight { get; set; }

	public decimal WtCut { get; set; }

	public decimal WtCutTotal { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int ContractorId { get; set; }

	public int GatePassInwarDetailId { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int LabPurchaseAnalysisHeaderId { get; set; }

	public int PackingTypeId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PurchaserOrderNo { get; set; }

	public int WarehouseId { get; set; }

	public string LabReportRef { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual double PackUom { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CityName { get; set; }

	public virtual string LabPurchaseDocNo { get; set; }
}
