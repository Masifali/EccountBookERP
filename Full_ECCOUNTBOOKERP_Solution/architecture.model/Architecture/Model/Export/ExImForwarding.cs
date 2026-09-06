using System;
using System.Collections.Generic;
using Architecture.Model.Inventory;

namespace Architecture.Model.Export;

public class ExImForwarding
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime GPDate { get; set; }

	public DateTime? LotCompletedDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double FreightAmt { get; set; }

	public double OtherCharges { get; set; }

	public double FactoryWeight { get; set; }

	public int ApprovedUser { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int ExImInvoiceId { get; set; }

	public int ExportSoNoId { get; set; }

	public int GatePassOutwardId { get; set; }

	public int Id { get; set; }

	public int LCOrderId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfContainer { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int ShippedContainer { get; set; }

	public int ShippingAgentId { get; set; }

	public int AutoUpdateId { get; set; }

	public int ShippingLineId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int CarrierTypeId { get; set; }

	public int FinancialYearId { get; set; }

	public int ActionId { get; set; }

	public string BiltyNo { get; set; }

	public string Container { get; set; }

	public string Container1 { get; set; }

	public string DocAttachment { get; set; }

	public string DriverCellNo { get; set; }

	public string DriverCnicNo { get; set; }

	public string DriverName { get; set; }

	public string GpNo { get; set; }

	public string LotStatus { get; set; }

	public string OtherRemarks { get; set; }

	public string Status { get; set; }

	public string VehicleNo { get; set; }

	public string ReferenceNo { get; set; }

	public virtual string LcOrderNo { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string Customer { get; set; }

	public virtual string LoadingPort { get; set; }

	public virtual string DestinationPort { get; set; }

	public virtual int GpSrNo { get; set; }

	public virtual List<ExImForwardingPackingDetail> ExImForwardingDetail { get; set; }

	public virtual List<ExImForwardingOtherItems> ExImForwardingOtherItemslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public ExImForwarding()
	{
		ExImForwardingDetail = new List<ExImForwardingPackingDetail>();
		ExImForwardingOtherItemslist = new List<ExImForwardingOtherItems>();
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
	}
}
