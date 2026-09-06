using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.AssetSchema;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.AssetSchema;

namespace Architecture.BLL.AssetSchema;

public class AssetRegister
{
	public static int Save(Architecture.Model.AssetSchema.AssetRegister obj)
	{
		try
		{
			return new Architecture.DAL.AssetSchema.AssetRegister().SetData(obj, "[Asset].[USP_AssetRegister_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.AssetSchema.AssetRegister GetById(long Id)
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
			return new Architecture.DAL.AssetSchema.AssetRegister().GetAll("[Asset].[USP_AssetRegister_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.AssetRegister>("[Asset].[USP_AssetRegister_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAssetDepartment(int OrganizationId, int CompanyId)
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
				ParameterName = "@Activity",
				Value = "GetAssetDepartment"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.AssetRegister>("Asset.USP_AssetRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GenerateSerailNoByCategoryId(int OrganizationId, int CompanyId, int CategoryId)
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
				ParameterName = "@CategoryId",
				Value = CategoryId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateSerailNoByCategoryId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.Category>("[asset].[USP_AssetRegister_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAssetCondition()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAssetCondition"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.AssetRegister>("Asset.USP_AssetRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAssetStatus()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAssetStatus"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.AssetRegister>("Asset.USP_AssetRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAllAssetFromAssetRegister(int OrganizationId, int CompanyId, int CategoryId = 0)
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
			if (CategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CategoryId",
					Value = CategoryId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllAssetFromAssetRegister"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AssetSchema.Category>("[asset].[USP_AssetRegister_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
