using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class TaxSalesNotesGLMaping
{
	public static int Save(Architecture.Model.Inventory.TaxSalesNotesGLMaping obj)
	{
		try
		{
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.EntryUserId = clsGlobalVariables.UserAccount.ID;
			obj.ModifyUserId = clsGlobalVariables.UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.TaxSalesNotesGLMaping().SetData(obj, "Sp_TaxSalesNotesGLMaping_Insert");
			}
			return new Architecture.DAL.Inventory.TaxSalesNotesGLMaping().SetData(obj, "Sp_TaxSalesNotesGLMaping_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.TaxSalesNotesGLMaping GetByID(int Id)
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
			new List<Architecture.Model.Inventory.TaxSalesNotesGLMaping>();
			return new Architecture.DAL.Inventory.TaxSalesNotesGLMaping().GetData("Sp_TaxSalesNotesGLMaping_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Inventory.TaxSalesNotesGLMaping obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxSalesNotesGLMaping>("Sp_TaxSalesNotesGLMaping_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleTaxSummary(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxSalesNotesGLMaping>("Sp_Accounts_GenerateSalesTaxSummery_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
