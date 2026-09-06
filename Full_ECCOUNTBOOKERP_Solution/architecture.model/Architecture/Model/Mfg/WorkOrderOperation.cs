using System;

namespace Architecture.Model.Mfg;

public class WorkOrderOperation
{
	public DateTime DeadLine { get; set; }

	public int ActionTypeId { get; set; }

	public int WorkOrderOperationId { get; set; }

	public decimal OperationDays { get; set; }

	public decimal OperationHours { get; set; }

	public int BOMHeaderId { get; set; }

	public int BOMOperationDetailId { get; set; }

	public decimal OpHoursPerDay { get; set; }

	public decimal Qty { get; set; }

	public int OperationSeq { get; set; }

	public int WorkOrderId { get; set; }

	public int WorkStationOperationId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public string Remarks { get; set; }

	public virtual string WorkStationName { get; set; }

	public virtual string OperationName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }
}
