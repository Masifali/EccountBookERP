using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class SupplierCustomerTaxSchedule
{
	public static int Save(Architecture.Model.Inventory.SupplierCustomerTaxSchedule obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.SupplierCustomerTaxSchedule().SetData(obj, "USP_SupplierCustomerTaxSchedule_InsertOrUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.SupplierCustomerTaxSchedule GetByID(int Id)
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
			new List<Architecture.Model.Inventory.SupplierCustomerTaxSchedule>();
			return new Architecture.DAL.Inventory.SupplierCustomerTaxSchedule().GetData("USP_SupplierCustomerTaxSchedule_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Inventory.SupplierCustomerTaxSchedule obj)
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
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomerTaxSchedule>("USP_SupplierCustomerTaxSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TaxSchedule_GetBySupplierCustomer(ReportsParameters obj)
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
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomerTaxSchedule>("USP_TaxSchedule_GetBySupplierCustomer", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
