using System;

namespace Architecture.Model.Tax.Purchase;

public class PurchaseInvoiceDetail
{
	public DateTime EntryDate { get; set; }

	public DateTime GpDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public double AdLsWeight { get; set; }

	public double BillAmount { get; set; }

	public double Brokery { get; set; }

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

	public double OtherRate { get; set; }

	public double RateCut { get; set; }

	public double RateCutAmount { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double WagesAmount { get; set; }

	public double WeightCut { get; set; }

	public double WeightCutTotal { get; set; }

	public int ActionTypeId { get; set; }

	public int CurrencyId { get; set; }

	public int EntryUserId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGrnDetailId { get; set; }

	public int InvGrnId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int ModifyUserId { get; set; }

	public int OtherRateUomId { get; set; }

	public int PackingTypeId { get; set; }

	public int PurchaseInvoiceId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int RevisionNo { get; set; }

	public int TaxNameId { get; set; }

	public int UomScheduleIdRate { get; set; }

	public int WarehouseId { get; set; }

	public string CropYear { get; set; }

	public string IsTaxable { get; set; }

	public string LabAnalisysNo { get; set; }

	public string RemarksDetail { get; set; }

	public string TaxDescriptions { get; set; }

	public string VehicleNo { get; set; }

	public virtual int PurchaseOrder { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual double PackUom { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CityName { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double EquivalentPoRate { get; set; }

	public virtual int GrnNo { get; set; }
}
