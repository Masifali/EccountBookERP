using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Architecture.DAL.SMS;
using Architecture.Model.SMS;

namespace Architecture.BLL.SMS;

internal class SmsProvider
{
	public static Architecture.Model.SMS.SmsProvider ReadByCompanyId(int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByCompanyId"
			});
			return new Architecture.DAL.SMS.SmsProvider().GetData("Sp_SmsProvider_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string SendSmsViaLifeTimeSmsApi(Architecture.Model.SMS.SmsProvider ProviderInfo, List<string> ToNumbers, string SmsMessage)
	{
		try
		{
			if (ProviderInfo.ProviderApi.Length <= 0)
			{
				throw new Exception("Provider Api Not Found!");
			}
			if (ProviderInfo.ProviderApiToken.Length <= 0)
			{
				throw new Exception("Provider Api Token Not Found!");
			}
			if (ToNumbers.Count <= 0)
			{
				throw new Exception("No Number Found to Send Sms");
			}
			if (ProviderInfo.ProviderApiMasking.Length <= 0)
			{
				throw new Exception("Sms Mask Not Found!");
			}
			if (SmsMessage.Length <= 0)
			{
				throw new Exception("Sms Message Not Found!");
			}
			if (ToNumbers.Count > 50)
			{
				throw new Exception("You can only send SMS to 50 Numbers at once!");
			}
			string.Join(",", ToNumbers.ToArray());
			string apiToken = "7e18ee7c42458b82f5237543f627a03eb1e9636708";
			string apiSecret = "EccountBookCloudERP";
			string toNumber = "923175404113";
			string Masking = "SMS Alert";
			string MsgType = "unicode";
			string MessageText = "Aoa" + SmsMessage;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://lifetimesms.com/json");
			httpWebRequest.Accept = "application/json";
			httpWebRequest.ContentType = " application/x-www-form-urlencoded";
			httpWebRequest.Method = "POST";
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string parameters = "api_token=" + apiToken + "&api_secret=" + apiSecret + "&to=" + toNumber + "&from=" + Masking + "&type=" + MsgType + "&message=" + MessageText;
				streamWriter.Write(parameters);
				streamWriter.Flush();
				streamWriter.Close();
			}
			HttpWebResponse obj = (HttpWebResponse)httpWebRequest.GetResponse();
			string ApiResponse = "";
			using (StreamReader streamReader = new StreamReader(obj.GetResponseStream()))
			{
				ApiResponse = streamReader.ReadToEnd().ToString();
			}
			return ApiResponse;
		}
		catch (Exception ex)
		{
			throw new Exception("LifeTimeSmsApi : " + ex.Message);
		}
	}
}
