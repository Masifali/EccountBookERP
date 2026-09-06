using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class ChartofAccount
{
	public virtual EnumAccountClass AccountClassEnum { get; set; }

	public bool IsActive { get; set; }

	public bool PostState { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public int Account_Level { get; set; }

	public int AccountClass { get; set; }

	public int AccountTypeId { get; set; }

	public int BSNoteId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int CurrencyId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PLNoteId { get; set; }

	public int PostUser { get; set; }

	public int ParentCodeId { get; set; }

	public int BranchId { get; set; }

	public int CustomGroupId { get; set; }

	public string AccountCode { get; set; }

	public string AccountGroup { get; set; }

	public string AccountTitle { get; set; }

	public string AccountTitleOtherLingo { get; set; }

	public string OtherErpCode { get; set; }

	public string ParentAccountCode { get; set; }

	public string ContactNo { get; set; }

	public string QrCode { get; set; }

	public int CityId { get; set; }

	public int CustomerGroupId { get; set; }

	public virtual bool SearchDetailAccount { get; set; }

	public virtual bool SearchGroupAccount { get; set; }

	public virtual List<COAAllocation> COAAllocationList { get; set; }

	public virtual List<ChartofAccount> ChartofAccountsList { get; set; }

	public virtual double YearObCredit { get; set; }

	public virtual double YearObDebit { get; set; }

	public virtual string AccountDisplay { get; set; }

	public virtual string ParentAccountCodeTitle { get; set; }

	public virtual int ActionId { get; set; }

	public virtual string PhoneNumber { get; set; }
}
