using System;

namespace Architecture.Model.Mfg;

public class WorkOrderInProgressDetail
{
	public DateTime WsOpCompleteDate { get; set; }

	public DateTime WsOpStartDate { get; set; }

	public DateTime WsOpCompleteTime { get; set; }

	public DateTime WsOpStartTime { get; set; }

	public decimal Qty { get; set; }

	public int ActionTypeId { get; set; }

	public int WorkOrderInProgressDetailId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int SortNo { get; set; }

	public int WorkOrderInProgressId { get; set; }

	public int WorkOrderOperationId { get; set; }

	public int WorkStationOperationId { get; set; }

	public string Remarks { get; set; }

	public virtual string WorkStationName { get; set; }

	public virtual string OperationName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }
}
