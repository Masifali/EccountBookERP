namespace Architecture.Model.Inventory;

public class InvOrderCategory
{
	public int Id { get; set; }

	public int OrderCatagoryId { get; set; }

	public string OrderCategoryName { get; set; }

	public string OrderCategoryDescription { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectId { get; set; }

	public int FinancialYearId { get; set; }
}
