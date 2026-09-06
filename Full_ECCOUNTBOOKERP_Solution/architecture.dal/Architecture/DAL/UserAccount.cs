using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.VPS;

namespace Architecture.DAL;

public class UserAccount : DALBase
{
	public int SetData(Architecture.Model.UserAccount obj, string ProcName)
	{
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		List<string> copiedFiles = new List<string>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			obj.Password = Encryption.UserPasswordEncryptString(obj.Password);
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ID = returnID;
			}
			else
			{
				returnID = obj.ID;
			}
			if (obj.UserProfileList != null)
			{
				Architecture.Model.UserProfile profile = obj.UserProfileList;
				profile.UserId = returnID;
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
				}
				else
				{
					AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
					if (profile.ProfileImageFileName != null && profile.ProfileImageFileName != string.Empty && !Directory.Exists(AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
				}
				if (profile.ProfileImageFileName != string.Empty && profile.ProfileImageFileName != null && System.IO.Path.IsPathRooted(profile.ProfileImageFileName))
				{
					double FileSize = 0.0;
					FileInfo info = new FileInfo(profile.ProfileImageFileName);
					if (info.Exists)
					{
						FileSize = (double)info.Length / 1048576.0;
					}
					if (FileSize > 5.0)
					{
						throw new Exception("File Size Exceeds 5MB Of File: " + System.IO.Path.GetFileName(obj.PictureURL));
					}
					string fileName = System.IO.Path.GetFileNameWithoutExtension(profile.ProfileImageFileName);
					string exten = System.IO.Path.GetExtension(profile.ProfileImageFileName);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + "_" + date + exten;
					if (!IsVpsAttachmentsServiceOn)
					{
						string PathToSave = System.IO.Path.Combine(AttachmentFolderPath, fileName);
						File.Copy(profile.ProfileImageFileName, PathToSave, overwrite: true);
						copiedFiles.Add(fileName);
					}
					else
					{
						string url = "ftp://" + Sc.ServerIp + "/" + fileName;
						try
						{
							FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
							request.Method = "STOR";
							request.UsePassive = true;
							request.UseBinary = true;
							request.KeepAlive = false;
							request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							byte[] fileContents;
							using (FileStream fileStream = info.OpenRead())
							{
								fileContents = new byte[info.Length];
								fileStream.Read(fileContents, 0, fileContents.Length);
							}
							request.ContentLength = fileContents.Length;
							using (Stream requestStream = request.GetRequestStream())
							{
								requestStream.Write(fileContents, 0, fileContents.Length);
							}
							using ((FtpWebResponse)request.GetResponse())
							{
								copiedFiles.Add(fileName);
							}
						}
						catch (WebException webEx)
						{
							if (webEx.Response is FtpWebResponse response)
							{
								throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
							}
							throw new Exception("Error: " + webEx.Message);
						}
						catch (Exception ex2)
						{
							throw new Exception("Error: " + ex2.Message);
						}
					}
					profile.ProfileImageFileName = fileName;
				}
				if (profile.PictureToDeleteInUpdateIfAny != null && profile.PictureToDeleteInUpdateIfAny != string.Empty)
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						string Path = AttachmentFolderPath + "\\" + profile.PictureToDeleteInUpdateIfAny;
						if (!File.Exists(Path))
						{
							throw new Exception("File not exist.");
						}
						using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
					else
					{
						string uri = "ftp://" + Sc.ServerIp + "/" + profile.PictureToDeleteInUpdateIfAny;
						try
						{
							FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(uri);
							obj2.Method = "DELE";
							obj2.UsePassive = true;
							obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using FtpWebResponse deleteResponse = (FtpWebResponse)obj2.GetResponse();
							if (deleteResponse.StatusCode != FtpStatusCode.FileActionOK)
							{
								throw new Exception("Failed to delete file from VPS.");
							}
						}
						catch (Exception ex)
						{
							throw new Exception("Error while deleting file from VPS: " + ex.Message);
						}
					}
				}
				if (profile.Id == 0)
				{
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, profile, "Sp_UserProfile_Insert"));
				}
				else
				{
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, profile, "Sp_UserProfile_Update"));
				}
			}
			foreach (UserAccountAllocation item in obj.UserAccountAllocationsList)
			{
				ScreenRightstoUser rightstoUser = new ScreenRightstoUser();
				rightstoUser.UserId = obj.UserGroupId;
				rightstoUser.NewUserid = returnID;
				rightstoUser.CompanyId = item.CompanyId;
				rightstoUser.Activity = "ScreenRightstoUser";
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, rightstoUser, "Sp_tblUserRights_GetAllMethod"));
				item.UserAccountId = returnID;
				item.ModifyUserId = returnID;
				item.EntryUserId = returnID;
				item.ModifyDate = DateTime.Now;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_UserAccountAllocation_Insert"));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex4)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex4;
		}
	}

	public List<Architecture.Model.UserAccount> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.UserAccount> lst = new List<Architecture.Model.UserAccount>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.UserAccount>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.UserAccount item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.ID));
						lstPrm2.Add(new SqlParameter("@Activity", "GetUserAllocationByUserId"));
						item2.UserAccountAllocationsList = GenericProvider.GetAllDetail<UserAccountAllocation>(sqlConnection2, "Sp_UserAccount_GetAllMethod", lstPrm2);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null && lst.Count > 0)
				{
					List<SqlParameter> lstProfile = new List<SqlParameter>();
					lstProfile.Add(new SqlParameter("@UserId", lst[0].ID));
					lstProfile.Add(new SqlParameter("@Activity", "GetUserProfileByUserId"));
					List<Architecture.Model.UserProfile> ProfileLst = GenericProvider.GetAllDetail<Architecture.Model.UserProfile>(sqlConnection3, "SP_UserProfile_GetAllMethod", lstProfile);
					lst[0].UserProfileList = ((ProfileLst != null && ProfileLst.Count > 0) ? ProfileLst[0] : null);
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.UserAccount item in lst)
				{
					string CompanyIds = string.Join(",", from app in item.UserAccountAllocationsList
						where app.CompanyId > 0
						select app.CompanyId.ToString());
					int UserId = Conversion.ToInt(item.ID);
					List<SqlParameter> lstPrm = new List<SqlParameter>
					{
						new SqlParameter("@UserId", UserId),
						new SqlParameter("@CompanyIds", CompanyIds)
					};
					item.ApplicationsAllocateToUserList = GenericProvider.GetAllDetail<Architecture.Model.ApplicationsAllocateToUser>(sqlcon, "[dbo].[USP_ApplicationsAllocateToUser_ForUserAccount]", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public List<AdminUser> GetDataAdminUser(string ProcName, List<SqlParameter> p = null)
	{
		List<AdminUser> lst = new List<AdminUser>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<AdminUser>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public List<Architecture.Model.UserAccount> GetDataAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.UserAccount> lst = new List<Architecture.Model.UserAccount>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.UserAccount>(sqlcon, ProcName, p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public Architecture.Model.UserAccount Login(string userName, string password)
	{
		Architecture.Model.UserAccount userAccount = null;
		IDataReader reader = null;
		try
		{
			DbCommand cmdUserAccount = sqlDataBase.GetStoredProcCommand("Proc_UserAccount_Login");
			sqlDataBase.AddInParameter(cmdUserAccount, "UserName", SqlDbType.NVarChar, userName);
			sqlDataBase.AddInParameter(cmdUserAccount, "Password", SqlDbType.NVarChar, password);
			reader = sqlDataBase.ExecuteReader(cmdUserAccount);
			while (reader.Read())
			{
				userAccount = FillDTO(reader);
			}
			return userAccount;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	private Architecture.Model.UserAccount FillDTO(IDataReader reader)
	{
		try
		{
			return new Architecture.Model.UserAccount
			{
				ID = Conversion.ToInt(reader["Id"]),
				FirstName = Conversion.ToString(reader["FirstName"]),
				LastName = Conversion.ToString(reader["LastName"]),
				UserName = Conversion.ToString(reader["UserName"]),
				Password = Encryption.UserPasswordDecryptString(Conversion.ToString(reader["Password"])),
				IsActive = Conversion.ToBool(reader["IsActive"]),
				UserRoleId = Conversion.ToInt(reader["UserRoleId"]),
				UserGroupId = Conversion.ToInt(reader["UserGroupId"]),
				RoleName = Conversion.ToString(reader["RoleName"]),
				OrganizationId = Conversion.ToInt(reader["OrganizationId"]),
				CompanyId = Conversion.ToInt(reader["CompanyId"]),
				CompName = Conversion.ToString(reader["CompName"]),
				OrgName = Conversion.ToString(reader["OrgName"]),
				CompAddress = Conversion.ToString(reader["CompAddress"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.UserAccount> VerifyCode(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.UserAccount> lst = new List<Architecture.Model.UserAccount>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.UserAccount>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
