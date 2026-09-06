using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class GenerateExportInvoiceNos
{
	public int SetData(Architecture.Model.GenerateExportInvoiceNos obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "USP_ExportInvoiceNosValidateAndDelete";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
			sqlCommand.Parameters.AddWithValue("@MainId", obj.MainId);
			sqlCommand.ExecuteNonQuery();
			foreach (Architecture.Model.GenerateExportInvoiceNos item in obj.GenerateExportInvoiceNosList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			obj.Id = Convert.ToInt32(returnID);
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.GenerateExportInvoiceNos> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.GenerateExportInvoiceNos> lst = new List<Architecture.Model.GenerateExportInvoiceNos>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.GenerateExportInvoiceNos>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
