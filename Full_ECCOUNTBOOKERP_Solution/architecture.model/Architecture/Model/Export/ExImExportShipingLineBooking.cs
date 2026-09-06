using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImExportShipingLineBooking
{
	public DateTime BookingDate { get; set; }

	public DateTime? ETADate { get; set; }

	public DateTime? ETDDate { get; set; }

	public DateTime? CuttOFFDate { get; set; }

	public int ContrsQty { get; set; }

	public int TransitDays { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int ShippingAgentId { get; set; }

	public int TransporterId { get; set; }

	public int ShippingLineId { get; set; }

	public int CarierMediumId { get; set; }

	public string BookingCroNo { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public int FreeDaysAtDestinations { get; set; }

	public string RemarksHeader { get; set; }

	public string VesselName { get; set; }

	public string VoyageNo { get; set; }

	public int PortId { get; set; }

	public int ContainerDispatchedAtPortId { get; set; }

	public int ContainerReceivedFromPortId { get; set; }

	public int DeliveredAtId { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int EntryUser { get; set; }

	public decimal BookingRate { get; set; }

	public DateTime? EntryDate { get; set; }

	public int ModifyUser { get; set; }

	public DateTime? ModifyDate { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual List<ExImExportShipingLineBookingDetail> ExImExportShipingLineBookingDetailslist { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
