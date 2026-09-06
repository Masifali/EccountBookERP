namespace Architecture.Model.TaxProject.Inventory;

public class GetJobLotGlIdsandName
{
	public int Id { get; set; }

	public int AccountId { get; set; }

	public int BranchId { get; set; }

	public string JobLotCode { get; set; }

	public string JobLotDescription { get; set; }

	public string BranchName { get; set; }

	public string AccountTitle { get; set; }
}
