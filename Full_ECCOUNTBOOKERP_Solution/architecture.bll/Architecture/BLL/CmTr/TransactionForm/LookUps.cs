using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.CmTr.TransactionForm;
using Architecture.DAL.Common;
using Architecture.Model.CmTr.TransactionForm;

namespace Architecture.BLL.CmTr.TransactionForm;

public class LookUps
{
	public static int Save_Lookups(Architecture.Model.CmTr.TransactionForm.LookUps obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.CmTr.TransactionForm.LookUps().SetData(obj, "[Cmtr].[USP_LookUps_Insert]");
			}
			return new Architecture.DAL.CmTr.TransactionForm.LookUps().SetData(obj, "[Cmtr].[USP_LookUps_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.CmTr.TransactionForm.LookUps ReadById_LookUps(int Id)
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
				Value = "ReadById_LookUps"
			});
			new List<Architecture.Model.CmTr.TransactionForm.LookUps>();
			return new Architecture.DAL.CmTr.TransactionForm.LookUps().GetAll("[CmTr].[USP_LookUps_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory_LookUps()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory_LookUps"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.LookUps>("[CmTr].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int Save_LookUpType(Architecture.Model.CmTr.TransactionForm.LookUpType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.CmTr.TransactionForm.LookUpType().SetData(obj, "[Cmtr].[USP_LookUpType_Insert]");
			}
			return new Architecture.DAL.CmTr.TransactionForm.LookUpType().SetData(obj, "[Cmtr].[USP_LookUpType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.CmTr.TransactionForm.LookUpType ReadById_LookUpType(int Id)
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
				Value = "ReadById_LookUpType"
			});
			new List<Architecture.Model.CmTr.TransactionForm.LookUpType>();
			return new Architecture.DAL.CmTr.TransactionForm.LookUpType().GetAll("[CmTr].[USP_LookUps_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory_LookUpType()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory_LookUpType"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.LookUpType>("[CmTr].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable RevExpAcAllocatedToProductAndDiscTypeForComboService(int OrganizationId, int CompanyId, int ItemId, int SupplierCustomerId)
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
				ParameterName = "@ItemId",
				Value = ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "RevExpAcAllocatedToProductAndDiscTypeForComboService"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.LookUpType>("[CmTr].[USP_LookUps_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
