using System;

namespace Architecture.Model.lgstcm;

public class logisticRateNegotiationDetail
{
	public DateTime cutOffDate { get; set; }

	public DateTime OfferValidityDate { get; set; }

	public DateTime etdPOL { get; set; }

	public double fcyAmount { get; set; }

	public double lcyAmount { get; set; }

	public double serviceRate { get; set; }

	public double tcyAmount { get; set; }

	public double tcyExchangeRate { get; set; }

	public int ActionTypeId { get; set; }

	public int DueDays { get; set; }

	public int freeDaysAtPOD { get; set; }

	public int logisticRateNegotiationDetailId { get; set; }

	public int logisticRateNegotiationHeaderId { get; set; }

	public int OffervalidityDays { get; set; }

	public int partyIdAgent { get; set; }

	public int paymentTermId { get; set; }

	public int portOfDischargeId { get; set; }

	public int portOfLoadingId { get; set; }

	public int ServiceItemId { get; set; }

	public int serviceProviderId { get; set; }

	public int serviceRateBaseUomId { get; set; }

	public int serviceTypeId { get; set; }

	public int tcyCurrencyId { get; set; }

	public int VesselRoutingTypeId { get; set; }

	public int vesselTransitDays { get; set; }

	public string Remarks { get; set; }

	public string VesselRoutingDescription { get; set; }

	public int CityFromId { get; set; }

	public int CityToId { get; set; }

	public virtual string ServiceType { get; set; }

	public virtual string BrokerAgentName { get; set; }

	public virtual string ServiceProviderName { get; set; }

	public virtual string PaymentTerm { get; set; }

	public virtual string VesselRoutingType { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string TransactionCurrencyCode { get; set; }

	public virtual string RateBaseUom { get; set; }

	public virtual double RateBaseUomEquivalent { get; set; }

	public virtual string DestinationPort { get; set; }

	public virtual string LoadingPort { get; set; }

	public virtual string CityFrom { get; set; }

	public virtual string CityTo { get; set; }
}
