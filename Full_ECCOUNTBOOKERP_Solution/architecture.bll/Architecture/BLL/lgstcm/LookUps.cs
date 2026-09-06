using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.lgstcm;
using Architecture.Model.lgstcm;

namespace Architecture.BLL.lgstcm;

public class LookUps
{
	public static int Save_Lookups(Architecture.Model.lgstcm.LookUps obj)
	{
		try
		{
			return new Architecture.DAL.lgstcm.LookUps().SetData(obj, "[lgstcm].[USP_LookUp_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.lgstcm.LookUps ReadById_LookUps(int Id)
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
				Value = "ReadById_LookUps"
			});
			new List<Architecture.Model.lgstcm.LookUps>();
			return new Architecture.DAL.lgstcm.LookUps().GetAll("[lgstcm].[USP_LookUps_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory_LookUps(int OrganizationId, int CompanyId, int TypeId = 0)
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
			if (TypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = TypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory_LookUps"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.LookUps>("[lgstcm].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory_LookUpType()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory_LookUpType"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<LookUpType>("[lgstcm].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByLookUpTypeId(int OrganizationId, int CompanyId, int LookupTypeId)
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
				ParameterName = "@Id",
				Value = LookupTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetByLookUpTypeId"
			});
			return GenericProvider.GetDataTableProc<LookUpType>("[lgstcm].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LookUpAllServices(int OrganizationId, int CompanyId, string Activity = null)
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
			if (Activity != string.Empty && Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.LookUps>("[lgstcm].[USP_LookAllServices]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
