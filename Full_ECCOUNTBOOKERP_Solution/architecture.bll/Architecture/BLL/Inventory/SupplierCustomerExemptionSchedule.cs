using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class SupplierCustomerExemptionSchedule
{
	public static int Save(Architecture.Model.Inventory.SupplierCustomerExemptionSchedule obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.SupplierCustomerExemptionSchedule().SetData(obj, "Sp_SupplierCustomerExemptionSchedule_Insert");
			}
			return new Architecture.DAL.Inventory.SupplierCustomerExemptionSchedule().SetData(obj, "Sp_SupplierCustomerExemptionSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.SupplierCustomerExemptionSchedule GetByID(int Id)
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
			new List<Architecture.Model.Inventory.SupplierCustomerExemptionSchedule>();
			return new Architecture.DAL.Inventory.SupplierCustomerExemptionSchedule().GetAll("Sp_SupplierCustomerExemptionSchedule_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomerExemptionSchedule>("Sp_SupplierCustomerExemptionSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
