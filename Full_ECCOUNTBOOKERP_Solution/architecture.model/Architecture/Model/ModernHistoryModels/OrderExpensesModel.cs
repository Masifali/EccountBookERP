namespace Architecture.Model.ModernHistoryModels;

public class OrderExpensesModel
{
	public int Id { get; set; }

	public int OrderId { get; set; }

	public string AccountTitle { get; set; }

	public double Percentage { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double Amount { get; set; }

	public string Remarks { get; set; }
}
