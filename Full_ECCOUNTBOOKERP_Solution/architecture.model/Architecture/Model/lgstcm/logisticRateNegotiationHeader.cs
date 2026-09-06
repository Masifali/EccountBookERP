using System;
using System.Collections.Generic;

namespace Architecture.Model.lgstcm;

public class logisticRateNegotiationHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime documentDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double fcyExchangeRate { get; set; }

	public double transactionExchangeRate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DealStatusId { get; set; }

	public int documentNo { get; set; }

	public int documentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int fcyCurrencyId { get; set; }

	public int FinancialYearId { get; set; }

	public int logisticRateNegotiationHeaderId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int ServiceTypeId { get; set; }

	public int transactionCurrencyId { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string RemarksHeader { get; set; }

	public int CityFromId { get; set; }

	public int CityToId { get; set; }

	public virtual string ServiceType { get; set; }

	public virtual string TransactionCurrencyCode { get; set; }

	public virtual string DealStatus { get; set; }

	public virtual string CityFrom { get; set; }

	public virtual string CityTo { get; set; }

	public virtual List<logisticRateNegotiationDetail> logisticRateNegotiationDetailList { get; set; }

	public virtual List<logisticRateNegotiationSourceDocument> logisticRateNegotiationSourceDocumentList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
