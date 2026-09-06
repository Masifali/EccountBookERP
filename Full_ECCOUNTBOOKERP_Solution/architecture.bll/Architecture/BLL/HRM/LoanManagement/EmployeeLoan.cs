using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.LoanManagement;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.HRM.LoanManagement;

namespace Architecture.BLL.HRM.LoanManagement;

public class EmployeeLoan
{
	public static VoucherHead MakeVoucher(Architecture.Model.HRM.LoanManagement.EmployeeLoan obj)
	{
		List<GetAccountsByEmployeeId> lstEmployee = new List<GetAccountsByEmployeeId>();
		new DataTable();
		VoucherHead vh = new VoucherHead();
		try
		{
			int CreatedById = Conversion.ToInt(obj.ApprovedById);
			int AlteredById = Conversion.ToInt(obj.AlteredById);
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.EmployeeLoanId;
			vh.RefDocNoId = obj.EmployeeLoanId;
			vh.VoucherCode = Conversion.ToInt(obj.EmployeeLoanId);
			vh.VoucherDate = obj.AppliedOn;
			vh.Remarks = Conversion.ToString(obj.Reason);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchId;
			vh.ProjectId = 0;
			vh.ManualBillNo = "";
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = CreatedById;
			vh.EntryDate = obj.CreatedOn;
			vh.ModifyDate = obj.AlteredOn;
			vh.ModifyUser = AlteredById;
			vh.VoucherAmount = Conversion.ToDouble(obj.ApprovedAmount);
			vh.BillAmount = Conversion.ToDouble(obj.ApprovedAmount);
			vh.voucherDetailList = new List<VoucherDetail>();
			lstEmployee = CommonServies.GetGlAccountsByEmployeeId(obj.OrganizationId, obj.CompanyId);
			if (lstEmployee.Count > 0)
			{
				List<GetAccountsByEmployeeId> find = lstEmployee.Where((GetAccountsByEmployeeId x) => x.EmployeeId == Conversion.ToInt(obj.EmployeeId)).ToList();
				if (find.Count > 0)
				{
					double MonthlyInstallment = Conversion.ToDouble(obj.ApprovedAmount) / (double)obj.NoOfInstallment;
					string Remarks = obj.Reason + " Loan Approved " + obj.ApprovedAmount + " Noof Installments " + obj.NoOfInstallment + " Monthly Installment " + Math.Round(MonthlyInstallment, 2);
					VoucherDetail vd3 = new VoucherDetail();
					vd3.AccountId = Conversion.ToInt(find[0].LoanAcId);
					vd3.AgainstAccountId = Conversion.ToInt(find[0].PayableAcId);
					vd3.Comments = Conversion.ToString(Remarks);
					vd3.DebitAmount = Conversion.ToDouble(obj.ApprovedAmount);
					vd3.EmployeeId = Conversion.ToInt(obj.EmployeeId);
					vd3.SubsidiaryTypeId = 2;
					vd3.SubsidiaryAccountId = Conversion.ToInt(obj.EmployeeId);
					vh.voucherDetailList.Add(vd3);
					VoucherDetail vd4 = new VoucherDetail();
					vd4.AccountId = Conversion.ToInt(find[0].PayableAcId);
					vd4.AgainstAccountId = Conversion.ToInt(find[0].LoanAcId);
					vd4.Comments = Conversion.ToString(Remarks);
					vd4.CreditAmount = Conversion.ToDouble(obj.ApprovedAmount);
					vd4.EmployeeId = Conversion.ToInt(obj.EmployeeId);
					vd4.SubsidiaryTypeId = 2;
					vd4.SubsidiaryAccountId = Conversion.ToInt(obj.EmployeeId);
					vh.voucherDetailList.Add(vd4);
					return vh;
				}
				throw new Exception("Employee Accounts List Not found");
			}
			throw new Exception("Employee Accounts List Not found");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static int Save(Architecture.Model.HRM.LoanManagement.EmployeeLoan obj)
	{
		try
		{
			if (obj.EmployeeLoanId == 0)
			{
				obj.ActionTypeId = 1;
				return new Architecture.DAL.HRM.LoanManagement.EmployeeLoan().SetData(obj, "hrm.Sp_EmployeeLoan_Insert");
			}
			obj.ActionTypeId = 2;
			if (obj.IsApproved)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
			}
			return new Architecture.DAL.HRM.LoanManagement.EmployeeLoan().SetData(obj, "hrm.Sp_EmployeeLoan_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.LoanManagement.EmployeeLoan GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EmployeeLoanId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.HRM.LoanManagement.EmployeeLoan>();
			return new Architecture.DAL.HRM.LoanManagement.EmployeeLoan().GetAll("hrm.Sp_EmployeeLoan_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
	{
		try
		{
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.LoanManagement.EmployeeLoan>("hrm.Sp_EmployeeLoan_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
