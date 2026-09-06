using System;

namespace Architecture.Model.Steel.Inventory;

public class InvSaleInvoiceDetail
{
	public DateTime GpDate { get; set; }

	public decimal AdLsWeight { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal ExpenseAmount { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FreightAmount { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemCgsRate { get; set; }

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

	public int CurrencyId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGdnDetailId { get; set; }

	public int InvGdnId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUOMId { get; set; }

	public int RateUomScheduleId { get; set; }

	public int RefDocSubId { get; set; }

	public int RefRefDocIdNo { get; set; }

	public int RefRefDocumentTypeId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int TaxNameId { get; set; }

	public int WarehouseId { get; set; }

	public string IsTaxable { get; set; }

	public string LabAnalisysNo { get; set; }

	public string RemarksDetail { get; set; }

	public string TaxDescriptions { get; set; }

	public string VehicleNo { get; set; }

	public virtual int SaleOrder { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double EquivalentSoRate { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CityName { get; set; }

	public virtual int SaleGLAC { get; set; }
}
