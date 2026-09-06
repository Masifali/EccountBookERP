using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.VPS;

namespace Architecture.DAL.Accounts;

public class VoucherHead : DALBase
{
	public int SetData(Architecture.Model.Accounts.VoucherHead obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj != null && obj.costCenterDetailList?.Count > 0)
			{
				obj.CostCenterAmount = obj.costCenterDetailList.Sum((voucherCostCenterDetail x) => x.costAmount);
			}
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
			}
			if (obj.DetailAttachmentsList != null && obj.DetailAttachmentsList.Count > 0)
			{
				obj.DetailAttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DetailAttachmentsList, copiedFiles);
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
				foreach (VoucherDetail item6 in obj.voucherDetailList)
				{
					if (obj.DocumentTypeId == 5)
					{
						if (item6.LineId % 2 == 0)
						{
							item6.SubNo = item6.LineId - 1;
						}
						else
						{
							item6.SubNo = item6.LineId + 1;
						}
					}
					item6.VoucherHeadId = obj.Id;
					LimitAmount += Conversion.ToDecimal(item6.DebitAmount);
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_VoucherDetail_Insert"));
					item6.Id = code;
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
					foreach (voucherCostCenterDetail item7 in obj.costCenterDetailList)
					{
						item7.VoucherHeaderId = obj.Id;
						if (obj.voucherDetailList.Any((VoucherDetail row) => row.SortNo == item7.SortNo))
						{
							item7.VoucherDetailId = obj.voucherDetailList.Find((VoucherDetail row) => row.SortNo == item7.SortNo).Id;
						}
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "USP_voucherCostCenterDetail_Insert"));
					}
				}
				if (obj.VoucherDetailMultiCurrencyList != null && obj.VoucherDetailMultiCurrencyList.Count > 0)
				{
					foreach (VoucherDetailMultiCurrency item5 in obj.VoucherDetailMultiCurrencyList)
					{
						item5.VoucherHeadId = obj.Id;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_VoucherDetailMultiCurrency_Insert"));
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
					CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
				}
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("Voucher Detail Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public int SetDataDetail(VoucherDetail obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
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

	public List<Architecture.Model.Accounts.VoucherHead> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.VoucherHead> lst = new List<Architecture.Model.Accounts.VoucherHead>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Accounts.VoucherHead>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Accounts.VoucherHead item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "VoucherDetail_ReadByVoucherHeadID"));
						item4.voucherDetailList = GenericProvider.Get<VoucherDetail>(sqlConnection2, "Sp_Vouchers_GetMethods", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Accounts.VoucherHead item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@DocumentTypeId", item3.DocumentTypeId));
						lstPrm3.Add(new SqlParameter("@RefDocNoId", item3.Id));
						item3.PaymentDuesSchedules = GenericProvider.Get<PaymentDueSchedule>(sqlConnection3, "Sp_PaymentDueSchedule_ReadAll", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Accounts.VoucherHead item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "voucherCostCenterDetailByHeaderId"));
						item2.costCenterDetailList = GenericProvider.Get<voucherCostCenterDetail>(sqlConnection4, "Sp_Vouchers_GetMethods", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Accounts.VoucherHead item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "VoucherDetailMultiCurrecncyReadByHeaderId"));
					item.VoucherDetailMultiCurrencyList = GenericProvider.Get<VoucherDetailMultiCurrency>(sqlcon, "Sp_Vouchers_GetMethods", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public List<Architecture.Model.Accounts.VoucherHead> GetDateAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.VoucherHead> lst = new List<Architecture.Model.Accounts.VoucherHead>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.VoucherHead>(sqlcon, ProcName, p);
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
