using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Architecture.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class SMSTemplateConfigurations : DALBase
{
	private const string PROC_READALLMODULES = "Proc_GetModules";

	private const string PROC_READSCREENS = "Proc_GetScreens";

	private const string PROC_GETSCREENSMSRECEIVERS = "Proc_GetMessageReceivers";

	private const string PROC_INSERTMESSAGERECEIVERS = "Proc_InsertMessageReceivers";

	private const string PROC_DELETEMESSAGERECEIVERS = "Proc_DeleteMessageReceivers";

	private const string PROC_GETSCREENSMSRECEIVERSACTIVE = "Proc_GetMessageActiveReceivers";

	private const string MODULEDESCRIPTION = "ModuleDescription";

	private const string ID = "Id";

	private const string MODULEID = "ModuleId";

	private const string SCREENNAME = "ScreenName";

	private const string SCREENALIAS = "ScreenAlias";

	private const string SCREENID = "ScreenId";

	private const string ORGANIZATIONID = "OrganizationId";

	private const string COMPANYID = "CompanyId";

	private const string COMPANYNAME = "CompanyName";

	private const string ADDRESS1 = "Address1";

	private const string NAME = "Name";

	private const string PHONE = "Phone";

	private const string MOBILEOFFICE = "MobileOffice";

	private const string MOBILEPERSONAL = "MobilePersonal";

	private const string ADDED = "Added";

	private const string TOCOMPANYID = "ToCompanyId";

	private const string DATE = "Date";

	private const string MOBILENO = "MobileNo";

	private const string REFERENCEDOCNO = "ReferenceDocNo";

	private const string REFERENCEDOCTYPEID = "ReferenceDocTypeId";

	public List<Architecture.Model.AppModules> ReadAllModules(Architecture.Model.AppModules AppModules)
	{
		IDataReader reader = null;
		List<Architecture.Model.AppModules> AppModulelist = new List<Architecture.Model.AppModules>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_GetModules");
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				AppModulelist.Add(FillDTOMODULES(reader));
			}
			return AppModulelist;
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

	public Architecture.Model.AppModules FillDTOMODULES(IDataReader reader)
	{
		try
		{
			return new Architecture.Model.AppModules
			{
				Id = Conversion.ToInt(reader["Id"]),
				ModuleDescription = Conversion.ToString(reader["ModuleDescription"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Screens> GetScreens(int ScreenId)
	{
		IDataReader reader = null;
		List<Screens> Screens = new List<Screens>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_GetScreens");
			sqlDataBase.AddInParameter(cmd, "ModuleId", SqlDbType.Int, ScreenId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				Screens.Add(FillDTOSCREENS(reader));
			}
			return Screens;
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

	public Screens FillDTOSCREENS(IDataReader reader)
	{
		try
		{
			return new Screens
			{
				Id = Conversion.ToInt(reader["Id"]),
				ScreenName = Conversion.ToString(reader["ScreenName"]),
				ScreenAlias = Conversion.ToString(reader["ScreenAlias"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<SMSReceivers> GetScreensUsers(int ScreenId, int OrganizationId, int CompanyId)
	{
		IDataReader reader = null;
		List<SMSReceivers> SMSReceivers = new List<SMSReceivers>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_GetMessageReceivers");
			sqlDataBase.AddInParameter(cmd, "ScreenId", SqlDbType.Int, ScreenId);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				SMSReceivers.Add(FillDTOSCREENRECEIVERS(reader));
			}
			return SMSReceivers;
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

	public List<SMSReceivers> GetScreensActiveUsers(string ScreenName, int OrganizationId, int CompanyId)
	{
		IDataReader reader = null;
		List<SMSReceivers> SMSReceivers = new List<SMSReceivers>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_GetMessageActiveReceivers");
			sqlDataBase.AddInParameter(cmd, "ScreenName", SqlDbType.NVarChar, ScreenName);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				SMSReceivers.Add(FillDTOSCREENRECEIVERS(reader));
			}
			return SMSReceivers;
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

	public SMSReceivers FillDTOSCREENRECEIVERS(IDataReader reader)
	{
		try
		{
			return new SMSReceivers
			{
				Id = Conversion.ToInt(reader["Id"]),
				Name = Conversion.ToString(reader["Name"]),
				MobileNo = Conversion.ToString(reader["MobileNo"]),
				ReferenceDocNo = Conversion.ToString(reader["ReferenceDocNo"]),
				ReferenceDocTypeId = Conversion.ToInt(reader["ReferenceDocTypeId"]),
				OrganizationId = Conversion.ToInt(reader["OrganizationId"]),
				CompanyId = Conversion.ToInt(reader["CompanyId"]),
				Added = Conversion.ToBool(reader["Added"]),
				Date = Conversion.ToDateTime(reader["Date"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int InsertSMSReceivers(SMSReceiverInsert SMSReceiverInsert)
	{
		DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_InsertMessageReceivers");
		sqlDataBase.AddInParameter(cmd, "ScreenId", SqlDbType.Int, SMSReceiverInsert.Screen_Id);
		sqlDataBase.AddInParameter(cmd, "ToCompanyId", SqlDbType.Int, SMSReceiverInsert.ToCompanyId);
		sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, SMSReceiverInsert.OrganizationId);
		sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Bit, SMSReceiverInsert.CompanyId);
		sqlDataBase.AddInParameter(cmd, "Date", SqlDbType.Date, Conversion.ToDateTime(SMSReceiverInsert.Date));
		return Conversion.ToInt(sqlDataBase.ExecuteScalar(cmd));
	}

	public int DeleteSMSReceivers(int ScreenId, int OrganizationId, int CompanyId)
	{
		DbCommand cmd = sqlDataBase.GetStoredProcCommand("Proc_DeleteMessageReceivers");
		sqlDataBase.AddInParameter(cmd, "ScreenId", SqlDbType.Int, ScreenId);
		sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
		sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
		return Conversion.ToInt(sqlDataBase.ExecuteScalar(cmd));
	}
}
