namespace Architecture.Model.Templates;

public class OrganizationTemplates
{
	public int Id { get; set; }

	public int PreviousTemplateId { get; set; }

	public int CompanyTypeId { get; set; }

	public string TemplateDescription { get; set; }

	public string TemplateName { get; set; }
}
