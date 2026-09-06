using System;
using System.Collections.Generic;

namespace Architecture.Model.pcc;

public class PurchaseOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal CommissionRate { get; set; }

	public decimal CommissionUom { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal OrderAmount { get; set; }

	public decimal OrderQty { get; set; }

	public decimal OrderWeight { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierCustomerId { get; set; }

	public long RowVersion { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string OrderStatus { get; set; }

	public string RemarksHeader { get; set; }

	public string ReferenceNo { get; set; }

	public virtual List<PurchaseOrderDetail> PurchaseOrderDetaillist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
