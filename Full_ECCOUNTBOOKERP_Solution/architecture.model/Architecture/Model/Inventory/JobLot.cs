using System;

namespace Architecture.Model.Inventory;

public class JobLot
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public bool IsCompany { get; set; }

	public bool IsThirdParty { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime StartDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string JobLotCode { get; set; }

	public string JobLotDescription { get; set; }

	public string JobStatus { get; set; }

	public int JobTypeId { get; set; }

	public int AccountId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public virtual string AccountTitle { get; set; }
}
