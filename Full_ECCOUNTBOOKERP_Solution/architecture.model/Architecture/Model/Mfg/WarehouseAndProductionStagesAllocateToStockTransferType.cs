using System;

namespace Architecture.Model.Mfg;

public class WarehouseAndProductionStagesAllocateToStockTransferType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FromProductionStageId { get; set; }

	public int FromWarehouseId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ToProductionStageId { get; set; }

	public int ToWarehouseId { get; set; }

	public int TransferTypeId { get; set; }

	public virtual string TransferType { get; set; }

	public virtual string FromWarehouse { get; set; }

	public virtual string ToWarehouse { get; set; }

	public virtual string FromProductionStage { get; set; }

	public virtual string ToProductionStage { get; set; }
}
