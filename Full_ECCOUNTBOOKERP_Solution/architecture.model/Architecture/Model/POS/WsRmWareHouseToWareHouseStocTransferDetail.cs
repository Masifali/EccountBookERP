namespace Architecture.Model.POS;

public class WsRmWareHouseToWareHouseStocTransferDetail
{
	public int Id { get; set; }

	public int WsRmStockTransferId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public double ItemQty { get; set; }

	public double NetWeight { get; set; }

	public double ItemRate { get; set; }

	public double ItemAmount { get; set; }

	public string RemarksDetail { get; set; }

	public int WareHouseFromId { get; set; }

	public int WareHouseToId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual string WarehouseFromName { get; set; }

	public virtual string WarehouseToName { get; set; }
}
