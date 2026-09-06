namespace Architecture.Model.Inventory;

public class InvStockTransferDetail
{
	public int Id { get; set; }

	public int InvStockTransferHeaderId { get; set; }

	public int ItemId { get; set; }

	public int InvPackingTypeId { get; set; }

	public int CropYearId { get; set; }

	public string CropYear { get; set; }

	public int JobLotId { get; set; }

	public int JobLotIdTo { get; set; }

	public int PackUomId { get; set; }

	public double Qty { get; set; }

	public double GrossWeight { get; set; }

	public double AdLsWeight { get; set; }

	public double NetWeight { get; set; }

	public double EbUnit { get; set; }

	public double EbTotal { get; set; }

	public double ItemRate { get; set; }

	public int RateUomId { get; set; }

	public double ItemAmount { get; set; }

	public double ExpenseAmount { get; set; }

	public int FromWarehouseId { get; set; }

	public int ToWarehouseId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int TransferDocumentTypeId { get; set; }

	public int TransferDetailId { get; set; }

	public int TransferId { get; set; }

	public int RefDocInvoiceId { get; set; }

	public int LineId { get; set; }

	public int ItemConditionId { get; set; }

	public int RackId { get; set; }

	public int ToRackId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual double RateUom { get; set; }

	public virtual string FromWareHouseName { get; set; }

	public virtual string ToWareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string JobLotTo { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string RefDocNo { get; set; }

	public virtual string RefDocInvoiceNo { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string rackName { get; set; }

	public virtual string TorackName { get; set; }
}
