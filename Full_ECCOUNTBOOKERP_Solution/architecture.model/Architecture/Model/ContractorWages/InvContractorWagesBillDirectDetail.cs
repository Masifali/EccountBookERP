namespace Architecture.Model.ContractorWages;

public class InvContractorWagesBillDirectDetail
{
	public double PackSize { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double Amount { get; set; }

	public int CompanyRepresentativeId { get; set; }

	public int Id { get; set; }

	public int InvContractorWagesBillDirectId { get; set; }

	public int WagesActivityDescription { get; set; }

	public int WagesContractorId { get; set; }

	public int WagesItemId { get; set; }

	public int WagesAccountId { get; set; }

	public string RefDocNo { get; set; }

	public string RemarksDetail { get; set; }

	public string ItemDescription { get; set; }

	public int WarehouseFromId { get; set; }

	public int WarehouseToId { get; set; }

	public virtual string ContractorName { get; set; }

	public virtual string WagesAccountName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ActivityName { get; set; }

	public virtual string WareHouseFrom { get; set; }

	public virtual string WareHouseTo { get; set; }
}
