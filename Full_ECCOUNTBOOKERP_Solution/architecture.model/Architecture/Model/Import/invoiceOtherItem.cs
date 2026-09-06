using System;

namespace Architecture.Model.Import;

public class invoiceOtherItem
{
	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal NetWeightKgs { get; set; }

	public double ItemRate { get; set; }

	public int appActionId { get; set; }

	public int approvedUserId { get; set; }

	public int createdUserId { get; set; }

	public long invoiceOtherItemId { get; set; }

	public int ItemId { get; set; }

	public int locationBranchId { get; set; }

	public int WareHouseFromId { get; set; }

	public long invoiceMasterId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long RowVersionLong { get; set; }

	public string ContainerNo { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }
}
