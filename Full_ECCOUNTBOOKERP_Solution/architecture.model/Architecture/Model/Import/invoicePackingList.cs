using System;
using System.Collections.Generic;

namespace Architecture.Model.Import;

public class invoicePackingList
{
	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public double netWeightInner { get; set; }

	public double netWeightOuter { get; set; }

	public double qtyInner { get; set; }

	public double qtyOuter { get; set; }

	public double NetWeightKgs { get; set; }

	public int appActionId { get; set; }

	public int JobLotId { get; set; }

	public int approvedUserId { get; set; }

	public int createdUserId { get; set; }

	public int ItemId { get; set; }

	public int locationBranchId { get; set; }

	public int locOrderMasterId { get; set; }

	public int packingTypeIdInner { get; set; }

	public int packingTypeIdOuter { get; set; }

	public int packSizeIdinner { get; set; }

	public int packSizeIdOuter { get; set; }

	public long invoiceDetailId { get; set; }

	public long invoiceMasterId { get; set; }

	public long invoicePackingListId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long locOrderDetailId { get; set; }

	public long rowVersionLong { get; set; }

	public string containerNo { get; set; }

	public string ItemDescription { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual string JobLot { get; set; }

	public virtual List<invoicePackingList> invoicePackingListDetail { get; set; }
}
