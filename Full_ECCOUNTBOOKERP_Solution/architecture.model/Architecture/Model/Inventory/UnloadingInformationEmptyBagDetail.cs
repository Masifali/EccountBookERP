namespace Architecture.Model.Inventory;

public class UnloadingInformationEmptyBagDetail
{
	public double CompanyBags { get; set; }

	public double PartyBags { get; set; }

	public int LabId { get; set; }

	public int WbId { get; set; }

	public int UnloadingInformationId { get; set; }

	public int ActionTypeId { get; set; }

	public int BagsConditionId { get; set; }

	public int BagsTypeId { get; set; }

	public int GatePassId { get; set; }

	public int Id { get; set; }

	public string Remarks { get; set; }

	public virtual string BagsType { get; set; }

	public virtual string BagsCondition { get; set; }
}
