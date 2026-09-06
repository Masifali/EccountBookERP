using System;

namespace Architecture.Model.Inventory;

public class SupCustCreditLimitSchedule
{
	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double CreditLimit { get; set; }

	public double DebitLimit { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int SuplierCustomerId { get; set; }
}
