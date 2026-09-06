using System;

namespace Architecture.Model.PartyProcessing;

public class JobLotPartyProcessing
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

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
}
