using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.CRM;
using Architecture.Model.CRM;

namespace Architecture.BLL.CRM;

public class CrmSMSTemplates
{
	public static int Save(Architecture.Model.CRM.CrmSMSTemplates obj)
	{
		try
		{
			if (obj.SMSHeaderId == 0)
			{
				return new Architecture.DAL.CRM.CrmSMSTemplates().SetData(obj, "Sp_CrmSMSTemplates_Insert");
			}
			return new Architecture.DAL.CRM.CrmSMSTemplates().SetData(obj, "Sp_CrmSMSTemplates_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.CRM.CrmSMSTemplates> GetAll(Architecture.Model.CRM.CrmSMSTemplates obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "ReadAll"));
			new List<Architecture.Model.CRM.CrmSMSTemplates>();
			return new Architecture.DAL.CRM.CrmSMSTemplates().GetData("Sp_CrmSMSTemplates_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
