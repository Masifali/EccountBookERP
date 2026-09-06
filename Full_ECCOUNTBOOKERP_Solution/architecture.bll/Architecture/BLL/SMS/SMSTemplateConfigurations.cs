using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.SMS;

namespace Architecture.BLL.SMS;

public class SMSTemplateConfigurations
{
	public static DataTable GetScreensActiveUsers(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetMessageActiveReceivers"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenName",
				Value = obj.ScreenName
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
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.SMSReceivers>("Sp_SMS_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetScreens(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetScreensByModuleId"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ModuleId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ScreenRights>("Sp_ScreenDefinition_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
