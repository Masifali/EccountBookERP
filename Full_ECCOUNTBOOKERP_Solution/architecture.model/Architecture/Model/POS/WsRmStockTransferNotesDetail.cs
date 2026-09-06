namespace Architecture.Model.POS;

public class WsRmStockTransferNotesDetail
{
	public double BillWeight { get; set; }

	public double IssuedAmount { get; set; }

	public double IssuedQty { get; set; }

	public double IssuedRate { get; set; }

	public double StockWeight { get; set; }

	public double ExpenseAmount { get; set; }

	public double ItemNetAmount { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int WarehouseId { get; set; }

	public int? LineId { get; set; }

	public int WsRmRequisitionPoDetailId { get; set; }

	public int WsRmRequisitionPoId { get; set; }

	public int WsRmStockTransferNotesId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual int RequisitionNo { get; set; }

	public virtual double ApprovedQty { get; set; }
}
