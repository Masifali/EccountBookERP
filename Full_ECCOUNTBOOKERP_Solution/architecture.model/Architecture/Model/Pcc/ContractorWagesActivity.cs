namespace Architecture.Model.pcc;

public class ContractorWagesActivity
{
	public bool IsActive { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int GlAccountId { get; set; }

	public int WagesTypeId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int SortNo { get; set; }

	public string WagesActivity { get; set; }

	public virtual string AccountTitle { get; set; }
}
