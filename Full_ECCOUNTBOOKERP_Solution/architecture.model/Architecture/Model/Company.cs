using System;

namespace Architecture.Model;

public class Company
{
	public int Id { get; set; }

	public int OrgCompanyTypeId { get; set; }

	public string CompAddress { get; set; }

	public string CompBaseCurr { get; set; }

	public string CompCode { get; set; }

	public string CompContactPerson { get; set; }

	public string CompCountry { get; set; }

	public string CompEmailA { get; set; }

	public string CompEmailB { get; set; }

	public string CompLogo { get; set; }

	public string CompMobileA { get; set; }

	public string CompMobileB { get; set; }

	public string CompMobileC { get; set; }

	public string CompName { get; set; }

	public string CompReportingTitle { get; set; }

	public string CompState { get; set; }

	public string CompTel { get; set; }

	public string CompType { get; set; }

	public string PictureURL { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public byte[] CompLogoImage { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public string CityName { get; set; }

	public int CompanyTemplateId { get; set; }

	public int CompanyId { get; set; }

	public string CompanyNameOtherLanguage { get; set; }

	public string CompanyNameOtherLing { get; set; }

	public string CompanyAddressOtherLanguage { get; set; }

	public string CompanyWebsite { get; set; }

	public int AllowedUserCount { get; set; }

	public string CompanyFaxNo { get; set; }

	public virtual string FirstName { get; set; }

	public virtual string LastName { get; set; }
}
