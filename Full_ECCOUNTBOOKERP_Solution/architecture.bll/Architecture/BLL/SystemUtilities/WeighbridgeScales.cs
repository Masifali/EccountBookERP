using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.SystemUtilities;
using Architecture.Model.SystemUtilities;

namespace Architecture.BLL.SystemUtilities;

public class WeighbridgeScales
{
	public static int Save(Architecture.Model.SystemUtilities.WeighbridgeScales obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SystemUtilities.WeighbridgeScales().Save(obj, "Sp_WeighbridgeScalesList_Insert");
			}
			return new Architecture.DAL.SystemUtilities.WeighbridgeScales().Save(obj, "Sp_WeighbridgeScalesList_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.SystemUtilities.WeighbridgeScales ReadByCompanyId(int OrganizationId, int CompanyId)
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
				Value = "ReadByCompanyId"
			});
			return new Architecture.DAL.SystemUtilities.WeighbridgeScales().ReadByCompanyId("Sp_WeighbridgeScalesList_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
