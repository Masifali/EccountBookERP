using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InvAllocateBrandItemToDiscType
{
	public static int Save(Architecture.Model.Inventory.InvAllocateBrandItemToDiscType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvAllocateBrandItemToDiscType().SetData(obj, "[USP_InvAllocateBrandItemToDiscType_Insert]");
			}
			return new Architecture.DAL.Inventory.InvAllocateBrandItemToDiscType().SetData(obj, "[USP_InvAllocateBrandItemToDiscType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvAllocateBrandItemToDiscType GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Inventory.InvAllocateBrandItemToDiscType>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Inventory.InvAllocateBrandItemToDiscType().GetData("[USP_InvAllocateBrandItemToDiscType_ReadById]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.InvAllocateBrandItemToDiscType obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvAllocateBrandItemToDiscType>("[USP_InvAllocateBrandItemToDiscType_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
