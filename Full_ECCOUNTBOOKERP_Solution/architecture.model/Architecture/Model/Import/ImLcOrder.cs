using System;
using System.Collections.Generic;

namespace Architecture.Model.Import;

public class ImLcOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public DateTime LastShipmentDate { get; set; }

	public DateTime LcOrderDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal GrossWeightKgs { get; set; }

	public decimal LcyAmount { get; set; }

	public decimal NetWeightKgs { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryTermId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ExporterBankId { get; set; }

	public int FcurrencyId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ImporterBankId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUserId { get; set; }

	public int NoOfContainers { get; set; }

	public int NotifyPartyId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermId { get; set; }

	public int ProjectsId { get; set; }

	public int SalesPersonId { get; set; }

	public int ShipedToId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string CommodityDetial { get; set; }

	public string ExpiryPlace { get; set; }

	public string InquiryReference { get; set; }

	public string InsepctionDescription { get; set; }

	public string InsepctionRequired { get; set; }

	public string LcOrderNo { get; set; }

	public string LegalizationDescription { get; set; }

	public string LegalizationRequired { get; set; }

	public string PartialShipment { get; set; }

	public string QuotReference { get; set; }

	public string RemarksHeader { get; set; }

	public string ShippingMarks { get; set; }

	public string Status { get; set; }

	public string TransShipment { get; set; }

	public virtual List<ImLcOrderPackingDetail> ImLcOrderPackingDetailList { get; set; }

	public virtual List<ImLcOrderPaymnetTerm> ImLcOrderPaymnetTermList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
