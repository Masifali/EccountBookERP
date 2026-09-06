using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.DAL.HRM.EmployeeManagement;

public class hrmEmployeeBenefit
{
	public int SetData(Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit item in obj.EmployeeBenefitsList)
			{
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit> lst = new List<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
