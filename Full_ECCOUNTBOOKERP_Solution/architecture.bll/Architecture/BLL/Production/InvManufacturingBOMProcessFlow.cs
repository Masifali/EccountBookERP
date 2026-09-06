using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class InvManufacturingBOMProcessFlow
{
	public static int Save(Architecture.Model.Production.InvManufacturingBOMProcessFlow obj)
	{
		try
		{
			return new Architecture.DAL.Production.InvManufacturingBOMProcessFlow().SetData(obj, "Sp_InvManufacturingBOMProcessFlow_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByID(int Id)
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
				Value = "BOMProcessFlowReadByHeaderId"
			});
			new List<Architecture.Model.Production.InvManufacturingBOMProcessFlow>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvManufacturingBOMHeader>("Sp_InvManufacturingBOMHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Production.InvManufacturingBOMHeader obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvManufacturingBOMHeader>("Sp_InvManufacturingBOMHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveByID(int DelId)
	{
		try
		{
			new Architecture.DAL.Production.InvManufacturingBOMProcessFlow().DeleteByID(DelId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
