using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ThirdPartyType
{
	public static int Save(Architecture.Model.Export.ThirdPartyType obj)
	{
		try
		{
			return new Architecture.DAL.Export.ThirdPartyType().SetData(obj, "USP_ThirdPartyType_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(int OrganizationId, int CompanyId, int Id = 0, bool ActiveOnly = false)
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
			if (Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			if (ActiveOnly)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsActive",
					Value = ActiveOnly
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ThirdPartyType>("USP_ThirdPartyType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ComboBind(int OrganizationId, int CompanyId)
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
				ParameterName = "@Activity",
				Value = "ComboBind"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ThirdPartyType>("USP_ThirdPartyType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ThirdPartyType ReadById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.Export.ThirdPartyType>();
			return new Architecture.DAL.Export.ThirdPartyType().GetAll("[dbo].[USP_ThirdPartyType_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LotTrackingCurrentStage()
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			using SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[V_LotTrackingCurrentStage]", sqlcon);
			cmd.CommandType = CommandType.Text;
			using SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void DeleteByIDs(int Id)
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
			sqlCommand.CommandText = "USP_ThirdPartyType_GetAllMethod";
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
