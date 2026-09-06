using System;
using System.Data;
using System.Data.SqlServerCe;

namespace Architecture.PSDSMS;

public class SQLCompact40
{
	private static SQLCompact40 sQLCompact40;

	private string _dbCon = string.Empty;

	private SqlCeEngine en;

	private SqlCeConnection con;

	private SqlCeCommand cmd;

	public static SQLCompact40 Instance
	{
		get
		{
			if (sQLCompact40 == null)
			{
				sQLCompact40 = new SQLCompact40();
			}
			return sQLCompact40;
		}
	}

	private SQLCompact40()
	{
		_dbCon = "Datasource=\"data.sdf\"; Password=\"Admin123#\"";
		Init();
	}

	public void Init()
	{
		CreateDatabase();
		CreateTable();
	}

	private void CreateDatabase()
	{
		try
		{
			en = new SqlCeEngine(_dbCon);
			if (!en.Verify())
			{
				en.CreateDatabase();
			}
		}
		catch
		{
		}
	}

	private void CreateTable()
	{
		try
		{
			con = new SqlCeConnection(_dbCon);
			con.Close();
			if (con.State == ConnectionState.Closed)
			{
				con.Open();
			}
			cmd = new SqlCeCommand("SELECT COUNT(*) AS ID FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Messages'", con);
			object obj = cmd.ExecuteScalar();
			con.Close();
			if ((int)obj == 0)
			{
				if (con.State == ConnectionState.Closed)
				{
					con.Open();
				}
				cmd = new SqlCeCommand("CREATE TABLE Messages(\r\n                                         ID INT PRIMARY KEY IDENTITY(1,1),\r\n                                         MessageId INT DEFAULT((1)),\r\n                                         MessageLogDate NVARCHAR(500) DEFAULT(''),\r\n                                         MessageTExt NVARCHAR(3000) DEFAULT(''),\r\n                                         Mobile NVARCHAR(30) DEFAULT(''),\r\n                                         PassCode NVARCHAR(30) DEFAULT(''),\r\n                                         Status NVARCHAR(50) DEFAULT(''))", con);
				cmd.ExecuteNonQuery();
				con.Close();
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void AlterTable()
	{
		try
		{
			con = new SqlCeConnection(_dbCon);
			con.Close();
			if (con.State == ConnectionState.Closed)
			{
				con.Open();
			}
			cmd = new SqlCeCommand("ALTER TABLE USERS \r\n                                             ADD AccountId INT", con);
			cmd.ExecuteNonQuery();
			con.Close();
		}
		catch
		{
		}
		finally
		{
			con.Dispose();
			cmd.Dispose();
		}
	}

	public int ExecuteNonQuery(string strSQL)
	{
		con = new SqlCeConnection(_dbCon);
		con.Close();
		if (con.State == ConnectionState.Closed)
		{
			con.Open();
		}
		cmd = new SqlCeCommand();
		try
		{
			cmd.Connection = con;
			cmd.CommandText = strSQL;
			int result = cmd.ExecuteNonQuery();
			con.Close();
			return result;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			con.Dispose();
			cmd.Dispose();
		}
	}

	public int ExecuteScalar(string strSQL)
	{
		int Id = 0;
		con = new SqlCeConnection(_dbCon);
		con.Close();
		if (con.State == ConnectionState.Closed)
		{
			con.Open();
		}
		cmd = new SqlCeCommand();
		try
		{
			cmd.Connection = con;
			cmd.CommandText = strSQL;
			object ret = cmd.ExecuteScalar();
			Id = ((ret != null) ? ((int)ret) : 0);
			con.Close();
			return Id;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			con.Dispose();
			cmd.Dispose();
		}
	}

	public DataTable GetDataTable(string strSQL)
	{
		con = new SqlCeConnection(_dbCon);
		con.Close();
		if (con.State == ConnectionState.Closed)
		{
			con.Open();
		}
		cmd = new SqlCeCommand();
		SqlCeDataAdapter da = new SqlCeDataAdapter();
		DataTable dt = new DataTable();
		try
		{
			cmd.Connection = con;
			cmd.CommandText = strSQL;
			da.SelectCommand = cmd;
			da.Fill(dt);
			dt.AcceptChanges();
			con.Close();
			return dt;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			cmd.Dispose();
			da.Dispose();
			dt.Dispose();
		}
	}
}
