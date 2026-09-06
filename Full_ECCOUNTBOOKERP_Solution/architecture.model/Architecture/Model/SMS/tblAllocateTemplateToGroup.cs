using System;

namespace Architecture.Model.SMS;

public class tblAllocateTemplateToGroup
{
	public DateTime EntryDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int GroupId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int TemplateId { get; set; }

	public virtual string TemplateText { get; set; }

	public virtual DateTime Date { get; set; }

	public virtual int Screen_Id { get; set; }

	public virtual string MessageBody { get; set; }

	public virtual string MobileNo { get; set; }

	public virtual string Name { get; set; }

	public virtual string ScreenName { get; set; }
}
