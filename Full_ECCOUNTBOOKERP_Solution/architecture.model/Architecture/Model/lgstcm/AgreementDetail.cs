using System;

namespace Architecture.Model.lgstcm;

public class AgreementDetail
{
	public bool isActive { get; set; }

	public DateTime effectiveDateFrom { get; set; }

	public DateTime effectiveDateTo { get; set; }

	public DateTime partyRefDocDate { get; set; }

	public double fcyAmount { get; set; }

	public double lcyAmount { get; set; }

	public double serviceRate { get; set; }

	public double tcyAmount { get; set; }

	public double transactionExchangeRate { get; set; }

	public int ActionTypeId { get; set; }

	public int agreementDetailId { get; set; }

	public int AgreementHeaderId { get; set; }

	public int serviceItemId { get; set; }

	public int serviceRateBaseUomId { get; set; }

	public int transactionCurrencyId { get; set; }

	public string partyRefDocNo { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string TransactionCurrencyCode { get; set; }

	public virtual string RateBaseUom { get; set; }

	public virtual double RateBaseUomEquivalent { get; set; }
}
