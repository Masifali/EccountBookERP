namespace Architecture.Model.pcc;

public class SaleOrderDetail
{
	public bool CommOnSale { get; set; }

	public bool IsApproved { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal ItemDiscountAmount { get; set; }

	public decimal ItemDiscountRate { get; set; }

	public decimal ItemNetAmount { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal ItemNetWeight { get; set; }

	public decimal OrderItemAmount { get; set; }

	public decimal OrderItemQty { get; set; }

	public decimal OrderItemRate { get; set; }

	public decimal VarientEquivalent { get; set; }

	public decimal ItemRateWithOutAddLess { get; set; }

	public decimal RateAddLess { get; set; }

	public int ActionTypeId { get; set; }

	public int ScheduleId { get; set; }

	public int CityId { get; set; }

	public int CurrencyId { get; set; }

	public int Id { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemDiscountTypeId { get; set; }

	public int JobLotId { get; set; }

	public int OrderItemId { get; set; }

	public int SaleOrderId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string CityName { get; set; }

	public virtual string ItemDiscountType { get; set; }
}
