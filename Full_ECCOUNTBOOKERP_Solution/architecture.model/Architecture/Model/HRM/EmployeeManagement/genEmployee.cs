using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genEmployee
{
	public int EmployeeId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int ControlTypeProfileId { get; set; }

	public int PersonId { get; set; }

	public int PartyLocationId { get; set; }

	public int BillCategoryProfileId { get; set; }

	public int EmployeeTypeProfileId { get; set; }

	public int EmployeeCategoryId { get; set; }

	public int ConsultantTypeProfileId { get; set; }

	public int EmployeeHistoryId { get; set; }

	public int AddressId { get; set; }

	public string AddressDetail { get; set; }

	public int TitleId { get; set; }

	public string EmployeeNo { get; set; }

	public string FistName { get; set; }

	public string MiddleName { get; set; }

	public string LastName { get; set; }

	public string CNIC { get; set; }

	public DateTime DOB { get; set; }

	public DateTime CNICIssueDate { get; set; }

	public DateTime CNICExpiryDate { get; set; }

	public int GenderProfileId { get; set; }

	public int NationalityProfileId { get; set; }

	public int BloodGroupProfileId { get; set; }

	public bool NeedLogin { get; set; }

	public string PictureFilePath { get; set; }

	public string SignPictureFilePath { get; set; }

	public string SignPictureString { get; set; }

	public string EmployeePictureString { get; set; }

	public string Signature { get; set; }

	public string Mobile1 { get; set; }

	public string Mobile2 { get; set; }

	public string Email { get; set; }

	public string LicenseNo { get; set; }

	public string PassportNo { get; set; }

	public int SubTitleId { get; set; }

	public string RelationName { get; set; }

	public string RelationMobile { get; set; }

	public bool Active { get; set; }

	public bool IsApproved { get; set; }

	public int CreatedById { get; set; }

	public DateTime CreatedOn { get; set; }

	public int AlteredById { get; set; }

	public DateTime AlteredOn { get; set; }

	public int ActionTypeId { get; set; }

	public int UserLogId { get; set; }

	public bool IsAllowShare { get; set; }

	public double FlatSharePercent { get; set; }

	public virtual string EmployeeType { get; set; }

	public virtual string EmployeeCategory { get; set; }

	public virtual string TitleName { get; set; }

	public virtual string RelationTitleName { get; set; }

	public virtual string Gender { get; set; }

	public virtual string Nationality { get; set; }

	public virtual string BloodGroup { get; set; }

	public int BranchesId { get; set; }

	public int ProjectId { get; set; }

	public virtual List<genEmployeeHistory> EmployeeHistorylist { get; set; }

	public virtual List<EmployeeSalary> EmployeeSalarieslist { get; set; }

	public virtual List<hrmEmployeeBenefit> HrmEmployeeBenefitslist { get; set; }

	public virtual List<genEmployeeWeekDay> EmployeeWeekDayslist { get; set; }

	public virtual List<tblEmployeeAddressDetail> EmployeeAddressDetailslist { get; set; }

	public virtual List<hrmEmployeeExperience> EmployeeExperienceslist { get; set; }

	public virtual List<GenEmployeeEducation> EmployeeEducationslist { get; set; }

	public virtual List<hrmEmployeeFamilyInfo> EmployeeFamilyInfoslist { get; set; }

	public virtual List<hrmEmployeeReference> EmployeeReferenceslist { get; set; }

	public virtual List<hrmEmployeeBankAccount> EmployeeBankAccountslist { get; set; }
}
