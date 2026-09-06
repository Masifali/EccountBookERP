using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Export;

namespace Architecture.DAL.Accounts;

public class VoucherInvoicesAdjustment : DALBase
{
	public int SetData(Architecture.Model.Accounts.VoucherInvoicesAdjustment obj, string ProcName, ExImFCBankReceipts objFcyReceipts = null)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.TransactionTypeId == 3)
			{
				using SqlCommand sqlCommandDL = new SqlCommand("[dbo].[USP_FcyBankReceiptPaymentTermDetail_DeleteByHeaderId]", sqlcon, sqlTrn);
				sqlCommandDL.CommandType = CommandType.StoredProcedure;
				sqlCommandDL.Parameters.AddWithValue("@FcyBankReceiptIds", objFcyReceipts.Id);
				sqlCommandDL.ExecuteNonQuery();
			}
			foreach (Architecture.Model.Accounts.VoucherInvoicesAdjustment item3 in obj.VoucherInvoicesAdjustmentList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, ProcName));
			}
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			if (obj.TransactionTypeId == 3)
			{
				List<SqlParameter> s = new List<SqlParameter>();
				s.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = objFcyReceipts.OrganizationId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = objFcyReceipts.CompanyId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = objFcyReceipts.DocumentTypeId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeSrNo",
					Value = objFcyReceipts.Id
				});
				DataTable dtvh = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", s);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					objFcyReceipts.FcBankVoucherHead.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					objFcyReceipts.FcBankVoucherHead.DocumentTypeSrNo = objFcyReceipts.Id;
					objFcyReceipts.FcBankVoucherHead.RefDocNoId = objFcyReceipts.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, objFcyReceipts.FcBankVoucherHead, "Sp_VoucherHead_Insert"));
				}
				else
				{
					objFcyReceipts.FcBankVoucherHead.DocumentTypeSrNo = objFcyReceipts.Id;
					objFcyReceipts.FcBankVoucherHead.RefDocNoId = objFcyReceipts.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, objFcyReceipts.FcBankVoucherHead, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					objFcyReceipts.FcBankVoucherHead.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = objFcyReceipts.FcBankVoucherHead.Id;
				}
				foreach (VoucherDetail item2 in objFcyReceipts.FcBankVoucherHead.voucherDetailList)
				{
					item2.VoucherHeadId = objFcyReceipts.FcBankVoucherHead.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_VoucherBalanceCheck";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", objFcyReceipts.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", objFcyReceipts.CompanyId);
				sqlCommand.Parameters.AddWithValue("@Id", objFcyReceipts.FcBankVoucherHead.Id);
				sqlCommand.ExecuteNonQuery();
				objFcyReceipts.FcBankVoucherHead.RefDocNoId = objFcyReceipts.FcBankVoucherHead.Id;
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, objFcyReceipts.FcBankVoucherHead, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in objFcyReceipts.FcBankVoucherHead.voucherDetailList)
				{
					item.VoucherHeadId = objFcyReceipts.FcBankVoucherHead.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
				}
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
