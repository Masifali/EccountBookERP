using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.WeightBridge;
using Architecture.Model.WeightBridge;

namespace Architecture.BLL.WeightBridge;

public class WbPartyandItemsDefine
{
	public static int Save(Architecture.Model.WeightBridge.WbPartyandItemsDefine obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.WeightBridge.WbPartyandItemsDefine().SetDate(obj, "Sp_WbPartyandItemsDefine_Insert");
			}
			return new Architecture.DAL.WeightBridge.WbPartyandItemsDefine().SetDate(obj, "Sp_WbPartyandItemsDefine_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.WeightBridge.WbPartyandItemsDefine obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbPartyandItemsDefine>("SP_WbPartyandItemsDefine_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWbParties(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbPartyandItemsDefine>("Usp_GetWbParties", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
