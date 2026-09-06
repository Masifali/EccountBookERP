using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class CompanyRights
{
	public static int Save(Architecture.Model.CompanyRights obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.CompanyRights().SetData(obj, "Sp_CompanyRights_Insert");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaveAdminRights(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUserId",
				Value = obj.EntryUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SelectedScreedIds",
				Value = obj.SelectedScreedIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ScreenRightstoAdmin"
			});
			new DataTable();
			GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("Sp_CompanyRights_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllScreens(Architecture.Model.CompanyRights obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.ScreenId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadForCompanyAllocation"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("Sp_ScreenDefinition_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetScreenByCompanyId(int Id = 0, int CompanyId = 0, int UserId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (CompanyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = CompanyId
				});
			}
			if (Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			if (UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = UserId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadbyCompanyId"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("Sp_CompanyRights_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByCompanyId(Architecture.Model.CompanyRights obj)
	{
		try
		{
			new Architecture.DAL.CompanyRights().DeleteByCompanyId(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UserWiseAllocatedOrUnAllocatedScreens(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ModuleId",
				Value = obj.AppModuleId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (obj.AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("[dbo].[USP_UserWiseAllocatedOrUnAllocatedScreens]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllocateScreensToUser(int CompanyId, int UserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("[dbo].[USP_GetAllocateScreensToUser]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByUserId(CompanyRightsInsertAndDelete obj)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (CompanyRightsInsertAndDelete item in obj.CompanyRightsInsertAndDeletelist)
				{
					new List<SqlParameter>();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "Sp_CompanyRights_GetAllMethod";
					sqlCommand.Parameters.AddWithValue("OrganizationId", item.OrganizationId);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
					sqlCommand.Parameters.AddWithValue("Id", item.ScreenId);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("AppId", item.AppId);
					sqlCommand.Parameters.AddWithValue("EntryUserId", item.EntryUserId);
					sqlCommand.Parameters.AddWithValue("Activity", "DeleteByCompanyIdAndUserId");
					sqlCommand.ExecuteNonQuery();
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				throw ex;
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	public static DataTable GetScreensAllocatedorNotToCompany(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ModuleId",
				Value = obj.AppModuleId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ActionId",
				Value = obj.ActionId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("Usp_GetScreensAllocatedorNotToCompany", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CompanyRights_AllocatedData(int CompanyId, int ModuleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (ModuleId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModuleId",
					Value = ModuleId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("USP_CompanyRights_AllocatedData", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CompanyRights_UnAllocatedData(int CompanyId, int ModuleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (ModuleId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModuleId",
					Value = ModuleId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("USP_CompanyRights_UnAllocatedData", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
