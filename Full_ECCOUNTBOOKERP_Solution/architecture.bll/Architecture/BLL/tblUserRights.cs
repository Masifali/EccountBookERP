using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Authentication;

namespace Architecture.BLL;

public class tblUserRights
{
	public static int Insert(Architecture.Model.tblUserRights obj)
	{
		try
		{
			return new Architecture.DAL.tblUserRights().SetData(obj, "Sp_tblUserRights_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SpecialRightsUpdate(Architecture.Model.tblUserRights obj)
	{
		try
		{
			return new Architecture.DAL.tblUserRights().RightsUpdateNew(obj, "Sp_tblUserRights_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.tblUserRights> GetByUserId(Architecture.Model.tblUserRights obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenName",
				Value = obj.ScreenName
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RightName",
				Value = obj.RightName
			});
			if (obj.CompanyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
			}
			else
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = clsGlobalVariables.UserAccount.CompanyId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetByUserId"
			});
			return new Architecture.DAL.tblUserRights().GetData("Sp_tblUserRights_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateRights(Architecture.Model.tblUserRights obj)
	{
		try
		{
			foreach (Architecture.Model.tblUserRights item in obj.TblUserRightslst)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = item.UserId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = item.ScreenID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@RightsID",
					Value = item.RightsID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@RightName",
					Value = item.RightName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Value",
					Value = item.Value
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = item.AppId
				});
				GenericProvider.GetDataTableProc<Architecture.Model.tblUserRights>("usp_UserRightsUpdate", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.tblUserRights> GetViewByUserId(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.EntryUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			if (obj.AppId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			return new Architecture.DAL.tblUserRights().GetData("Proc_GetUserRightsForView_UserId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.tblUserRights> GetbyId(int Id, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetbyId"
			});
			return new Architecture.DAL.tblUserRights().GetData("Sp_tblUserRights_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllModules()
	{
		try
		{
			return GenericProvider.GetDataTableProc<Architecture.Model.tblUserRights>("Sp_AppModules_ReadAll");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.tblUserRights> GetUserRightByUserIdAndModuleName(Architecture.Model.tblUserRights obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RightName",
				Value = obj.RightName
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ModuleName",
				Value = obj.ModuleDescription
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetUserRightByUserIdAndModuleName"
			});
			return new Architecture.DAL.tblUserRights().GetData("Sp_tblUserRights_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool CheckIfScreenExistsInCompany(int CompanyId, string ScreenName)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenName",
				Value = ScreenName
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "CheckIfScreenExistsInCompany"
			});
			return Conversion.ToBool(GenericProvider.GetDataTableProc<RoleRights>("Sp_CompanyRights_GetAllMethod", p).Rows[0][0]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.tblUserRights> GetUserRightsForViewbyUserId(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.EntryUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			if (obj.AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			if (obj.AppModuleId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppModuleId",
					Value = obj.AppModuleId
				});
			}
			return new Architecture.DAL.tblUserRights().GetData("USP_GetUserRightsForViewbyUserId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetScreensForAuditReport()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.tblUserRights>("[dbo].[USP_GetScreensForAuditReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetModulesAgainstCompanyId(int CompanyId, int UserId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = UserId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("[dbo].[USP_GetAppModulesByCompany]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UserScreenRights_Special(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("[dbo].[USP_UserScreenRights_Special]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetModulesByCompany(int CompanyId, int ModuleTypeId = 0, int UserId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (ModuleTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModuleTypeId",
					Value = ModuleTypeId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("usp_getModulesByCompanyId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getScreensByModuleId(int CompanyId, int ModuleId = 0, int ModuleTypeId = 0, int UserId = 0)
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
			if (ModuleTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModuleTypeId",
					Value = ModuleTypeId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("usp_getScreensByModuleId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getScreenRightsByModuleAndUserId(int CompanyId, int ModuleId = 0, int ScreenId = 0, int UserId = 0, int AppId = 0)
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
			if (ScreenId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = ScreenId
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
			if (AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = AppId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("usp_getScreenRightsByModuleAndUserId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getCustomerScreens(int ScreenTypeId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (ScreenTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenTypeId",
					Value = ScreenTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.CompanyRights>("usp_getCustomerScreens", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
