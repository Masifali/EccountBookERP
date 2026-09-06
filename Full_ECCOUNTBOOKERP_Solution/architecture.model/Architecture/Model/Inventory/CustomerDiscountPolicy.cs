using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class CustomerDiscountPolicy
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public double DiscountRate { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int DiscountItemId { get; set; }

	public int DiscountTypeId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string DiscountType { get; set; }

	public virtual List<CustomerDiscountPolicy> CustomerDiscountPolicieslist { get; set; }
}
