using System;
using System.Collections.Generic;

namespace Architecture.Model.Templates;

public class Organization_ChartOfAccountTemplate
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

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int PLNoteId { get; set; }

	public int PostUser { get; set; }

	public int ParentCodeId { get; set; }

	public string AccountCode { get; set; }

	public string AccountGroup { get; set; }

	public string AccountTitle { get; set; }

	public string AccountTitleOtherLingo { get; set; }

	public string OtherErpCode { get; set; }

	public string ParentAccountCode { get; set; }

	public string QrCode { get; set; }

	public int FinancialYearId { get; set; }

	public int Lvl01Id { get; set; }

	public int Lvl02Id { get; set; }

	public int Lvl03Id { get; set; }

	public int Lvl04Id { get; set; }

	public int OrganizationTemplateId { get; set; }

	public virtual List<Organization_ChartOfAccountTemplate> Organization_ChartOfAccountTemplateList { get; set; }
}
