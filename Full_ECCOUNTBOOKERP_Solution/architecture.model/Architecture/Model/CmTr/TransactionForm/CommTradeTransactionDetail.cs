using System;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionDetail
{
	public DateTime dDocDate { get; set; }

	public decimal CusAdLsWt { get; set; }

	public decimal CusAmount { get; set; }

	public int CusDueDays { get; set; }

	public decimal CusGrossWeight { get; set; }

	public decimal CusNetWeight { get; set; }

	public decimal CusPckMtrlWeight { get; set; }

	public decimal CusRate { get; set; }

	public decimal CustFcyAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal SupAdLsWt { get; set; }

	public decimal SupAmount { get; set; }

	public int SupDueDays { get; set; }

	public decimal SupFcyAmount { get; set; }

	public decimal SupGrossWeight { get; set; }

	public decimal SupNetWeight { get; set; }

	public decimal SupPckMtrlWeight { get; set; }

	public decimal SupRate { get; set; }

	public decimal Qty { get; set; }

	public DateTime BiltyDate { get; set; }

	public decimal VehicleQty { get; set; }

	public decimal VehicleGrossWeight { get; set; }

	public decimal BiltyFreight { get; set; }

	public string BiltyNo { get; set; }

	public string VehicleNo { get; set; }

	public int ActionTypeId { get; set; }

	public int CommTradeLoadingDeliveryDetailId { get; set; }

	public int CommTradeLoadingDeliveryId { get; set; }

	public int CommTradeOrderDetailId { get; set; }

	public int CommTradeOrderId { get; set; }

	public int CommTradeTransactionHeaderId { get; set; }

	public int CropYearId { get; set; }

	public int CurrencyId { get; set; }

	public int CusPaymentTermsId { get; set; }

	public int CusRateUomId { get; set; }

	public int CustomerId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int SortNo { get; set; }

	public int SupplierId { get; set; }

	public int SupRateUomId { get; set; }

	public int UomId { get; set; }

	public string CusDeliveryTerm { get; set; }

	public string CusRemarks { get; set; }

	public string SupRemarks { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual int ProductTypeId { get; set; }

	public virtual int LoadingDeliveryDocNo { get; set; }

	public virtual DateTime OrderDate { get; set; }

	public virtual DateTime LoadingDeliveryDate { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string TransporterName { get; set; }
}
