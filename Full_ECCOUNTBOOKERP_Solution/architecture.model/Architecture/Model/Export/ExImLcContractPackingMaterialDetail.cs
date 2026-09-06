namespace Architecture.Model.Export;

public class ExImLcContractPackingMaterialDetail
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int ExImLcOrderId { get; set; }

	public int Id { get; set; }

	public int BrandId { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int SortNo { get; set; }

	public string pmRemarks { get; set; }

	public int BrandUomId { get; set; }

	public double BrandOuterQty { get; set; }

	public double BrandInnerQty { get; set; }

	public int ItemUomId { get; set; }

	public double InnerQty { get; set; }

	public virtual string BrandCode { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandUomCode { get; set; }

	public virtual double BrandUomEquivalent { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }
}
