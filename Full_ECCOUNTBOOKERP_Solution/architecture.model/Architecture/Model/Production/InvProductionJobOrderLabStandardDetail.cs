namespace Architecture.Model.Production;

public class InvProductionJobOrderLabStandardDetail
{
	public bool Active { get; set; }

	public decimal MaxValue { get; set; }

	public decimal MinValue { get; set; }

	public int ActionTypeId { get; set; }

	public int BrandItemId { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int InvProductionJobOrderLabStandardDetailId { get; set; }

	public int LabAnalysisItemId { get; set; }

	public int LabStandardMainId { get; set; }

	public int SupplierCustomerId { get; set; }

	public virtual int LabAnalysisGroupId { get; set; }

	public virtual string LabScheduleName { get; set; }

	public virtual string PartyName { get; set; }

	public virtual string BrandItemName { get; set; }

	public virtual string BrandItemCode { get; set; }

	public virtual string LabAnalysisItemName { get; set; }

	public virtual string LabAnalysisItemCode { get; set; }

	public virtual string LabAnalysisGroupDescription { get; set; }

	public virtual string LabAnalysisGroupCode { get; set; }
}
