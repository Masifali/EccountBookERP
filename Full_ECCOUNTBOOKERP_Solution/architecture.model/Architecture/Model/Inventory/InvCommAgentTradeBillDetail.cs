using System;

namespace Architecture.Model.Inventory;

public class InvCommAgentTradeBillDetail
{
	public decimal BillWeight { get; set; }

	public decimal AddLessSale { get; set; }

	public decimal SaleBillWeight { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal SaleGrossWeight { get; set; }

	public decimal Qty { get; set; }

	public decimal RatePurchase { get; set; }

	public decimal PurchaseRateWithoutAddLess { get; set; }

	public decimal PurchaseRateAddLess { get; set; }

	public decimal RateSale { get; set; }

	public decimal SaleRateAddLess { get; set; }

	public decimal SaleRateWithoutAddLess { get; set; }

	public decimal PurchaseFreight { get; set; }

	public decimal PurchaseCommission { get; set; }

	public decimal PurchaseBrokery { get; set; }

	public decimal SaleCommission { get; set; }

	public decimal SaleBrokery { get; set; }

	public double PurchaseFreightLess { get; set; }

	public double ItemAmount { get; set; }

	public double EbCut { get; set; }

	public double EbCutTotal { get; set; }

	public double PurchaseExpenses { get; set; }

	public double SaleExpenses { get; set; }

	public double PurchaseNetAmount { get; set; }

	public double SaleNetAmount { get; set; }

	public double SaleEbCut { get; set; }

	public double SaleEbCutTotal { get; set; }

	public int ActionTypeId { get; set; }

	public int Crop { get; set; }

	public int Id { get; set; }

	public int InvCommAgentTradeBillId { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int RateUomId { get; set; }

	public string RefDocNo { get; set; }

	public string RemarksDetail { get; set; }

	public string VehicleNo { get; set; }

	public double SaleAmount { get; set; }

	public decimal AddLessPurchase { get; set; }

	public decimal AmountAfterTaxPurchase { get; set; }

	public decimal AmountAfterTaxSale { get; set; }

	public decimal FcyAmountPurchase { get; set; }

	public decimal FcyAmountSale { get; set; }

	public decimal TaxAmountPurchase { get; set; }

	public decimal TaxAmountSale { get; set; }

	public decimal TaxPercentPurchase { get; set; }

	public decimal TaxPercentSale { get; set; }

	public int SaleRateUomId { get; set; }

	public int TaxNameIdPurchase { get; set; }

	public int TaxNameIdSale { get; set; }

	public string BiltyNo { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int grnSupplierLoadingDetailId { get; set; }

	public int gdnBuyerDispatchMasterId { get; set; }

	public int gdnBuyerDispatchDetailId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int purchaseOrderDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int saleOrderDetailId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string SaleRateUom { get; set; }

	public virtual double SaleRateUomEquivalent { get; set; }

	public virtual string TaxNamePurchase { get; set; }

	public virtual string TaxNameSale { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime? PurchaseOrderDate { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual DateTime? SaleOrderDate { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual DateTime? GdnDate { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual DateTime? GrnDate { get; set; }
}
