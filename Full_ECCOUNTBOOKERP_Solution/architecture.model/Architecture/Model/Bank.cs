using System;

namespace Architecture.Model;

public class Bank
{
	public int Id { get; set; }

	public string BranchCode { get; set; }

	public string BranchName { get; set; }

	public string BranchAddress { get; set; }

	public int BranchCity { get; set; }

	public int CountryId { get; set; }

	public string Contact1Tel { get; set; }

	public string Contact2Tel { get; set; }

	public string Contact3Mobile { get; set; }

	public string emailPrimery { get; set; }

	public string emailAlternate { get; set; }

	public string IsHomeland { get; set; }

	public string OtherInfo { get; set; }

	public int ChartOfAccountId { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUser { get; set; }

	public DateTime? PostDate { get; set; }

	public int PostUser { get; set; }

	public bool PostState { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public string ChequeTemplete { get; set; }

	public string BankIBANNo { get; set; }

	public string BankAccountNo { get; set; }

	public string BankAccountTitle { get; set; }

	public virtual string CodeCity { get; set; }

	public virtual string DescriptionCity { get; set; }

	public virtual string CodeCountry { get; set; }

	public virtual string DescriptionCountry { get; set; }

	public virtual string AccountCode { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual int AccountTitleOtherLingo { get; set; }
}
