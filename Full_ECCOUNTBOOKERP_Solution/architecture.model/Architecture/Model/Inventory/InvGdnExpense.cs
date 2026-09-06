namespace Architecture.Model.Inventory;

public class InvGdnExpense
{
	public double Qty { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int ItemId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
