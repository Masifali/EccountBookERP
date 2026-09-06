using System;

namespace Architecture.Model.Inventory;

public class InvSaleInvoiceDetail
{
	public DateTime GpDate { get; set; }

	public DateTime? DueDate { get; set; }

	public double AdLsWeight { get; set; }

	public double AvgCgsRate { get; set; }

	public double BillAmount { get; set; }

	public double CommissionAmount { get; set; }

	public double EBTotalWt { get; set; }

	public double EBWeight { get; set; }

	public double ExpenseAmount { get; set; }

	public double FreightAmount { get; set; }

	public double GrossWeight { get; set; }

	public double ItemAmount { get; set; }

	public double ItemCgsRate { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public double JournalAmount { get; set; }

	public double NetBillWeight { get; set; }

	public double NetStockWeight { get; set; }

	public double RateCut { get; set; }

	public double RateCutAmount { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double WeightCut { get; set; }

	public double WeightCutTotal { get; set; }

	public double ItemDiscount { get; set; }

	public double ItemDiscountAmount { get; set; }

	public decimal ItemRateWOExp { get; set; }

	public decimal PackingAddLess { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public int CurrencyId { get; set; }

	public int GpNo { get; set; }

	public int DiscountTypeId { get; set; }

	public int Id { get; set; }

	public int InvGdnDetailId { get; set; }

	public int InvGdnId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int ItemVariantId { get; set; }

	public int CastingTypeId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int SaleOrderId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int TaxNameId { get; set; }

	public int UomScheduleIdRate { get; set; }

	public int WarehouseId { get; set; }

	public int RackId { get; set; }

	public int InvForwardingId { get; set; }

	public int InvForwardingDetailId { get; set; }

	public int RefRefDocumentTypeId { get; set; }

	public int RefRefDocIdNo { get; set; }

	public int RefDocSubId { get; set; }

	public int LineId { get; set; }

	public int CityId { get; set; }

	public int BranchId { get; set; }

	public int ReserveWareHouse { get; set; }

	public int PaymentTermId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public int DueDays { get; set; }

	public int CgsRateUomId { get; set; }

	public int GdnPartyProcessingId { get; set; }

	public int ProductionStageId { get; set; }

	public int GdnDetailPartyProcessingId { get; set; }

	public int CostCenterId { get; set; }

	public int BrandItemId { get; set; }

	public int ItemConditionId { get; set; }

	public string CropYear { get; set; }

	public string IsTaxable { get; set; }

	public string LabAnalisysNo { get; set; }

	public string RemarksDetail { get; set; }

	public string TaxDescriptions { get; set; }

	public string VehicleNo { get; set; }

	public string ReferenceNo { get; set; }

	public bool CommOnSale { get; set; }

	public int SecondaryUomId { get; set; }

	public int ReasonId { get; set; }

	public decimal SecondaryUomQty { get; set; }

	public decimal SecondaryUomItemRate { get; set; }

	public virtual double SecondaryUomEquivalent { get; set; }

	public virtual double PerItemSecondaryUomQty { get; set; }

	public virtual string SecondaryUomCode { get; set; }

	public virtual int SaleGLAC { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string BrandItemCode { get; set; }

	public virtual string BrandItemName { get; set; }

	public virtual string DiscountType { get; set; }

	public virtual string BranchName { get; set; }

	public virtual string UOMCodeItem { get; set; }

	public virtual string UOMCodeRate { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ReserveWareHouseName { get; set; }

	public virtual string CityName { get; set; }

	public virtual string TaxName { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual double RefInvoiceQty { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual int SaleOrder { get; set; }

	public virtual int ForwardingDocNo { get; set; }

	public virtual int OtherCategoryId { get; set; }

	public virtual string OtherCategory { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual double RateUOM { get; set; }

	public virtual double PackUom { get; set; }

	public virtual string RefRefDocumentType { get; set; }

	public virtual string PaymentTerm { get; set; }

	public virtual string CastingType { get; set; }

	public virtual string ProductionProcessStage { get; set; }

	public virtual string VariantDescription { get; set; }

	public virtual string rackName { get; set; }

	public virtual DateTime OrderDate { get; set; }

	public virtual double WeightFinishGoods { get; set; }

	public virtual double PurchaseInvoiceQty { get; set; }

	public virtual double PurchaseUsedQty { get; set; }

	public virtual double PurchaseBalanceQty { get; set; }
}
