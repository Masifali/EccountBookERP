using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Authentication;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Authentication;

namespace Architecture.BLL.Authentication;

public class ConfigrationsAllocation
{
	public static int Save(Architecture.Model.Authentication.ConfigrationsAllocation configrations)
	{
		try
		{
			if (configrations.Id == 0)
			{
				return new Architecture.DAL.Authentication.ConfigrationsAllocation().Insert(configrations);
			}
			return new Architecture.DAL.Authentication.ConfigrationsAllocation().Update(configrations);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Authentication.ConfigrationsAllocation GetByID(int Id)
	{
		try
		{
			return new Architecture.DAL.Authentication.ConfigrationsAllocation().ReadByID(Id);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Authentication.ConfigrationsAllocation GetByKey(string ConfigName, int OrganizationId, int CompanyId)
	{
		try
		{
			return new Architecture.DAL.Authentication.ConfigrationsAllocation().ReadByKey(ConfigName, OrganizationId, CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Authentication.ConfigrationsAllocation> GetByDefinitionID(Architecture.Model.Authentication.ConfigrationsAllocation configrations)
	{
		try
		{
			return new Architecture.DAL.Authentication.ConfigrationsAllocation().BydefinitionID(configrations);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Authentication.ConfigrationsAllocation> HistoryConfiquration(Architecture.Model.Authentication.ConfigrationsAllocation configrations)
	{
		try
		{
			return new Architecture.DAL.Authentication.ConfigrationsAllocation().History(configrations);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Authentication.ConfigrationsAllocation> HistoryConfiquration(int OrganizationId, int CompanyId)
	{
		try
		{
			return new Architecture.DAL.Authentication.ConfigrationsAllocation().History(OrganizationId, CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetConfigurationByOrgCompandConfigDescription(ReportsParameters obj)
	{
		DataTable dt = null;
		try
		{
			string ConfigKey = "";
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
				ParameterName = "@ConfigDescription",
				Value = obj.ConfigDescription
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetConfigurationByOrgCompandConfigDescription"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Authentication.ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				ConfigKey = Conversion.ToString(dt.Rows[0]["ConfigKey"]);
			}
			return ConfigKey;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetMultipleConfigurationsByConfigDescriptions(ReportsParameters obj)
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
				ParameterName = "@ConfigDescription",
				Value = obj.ConfigDescription
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetMultipleConfigurationsByConfigDescriptions"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Authentication.ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetConfigurationsByDefinitionIds(ReportsParameters obj)
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
				ParameterName = "@DefinitionIds",
				Value = obj.Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetConfigurationsByDefinitionIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Authentication.ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
