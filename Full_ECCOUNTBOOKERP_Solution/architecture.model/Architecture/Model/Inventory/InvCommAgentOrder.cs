using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InvCommAgentOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EnteryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BrokeryPrcnt { get; set; }

	public decimal CommPrcnt { get; set; }

	public int ApprovalUserId { get; set; }

	public int BranchesId { get; set; }

	public int BrokerId { get; set; }

	public int DocumentTypeId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocNo { get; set; }

	public int DueDays { get; set; }

	public int EnteryUserId { get; set; }

	public int Id { get; set; }

	public int InvDueTermsId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierId { get; set; }

	public string DeliveryTerm { get; set; }

	public string OrderStatus { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<InvCommAgentOrderDetail> InvCommAgentOrderDetailslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
