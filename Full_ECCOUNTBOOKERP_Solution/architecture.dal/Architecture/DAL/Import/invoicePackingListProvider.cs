using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Common;
using Architecture.Model.Import;

namespace Architecture.DAL.Import;

public class invoicePackingListProvider
{
	public static List<invoicePackingList> GetAll(string Activity, List<ParameterList> p = null)
	{
		List<invoicePackingList> lst = new List<invoicePackingList>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAll<invoicePackingList>(sqlcon, Activity, p);
		}
		catch
		{
			throw;
		}
	}

	public static object Set(invoicePackingList obj, string Activity, ParameterObject p = null)
	{
		object returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (invoicePackingList item in obj.invoicePackingListDetail)
			{
				item.invoiceMasterId = obj.invoiceMasterId;
				string ReturnActivity = CommonProvider.GetActivity(item.appActionId, Activity);
				returnID = GenericProvider.Set(sqlTrn, ReturnActivity, item);
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public invoicePackingList Get(string Activity, List<ParameterList> p = null)
	{
		invoicePackingList lst = new invoicePackingList();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<invoicePackingList>(sqlcon, Activity, p);
		}
		catch
		{
			throw;
		}
	}

	public static void SetAll(List<invoicePackingList> lst, ParameterObject p = null)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (invoicePackingList item in lst)
			{
				string ReturnActivity = CommonProvider.GetActivity(item.appActionId);
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

	public void SetXML(object Id, string objXML)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			GenericProvider.SetXML<invoiceMaster>(sqlTrn, Id, objXML);
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
			GenericProvider.SetJSON<invoiceMaster>(sqlTrn, Id, objJSON);
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
