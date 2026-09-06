using System;
using System.Collections.Generic;

namespace Architecture.Model.pcc;

public class InvDeliveryOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal DoTotalQty { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryOrderTypeId { get; set; }

	public int DocNo { get; set; }

	public string Distance { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int VehicleTypeId { get; set; }

	public string RefrenenceNo { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public virtual string VehicleType { get; set; }

	public virtual List<InvDeliveryOrderdetail> InvDeliveryOrderDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
