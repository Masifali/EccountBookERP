namespace Architecture.Model.Steel.Inventory;

public class InvDeliveryOrderDetail
{
	public decimal DoQty { get; set; }

	public decimal DoWeight { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal LoadingQty { get; set; }

	public decimal LoadingWeight { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int Id { get; set; }

	public int InvDeliveryOrderId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefPartyId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WarehouseId { get; set; }

	public string LoadingRemarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string WareHouseCode { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual double CityName { get; set; }

	public virtual int OrderNo { get; set; }
}
