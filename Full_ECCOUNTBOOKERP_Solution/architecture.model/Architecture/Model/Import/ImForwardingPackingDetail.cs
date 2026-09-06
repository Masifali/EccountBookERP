namespace Architecture.Model.Import;

public class ImForwardingPackingDetail
{
	public double AdLsWeight { get; set; }

	public double GrossWeight { get; set; }

	public double StockWeight { get; set; }

	public double InnerQty { get; set; }

	public double NetWeight { get; set; }

	public double OuterQty { get; set; }

	public int ExImForwardingId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int LotJobId { get; set; }

	public int PackingMaterialId { get; set; }

	public int UOMScheduleIdInner { get; set; }

	public int UOMScheduleIdOuter { get; set; }

	public int WarehouseId { get; set; }

	public int ImPurchaseOrderId { get; set; }

	public int ImPurchaseOrderDetailId { get; set; }

	public string ContainerNo { get; set; }

	public string ItemDescription { get; set; }

	public string LabReportNo { get; set; }

	public string OtherInfo { get; set; }

	public string OuterPackDescription { get; set; }

	public string SealNo { get; set; }

	public virtual double InnerUOM { get; set; }

	public virtual double OuterUOM { get; set; }

	public virtual string Packtype { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotName { get; set; }

	public virtual string WareHouseName { get; set; }
}
