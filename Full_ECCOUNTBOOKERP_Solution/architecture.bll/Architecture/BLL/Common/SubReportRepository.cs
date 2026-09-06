using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Common;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Common;

public static class SubReportRepository
{
	public static SubReportObject GetCompanyHeader(int ReportId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ReportConfigId",
				Value = ReportId
			});
			return new SubReportObject(GenericProvider.GetDataTableProc<InvPurchaseInvoice>("SP_GetCompanyHeader", p), "CompanyHeader.rpt");
		}
		catch
		{
			throw;
		}
	}

	public static SubReportObject GetPurchaseInvoiceSupplier(int Id)
	{
		try
		{
			return new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(Id), "InvRptPurchaseBillSupplierOthers");
		}
		catch
		{
			throw;
		}
	}

	public static DataTable InvPurchaseInvoiceSlipReport220SupReprtForItem(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PihId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<InvPurchaseInvoice>("SP_InvPurchaseInvoice_ItemOthersAddLess_SubRpt", p);
		}
		catch
		{
			throw;
		}
	}
}
