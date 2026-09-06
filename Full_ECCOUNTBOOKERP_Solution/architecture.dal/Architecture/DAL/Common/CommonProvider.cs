using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Architecture.Model.Common;

namespace Architecture.DAL.Common;

public static class CommonProvider
{
	public static DataTable GetDataTable(string procedureName, List<SqlParameter> p = null)
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = procedureName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item in p)
				{
					cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
				}
			}
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
			sqlDataAdapter.SelectCommand = cmd;
			sqlDataAdapter.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataSet GetDataSet(string procedureName, List<SqlParameter> p = null)
	{
		DataSet ds = new DataSet();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = procedureName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item in p)
				{
					cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
				}
			}
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
			sqlDataAdapter.SelectCommand = cmd;
			sqlDataAdapter.Fill(ds);
			return ds;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static bool ExecuteProcedure(string procedureName, List<SqlParameter> p = null)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = procedureName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item in p)
				{
					cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
				}
			}
			cmd.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return true;
	}

	public static string GetSchema(string tblName)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.CommandText = "usp_Sys_GetSchema";
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.Parameters.AddWithValue("@tblName", tblName);
			return sqlCommand.ExecuteScalar().ToString();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static string GetActivity(int AppActionId)
	{
		string ReturnActivity = string.Empty;
		try
		{
			if (Enum.TryParse<DBActionMode>(AppActionId.ToString(), out var getActivity))
			{
				ReturnActivity = getActivity.ToString();
			}
		}
		catch (Exception)
		{
			throw;
		}
		return ReturnActivity;
	}

	public static string GetActivity(int AppActionId, string Activity)
	{
		string ReturnActivity = Activity;
		try
		{
			if (Enum.TryParse<DBActionMode>(AppActionId.ToString(), out var getActivity))
			{
				return getActivity.ToString();
			}
			return Activity;
		}
		catch (Exception)
		{
			throw;
		}
	}

	internal static bool ColumnExists(IDataReader reader, string columnName)
	{
		for (int i = 0; i < reader.FieldCount; i++)
		{
			if (reader.GetName(i) == columnName)
			{
				return true;
			}
		}
		return false;
	}

	public static List<T> GetAll<T>(SqlConnection sqlcon, string Activity, List<ParameterList> p = null)
	{
		List<T> lst = new List<T>();
		string getItemName = string.Empty;
		Type typ = typeof(T);
		string SchemaName = GetSchema(typ.Name);
		Type getItemType = null;
		try
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = SchemaName + "usp_Get_" + typ.Name;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Activity", Activity);
			if (p != null)
			{
				foreach (ParameterList item2 in p)
				{
					cmd.Parameters.AddWithValue(item2.ParameterName, item2.ParameterValue);
				}
			}
			SqlDataReader dr = cmd.ExecuteReader();
			if (dr.HasRows)
			{
				while (dr.Read())
				{
					T obj = Activator.CreateInstance<T>();
					PropertyInfo[] array = (from x in typ.GetProperties()
						where !x.GetMethod.IsVirtual
						select x).ToArray();
					foreach (PropertyInfo item in array)
					{
						getItemName = item.Name;
						if (ColumnExists(dr, item.Name))
						{
							getItemType = dr[item.Name].GetType();
							Type type = dr[item.Name].GetType();
							string readerValue = string.Empty;
							if (type == typeof(byte[]))
							{
								byte[] readByte = dr[item.Name] as byte[];
								item.SetValue(obj, readByte, null);
							}
							else if (type == typeof(TimeSpan))
							{
								TimeSpan readerTimeSpan = TimeSpan.Parse(dr[item.Name].ToString());
								item.SetValue(obj, Convert.ChangeType(readerTimeSpan, type), null);
							}
							else if (dr[item.Name] != DBNull.Value)
							{
								readerValue = dr[item.Name].ToString();
								item.SetValue(obj, Convert.ChangeType(readerValue, type), null);
							}
						}
					}
					lst.Add(obj);
				}
			}
		}
		catch (Exception ex)
		{
			string msg = string.Empty;
			msg = "Error ";
			if (typ != null)
			{
				msg = msg + " In " + typ.Name;
			}
			if (getItemName != string.Empty)
			{
				msg = msg + " AND COLOUMN IS " + getItemName;
			}
			if (getItemType != null)
			{
				msg = msg + " Data type is " + getItemType.ToString();
			}
			msg = msg + " Exception is " + ex.Message;
			throw new Exception(msg);
		}
		return lst;
	}
}
