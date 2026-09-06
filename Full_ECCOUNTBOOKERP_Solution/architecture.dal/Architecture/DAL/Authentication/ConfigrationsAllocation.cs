using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Architecture.Common;
using Architecture.Model.Authentication;

namespace Architecture.DAL.Authentication;

public class ConfigrationsAllocation : DALBase
{
	private const string PROC_INSERT = "Proc_ConfigrationsAllocation_Insert";

	private const string PROC_UPDATE = "Proc_ConfigrationsAllocation_Update";

	private const string PROC_HISTORY = "Proc_ConfigrationsAllocation_History";

	private const string PROC_READBYID = "Proc_ConfigrationsAllocation_ReadById";

	private const string PROC_READBYKey = "Proc_ConfigrationsAllocation_ReadByKey";

	private const string PROC_DEFINITIONID = "Proc_ConfigrationsAllocation_ReadByDefinitionID";

	private const string ID = "Id";

	private const string CONFIGRATIONSDEFINITIONID = "ConfigrationsDefinitionId";

	private const string CONFIGDESCRIPTION = "ConfigDescription";

	private const string CONFIGMODULEDESCRIPTION = "ConfigModuleDescription";

	private const string CONFIGVALUE = "ConfigValue";

	private const string CONFIGKEY = "ConfigKey";

	private const string ISACTIVE = "IsActive";

	private const string ORGANIZATIONID = "OrganizationId";

	private const string COMPANYID = "CompanyId";

	private const string EntryUserId = "EntryUserId";

	private const string ModifyUserId = "ModifyUserId";

	public int Insert(Architecture.Model.Authentication.ConfigrationsAllocation configrations)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_ConfigrationsAllocation_Insert");
			sqlDataBase.AddInParameter(cmd, "ConfigrationsDefinitionId", SqlDbType.Int, configrations.ConfigrationsDefinitionId);
			sqlDataBase.AddInParameter(cmd, "ConfigValue", SqlDbType.NVarChar, configrations.ConfigValue);
			sqlDataBase.AddInParameter(cmd, "ConfigKey", SqlDbType.NVarChar, configrations.ConfigKey);
			sqlDataBase.AddInParameter(cmd, "IsActive", SqlDbType.Bit, configrations.IsActive);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, configrations.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, configrations.CompanyId);
			sqlDataBase.AddInParameter(cmd, "EntryUserId", SqlDbType.Int, configrations.EntryUserId);
			sqlDataBase.AddInParameter(cmd, "ModifyUserId", SqlDbType.Int, configrations.ModifyUserId);
			return Conversion.ToInt(sqlDataBase.ExecuteScalar(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int Update(Architecture.Model.Authentication.ConfigrationsAllocation configrations)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_ConfigrationsAllocation_Update");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.Int, configrations.Id);
			sqlDataBase.AddInParameter(cmd, "ConfigrationsDefinitionId", SqlDbType.Int, configrations.ConfigrationsDefinitionId);
			sqlDataBase.AddInParameter(cmd, "ConfigValue", SqlDbType.NVarChar, configrations.ConfigValue);
			sqlDataBase.AddInParameter(cmd, "ConfigKey", SqlDbType.NVarChar, configrations.ConfigKey);
			sqlDataBase.AddInParameter(cmd, "IsActive", SqlDbType.Bit, configrations.IsActive);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, configrations.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, configrations.CompanyId);
			sqlDataBase.AddInParameter(cmd, "EntryUserId", SqlDbType.Int, configrations.EntryUserId);
			sqlDataBase.AddInParameter(cmd, "ModifyUserId", SqlDbType.Int, configrations.ModifyUserId);
			sqlDataBase.ExecuteNonQuery(cmd);
			return configrations.Id;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Authentication.ConfigrationsAllocation> History(Architecture.Model.Authentication.ConfigrationsAllocation configrations)
	{
		IDataReader reader = null;
		List<Architecture.Model.Authentication.ConfigrationsAllocation> configlist = new List<Architecture.Model.Authentication.ConfigrationsAllocation>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_ConfigrationsAllocation_History");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, configrations.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, configrations.CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				configlist.Add(FillDTO(reader));
			}
			return configlist;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	public List<Architecture.Model.Authentication.ConfigrationsAllocation> History(int OrganizationId, int CompanyId)
	{
		IDataReader reader = null;
		List<Architecture.Model.Authentication.ConfigrationsAllocation> configlist = new List<Architecture.Model.Authentication.ConfigrationsAllocation>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_ConfigrationsAllocation_History");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				configlist.Add(FillDTO(reader));
			}
			return configlist;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	public List<Architecture.Model.Authentication.ConfigrationsAllocation> BydefinitionID(Architecture.Model.Authentication.ConfigrationsAllocation defId)
	{
		IDataReader reader = null;
		List<Architecture.Model.Authentication.ConfigrationsAllocation> Deflist = new List<Architecture.Model.Authentication.ConfigrationsAllocation>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_ConfigrationsAllocation_ReadByDefinitionID");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, defId.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, defId.CompanyId);
			sqlDataBase.AddInParameter(cmd, "ConfigrationsDefinitionId", SqlDbType.Int, defId.ConfigrationsDefinitionId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				Deflist.Add(FillDTO(reader));
			}
			return Deflist;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	public Architecture.Model.Authentication.ConfigrationsAllocation ReadByID(int Id)
	{
		Architecture.Model.Authentication.ConfigrationsAllocation configrations = null;
		IDataReader reader = null;
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_ConfigrationsAllocation_ReadById");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.Int, Id);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				configrations = FillDTO(reader);
			}
			return configrations;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	public Architecture.Model.Authentication.ConfigrationsAllocation ReadByKey(string ConfigName, int OrganizationId, int CompanyId)
	{
		Architecture.Model.Authentication.ConfigrationsAllocation configrations = null;
		IDataReader reader = null;
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_ConfigrationsAllocation_ReadByKey");
			sqlDataBase.AddInParameter(cmd, "ConfigDescription", SqlDbType.NVarChar, ConfigName);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				configrations = FillDTO(reader);
			}
			return configrations;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	public Architecture.Model.Authentication.ConfigrationsAllocation FillDTO(IDataReader reader)
	{
		try
		{
			return new Architecture.Model.Authentication.ConfigrationsAllocation
			{
				Id = Conversion.ToInt(reader["Id"]),
				ConfigrationsDefinitionId = Conversion.ToInt(reader["ConfigrationsDefinitionId"]),
				ConfigValue = Conversion.ToString(reader["ConfigValue"]),
				ConfigKey = Conversion.ToString(reader["ConfigKey"]),
				IsActive = Conversion.ToBool(reader["IsActive"]),
				OrganizationId = Conversion.ToInt(reader["OrganizationId"]),
				CompanyId = Conversion.ToInt(reader["CompanyId"]),
				ConfigModuleDescription = Conversion.ToString(reader["ConfigModuleDescription"]),
				ConfigDescription = Conversion.ToString(reader["ConfigDescription"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
