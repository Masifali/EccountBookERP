using System.Collections.Generic;

namespace Architecture.Model.OrganizationRegistration;

public class OrganizationTemplateRights
{
	public int Id { get; set; }

	public int OrganizationTemplateId { get; set; }

	public int ScreenId { get; set; }

	public virtual string ScreenIds { get; set; }

	public virtual List<OrganizationTemplateRights> OrganizationTemplateRightsList { get; set; }
}
