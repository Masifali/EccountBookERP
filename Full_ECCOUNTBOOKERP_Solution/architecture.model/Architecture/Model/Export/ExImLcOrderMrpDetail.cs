namespace Architecture.Model.Export;

public class ExImLcOrderMrpDetail
{
	public decimal ReqQty { get; set; }

	public int ExImLcOrderId { get; set; }

	public int ExImLcOrderMrpHeaderId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemIdPm { get; set; }

	public int ItemUomId { get; set; }

	public int ItemUomIdPm { get; set; }

	public int SortNo { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemNamePm { get; set; }

	public virtual string PackUom { get; set; }

	public virtual string PackUomPm { get; set; }
}
