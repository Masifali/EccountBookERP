using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class Applications
{
	public static DataTable getApplicationsByMasterAppId(int MasterAppId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "getApplicationsByMasterAppId"
			});
			if (MasterAppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MasterAppId",
					Value = MasterAppId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.AppModules>("usp_Applications_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<ApplicationsAllocateToScreen_Retrieval> ReadByScreenId(int ScreenId)
	{
		try
		{
			new List<ApplicationsAllocateToScreen_Retrieval>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenId",
				Value = ScreenId
			});
			return Conversion.ConvertDataTableToList<ApplicationsAllocateToScreen_Retrieval>(GenericProvider.GetDataTableProc<ApplicationsAllocateToScreen_Retrieval>("[dbo].[Usp_ApplicationsAllocateToScreen]", p));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
