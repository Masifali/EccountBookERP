using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemCustomGroup
{
	public static int save(Architecture.Model.Inventory.ItemCustomGroup cty)
	{
		try
		{
			cty.EntryDate = DateTime.Now;
			cty.ModifyDate = DateTime.Now;
			if (cty.Id == 0)
			{
				return new Architecture.DAL.Inventory.ItemCustomGroup().SetData(cty, "[dbo].[USP_ItemCustomGroup_Insert]");
			}
			return new Architecture.DAL.Inventory.ItemCustomGroup().SetData(cty, "[dbo].[USP_ItemCustomGroup_Update]");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.ItemCustomGroup obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemCustomGroup>("[dbo].[USP_ItemCustomGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadById(Architecture.Model.Inventory.ItemCustomGroup obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Id", obj.Id));
			p.Add(new SqlParameter("@Activity", "ReadById"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemCustomGroup>("[dbo].[USP_ItemCustomGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
