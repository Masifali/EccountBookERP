using System;

namespace Architecture.Model.Inventory;

public class InvPurchaseInvoiceDetail
{
	public DateTime? GpDate { get; set; }

	public double AdLsWeight { get; set; }

	public double AvgCgsRate { get; set; }

	public double BillAmount { get; set; }

	public double CommissionAmount { get; set; }

	public double EBTotalWt { get; set; }

	public double EbPurAgainstWeight { get; set; }

	public double EBWeight { get; set; }

	public double ExpenseAmount { get; set; }

	public double FreightAmount { get; set; }

	public double GrossWeight { get; set; }

	public double WagesAmount { get; set; }

	public double FreightDeduction { get; set; }

	public double EbPurAgainstWeightAmount { get; set; }

	public double ScaleKart { get; set; }

	public double ItemAmount { get; set; }

	public double ItemCgsRate { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public double JournalAmount { get; set; }

	public double NetBillWeight { get; set; }

	public double NetStockWeight { get; set; }

	public double RateCut { get; set; }

	public double RateCutAmount { get; set; }

	public double ExcessWeight { get; set; }

	public double RateCutForExcessWeight { get; set; }

	public double RateCutAmountForExcessWeight { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double WeightCut { get; set; }

	public double WeightCutTotal { get; set; }

	public double Brokery { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal ItemAmountWithoutDiscount { get; set; }

	public int GpNo { get; set; }

	public int BranchId { get; set; }

	public int Id { get; set; }

	public int ActionTypeId { get; set; }

	public int InvGrnDetailId { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int TaxNameId { get; set; }

	public int UomScheduleIdRate { get; set; }

	public int WarehouseId { get; set; }

	public int LineId { get; set; }

	public int CurrencyId { get; set; }

	public int AttributeVariantId { get; set; }

	public int ItemConditionId { get; set; }

	public int RackId { get; set; }

	public string CropYear { get; set; }

	public string IsTaxable { get; set; }

	public string LabAnalisysNo { get; set; }

	public string RemarksDetail { get; set; }

	public string TaxDescriptions { get; set; }

	public string VehicleNo { get; set; }

	public DateTime? PackingDate { get; set; }

	public DateTime? ExpiryDate { get; set; }

	public int CropYearId { get; set; }

	public int DiscountTypeId { get; set; }

	public double DiscountPercent { get; set; }

	public double DiscountAmount { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocId { get; set; }

	public int RefDocSubId { get; set; }

	public int RefDocInvoiceId { get; set; }

	public int CityId { get; set; }

	public int ProductionStageId { get; set; }

	public int CastingTypeId { get; set; }

	public int GdnDocumentTypeId { get; set; }

	public int GdnId { get; set; }

	public int GdnDetailId { get; set; }

	public int LocationTypeId { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual DateTime? GdnDate { get; set; }

	public virtual int PurchaseGLAC { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string BranchName { get; set; }

	public virtual string UOMCodeItem { get; set; }

	public virtual double ItemUOMEquivalent { get; set; }

	public virtual string RateUom { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string AttributeVariant { get; set; }

	public virtual string VariantDescription { get; set; }

	public virtual int PurchaseOrder { get; set; }

	public virtual double EquivalentPoRate { get; set; }

	public virtual DateTime? OrderDate { get; set; }

	public virtual DateTime? GrnDate { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string RefDocNo { get; set; }

	public virtual string RefDocInvoiceNo { get; set; }

	public virtual string CityName { get; set; }

	public virtual string DiscountType { get; set; }

	public virtual string ProductionProcessStage { get; set; }

	public virtual string CastingType { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string rackName { get; set; }

	public virtual double RefInvoiceQty { get; set; }

	public virtual double RefInvoiceWeight { get; set; }

	public virtual double WeightFinishGoods { get; set; }

	public virtual int NoOfAttachmentsPO { get; set; }

	public virtual int NoOfAttachmentsPD { get; set; }

	public virtual int NoOfAttachmentsPreBill { get; set; }

	public virtual int NoOfAttachmentsDC { get; set; }

	public virtual int NoOfAttachmentsGrn { get; set; }

	public virtual int GrnDocumentTypeId { get; set; }

	public virtual int PurchaseOrderDocumentTypeId { get; set; }

	public virtual int PurchaseDemandDocumentTypeId { get; set; }

	public virtual int PurchaseDemandId { get; set; }

	public virtual int PurchaseDemandNo { get; set; }

	public virtual int PreBillDocumentTypeId { get; set; }

	public virtual int PreBillId { get; set; }

	public virtual int PreBillNo { get; set; }

	public virtual int DeliveryChallanDocumentTypeId { get; set; }

	public virtual int DeliveryChallanId { get; set; }

	public virtual int DeliveryChallanNo { get; set; }
}
