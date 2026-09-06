using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.BLL.Authentication;
using Architecture.DAL.SystemUtilities;
using Architecture.Model;
using Architecture.Model.SystemUtilities;

namespace Architecture.BLL.SystemUtilities;

public class Email
{
	public static int SaveEmailGatewayInfo(EmailGatewayInfo obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SystemUtilities.Email().SaveEmailGatewayInfo(obj, "Sp_EmailGatewayInfo_Insert");
			}
			return new Architecture.DAL.SystemUtilities.Email().SaveEmailGatewayInfo(obj, "Sp_EmailGatewayInfo_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveList(EmailHistory obj)
	{
		try
		{
			EmailGatewayInfo EmailGatewayInfo = GetEmailGatewayInfoByCompanyId(obj.OrganizationId, obj.CompanyId);
			string ServerFilePathUrl = "";
			string ConfiguredPath = ConfigrationsAllocation.GetConfigurationByOrgCompandConfigDescription(new ReportsParameters
			{
				OrganizationId = obj.OrganizationId,
				CompanyId = obj.CompanyId,
				ConfigDescription = "ShareLocalDrivePath"
			});
			if (ConfiguredPath != string.Empty)
			{
				ServerFilePathUrl = ConfiguredPath.ToString() + "\\";
			}
			foreach (EmailHistory item in obj.EmailHistoryList)
			{
				item.EmailFrom = EmailGatewayInfo.EmailAddress;
				if (item.FileUrl1 != string.Empty && item.FileUrl1 != null)
				{
					string AttachmentPath2 = ServerFilePathUrl + "\\" + item.FileUrl1 + ".pdf";
					item.FileUrl1 = AttachmentPath2;
				}
				if (item.FileUrl2 != string.Empty && item.FileUrl2 != null)
				{
					string AttachmentPath = ServerFilePathUrl + "\\" + item.FileUrl2 + ".pdf";
					item.FileUrl2 = AttachmentPath;
				}
			}
			new Architecture.DAL.SystemUtilities.Email().SaveList(obj, "Sp_EmailHistory_Insert");
			new Architecture.DAL.SystemUtilities.Email().SendEmail(obj.EmailHistoryList, EmailGatewayInfo);
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static EmailGatewayInfo GetEmailGatewayInfoByCompanyId(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByCompanyId"
			});
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
			List<EmailGatewayInfo> lst = new List<EmailGatewayInfo>();
			lst = new Architecture.DAL.SystemUtilities.Email().GetEmailGatewayInfoByCompanyId("Sp_EmailGatewayInfo_GetAll", p);
			if (lst.Count > 0)
			{
				return lst[0];
			}
			return null;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
