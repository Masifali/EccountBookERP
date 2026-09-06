using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.HRM.PayrollManagement;

namespace Architecture.DAL.HRM.PayrollManagement;

public class Payroll
{
	public long SetData(Architecture.Model.HRM.PayrollManagement.Payroll obj, string ProcName)
	{
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.PayrollId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.PayrollId;
			}
			foreach (PayrollDetail item3 in obj.PayrollDetailList)
			{
				item3.PayrollId = obj.PayrollId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "hrm.Sp_PayrollDetail_Insert"));
			}
			bool PayRollPostingWithoutApproval = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(Conversion.ToInt(obj.OrganizationId), Conversion.ToInt(obj.CompanyId), "PayRollPostingWithoutApproval"));
			if (obj.IsApproved || PayRollPostingWithoutApproval)
			{
				int PayrollId = Conversion.ToInt(obj.PayrollId);
				int OrganizationId = Conversion.ToInt(obj.OrganizationId);
				int CompanyId = Conversion.ToInt(obj.CompanyId);
				DataTable dtvh = CommonServices.GetVoucherHeadId(OrganizationId, CompanyId, obj.DocMovementId, PayrollId);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = PayrollId;
					obj.VoucherHeadInvoices.RefDocNoId = PayrollId;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = PayrollId;
					obj.VoucherHeadInvoices.RefDocNoId = PayrollId;
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
				sqlCommand.Parameters.AddWithValue("@OrganizationId", OrganizationId);
				sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
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

	public List<Architecture.Model.HRM.PayrollManagement.Payroll> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.PayrollManagement.Payroll> lst = new List<Architecture.Model.HRM.PayrollManagement.Payroll>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.PayrollManagement.Payroll>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.HRM.PayrollManagement.Payroll item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@PayrollId", item.PayrollId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.PayrollDetailList = GenericProvider.GetAllDetail<PayrollDetail>(sqlcon, "hrm.Sp_Payroll_GetAllMethod", lstPrm);
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
