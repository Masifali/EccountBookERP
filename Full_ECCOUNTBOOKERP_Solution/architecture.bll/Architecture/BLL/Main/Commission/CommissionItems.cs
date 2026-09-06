using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Main.Commission;
using Architecture.Model.Main.Commission;

namespace Architecture.BLL.Main.Commission;

public class CommissionItems
{
	public static int Save(Architecture.Model.Main.Commission.CommissionItems obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Main.Commission.CommissionItems().SetData(obj, "[USP_CommissionItems_Insert]");
			}
			return new Architecture.DAL.Main.Commission.CommissionItems().SetData(obj, "[USP_CommissionItems_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Main.Commission.CommissionItems GetById(int Id)
	{
		try
		{
			new List<Architecture.Model.Main.Commission.CommissionItems>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.Main.Commission.CommissionItems().GetData("[USP_CommissionItems_ReadById]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Main.Commission.CommissionItems obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Main.Commission.CommissionItems>("USP_CommissionItems_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
