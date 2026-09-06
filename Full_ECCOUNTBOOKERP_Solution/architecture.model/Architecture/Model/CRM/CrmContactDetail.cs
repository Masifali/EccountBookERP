namespace Architecture.Model.CRM;

public class CrmContactDetail
{
	public bool IsActive { get; set; }

	public int CrmContactHeaderId { get; set; }

	public int CrmLookupTypeId { get; set; }

	public int DesignationId { get; set; }

	public int Id { get; set; }

	public string CrmLookupDescription { get; set; }

	public string NameIdentity { get; set; }

	public virtual string Designation { get; set; }

	public virtual string Title { get; set; }
}
