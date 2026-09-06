namespace Architecture.Model.PartyProcessing;

public class InvStockAdjustmentDetailPartyProcessing
{
	public double NetWeight { get; set; }

	public double Qty { get; set; }

	public int ActionTypeId { get; set; }

	public int CropYearId { get; set; }

	public int Id { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int InvStockAdjustmentId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int WarehouseId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }
}
