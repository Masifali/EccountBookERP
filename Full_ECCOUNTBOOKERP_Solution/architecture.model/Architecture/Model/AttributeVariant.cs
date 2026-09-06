using System;

namespace Architecture.Model;

public class AttributeVariant
{
	public bool isActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public long attributeId { get; set; }

	public long attributeVariantId { get; set; }

	public int companyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int organizationId { get; set; }

	public int seqNo { get; set; }

	public long variantId { get; set; }

	public string VariantDescription { get; set; }

	public string variantName { get; set; }

	public virtual string AttributeName { get; set; }
}
