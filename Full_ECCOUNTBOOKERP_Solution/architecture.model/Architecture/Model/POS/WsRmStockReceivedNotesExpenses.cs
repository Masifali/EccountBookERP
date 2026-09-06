namespace Architecture.Model.POS;

public class WsRmStockReceivedNotesExpenses
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int WsRmStockReceivedNotesId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
