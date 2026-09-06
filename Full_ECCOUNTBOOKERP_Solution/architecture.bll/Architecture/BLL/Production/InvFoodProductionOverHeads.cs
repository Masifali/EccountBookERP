using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class InvFoodProductionOverHeads
{
	public static int Save(Architecture.Model.Production.InvFoodProductionOverHeads obj)
	{
		try
		{
			if (obj.InvFoodProductionOverHeadsList.Count > 0)
			{
				obj.OrganizationId = obj.InvFoodProductionOverHeadsList[0].OrganizationId;
				obj.CompanyId = obj.InvFoodProductionOverHeadsList[0].CompanyId;
			}
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.InvFoodProductionOverHeads().SetData(obj, "Sp_InvFoodProductionOverHeads_Insert");
			}
			return new Architecture.DAL.Production.InvFoodProductionOverHeads().SetData(obj, "Sp_InvFoodProductionOverHeads_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Production.InvFoodProductionOverHeads GetByID(int Id)
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
			new List<Architecture.Model.Production.InvFoodProductionOverHeads>();
			return new Architecture.DAL.Production.InvFoodProductionOverHeads().GetDate("Sp_InvFoodProductionOverHeads_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Production.InvFoodProduction obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadSerialNumber"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProductionOverHeads>("Sp_InvFoodProductionOverHeads_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Production.InvFoodProduction obj)
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
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProductionOverHeads>("Sp_InvFoodProductionOverHeads_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetJobOrderIdByOHId(int Id)
	{
		int JobId = 0;
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
				Value = "GetJobOrderIdByOHId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProductionOverHeads>("Sp_InvFoodProductionOverHeads_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				JobId = Conversion.ToInt(dt.Rows[0][0]);
			}
			return JobId;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByJobOrderID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByjobProductionId"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProductionPackingMaterial>("Sp_InvFoodProductionOverHeads_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
