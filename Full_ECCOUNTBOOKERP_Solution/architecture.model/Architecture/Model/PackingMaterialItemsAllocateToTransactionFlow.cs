using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class PackingMaterialItemsAllocateToTransactionFlow
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int TransactionFlowId { get; set; }

	public virtual string ProductType { get; set; }

	public virtual List<PackingMaterialItemsAllocateToTransactionFlow> PackingMaterialItemsAllocateToTransactionFlowList { get; set; }
}
