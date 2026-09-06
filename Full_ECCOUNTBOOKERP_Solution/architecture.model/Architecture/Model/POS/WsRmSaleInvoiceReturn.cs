using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.POS;

public class WsRmSaleInvoiceReturn
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double BillAmount { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryTermsId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefInvoiceDocNo { get; set; }

	public int RefInvoiceId { get; set; }

	public int SalesPersonId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string IsAttachments { get; set; }

	public string ManualBillNo { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<WsRmSaleInvoiceReturnDetail> WsRmSaleInvoiceReturnDetailslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }
}
