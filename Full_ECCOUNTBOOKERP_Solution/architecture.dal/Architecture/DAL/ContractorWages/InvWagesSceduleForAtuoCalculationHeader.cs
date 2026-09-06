using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ContractorWages;

namespace Architecture.DAL.ContractorWages;

public class InvWagesSceduleForAtuoCalculationHeader : DALBase
{
	public int SetData(Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (InvWagesSceduleForAtuoCalculationDetail item in obj.WagesScheduleAutoCalculationDateil)
			{
				item.InvWagesSceduleForAtuoCalculationHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_InvWagesSceduleForAtuoCalculationDetail_Insert"));
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

	public List<Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader> lst = new List<Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.WagesScheduleAutoCalculationDateil = GenericProvider.GetAllDetail<InvWagesSceduleForAtuoCalculationDetail>(sqlcon, "Sp_InvWagesSceduleForAtuoCalculation_GetAllMethod", lstPrm);
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
