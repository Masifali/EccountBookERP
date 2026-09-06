namespace Architecture.Model.Inventory;

public class DeliveryScheduleCustomerDetail
{
	public decimal ItemQty { get; set; }

	public decimal NetWeight { get; set; }

	public int ActionTypeId { get; set; }

	public int DeliveryScheduleCustomerId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public virtual int RNo { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual string PackingType { get; set; }

	public virtual double ItemRate { get; set; }

	public virtual decimal BalQty { get; set; }

	public virtual decimal BalWeight { get; set; }
}
