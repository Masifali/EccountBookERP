namespace Architecture.Model.pcc;

public class ServiceActivityAllocationToDocumentType
{
	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int ContractorWagesActivityId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SortNo { get; set; }

	public virtual string DocumentTypeDescription { get; set; }

	public virtual string ContractorWagesActivity { get; set; }
}
