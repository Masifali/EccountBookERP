using System;

namespace Architecture.Model.Inventory;

public class SupplierCustomer
{
	public bool IsDebitCredit { get; set; }

	public bool IsSubSupCust { get; set; }

	public bool PostState { get; set; }

	public bool Status { get; set; }

	public DateTime CNIC_EXPIRY_DATE { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public double CreditLimit { get; set; }

	public double DebitCreditAmount { get; set; }

	public int ActionId { get; set; }

	public int AdvanceGlAcId { get; set; }

	public int BranchId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int CountryId { get; set; }

	public int CustomerGroupId { get; set; }

	public int EntryUser { get; set; }

	public int GlAccountId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ParentsSupCustId { get; set; }

	public int DiscountPolicyId { get; set; }

	public int PostUser { get; set; }

	public int ProfileGroupId { get; set; }

	public int ProjectId { get; set; }

	public int StateProvinceId { get; set; }

	public int CustomerTypeId { get; set; }

	public int PartyTypeId { get; set; }

	public int BusinessTypeId { get; set; }

	public string PartyTypePrefix { get; set; }

	public string Address1 { get; set; }

	public string Address2 { get; set; }

	public string CNIC { get; set; }

	public string CompanyName { get; set; }

	public string Email { get; set; }

	public string FirstName { get; set; }

	public string FTN_No { get; set; }

	public string LastName { get; set; }

	public string MobileOffice { get; set; }

	public string MobilePersonal { get; set; }

	public string NTN_No { get; set; }

	public string Phone { get; set; }

	public string PictureURL { get; set; }

	public string NickName { get; set; }

	public string ReportingTitle { get; set; }

	public string STRN_No { get; set; }

	public string SupCustCode { get; set; }

	public string Title { get; set; }

	public string Town { get; set; }

	public string WebPage { get; set; }

	public string ZipCode { get; set; }

	public string ManualPartyCode { get; set; }

	public string WhatsAppNo { get; set; }

	public string CompanyIndividuals { get; set; }

	public bool IsTaxable { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string AccountTitleAdvance { get; set; }

	public virtual string CustomerType { get; set; }
}
