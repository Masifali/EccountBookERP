using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class InvFoodProductionOverHeads
{
	public int SetData(Architecture.Model.Production.InvFoodProductionOverHeads OverHead, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		int FinancialInActive = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			CommonServices.GetERPFeaturesByCompanyId(OverHead.OrganizationId, OverHead.CompanyId, 5);
			if (Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(OverHead.OrganizationId, OverHead.CompanyId, "InventoryFinancialsEffectsInActive")))
			{
				FinancialInActive = 1;
			}
			if (OverHead.OHDetailRowsRemoveIds != null && OverHead.OHDetailRowsRemoveIds != string.Empty)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_InvFoodProductionOHRowsDeleteByIds";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", OverHead.InvFoodProductionOverHeadsList[0].OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", OverHead.InvFoodProductionOverHeadsList[0].CompanyId);
				sqlCommand.Parameters.AddWithValue("@DocumentTypeId", OverHead.InvFoodProductionOverHeadsList[0].DocumentTypeId);
				sqlCommand.Parameters.AddWithValue("@DetailIds", OverHead.OHDetailRowsRemoveIds);
				sqlCommand.ExecuteNonQuery();
			}
			foreach (Architecture.Model.Production.InvFoodProductionOverHeads obj in OverHead.InvFoodProductionOverHeadsList)
			{
				returnID = 0;
				VoucherHeadIdGet = 0;
				VoucherHeadId = 0;
				returnID = ((obj.Id != 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, "Sp_InvFoodProductionOverHeads_Update")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, "Sp_InvFoodProductionOverHeads_Insert")));
				if (returnID > 0)
				{
					obj.ModifyUser = 0;
					obj.Id = Convert.ToInt32(returnID);
				}
				else
				{
					obj.EntryUser = 0;
					returnID = obj.Id;
				}
				if (FinancialInActive == 1)
				{
					continue;
				}
				VoucherHead voucherHead = new VoucherHead();
				if (obj.ModifyUser > 0)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "Sp_Vouchers_GetMethods";
					sqlCommand2.Parameters.AddWithValue("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
					sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
					sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
					sqlCommand2.Parameters.AddWithValue("@DocumentTypeSrNo", obj.Id);
					VoucherHeadIdGet = (voucherHead.Id = Conversion.ToInt(sqlCommand2.ExecuteScalar()));
				}
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
					Value = obj.InvProductionJobOrderId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "GetGlAccountsByJobOrderId"
				});
				DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Production.InvProductionJobOrder>("Sp_InvProductionJobOrder_GetAllMethod", p);
				if (dt.Rows.Count > 0)
				{
					voucherHead.DocumentTypeSrNo = returnID;
					voucherHead.VoucherDate = obj.DocDate;
					voucherHead.VoucherCode = obj.DocNo;
					voucherHead.DocumentTypeId = obj.DocumentTypeId;
					voucherHead.BaseDocumentTypeId = obj.BaseDocumentTypeId;
					voucherHead.RefAccountId = obj.CharOfAccountId;
					voucherHead.AgainstAccountId = Conversion.ToInt(dt.Rows[0]["WorkInProccessAcId"]);
					voucherHead.EntryDate = DateTime.Now;
					voucherHead.ModifyDate = DateTime.Now;
					voucherHead.VoucherAmount = Conversion.ToDouble(obj.Amount);
					voucherHead.EntryUser = obj.EntryUser;
					voucherHead.ModifyUser = obj.ModifyUser;
					voucherHead.OrganizationId = obj.OrganizationId;
					voucherHead.CompanyId = obj.CompanyId;
					voucherHead.FinancialYearId = obj.FinancialYearId;
					voucherHead.BranchId = obj.BranchesId;
					voucherHead.voucherDetailList = new List<VoucherDetail>();
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = voucherHead.AgainstAccountId;
					vd.AgainstAccountId = Conversion.ToInt(obj.CharOfAccountId);
					vd.Comments = Conversion.ToString(obj.ohRemarks);
					vd.DebitAmount = Conversion.ToDouble(obj.Amount);
					vd.OrderNo = Conversion.ToInt(obj.InvProductionJobOrderId);
					voucherHead.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(obj.CharOfAccountId);
					vd2.AgainstAccountId = voucherHead.AgainstAccountId;
					vd2.Comments = Conversion.ToString(obj.ohRemarks);
					vd2.CreditAmount = Conversion.ToDouble(obj.Amount);
					vd2.OrderNo = Conversion.ToInt(obj.InvProductionJobOrderId);
					voucherHead.voucherDetailList.Add(vd2);
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
						SqlCommand sqlCommand3 = new SqlCommand();
						sqlCommand3.Connection = sqlcon;
						sqlCommand3.Transaction = sqlTrn;
						sqlCommand3.CommandType = CommandType.StoredProcedure;
						sqlCommand3.CommandText = "USP_VoucherBalanceCheck";
						sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand3.Parameters.AddWithValue("@Id", voucherHead.Id);
						sqlCommand3.ExecuteScalar();
					}
					continue;
				}
				throw new Exception("JobOrder WIPAccount Not Found");
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

	public List<Architecture.Model.Production.InvFoodProductionOverHeads> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvFoodProductionOverHeads> lst = new List<Architecture.Model.Production.InvFoodProductionOverHeads>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Production.InvFoodProductionOverHeads>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
