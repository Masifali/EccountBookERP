using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class FcyBankReceiptBreakUp
{
	public static int Save(Architecture.Model.Export.FcyBankReceiptBreakUp obj)
	{
		try
		{
			return new Architecture.DAL.Export.FcyBankReceiptBreakUp().SetData(obj, "[dbo].[USP_FcyBankReceiptBreakUp_Insert]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
	{
		try
		{
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.SupplierCustomerId != 0 && obj.GlAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("[dbo].[Usp_GDUtilizationMainHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGDUtilizationHistoryByFcyReceipt(int FcyReceiptId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = FcyReceiptId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.FcyBankReceiptBreakUp>("[dbo].[Usp_GetGDUtilizationHistoryByFcyReceipt]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByFcyReceipt(int FcyReceiptId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[Sp_ExImFCBankReceipts_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@Id", FcyReceiptId);
			sqlCommand.Parameters.AddWithValue("@Activity", "GdUtilizeDeleteByFcyId");
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static List<Architecture.Model.Export.FcyBankReceiptBreakUp> GetGDUtilizationHistoryByFcyReceiptId(int FcyReceiptId)
	{
		try
		{
			new List<Architecture.Model.Export.FcyBankReceiptBreakUp>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = FcyReceiptId
			});
			return new Architecture.DAL.Export.FcyBankReceiptBreakUp().GetData("[dbo].[Usp_GetGDUtilizationHistoryByFcyReceipt]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
