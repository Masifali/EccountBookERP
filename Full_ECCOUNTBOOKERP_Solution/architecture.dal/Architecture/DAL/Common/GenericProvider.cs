using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Architecture.Model.Common;

namespace Architecture.DAL.Common;

public static class GenericProvider
{
	public static object ExecuteProcedure<T>(string Activity, List<SqlParameter> p = null)
	{
		try
		{
			Type typ = typeof(T);
			string SchemaName = CommonProvider.GetSchema(typ.Name);
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = SchemaName + "usp_Get_" + typ.Name;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Activity", Activity);
			if (p != null)
			{
				foreach (SqlParameter item in p)
				{
					cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
				}
			}
			return cmd.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<T> GetData<T>(string Activity, List<SqlParameter> p = null)
	{
		List<T> lst = new List<T>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return Get<T>(sqlcon, Activity, p);
		}
		catch
		{
			throw;
		}
	}

	public static object SetData<T>(T obj, string Activity)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			object returnID = Set(sqlTrn, Activity, obj);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static object SetDataNew<T>(T obj, string Activity, string ProcName = "")
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			object returnID = SetNew(sqlTrn, Activity, obj, ProcName);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static object SetData<T>(T obj, string ProcName, string Activity)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			object returnID = Set(sqlTrn, Activity, obj);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable GetDataTableProc<T>(string ProcName, List<SqlParameter> p = null)
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = ProcName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item in p)
				{
					cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
				}
			}
			SqlDataAdapter obj = new SqlDataAdapter
			{
				SelectCommand = cmd
			};
			cmd.CommandTimeout = 220;
			obj.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable GetDataTableProcSetTime<T>(string ProcName, List<SqlParameter> p = null)
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = ProcName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item in p)
				{
					cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
				}
			}
			SqlDataAdapter obj = new SqlDataAdapter
			{
				SelectCommand = cmd
			};
			cmd.CommandTimeout = 220;
			obj.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataSet GetDataSetProc<T>(string ProcName, List<SqlParameter> p = null)
	{
		DataSet ds = new DataSet();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = ProcName;
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

	public static DataTable GetDataTable<T>(string Activity, List<SqlParameter> p = null)
	{
		DataTable dt = new DataTable();
		Type typ = typeof(T);
		string SchemaName = CommonProvider.GetSchema(typ.Name);
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = SchemaName + "usp_Get_" + typ.Name;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Activity", Activity);
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

	public static object SetNew<T>(SqlTransaction sqlTrn, string Activity, T obj, string ProcName = "")
	{
		try
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlTrn.Connection;
			cmd.Transaction = sqlTrn;
			Type typ = typeof(T);
			if (ProcName != string.Empty)
			{
				string SchemaName = CommonProvider.GetSchema(typ.Name);
				if (ProcName == "Insert" || ProcName == "Update")
				{
					cmd.CommandText = SchemaName + "usp_" + typ.Name + "_" + ProcName;
				}
				else
				{
					cmd.CommandText = ProcName;
				}
			}
			else
			{
				string SchemaName2 = CommonProvider.GetSchema(typ.Name);
				cmd.CommandText = SchemaName2 + "usp_" + typ.Name + "_Insert";
			}
			cmd.CommandType = CommandType.StoredProcedure;
			PropertyInfo[] array = typ.GetProperties().ToArray();
			foreach (PropertyInfo item in array)
			{
				if (!item.GetMethod.IsVirtual)
				{
					_ = item.PropertyType;
					object getValue = item.GetValue(obj);
					cmd.Parameters.AddWithValue("@" + item.Name, getValue);
				}
			}
			if (Activity != null && Activity != string.Empty)
			{
				cmd.Parameters.AddWithValue("@Activity", Activity);
			}
			return cmd.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static object SetProc<T>(SqlTransaction sqlTrn, T obj, string ProcName, string Activity = "")
	{
		try
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlTrn.Connection;
			cmd.Transaction = sqlTrn;
			Type typ = typeof(T);
			cmd.CommandText = ProcName;
			cmd.CommandType = CommandType.StoredProcedure;
			PropertyInfo[] array = typ.GetProperties().ToArray();
			foreach (PropertyInfo item in array)
			{
				if (!item.GetMethod.IsVirtual)
				{
					_ = item.PropertyType;
					object getValue = item.GetValue(obj);
					cmd.Parameters.AddWithValue("@" + item.Name, getValue);
				}
			}
			if (Activity != null && Activity != string.Empty)
			{
				cmd.Parameters.AddWithValue("@Activity", Activity);
			}
			return cmd.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<T> Get<T>(SqlConnection sqlcon, string ProcName, List<SqlParameter> p = null)
	{
		List<T> lst = new List<T>();
		string getItemName = string.Empty;
		Type typ = typeof(T);
		Type getItemType = null;
		try
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = ProcName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item2 in p)
				{
					cmd.Parameters.AddWithValue(item2.ParameterName, item2.Value);
				}
			}
			SqlDataReader dr = cmd.ExecuteReader();
			if (dr.HasRows)
			{
				while (dr.Read())
				{
					T obj = Activator.CreateInstance<T>();
					PropertyInfo[] array = typ.GetProperties().ToArray();
					foreach (PropertyInfo item in array)
					{
						getItemName = item.Name;
						getItemType = dr[item.Name].GetType();
						if (!ColumnExists(dr, item.Name))
						{
							continue;
						}
						Type type = dr[item.Name].GetType();
						string readerValue = string.Empty;
						if (type == typeof(byte[]))
						{
							byte[] readByte = dr[item.Name] as byte[];
							item.SetValue(obj, readByte, null);
						}
						else if (dr[item.Name] != DBNull.Value)
						{
							readerValue = dr[item.Name].ToString();
							if (!string.IsNullOrEmpty(readerValue))
							{
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

	public static List<T> GetAllDetail<T>(SqlConnection sqlcon, string ProcName, List<SqlParameter> p = null)
	{
		List<T> lst = new List<T>();
		string getItemName = string.Empty;
		Type typ = typeof(T);
		Type getItemType = null;
		try
		{
			using SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = ProcName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item2 in p)
				{
					cmd.Parameters.AddWithValue(item2.ParameterName, item2.Value);
				}
			}
			using SqlDataReader dr = cmd.ExecuteReader();
			if (dr.HasRows)
			{
				while (dr.Read())
				{
					T obj = Activator.CreateInstance<T>();
					PropertyInfo[] array = typ.GetProperties().ToArray();
					foreach (PropertyInfo item in array)
					{
						getItemName = item.Name;
						getItemType = dr[item.Name].GetType();
						if (!ColumnExists(dr, item.Name))
						{
							continue;
						}
						Type type = dr[item.Name].GetType();
						string readerValue = string.Empty;
						if (type == typeof(byte[]))
						{
							byte[] readByte = dr[item.Name] as byte[];
							item.SetValue(obj, readByte, null);
						}
						else if (dr[item.Name] != DBNull.Value)
						{
							readerValue = dr[item.Name].ToString();
							if (!string.IsNullOrEmpty(readerValue))
							{
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

	public static List<T> GetProc<T>(SqlConnection sqlcon, string ProcName, List<SqlParameter> p = null)
	{
		List<T> lst = new List<T>();
		string getItemName = string.Empty;
		Type typ = typeof(T);
		Type getItemType = null;
		try
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = ProcName;
			cmd.CommandType = CommandType.StoredProcedure;
			if (p != null)
			{
				foreach (SqlParameter item2 in p)
				{
					cmd.Parameters.AddWithValue(item2.ParameterName, item2.Value);
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
						getItemType = dr[item.Name].GetType();
						if (!ColumnExists(dr, item.Name))
						{
							continue;
						}
						Type type = dr[item.Name].GetType();
						string readerValue = string.Empty;
						if (type == typeof(byte[]))
						{
							byte[] readByte = dr[item.Name] as byte[];
							item.SetValue(obj, readByte, null);
						}
						else if (dr[item.Name] != DBNull.Value)
						{
							readerValue = dr[item.Name].ToString();
							if (!string.IsNullOrEmpty(readerValue))
							{
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

	public static object Set<T>(SqlTransaction sqlTrn, string Activity, T obj, string ProcName = "")
	{
		try
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlTrn.Connection;
			cmd.Transaction = sqlTrn;
			Type typ = typeof(T);
			if (ProcName != string.Empty)
			{
				cmd.CommandText = ProcName;
			}
			else
			{
				string SchemaName = CommonProvider.GetSchema(typ.Name);
				cmd.CommandText = SchemaName + "usp_Set_" + typ.Name;
			}
			cmd.CommandType = CommandType.StoredProcedure;
			PropertyInfo[] array = typ.GetProperties().ToArray();
			foreach (PropertyInfo item in array)
			{
				if (!item.GetMethod.IsVirtual)
				{
					_ = item.PropertyType;
					object getValue = item.GetValue(obj);
					cmd.Parameters.AddWithValue("@" + item.Name, getValue);
				}
			}
			cmd.Parameters.AddWithValue("@Activity", Activity);
			return cmd.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static T Get<T>(SqlConnection sqlcon, string Activity, List<ParameterList> p = null)
	{
		object getdata = null;
		try
		{
			List<T> lst = GetAll<T>(sqlcon, Activity, p);
			getdata = ((lst == null || lst.Count <= 0) ? null : ((object)lst[0]));
		}
		catch
		{
			throw;
		}
		return (T)getdata;
	}

	public static List<T> GetAll<T>(SqlConnection sqlcon, string Activity, List<ParameterList> p = null)
	{
		List<T> lst = new List<T>();
		string getItemName = string.Empty;
		Type typ = typeof(T);
		string SchemaName = CommonProvider.GetSchema(typ.Name);
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
					PropertyInfo[] array = typ.GetProperties().ToArray();
					foreach (PropertyInfo item in array)
					{
						getItemName = item.Name;
						getItemType = dr[item.Name].GetType();
						if (!ColumnExists(dr, item.Name))
						{
							continue;
						}
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
							if (!string.IsNullOrEmpty(readerValue))
							{
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

	public static object SetXML<T>(SqlTransaction sqlTrn, object Id, string objXML)
	{
		try
		{
			SqlCommand obj = new SqlCommand
			{
				Connection = sqlTrn.Connection,
				Transaction = sqlTrn
			};
			Type typ = typeof(T);
			string SchemaName = CommonProvider.GetSchema(typ.Name);
			obj.CommandText = SchemaName + "usp_Set_XML_" + typ.Name;
			obj.CommandType = CommandType.StoredProcedure;
			typ.GetProperties().ToArray();
			obj.Parameters.AddWithValue("@Id", Id);
			obj.Parameters.AddWithValue("@" + typ.Name + "XML", objXML);
			return obj.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static object SetJSON<T>(SqlTransaction sqlTrn, object Id, string objJSON)
	{
		try
		{
			SqlCommand obj = new SqlCommand
			{
				Connection = sqlTrn.Connection,
				Transaction = sqlTrn
			};
			Type typ = typeof(T);
			string SchemaName = CommonProvider.GetSchema(typ.Name);
			obj.CommandText = SchemaName + "usp_Set_JSON_" + typ.Name;
			obj.CommandType = CommandType.StoredProcedure;
			typ.GetProperties().ToArray();
			obj.Parameters.AddWithValue("@Id", Id);
			obj.Parameters.AddWithValue("@" + typ.Name + "JSON", objJSON);
			return obj.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static object Validate<T>(SqlTransaction sqlTrn, string Activity, T obj)
	{
		try
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlTrn.Connection;
			cmd.Transaction = sqlTrn;
			Type typ = typeof(T);
			string SchemaName = CommonProvider.GetSchema(typ.Name);
			cmd.CommandText = SchemaName + "usp_Validate_" + typ.Name;
			cmd.CommandType = CommandType.StoredProcedure;
			PropertyInfo[] array = typ.GetProperties().ToArray();
			foreach (PropertyInfo item in array)
			{
				if (!item.GetMethod.IsVirtual)
				{
					_ = item.PropertyType;
					object getValue = item.GetValue(obj);
					cmd.Parameters.AddWithValue("@" + item.Name, getValue);
				}
			}
			cmd.Parameters.AddWithValue("@Activity", Activity);
			return cmd.ExecuteScalar();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAllDataTableCore<T>(string Activity, List<SqlParameter> p = null)
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			Type typ = typeof(T);
			string SchemaName = CommonProvider.GetSchema(typ.Name);
			_ = string.Empty;
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = SchemaName + "usp_Get_" + typ.Name;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Activity", Activity);
			if (p != null)
			{
				foreach (SqlParameter item in p)
				{
					cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
				}
			}
			SqlDataReader dr = cmd.ExecuteReader();
			if (dr.HasRows)
			{
				dt.Load(dr);
			}
			dr.Close();
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception("Error in GetAll method. Exception: " + ex.Message);
		}
	}

	public static DataTable GetDataTableProcForServer<T>(string ProcName, List<SqlParameter> p = null)
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQLForServer();
			sqlcon.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = sqlcon;
			cmd.CommandText = ProcName;
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

	public static void ExecuteStoredProcedure(SqlConnection connection, SqlTransaction transaction, string procedureName, Dictionary<string, object> parameters)
	{
		using SqlCommand command = new SqlCommand(procedureName, connection, transaction);
		command.CommandType = CommandType.StoredProcedure;
		command.CommandTimeout = 180;
		foreach (KeyValuePair<string, object> param in parameters)
		{
			command.Parameters.AddWithValue(param.Key, param.Value);
		}
		command.ExecuteNonQuery();
	}
}
