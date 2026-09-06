using System;
using System.Collections.Generic;

namespace Architecture.Model.Import;

public class ExImLcOrderPurchaseOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime ArrivalDate { get; set; }

	public DateTime BillOfLadingDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ETADate { get; set; }

	public DateTime ETDDate { get; set; }

	public DateTime? ExpiryDate { get; set; }

	public DateTime? LastShipmentDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime SupplierInvoiceDate { get; set; }

	public DateTime SupplierOrderDate { get; set; }

	public double Amount { get; set; }

	public double ExchangeRate { get; set; }

	public double FcyAmount { get; set; }

	public double GrossWeight { get; set; }

	public double NetWeight { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int ClearingAgentId { get; set; }

	public int CompanyId { get; set; }

	public int Containers { get; set; }

	public int DeliveryTermId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocNo { get; set; }

	public int DocNoByLcOrder { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int FcyId { get; set; }

	public int HomeCurrencyId { get; set; }

	public int Id { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermId { get; set; }

	public int ProjectsId { get; set; }

	public int ShippingAgentId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransitDays { get; set; }

	public int TransporterId { get; set; }

	public int ImporterBankId { get; set; }

	public int ExporterBankId { get; set; }

	public string BillOfLadingNo { get; set; }

	public string LcPoOrderStatus { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierInvoiceNo { get; set; }

	public string SupplierOrderNo { get; set; }

	public string Vessel { get; set; }

	public string VoyageNo { get; set; }

	public virtual string ImportPurchaseOrderNo { get; set; }

	public virtual List<ExImLcOrderPurchaseOrderDetail> EximLcorderPurchaseDetail { get; set; }

	public virtual List<ImPurchaseOrderPackingDetail> ImPurchaseOrderPackingDetailslst { get; set; }
}
