using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ProductionFromPaddyManualRate
{
	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal RateUom { get; set; }

	public decimal Weight { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public virtual List<ProductionFromPaddyManualRate> ProductionFromPaddyManualRatesList { get; set; }
}
