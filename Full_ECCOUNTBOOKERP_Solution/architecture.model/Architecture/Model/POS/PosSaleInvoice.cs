using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.POS;

public class PosSaleInvoice
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double BillAmount { get; set; }

	public double AddLess { get; set; }

	public double CashReceived { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DocumentTypeSrNo { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int SalesPersonId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int SupplierReferenceNo { get; set; }

	public int DueDays { get; set; }

	public int PaymentTermsId { get; set; }

	public string IsAttachments { get; set; }

	public string IsTaxable { get; set; }

	public string ManualBillNo { get; set; }

	public string RemarksHeader { get; set; }

	public DateTime DocTime { get; set; }

	public DateTime DueDate { get; set; }

	public string CustomerMobileNumber { get; set; }

	public string CustomerName { get; set; }

	public virtual int ItemId { get; set; }

	public virtual List<PosSaleInvoiceDetail> PosSaleInvoiceDetailList { get; set; }

	public virtual List<PosSaleInvoiceDetailXGetYFree> PosSaleInvoiceDetailXGetYFreeslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
