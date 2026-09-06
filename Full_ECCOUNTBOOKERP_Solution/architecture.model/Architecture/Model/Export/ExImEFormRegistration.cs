using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImEFormRegistration
{
	public bool PostState { get; set; }

	public DateTime EFormDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double AmountRs { get; set; }

	public double EFormValueTotal { get; set; }

	public double ExchangeRate { get; set; }

	public double NetWeightTotal { get; set; }

	public int CompanyId { get; set; }

	public int ConsigneeId { get; set; }

	public int CountryId { get; set; }

	public int DeliveryTermId { get; set; }

	public int DestinationPortId { get; set; }

	public int EntryUser { get; set; }

	public int FcurrencyId { get; set; }

	public int Id { get; set; }

	public int IssuingBankId { get; set; }

	public int DocumentTypeId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string EFormNo { get; set; }

	public string IbanNo { get; set; }

	public string TradeType { get; set; }

	public string ConsigneeIban { get; set; }

	public string ConsigneeAddress { get; set; }

	public DateTime ExpiryDate { get; set; }

	public bool Status { get; set; }

	public double BalanceAmount { get; set; }

	public int PaymenttermId { get; set; }

	public virtual List<ExImEFormRegistrationAdvancePaymentUtilizationDetail> EFormAdvancePaymentUtilizationDetail { get; set; }

	public virtual List<ExImEFormRegistrationComodityDetail> EFormRegistrationComodityDetail { get; set; }

	public virtual List<ExImEFormRegistrationPaymentTerms> EFormRegistrationPaymentTerms { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
