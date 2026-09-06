using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class SreenRightsUserWise
{
	public static int Save(Architecture.Model.SreenRightsUserWise obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.SreenRightsUserWise().SetData(obj, "[dbo].[USP_ScreenRightsUserWise_Insert]");
			}
			return new Architecture.DAL.SreenRightsUserWise().SetData(obj, "[dbo].[USP_ScreenRightsUserWise_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.SreenRightsUserWise obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
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
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.ScreenId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenId",
					Value = obj.ScreenId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SreenRightsUserWise>("dbo.[USP_ScreenRightsUserWise_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SpecialRightsByScreenIdAndUserId(Architecture.Model.SreenRightsUserWise obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SpecialRightsByScreenIdAndUserId"
			});
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScreenId",
				Value = obj.ScreenId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SreenRightsUserWise>("dbo.[USP_ScreenRightsUserWise_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.SreenRightsUserWise GetByID(int Id)
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
			new List<Architecture.Model.SreenRightsUserWise>();
			return new Architecture.DAL.SreenRightsUserWise().GetAll("dbo.[USP_ScreenRightsUserWise_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllSpecialRights(int ScreenId, string ScreenName = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (!string.IsNullOrEmpty(ScreenName))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = ScreenName
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllSpecialRights"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SreenRightsUserWise>("dbo.[USP_ScreenRightsUserWise_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
