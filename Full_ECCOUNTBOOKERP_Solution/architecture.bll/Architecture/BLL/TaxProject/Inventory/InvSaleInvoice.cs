using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.BLL.TaxProject.Inventory.Financials.SaleInvoice;
using Architecture.DAL.Common;
using Architecture.DAL.TaxProject.Inventory;
using Architecture.Model.Inventory;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.BLL.TaxProject.Inventory;

public class InvSaleInvoice
{
	public static int Save(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				if (obj.InvSaleInvoiceDetaillist.Exists((Architecture.Model.TaxProject.Inventory.InvSaleInvoiceDetail p) => p.Id > 0))
				{
					throw new Exception("Record cannot be inserted because detailId greater than zero");
				}
				obj.ModifyUser = 0;
				obj.VoucherHeadInvoices = SaleInvoiceFinancial.MakeVoucherForSaleInvoice(obj);
				return new Architecture.DAL.TaxProject.Inventory.InvSaleInvoice().SetData(obj, "Sp_InvSaleInvoice_Insert");
			}
			obj.EntryUser = 0;
			obj.VoucherHeadInvoices = SaleInvoiceFinancial.MakeVoucherForSaleInvoice(obj);
			return new Architecture.DAL.TaxProject.Inventory.InvSaleInvoice().SetData(obj, "Sp_InvSaleInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TaxScheduleMain_GetLatestSchedule(int OrganizationId, int CompanyId, DateTime EffectedDate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectedDate",
				Value = EffectedDate
			});
			return GenericProvider.GetDataTableProcForServer<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[Sp_TaxScheduleMain_GetLatestSchedule]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
