using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class ProductionOutputAllocationWithExportInvoice
{
	public static int Save(Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice obj)
	{
		try
		{
			return new Architecture.DAL.Production.ProductionOutputAllocationWithExportInvoice().SetData(obj, "[dbo].[USP_ProductionOutputAllocationWithExportInvoice_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataAgainstJobOrder(ReportsParameters obj)
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
				ParameterName = "@JobOrderId",
				Value = obj.JobOrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice>("USP_GetOutPutItemsWithSumOfQtyWeight", p);
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = obj.JobOrderId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice>("[dbo].[USP_ProductionOutputAllocationWithExportInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromProductionOutputAllocationWithExportInvoice(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice>("[dbo].[USP_GetDataForDropDownFromProductionOutputAllocationWithExportInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice> GetByID(int JobOrderId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@JobOrderId",
				Value = JobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice>();
			return new Architecture.DAL.Production.ProductionOutputAllocationWithExportInvoice().GetData("[dbo].[USP_ProductionOutputAllocationWithExportInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionOutputAllocationWithExportInvoice_SlipAndRegister(int JobOrderId = 0, int InvoiceId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (JobOrderId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = JobOrderId
				});
			}
			if (InvoiceId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = InvoiceId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProductionOutputAllocationWithExportInvoice>("[dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
