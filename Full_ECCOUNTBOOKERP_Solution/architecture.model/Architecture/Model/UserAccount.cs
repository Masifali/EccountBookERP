using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class UserAccount
{
	public bool IsActive { get; set; }

	public bool DeviceDependency { get; set; }

	public int CompanyId { get; set; }

	public int ID { get; set; }

	public int OrganizationId { get; set; }

	public int UserGroupId { get; set; }

	public int UserRoleId { get; set; }

	public int BranchesId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int AuthenticationCode { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Password { get; set; }

	public string UserName { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int AuthenticationEnabledForUser { get; set; }

	public int UserTypeId { get; set; }

	public int AppId { get; set; }

	public int EmployeeId { get; set; }

	public string CellNo { get; set; }

	public virtual bool IsHeadOffice { get; set; }

	public virtual string OrgName { get; set; }

	public virtual string CompName { get; set; }

	public virtual string ClientsNameForReports { get; set; }

	public virtual string BranchName { get; set; }

	public virtual string PictureURL { get; set; }

	public virtual string RoleName { get; set; }

	public virtual string CompAddress { get; set; }

	public virtual string LicenseKey { get; set; }

	public virtual DateTime SystemDate { get; set; }

	public virtual int PartyGlAccountId { get; set; }

	public virtual string FatherName { get; set; }

	public virtual string Education { get; set; }

	public virtual string Email { get; set; }

	public virtual string ProfileImageFileName { get; set; }

	public virtual List<UserAccountAllocation> UserAccountAllocationsList { get; set; }

	public virtual List<ApplicationsAllocateToUser> ApplicationsAllocateToUserList { get; set; }

	public virtual UserProfile UserProfileList { get; set; }
}
