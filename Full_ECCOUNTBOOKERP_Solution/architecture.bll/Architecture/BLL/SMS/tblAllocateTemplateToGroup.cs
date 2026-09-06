using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SMS;
using Architecture.Model.SMS;

namespace Architecture.BLL.SMS;

public class tblAllocateTemplateToGroup
{
	public static int Save(Architecture.Model.SMS.tblAllocateTemplateToGroup obj)
	{
		try
		{
			return new Architecture.DAL.SMS.tblAllocateTemplateToGroup().SetData(obj, "Sp_tblAllocateTemplateToGroup_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetTemplate(Architecture.Model.SMS.tblAllocateTemplateToGroup obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetMessageBody"
			});
			if (obj.ScreenName != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.ScreenName
				});
			}
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
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.MessageBody>("Sp_SMS_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
