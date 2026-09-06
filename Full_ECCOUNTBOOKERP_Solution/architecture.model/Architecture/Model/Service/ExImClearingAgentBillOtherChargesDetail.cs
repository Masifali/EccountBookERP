namespace Architecture.Model.Service;

public class ExImClearingAgentBillOtherChargesDetail
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int BillHeaderId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int Id { get; set; }

	public string OtherCharges { get; set; }
}
