using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Architecture.DAL.Common;
using Architecture.Model.SystemUtilities;

namespace Architecture.DAL.SystemUtilities;

public class Email
{
	public int SaveEmailGatewayInfo(EmailGatewayInfo obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public int SaveList(EmailHistory obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (EmailHistory item in obj.EmailHistoryList)
			{
				item.OrganizationId = obj.OrganizationId;
				item.CompanyId = obj.CompanyId;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public int SendEmail(List<EmailHistory> EmailsList, EmailGatewayInfo EmailGatewayInfo)
	{
		using (List<EmailHistory>.Enumerator enumerator = EmailsList.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				EmailHistory item = enumerator.Current;
				MailMessage MailMessage = new MailMessage();
				string[] EmailTo = item.EmailTo.Split(',');
				if (EmailTo.Length != 0)
				{
					for (int i = 0; i < EmailTo.Length; i++)
					{
						MailMessage.CC.Add(EmailTo[i]);
					}
				}
				MailMessage.From = new MailAddress(item.EmailFrom, "EccountBook ERP");
				MailMessage.Subject = item.EmailSubject;
				MailMessage.IsBodyHtml = false;
				MailMessage.Body = item.EmailBody;
				MailMessage.Priority = MailPriority.Normal;
				MailMessage.BodyEncoding = Encoding.UTF8;
				MailMessage.SubjectEncoding = Encoding.UTF8;
				if (item.FileUrl1 != string.Empty && item.FileUrl1 != null)
				{
					Attachment EmailAttachment2 = new Attachment(item.FileUrl1);
					MailMessage.Attachments.Add(EmailAttachment2);
				}
				if (item.FileUrl2 != string.Empty && item.FileUrl2 != null)
				{
					Attachment EmailAttachment = new Attachment(item.FileUrl2);
					MailMessage.Attachments.Add(EmailAttachment);
				}
				SmtpClient client = new SmtpClient();
				client.Port = EmailGatewayInfo.ServerPort;
				client.Host = EmailGatewayInfo.OutgoingSMTP;
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(EmailGatewayInfo.EmailAddress, EmailGatewayInfo.Password);
				client.EnableSsl = true;
				try
				{
					client.Send(MailMessage);
					return 0;
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				finally
				{
					MailMessage.Attachments.Dispose();
					MailMessage.Dispose();
				}
			}
		}
		return 0;
	}

	public int SendAuthenticationEmail(int OrganizationId, int CompanyId, string ToEmail, string OTP)
	{
		try
		{
			MailMessage MailMessage = new MailMessage("hrmsoftware2025@gmail.com", ToEmail);
			MailMessage.From = new MailAddress("hrmsoftware2025@gmail.com", "EccountBook ERP");
			MailMessage.Subject = "EccountBook Verification Code";
			MailMessage.IsBodyHtml = true;
			string Message = string.Empty;
			string relativePath = "AuthenticationTemplate.html";
			string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
			Reportlogwriteondrive("TemplatePath : " + fullPath);
			Message = new StreamReader(new FileStream(fullPath, FileMode.Open, FileAccess.Read), Encoding.UTF8).ReadToEnd();
			Message = Message.Replace("SIX_DIGIT_OTP", OTP);
			MailMessage.Body = Message;
			MailMessage.Priority = MailPriority.Normal;
			MailMessage.BodyEncoding = Encoding.UTF8;
			MailMessage.SubjectEncoding = Encoding.UTF8;
			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Port = 587;
			smtpClient.Host = "smtp.gmail.com";
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential("hrmsoftware2025@gmail.com", "nyta eeko fule vbys");
			smtpClient.EnableSsl = true;
			smtpClient.Send(MailMessage);
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public List<EmailGatewayInfo> GetEmailGatewayInfoByCompanyId(string ProcName, List<SqlParameter> p = null)
	{
		List<EmailGatewayInfo> lst = new List<EmailGatewayInfo>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<EmailGatewayInfo>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public static void Reportlogwriteondrive(string strMessage, bool IsClear = false)
	{
		try
		{
			if (IsClear)
			{
				File.WriteAllText("C:\\ReportLog.txt", string.Empty);
			}
			using StreamWriter sw = new StreamWriter("C:\\ReportLog.txt", append: true);
			sw.WriteLine(DateTime.Now.ToString() + "----" + strMessage);
		}
		catch
		{
		}
	}
}
