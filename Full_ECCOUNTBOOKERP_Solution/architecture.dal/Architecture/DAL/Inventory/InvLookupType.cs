using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Architecture.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class InvLookupType : DALBase
{
	private const string PROC_INSERT = "[dbo].[Proc_InvLookupType_Insert]";

	private const string PROC_UPDATE = "[dbo].[Proc_InvLookupType_Update]";

	private const string PROC_READBYID = "[dbo].[Proc_InvLookupType_ReadById]";

	private const string PROC_READALL = "[dbo].[Proc_InvLookupType_ReadAll]";

	private const string COMPANYID = "CompanyId";

	private const string ENTRYDATE = "EntryDate";

	private const string ENTRYUSER = "EntryUser";

	private const string ID = "Id";

	private const string MODIFYDATE = "ModifyDate";

	private const string MODIFYUSER = "ModifyUser";

	private const string ORGANIZATIONID = "OrganizationId";

	private const string POSTDATE = "PostDate";

	private const string POSTSTATE = "PostState";

	private const string POSTUSER = "PostUser";

	private const string PREFIX = "Prefix";

	private const string PROFILETYPENAME = "ProfileTypeName";

	private const string SERIALFROM = "SerialFrom";

	private const string SERIALTO = "SerialTo";

	public int Insert(Architecture.Model.Inventory.InvLookupType lookupType)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("[dbo].[Proc_InvLookupType_Insert]");
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, lookupType.CompanyId);
			if (Conversion.CheckDateTimeNull(lookupType.EntryDate))
			{
				sqlDataBase.AddInParameter(cmd, "EntryDate", SqlDbType.Date, lookupType.EntryDate);
			}
			else
			{
				sqlDataBase.AddInParameter(cmd, "EntryDate", SqlDbType.DateTime, lookupType.EntryDate);
			}
			if (Conversion.CheckDateTimeNull(lookupType.PostDate))
			{
				sqlDataBase.AddInParameter(cmd, "PostDate", SqlDbType.Date, lookupType.PostDate);
			}
			else
			{
				sqlDataBase.AddInParameter(cmd, "PostDate", SqlDbType.DateTime, lookupType.PostDate);
			}
			if (Conversion.CheckDateTimeNull(lookupType.ModifyDate))
			{
				sqlDataBase.AddInParameter(cmd, "ModifyDate", SqlDbType.NVarChar, lookupType.ModifyDate);
			}
			else
			{
				sqlDataBase.AddInParameter(cmd, "ModifyDate", SqlDbType.NVarChar, lookupType.ModifyDate);
			}
			sqlDataBase.AddInParameter(cmd, "EntryUser", SqlDbType.Int, lookupType.EntryUser);
			sqlDataBase.AddInParameter(cmd, "ModifyUser", SqlDbType.Int, lookupType.ModifyUser);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, lookupType.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "PostState", SqlDbType.Bit, lookupType.PostState);
			sqlDataBase.AddInParameter(cmd, "PostUser", SqlDbType.Int, lookupType.PostUser);
			sqlDataBase.AddInParameter(cmd, "Prefix", SqlDbType.NVarChar, lookupType.Prefix);
			sqlDataBase.AddInParameter(cmd, "ProfileTypeName", SqlDbType.NVarChar, lookupType.ProfileTypeName);
			sqlDataBase.AddInParameter(cmd, "SerialFrom", SqlDbType.Int, lookupType.SerialFrom);
			sqlDataBase.AddInParameter(cmd, "SerialTo", SqlDbType.Int, lookupType.SerialTo);
			return Conversion.ToInt(sqlDataBase.ExecuteScalar(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int Update(Architecture.Model.Inventory.InvLookupType lookupType)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("[dbo].[Proc_InvLookupType_Update]");
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, lookupType.CompanyId);
			if (Conversion.CheckDateTimeNull(lookupType.EntryDate))
			{
				sqlDataBase.AddInParameter(cmd, "EntryDate", SqlDbType.Date, lookupType.EntryDate);
			}
			else
			{
				sqlDataBase.AddInParameter(cmd, "EntryDate", SqlDbType.DateTime, lookupType.EntryDate);
			}
			if (Conversion.CheckDateTimeNull(lookupType.PostDate))
			{
				sqlDataBase.AddInParameter(cmd, "PostDate", SqlDbType.Date, lookupType.PostDate);
			}
			else
			{
				sqlDataBase.AddInParameter(cmd, "PostDate", SqlDbType.DateTime, lookupType.PostDate);
			}
			if (Conversion.CheckDateTimeNull(lookupType.ModifyDate))
			{
				sqlDataBase.AddInParameter(cmd, "ModifyDate", SqlDbType.NVarChar, lookupType.ModifyDate);
			}
			else
			{
				sqlDataBase.AddInParameter(cmd, "ModifyDate", SqlDbType.NVarChar, lookupType.ModifyDate);
			}
			sqlDataBase.AddInParameter(cmd, "EntryUser", SqlDbType.Int, lookupType.EntryUser);
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.Int, lookupType.Id);
			sqlDataBase.AddInParameter(cmd, "ModifyUser", SqlDbType.Int, lookupType.ModifyUser);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, lookupType.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "PostState", SqlDbType.Bit, lookupType.PostState);
			sqlDataBase.AddInParameter(cmd, "PostUser", SqlDbType.Int, lookupType.PostUser);
			sqlDataBase.AddInParameter(cmd, "Prefix", SqlDbType.NVarChar, lookupType.Prefix);
			sqlDataBase.AddInParameter(cmd, "ProfileTypeName", SqlDbType.NVarChar, lookupType.ProfileTypeName);
			sqlDataBase.AddInParameter(cmd, "SerialFrom", SqlDbType.Int, lookupType.SerialFrom);
			sqlDataBase.AddInParameter(cmd, "SerialTo", SqlDbType.Int, lookupType.SerialTo);
			sqlDataBase.ExecuteNonQuery(cmd);
			return lookupType.Id;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Inventory.InvLookupType> ReadAll(Architecture.Model.Inventory.InvLookupType lookupType)
	{
		IDataReader reader = null;
		List<Architecture.Model.Inventory.InvLookupType> List = new List<Architecture.Model.Inventory.InvLookupType>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("[dbo].[Proc_InvLookupType_ReadAll]");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, lookupType.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, lookupType.CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				List.Add(FillDTO(reader));
			}
			return List;
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

	public Architecture.Model.Inventory.InvLookupType ReadByID(int Id)
	{
		Architecture.Model.Inventory.InvLookupType eduLookup = null;
		IDataReader reader = null;
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("[dbo].[Proc_InvLookupType_ReadById]");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.Int, Id);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				eduLookup = FillDTO(reader);
			}
			return eduLookup;
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

	private Architecture.Model.Inventory.InvLookupType FillDTO(IDataReader reader)
	{
		try
		{
			return new Architecture.Model.Inventory.InvLookupType
			{
				CompanyId = Conversion.ToInt(reader["CompanyId"]),
				EntryDate = Conversion.ToDateTime(reader["EntryDate"]),
				EntryUser = Conversion.ToInt(reader["EntryUser"]),
				Id = Conversion.ToInt(reader["Id"]),
				ModifyDate = Conversion.ToDateTime(reader["ModifyDate"]),
				ModifyUser = Conversion.ToInt(reader["ModifyUser"]),
				OrganizationId = Conversion.ToInt(reader["OrganizationId"]),
				PostDate = Conversion.ToDateTime(reader["PostDate"]),
				PostState = Conversion.ToBool(reader["PostState"]),
				PostUser = Conversion.ToInt(reader["PostUser"]),
				Prefix = Conversion.ToString(reader["Prefix"]),
				ProfileTypeName = Conversion.ToString(reader["ProfileTypeName"]),
				SerialFrom = Conversion.ToInt(reader["SerialFrom"]),
				SerialTo = Conversion.ToInt(reader["SerialTo"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
