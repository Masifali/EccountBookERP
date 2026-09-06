using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.CmTr.TransactionForm;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.CmTr.TransactionForm;

namespace Architecture.BLL.CmTr.TransactionForm;

public class CommTradeRevenueExpenseSchedule
{
	public static int Save(Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule().SetData(obj, "[CmTr].[USP_CommTradeRevenueExpenseSchedule_Insert]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule().SetData(obj, "[CmTr].[USP_CommTradeRevenueExpenseSchedule_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule ReadById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>();
			return new Architecture.DAL.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule().GetData("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLookUpsBylookTypeId(int InvLookupTypeId = 0, int LookupId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (InvLookupTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvLookupTypeId",
					Value = InvLookupTypeId
				});
			}
			if (LookupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = LookupId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByInvlookTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCommScheduleRateByDateCalParamAndRevExpId(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectiveDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CalculationParameterId",
				Value = obj.CalculationParameterId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RevenueExpenseId",
				Value = obj.RevenueExpenseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ChargesIncentiveId",
				Value = obj.ChargesIncentiveId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PaymentTermId",
				Value = obj.PaymenetTermId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DueDays",
				Value = obj.IntervalDays
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCommScheduleRateByDateCalParamAndRevExpId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryDetail(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistoryDetail"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]", p);
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int EntryUserId, int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUserId",
				Value = EntryUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteById"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>("[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
