using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class ProductionOutputAllocationWithExportInvoice : DALBase
{
	public int SetData(Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice item in obj.ProductionOutputAllocationWithExportInvoicesList)
			{
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
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

	public List<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice> lst = new List<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
