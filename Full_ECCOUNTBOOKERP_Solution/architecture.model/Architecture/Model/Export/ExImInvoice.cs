using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Export;

public class ExImInvoice
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double BuyerGLValueRs { get; set; }

	public double ConversionRate { get; set; }

	public double EquivalentAmount { get; set; }

	public double FCurrencyAmount { get; set; }

	public double FobValue { get; set; }

	public double GrossWeight { get; set; }

	public double InvoiceCommercialValue { get; set; }

	public double NetWeight { get; set; }

	public double NoOfContainers { get; set; }

	public double AddLessAmount { get; set; }

	public double CommissionPercentage { get; set; }

	public double CommissionAmount { get; set; }

	public double TotalAmount { get; set; }

	public decimal GrossMton { get; set; }

	public decimal NetMton { get; set; }

	public string AddLessComments { get; set; }

	public int CommercialInvoiceId { get; set; }

	public int ApprovedUser { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int PrefixTypeId { get; set; }

	public int DeliveryTermId { get; set; }

	public int ContinentId { get; set; }

	public int OriginCountryId { get; set; }

	public int ImporterCountryId { get; set; }

	public int PlaceOfDeliveryId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocCode { get; set; }

	public int DocumentTypeId { get; set; }

	public int EFormId { get; set; }

	public int EntryUser { get; set; }

	public int ExporteBankId { get; set; }

	public int ExportCompanyId { get; set; }

	public int FcurrencyId { get; set; }

	public int Id { get; set; }

	public int ImporterBankId { get; set; }

	public int LcOrderNoId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermId { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int CommissionAgentId { get; set; }

	public int FinancialYearId { get; set; }

	public int PreInvoiceId { get; set; }

	public int CommissionDebitAcId { get; set; }

	public int OtherCustomerId { get; set; }

	public int OtherDestinationPortId { get; set; }

	public int ConsigneeId { get; set; }

	public int ActionId { get; set; }

	public string CarierType { get; set; }

	public string InvoiceNo { get; set; }

	public string InvStatus { get; set; }

	public string LotNoRef { get; set; }

	public string OtherInstructions { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierCode { get; set; }

	public string TermsConditions { get; set; }

	public string EFormNo { get; set; }

	public string ContractIds { get; set; }

	public string ContractDetailIds { get; set; }

	public string ContractNos { get; set; }

	public string ContractScheduleIds { get; set; }

	public string ContractScheduleNos { get; set; }

	public string ContractDates { get; set; }

	public string DocumentaryCreditNo { get; set; }

	public DateTime? DocumentaryCreditNoIssueDate { get; set; }

	public string PaymentRemarks { get; set; }

	public string DeliveryRemarks { get; set; }

	public string TCPRegNo { get; set; }

	public int NotifyParty1 { get; set; }

	public int NotifyParty2 { get; set; }

	public int NotifyParty3 { get; set; }

	public int CreditAccountId { get; set; }

	public int SubContractExpiryDays { get; set; }

	public string Certificate1 { get; set; }

	public string Certificate2 { get; set; }

	public string Remarks1 { get; set; }

	public string Remarks2 { get; set; }

	public string OtherRemarks1 { get; set; }

	public string OtherRemarks2 { get; set; }

	public string CustomerContractNos { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public DateTime EFormDate { get; set; }

	public virtual string GDNo { get; set; }

	public virtual DateTime? GDDate { get; set; }

	public virtual int ExportVoucherId { get; set; }

	public virtual int ForwardingId { get; set; }

	public virtual List<ExImInvoicePackingDetail> ExImInvoiceDetail { get; set; }

	public virtual List<ExImInvoicePackingDetailCustom> ExImInvoiceDetailCustom { get; set; }

	public virtual List<ExImInvoicePaymentTermsDetail> ExImInvoicePaymentTermsDetail { get; set; }

	public virtual List<ExImInvoicePaymentTermsDetailCustom> ExImInvoicePaymentTermsDetailCustom { get; set; }

	public virtual List<ExImInvoiceOtherChargesDetail> ExImInvoiceOtherChargesDetailList { get; set; }

	public virtual List<ExImInvoiceOtherChargesDetailCustom> ExImInvoiceOtherChargesDetailCustomList { get; set; }

	public virtual List<ExImInvoicePackingListDetail> ExImInvoicePackingListDetailList { get; set; }

	public virtual List<ExImInvoicePackingListDetailCustom> ExImInvoicePackingListDetailCustomList { get; set; }

	public virtual List<ExImInvoiceOtherItems> ExImInvoiceOtherItemslist { get; set; }

	public virtual List<ExImPackingListDetail> ExImPackingListDetailList { get; set; }

	public virtual List<ExImInvoiceSaleManCommission> ExImInvoiceSaleManCommissionList { get; set; }

	public virtual VoucherHead VoucherHeadExImInvoices { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
