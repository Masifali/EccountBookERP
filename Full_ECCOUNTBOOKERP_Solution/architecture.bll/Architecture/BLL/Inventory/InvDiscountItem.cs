using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InvDiscountItem
{
	public static int Save(Architecture.Model.Inventory.InvDiscountItem obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvDiscountItem().SetData(obj, "[USP_InvDiscountItem_Insert]");
			}
			return new Architecture.DAL.Inventory.InvDiscountItem().SetData(obj, "[USP_InvDiscountItem_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvDiscountItem GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Inventory.InvDiscountItem>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Inventory.InvDiscountItem().GetData("[USP_InvDiscountItem_ReadById]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.InvDiscountItem obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDiscountItem>("USP_InvDiscountItem_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
