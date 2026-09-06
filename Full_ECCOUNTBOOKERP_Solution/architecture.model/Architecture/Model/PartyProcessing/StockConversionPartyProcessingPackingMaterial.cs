namespace Architecture.Model.PartyProcessing;

public class StockConversionPartyProcessingPackingMaterial
{
	public double ItemQty { get; set; }

	public int Id { get; set; }

	public int InvStockConversionPartyProcessingId { get; set; }

	public int ItemId { get; set; }

	public int WarehouseId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }
}
