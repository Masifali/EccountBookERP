using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.Export;

public class ExportReturnGrn
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime GPDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal FreightAmount { get; set; }

	public decimal NoOfContainer { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int FinancialYearId { get; set; }

	public int GPId { get; set; }

	public int ExImForwardingId { get; set; }

	public int GatePassOutwardId { get; set; }

	public int Id { get; set; }

	public int LCOrderId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectsId { get; set; }

	public int ShippingAgentId { get; set; }

	public int ShippingLineId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public string BiltyNo { get; set; }

	public string Container { get; set; }

	public string Container1 { get; set; }

	public string DriverCellNo { get; set; }

	public string DriverCnicNo { get; set; }

	public string DriverName { get; set; }

	public string GpNo { get; set; }

	public string ReferenceNo { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string LcOrderNo { get; set; }

	public virtual string LoadingPort { get; set; }

	public virtual string DestinationPort { get; set; }

	public virtual decimal GpFactoryWeight { get; set; }

	public virtual VoucherHead VoucherHeadExportReturnGrn { get; set; }

	public virtual List<ExportReturnGrnDetail> ExportReturnGrnDetailList { get; set; }

	public virtual List<ExportReturnGrnOtherItems> ExportReturnGrnOtherItemsList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
