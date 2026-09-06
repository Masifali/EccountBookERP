namespace Architecture.Model.Mfg;

public class BomByProductDetail
{
	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal Weight { get; set; }

	public int ActionTypeId { get; set; }

	public int BomHeaderId { get; set; }

	public int BomByProductDetailId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int RateUomId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
