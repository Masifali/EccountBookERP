using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.DAL.HRM.PayrollManagement;

public class EmployeeLoanDeduction
{
	public long SetData(Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction item in obj.LoanDeductionsList)
			{
				if (item.EmployeeLoanDeductionId == 0L)
				{
					item.ActionTypeId = 1;
				}
				else
				{
					item.ActionTypeId = 2;
				}
				returnID = Convert.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
		return returnID;
	}

	public List<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction> lst = new List<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
