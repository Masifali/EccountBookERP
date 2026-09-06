using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class SubsidiaryAccountsDefine
{
	public static int Save(Architecture.Model.SubsidiaryAccountsDefine obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SubsidiaryAccountsDefine().SetData(obj, "USP_SubsidiaryAccountsDefine_Insert");
			}
			return new Architecture.DAL.SubsidiaryAccountsDefine().SetData(obj, "USP_SubsidiaryAccountsDefine_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.SubsidiaryAccountsDefine obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.SubsidiaryAccountsDefine>("USP_SubsidiaryAccountsDefine_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
