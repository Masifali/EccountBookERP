using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Lab;

namespace Architecture.DAL.Lab;

public class LabAnalysisScheduleItemAndPartyWise
{
	public int SetData(Architecture.Model.Lab.LabAnalysisScheduleItemAndPartyWise obj)
	{
		int returnID = 0;
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (Architecture.Model.Lab.LabAnalysisScheduleItemAndPartyWise item in obj.LabAnalysisScheduleItemAndPartyWiseList)
				{
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "[dbo].[USP_LabAnalysisScheduleItemAndPartyWise_InsertAndUpdate]"));
					item.LabAnalysisScheduleItemAndPartyWiseId = ((returnID > 0) ? returnID : item.LabAnalysisScheduleItemAndPartyWiseId);
				}
				sqlTrn.Commit();
			}
			catch
			{
				sqlTrn.Rollback();
				throw;
			}
		}
		return obj.LabAnalysisScheduleItemAndPartyWiseId;
	}

	public List<Architecture.Model.Lab.LabAnalysisScheduleItemAndPartyWise> GetData(string procName, List<SqlParameter> p = null)
	{
		try
		{
			using SqlConnection sqlCon = ConnectionObject.SQL();
			sqlCon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Lab.LabAnalysisScheduleItemAndPartyWise>(sqlCon, procName, p);
		}
		catch
		{
			throw;
		}
	}
}
