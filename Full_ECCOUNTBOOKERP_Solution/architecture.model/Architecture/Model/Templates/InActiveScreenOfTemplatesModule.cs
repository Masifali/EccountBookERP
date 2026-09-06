using System;
using System.Collections.Generic;

namespace Architecture.Model.Templates;

public class InActiveScreenOfTemplatesModule
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int ModuleId { get; set; }

	public int OrganizationTemplateId { get; set; }

	public string ScreenName { get; set; }

	public virtual List<InActiveScreenOfTemplatesModule> InActiveScreenOfTemplatesModuleList { get; set; }
}
