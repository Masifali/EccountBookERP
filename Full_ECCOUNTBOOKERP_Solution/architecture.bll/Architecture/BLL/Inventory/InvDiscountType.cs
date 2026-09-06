using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InvDiscountType
{
	public static int Save(Architecture.Model.Inventory.InvDiscountType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvDiscountType().SetData(obj, "[USp_InvDiscountType_Insert]");
			}
			return new Architecture.DAL.Inventory.InvDiscountType().SetData(obj, "[Usp_InvDiscountType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvDiscountType GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Inventory.InvDiscountType>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Inventory.InvDiscountType().GetData("[Usp_InvDiscountType_ReadById]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.InvDiscountType obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvDiscountType>("[Usp_InvDiscountType_History]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
