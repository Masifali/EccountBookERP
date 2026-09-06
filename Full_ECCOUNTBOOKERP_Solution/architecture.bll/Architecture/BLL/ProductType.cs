using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class ProductType
{
	public static int save(Architecture.Model.ProductType cty)
	{
		try
		{
			cty.EntryDate = DateTime.Now;
			cty.ModifyDate = DateTime.Now;
			if (cty.Id == 0)
			{
				return new Architecture.DAL.ProductType().SetData(cty, "[dbo].[USP_ProductType_Insert]");
			}
			return new Architecture.DAL.ProductType().SetData(cty, "[dbo].[USP_ProductType_Update]");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FormHistory(Architecture.Model.ProductType obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<Architecture.Model.ProductType>("[dbo].[USP_ProductType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ProductType ReadById(int Id)
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
			new List<Architecture.Model.ProductType>();
			return new Architecture.DAL.ProductType().GetAll("USP_ProductType_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
