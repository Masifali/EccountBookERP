using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.HRM.PayrollManagement;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.HRM.PayrollManagement;
using Architecture.Model.POS;

namespace Architecture.BLL.HRM.PayrollManagement;

public class Payroll
{
	public static VoucherHead MakeVoucher(Architecture.Model.HRM.PayrollManagement.Payroll obj)
	{
		List<GetAccountsByEmployeeId> lstEmployee = new List<GetAccountsByEmployeeId>();
		new DataTable();
		VoucherHead vh = new VoucherHead();
		try
		{
			int PayrollId = Conversion.ToInt(obj.PayrollId);
			int OrganizationId = Conversion.ToInt(obj.OrganizationId);
			int CompanyId = Conversion.ToInt(obj.CompanyId);
			int CreatedById = Conversion.ToInt(obj.CreatedById);
			int AlteredById = Conversion.ToInt(obj.AlteredById);
			vh.DocumentTypeId = obj.DocMovementId;
			vh.DocumentTypeSrNo = PayrollId;
			vh.RefDocNoId = PayrollId;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.PayrollDate;
			vh.Remarks = Conversion.ToString(obj.Remarks);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.ManualBillNo = Conversion.ToString(obj.DocNo);
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = OrganizationId;
			vh.CompanyId = CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = CreatedById;
			vh.EntryDate = obj.CreatedOn;
			vh.ModifyDate = obj.AlteredOn;
			vh.ModifyUser = AlteredById;
			vh.voucherDetailList = new List<VoucherDetail>();
			lstEmployee = CommonServies.GetGlAccountsByEmployeeId(OrganizationId, CompanyId);
			if (obj.PayrollDetailList.Count > 0)
			{
				string Remarks = string.Empty;
				double TotalAmount = 0.0;
				string MonthName = "";
				MonthName = new DateTime(obj.PayrollYear, obj.PayrollMonth, 1).ToString("MMM");
				foreach (PayrollDetail item in obj.PayrollDetailList.ToList())
				{
					if (item.ActionTypeId != 3)
					{
						if (lstEmployee.Count <= 0)
						{
							throw new Exception("Employee Accounts List Not found");
						}
						List<GetAccountsByEmployeeId> find = lstEmployee.Where((GetAccountsByEmployeeId x) => x.EmployeeId == Conversion.ToInt(item.EmployeeId)).ToList();
						if (find.Count <= 0)
						{
							throw new Exception("Employee Accounts List Not found");
						}
						Remarks = BuildSalaryRemarks(item, MonthName, obj.PayrollYear);
						double NetSalaryWithAdvance = Conversion.ToDouble(item.NetSalary) + Conversion.ToDouble(item.AdvanceAmount) + Conversion.ToDouble(item.LoanAmount);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(find[0].ExpenseAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(find[0].PayableAcId);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.DebitAmount = Conversion.ToDouble(NetSalaryWithAdvance);
						vh.voucherDetailList.Add(vd3);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(find[0].PayableAcId);
						vd4.AgainstAccountId = Conversion.ToInt(find[0].ExpenseAccountId);
						vd4.Comments = Conversion.ToString(Remarks);
						vd4.CreditAmount = Conversion.ToDouble(NetSalaryWithAdvance);
						vd4.EmployeeId = Conversion.ToInt(item.EmployeeId);
						vd4.SubsidiaryTypeId = 2;
						vd4.SubsidiaryAccountId = Conversion.ToInt(item.EmployeeId);
						vh.voucherDetailList.Add(vd4);
						TotalAmount += vd4.CreditAmount;
						if (item.LoanAmount > 0m)
						{
							Remarks = "Salary M/O " + MonthName + " - " + obj.PayrollYear;
							Remarks = Remarks + "  Loan No of Instalments " + item.LoanNOS;
							Remarks = Remarks + "  and Loan Amount is " + Math.Round(item.LoanAmount, 0, MidpointRounding.AwayFromZero);
							VoucherDetail vd5 = new VoucherDetail();
							vd5.AccountId = Conversion.ToInt(find[0].PayableAcId);
							vd5.AgainstAccountId = Conversion.ToInt(find[0].LoanAcId);
							vd5.Comments = Conversion.ToString(Remarks);
							vd5.DebitAmount = Conversion.ToDouble(item.LoanAmount);
							vd5.EmployeeId = Conversion.ToInt(item.EmployeeId);
							vd5.SubsidiaryTypeId = 2;
							vd5.SubsidiaryAccountId = Conversion.ToInt(item.EmployeeId);
							vh.voucherDetailList.Add(vd5);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.AccountId = Conversion.ToInt(find[0].LoanAcId);
							vd6.AgainstAccountId = Conversion.ToInt(find[0].PayableAcId);
							vd6.Comments = Conversion.ToString(Remarks);
							vd6.CreditAmount = Conversion.ToDouble(item.LoanAmount);
							vd6.EmployeeId = Conversion.ToInt(item.EmployeeId);
							vd6.SubsidiaryTypeId = 2;
							vd6.SubsidiaryAccountId = Conversion.ToInt(item.EmployeeId);
							vh.voucherDetailList.Add(vd6);
							TotalAmount += vd6.CreditAmount;
						}
					}
				}
				vh.VoucherAmount = TotalAmount;
				vh.BillAmount = TotalAmount;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	private static string BuildSalaryRemarks(PayrollDetail item, string monthName, int payrollYear)
	{
		string remarks = $"Salary M/O {monthName} - {payrollYear} Gross Salary {Math.Round(item.GrossSalary, 0, MidpointRounding.AwayFromZero)}";
		(string, decimal, bool)[] array = new(string, decimal, bool)[18]
		{
			("Late Deduction", item.LateDeductionAmount, true),
			("IncomeTax", item.IncomeTaxAmount, true),
			("LeaveAmount", item.LeaveAmount, true),
			("Advance", item.AdvanceAmount, true),
			("Loan", item.LoanAmount, true),
			("PFAmount", item.PFAmount, true),
			("EOBIA", item.EOBIAmount, true),
			("MiscLessAmount", item.MiscLessAmount, true),
			("Total Less", item.TotalAmount, true),
			("OTAmount", item.OTAmount, true),
			("Travelling", item.TravellingAmount, true),
			("Medical", item.MedicalAmount, true),
			("MobileAmount", item.MobileAmount, true),
			("FuelAmount", item.FuelAmount, true),
			("FoodAmount", item.FoodAmount, true),
			("MiscAddAmount", item.MiscAddAmount, true),
			("Extra AddLess", item.AddLessAmount, false),
			("NetSalary", item.NetSalary, true)
		};
		for (int i = 0; i < array.Length; i++)
		{
			(string, decimal, bool) field = array[i];
			if (!(field.Item2 == 0m))
			{
				decimal value = (field.Item3 ? Math.Round(field.Item2, 0, MidpointRounding.AwayFromZero) : field.Item2);
				remarks += $"  {field.Item1} {value}";
			}
		}
		return remarks;
	}

	public static long Save(Architecture.Model.HRM.PayrollManagement.Payroll obj)
	{
		try
		{
			bool PayRollPostingWithoutApproval = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(Conversion.ToInt(obj.OrganizationId), Conversion.ToInt(obj.CompanyId), "PayRollPostingWithoutApproval"));
			if (obj.IsApproved || PayRollPostingWithoutApproval)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
			}
			if (obj.PayrollId == 0L)
			{
				return new Architecture.DAL.HRM.PayrollManagement.Payroll().SetData(obj, "hrm.Sp_Payroll_Insert");
			}
			return new Architecture.DAL.HRM.PayrollManagement.Payroll().SetData(obj, "hrm.Sp_Payroll_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.HRM.PayrollManagement.Payroll GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PayrollId",
				Value = Id
			});
			new List<Architecture.Model.HRM.PayrollManagement.Payroll>();
			return new Architecture.DAL.HRM.PayrollManagement.Payroll().GetData("hrm.Sp_Payroll_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAll(ReportsParameters obj)
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
				ParameterName = "@DocMovementId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<WsRmCreditNoteHeader>("hrm.Sp_Payroll_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayrollPostingForMultiApproval(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
				});
			}
			if (obj.EmployeeCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EmployeeCategoryId",
					Value = obj.EmployeeCategoryId
				});
			}
			if (obj.DepartmentId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DepartmentId",
					Value = obj.DepartmentId
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
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
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.HRM.PayrollManagement.Payroll>("[dbo].[USP_PayrollPostingForMultiApproval]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.HRM.PayrollManagement.Payroll obj)
	{
		int DocNo = 0;
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
				ParameterName = "@DocMovementId",
				Value = obj.DocMovementId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<WsRmCreditNoteHeader>("hrm.Sp_Payroll_GetAllMethod", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				DocNo = Conversion.ToInt(dt.Rows[0][0]);
			}
			return DocNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
