using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Architecture.PSDSMS.DataLayer;

public class SQLHelper
{
	private string ConStr = string.Empty;

	private static SQLHelper helper;

	public static SQLHelper Instance
	{
		get
		{
			if (helper == null)
			{
				helper = new SQLHelper();
			}
			return helper;
		}
	}

	private SQLHelper()
	{
		ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
	}

	public int ExecuteNonQuery(string CommandText)
	{
		SqlConnection con = new SqlConnection(ConStr);
		if (con.State == ConnectionState.Closed)
		{
			con.Open();
		}
		SqlCommand cmd = new SqlCommand();
		try
		{
			cmd.Connection = con;
			cmd.CommandText = CommandText;
			cmd.CommandType = CommandType.Text;
			return cmd.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			con.Close();
			cmd.Dispose();
		}
	}

	public DataTable GetDataTable(string CommandText)
	{
		SqlConnection con = new SqlConnection(ConStr);
		if (con.State == ConnectionState.Closed)
		{
			con.Open();
		}
		SqlCommand cmd = new SqlCommand();
		DataTable dt = new DataTable();
		SqlDataAdapter da = new SqlDataAdapter();
		try
		{
			cmd.Connection = con;
			cmd.CommandText = CommandText;
			cmd.CommandType = CommandType.Text;
			da.SelectCommand = cmd;
			da.Fill(dt);
			dt.AcceptChanges();
			return dt;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			con.Close();
			cmd.Dispose();
			da.Dispose();
			dt.Dispose();
		}
	}
}
