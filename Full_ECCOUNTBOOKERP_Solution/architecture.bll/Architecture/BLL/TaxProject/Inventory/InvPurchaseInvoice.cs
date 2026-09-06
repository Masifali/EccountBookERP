using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.BLL.TaxProject.Inventory.Financials.PurchaseInvoice;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.TaxProject.Inventory;
using Architecture.Model.Inventory;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.BLL.TaxProject.Inventory;

public class InvPurchaseInvoice
{
	public static int Save(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj)
	{
		try
		{
			obj.VoucherHeadInvoices = PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice(obj);
			if (obj.Id == 0)
			{
				if (obj.invPurchaseInvoiceDetailList.Exists((Architecture.Model.TaxProject.Inventory.InvPurchaseInvoiceDetail p) => p.Id > 0))
				{
					throw new Exception("Record cannot be inserted because detailId greater than zero");
				}
				obj.ActionId = 1;
				return new Architecture.DAL.TaxProject.Inventory.InvPurchaseInvoice().SetData(obj, "Sp_InvPurchaseInvoice_Insert");
			}
			obj.PreviousDate = GetRecordsById(obj.Id);
			obj.ActionId = 2;
			return new Architecture.DAL.TaxProject.Inventory.InvPurchaseInvoice().SetData(obj, "Sp_InvPurchaseInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DateTime GetRecordsById(int Id)
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
				Value = "GetRecordId"
			});
			return Conversion.ToDateTime(GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvPurchaseInvoice_GetAllMethod", p).Rows[0][0]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
