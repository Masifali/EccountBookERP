using System;

namespace Architecture.Model.FeedMill.DiscountPolicy;

public class CustomerDiscountPolicyDetail
{
	public DateTime EffectiveDate { get; set; }

	public double ItemDiscountPercentage { get; set; }

	public int ActionTypeId { get; set; }

	public int CustomerDiscountPolicyId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }
}
