using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Import;

public class ImInvoice
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

	public double TotalAmount { get; set; }

	public string AddLessComments { get; set; }

	public int ApprovedUser { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryTermId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocCode { get; set; }

	public int DocumentTypeId { get; set; }

	public int EFormId { get; set; }

	public int EntryUser { get; set; }

	public int ExporteBankId { get; set; }

	public int FcurrencyId { get; set; }

	public int Id { get; set; }

	public int ImporterBankId { get; set; }

	public int LcOrderNoId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermId { get; set; }

	public int ProjectsId { get; set; }

	public int HomeCurrencyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int FinancialYearId { get; set; }

	public string CarierType { get; set; }

	public string InvoiceNo { get; set; }

	public string InvStatus { get; set; }

	public string LotNoRef { get; set; }

	public string OtherInstructions { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierCode { get; set; }

	public string TermsConditions { get; set; }

	public string EFormNo { get; set; }

	public int NotifyParty1 { get; set; }

	public int NotifyParty2 { get; set; }

	public string Certificate1 { get; set; }

	public string Certificate2 { get; set; }

	public DateTime EFormDate { get; set; }

	public virtual string PurchaseOrderNo { get; set; }

	public virtual List<ImInvoicePackingDetail> ExImInvoiceDetail { get; set; }

	public virtual List<ImInvoiceExpenses> ImInvoiceExpenseslst { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
