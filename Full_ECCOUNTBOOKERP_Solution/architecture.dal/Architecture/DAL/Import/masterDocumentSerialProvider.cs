using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Common;
using Architecture.Model.Import;

namespace Architecture.DAL.Import;

public class masterDocumentSerialProvider
{
	public static List<masterDocumentSerial> GetAll(string Activity, List<ParameterList> p = null)
	{
		List<masterDocumentSerial> lst = new List<masterDocumentSerial>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAll<masterDocumentSerial>(sqlcon, Activity, p);
		}
		catch
		{
			throw;
		}
	}

	public static object Set(masterDocumentSerial obj, string Activity, ParameterObject p = null)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			object returnID = GenericProvider.Set(sqlTrn, Activity, obj);
			obj.masterDocumentSerialId = Convert.ToInt32(returnID);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static void SetAll(List<masterDocumentSerial> lst, ParameterObject p = null)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (masterDocumentSerial item in lst)
			{
				string ReturnActivity = CommonProvider.GetActivity(item.AppActionId);
				GenericProvider.Set(sqlTrn, ReturnActivity, item);
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public masterDocumentSerial Get(string Activity, List<ParameterList> p = null)
	{
		masterDocumentSerial lst = new masterDocumentSerial();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<masterDocumentSerial>(sqlcon, Activity, p);
		}
		catch
		{
			throw;
		}
	}

	public void SetXML(object Id, string objXML)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			GenericProvider.SetXML<masterDocumentSerial>(sqlTrn, Id, objXML);
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public void SetJSON(object Id, string objJSON)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			GenericProvider.SetJSON<masterDocumentSerial>(sqlTrn, Id, objJSON);
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
