using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.ApprovalManagement;

namespace Architecture.DAL.HRM.ApprovalManagement;

public class EmployeeLoanInstallment
{
	public long SetData(Architecture.Model.HRM.ApprovalManagement.EmployeeLoanInstallment obj, string ProcName)
	{
		long returnID = 0L;
		using (SqlConnection sqlcon = ConnectionObject.SQL())
		{
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (Architecture.Model.HRM.ApprovalManagement.EmployeeLoanInstallment item in obj.EmployeeLoanInstallmentslist)
				{
					if (item.EmployeeLoanInstallmentId == 0L)
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
		p.Add(new SqlParameter
		{
			ParameterName = "@EmployeeLoanId",
			Value = obj.EmployeeLoanId
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@AppliedOn",
			Value = obj.EmployeeLoanInstallmentslist[0].CreatedOn
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@ApprovedDate",
			Value = obj.EmployeeLoanInstallmentslist[0].CreatedOn
		});
		p.Add(new SqlParameter
		{
			ParameterName = "@ApprovedBy",
			Value = obj.EmployeeLoanInstallmentslist[0].CreatedById
		});
		CommonProvider.ExecuteProcedure("[dbo].[Sp_EmployeeLoan_ApprovedAmountUpdate]", p);
		return returnID;
	}
}
