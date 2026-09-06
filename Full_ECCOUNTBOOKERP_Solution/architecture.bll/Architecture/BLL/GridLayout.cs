using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Export;

namespace Architecture.BLL;

public class GridLayout
{
	public static int Save(Architecture.Model.GridLayout obj)
	{
		try
		{
			return new Architecture.DAL.GridLayout().SetData(obj, "Sp_GridLayout_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.GridLayout GetByID(int OrganizationId, int CompanyId, string Layout)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Layout",
				Value = Layout
			});
			List<Architecture.Model.GridLayout> lst = new List<Architecture.Model.GridLayout>();
			lst = new Architecture.DAL.GridLayout().GetAll("USP_GridLayout_ReadById", p);
			if (lst.Count > 0)
			{
				return lst[0];
			}
			return null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByLayout(int OrganizationId, int CompanyId, string Layout)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Layout",
				Value = Layout
			});
			GenericProvider.GetDataTableProc<ExImInvoice>("USP_GridLayout_DeleteById", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
