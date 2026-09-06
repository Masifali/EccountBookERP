using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.VPS;

namespace Architecture.DAL.Accounts;

public class AccountsPrematureReceipts : DALBase
{
	public int SetData(Architecture.Model.Accounts.AccountsPrematureReceipts obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.AccountsPrematureReceipts detail in obj.AccountsPrematureReceiptsList)
			{
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
				}
				else
				{
					AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(detail.OrganizationId, detail.CompanyId, "Attachment Folder Path");
					if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
				}
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
				{
					obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
					detail.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
					detail.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
				}
				if (detail.Id > 0)
				{
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, detail, "Sp_AccountsPrematurePaymentsReceipts_Update"));
					if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
					{
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlcon;
						sqlCommand.Transaction = sqlTrn;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandText = "Sp_DMSAttachments_GetAllMethod";
						sqlCommand.Parameters.AddWithValue("@Id", detail.Id);
						sqlCommand.Parameters.AddWithValue("@ScreenName", obj.AttachmentsList[0].ScreenName);
						sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
						sqlCommand.ExecuteNonQuery();
					}
				}
				else
				{
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, detail, "Sp_AccountsPrematurePaymentsReceipts_Insert"));
				}
				if (obj.AttachmentsList == null || obj.AttachmentsList.Count <= 0)
				{
					continue;
				}
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = detail.ChartOfAccountIdReceiver;
					item2.RefDocumentTypeId = detail.DocumentTypeId;
					item2.RefDocumentNo = ((returnID > 0) ? returnID : detail.Id);
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = detail.EnteryUserId;
					item2.ModifyUser = detail.ModifyUserId;
					item2.OrganizationId = detail.OrganizationId;
					item2.CompanyId = detail.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public int SetDataForUpdate(Architecture.Model.Accounts.AccountsPrematureReceipts obj, string ProcName)
	{
		int returnID = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			Architecture.Model.Accounts.VoucherHead voucherHead = new Architecture.Model.Accounts.VoucherHead();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherTypeId",
				Value = obj.VouchersId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ReceiverAcId",
				Value = obj.ChartOfAccountIdReceiver
			});
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@FromDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@TrackingSlipRef",
				Value = obj.TrackingSlipRef
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SlipAmount",
				Value = obj.SlipAmount
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByTrackingNo"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountsPrematureReceipts>("Sp_AccountsPrematurePaymentsReceipts_GetAllMethod", p);
			DataTable dataTable = dt;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				voucherHead.DocumentTypeId = Conversion.ToInt(dt.Rows[0]["VoucherTypeId"]);
				if ((from row in dt.AsEnumerable()
					select row["Status"].ToString()).Distinct().ToList().Count > 1)
				{
					throw new Exception("EntryStatus column contains multiple different values.");
				}
				if (obj.EntryStatus.ToUpper() == "CONFIRM")
				{
					if ((from row in dt.AsEnumerable()
						select row["VoucherTypeId"].ToString()).Distinct().ToList().Count > 1)
					{
						throw new Exception("VoucherTypeId column contains multiple different values.");
					}
					if ((from row in dt.AsEnumerable()
						select row["CostCenterId"].ToString()).Distinct().ToList().Count > 1)
					{
						throw new Exception("Can't Confirm Multiple CostCenters Rows.");
					}
					if ((voucherHead.DocumentTypeId == 3 || voucherHead.DocumentTypeId == 4) && (from row in dt.AsEnumerable()
						select row["ReceiverAcId"].ToString()).Distinct().ToList().Count > 1)
					{
						throw new Exception("ReceiverAcId must be the same for VoucherTypeId CASH and BANK.");
					}
					int count = dt.Rows.Count;
					if (!dt.AsEnumerable().All((DataRow row) => (double)row["SlipAmount"] == Conversion.ToDouble(dt.Rows[0]["SlipAmount"])))
					{
						throw new Exception("The Slip Amount of All rows are not same...");
					}
					decimal totalAmount = dt.AsEnumerable().Sum((DataRow row) => row.Field<decimal>("Amount"));
					voucherHead.RefDocNoId = obj.Id;
					voucherHead.RefDocumentTypeId = obj.DocumentTypeId;
					voucherHead.VoucherDate = Conversion.ToDateTime(dt.Rows[0]["DocDate"]);
					voucherHead.VoucherCode = Conversion.ToInt(dt.Rows[0]["DocNo"]);
					voucherHead.RefAccountId = ((voucherHead.DocumentTypeId != 5) ? Conversion.ToInt(dt.Rows[0]["ReceiverAcId"]) : 0);
					voucherHead.ManualBillNo = Conversion.ToString(dt.Rows[0]["TrakingNo"]);
					if (count == 1)
					{
						voucherHead.AgainstAccountId = Conversion.ToInt(dt.Rows[0]["SenderAcId"]);
						voucherHead.ChequeNo = Conversion.ToString(dt.Rows[0]["ChequeNo"]);
						voucherHead.ChequeDate = Conversion.ToDateTime(dt.Rows[0]["ChequeDate"]);
					}
					voucherHead.Remarks = Conversion.ToString(dt.Rows[0]["Remarks"]);
					voucherHead.EntryDate = DateTime.Now;
					voucherHead.ModifyDate = DateTime.Now;
					voucherHead.VoucherAmount = Conversion.ToDouble(totalAmount);
					voucherHead.EntryUser = obj.EnteryUserId;
					voucherHead.ModifyUser = obj.EnteryUserId;
					voucherHead.OrganizationId = obj.OrganizationId;
					voucherHead.CompanyId = obj.CompanyId;
					voucherHead.FinancialYearId = obj.FinancialYearId;
					voucherHead.ActionId = 1;
					voucherHead.voucherDetailList = new List<VoucherDetail>();
					string CreditRemarks = "";
					if (string.IsNullOrEmpty(voucherHead.Remarks))
					{
						CreditRemarks = (voucherHead.Remarks = ((voucherHead.DocumentTypeId != 3) ? ("Amount Received From " + Conversion.ToString(dt.Rows[0]["SenderAc"]) + " In " + Conversion.ToString(dt.Rows[0]["ReceiverAc"])) : ("Cash Received From " + Conversion.ToString(dt.Rows[0]["SenderAc"]) + " In " + Conversion.ToString(dt.Rows[0]["ReceiverAc"]))));
					}
					voucherHead.Remarks = CreditRemarks + " " + voucherHead.Remarks;
					if (Conversion.ToInt(voucherHead.DocumentTypeId) != 3 && Conversion.ToInt(voucherHead.DocumentTypeId) != 4 && Conversion.ToInt(voucherHead.DocumentTypeId) != 5)
					{
						throw new Exception("DocumentTypeId not found...");
					}
					voucherHead.costCenterDetailList = new List<voucherCostCenterDetail>();
					int LineId = 1;
					int SortNo = 1;
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						int CostCenterId = Conversion.ToInt(dt.Rows[i]["CostCenterId"]);
						string Comments = "";
						Comments = ((voucherHead.DocumentTypeId != 3) ? ("Amount Received From " + Conversion.ToString(dt.Rows[i]["SenderAc"]) + " In " + Conversion.ToString(dt.Rows[i]["ReceiverAc"])) : ("Cash Received From " + Conversion.ToString(dt.Rows[i]["SenderAc"]) + " In " + Conversion.ToString(dt.Rows[i]["ReceiverAc"])));
						VoucherDetail vd = new VoucherDetail();
						vd.LineId = LineId;
						vd.SortNo = SortNo;
						vd.AccountId = Conversion.ToInt(dt.Rows[i]["ReceiverAcId"]);
						vd.AgainstAccountId = Conversion.ToInt(dt.Rows[i]["SenderAcId"]);
						vd.Comments = Comments;
						vd.CheqNoDetail = Conversion.ToString(dt.Rows[i]["ChequeNo"]);
						vd.DCheqDate = Conversion.ToDateTime(dt.Rows[i]["ChequeDate"]);
						vd.DebitAmount = Conversion.ToDouble(dt.Rows[i]["Amount"]);
						vd.IsTaxable = "False";
						vd.PaymentType = "Regular";
						vd.CostCenterId = CostCenterId;
						voucherHead.voucherDetailList.Add(vd);
						LineId++;
						VoucherDetail vd2 = new VoucherDetail();
						vd2.LineId = LineId;
						vd2.SortNo = SortNo;
						vd2.AccountId = Conversion.ToInt(dt.Rows[i]["SenderAcId"]);
						vd2.AgainstAccountId = Conversion.ToInt(dt.Rows[i]["ReceiverAcId"]);
						vd2.Comments = Comments;
						vd2.CheqNoDetail = Conversion.ToString(dt.Rows[i]["ChequeNo"]);
						vd2.DCheqDate = Conversion.ToDateTime(dt.Rows[i]["ChequeDate"]);
						vd2.CreditAmount = Conversion.ToDouble(dt.Rows[i]["Amount"]);
						vd2.IsTaxable = "False";
						vd2.PaymentType = "Regular";
						vd2.CostCenterId = CostCenterId;
						voucherHead.voucherDetailList.Add(vd2);
						if (CostCenterId > 0)
						{
							voucherCostCenterDetail costCenterDetail = new voucherCostCenterDetail();
							costCenterDetail.SortNo = SortNo;
							costCenterDetail.costPrcent = 100m;
							costCenterDetail.costAmount = vd.DebitAmount;
							costCenterDetail.CostCenterId = CostCenterId;
							voucherHead.costCenterDetailList.Add(costCenterDetail);
						}
						SortNo++;
					}
					List<SqlParameter> x = new List<SqlParameter>();
					x.Add(new SqlParameter
					{
						ParameterName = "@RefDocumentTypeId",
						Value = obj.DocumentTypeId
					});
					x.Add(new SqlParameter
					{
						ParameterName = "@Id",
						Value = Conversion.ToInt(dt.Rows[0]["Id"])
					});
					x.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "ReadAttachmentsbyRefDocumentTypeId"
					});
					List<DMSAttachments> lst = new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", x);
					if (lst != null && lst.Count > 0)
					{
						foreach (DMSAttachments item5 in lst)
						{
							if (IsVpsAttachmentsServiceOn)
							{
								Sc = CommonServices.GetServerCredentials();
								string url = "ftp://" + Sc.ServerIp + "/" + item5.UploadedFileCustomName;
								try
								{
									FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(url);
									obj2.Method = "RETR";
									obj2.UsePassive = true;
									obj2.UseBinary = true;
									obj2.KeepAlive = false;
									obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
									using FtpWebResponse ftpWebResponse = (FtpWebResponse)obj2.GetResponse();
									using Stream responseStream = ftpWebResponse.GetResponseStream();
									using MemoryStream memoryStream = new MemoryStream();
									responseStream.CopyTo(memoryStream);
									item5.UploadedFileCustomName = GenerateCustomFileNameForBankReceiptVoucher(item5.Attachment, obj.TrackingSlipRef, voucherHead.DocumentTypeId);
									FtpWebRequest obj3 = (FtpWebRequest)WebRequest.Create("ftp://" + Sc.ServerIp + "/" + item5.FileCustomName);
									obj3.Method = "STOR";
									obj3.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
									obj3.UseBinary = true;
									obj3.UsePassive = true;
									obj3.KeepAlive = false;
									using Stream uploadStream = obj3.GetRequestStream();
									memoryStream.Position = 0L;
									memoryStream.CopyTo(uploadStream);
									copiedFiles.Add(item5.UploadedFileCustomName);
								}
								catch (WebException webEx)
								{
									if (webEx.Response is FtpWebResponse response)
									{
										throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
									}
									throw new Exception("Error: " + webEx.Message);
								}
								catch (Exception ex)
								{
									throw new Exception("Error: " + ex.Message);
								}
								continue;
							}
							AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
							if (!Directory.Exists(AttachmentFolderPath))
							{
								throw new Exception("Attachment Folder Path Not Configure Properly");
							}
							string DestinationPath = "";
							DestinationPath = ((item5.UploadedFileCustomName != null && !(item5.UploadedFileCustomName == string.Empty)) ? Path.Combine(AttachmentFolderPath, item5.UploadedFileCustomName) : Path.Combine(AttachmentFolderPath, item5.Attachment));
							FileInfo fileInfo = null;
							if (File.Exists(DestinationPath))
							{
								fileInfo = new FileInfo(DestinationPath);
								string newFileName = GenerateCustomFileNameForBankReceiptVoucher(fileInfo.Name, obj.TrackingSlipRef, voucherHead.DocumentTypeId);
								string newFilePath = Path.Combine(AttachmentFolderPath, newFileName);
								if (!File.Exists(newFilePath))
								{
									File.Copy(DestinationPath, newFilePath, overwrite: true);
									copiedFiles.Add(newFileName);
								}
								item5.UploadedFileCustomName = newFileName;
							}
						}
						voucherHead.AttachmentsValues = string.Join(",", lst.Select((DMSAttachments item) => item.Attachment));
						voucherHead.CustomAttachmentsValues = string.Join(",", lst.Select((DMSAttachments item) => item.UploadedFileCustomName));
					}
					int VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Insert"));
					foreach (VoucherDetail item4 in voucherHead.voucherDetailList)
					{
						item4.VoucherHeadId = VoucherHeadId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_VoucherDetail_Insert"));
					}
					foreach (voucherCostCenterDetail item2 in voucherHead.costCenterDetailList)
					{
						item2.VoucherHeaderId = voucherHead.Id;
						if (voucherHead.voucherDetailList.Any((VoucherDetail row) => row.SortNo == item2.SortNo))
						{
							item2.VoucherDetailId = voucherHead.voucherDetailList.Find((VoucherDetail row) => row.SortNo == item2.SortNo).Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_voucherCostCenterDetail_Insert"));
					}
					foreach (DMSAttachments item3 in lst)
					{
						item3.ScreenName = ((voucherHead.DocumentTypeId == 5) ? "VoucherEntry" : "ReceiptsVoucherNew");
						item3.RefAccountId = voucherHead.RefAccountId;
						item3.RefDocumentTypeId = voucherHead.DocumentTypeId;
						item3.RefDocumentNo = VoucherHeadId;
						item3.EntryDate = DateTime.Now;
						item3.ModifyDate = DateTime.Now;
						item3.EntryUser = obj.EnteryUserId;
						item3.ModifyUser = obj.ModifyUserId;
						item3.OrganizationId = obj.OrganizationId;
						item3.CompanyId = obj.CompanyId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
					}
				}
				foreach (DataRow row2 in dt.Rows)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "Sp_AccountsPrematurePaymentsReceipts_GetAllMethod";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand.Parameters.AddWithValue("@EntryUserId", obj.EnteryUserId);
					sqlCommand.Parameters.AddWithValue("@EntryStatus", obj.EntryStatus);
					sqlCommand.Parameters.AddWithValue("@Id", Conversion.ToInt(row2["Id"]));
					sqlCommand.Parameters.AddWithValue("@Activity", "RecordStatusUpdateById");
					sqlCommand.ExecuteNonQuery();
				}
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("No data found!");
		}
		catch (Exception ex2)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex2;
		}
	}

	private string GenerateCustomFileNameForBankReceiptVoucher(string originalFileName, string transactionId, int VoucherTypeId)
	{
		string fileExtension = Path.GetExtension(originalFileName);
		string newFileName = "";
		switch (VoucherTypeId)
		{
		case 3:
			newFileName = $"CashReceipt_{transactionId}_{Guid.NewGuid()}{fileExtension}";
			break;
		case 4:
			newFileName = $"BankReceipt_{transactionId}_{Guid.NewGuid()}{fileExtension}";
			break;
		case 5:
			newFileName = $"JV_{transactionId}_{Guid.NewGuid()}{fileExtension}";
			break;
		}
		return newFileName;
	}

	public List<Architecture.Model.Accounts.AccountsPrematureReceipts> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.AccountsPrematureReceipts> lst = new List<Architecture.Model.Accounts.AccountsPrematureReceipts>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.AccountsPrematureReceipts>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
