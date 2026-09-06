namespace Architecture.Model.Mfg;

public class InvStockTransferDetail
{
	public bool IsApproved { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal NetWeight { get; set; }

	public int ActionTypeId { get; set; }

	public int Id { get; set; }

	public int InvStockTransferHeaderId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int ItemVariantId { get; set; }

	public int LineId { get; set; }

	public int ProductionStageFromId { get; set; }

	public int ProductionStageToId { get; set; }

	public int RateUomId { get; set; }

	public int WarehouseFromId { get; set; }

	public int WarehouseToId { get; set; }

	public int WorkOrderId { get; set; }

	public int WorkStationFromId { get; set; }

	public int WorkStationToId { get; set; }

	public int CastingTypeId { get; set; }

	public int JobLotId { get; set; }

	public string BatchDescription { get; set; }

	public string RemarksDetail { get; set; }

	public int RejectionReasonTypeId { get; set; }

	public int TeamWorkerNameId { get; set; }

	public string TeamWorkerName { get; set; }

	public string RejectionReasonRemarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string VariantDescription { get; set; }

	public virtual string ItemUom { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string RejectionReasonType { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual string WarehouseFrom { get; set; }

	public virtual string WarehouseTo { get; set; }

	public virtual string ProductionStageFrom { get; set; }

	public virtual string ProductionStageTo { get; set; }

	public virtual string WorkStationFrom { get; set; }

	public virtual string WorkStationTo { get; set; }

	public virtual string CastingType { get; set; }
}
