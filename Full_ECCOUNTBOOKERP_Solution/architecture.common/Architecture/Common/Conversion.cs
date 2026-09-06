using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace Architecture.Common;

public class Conversion
{
	public static bool ToBool(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToBoolean(value);
			}
			return false;
		}
		catch (Exception)
		{
			try
			{
				if (value != null && value != DBNull.Value && value != string.Empty)
				{
					return Convert.ToBoolean(Convert.ToInt32(value));
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	public static Guid ToGuid(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Guid.Parse(Convert.ToString(value));
			}
			return Guid.Empty;
		}
		catch (Exception)
		{
			return Guid.Empty;
		}
	}

	public static string ToString(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToString(value);
			}
			return string.Empty;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public static TimeSpan ToTimeSpan(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return (TimeSpan)value;
			}
			return new TimeSpan(0, 0, 0);
		}
		catch (Exception)
		{
			return new TimeSpan(0, 0, 0);
		}
	}

	public static int ToInt(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToInt32(value);
			}
			return 0;
		}
		catch (Exception)
		{
			return 0;
		}
	}

	public static short ToShort(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToInt16(value);
			}
			return 0;
		}
		catch (Exception)
		{
			return 0;
		}
	}

	public static long ToInt64(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToInt64(value);
			}
			return 0L;
		}
		catch (Exception)
		{
			return 0L;
		}
	}

	public static decimal ToDecimal(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToDecimal(value);
			}
			return 0m;
		}
		catch (Exception)
		{
			return 0m;
		}
	}

	public static double ToDouble(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				double result = Convert.ToDouble(value);
				if (double.IsInfinity(result))
				{
					return 0.0;
				}
				return result;
			}
			return 0.0;
		}
		catch (Exception)
		{
			return 0.0;
		}
	}

	public static float ToSingle(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToSingle(value);
			}
			return 0f;
		}
		catch (Exception)
		{
			return 0f;
		}
	}

	public static DateTime ToDateTime(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return Convert.ToDateTime(value);
			}
			return new DateTime(1900, 1, 1);
		}
		catch (Exception)
		{
			return new DateTime(1900, 1, 1);
		}
	}

	public static string ToHash(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return value.ToString();
			}
			return string.Empty;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public static byte[] ToByteArray(object value)
	{
		try
		{
			if (value != null && value != DBNull.Value && value != string.Empty)
			{
				return (byte[])value;
			}
			return null;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static string OppositeDateTimeConvert(string yyyymmdd)
	{
		try
		{
			if (yyyymmdd.Length >= 14)
			{
				yyyymmdd = yyyymmdd.Substring(4, 2) + "/" + yyyymmdd.Substring(6, 2) + "/" + yyyymmdd.Substring(0, 4) + " " + yyyymmdd.Substring(8, 2) + ":" + yyyymmdd.Substring(10, 2) + ":" + yyyymmdd.Substring(12, 2);
			}
			else if (yyyymmdd.Length >= 12)
			{
				yyyymmdd = yyyymmdd.Substring(4, 2) + "/" + yyyymmdd.Substring(6, 2) + "/" + yyyymmdd.Substring(0, 4) + " " + yyyymmdd.Substring(8, 2) + ":" + yyyymmdd.Substring(10, 2);
			}
			else if (yyyymmdd.Length == 8)
			{
				yyyymmdd = yyyymmdd.Substring(4, 2) + "/" + yyyymmdd.Substring(6, 2) + "/" + yyyymmdd.Substring(0, 4);
			}
		}
		catch (Exception)
		{
			return "";
		}
		return yyyymmdd;
	}

	public static bool CheckDateTimeNull(DateTime date)
	{
		string[] array = ToString(date).Split(' ');
		if (array[0] == "1/1/0001" || ((array[0] == "1/1/1900") | (array[0] == "0001-01-01")) || array[0] == "01-Jan-1900" || array[0] == "01/01/1900" || array[0] == "01/01/0001" || array[0] == "01/01/1" || array[0] == "01-Jan-00" || array[0] == "01-Jan-01" || array[0] == "01-Jan-0001" || array[0] == "00/01/01" || array[0] == "01/01/01")
		{
			return true;
		}
		return false;
	}

	public static List<T> ConvertDataTableToList<T>(DataTable table)
	{
		List<T> NewList = new List<T>();
		foreach (DataRow row in table.Rows)
		{
			T RowObject = GetRowObjectOfType_T<T>(row);
			NewList.Add(RowObject);
		}
		return NewList;
	}

	public static DataTable ConvertListToDataTable<T>(List<T> list)
	{
		DataTable table = new DataTable();
		if (list.Count > 0)
		{
			PropertyInfo[] properties = typeof(T).GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo prop2 in array)
			{
				table.Columns.Add(prop2.Name, Nullable.GetUnderlyingType(prop2.PropertyType) ?? prop2.PropertyType);
			}
			foreach (T item in list)
			{
				DataRow row = table.NewRow();
				array = properties;
				foreach (PropertyInfo prop in array)
				{
					row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
				}
				table.Rows.Add(row);
			}
		}
		return table;
	}

	private static T GetRowObjectOfType_T<T>(DataRow Row)
	{
		T Object = Activator.CreateInstance<T>();
		string getItemName = string.Empty;
		Type ReflectionOfOriginalType_T = typeof(T);
		Type getItemType = null;
		try
		{
			foreach (DataColumn column in Row.Table.Columns)
			{
				PropertyInfo[] properties = ReflectionOfOriginalType_T.GetProperties();
				foreach (PropertyInfo property in properties)
				{
					getItemName = property.Name;
					property.GetType();
					getItemType = column.GetType();
					Row[column.ColumnName].GetType();
					if (property.Name == column.ColumnName && !DBNull.Value.Equals(Row[column.ColumnName]))
					{
						property.SetValue(Object, Row[column.ColumnName], null);
					}
				}
			}
			return Object;
		}
		catch (Exception ex)
		{
			string msg = string.Empty;
			msg = "";
			if (ReflectionOfOriginalType_T != null)
			{
				msg = msg + "In Model: " + ReflectionOfOriginalType_T.Name;
			}
			if (getItemName != string.Empty)
			{
				msg = msg + "In Column " + getItemName;
			}
			if (getItemType != null)
			{
				msg = msg + "of Data Type " + getItemType.ToString();
			}
			msg = msg + "And Exception is " + ex.Message;
			throw new Exception(msg);
		}
	}

	public static string ConvertListToXmlSerializer<T>(List<T> lst)
	{
		string serializedXml = string.Empty;
		string DeafultOmitDeclaration = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
		string objectName = typeof(T).Name;
		using (StringWriter writer = new StringWriter())
		{
			XmlSerializer serializer = new XmlSerializer(typeof(SerializationSurrogate));
			foreach (T item in lst)
			{
				SerializationSurrogate surrogate = new SerializationSurrogate(item);
				serializer.Serialize(writer, surrogate);
			}
			serializedXml = writer.ToString();
			if (!string.IsNullOrEmpty(serializedXml))
			{
				serializedXml = serializedXml.Replace(DeafultOmitDeclaration, "");
				serializedXml = serializedXml.Replace("SerializationSurrogate", objectName);
				serializedXml = "<ArrayOf" + objectName + ">" + serializedXml + "\n</ArrayOf" + objectName + ">";
			}
		}
		return serializedXml;
	}
}
