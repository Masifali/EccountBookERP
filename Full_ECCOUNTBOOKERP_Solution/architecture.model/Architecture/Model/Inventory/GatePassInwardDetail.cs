namespace Architecture.Model.Inventory;

public class GatePassInwardDetail
{
	public int Id { get; set; }

	public int GatePassInwardId { get; set; }

	public int ItemId { get; set; }

	public double PackUnit { get; set; }

	public double Weight { get; set; }

	public int ItemUOMId { get; set; }

	public double ItemQty { get; set; }

	public string CropYear { get; set; }

	public int JobLotId { get; set; }

	public int SupplyScheduleId { get; set; }

	public int WareHouseId { get; set; }

	public int PackingTypeId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int CityId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual int DocNo { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string CityName { get; set; }
}
