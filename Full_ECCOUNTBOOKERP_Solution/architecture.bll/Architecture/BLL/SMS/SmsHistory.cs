using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SMS;
using Architecture.Model.SMS;

namespace Architecture.BLL.SMS;

public class SmsHistory
{
	public static int Save(Architecture.Model.SMS.SmsHistory obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SMS.SmsHistory().SetData(obj, "Sp_SMSHistory_Insert");
			}
			return new Architecture.DAL.SMS.SmsHistory().SetData(obj, "Sp_SMSHistory_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SmsStatusUpdate(string Ids)
	{
		try
		{
			new Architecture.DAL.SMS.SmsHistory().SmsStatusUpdate(Ids);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getSmsHistory(int OrganizationId, int CompanyId, int SupplierCustomerId = 0, string ScreenName = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = SupplierCustomerId
				});
			}
			if (ScreenName != null && ScreenName != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = ScreenName
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.MessageBody>("usp_getSmsHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getNotificationDataByGroupId(int OrganizationId, int CompanyId, int GroupId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (GroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GroupId",
					Value = GroupId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SMS.MessageBody>("usp_getNotificationDataByGroupId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
