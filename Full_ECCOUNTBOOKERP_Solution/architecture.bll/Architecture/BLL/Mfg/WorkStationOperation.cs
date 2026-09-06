using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Mfg;
using Architecture.Model.Mfg;

namespace Architecture.BLL.Mfg;

public class WorkStationOperation
{
	public static int Save(Architecture.Model.Mfg.WorkStationOperation obj)
	{
		try
		{
			if (obj.WorkStationOperationId == 0)
			{
				return new Architecture.DAL.Mfg.WorkStationOperation().SetData(obj, "[Mfg].[USP_WorkStationOperation_Insert]");
			}
			return new Architecture.DAL.Mfg.WorkStationOperation().SetData(obj, "[Mfg].[USP_WorkStationOperation_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Mfg.WorkStationOperation obj)
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
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WorkStationOperation>("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Mfg.WorkStationOperation> FormHistoryWithList(Architecture.Model.Mfg.WorkStationOperation obj)
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
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.Mfg.WorkStationOperation>();
			return new Architecture.DAL.Mfg.WorkStationOperation().GetAll("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Mfg.WorkStationOperation ReadById(int Id)
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
			new List<Architecture.Model.Mfg.WorkStationOperation>();
			return new Architecture.DAL.Mfg.WorkStationOperation().GetAll("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllWorkStationOpertaion(int OrganizationId, int CompanyId, int BranchesId)
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
				Value = "GetAllWorkStationOpertaion"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WorkStationOperation>("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWorkStationByWorkStationOpertaionId(int Id)
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
				Value = "GetWorkStationByWorkStationOpertaionId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WorkStationOperation>("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOpertaionByWorkStationOpertaionId(int Id)
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
				Value = "GetOpertaionByWorkStationOpertaionId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WorkStationOperation>("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWorkStationOpertaionByWSOId(int OrganizationId, int CompanyId, int Id)
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
			if (Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWorkStationOpertaionByWSOId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WorkStationOperation>("[Mfg].[USP_WorkStationOperation_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
