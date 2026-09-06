namespace Architecture.Model.PartyProcessing;

public class InvStockTransferDetailPartyProcessing
{
	public double AdLsWeight { get; set; }

	public double EbTotal { get; set; }

	public double EbUnit { get; set; }

	public double ExpenseAmount { get; set; }

	public double GrossWeight { get; set; }

	public double ItemAmount { get; set; }

	public double ItemRate { get; set; }

	public double NetWeight { get; set; }

	public double Qty { get; set; }

	public int CropYearId { get; set; }

	public int FromStockPartyId { get; set; }

	public int FromWarehouseId { get; set; }

	public int Id { get; set; }

	public int InvPackingTypeId { get; set; }

	public int InvStockTransferHeaderId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int JobLotIdTo { get; set; }

	public int LineId { get; set; }

	public int PackUomId { get; set; }

	public int RateUomId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int ToStockPartyId { get; set; }

	public int ToWarehouseId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string FromStockParty { get; set; }

	public virtual string ToStockParty { get; set; }

	public virtual string FromWareHouseName { get; set; }

	public virtual string ToWareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string JobLotTo { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double Equivalent { get; set; }
}
