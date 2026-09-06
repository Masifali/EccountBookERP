namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionVehicleFreightDetail
{
	public decimal BiltyFreight { get; set; }

	public decimal NetPaid { get; set; }

	public decimal VehicleQty { get; set; }

	public int CommTradeTransactionHeaderId { get; set; }

	public int CreditAccountId { get; set; }

	public int CustomerId { get; set; }

	public int Id { get; set; }

	public int LoadingDeliveryId { get; set; }

	public int SortNo { get; set; }

	public string BiltyNo { get; set; }

	public string Remarks { get; set; }

	public string VehicleNo { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string CreditAccountTitle { get; set; }
}
