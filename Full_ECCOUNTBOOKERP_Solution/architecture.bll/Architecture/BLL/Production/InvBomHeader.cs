using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class InvBomHeader
{
	public static int Save(Architecture.Model.Production.InvBomHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.InvBomHeader().SetData(obj, "Sp_InvBomHeader_Insert");
			}
			return new Architecture.DAL.Production.InvBomHeader().SetData(obj, "Sp_InvBomHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Production.InvBomHeader obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvBomHeader>("Sp_InvBomHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Production.InvBomHeader GetByID(int Id)
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
			new List<Architecture.Model.Production.InvBomHeader>();
			return new Architecture.DAL.Production.InvBomHeader().GetDate("Sp_InvBomHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetIdBySrNo(Architecture.Model.Production.InvBomHeader obj)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@BomCode",
				Value = obj.BomCode
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadIdByBOMCode"
			});
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvBomHeader>("Sp_InvBomHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
