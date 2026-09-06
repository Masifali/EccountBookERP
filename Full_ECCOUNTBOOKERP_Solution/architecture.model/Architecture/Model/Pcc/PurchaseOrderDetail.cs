namespace Architecture.Model.pcc;

public class PurchaseOrderDetail
{
	public bool IsApproved { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemNetAmount { get; set; }

	public decimal ItemNetWeight { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal TaxAmount { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal VarientEquivalent { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CurrencyId { get; set; }

	public int Id { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int TaxTypeId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemCodeNew { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string CityName { get; set; }

	public virtual string CurrencyCode { get; set; }

	public virtual string TaxName { get; set; }
}
