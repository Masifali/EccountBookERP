using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImGoodsReceiptsAtPort
{
	public DateTime EnteryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal QtyReceived { get; set; }

	public decimal WeightReceived { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EnteryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int ExImForwardingId { get; set; }

	public int ExImForwardingPackingDetailId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int SortNo { get; set; }

	public int WarehouseId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual List<ExImGoodsReceiptsAtPort> ExImGoodsReceiptsAtPortsList { get; set; }
}
