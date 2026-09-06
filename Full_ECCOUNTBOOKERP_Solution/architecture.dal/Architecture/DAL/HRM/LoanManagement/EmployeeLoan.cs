using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.HRM.ApprovalManagement;
using Architecture.Model.HRM.LoanManagement;

namespace Architecture.DAL.HRM.LoanManagement;

public class EmployeeLoan
{
	public int SetData(Architecture.Model.HRM.LoanManagement.EmployeeLoan obj, string ProcName)
	{
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EmployeeLoanId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.EmployeeLoanId;
			}
			foreach (EmployeeLoanInstallment item3 in obj.EmployeeLoanInstallmentslist)
			{
				item3.EmployeeLoanId = obj.EmployeeLoanId;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "hrm.Sp_EmployeeLoanInstallment_Insert"));
			}
			if (obj.IsApproved && obj.ActionTypeId == 2)
			{
				DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.EmployeeLoanId);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.EmployeeLoanId;
					obj.VoucherHeadInvoices.RefDocNoId = obj.EmployeeLoanId;
					obj.VoucherHeadInvoices.VoucherCode = obj.EmployeeLoanId;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.EmployeeLoanId;
					obj.VoucherHeadInvoices.RefDocNoId = obj.EmployeeLoanId;
					obj.VoucherHeadInvoices.VoucherCode = obj.EmployeeLoanId;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadInvoices.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadInvoices.Id;
				}
				if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count <= 0)
				{
					throw new Exception("VoucherDetail List Not Found");
				}
				foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "USP_VoucherBalanceCheck";
				sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCommand.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
				sqlCommand.ExecuteScalar();
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
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

	public List<Architecture.Model.HRM.LoanManagement.EmployeeLoan> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.LoanManagement.EmployeeLoan> lst = new List<Architecture.Model.HRM.LoanManagement.EmployeeLoan>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.LoanManagement.EmployeeLoan>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
