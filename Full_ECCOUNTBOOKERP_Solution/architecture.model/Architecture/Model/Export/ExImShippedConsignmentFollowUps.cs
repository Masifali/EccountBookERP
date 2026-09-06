using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImShippedConsignmentFollowUps
{
	public DateTime? DocCourierToPartyDate { get; set; }

	public DateTime? DocToBankDate { get; set; }

	public DateTime? LoadOnTrainDate { get; set; }

	public DateTime? ReachedAtLoadingPortDate { get; set; }

	public DateTime? LoadedOnVesselDate { get; set; }

	public DateTime? ETDFinalDate { get; set; }

	public DateTime? GdDate { get; set; }

	public DateTime? PortPayment { get; set; }

	public DateTime? CuttOfDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? ETDLoadingPort { get; set; }

	public int EntryUserId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int CurrentStatusId { get; set; }

	public int DocumentStatusId { get; set; }

	public string CourierTracingNo { get; set; }

	public string GdNo { get; set; }

	public DateTime? ReachedAtDestinationPort { get; set; }

	public virtual List<ExImShippedConsignmentFollowUps> ShippedConsignmentFollowUpList { get; set; }
}
