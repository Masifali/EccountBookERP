using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SMS;
using Architecture.Model;
using Architecture.Model.SMS;

namespace Architecture.BLL.SMS;

public class MessageBody
{
	public static int Save(Architecture.Model.SMS.MessageBody obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SMS.MessageBody().SetData(obj, "Sp_InsertMessageBody");
			}
			return new Architecture.DAL.SMS.MessageBody().SetData(obj, "Sp_UpdateMessageBody");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.SMS.MessageBody GetMessageBody(Architecture.Model.SMS.MessageBody obj)
	{
		DataTable dt = null;
		try
		{
			List<Architecture.Model.SMS.MessageBody> lst = new List<Architecture.Model.SMS.MessageBody>();
			List<SqlParameter> p = new List<SqlParameter>();
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.SMS.MessageBody>("Sp_GetMessageBody", p);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				Architecture.Model.SMS.MessageBody body = new Architecture.Model.SMS.MessageBody();
				body.GroupId = Convert.ToInt32(dt.Rows[i]["GroupId"]);
				body.SMSBody = Convert.ToString(dt.Rows[i]["SMSBody"]);
				lst.Add(body);
			}
			return lst[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetMessageBodytable(Architecture.Model.SMS.MessageBody obj)
	{
		try
		{
			new List<Architecture.Model.SMS.MessageBody>();
			List<SqlParameter> p = new List<SqlParameter>();
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
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.MessageBody>("Sp_GetMessageBody", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetScreenName(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.IsCreateSMSTemplate)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsCreateSMSTemplate",
					Value = obj.IsCreateSMSTemplate
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.MessageBody>("Sp_GetScreenName", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
