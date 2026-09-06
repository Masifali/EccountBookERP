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

public class InvOrderCategory
{
	public static int Save(Architecture.Model.Inventory.InvOrderCategory obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvOrderCategory().SetData(obj, "Sp_InvOrderCategory_Insert");
			}
			return new Architecture.DAL.Inventory.InvOrderCategory().SetData(obj, "Sp_InvOrderCategory_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvOrderCategory>("Sp_InvOrderCategory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetForStoreAndPm()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetForStoreAndPM"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvOrderCategory>("Sp_InvOrderCategory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvOrderCategory GetByID(int Id)
	{
		try
		{
			new List<SqlParameter>
			{
				new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				},
				new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "GetById"
				}
			};
			new List<Architecture.Model.Inventory.InvOrderCategory>();
			return new Architecture.DAL.Inventory.InvOrderCategory().GetById("Sp_InvOrderCategory_GetAllMethod", Id)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateOrderCategoryCodebyId(ReportsParameters obj)
	{
		try
		{
			int Code = 0;
			DataTable dt = new DataTable();
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
				ParameterName = "@OrderCatagoryId",
				Value = obj.Id
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateOrderCategoryCodeById"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvOrderCategory>("Sp_InvOrderCategory_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["CatagorySrNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateSrNoByIdForFed(ReportsParameters obj)
	{
		try
		{
			int Code = 0;
			DataTable dt = new DataTable();
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
				ParameterName = "@OrderCatagoryId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateSrNoByIdForFed"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvOrderCategory>("Sp_InvOrderCategory_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["CatagorySrNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateSaleOrderCategoryCodebyId(Architecture.Model.Inventory.InvOrderCategory obj)
	{
		try
		{
			int Code = 0;
			DataTable dt = new DataTable();
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
				ParameterName = "@OrderCatagoryId",
				Value = obj.Id
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateSaleOrderCategoryCodeById"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvOrderCategory>("Sp_InvOrderCategory_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["CatagorySrNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
