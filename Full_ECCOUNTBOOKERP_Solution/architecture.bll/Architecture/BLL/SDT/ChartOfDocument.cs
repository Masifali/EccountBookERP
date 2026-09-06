using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.SDT;
using Architecture.Model.Inventory;
using Architecture.Model.SDT;

namespace Architecture.BLL.SDT;

public class ChartOfDocument
{
	public static int save(Architecture.Model.SDT.ChartOfDocument obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.SDT.ChartOfDocument().SetData(obj, "[sdt].[USP_ChartOfDocument_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FormHistory(Architecture.Model.SDT.ChartOfDocument obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<ChartOfDocument>("[sdt].[USP_ChartOfDocument_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.SDT.ChartOfDocument ReadById(int id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@chartOfDocumentId",
				Value = id
			});
			new List<Architecture.Model.SDT.ChartOfDocument>();
			return new Architecture.DAL.SDT.ChartOfDocument().GetAll("[sdt].[USP_ChartOfDocument_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetChartOfDocumentForCombo(Architecture.Model.SDT.ChartOfDocument obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@Activity", "GetChartOfDocumentForCombo"));
			return GenericProvider.GetDataTableProc<ChartOfDocument>("[sdt].[USP_ChartOfDocument_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetChartOfDocumentByCustomGroupId(int OrganizationId, int CompanyId, int CustomGroupId)
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
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[sdt].[USP_GetChartOfDocumentByCustomGroupId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable RequiredLevelForCombo()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "RequiredLevelForCombo"));
			return GenericProvider.GetDataTableProc<ChartOfDocument>("[sdt].[USP_ChartOfDocument_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CriteriaDateTypeForCombo()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "CriteriaDateTypeForCombo"));
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[sdt].[USP_ChartOfDocument_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DocumentProviderTypeForCombo()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "DocumentProviderForCombo"));
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[sdt].[USP_ChartOfDocument_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LevelNameForCombo()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Activity", "LevelNameForCombo"));
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[sdt].[USP_ChartOfDocument_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
