using System;

namespace Architecture.Model.Inventory;

public class DiscountPolicy
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public double DiscountRate { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string DiscountType { get; set; }

	public string PolicyName { get; set; }
}
