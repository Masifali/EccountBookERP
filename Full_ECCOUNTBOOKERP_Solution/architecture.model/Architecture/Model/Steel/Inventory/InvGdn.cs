using System;
using System.Collections.Generic;

namespace Architecture.Model.Steel.Inventory;

public class InvGdn
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal CarriageAmount { get; set; }

	public decimal FactoryWeight { get; set; }

	public decimal PartyWeight { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int OutwardGatePassId { get; set; }

	public int ProjectsId { get; set; }

	public int ReferencePartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int VehicleTypeId { get; set; }

	public string BiltyNo { get; set; }

	public string DeliveryTerm { get; set; }

	public string PartyReference { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public virtual string OrderType { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<InvGdnDetail> invGdnDetail { get; set; }
}
