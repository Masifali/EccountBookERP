using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genParty
{
	public int PartyId { get; set; }

	public int OrginizationId { get; set; }

	public int CurrencyId { get; set; }

	public int PartyTypeProfileId { get; set; }

	public string PartyName { get; set; }

	public string PartyCode { get; set; }

	public string GST { get; set; }

	public string NTN { get; set; }

	public string CNIC { get; set; }

	public string PartyLogo { get; set; }

	public string PartyLogoImageURL { get; set; }

	public bool IsActive { get; set; }

	public DateTime? CreatedOn { get; set; }

	public int CreatedById { get; set; }

	public int AlteredById { get; set; }

	public DateTime? AlteredOn { get; set; }

	public int ActionTypeId { get; set; }

	public int UserLogId { get; set; }

	public int CompanyId { get; set; }
}
