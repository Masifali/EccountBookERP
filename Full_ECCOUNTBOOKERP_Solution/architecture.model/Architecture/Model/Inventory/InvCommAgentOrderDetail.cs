namespace Architecture.Model.Inventory;

public class InvCommAgentOrderDetail
{
	public decimal Qty { get; set; }

	public decimal Weight { get; set; }

	public double Amount { get; set; }

	public double CustomerRate { get; set; }

	public double SupplierRate { get; set; }

	public int ActionId { get; set; }

	public int CityAreaId { get; set; }

	public int Crop { get; set; }

	public int Id { get; set; }

	public int InvCommAgentOrderId { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public string RemarksDetail { get; set; }

	public double CustomerAmount { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string CityName { get; set; }
}
