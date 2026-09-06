using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SMS;
using Architecture.Model.SMS;

namespace Architecture.BLL.SMS;

public class SMSContacts
{
	public static int Save(Architecture.Model.SMS.SMSContacts obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SMS.SMSContacts().SetData(obj, "Sp_tblSMSContacts_Insert");
			}
			return new Architecture.DAL.SMS.SMSContacts().SetData(obj, "Sp_tblSMSContacts_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.SMS.SMSContacts obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.SMSContacts>("Sp_tblSMSContacts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteSMSContacts(Architecture.Model.SMS.SMSContacts obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteByOrganizationCompanyId"
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			GenericProvider.GetDataTableProc<Architecture.Model.SMS.SMSContacts>("Sp_tblSMSContacts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
