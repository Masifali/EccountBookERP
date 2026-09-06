using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Tax.Definitions;

namespace Architecture.DAL.Tax.Definitions;

public class TaxTypeAndBusinessTypeMapping
{
	public long SetData(Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping item in obj.TaxTypeAndBusinessTypeMappingList)
			{
				returnID = Conversion.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			using (SqlCommand sqlCmd = new SqlCommand())
			{
				sqlCmd.Connection = sqlcon;
				sqlCmd.Transaction = sqlTrn;
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "[dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping]";
				sqlCmd.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCmd.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public List<Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping> lst = new List<Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Tax.Definitions.TaxTypeAndBusinessTypeMapping>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
