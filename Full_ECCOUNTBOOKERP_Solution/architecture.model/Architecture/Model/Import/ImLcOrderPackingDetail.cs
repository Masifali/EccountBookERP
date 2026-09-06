namespace Architecture.Model.Import;

public class ImLcOrderPackingDetail
{
	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemRate { get; set; }

	public decimal LcyAmount { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Qty { get; set; }

	public int ActionTypeId { get; set; }

	public int CurrencyId { get; set; }

	public int Id { get; set; }

	public int ImItemId { get; set; }

	public int ImLcOrderId { get; set; }

	public int InvPackingTypeId { get; set; }

	public int ItemBrandId { get; set; }

	public int ItemUomId { get; set; }

	public int RateUomId { get; set; }

	public string ItemDescription { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
