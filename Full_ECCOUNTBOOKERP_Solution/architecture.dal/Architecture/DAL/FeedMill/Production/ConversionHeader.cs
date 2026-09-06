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
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Production;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.FeedMill.Production;

public class ConversionHeader
{
	public int SetData(Architecture.Model.FeedMill.Production.ConversionHeader obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			int LineId = 1;
			foreach (ConversionOutPutDetail item9 in obj.ConversionOutPutDetailList)
			{
				item9.ConversionHeaderId = obj.Id;
				item9.LineId = LineId;
				int code4 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "[fed].[USP_ConversionOutPutDetail_Insert]"));
				item9.Id = code4;
				LineId++;
			}
			LineId = 1000;
			foreach (ConversionInputDetail item7 in obj.ConversionInputDetailDetailList)
			{
				item7.ConversionHeaderId = obj.Id;
				item7.LineId = LineId;
				int code3 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "[fed].[USP_ConversionInputDetail_Insert]"));
				item7.Id = code3;
				LineId++;
			}
			foreach (ConversionOverHeadDetail item6 in obj.ConversionOverHeadDetailList)
			{
				item6.ConversionHeaderId = obj.Id;
				int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[fed].[USP_ConversionOverHeadsDetail_Insert]"));
				item6.Id = code2;
			}
			LineId = 2000;
			foreach (ConversionConsumptionDetail item5 in obj.ConversionConsumptionDetailList)
			{
				item5.LineId = LineId;
				item5.ConversionHeaderId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[fed].[USP_ConversionConsumptionDetail_Insert]"));
				item5.Id = code;
				LineId++;
			}
			InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
			inventoryTransactions.OrganizationId = obj.OrganizationId;
			inventoryTransactions.CompanyId = obj.CompanyId;
			inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
			inventoryTransactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			foreach (ConversionInputDetail item4 in obj.ConversionInputDetailDetailList)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_InventoryValidation";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand.Parameters.AddWithValue("@DocDate", obj.DocDate);
				sqlCommand.Parameters.AddWithValue("@ItemId", item4.ItemId);
				sqlCommand.Parameters.AddWithValue("@WarehouseId", item4.WareHouseId);
				sqlCommand.Parameters.AddWithValue("@NetWeight", item4.NetWeight);
				sqlCommand.ExecuteNonQuery();
			}
			if (obj.ConversionConsumptionDetailList.Count > 0 && obj.ConversionConsumptionDetailList != null)
			{
				foreach (ConversionConsumptionDetail item3 in obj.ConversionConsumptionDetailList)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_InventoryValidation";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@DocDate", obj.DocDate);
					sqlCommand2.Parameters.AddWithValue("@ItemId", item3.ItemId);
					sqlCommand2.Parameters.AddWithValue("@WarehouseId", item3.WareHouseId);
					sqlCommand2.Parameters.AddWithValue("@NetWeight", item3.NetWeight);
					sqlCommand2.ExecuteNonQuery();
				}
			}
			DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
			if (dtvh.Rows.Count > 0)
			{
				VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
				obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				obj.VoucherHeadInvoices.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = obj.VoucherHeadInvoices.Id;
			}
			if (obj.VoucherHeadInvoices.voucherDetailList.Count > 0 && obj.VoucherHeadInvoices.voucherDetailList != null)
			{
				foreach (VoucherDetail item8 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item8.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					ConversionInputDetail sid = obj.ConversionInputDetailDetailList.Find((ConversionInputDetail x) => x.LineId == item8.LineId && x.LineId > 0 && item8.LineId > 0 && item8.CreditAmount > 0.0);
					if (sid != null)
					{
						item8.RefDocSubIdNo = sid.Id;
					}
					ConversionOutPutDetail Op = obj.ConversionOutPutDetailList.Find((ConversionOutPutDetail x) => x.LineId == item8.LineId && x.LineId > 0 && item8.LineId > 0 && item8.DebitAmount > 0.0);
					if (Op != null)
					{
						item8.RefDocSubIdNo = Op.Id;
					}
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_VoucherDetail_Insert"));
				}
				if (obj.VoucherHeadInvoices.Id > 0)
				{
					SqlCommand sqlCommand3 = new SqlCommand();
					sqlCommand3.Connection = sqlcon;
					sqlCommand3.Transaction = sqlTrn;
					sqlCommand3.CommandType = CommandType.StoredProcedure;
					sqlCommand3.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand3.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
					sqlCommand3.ExecuteScalar();
				}
			}
			string AttachmentFolderPath = "";
			ServerCredentials Sc = new ServerCredentials();
			bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (!Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					double FileSize = 0.0;
					string FileFullPath = "";
					if (!IsVpsAttachmentsServiceOn)
					{
						if (item2.FileCustomName != null && item2.FileCustomName != string.Empty)
						{
							System.IO.Path.Combine(AttachmentFolderPath, item2.FileCustomName);
							item2.Attachment = item2.Attachment;
							item2.FileCustomName = item2.FileCustomName;
						}
						else
						{
							FileFullPath = item2.Attachment;
							if (!File.Exists(System.IO.Path.Combine(AttachmentFolderPath, System.IO.Path.GetFileName(item2.Attachment))))
							{
								item2.Attachment = System.IO.Path.GetFileName(FileFullPath);
								string baseName = FileFullPath.Split('?')[0];
								string extension = ((baseName.Split('.').Length > 1) ? baseName.Split('.').Last() : string.Empty);
								string CustomFullName = CommonServices.GenerateCustomFileName() + "." + extension;
								item2.FileCustomName = CustomFullName;
								FileInfo info = new FileInfo(FileFullPath);
								if (info.Exists)
								{
									FileSize = (double)info.Length / 1048576.0;
								}
								if (FileSize > 5.0)
								{
									throw new Exception("File Size Exceeds 5MB Of File: " + item2.Attachment);
								}
								string SavePath = System.IO.Path.Combine(AttachmentFolderPath, item2.FileCustomName);
								if (!File.Exists(SavePath))
								{
									File.Copy(FileFullPath, SavePath, overwrite: true);
								}
							}
							else
							{
								item2.Attachment = System.IO.Path.GetFileName(item2.Attachment);
								item2.FileCustomName = "";
							}
						}
					}
					else if (item2.FileCustomName != null && item2.FileCustomName != string.Empty)
					{
						item2.Attachment = item2.Attachment;
						item2.FileCustomName = item2.FileCustomName;
					}
					else
					{
						bool FileExist = false;
						string uri2 = "ftp://" + Sc.ServerIp + "/" + System.IO.Path.GetFileName(item2.Attachment);
						try
						{
							FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(uri2);
							obj2.Method = "MDTM";
							obj2.UsePassive = true;
							obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using ((FtpWebResponse)obj2.GetResponse())
							{
								FileExist = true;
							}
						}
						catch (WebException ex3)
						{
							if (ex3.Response is FtpWebResponse { StatusCode: not FtpStatusCode.ActionNotTakenFileUnavailable })
							{
								throw new Exception(ex3.Message);
							}
						}
						if (!FileExist)
						{
							FileFullPath = item2.Attachment;
							item2.Attachment = System.IO.Path.GetFileName(FileFullPath);
							string baseName2 = FileFullPath.Split('?')[0];
							string extension2 = ((baseName2.Split('.').Length > 1) ? baseName2.Split('.').Last() : string.Empty);
							string CustomFullName2 = CommonServices.GenerateCustomFileName() + "." + extension2;
							item2.FileCustomName = CustomFullName2;
							FileInfo info2 = new FileInfo(FileFullPath);
							if (info2.Exists)
							{
								FileSize = (double)info2.Length / 1048576.0;
							}
							if (FileSize > 5.0)
							{
								throw new Exception("File Size Exceeds 5MB Of File: " + item2.Attachment);
							}
							string url = "ftp://" + Sc.ServerIp + "/" + item2.FileCustomName;
							try
							{
								FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
								request.Method = "STOR";
								request.UsePassive = true;
								request.UseBinary = true;
								request.KeepAlive = false;
								request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
								byte[] fileContents;
								using (FileStream fileStream = info2.OpenRead())
								{
									fileContents = new byte[info2.Length];
									fileStream.Read(fileContents, 0, fileContents.Length);
								}
								request.ContentLength = fileContents.Length;
								using (Stream requestStream = request.GetRequestStream())
								{
									requestStream.Write(fileContents, 0, fileContents.Length);
								}
								using ((FtpWebResponse)request.GetResponse())
								{
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
						else
						{
							item2.Attachment = System.IO.Path.GetFileName(item2.Attachment);
							item2.FileCustomName = "";
						}
					}
					item2.RefAccountId = obj.EntryUserId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUserId;
					item2.ModifyUser = obj.ModifyUserId;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item in obj.DeleteAttachmentsList.ToList())
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						string Path = AttachmentFolderPath + "\\" + item.Attachment;
						if (File.Exists(Path))
						{
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
							continue;
						}
						throw new Exception("File not exist.");
					}
					string uri = "ftp://" + Sc.ServerIp + "/" + item.Attachment;
					try
					{
						FtpWebRequest obj3 = (FtpWebRequest)WebRequest.Create(uri);
						obj3.Method = "DELE";
						obj3.UsePassive = true;
						obj3.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse deleteResponse = (FtpWebResponse)obj3.GetResponse();
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex5)
		{
			sqlTrn.Rollback();
			throw ex5;
		}
	}

	public List<Architecture.Model.FeedMill.Production.ConversionHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.Production.ConversionHeader> lst = new List<Architecture.Model.FeedMill.Production.ConversionHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FeedMill.Production.ConversionHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Production.ConversionHeader item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionInputDetail"));
						item4.ConversionInputDetailDetailList = GenericProvider.GetAllDetail<ConversionInputDetail>(sqlConnection2, "[fed].[USP_ConversionHeader_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Production.ConversionHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionOutPutDetail"));
						item3.ConversionOutPutDetailList = GenericProvider.GetAllDetail<ConversionOutPutDetail>(sqlConnection3, "[fed].[USP_ConversionHeader_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Production.ConversionHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionOverHeadsDetail"));
						item2.ConversionOverHeadDetailList = GenericProvider.GetAllDetail<ConversionOverHeadDetail>(sqlConnection4, "[fed].[USP_ConversionHeader_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.FeedMill.Production.ConversionHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_ConversionConsumptionDetail"));
					item.ConversionConsumptionDetailList = GenericProvider.GetAllDetail<ConversionConsumptionDetail>(sqlcon, "[fed].[USP_ConversionHeader_GetAllMethod]", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
