namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionPmOther
{
	public decimal AdLs { get; set; }

	public decimal Amount { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int CommTradeTransactionHeaderId { get; set; }

	public int LoadingDeliveryPmOtherId { get; set; }

	public int LoadingDeliveryId { get; set; }

	public int CreditAccountId { get; set; }

	public int StockTypeId { get; set; }

	public int WareHouseId { get; set; }

	public int DebitAccountId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int PackingMaterialId { get; set; }

	public int UomId { get; set; }

	public string ItemSize { get; set; }

	public virtual string OtherItemName { get; set; }

	public virtual string CreditAccountTitle { get; set; }

	public virtual string DebitAccountTitle { get; set; }
}
