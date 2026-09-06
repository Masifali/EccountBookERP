using System;
using System.Collections.Generic;

namespace Architecture.Model.SDT;

public class DocumentOfGroup
{
	public bool IsApproved { get; set; }

	public bool isActive { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int afterDays { get; set; }

	public int ApprovedUserId { get; set; }

	public int beforeDays { get; set; }

	public int chartOfDocumentId { get; set; }

	public int RequiredLevelId { get; set; }

	public int CompanyId { get; set; }

	public int CriteriaDateTypeId { get; set; }

	public int customGroupId { get; set; }

	public int documentOfGroupId { get; set; }

	public int documentProviderId { get; set; }

	public int duplicate { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int original { get; set; }

	public int prioritySeqNo { get; set; }

	public string Remarks { get; set; }

	public virtual string customGroupName { get; set; }

	public virtual string documentName { get; set; }

	public virtual string DocProviderName { get; set; }

	public virtual string criteriaDateTypeDesc { get; set; }

	public virtual string requiredLevelDesc { get; set; }

	public virtual List<DocumentOfGroup> DocumentOfGroupList { get; set; }
}
