using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class InvoiceWiseOpening
{
	public static int Save(Architecture.Model.Accounts.InvoiceWiseOpening obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.InvoiceWiseOpening().SetData(obj, "USP_InvoiceWiseOpening_Insert");
			}
			return new Architecture.DAL.Accounts.InvoiceWiseOpening().SetData(obj, "USP_InvoiceWiseOpening_Update");
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
			if (obj.CompanyId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
			}
			if (obj.Id > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.AccountId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
				});
			}
			if (obj.SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.FinancialYearId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryDateFrom",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryDateTo",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyDateFrom",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyDateTo",
					Value = obj.ModifyDateTo
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.InvoiceWiseOpening>("usp_InvoiceWiseOpening_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvoiceWiseOpening_DropDown(int CompanyId, int FinancialYearId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (CompanyId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = CompanyId
				});
			}
			if (FinancialYearId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = FinancialYearId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.InvoiceWiseOpening>("usp_InvoiceWiseOpening_DropDown", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteById(int CompanyId, int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
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
			GenericProvider.GetDataTableProc<Architecture.Model.Accounts.InvoiceWiseOpening>("usp_InvoiceWiseOpening_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
