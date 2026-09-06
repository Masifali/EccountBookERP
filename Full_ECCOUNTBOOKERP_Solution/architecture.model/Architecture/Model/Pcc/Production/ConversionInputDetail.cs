namespace Architecture.Model.pcc.Production;

public class ConversionInputDetail
{
	public int Id { get; set; }

	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Rate { get; set; }

	public int ActionTypeId { get; set; }

	public int ConversionHeaderId { get; set; }

	public decimal VarientEquivalent { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public decimal VarientQty { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int LineId { get; set; }

	public int RateUomId { get; set; }

	public int WareHouseId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemAttribute { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
