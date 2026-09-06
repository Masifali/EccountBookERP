using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.ContractorWages;

namespace Architecture.DAL.ContractorWages;

public class InvContractorWagesBillHeader
{
	public int SetData(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj, string ProcName, SqlTransaction defaultSqlTran = null, SqlConnection defaultSqlCon = null)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		int returnID = 0;
		obj.QtyTotal = Conversion.ToDouble(obj.QtyTotal);
		bool hasExternalTran = defaultSqlTran != null;
		SqlConnection sqlCon = (hasExternalTran ? defaultSqlCon : ConnectionObject.SQL());
		if (!hasExternalTran)
		{
			sqlCon.Open();
		}
		SqlTransaction sqlTran = (hasExternalTran ? defaultSqlTran : sqlCon.BeginTransaction());
		try
		{
			obj.QtyTotal = Conversion.ToDouble(obj.QtyTotal);
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTran, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (InvContractorWagesBillDetail item3 in obj.invContractWagesBillDateil)
			{
				item3.InvContractorWagesBillHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTran, item3, "Sp_InvContractorWagesBillDetail_Insert"));
			}
			VoucherHead voucherHeadForContractorWages = obj.VoucherHeadForContractorWages;
			if (voucherHeadForContractorWages != null && voucherHeadForContractorWages.voucherDetailList?.Count > 0 && obj.VoucherHeadForContractorWages?.voucherDetailList != null)
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
					obj.VoucherHeadForContractorWages.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadForContractorWages.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTran, obj.VoucherHeadForContractorWages, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadForContractorWages.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTran, obj.VoucherHeadForContractorWages, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadForContractorWages.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadForContractorWages.Id;
				}
				if (obj.VoucherHeadForContractorWages.voucherDetailList.Count > 0 && obj.VoucherHeadForContractorWages.voucherDetailList != null)
				{
					foreach (VoucherDetail item2 in obj.VoucherHeadForContractorWages.voucherDetailList)
					{
						item2.VoucherHeadId = obj.VoucherHeadForContractorWages.Id;
						Convert.ToInt32(GenericProvider.SetProc(sqlTran, item2, "Sp_VoucherDetail_Insert"));
					}
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlCon;
				sqlCommand.Transaction = sqlTran;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_VoucherBalanceCheck";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@Id", obj.VoucherHeadForContractorWages.Id);
				sqlCommand.ExecuteNonQuery();
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTran, obj.VoucherHeadForContractorWages, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadForContractorWages.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadForContractorWages.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTran, item, "Sp_VoucherDetail_H_Insert"));
				}
			}
			else
			{
				SqlCommand sqlCommand2 = new SqlCommand();
				sqlCommand2.Connection = sqlCon;
				sqlCommand2.Transaction = sqlTran;
				sqlCommand2.CommandType = CommandType.StoredProcedure;
				sqlCommand2.CommandText = "usp_ContractorWagesVoucherDeleteByWagesId";
				sqlCommand2.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand2.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand2.Parameters.AddWithValue("@DocumentTypeId", obj.DocumentTypeId);
				sqlCommand2.Parameters.AddWithValue("@Id", obj.Id);
				sqlCommand2.ExecuteNonQuery();
			}
			if (obj.RefDocumentTypeId == 806 || obj.RefDocumentTypeId == 68)
			{
				SqlCommand sqlCommand3 = new SqlCommand();
				sqlCommand3.Connection = sqlCon;
				sqlCommand3.Transaction = sqlTran;
				sqlCommand3.CommandType = CommandType.StoredProcedure;
				sqlCommand3.CommandText = "[dbo].[usp_WagesProportionateToStockEvaluation]";
				sqlCommand3.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand3.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand3.Parameters.AddWithValue("@RefDocumentTypeId", obj.RefDocumentTypeId);
				sqlCommand3.Parameters.AddWithValue("@RefDocNoId", obj.RefDocNoId);
				sqlCommand3.ExecuteNonQuery();
			}
			if (!hasExternalTran)
			{
				sqlTran.Commit();
			}
			return returnID;
		}
		catch
		{
			if (!hasExternalTran)
			{
				sqlTran.Rollback();
			}
			throw;
		}
		finally
		{
			if (!hasExternalTran && sqlCon.State == ConnectionState.Open)
			{
				sqlCon.Close();
			}
		}
	}

	public List<Architecture.Model.ContractorWages.InvContractorWagesBillHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ContractorWages.InvContractorWagesBillHeader> lst = new List<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.ContractorWages.InvContractorWagesBillHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.invContractWagesBillDateil = GenericProvider.Get<InvContractorWagesBillDetail>(sqlcon, "Sp_InvContractorWagesBillHeader_GetAllMethod", lstPrm);
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
