using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model;
using Architecture.Model.Lab;

namespace Architecture.BLL.Lab;

public class InvLabAnalysisGroup
{
	public static int Save(Architecture.Model.Lab.InvLabAnalysisGroup obj)
	{
		try
		{
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.InvLabAnalysisGroup().SetDate(obj, "Sp_InvLabAnalysisGroup_Insert");
			}
			return new Architecture.DAL.Lab.InvLabAnalysisGroup().SetDate(obj, "Sp_InvLabAnalysisGroup_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AnalysisType()
	{
		DataTable dt = new DataTable();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			using SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[vAnalysisType]", sqlcon);
			cmd.CommandType = CommandType.Text;
			using SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			return dt;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable GetAllOrById(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryId",
					Value = obj.ParentCategoryId
				});
			}
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisGroup>("Sp_InvLabAnalysisGroup_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllItemsFromByAnalysisGroup(int OrganizationId, int CompanyId, int GroupTypeId)
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
				ParameterName = "@Id",
				Value = GroupTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllItemsFromByAnalysisGroup"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisGroup>("Sp_InvLabAnalysisGroup_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllItemsFromByAnalysisGroupandPurchaseOrder(int OrganizationId, int CompanyId, int GroupTypeId, int OrderId)
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
				ParameterName = "@Id",
				Value = GroupTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderId",
				Value = OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllItemsFromByAnalysisGroupandPurchaseOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisGroup>("Sp_InvLabAnalysisGroup_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
