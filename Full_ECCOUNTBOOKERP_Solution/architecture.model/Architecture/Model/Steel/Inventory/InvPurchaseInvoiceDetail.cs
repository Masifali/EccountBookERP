using System;

namespace Architecture.Model.Steel.Inventory;

public class InvPurchaseInvoiceDetail
{
	public DateTime GpDate { get; set; }

	public decimal AdLsWeight { get; set; }

	public decimal BrokeryAmount { get; set; }

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

	public decimal NetBillWeight { get; set; }

	public decimal NetStockWeight { get; set; }

	public decimal RateCut { get; set; }

	public decimal RateCutAmount { get; set; }

	public decimal TaxAmount { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal WCut { get; set; }

	public decimal WcutTotal { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CurrencyId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvGrnDetailId { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int JobLotId { get; set; }

	public int LabAnalysisHeaderId { get; set; }

	public int PackingTypeId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int TaxNameId { get; set; }

	public int UomScheduleIdRate { get; set; }

	public int WarehouseId { get; set; }

	public string IsTaxable { get; set; }

	public string LabAnalysisNo { get; set; }

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
