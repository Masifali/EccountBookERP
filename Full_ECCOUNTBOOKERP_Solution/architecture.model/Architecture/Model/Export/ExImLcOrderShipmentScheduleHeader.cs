using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImLcOrderShipmentScheduleHeader
{
	public bool IsApproved { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? ShipmentStartDate { get; set; }

	public DateTime LastShipmentDate { get; set; }

	public double NetWeight { get; set; }

	public int EntryUser { get; set; }

	public int ExImLcOrderId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfContainers { get; set; }

	public int NoOfShipments { get; set; }

	public int ProductionSchDaysBefore { get; set; }

	public int SampleSchDaysBefore { get; set; }

	public int ShipmentIntervalDays { get; set; }

	public string ShipmentStatus { get; set; }

	public int SupplierCustomerId { get; set; }

	public string ScheduleType { get; set; }

	public virtual List<ExImLcOrderDocumentList> LcOrderDocumentDetail { get; set; }

	public virtual List<ExImLcOrderAnalysisTestList> LcOrderAnalysisTestDetail { get; set; }

	public virtual List<ExImLcOrderShipmentNInspectionNProductionSchedule> LcOrderShipmentNInspectioNProductionScheduleDetail { get; set; }
}
