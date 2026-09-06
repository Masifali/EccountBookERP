using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Main.Commission;
using Architecture.Model.Main.Commission;

namespace Architecture.BLL.Main.Commission;

public class CommissionType
{
	public static int Save(Architecture.Model.Main.Commission.CommissionType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Main.Commission.CommissionType().SetData(obj, "[USP_CommissionType_Insert]");
			}
			return new Architecture.DAL.Main.Commission.CommissionType().SetData(obj, "[dbo].[USP_CommissionType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Main.Commission.CommissionType GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Main.Commission.CommissionType>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Main.Commission.CommissionType().GetData("[USP_CommissionType_ReadById]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Main.Commission.CommissionType obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Main.Commission.CommissionType>("[USP_CommissionType_History]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DropDown(int OrganizationId, int CompanyId, string Activity = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Main.Commission.CommissionType>("[dbo].[USP_GetDropDownDataForAllocateBrandItemToCommissionType]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
