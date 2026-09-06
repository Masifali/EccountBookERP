using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.FixedAssets;
using Architecture.Model;
using Architecture.Model.FixedAssets;

namespace Architecture.BLL.FixedAssets;

public class FixedAssetsRegister
{
	public static int Save(Architecture.Model.FixedAssets.FixedAssetsRegister obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.FixedAssets.FixedAssetsRegister().SetData(obj, "Sp_FixedAssetsRegister_Insert");
			}
			return new Architecture.DAL.FixedAssets.FixedAssetsRegister().SetData(obj, "Sp_FixedAssetsRegister_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHistory(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryFromDate",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryToDate",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyFromDate",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyToDate",
					Value = obj.ModifyDateTo
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CategoryId",
					Value = obj.ItemCategoryId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FixedAssets.FixedAssetsRegister>("Sp_FixedAssetsRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable HistoryForm(Architecture.Model.FixedAssets.FixedAssetsRegister obj)
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
			if (obj.Brand != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Brand",
					Value = obj.Brand
				});
			}
			if (obj.FixedAssetsCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FixedAssetsCategoryId",
					Value = obj.FixedAssetsCategoryId
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.AssetGLAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AssetGLAccountId",
					Value = obj.AssetGLAccountId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.FixedAssets.FixedAssetsRegister>("Sp_FixedAssetsRegister_History", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Architecture.Model.FixedAssets.FixedAssetsRegister GetById(int Id)
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
			return new Architecture.DAL.FixedAssets.FixedAssetsRegister().GetAll("Sp_FixedAssetsRegister_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.FixedAssets.FixedAssetsRegister obj)
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FixedAssets.FixedAssetsRegister>("Sp_FixedAssetsRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.FixedAssets.FixedAssetsRegister> GetAllList(Architecture.Model.FixedAssets.FixedAssetsRegister obj)
	{
		try
		{
			List<Architecture.Model.FixedAssets.FixedAssetsRegister> lst = new List<Architecture.Model.FixedAssets.FixedAssetsRegister>();
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
				Value = "ReadAll"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.FixedAssets.FixedAssetsRegister>("Sp_FixedAssetsRegister_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					Architecture.Model.FixedAssets.FixedAssetsRegister fxd = new Architecture.Model.FixedAssets.FixedAssetsRegister();
					fxd.Id = Conversion.ToInt(dt.Rows[i]["Id"].ToString());
					fxd.AssetName = Conversion.ToString(dt.Rows[i]["AssetName"]);
					lst.Add(fxd);
				}
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
