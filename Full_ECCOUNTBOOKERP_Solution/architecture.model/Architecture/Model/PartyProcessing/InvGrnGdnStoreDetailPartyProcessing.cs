namespace Architecture.Model.PartyProcessing;

public class InvGrnGdnStoreDetailPartyProcessing
{
	public decimal ItemQty { get; set; }

	public int Id { get; set; }

	public int InvGrnStorePartyProcessingId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int WarehouseId { get; set; }

	public int ActionTypeId { get; set; }

	public string RemarksSub { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string WareHouseName { get; set; }
}
