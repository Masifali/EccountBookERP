using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Service;
using Architecture.Model.Service;

namespace Architecture.BLL.Service;

public class ItemPricingSupplierCustomer
{
	public static int Save(Architecture.Model.Service.ItemPricingSupplierCustomer obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Service.ItemPricingSupplierCustomer().SetDate(obj, "Sp_ItemPricingSupplierCustomer_Insert");
			}
			return new Architecture.DAL.Service.ItemPricingSupplierCustomer().SetDate(obj, "Sp_ItemPricingSupplierCustomer_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Service.ItemPricingSupplierCustomer obj)
	{
		DataTable dt = null;
		try
		{
			int code = 0;
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
				Value = "GenerateDocNoCompanyIdOrganizationId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Service.ItemPricingSupplierCustomer>("Sp_ItemPricingSupplierCustomer_GetAll", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Service.ItemPricingSupplierCustomer GetByID(int Id)
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
			new List<Architecture.Model.Service.ItemPricingSupplierCustomer>();
			return new Architecture.DAL.Service.ItemPricingSupplierCustomer().GetDate("Sp_ItemPricingSupplierCustomer_GetAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryData(Architecture.Model.Service.ItemPricingSupplierCustomer obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Service.ItemPricingSupplierCustomer>("Sp_ItemPricingSupplierCustomer_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerFromItemPricingSupplierCustomer(Architecture.Model.Service.ItemPricingSupplierCustomer obj)
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
				Value = "GetSupplierCustomerFromItemPricingSupplierCustomer"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Service.ItemPricingSupplierCustomer>("Sp_ItemPricingSupplierCustomer_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadDetailByOrganizationCompanyIdNSupplierId(Architecture.Model.Service.ItemPricingSupplierCustomer obj)
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
				ParameterName = "@Activity",
				Value = "ReadDetailByOrganizationCompanyIdNSupplierId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Service.ItemPricingSupplierCustomer>("Sp_ItemPricingSupplierCustomer_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
