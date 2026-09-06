using System;
using System.Collections.Generic;

namespace Architecture.Model.FeedMill.DiscountPolicy;

public class CustomerDiscountPolicy
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double FrieghtDiscount { get; set; }

	public double MasterDiscountPercentage { get; set; }

	public double UnLoadingDiscount { get; set; }

	public int DiscountDebitAccountId { get; set; }

	public int UnloadingAccountId { get; set; }

	public int FreightAccountId { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public virtual List<CustomerDiscountPolicyDetail> CustomerDiscountPolicyDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
