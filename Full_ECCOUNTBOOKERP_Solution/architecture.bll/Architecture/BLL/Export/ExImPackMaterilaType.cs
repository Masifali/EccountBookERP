using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImPackMaterilaType
{
	public static int Save(Architecture.Model.Export.ExImPackMaterilaType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImPackMaterilaType().SetData(obj, "Sp_ExImPackMaterilaType_Insert");
			}
			return new Architecture.DAL.Export.ExImPackMaterilaType().SetData(obj, "Sp_ExImPackMaterilaType_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImPackMaterilaType GetByID(int Id)
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
				Value = "ReadById"
			});
			new List<Architecture.Model.Export.ExImPackMaterilaType>();
			return new Architecture.DAL.Export.ExImPackMaterilaType().GetAll("Sp_ExImPackMaterilaType_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Export.ExImPackMaterilaType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImPackMaterilaType>("Sp_ExImPackMaterilaType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
