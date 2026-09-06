using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Import;

public class ExImFCBankPaymensts
{
	public int SetDate(Architecture.Model.Import.ExImFCBankPaymensts obj, string ProcName)
	{
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		new List<ExImProceedsChargesType>();
		new List<SupplierCustomer>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (ExImFcBankPaymentsDeductions item4 in obj.ExImFcBankPaymentsDeductionslst)
			{
				item4.ExImFCBankPaymentsId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_ExImFcBankPaymentsDeductions_Insert"));
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
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.SupplierCustomerId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = returnID;
					item3.EntryDate = DateTime.Now;
					item3.ModifyDate = DateTime.Now;
					item3.EntryUser = obj.EntryUser;
					item3.ModifyUser = obj.EntryUser;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			List<SqlParameter> s = new List<SqlParameter>();
			s.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeSrNo",
				Value = obj.Id
			});
			DataTable dtvh = GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_GetMethods", s);
			if (dtvh.Rows.Count > 0)
			{
				VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
				obj.FcBankVoucherHead.Id = VoucherHeadIdGet;
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.FcBankVoucherHead.DocumentTypeSrNo = obj.Id;
				obj.FcBankVoucherHead.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.FcBankVoucherHead, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.FcBankVoucherHead.DocumentTypeSrNo = obj.Id;
				obj.FcBankVoucherHead.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.FcBankVoucherHead, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				obj.FcBankVoucherHead.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = obj.FcBankVoucherHead.Id;
			}
			if (obj.FcBankVoucherHead.voucherDetailList != null && obj.FcBankVoucherHead.voucherDetailList.Count > 0)
			{
				foreach (VoucherDetail item2 in obj.FcBankVoucherHead.voucherDetailList)
				{
					item2.VoucherHeadId = obj.FcBankVoucherHead.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
				obj.FcBankVoucherHead.RefDocNoId = obj.FcBankVoucherHead.Id;
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.FcBankVoucherHead, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.FcBankVoucherHead.voucherDetailList)
				{
					item.VoucherHeadId = obj.FcBankVoucherHead.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
				sqlTrn.Commit();
				return returnID;
			}
			throw new Exception("VoucherDetail List Not Found");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Import.ExImFCBankPaymensts> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.ExImFCBankPaymensts> lst = new List<Architecture.Model.Import.ExImFCBankPaymensts>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Import.ExImFCBankPaymensts>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Import.ExImFCBankPaymensts item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.ExImFcBankPaymentsDeductionslst = GenericProvider.Get<ExImFcBankPaymentsDeductions>(sqlcon, "Sp_ExImFCBankPayments_GetAllMethod", lstPrm);
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
