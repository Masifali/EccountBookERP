namespace Architecture.Model.PartyProcessing;

public class InvProductionJobOrderInputPartyProcessing
{
	public double NetWeight { get; set; }

	public double Qty { get; set; }

	public int CropYearId { get; set; }

	public int ActionTypeId { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderPartyProcessingId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int UomSchIdQty { get; set; }

	public int WarehouseId { get; set; }

	public string BatchCrop { get; set; }

	public string RemarksInputDt { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotCode { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string PackingType { get; set; }
}
