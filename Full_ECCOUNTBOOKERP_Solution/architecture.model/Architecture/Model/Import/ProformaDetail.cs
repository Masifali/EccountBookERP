using System;

namespace Architecture.Model.Import;

public class ProformaDetail
{
	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public double FcAmount { get; set; }

	public double ItemRate { get; set; }

	public double netWeightInner { get; set; }

	public double netWeightOuter { get; set; }

	public double qtyInner { get; set; }

	public double qtyOuter { get; set; }

	public double NetWeightKgs { get; set; }

	public int appActionId { get; set; }

	public int approvedUserId { get; set; }

	public int ItemId { get; set; }

	public int packingTypeIdInner { get; set; }

	public int packingTypeIdOuter { get; set; }

	public int packSizeIdinner { get; set; }

	public int packSizeIdOuter { get; set; }

	public long proformaMasterId { get; set; }

	public int uomIdRate { get; set; }

	public long appUserLogId { get; set; }

	public long createdUserId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long proformaDetailId { get; set; }

	public long RowVersionLong { get; set; }

	public string ItemDescription { get; set; }

	public string RemarksDetail { get; set; }

	public int locationBranchId { get; set; }

	public int lotJobId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackingTypeOuter { get; set; }

	public virtual string PackingTypeInner { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string OuterUOM { get; set; }

	public virtual double OuterEquivalent { get; set; }

	public virtual string InnerUOM { get; set; }

	public virtual double InnerEquivalent { get; set; }

	public virtual string RateUOM { get; set; }

	public virtual double RateEquivalent { get; set; }
}
