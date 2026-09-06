using System;
using System.Collections.Generic;
using Architecture.DAL;
using Architecture.Model;

namespace Architecture.BLL;

public class SMSTemplateConfigurations
{
	public static List<Architecture.Model.AppModules> GetAllModules(Architecture.Model.AppModules AppModules)
	{
		try
		{
			return new Architecture.DAL.SMSTemplateConfigurations().ReadAllModules(AppModules);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<SMSReceivers> GetScreensUsers(int Id, int OrganizationId, int CompanyId)
	{
		try
		{
			return new Architecture.DAL.SMSTemplateConfigurations().GetScreensUsers(Id, OrganizationId, CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<SMSReceivers> GetScreensActiveUsers(string ScreenName, int OrganizationId, int CompanyId)
	{
		try
		{
			return new Architecture.DAL.SMSTemplateConfigurations().GetScreensActiveUsers(ScreenName, OrganizationId, CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int DeleteSMSReceivers(int ScreenId, int OrganizationId, int CompanyId)
	{
		try
		{
			return new Architecture.DAL.SMSTemplateConfigurations().DeleteSMSReceivers(ScreenId, OrganizationId, CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveSMSReceivers(SMSReceiverInsert SMSReceiverInsert)
	{
		try
		{
			return new Architecture.DAL.SMSTemplateConfigurations().InsertSMSReceivers(SMSReceiverInsert);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
