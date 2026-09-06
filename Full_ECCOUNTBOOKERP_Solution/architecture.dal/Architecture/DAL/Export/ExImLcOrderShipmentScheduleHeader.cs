using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImLcOrderShipmentScheduleHeader : DALBase
{
	public int SetData(Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader obj, string ProcName)
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
				if (obj.LcOrderShipmentNInspectioNProductionScheduleDetail != null)
				{
					foreach (ExImLcOrderShipmentNInspectionNProductionSchedule item3 in obj.LcOrderShipmentNInspectioNProductionScheduleDetail)
					{
						item3.LcOrderId = obj.ExImLcOrderId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_ExImLcOrderShipmentNInspectionNProductionSchedule_Insert"));
					}
				}
			}
			else
			{
				returnID = obj.Id;
				if (obj.LcOrderShipmentNInspectioNProductionScheduleDetail != null)
				{
					foreach (ExImLcOrderShipmentNInspectionNProductionSchedule item4 in obj.LcOrderShipmentNInspectioNProductionScheduleDetail)
					{
						item4.LcOrderId = obj.ExImLcOrderId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_ExImLcOrderShipmentNInspectionNProductionSchedule_Update"));
					}
				}
			}
			if (obj.LcOrderAnalysisTestDetail != null)
			{
				foreach (ExImLcOrderAnalysisTestList item2 in obj.LcOrderAnalysisTestDetail)
				{
					item2.LcOrderId = obj.ExImLcOrderId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_ExImLcOrderAnalysisTestList_Insert"));
				}
			}
			if (obj.LcOrderDocumentDetail != null)
			{
				foreach (ExImLcOrderDocumentList item in obj.LcOrderDocumentDetail)
				{
					item.LcOrderId = obj.ExImLcOrderId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ExImLcOrderDocumentList_Insert"));
				}
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

	public int SetDataForDetail(ExImLcOrderShipmentNInspectionNProductionSchedule obj, string ProcName)
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader> lst = new List<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@ExImLcOrder", item3.ExImLcOrderId));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByDetailId"));
						item3.LcOrderShipmentNInspectioNProductionScheduleDetail = GenericProvider.Get<ExImLcOrderShipmentNInspectionNProductionSchedule>(sqlConnection2, "Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@ExImLcOrder", item2.ExImLcOrderId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByDocumentList"));
						item2.LcOrderDocumentDetail = GenericProvider.GetAllDetail<ExImLcOrderDocumentList>(sqlConnection3, "Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@ExImLcOrder", item.ExImLcOrderId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByLcOrderAnaylsisTestList"));
					item.LcOrderAnalysisTestDetail = GenericProvider.GetAllDetail<ExImLcOrderAnalysisTestList>(sqlcon, "Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", lstPrm);
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
