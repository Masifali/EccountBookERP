using System;

namespace Architecture.Model.Inventory;

public class InvPurchaseDemandDetail
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime LastPurchaseDate { get; set; }

	public double lastPurchaseQty { get; set; }

	public double LastPurchaseRate { get; set; }

	public double RequiredQty { get; set; }

	public double ApprovedQty { get; set; }

	public int ApprovedUserId { get; set; }

	public int DepartmentId { get; set; }

	public int FixedAssetsRegisterId { get; set; }

	public int Id { get; set; }

	public int InvPurchaseDemandHeaderId { get; set; }

	public int ItemId { get; set; }

	public int ItemSchuomId { get; set; }

	public int JobLotId { get; set; }

	public int ItemConditionId { get; set; }

	public string RequestBy { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string DepartmentName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMDescription { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string AssetName { get; set; }

	public virtual string ItemCategory { get; set; }

	public virtual string ItemType { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual double AvaialableStockQty { get; set; }

	public virtual double OutstandingDemandAndPOQty { get; set; }
}
