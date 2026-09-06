namespace Architecture.Model.POS;

public class WsRmRequisitionPoDetail
{
	public double ApprovedQty { get; set; }

	public double ReqAmount { get; set; }

	public double ReqQty { get; set; }

	public double ReqRate { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int WsRmRequisitionPoId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string UOMDescription { get; set; }

	public virtual string ItemName { get; set; }
}
