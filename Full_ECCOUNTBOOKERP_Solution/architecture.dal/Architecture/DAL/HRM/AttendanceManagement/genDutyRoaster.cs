using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.AttendanceManagement;

namespace Architecture.DAL.HRM.AttendanceManagement;

public class genDutyRoaster
{
	public long SetData(Architecture.Model.HRM.AttendanceManagement.genDutyRoaster obj, string ProcName)
	{
		long returnID;
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				returnID = Convert.ToInt64(GenericProvider.SetProc(sqlTrn, obj, ProcName));
				if (returnID > 0)
				{
					obj.DutyRoasterId = Convert.ToInt64(returnID);
				}
				else
				{
					returnID = Convert.ToInt64(obj.DutyRoasterId);
				}
				foreach (genDutyRoasterDetail item2 in obj.GenDutyRoasterDetailsList)
				{
					item2.DutyRoasterId = obj.DutyRoasterId;
					Convert.ToInt64(GenericProvider.SetProc(sqlTrn, item2, "Sp_genDutyRoasterDetail_Insert"));
				}
				foreach (DutyRoasterGroups item in obj.DutyRoasterGroupsList)
				{
					item.DutyRoasterId = obj.DutyRoasterId;
					Convert.ToInt64(GenericProvider.SetProc(sqlTrn, item, "Sp_DutyRoasterGroups_Insert"));
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				throw ex;
			}
		}
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@OrganizationId",
			Value = obj.OrganizationId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@CompanyId",
			Value = obj.CompanyId
		});
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceFromPull]", p);
		CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceStatus]", p);
		return returnID;
	}

	public List<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster> lst = new List<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.AttendanceManagement.genDutyRoaster>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.AttendanceManagement.genDutyRoaster item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.DutyRoasterId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDutyRoasterGroupById"));
					item.DutyRoasterGroupsList = GenericProvider.GetAllDetail<DutyRoasterGroups>(sqlcon, "Sp_genDutyRoaster_GetAllMethod", lstPrm);
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
