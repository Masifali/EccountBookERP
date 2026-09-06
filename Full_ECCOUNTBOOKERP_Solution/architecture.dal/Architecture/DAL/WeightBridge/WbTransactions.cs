using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.WeightBridge;

namespace Architecture.DAL.WeightBridge;

public class WbTransactions
{
	public int SetDate(Architecture.Model.WeightBridge.WbTransactions obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
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
			if (obj.Id > 0)
			{
				obj.ActionId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, "Sp_WbTransactionsAudit_Insert"));
			}
			VoucherHead voucherHeadWeighBridge = obj.VoucherHeadWeighBridge;
			if (voucherHeadWeighBridge != null && voucherHeadWeighBridge.voucherDetailList?.Count > 0)
			{
				if (obj.ModifyUser > 0)
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
						Value = obj.DocTypeId
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
						obj.VoucherHeadWeighBridge.Id = VoucherHeadIdGet;
					}
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadWeighBridge.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadWeighBridge, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadWeighBridge.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadWeighBridge, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadWeighBridge.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadWeighBridge.Id;
				}
				if (obj.VoucherHeadWeighBridge.voucherDetailList.Count > 0 && obj.VoucherHeadWeighBridge.voucherDetailList != null)
				{
					foreach (VoucherDetail item2 in obj.VoucherHeadWeighBridge.voucherDetailList)
					{
						item2.VoucherHeadId = obj.VoucherHeadWeighBridge.Id;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
					}
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadWeighBridge, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadWeighBridge.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadWeighBridge.Id;
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
