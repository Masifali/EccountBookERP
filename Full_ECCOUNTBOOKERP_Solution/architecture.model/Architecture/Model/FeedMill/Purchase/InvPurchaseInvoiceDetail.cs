using System;

namespace Architecture.Model.FeedMill.Purchase;

public class InvPurchaseInvoiceDetail
{
	public bool IsTaxable { get; set; }

	public DateTime GpDate { get; set; }

	public decimal AdLsWeight { get; set; }

	public decimal BrokeryAmount { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal FreightDeduction { get; set; }

	public decimal EBTotalWt { get; set; }

	public decimal EBWeight { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal ExpenseAmount { get; set; }

	public decimal WagesAmount { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FreightAmount { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemNetAmount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal JournalAmount { get; set; }

	public decimal NetBillWeight { get; set; }

	public decimal NetStockWeight { get; set; }

	public decimal RateCut { get; set; }

	public decimal RateCutAmount { get; set; }

	public decimal TaxAmount { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal WeightCut { get; set; }

	public decimal WeightCutTotal { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CropYearId { get; set; }

	public int CurrencyId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGrnDetailId { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int JobLotId { get; set; }

	public int LabAnalisysId { get; set; }

	public int LineId { get; set; }

	public int PackingTypeId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int RateUomId { get; set; }

	public int TaxNameId { get; set; }

	public int WarehouseId { get; set; }

	public string LabAnalisysNo { get; set; }

	public string RemarksDetail { get; set; }

	public string TaxDescriptions { get; set; }

	public string VehicleNo { get; set; }

	public virtual string TaxName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string CityName { get; set; }

	public virtual string ItemUom { get; set; }

	public virtual double ItemEquivalent { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual int PurchaseOrder { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateEquivalent { get; set; }
}
