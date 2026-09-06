namespace Architecture.Model;

public class ItemAttributeVarient
{
	public decimal Feet { get; set; }

	public decimal Inch { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int ItemAttributeId { get; set; }

	public int OrganizationId { get; set; }

	public int Sort { get; set; }

	public int ItemId { get; set; }

	public string ComputedName { get; set; }

	public string VarientDescription { get; set; }

	public virtual string ItemAttribute { get; set; }
}
