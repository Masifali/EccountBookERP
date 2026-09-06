using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Production;
using Architecture.Model.Steel.Production;

namespace Architecture.DAL.Steel.Production;

public class InvFoodProductionOverHeads
{
	public int SetData(Architecture.Model.Steel.Production.InvFoodProductionOverHeads overHeads, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Steel.Production.InvFoodProductionOverHeads obj in overHeads.InvFoodProductionOverHeadsList)
			{
				returnID = 0;
				VoucherHeadIdGet = 0;
				VoucherHeadId = 0;
				if (obj.ActionId == 1)
				{
					obj.ModifyUserId = 0;
				}
				else
				{
					obj.EntryUserId = 0;
				}
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, "ST.USP_InvFoodProductionOverHeads_Insert"));
				if (returnID > 0)
				{
					obj.Id = Convert.ToInt32(returnID);
				}
				else
				{
					returnID = obj.Id;
				}
				if (obj.ActionId == 3 || returnID <= 0)
				{
					continue;
				}
				VoucherHead voucherHead = new VoucherHead();
				if (obj.ModifyUserId > 0)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "Sp_Vouchers_GetMethods";
					sqlCommand.Parameters.AddWithValue("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
					sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand.Parameters.AddWithValue("@DocumentTypeSrNo", obj.Id);
					VoucherHeadIdGet = (voucherHead.Id = Conversion.ToInt(sqlCommand.ExecuteScalar()));
				}
				DataTable dt = new DataTable();
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
					ParameterName = "@Id",
					Value = obj.ProductionJobOrderId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "GetGlAccountsByJobOrderId"
				});
				dt = GenericProvider.GetDataTableProc<Architecture.Model.Production.InvProductionJobOrder>("[ST].[USP_ProductionJobOrder_GetAllMethod]", p);
				if (dt.Rows.Count > 0)
				{
					voucherHead.DocumentTypeSrNo = returnID;
					voucherHead.VoucherDate = obj.DocDate;
					voucherHead.VoucherCode = obj.DocNo;
					voucherHead.DocumentTypeId = obj.DocumentTypeId;
					voucherHead.RefAccountId = obj.CharOfAccountId;
					voucherHead.AgainstAccountId = Conversion.ToInt(dt.Rows[0]["WorkInProccessAcId"]);
					voucherHead.EntryDate = DateTime.Now;
					voucherHead.ModifyDate = DateTime.Now;
					voucherHead.VoucherAmount = Conversion.ToDouble(obj.Amount);
					voucherHead.EntryUser = obj.EntryUserId;
					voucherHead.ModifyUser = obj.ModifyUserId;
					voucherHead.OrganizationId = obj.OrganizationId;
					voucherHead.CompanyId = obj.CompanyId;
					voucherHead.FinancialYearId = obj.FinancialYearId;
					voucherHead.voucherDetailList = new List<VoucherDetail>();
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = voucherHead.AgainstAccountId;
					vd.AgainstAccountId = Conversion.ToInt(obj.CharOfAccountId);
					vd.Comments = Conversion.ToString(obj.RemarksOH);
					vd.DebitAmount = Conversion.ToDouble(obj.Amount);
					vd.OrderNo = Conversion.ToInt(obj.ProductionJobOrderId);
					voucherHead.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(obj.CharOfAccountId);
					vd2.AgainstAccountId = voucherHead.AgainstAccountId;
					vd2.Comments = Conversion.ToString(obj.RemarksOH);
					vd2.CreditAmount = Conversion.ToDouble(obj.Amount);
					vd2.OrderNo = Conversion.ToInt(obj.ProductionJobOrderId);
					voucherHead.voucherDetailList.Add(vd2);
				}
				VoucherHeadId = ((VoucherHeadIdGet != 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Update")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Insert")));
				if (VoucherHeadId > 0)
				{
					voucherHead.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = voucherHead.Id;
				}
				foreach (VoucherDetail item in voucherHead.voucherDetailList)
				{
					item.VoucherHeadId = voucherHead.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_Insert"));
				}
				if (voucherHead.Id > 0)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "USP_VoucherBalanceCheck";
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@Id", voucherHead.Id);
					sqlCommand2.ExecuteScalar();
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

	public List<Architecture.Model.Steel.Production.InvFoodProductionOverHeads> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Steel.Production.InvFoodProductionOverHeads> lst = new List<Architecture.Model.Steel.Production.InvFoodProductionOverHeads>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Steel.Production.InvFoodProductionOverHeads>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
