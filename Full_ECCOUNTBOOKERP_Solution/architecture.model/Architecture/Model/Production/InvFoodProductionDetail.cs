using System;

namespace Architecture.Model.Production;

public class InvFoodProductionDetail
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double GrossWeight { get; set; }

	public double EbUnit { get; set; }

	public double EbTotal { get; set; }

	public double Weight { get; set; }

	public double ItemPmCost { get; set; }

	public double GeneralPmCost { get; set; }

	public double ItemOhCost { get; set; }

	public double GeneralOhCost { get; set; }

	public double NetRate { get; set; }

	public double TotalAmount { get; set; }

	public int Id { get; set; }

	public int InvFoodProductionId { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int InvProductionJobOrderNo { get; set; }

	public int OutPutIdFromJobOrder { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int StockAcId { get; set; }

	public int PackingtypeId { get; set; }

	public int PackUnit { get; set; }

	public int RateUOMId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int VoucherHeadId { get; set; }

	public int JobOrderInPutId { get; set; }

	public int WarehouseId { get; set; }

	public int? DeleteFlag { get; set; }

	public int LineId { get; set; }

	public int JobOrderScheduleId { get; set; }

	public string CropBatch { get; set; }

	public string EntryType { get; set; }

	public string Remarks { get; set; }

	public int MoveOrderDocumentTypeId { get; set; }

	public int MoveOrderDocId { get; set; }

	public virtual decimal TotalWbWeight { get; set; }

	public virtual decimal UsedWbWeight { get; set; }

	public virtual decimal AvailableWbWeight { get; set; }

	public virtual int WbTicketNo { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string ItemName { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double RateEquivalent { get; set; }

	public virtual string RateUOM { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ManualBillNo { get; set; }

	public virtual DateTime DocDate { get; set; }

	public virtual DateTime RefDocDate { get; set; }

	public virtual int DocNo { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual string JobLotCode { get; set; }

	public virtual string PackTypeCode { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual int SupplierCustomerId { get; set; }

	public virtual string PartyName { get; set; }

	public virtual decimal IssueWeight { get; set; }

	public virtual decimal BalanceWeight { get; set; }

	public virtual decimal BalanceQty { get; set; }
}
