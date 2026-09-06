using System;

namespace Architecture.Model.FeedMill.DiscountPolicy;

public class CustomerCommissionAgentPolicy
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double CommissionRate { get; set; }

	public int CommissionAgentId { get; set; }

	public int CommissionTypeId { get; set; }

	public int CommissionDebitAccountId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }
}
