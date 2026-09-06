using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ContractorWages;

namespace Architecture.DAL.ContractorWages;

public class WagesAccountAllocationToDocumentType : DALBase
{
	public int SetData(Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType item in obj.WagesAccountAllocationToDocumentTypesList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
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

	public List<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType> lst = new List<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int DeleteByID(Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType obj)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ContractorWages.WagesAccountAllocationToDocumentType item in obj.WagesAccountAllocationToDocumentTypesList)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_WagesAccountAllocationToDocumentType_GetAllMethod";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", item.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", item.CompanyId);
				sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
				sqlCommand.Parameters.AddWithValue("@WagesAccountId", item.WagesAccountId);
				sqlCommand.Parameters.AddWithValue("@EntryUserId", item.EntryUserId);
				sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
				sqlCommand.ExecuteNonQuery();
			}
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
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
