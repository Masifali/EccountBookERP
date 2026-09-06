using System.Collections.Generic;

namespace Architecture.Model.Templates;

public class OrganizationTemplateRights
{
	public int Id { get; set; }

	public int OrganizationTemplateId { get; set; }

	public int ScreenId { get; set; }

	public virtual List<OrganizationTemplateRights> OrgTemplatesRightsList { get; set; }
}
