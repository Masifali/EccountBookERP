using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.MultiLanguages;

namespace Architecture.DAL.MultiLanguages;

public class MultiLingo
{
	public int SetDataForContractorWages(InvConractorWagesAccountsMultilingo obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (InvConractorWagesAccountsMultilingo item in obj.InvConractorWagesAccountsMultilingoslist)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public int SetDataForItemDefine(ItemMultiLingo obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (ItemMultiLingo item in obj.ItemMultiLingoList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public int SetDataForItemCategory(ItemCategoryMultilingo obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (ItemCategoryMultilingo item in obj.ItemCategoryMultilingoList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public int SetDataForItemType(ItemTypeMultiLingo obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (ItemTypeMultiLingo item in obj.ItemTypeMultiLingoList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public int SetDataForWarehouseDefine(WarehouseMultilingo obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (WarehouseMultilingo item in obj.WarehouseMultilingoList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public int SetDataForSupplierCustomerDefine(SupplierCustomerMultilingo obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (SupplierCustomerMultilingo item in obj.SupplierCustomerMultilingoList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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
}
