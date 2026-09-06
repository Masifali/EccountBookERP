using System;
using System.Collections.Generic;
using Architecture.Model.Inventory;

namespace Architecture.Model.pcc;

public class InvGrn
{
	public long RowVersion { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime GpDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal CarriageAmount { get; set; }

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

	public int ProjectsId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int TransporterSupCustId { get; set; }

	public string BiltyNo { get; set; }

	public string DeliveryTerm { get; set; }

	public string DriverCellNo { get; set; }

	public string DriverCNICNo { get; set; }

	public string DriverName { get; set; }

	public string ReferenceNo { get; set; }

	public string RemarksHeader { get; set; }

	public string TransporterRemarks { get; set; }

	public string VehicleNo { get; set; }

	public int VehicleTypeId { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string TransporterName { get; set; }

	public virtual string VehicleType { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<InvGrnDetail> InvGrnDetailList { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
