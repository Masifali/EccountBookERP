using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class PackingChangePriceSchedule
{
	public static int Save(Architecture.Model.Inventory.PackingChangePriceSchedule obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.PackingChangePriceSchedule().SetData(obj, "USp_InvPackingChangePriceSchedule_Insert");
			}
			return new Architecture.DAL.Inventory.PackingChangePriceSchedule().SetData(obj, "USp_InvPackingChangePriceSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.PackingChangePriceSchedule GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Inventory.PackingChangePriceSchedule>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Inventory.PackingChangePriceSchedule().GetAll("USp_PackingChangePriceSchedule_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.PackingChangePriceSchedule obj)
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PackingChangePriceSchedule>("USp_InvPackingChangePriceSchedule_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPackingAddLess(ReportsParameters obj)
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
				ParameterName = "@UomId",
				Value = obj.ItemUomId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.EffectedDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PackingChangePriceSchedule>("Usp_InvPackingChangePriceScheduleGetPackingAddLess", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
