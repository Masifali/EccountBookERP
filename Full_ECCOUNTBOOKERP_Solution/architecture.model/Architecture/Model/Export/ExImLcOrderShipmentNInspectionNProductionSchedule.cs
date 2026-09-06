using System;

namespace Architecture.Model.Export;

public class ExImLcOrderShipmentNInspectionNProductionSchedule
{
	public DateTime? InspectionSchDate { get; set; }

	public DateTime? ProductionSchDate { get; set; }

	public DateTime? ShipmentSchDate { get; set; }

	public double ShpSchQtyMtons { get; set; }

	public int Id { get; set; }

	public int ShipmentSeq { get; set; }

	public int LcOrderId { get; set; }

	public int NoOfCntr { get; set; }

	public string SchRemarks { get; set; }

	public int ItemId { get; set; }

	public int DocNo { get; set; }

	public string PackingRemarks { get; set; }

	public string LoadingRemarks { get; set; }

	public int LoadingUserId { get; set; }

	public string LabRemarks { get; set; }

	public int LabUserId { get; set; }

	public string ProductionRemarks { get; set; }

	public int ProdcutionUserId { get; set; }

	public string ExportRemarks { get; set; }

	public string LotNo { get; set; }

	public string LineStatus { get; set; }

	public int ExportUserId { get; set; }

	public virtual string ItemName { get; set; }
}
