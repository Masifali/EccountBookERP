using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class CommodityDetailItemCustomerWise
{
	public static int Save(Architecture.Model.CommodityDetailItemCustomerWise obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.CommodityDetailItemCustomerWise().SetData(obj, "[dbo].[USP_CommodityDetailItemCustomerWise_Insert]");
			}
			return new Architecture.DAL.CommodityDetailItemCustomerWise().SetData(obj, "[dbo].[USP_CommodityDetailItemCustomerWise_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.CommodityDetailItemCustomerWise obj)
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CommodityDetailItemCustomerWise>("[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetRemarks(Architecture.Model.CommodityDetailItemCustomerWise obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetRemarks"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CommodityDetailItemCustomerWise>("[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.CommodityDetailItemCustomerWise> FormHistoryWithList(Architecture.Model.CommodityDetailItemCustomerWise obj)
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
				Value = "FormHistory"
			});
			new List<Architecture.Model.CommodityDetailItemCustomerWise>();
			return new Architecture.DAL.CommodityDetailItemCustomerWise().GetAll("[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.CommodityDetailItemCustomerWise ReadById(int Id)
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
			new List<Architecture.Model.CommodityDetailItemCustomerWise>();
			return new Architecture.DAL.CommodityDetailItemCustomerWise().GetAll("[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
