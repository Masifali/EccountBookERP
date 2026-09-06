using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.VPS;

namespace Architecture.DAL;

public class DMSAttachment
{
	public int SetData(DMSAttachments obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public int SetDataList(DMSAttachments obj, string ProcName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.AttachmentList[0].OrganizationId, obj.AttachmentList[0].CompanyId, "IsVpsAttachmentsServiceOn"));
		ServerCredentials Sc = new ServerCredentials();
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
				}
				else
				{
					AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.AttachmentList[0].OrganizationId, obj.AttachmentList[0].CompanyId, "Attachment Folder Path");
					if (obj.AttachmentList != null && obj.AttachmentList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
				}
				if (obj.AttachmentList != null && obj.AttachmentList.Count > 0)
				{
					string AttachmentNames = "";
					string CustomNameList = "";
					foreach (DMSAttachments item in obj.AttachmentList)
					{
						string FileFullPath = item.Attachment;
						item.Attachment = Path.GetFileName(FileFullPath);
						string baseName = FileFullPath.Split('?')[0];
						string extension = ((baseName.Split('.').Length > 1) ? baseName.Split('.').Last() : string.Empty);
						string CustomFullName = (item.UploadedFileCustomName = CommonServices.GenerateCustomFileName() + "." + extension);
						returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
						AttachmentNames = ((AttachmentNames == "") ? Path.GetFileName(item.Attachment) : (AttachmentNames + "," + Path.GetFileName(item.Attachment)));
						CustomNameList = ((CustomNameList == "") ? item.UploadedFileCustomName : (CustomNameList + "," + item.UploadedFileCustomName));
						if (!IsVpsAttachmentsServiceOn)
						{
							string fileSavePath = Path.Combine(AttachmentFolderPath, CustomFullName);
							if (!File.Exists(fileSavePath))
							{
								copiedFiles.Add(fileSavePath);
								File.Copy(FileFullPath, fileSavePath, overwrite: true);
							}
							continue;
						}
						string url = "ftp://" + Sc.ServerIp + "/" + item.UploadedFileCustomName;
						try
						{
							FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
							request.Method = "STOR";
							request.UsePassive = true;
							request.UseBinary = true;
							request.KeepAlive = false;
							request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							FileInfo info = new FileInfo(FileFullPath);
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
								copiedFiles.Add(item.UploadedFileCustomName);
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
					SqlCommand sqlCommandInv = new SqlCommand
					{
						Connection = sqlcon,
						Transaction = sqlTrn,
						CommandType = CommandType.StoredProcedure,
						CommandText = "[dbo].[USP_DMS_UpdateAttachmentsValuesOfRefDocument]"
					};
					sqlCommandInv.Parameters.AddWithValue("@AttachmentName", AttachmentNames);
					if (CustomNameList != null && CustomNameList != string.Empty)
					{
						sqlCommandInv.Parameters.AddWithValue("@CustomAttachmentsValues", CustomNameList);
					}
					sqlCommandInv.Parameters.AddWithValue("@RefDocumentTypeId", obj.AttachmentList[0].RefDocumentTypeId);
					sqlCommandInv.Parameters.AddWithValue("@Id", obj.AttachmentList[0].RefDocumentNo);
					sqlCommandInv.ExecuteNonQuery();
					sqlTrn.Commit();
				}
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				foreach (string file in copiedFiles)
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
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
								File.Delete(file);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						continue;
					}
					string uri = "ftp://" + Sc.ServerIp + "/" + file;
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
					catch (Exception)
					{
					}
				}
				throw ex;
			}
		}
		return returnID;
	}

	public List<DMSAttachments> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<DMSAttachments> lst = new List<DMSAttachments>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<DMSAttachments>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
