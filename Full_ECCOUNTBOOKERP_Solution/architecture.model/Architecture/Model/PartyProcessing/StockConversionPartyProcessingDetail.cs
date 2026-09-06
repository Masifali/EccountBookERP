namespace Architecture.Model.PartyProcessing;

public class StockConversionPartyProcessingDetail
{
	public double Moisture { get; set; }

	public double Qty { get; set; }

	public double Weight { get; set; }

	public int Id { get; set; }

	public int InvStockConversionPartyProcessingId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int MoistureSlabId { get; set; }

	public int PackingtypeId { get; set; }

	public int PackUnit { get; set; }

	public int ProjectId { get; set; }

	public int StockPartyId { get; set; }

	public int ReferencePartyId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int WarehouseId { get; set; }

	public int CropYearId { get; set; }

	public int ActionTypeId { get; set; }

	public int JobOrderId { get; set; }

	public string EntryType { get; set; }

	public string Remarks { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropBatch { get; set; }

	public virtual string ItemUom { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string MoistureSlabDescription { get; set; }

	public virtual string StockParty { get; set; }

	public virtual string ReferenceParty { get; set; }

	public virtual string JobOrder { get; set; }
}
