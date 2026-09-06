using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Architecture.BLL.Authentication;
using Architecture.BLL.SMS;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.DAL.SystemUtilities;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.SMS;

namespace Architecture.BLL;

public class UserAccount
{
	public static int Save(Architecture.Model.UserAccount obj)
	{
		try
		{
			if (obj.ID == 0)
			{
				return new Architecture.DAL.UserAccount().SetData(obj, "Sp_UserAccount_Insert");
			}
			return new Architecture.DAL.UserAccount().SetData(obj, "Sp_UserAccount_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.UserAccount Login(Architecture.Model.UserAccount obj, int AppVersion = 0, int ValidateVersion = 0, string DeviceId = null)
	{
		try
		{
			obj.Password = Encryption.UserPasswordEncryptString(obj.Password);
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserName",
				Value = obj.UserName
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Password",
				Value = obj.Password
			});
			if (obj.AppId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			if (ValidateVersion > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ValidateVersion",
					Value = ValidateVersion
				});
			}
			if (AppVersion > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppVersion",
					Value = AppVersion
				});
			}
			if (!string.IsNullOrEmpty(DeviceId))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeviceId",
					Value = DeviceId
				});
			}
			List<Architecture.Model.UserAccount> lst = new Architecture.DAL.UserAccount().GetDataAll("Sp_UserAccount_Login", p);
			if (lst.Count == 0)
			{
				throw new Exception("Invalid UserId or Password");
			}
			clsGlobalVariables.UserAccount = lst[0];
			if (lst[0].CompanyId > 0)
			{
				clsGlobalVariables.configrationsAllocation = ConfigrationsAllocation.HistoryConfiquration(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId);
			}
			clsGlobalVariables.UserAccount.UserProfileList = GetUserProfileByUserId(clsGlobalVariables.UserAccount.ID);
			switch (CheckTwoWayAuthenticationConfiguration(lst[0]))
			{
			case 1:
			{
				int AuthenticationCode = setAuthenticationCode(lst[0].ID);
				lst[0].AuthenticationCode = AuthenticationCode;
				lst[0].AuthenticationEnabledForUser = 1;
				break;
			}
			case 0:
				lst[0].AuthenticationEnabledForUser = 0;
				lst[0].AuthenticationCode = 0;
				break;
			}
			return lst[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static AdminUser AdminLogin(Architecture.Model.UserAccount obj)
	{
		try
		{
			obj.Password = Encryption.UserPasswordEncryptString(obj.Password);
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserName",
				Value = obj.UserName
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Password",
				Value = obj.Password
			});
			return new Architecture.DAL.UserAccount().GetDataAdminUser("[dbo].[USP_AdminLogin]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PlayIdUpdate(string UserName, string Password, string PlayerId)
	{
		try
		{
			Password = Encryption.UserPasswordEncryptString(Password);
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserName",
				Value = UserName
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Password",
				Value = Password
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PlayerId",
				Value = PlayerId
			});
			GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Usp_PlayIdUpdate_UserAccount", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.UserAccount GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			return new Architecture.DAL.UserAccount().GetData("Sp_UserAccount_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.UserProfile GetUserProfileByUserId(int UserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetUserProfileByUserId"
			});
			List<Architecture.Model.UserProfile> lst = new Architecture.DAL.UserProfile().GetData("SP_UserProfile_GetAllMethod", p);
			return (lst == null || lst.Count <= 0) ? null : lst?[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.UserAccount ReadByUsername(string UserName)
	{
		try
		{
			List<SqlParameter> p = null;
			p.Add(new SqlParameter
			{
				ParameterName = "@UserName",
				Value = UserName
			});
			new List<Architecture.Model.UserAccount>();
			return new Architecture.DAL.UserAccount().GetData("Proc_AccountUser_ReadByUsername", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(Architecture.Model.UserAccount obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_UserAccount_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadUserByCompanyId(int Id, int ActionId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = Id
			});
			if (ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAllUser"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_UserAccount_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllRole(Architecture.Model.UserAccount obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_AppRoles_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateUserNewPassword(ReportsParameters obj)
	{
		try
		{
			obj.OldPassword = Encryption.UserPasswordEncryptString(obj.OldPassword);
			obj.Password = Encryption.UserPasswordEncryptString(obj.Password);
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OldPassword",
				Value = obj.OldPassword
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Password",
				Value = obj.Password
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ID",
				Value = obj.Id
			});
			GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_UserAccount_UpdateNewPassword", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAdminUserByOrganizationId(int OrganizationId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAdminUserByOrganizationId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_UserAccount_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void LicenseCheck(int OrganizationId, int CompanyId, int UserId, string DeviceId = null, string machine = null, string ip = null, string macAddress = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLicenseKey"
			});
			DataTable dataTableProc = GenericProvider.GetDataTableProc<Architecture.Model.CLV>("Sp_UserAccount_GetAllMethod", p);
			if (dataTableProc.Rows.Count == 0)
			{
				throw new Exception("License Not Found!");
			}
			string Key = Conversion.ToString(dataTableProc.Rows[0]["LicenseKey"]);
			if (Key != string.Empty)
			{
				Key = LicenseKey.ReturnKey(Key);
				string key = Key;
				int startIndex = key.IndexOf('(');
				if (startIndex >= 0)
				{
					if (DateTime.TryParse(key.Substring(startIndex + 1), out var licenseExpiryDate))
					{
						if (DateTime.Now.Date > licenseExpiryDate.Date)
						{
							throw new Exception("Your License has Expired. Please contact EccountBookERP SupportTeam CellNo: 0304-4059175");
						}
						List<SqlParameter> y = new List<SqlParameter>();
						y.Add(new SqlParameter
						{
							ParameterName = "@OrganizationId",
							Value = OrganizationId
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@CompanyId",
							Value = CompanyId
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@LicenseKey",
							Value = Key
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@UserId",
							Value = UserId
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@DeviceId",
							Value = DeviceId
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@MachineName",
							Value = machine
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@IPAddress",
							Value = ip
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@LoginMacAddress",
							Value = macAddress
						});
						y.Add(new SqlParameter
						{
							ParameterName = "@SysDate",
							Value = DateTime.Now
						});
						GenericProvider.GetDataTableProc<Architecture.Model.CLV>("GetMaxDate", y);
						return;
					}
					throw new Exception("Unable to parse license date.");
				}
				throw new Exception("Invalid license format. No opening bracket found.");
			}
			throw new Exception("Your Licensed has been Expired Please contact EccountBookERP Support Team Cell No : 0304-4059175");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLicensCompanyAndExpiryDate(int OrganizationId, int CompanyId)
	{
		try
		{
			string Key = "";
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLicenseKey"
			});
			DataTable LicenseInformation = GenericProvider.GetDataTableProc<Architecture.Model.CLV>("Sp_UserAccount_GetAllMethod", p);
			if (LicenseInformation.Rows.Count == 0)
			{
				throw new Exception("License Not Found!");
			}
			Key = Conversion.ToString(LicenseInformation.Rows[0]["LicenseKey"]);
			if (!string.IsNullOrEmpty(Key))
			{
				Key = LicenseKey.ReturnKey(Key);
			}
			List<SqlParameter> y = new List<SqlParameter>();
			y.Add(new SqlParameter
			{
				ParameterName = "@LicenseKey",
				Value = Key
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CLV>("USP_GetLicenseExpiryDateByLicense", y);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int setAuthenticationCode(int ID, string UserEmail = null)
	{
		try
		{
			int AuthenticationCode = new Random().Next(100000, 999999);
			DataTable dtUserProfile = UserProfile.GetUserProfileByUserId(ID);
			if (dtUserProfile.Rows.Count <= 0)
			{
				throw new Exception("Please add user profile info");
			}
			string USER_EMAIL = "";
			USER_EMAIL = ((UserEmail == null || !(UserEmail != string.Empty)) ? Conversion.ToString(dtUserProfile.Rows[0]["Email"]) : UserEmail);
			if (USER_EMAIL == null || USER_EMAIL == string.Empty)
			{
				throw new Exception("Couldn't Find Email to Send Authentication Code!");
			}
			string EmailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$";
			if (!Regex.Match(USER_EMAIL, EmailPattern).Success)
			{
				throw new Exception("Email is not Valid!");
			}
			new Email().SendAuthenticationEmail(Conversion.ToInt(dtUserProfile.Rows[0]["OrganizationId"]), Conversion.ToInt(dtUserProfile.Rows[0]["CompanyId"]), Conversion.ToString(USER_EMAIL), Conversion.ToString(AuthenticationCode));
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "UpdateAuthenticationCode"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = ID
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AuthenticationCode",
				Value = AuthenticationCode
			});
			GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_UserAccount_GetAllMethod", p);
			return AuthenticationCode;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void expireAuthenticationCode(int ID)
	{
		try
		{
			int Code = 1;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "UpdateAuthenticationCode"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = ID
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AuthenticationCode",
				Value = Code
			});
			GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_UserAccount_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool VerifyCode(Architecture.Model.UserAccount obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.ID
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAuthenticationCode"
			});
			new List<Architecture.Model.UserAccount>();
			if (Conversion.ToInt(new Architecture.DAL.UserAccount().VerifyCode("Sp_UserAccount_GetAllMethod", p)[0].AuthenticationCode) == Conversion.ToInt(obj.AuthenticationCode))
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SendAuthenticationCodeToUserMobileNumber(Architecture.Model.UserAccount UserInfo, int Code)
	{
		try
		{
			Architecture.Model.SMS.SmsProvider obj = Architecture.BLL.SMS.SmsProvider.ReadByCompanyId(UserInfo.CompanyId);
			string Message = Conversion.ToString(Code);
			List<string> Numbers = new List<string>();
			Numbers.Add(UserInfo.CellNo);
			if (obj.ProviderName == "LifeTimesSms")
			{
				Architecture.BLL.SMS.SmsProvider.SendSmsViaLifeTimeSmsApi(obj, Numbers, Message);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int CheckTwoWayAuthenticationConfiguration(Architecture.Model.UserAccount obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.ID
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "CheckTwoWayAuthenticationConfiguration"
			});
			new List<Architecture.Model.UserAccount>();
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("Sp_UserAccount_GetAllMethod", p).Rows[0]["AuthenticationEnabledForUser"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool CheckIfPasswordIsExpired(int OrganizationId, int CompanyId, int UserId)
	{
		try
		{
			bool status = false;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "CheckIfPasswordIsExpired"
			});
			DataTable dt = GenericProvider.GetDataTableProc<UserAccount>("[dbo].[Sp_UserAccount_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				status = Conversion.ToBool(dt.Rows[0]["ExpiredStatus"]);
			}
			return status;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadUserDataByUserTypeId(int OrganizationId, int CompanyId, int UserTpeId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserTypeId",
				Value = UserTpeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadUserDataByUserTypeId"
			});
			return GenericProvider.GetDataTableProc<UserAccount>("[dbo].[Sp_UserAccount_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllocatedOrUnAllocatedUsersAgainstScreen(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			if (obj.ScreenName != null && obj.ScreenName != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.ScreenName
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RightName",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("USP_getUserByScreenName", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUsersForAccountsStatusAllocation(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("USP_GetUsersForAccountsStatusAllocation", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUsersFromAccountsShowHideStatus(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("USP_GetUsersFromAccountsShowHideStatus", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUsersWithPlayerId(int OrganizationId, int CompanyId, int DocumentTypeId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("usp_GetUsersWithPlayerId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetVersion(int userId)
	{
		int appVersion = 0;
		try
		{
			DataTable dt = null;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@userId",
				Value = userId
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.UserAccount>("usp_getAppVersion", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				appVersion = Conversion.ToInt(dt.Rows[0]["AppVersion"]);
			}
			return appVersion;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
