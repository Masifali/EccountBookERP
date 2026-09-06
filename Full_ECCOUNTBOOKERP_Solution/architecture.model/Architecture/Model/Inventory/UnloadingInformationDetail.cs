namespace Architecture.Model.Inventory;

public class UnloadingInformationDetail
{
	public double PackSize { get; set; }

	public double UnloadedQty { get; set; }

	public int UnloadingInformationId { get; set; }

	public int LabId { get; set; }

	public int WbId { get; set; }

	public int ActionTypeId { get; set; }

	public int GatePassId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int WareHouseId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string PackingType { get; set; }
}
