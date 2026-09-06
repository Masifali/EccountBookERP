using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Import;

namespace Architecture.DAL.Import;

public class ImProformaInvoice : DALBase
{
	public int SetDate(Architecture.Model.Import.ImProformaInvoice obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (ImProformaInvoiceDetail item in obj.ProformaInvoiceDetail)
			{
				item.ImProformaInvoiceHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ImProformaInvoiceDetail_Insert"));
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

	public List<Architecture.Model.Import.ImProformaInvoice> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.ImProformaInvoice> lst = new List<Architecture.Model.Import.ImProformaInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Import.ImProformaInvoice>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Import.ImProformaInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByIdProformaInvoiceHeaderId"));
					item.ProformaInvoiceDetail = GenericProvider.Get<ImProformaInvoiceDetail>(sqlcon, "Sp_ImProformaInvoice_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
