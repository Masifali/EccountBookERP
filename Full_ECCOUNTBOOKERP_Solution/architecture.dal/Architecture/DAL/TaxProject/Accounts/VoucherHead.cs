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
using Architecture.DAL.TaxProject.Common;
using Architecture.Model;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.VPS;

namespace Architecture.DAL.TaxProject.Accounts;

public class VoucherHead : DALBase
{
	public int SetData(Architecture.Model.TaxProject.Accounts.VoucherHead obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQLForServer();
		sqlcon.Open();
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(Architecture.DAL.TaxProject.Common.CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj != null && obj.costCenterDetailList?.Count > 0)
			{
				obj.CostCenterAmount = obj.costCenterDetailList.Sum((voucherCostCenterDetail x) => x.costAmount);
			}
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = Architecture.DAL.Common.CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = Architecture.DAL.TaxProject.Common.CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				string AttachmentsNamelist = "";
				string CustomNameList = "";
				foreach (DMSAttachments item7 in obj.AttachmentsList.ToList())
				{
					double FileSize2 = 0.0;
					string FileFullPath2 = "";
					if (!IsVpsAttachmentsServiceOn)
					{
						if (item7.FileCustomName != null && item7.FileCustomName != string.Empty)
						{
							item7.Attachment = item7.Attachment;
							item7.FileCustomName = item7.FileCustomName;
						}
						else
						{
							FileFullPath2 = item7.Attachment;
							if (!File.Exists(System.IO.Path.Combine(AttachmentFolderPath, System.IO.Path.GetFileName(item7.Attachment))))
							{
								item7.Attachment = System.IO.Path.GetFileName(FileFullPath2);
								string baseName3 = FileFullPath2.Split('?')[0];
								string extension3 = ((baseName3.Split('.').Length > 1) ? baseName3.Split('.').Last() : string.Empty);
								string CustomFullName3 = Architecture.DAL.Common.CommonServices.GenerateCustomFileName() + "." + extension3;
								item7.FileCustomName = CustomFullName3;
								FileInfo info3 = new FileInfo(FileFullPath2);
								if (info3.Exists)
								{
									FileSize2 = (double)info3.Length / 1048576.0;
								}
								if (FileSize2 > 5.0)
								{
									throw new Exception("File Size Exceeds 5MB Of File: " + item7.Attachment);
								}
								string SavePath2 = System.IO.Path.Combine(AttachmentFolderPath, item7.FileCustomName);
								if (!File.Exists(SavePath2))
								{
									File.Copy(FileFullPath2, SavePath2, overwrite: true);
									copiedFiles.Add(SavePath2);
								}
							}
							else
							{
								item7.Attachment = System.IO.Path.GetFileName(item7.Attachment);
								item7.FileCustomName = "";
							}
						}
					}
					else if (item7.FileCustomName != null && item7.FileCustomName != string.Empty)
					{
						item7.Attachment = item7.Attachment;
						item7.FileCustomName = item7.FileCustomName;
					}
					else
					{
						bool FileExist2 = false;
						string uri4 = "ftp://" + Sc.ServerIp + "/" + System.IO.Path.GetFileName(item7.Attachment);
						try
						{
							FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(uri4);
							obj2.Method = "MDTM";
							obj2.UsePassive = true;
							obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using ((FtpWebResponse)obj2.GetResponse())
							{
								FileExist2 = true;
							}
						}
						catch (WebException ex6)
						{
							if (ex6.Response is FtpWebResponse { StatusCode: not FtpStatusCode.ActionNotTakenFileUnavailable })
							{
								throw new Exception(ex6.Message);
							}
						}
						if (!FileExist2)
						{
							FileFullPath2 = item7.Attachment;
							item7.Attachment = System.IO.Path.GetFileName(FileFullPath2);
							string baseName4 = FileFullPath2.Split('?')[0];
							string extension4 = ((baseName4.Split('.').Length > 1) ? baseName4.Split('.').Last() : string.Empty);
							string CustomFullName4 = Architecture.DAL.Common.CommonServices.GenerateCustomFileName() + "." + extension4;
							item7.FileCustomName = CustomFullName4;
							FileInfo info4 = new FileInfo(FileFullPath2);
							if (info4.Exists)
							{
								FileSize2 = (double)info4.Length / 1048576.0;
							}
							if (FileSize2 > 5.0)
							{
								throw new Exception("File Size Exceeds 5MB Of File: " + item7.Attachment);
							}
							string url2 = "ftp://" + Sc.ServerIp + "/" + item7.FileCustomName;
							try
							{
								FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(url2);
								request2.Method = "STOR";
								request2.UsePassive = true;
								request2.UseBinary = true;
								request2.KeepAlive = false;
								request2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
								byte[] fileContents2;
								using (FileStream fileStream = info4.OpenRead())
								{
									fileContents2 = new byte[info4.Length];
									fileStream.Read(fileContents2, 0, fileContents2.Length);
								}
								request2.ContentLength = fileContents2.Length;
								using (Stream stream = request2.GetRequestStream())
								{
									stream.Write(fileContents2, 0, fileContents2.Length);
								}
								using ((FtpWebResponse)request2.GetResponse())
								{
									copiedFiles.Add(item7.FileCustomName);
								}
							}
							catch (WebException webEx2)
							{
								if (webEx2.Response is FtpWebResponse response2)
								{
									throw new Exception("FTP Error: " + response2.StatusDescription + " - " + webEx2.Message);
								}
								throw new Exception("Error: " + webEx2.Message);
							}
							catch (Exception ex5)
							{
								throw new Exception("Error: " + ex5.Message);
							}
						}
						else
						{
							item7.Attachment = System.IO.Path.GetFileName(item7.Attachment);
							item7.FileCustomName = "";
						}
					}
					AttachmentsNamelist = ((AttachmentsNamelist == "") ? item7.Attachment : (AttachmentsNamelist + "," + item7.Attachment));
					CustomNameList = ((CustomNameList == "") ? item7.FileCustomName : (CustomNameList + "," + item7.FileCustomName));
				}
				obj.AttachmentsValues = AttachmentsNamelist;
				obj.CustomAttachmentsValues = CustomNameList;
			}
			if (obj.DetailAttachmentsList != null && obj.DetailAttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item6 in obj.DetailAttachmentsList.ToList())
				{
					double FileSize = 0.0;
					string FileFullPath = "";
					if (!IsVpsAttachmentsServiceOn)
					{
						if (item6.FileCustomName != null && item6.FileCustomName != string.Empty)
						{
							item6.Attachment = item6.Attachment;
							item6.FileCustomName = item6.FileCustomName;
							continue;
						}
						FileFullPath = item6.Attachment;
						if (!File.Exists(System.IO.Path.Combine(AttachmentFolderPath, System.IO.Path.GetFileName(item6.Attachment))))
						{
							item6.Attachment = System.IO.Path.GetFileName(FileFullPath);
							string baseName = FileFullPath.Split('?')[0];
							string extension = ((baseName.Split('.').Length > 1) ? baseName.Split('.').Last() : string.Empty);
							string CustomFullName = Architecture.DAL.Common.CommonServices.GenerateCustomFileName() + "." + extension;
							item6.FileCustomName = CustomFullName;
							FileInfo info = new FileInfo(FileFullPath);
							if (info.Exists)
							{
								FileSize = (double)info.Length / 1048576.0;
							}
							if (FileSize > 5.0)
							{
								throw new Exception("File Size Exceeds 5MB Of File: " + item6.Attachment);
							}
							string SavePath = System.IO.Path.Combine(AttachmentFolderPath, item6.FileCustomName);
							if (!File.Exists(SavePath))
							{
								File.Copy(FileFullPath, SavePath, overwrite: true);
								copiedFiles.Add(SavePath);
							}
						}
						else
						{
							item6.Attachment = System.IO.Path.GetFileName(item6.Attachment);
							item6.FileCustomName = "";
						}
						continue;
					}
					if (item6.FileCustomName != null && item6.FileCustomName != string.Empty)
					{
						item6.Attachment = item6.Attachment;
						item6.FileCustomName = item6.FileCustomName;
						continue;
					}
					bool FileExist = false;
					string uri3 = "ftp://" + Sc.ServerIp + "/" + System.IO.Path.GetFileName(item6.Attachment);
					try
					{
						FtpWebRequest obj3 = (FtpWebRequest)WebRequest.Create(uri3);
						obj3.Method = "MDTM";
						obj3.UsePassive = true;
						obj3.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using ((FtpWebResponse)obj3.GetResponse())
						{
							FileExist = true;
						}
					}
					catch (WebException ex4)
					{
						if (ex4.Response is FtpWebResponse { StatusCode: not FtpStatusCode.ActionNotTakenFileUnavailable })
						{
							throw new Exception(ex4.Message);
						}
					}
					if (!FileExist)
					{
						FileFullPath = item6.Attachment;
						item6.Attachment = System.IO.Path.GetFileName(FileFullPath);
						string baseName2 = FileFullPath.Split('?')[0];
						string extension2 = ((baseName2.Split('.').Length > 1) ? baseName2.Split('.').Last() : string.Empty);
						string CustomFullName2 = Architecture.DAL.Common.CommonServices.GenerateCustomFileName() + "." + extension2;
						item6.FileCustomName = CustomFullName2;
						FileInfo info2 = new FileInfo(FileFullPath);
						if (info2.Exists)
						{
							FileSize = (double)info2.Length / 1048576.0;
						}
						if (FileSize > 5.0)
						{
							throw new Exception("File Size Exceeds 5MB Of File: " + item6.Attachment);
						}
						string url = "ftp://" + Sc.ServerIp + "/" + item6.FileCustomName;
						try
						{
							FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
							request.Method = "STOR";
							request.UsePassive = true;
							request.UseBinary = true;
							request.KeepAlive = false;
							request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							byte[] fileContents;
							using (FileStream fileStream2 = info2.OpenRead())
							{
								fileContents = new byte[info2.Length];
								fileStream2.Read(fileContents, 0, fileContents.Length);
							}
							request.ContentLength = fileContents.Length;
							using (Stream requestStream = request.GetRequestStream())
							{
								requestStream.Write(fileContents, 0, fileContents.Length);
							}
							using ((FtpWebResponse)request.GetResponse())
							{
								copiedFiles.Add(item6.FileCustomName);
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
						catch (Exception ex3)
						{
							throw new Exception("Error: " + ex3.Message);
						}
					}
					else
					{
						item6.Attachment = System.IO.Path.GetFileName(item6.Attachment);
						item6.FileCustomName = "";
					}
				}
			}
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			decimal LimitAmount = default(decimal);
			if (obj.voucherDetailList.Count > 0 && obj.voucherDetailList != null)
			{
				foreach (VoucherDetail item5 in obj.voucherDetailList)
				{
					if (obj.DocumentTypeId == 5)
					{
						if (item5.LineId % 2 == 0)
						{
							item5.SubNo = item5.LineId - 1;
						}
						else
						{
							item5.SubNo = item5.LineId + 1;
						}
					}
					item5.VoucherHeadId = obj.Id;
					LimitAmount += Conversion.ToDecimal(item5.DebitAmount);
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_VoucherDetail_Insert"));
					item5.Id = code;
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_VoucherBalanceCheck";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand.ExecuteScalar();
				if (obj.PaymentDuesSchedules != null)
				{
					foreach (PaymentDueSchedule items in obj.PaymentDuesSchedules)
					{
						items.RefdocNoId = obj.Id;
						items.refDocumentTypeId = obj.DocumentTypeId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, items, "Sp_PaymentDueSchedule_Insert"));
					}
				}
				if (obj.costCenterDetailList != null && obj.costCenterDetailList.Count > 0)
				{
					foreach (voucherCostCenterDetail item8 in obj.costCenterDetailList)
					{
						item8.VoucherHeaderId = obj.Id;
						if (obj.voucherDetailList.Any((VoucherDetail row) => row.SortNo == item8.SortNo))
						{
							item8.VoucherDetailId = obj.voucherDetailList.Find((VoucherDetail row) => row.SortNo == item8.SortNo).Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "USP_voucherCostCenterDetail_Insert"));
					}
				}
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
				{
					List<SqlParameter> p2 = new List<SqlParameter>();
					p2.Add(new SqlParameter
					{
						ParameterName = "@ScreenName",
						Value = obj.AttachmentsList[0].ScreenName
					});
					p2.Add(new SqlParameter
					{
						ParameterName = "@Id",
						Value = returnID
					});
					p2.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "DeleteById"
					});
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p2);
					foreach (DMSAttachments item4 in obj.AttachmentsList.ToList())
					{
						item4.RefAccountId = obj.RefAccountId;
						item4.RefDocumentTypeId = obj.DocumentTypeId;
						item4.RefDocumentNo = returnID;
						item4.EntryDate = DateTime.Now;
						item4.ModifyDate = DateTime.Now;
						item4.EntryUser = obj.EntryUser;
						item4.ModifyUser = obj.ModifyUser;
						item4.OrganizationId = obj.OrganizationId;
						item4.CompanyId = obj.CompanyId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Proc_DMSAttachments_Insert"));
					}
				}
				if (obj.DetailAttachmentsList != null && obj.DetailAttachmentsList.Count > 0)
				{
					List<SqlParameter> p = new List<SqlParameter>();
					p.Add(new SqlParameter
					{
						ParameterName = "@ScreenName",
						Value = obj.DetailAttachmentsList[0].ScreenName
					});
					p.Add(new SqlParameter
					{
						ParameterName = "@Id",
						Value = returnID
					});
					p.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "DetailDeleteById"
					});
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments item3 in obj.DetailAttachmentsList.ToList())
					{
						item3.RefDocumentTypeId = obj.DocumentTypeId;
						item3.RefDocumentNo = returnID;
						item3.EntryDate = DateTime.Now;
						item3.ModifyDate = DateTime.Now;
						item3.EntryUser = obj.EntryUser;
						item3.ModifyUser = obj.ModifyUser;
						item3.OrganizationId = obj.OrganizationId;
						item3.CompanyId = obj.CompanyId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
					}
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item2 in obj.voucherDetailList)
				{
					item2.VoucherHeadId = obj.Id;
					item2.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_H_Insert"));
				}
				DocumentApprovalDetail AP = new DocumentApprovalDetail();
				AP.OrganizationId = obj.OrganizationId;
				AP.CompanyId = obj.CompanyId;
				AP.DocumentTypeId = obj.DocumentTypeId;
				AP.Id = obj.Id;
				AP.LimitAmount = LimitAmount;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, AP, "[DAW].[USp_DocumentApprovalDetail_Insert]"));
				if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
				{
					foreach (DMSAttachments item in obj.DeleteAttachmentsList.ToList())
					{
						if (!IsVpsAttachmentsServiceOn)
						{
							string Path = AttachmentFolderPath + "\\" + item.Attachment;
							if (!File.Exists(Path))
							{
								continue;
							}
							using (FileStream fileStream3 = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite))
							{
								fileStream3.Flush();
								fileStream3.Close();
								fileStream3.Dispose();
							}
							bool fileDeleted2 = false;
							int attempts2 = 0;
							while (!fileDeleted2 && attempts2 < 5)
							{
								try
								{
									File.Delete(Path);
									fileDeleted2 = true;
								}
								catch (IOException)
								{
									attempts2++;
									Thread.Sleep(500);
								}
							}
							continue;
						}
						string uri2 = "ftp://" + Sc.ServerIp + "/" + item.Attachment;
						try
						{
							FtpWebRequest obj4 = (FtpWebRequest)WebRequest.Create(uri2);
							obj4.Method = "DELE";
							obj4.UsePassive = true;
							obj4.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using FtpWebResponse ftpWebResponse5 = (FtpWebResponse)obj4.GetResponse();
							if (ftpWebResponse5.StatusCode != FtpStatusCode.FileActionOK)
							{
								throw new Exception("Failed to delete file from VPS.");
							}
						}
						catch (Exception ex2)
						{
							throw new Exception("Error while deleting file from VPS: " + ex2.Message);
						}
					}
				}
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("Voucher Detail Not Found");
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
					FtpWebRequest obj5 = (FtpWebRequest)WebRequest.Create(uri);
					obj5.Method = "DELE";
					obj5.UsePassive = true;
					obj5.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
					using FtpWebResponse deleteResponse = (FtpWebResponse)obj5.GetResponse();
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

	public int SetDataDetail(VoucherDetail obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQLForServer();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.Id = Convert.ToInt32(returnID);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.TaxProject.Accounts.VoucherHead> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.TaxProject.Accounts.VoucherHead> lst = new List<Architecture.Model.TaxProject.Accounts.VoucherHead>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQLForServer())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.TaxProject.Accounts.VoucherHead>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQLForServer())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.TaxProject.Accounts.VoucherHead item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "VoucherDetail_ReadByVoucherHeadID"));
						item3.voucherDetailList = GenericProvider.Get<VoucherDetail>(sqlConnection2, "Sp_Vouchers_GetMethods", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQLForServer())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.TaxProject.Accounts.VoucherHead item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@DocumentTypeId", item2.DocumentTypeId));
						lstPrm2.Add(new SqlParameter("@RefDocNoId", item2.Id));
						item2.PaymentDuesSchedules = GenericProvider.Get<PaymentDueSchedule>(sqlConnection3, "Sp_PaymentDueSchedule_ReadAll", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQLForServer();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.TaxProject.Accounts.VoucherHead item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "voucherCostCenterDetailByHeaderId"));
					item.costCenterDetailList = GenericProvider.Get<voucherCostCenterDetail>(sqlcon, "Sp_Vouchers_GetMethods", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public List<Architecture.Model.TaxProject.Accounts.VoucherHead> GetDateAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.TaxProject.Accounts.VoucherHead> lst = new List<Architecture.Model.TaxProject.Accounts.VoucherHead>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQLForServer();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.TaxProject.Accounts.VoucherHead>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int VoucherDetailIdUpdate(int Id)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_VoucherHead_WHTTaxChallanDeposit_Update");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, Id);
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
