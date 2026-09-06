namespace Architecture.Model.Inventory;

public class UnloadingInformationWagesDetail
{
	public double PackSize { get; set; }

	public double UnloadedQty { get; set; }

	public int LabId { get; set; }

	public int WbId { get; set; }

	public int UnloadingInformationId { get; set; }

	public int ActionTypeId { get; set; }

	public int ContractorId { get; set; }

	public int GatePassId { get; set; }

	public int Id { get; set; }

	public int BagsTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual string BagsType { get; set; }

	public virtual string ContractorName { get; set; }
}
