using System;
using System.Collections.Generic;

namespace Architecture.Model.Steel.Inventory;

public class InvDeliveryOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal DoTotalQty { get; set; }

	public decimal TotalGrossWeight { get; set; }

	public decimal TotalNetWeight { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryOrderType { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectsId { get; set; }

	public int ToBranchId { get; set; }

	public string VehicleNo { get; set; }

	public string LoadingInstructions { get; set; }

	public string MannualNo { get; set; }

	public string VehicleTypeId { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<InvDeliveryOrderDetail> InvDeliveryOrderDetail { get; set; }
}
