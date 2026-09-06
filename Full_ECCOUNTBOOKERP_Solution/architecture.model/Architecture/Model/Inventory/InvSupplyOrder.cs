using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InvSupplyOrder
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime? DocDate { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public string BiltyNos { get; set; }

	public string DestinationsLocation { get; set; }

	public string ManualRefNo { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplyOrderStatus { get; set; }

	public string VehicleNos { get; set; }

	public virtual List<InvSupplyOrderDetail> InvSupplyOrderDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
