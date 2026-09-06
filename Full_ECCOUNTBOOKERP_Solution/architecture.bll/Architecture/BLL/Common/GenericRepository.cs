using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Architecture.BLL.Interface;
using Architecture.DAL.Common;
using Architecture.Model.Common;

namespace Architecture.BLL.Common;

public class GenericRepository<T> : BaseRepository, IBaseRepository<T>
{
	private Type typ = typeof(T);

	private string PrimaryKeyName = string.Empty;

	public GenericRepository()
	{
		PrimaryKeyName = typ.Name + "Id";
	}

	private void FillEntity(T obj, ActionMode mod)
	{
		try
		{
			string getItemName = string.Empty;
			Type getItemType = null;
			PropertyInfo[] array = (from x in typ.GetProperties()
				where !x.GetMethod.IsVirtual
				select x).ToArray();
			foreach (PropertyInfo item in array)
			{
				getItemName = item.Name;
				getItemType = item.PropertyType;
				switch (getItemName)
				{
				case "LastModifiedOn":
				case "CreatedOn":
					item.SetValue(obj, Convert.ChangeType(base.Createdon, getItemType), null);
					break;
				case "AppUserLogId":
					item.SetValue(obj, base.UserLogId, null);
					break;
				case "AppActionId":
					switch (mod)
					{
					case ActionMode.NEW:
						item.SetValue(obj, 1, null);
						break;
					case ActionMode.EDIT:
						item.SetValue(obj, 2, null);
						break;
					case ActionMode.DELETE:
						item.SetValue(obj, 3, null);
						break;
					}
					break;
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public object Delete(T obj)
	{
		try
		{
			FillEntity(obj, ActionMode.DELETE);
			return GenericProvider.SetData(obj, "Delete");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void DeleteAll(List<T> obj)
	{
	}

	public object Edit(T obj)
	{
		try
		{
			FillEntity(obj, ActionMode.EDIT);
			return GenericProvider.SetData(obj, "Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public object New(T obj)
	{
		try
		{
			FillEntity(obj, ActionMode.NEW);
			return GenericProvider.SetData(obj, "Insert");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public object Save(T obj)
	{
		try
		{
			object getPrimaryKeyValue = null;
			PropertyInfo getPrimaryKey = typ.GetProperty(PrimaryKeyName);
			if (getPrimaryKey != null)
			{
				getPrimaryKeyValue = getPrimaryKey.GetValue(obj);
			}
			if (Convert.ToUInt64(getPrimaryKeyValue) == 0)
			{
				return New(obj);
			}
			return Edit(obj);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void SaveAll(List<T> lst)
	{
	}

	public T GetById(long AppRoleID)
	{
		T obj = Activator.CreateInstance<T>();
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@" + PrimaryKeyName,
				Value = AppRoleID
			});
			return GenericProvider.GetData<T>("ID", p)[0];
		}
		catch (Exception)
		{
			throw;
		}
	}

	public object GetCode()
	{
		try
		{
			return GenericProvider.ExecuteProcedure<T>("GET CODE");
		}
		catch (Exception)
		{
			throw;
		}
	}

	public List<T> GetAll()
	{
		List<T> lst = new List<T>();
		try
		{
			return GenericProvider.GetData<T>("All");
		}
		catch (Exception)
		{
			throw;
		}
	}

	public List<T> GetBySearch(List<SqlParameter> p)
	{
		List<T> lst = new List<T>();
		try
		{
			return GenericProvider.GetData<T>("SEARCH", p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public DataTable GetDataTableForSearch(List<SqlParameter> p)
	{
		DataTable dt = null;
		try
		{
			return GenericProvider.GetDataTable<T>("SEARCH", p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public DataTable GetDataTableForList()
	{
		DataTable dt = null;
		try
		{
			return GenericProvider.GetDataTable<T>("LIST");
		}
		catch (Exception)
		{
			throw;
		}
	}

	public object GetCodeByID(string tblName, long ID)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@" + tblName + "Id",
				Value = ID
			});
			return GenericProvider.ExecuteProcedure<T>("GET CODE", p);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
