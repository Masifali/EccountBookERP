using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.BankSchema;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.BankSchema;

namespace Architecture.BLL.BankSchema;

public class PayeeTitle
{
	public static long save(Architecture.Model.BankSchema.PayeeTitle obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.payeeTitleId == 0L)
			{
				obj.ActionId = 1;
			}
			else
			{
				obj.ActionId = 2;
			}
			return new Architecture.DAL.BankSchema.PayeeTitle().SetData(obj, "[Bank].[USP_PayeeTitle_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw ex;
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
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryFromDate",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryToDate",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyFromDate",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyToDate",
					Value = obj.ModifyDateTo
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<PayeeTitle>("[Bank].[USP_payeeTitle_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable payeeTitle_ByGlAccountId(int OrganizationId, int CompanyId, int GlAccountId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GlAccountId",
				Value = GlAccountId
			});
			return GenericProvider.GetDataTableProc<PayeeTitle>("[Bank].[USP_payeeTitle_ByGlAccountId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int Id, int EntryUserId)
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
			sqlCommand.CommandText = "[Bank].[USP_payeeTitle_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static Architecture.Model.BankSchema.PayeeTitle ReadById(int id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = id
			});
			new List<Architecture.Model.BankSchema.PayeeTitle>();
			return new Architecture.DAL.BankSchema.PayeeTitle().GetData("[Bank].[USP_PayeeTitle_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetDataForDropDownFrompayeeTitle(ReportsParameters obj)
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
			if (obj.Activity != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.BankSchema.pdcTransaction>("[Bank].[USP_GetDataForDropDownFrompayeeTitle]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
