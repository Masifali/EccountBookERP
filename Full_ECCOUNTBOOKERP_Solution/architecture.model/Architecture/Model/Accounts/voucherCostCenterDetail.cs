namespace Architecture.Model.Accounts;

public class voucherCostCenterDetail
{
	public decimal costPrcent { get; set; }

	public double costAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int CostCenterId { get; set; }

	public int Id { get; set; }

	public int SortNo { get; set; }

	public int VoucherDetailId { get; set; }

	public int VoucherHeaderId { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual int ParentCostCenterId { get; set; }

	public virtual string CostCenterName { get; set; }
}
