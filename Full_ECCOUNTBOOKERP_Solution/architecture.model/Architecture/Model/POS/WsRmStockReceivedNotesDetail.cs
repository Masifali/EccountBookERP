namespace Architecture.Model.POS;

public class WsRmStockReceivedNotesDetail
{
	public double BillWeight { get; set; }

	public double ReceivedAmount { get; set; }

	public double ReceivedQty { get; set; }

	public double ReceivedRate { get; set; }

	public double StockWeight { get; set; }

	public double ExpenseAmount { get; set; }

	public double ItemNetAmount { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int WarehouseId { get; set; }

	public int? LineId { get; set; }

	public int WsRmStockReceivedNotesId { get; set; }

	public int WsRmStockTransferNotesDetailId { get; set; }

	public int WsRmStockTransferNotesId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual int TransferNo { get; set; }

	public virtual double IssuedQuantity { get; set; }
}
