using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class LookUps
{
	public static int Save_Lookups(Architecture.Model.pcc.LookUps obj)
	{
		try
		{
			_ = obj.Id;
			return new Architecture.DAL.pcc.LookUps().SetData(obj, "[pcc].[USP_LookUp_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.LookUps ReadById_LookUps(int Id)
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
			new List<Architecture.Model.pcc.LookUps>();
			return new Architecture.DAL.pcc.LookUps().GetAll("[pcc].[USP_LookUps_GetAllMethod]", p)[0];
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.LookUps>("[pcc].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int Save_LookUpType(Architecture.Model.pcc.LookUpType obj)
	{
		try
		{
			_ = obj.Id;
			return new Architecture.DAL.pcc.LookUpType().SetData(obj, "[pcc].[USP_LookUpType_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.LookUpType ReadById_LookUpType(int Id)
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
				Value = "ReadById_LookUpType"
			});
			new List<Architecture.Model.pcc.LookUpType>();
			return new Architecture.DAL.pcc.LookUpType().GetAll("[pcc].[USP_LookUps_GetAllMethod]", p)[0];
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.LookUpType>("[pcc].[USP_LookUps_GetAllMethod]", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.LookUpType>("[pcc].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
