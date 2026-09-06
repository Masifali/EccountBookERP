using System;
using System.Collections.Generic;

namespace Architecture.Model.Main.Commission;

public class CustomerCommissionPolicy
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public double CommissionRate { get; set; }

	public int CommissionItemsId { get; set; }

	public int CommissionTypeId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int PolicyTypeId { get; set; }

	public int CustomerIncentivePartyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string CommissionType { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public virtual List<CustomerCommissionPolicy> CustomerCommissionPolicyList { get; set; }
}
