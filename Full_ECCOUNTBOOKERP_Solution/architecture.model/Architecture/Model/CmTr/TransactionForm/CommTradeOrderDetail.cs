using System;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeOrderDetail
{
	public DateTime dDocDate { get; set; }

	public decimal CusAmount { get; set; }

	public decimal CusDueDays { get; set; }

	public decimal CusNetWeight { get; set; }

	public decimal CusRate { get; set; }

	public decimal CustFcyAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal SupAmount { get; set; }

	public decimal SupDueDays { get; set; }

	public decimal SupFcyAmount { get; set; }

	public decimal SupNetWeight { get; set; }

	public decimal SupRate { get; set; }

	public int ActionTypeId { get; set; }

	public int CommTradeOrderId { get; set; }

	public int CurrencyId { get; set; }

	public int CusPaymentTermsId { get; set; }

	public int CusRateUomId { get; set; }

	public int CustomerId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int Qty { get; set; }

	public int SortNo { get; set; }

	public int SupplierId { get; set; }

	public int SupRateUomId { get; set; }

	public int UomId { get; set; }

	public string CropYearId { get; set; }

	public string CusDeliveryTerm { get; set; }

	public string CusRemarks { get; set; }

	public string SupRemarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string CustomerName { get; set; }
}
