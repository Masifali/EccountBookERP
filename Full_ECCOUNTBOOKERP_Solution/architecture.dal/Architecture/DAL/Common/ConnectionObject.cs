using System.Configuration;
using System.Data.SqlClient;
using Architecture.Common;

namespace Architecture.DAL.Common;

public static class ConnectionObject
{
	public static bool ReadyToConnect;

	public static string sqlConnectionstring;

	public static string ServerName;

	public static string DataBaseName;

	public static string DataBaseUser;

	public static string DataBasePassword;

	public static string IntegratedSecurity;

	public static string sqlConnectionstringForServer;

	static ConnectionObject()
	{
		if (clsGlobalVariables.ServiceConnectionString != string.Empty && clsGlobalVariables.ServiceConnectionString != null)
		{
			sqlConnectionstring = clsGlobalVariables.ServiceConnectionString;
			return;
		}
		sqlConnectionstring = Services.GetConnectionString();
		if (ConfigurationManager.ConnectionStrings["TPSSDb_Server"] != null)
		{
			sqlConnectionstringForServer = Services.GetTaxServerConnectionString();
		}
	}

	public static SqlConnection SQL()
	{
		return new SqlConnection(sqlConnectionstring);
	}

	public static SqlConnection SQLForServer()
	{
		return new SqlConnection(sqlConnectionstringForServer);
	}
}
