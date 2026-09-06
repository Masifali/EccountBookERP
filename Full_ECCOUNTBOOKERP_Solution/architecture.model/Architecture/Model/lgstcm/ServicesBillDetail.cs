using System;

namespace Architecture.Model.lgstcm;

public class ServicesBillDetail
{
	public DateTime partyRefDocDate { get; set; }

	public double fcyAmount { get; set; }

	public double lcyAmount { get; set; }

	public double ServiceRate { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double tcyAmount { get; set; }

	public double TotalAmount { get; set; }

	public double transactionExchangeRate { get; set; }

	public int ActionTypeId { get; set; }

	public int AgreementDetailId { get; set; }

	public int AgreementHeaderId { get; set; }

	public int ChargesTypeId { get; set; }

	public int DebitAccountId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderHeaderId { get; set; }

	public int RateBaseUomId { get; set; }

	public int serviceItemId { get; set; }

	public int ServicesBillDetailId { get; set; }

	public int ServicesBillHeaderId { get; set; }

	public int TaxNameId { get; set; }

	public int transactionCurrencyId { get; set; }

	public string partyRefDocNo { get; set; }

	public string Remarks { get; set; }

	public int LocationTypeId { get; set; }

	public int FromCityId { get; set; }

	public int ToCityId { get; set; }

	public int LoadingPortId { get; set; }

	public int DestinationPortId { get; set; }

	public double Qty { get; set; }

	public double NetWeight { get; set; }

	public double FcyExchangeRate { get; set; }

	public int FcyCurrencyId { get; set; }

	public double GlcyExchangeRate { get; set; }

	public double GlcyAmount { get; set; }

	public int GlcyCurrencyId { get; set; }

	public virtual int AgreementDocNo { get; set; }

	public virtual int PurchaseOrderDocNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string TransactionCurrencyCode { get; set; }

	public virtual string RateBaseUom { get; set; }

	public virtual double RateBaseUomEquivalent { get; set; }

	public virtual string TaxName { get; set; }

	public virtual string DebitAccountCode { get; set; }

	public virtual string DebitAccountTitle { get; set; }

	public virtual string LocationType { get; set; }

	public virtual string FromCity { get; set; }

	public virtual string ToCity { get; set; }

	public virtual string LoadingPort { get; set; }

	public virtual string DestinationPort { get; set; }

	public virtual string GlcyCurrencyCode { get; set; }

	public virtual string FcyCurrencyCode { get; set; }
}
