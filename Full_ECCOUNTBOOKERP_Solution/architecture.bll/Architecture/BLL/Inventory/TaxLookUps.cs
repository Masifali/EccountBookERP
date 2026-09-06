using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class TaxLookUps
{
	public static int Save(Architecture.Model.Inventory.TaxLookUps obj)
	{
		try
		{
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.TaxLookUps().SetData(obj, "Sp_TaxLookUps_Insert");
			}
			return new Architecture.DAL.Inventory.TaxLookUps().SetData(obj, "Sp_TaxLookUps_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.TaxLookUps GetByID(int Id)
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
			new List<Architecture.Model.Inventory.TaxLookUps>();
			return new Architecture.DAL.Inventory.TaxLookUps().GetData("Sp_TaxLookUps_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Inventory.TaxLookUps obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxLookUps>("Sp_TaxLookUps_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByTaxLookTypeId(Architecture.Model.Inventory.TaxLookUps obj)
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
				ParameterName = "@TaxLookUptypesId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyIdNTaxeLookUpTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxLookUps>("Sp_TaxLookUps_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
