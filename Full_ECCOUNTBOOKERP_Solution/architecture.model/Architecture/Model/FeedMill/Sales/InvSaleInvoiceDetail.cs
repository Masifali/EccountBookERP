using System;

namespace Architecture.Model.FeedMill.Sales;

public class InvSaleInvoiceDetail
{
	public bool CommOnSale { get; set; }

	public bool IsTaxable { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime GpDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal ItemRateWOExp { get; set; }

	public decimal PackingAddLess { get; set; }

	public double AdLsWeight { get; set; }

	public double BillAmount { get; set; }

	public double CommissionAmount { get; set; }

	public double EBTotalWt { get; set; }

	public double EBWeight { get; set; }

	public double ExpenseAmount { get; set; }

	public double FreightAmount { get; set; }

	public double GrossWeight { get; set; }

	public double ItemAmount { get; set; }

	public double ItemCgsRate { get; set; }

	public double ItemDiscount { get; set; }

	public double ItemDiscountAmount { get; set; }

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

	public int ActionTypeId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public int CgsRateUomId { get; set; }

	public int CityId { get; set; }

	public int CropYearId { get; set; }

	public int CurrencyId { get; set; }

	public int DiscountTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGdnDetailId { get; set; }

	public int InvGdnId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int ModifyUserId { get; set; }

	public int PackingTypeId { get; set; }

	public int PaymentTermId { get; set; }

	public int RateUomId { get; set; }

	public int RefPartyId { get; set; }

	public int ReserveWareHouse { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int TaxNameId { get; set; }

	public int TaxTypeId { get; set; }

	public int WarehouseId { get; set; }

	public string LabAnalisysNo { get; set; }

	public string ReferenceNo { get; set; }

	public string RemarksDetail { get; set; }

	public string TaxDescriptions { get; set; }

	public string VehicleNo { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateEquivalent { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string TaxName { get; set; }

	public virtual string TaxType { get; set; }

	public virtual string CityName { get; set; }

	public virtual int SaleOrder { get; set; }
}
