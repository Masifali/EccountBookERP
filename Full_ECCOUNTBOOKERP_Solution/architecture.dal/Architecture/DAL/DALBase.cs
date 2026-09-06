using System.Data.Common;
using Architecture.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Architecture.DAL;

public class DALBase
{
	public SqlDatabase sqlDataBase;

	private DbConnection conn;

	public DALBase()
	{
		if (clsGlobalVariables.ServiceConnectionString != string.Empty && clsGlobalVariables.ServiceConnectionString != null)
		{
			sqlDataBase = new SqlDatabase(clsGlobalVariables.ServiceConnectionString);
		}
		else
		{
			sqlDataBase = new SqlDatabase(Services.GetConnectionString());
		}
	}

	public DbTransaction getTransaction()
	{
		conn = sqlDataBase.CreateConnection();
		conn.Open();
		return conn.BeginTransaction();
	}

	public void CloseConnection()
	{
		if (conn != null && conn.State != 0)
		{
			conn.Close();
		}
	}

	public DALBase(SqlDatabase _sqlDataBase)
	{
		sqlDataBase = _sqlDataBase;
	}
}
