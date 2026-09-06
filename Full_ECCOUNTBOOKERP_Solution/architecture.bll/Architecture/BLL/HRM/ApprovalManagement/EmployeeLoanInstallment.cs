using System;
using Architecture.DAL.HRM.ApprovalManagement;
using Architecture.Model.HRM.ApprovalManagement;

namespace Architecture.BLL.HRM.ApprovalManagement;

public class EmployeeLoanInstallment
{
	public static long Save(Architecture.Model.HRM.ApprovalManagement.EmployeeLoanInstallment obj)
	{
		try
		{
			return new Architecture.DAL.HRM.ApprovalManagement.EmployeeLoanInstallment().SetData(obj, "hrm.Sp_EmployeeLoanInstallment_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
