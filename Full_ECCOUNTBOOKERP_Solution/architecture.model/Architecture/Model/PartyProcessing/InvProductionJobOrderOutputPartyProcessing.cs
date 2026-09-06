namespace Architecture.Model.PartyProcessing;

public class InvProductionJobOrderOutputPartyProcessing
{
	public double NetWeight { get; set; }

	public double QtyInner { get; set; }

	public double QtyOuter { get; set; }

	public int CropYearId { get; set; }

	public int Id { get; set; }

	public int InvPackingTypeId { get; set; }

	public int InvProductionJobOrderPartyProcessingId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int UomSchIdInner { get; set; }

	public int UomSchIdOuter { get; set; }

	public int WarehouseId { get; set; }

	public string PackRemarks { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string InnerUomCode { get; set; }

	public virtual string outerUOMCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }
}
