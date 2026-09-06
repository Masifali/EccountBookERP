namespace Architecture.Model.pcc;

public class InvDeliveryOrderdetail
{
	public bool IsFOC { get; set; }

	public decimal DoNetWeight { get; set; }

	public decimal DoQty { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal VarientEquivalent { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int Id { get; set; }

	public int InvDeliveryOrderId { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WarehouseId { get; set; }

	public string ReferencePartyAddress { get; set; }

	public string ReferencePartyCellNo { get; set; }

	public string ReferencePartyName { get; set; }

	public string RemarksDetail { get; set; }

	public int ReferencePartyId { get; set; }

	public int BuildingHeightId { get; set; }

	public int BuildingStoreyId { get; set; }

	public string BuildingArea { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string CityName { get; set; }

	public virtual string BuildingHeight { get; set; }

	public virtual string BuildingStorey { get; set; }
}
