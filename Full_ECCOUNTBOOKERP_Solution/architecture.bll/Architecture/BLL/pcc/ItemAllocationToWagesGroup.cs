using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class ItemAllocationToWagesGroup
{
	public static int Save(Architecture.Model.pcc.ItemAllocationToWagesGroup obj)
	{
		try
		{
			return new Architecture.DAL.pcc.ItemAllocationToWagesGroup().SetData(obj, "[pcc].[USP_ItemAllocationToWagesGroup_Insert]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.ItemAllocationToWagesGroup ReadById(int Id)
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
			new List<Architecture.Model.pcc.ItemAllocationToWagesGroup>();
			return new Architecture.DAL.pcc.ItemAllocationToWagesGroup().GetData("[pcc].[USP_ItemAllocationToWagesGroup_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemAllocationToWagesGroup>("[pcc].[USP_ContractorWagesGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(Architecture.Model.pcc.ItemAllocationToWagesGroup obj)
	{
		try
		{
			foreach (Architecture.Model.pcc.ItemAllocationToWagesGroup item in obj.ItemAllocationToCustomGroupsList)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemAllocationToWagesGroup>("[pcc].[USP_ItemAllocationToWagesGroup_GetAllMethod]", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemNotReferedInWagesGroupAllocation(int OrganizationId, int CompanyId, int BranchesId)
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
				ParameterName = "@BranchesId",
				Value = BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ItemNotReferedInWagesGroupAllocation"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemAllocationToWagesGroup>("[pcc].[USP_ItemAllocationToWagesGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemAllocatedOrUnAllocatedToWagesGroup(Architecture.Model.pcc.ItemAllocationToWagesGroup obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractorWagesGroupId",
				Value = obj.ContractorWagesGroupId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ItemAllocatedOrUnAllocatedToWagesGroup"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ItemAllocationToWagesGroup>("[pcc].[USP_ItemAllocationToWagesGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
