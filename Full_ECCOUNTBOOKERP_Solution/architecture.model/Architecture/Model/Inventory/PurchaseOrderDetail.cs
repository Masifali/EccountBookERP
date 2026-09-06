using System;

namespace Architecture.Model.Inventory;

public class PurchaseOrderDetail
{
	public double Amount { get; set; }

	public double BagPrice { get; set; }

	public double BagWeight { get; set; }

	public double NetWeight { get; set; }

	public double OrderItemQty { get; set; }

	public double OrderItemRate { get; set; }

	public double RetailRate { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double TotalAmount { get; set; }

	public double WeightPerQty { get; set; }

	public int Id { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int OrderItemId { get; set; }

	public int OrderItemRateUOMId { get; set; }

	public int OrderItemUOMId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int TaxNameId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocId { get; set; }

	public int RefDocInvoiceId { get; set; }

	public int CityId { get; set; }

	public string AmountCalcType { get; set; }

	public string CityArea { get; set; }

	public string Crop { get; set; }

	public string LabSampleNo { get; set; }

	public string Moisture { get; set; }

	public string OrderRemarks { get; set; }

	public string TaxableStatus { get; set; }

	public int InvLabSampleAnalysisHeaderId { get; set; }

	public int LabAnalysisStandardScheduleId { get; set; }

	public int CurrencyId { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public DateTime? PackingDate { get; set; }

	public DateTime? ExpiryDate { get; set; }

	public int CropYearId { get; set; }

	public int LeadTime { get; set; }

	public string WeightCapacity { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string TaxName { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double UOMDescription { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual double EquivalentRate { get; set; }

	public virtual double DemandBalanceQty { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string RefDocNo { get; set; }

	public virtual string RefDocInvoiceNo { get; set; }
}
