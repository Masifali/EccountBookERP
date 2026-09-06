namespace Architecture.Model.POS;

public class WsRmStockTransferNotesExpenses
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int WsRmStockTransferNotesId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
