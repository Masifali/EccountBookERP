using System;
using System.Configuration;
using System.Linq;
using Architecture.Model;

namespace Architecture.Common;

public class Services
{
	public static string GetConnectionString()
	{
		try
		{
			string ConnString = "";
			string decryptedBase = Encryption.DecryptString(ConfigurationManager.ConnectionStrings["TPSSDb"].ConnectionString);
			string serverIP = ConfigurationManager.AppSettings["DbServerIP"];
			if (!ConfigurationManager.AppSettings.AllKeys.Contains("DbServerIP") || string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DbServerIP"]))
			{
				return decryptedBase;
			}
			return "Data Source=" + serverIP + ";" + decryptedBase;
		}
		catch (Exception Ex)
		{
			throw new Exception("Error on Decrypting Connection String: " + Ex.Message);
		}
	}

	public static string GetTaxServerConnectionString()
	{
		try
		{
			string ConnString = "";
			string decryptedBase = Encryption.DecryptString(ConfigurationManager.ConnectionStrings["TPSSDb_Server"].ConnectionString);
			string serverIP = ConfigurationManager.AppSettings["DbServerIP"];
			if (!ConfigurationManager.AppSettings.AllKeys.Contains("DbServerIP") || string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DbServerIP"]))
			{
				return decryptedBase;
			}
			return "Data Source=" + serverIP + ";" + decryptedBase;
		}
		catch (Exception Ex)
		{
			throw new Exception("Error on Decrypting Connection String: " + Ex.Message);
		}
	}
}
