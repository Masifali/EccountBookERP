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

public class InvLookUp
{
	public static int Save(Architecture.Model.Inventory.InvLookUp obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvLookUp().SetData(obj, "Proc_InvLookup_Insert");
			}
			return new Architecture.DAL.Inventory.InvLookUp().SetData(obj, "Proc_InvLookup_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvLookUp GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.Inventory.InvLookUp>();
			return new Architecture.DAL.Inventory.InvLookUp().GetAll("Proc_InvLookup_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(ReportsParameters obj)
	{
		try
		{
			int Code = 0;
			DataTable dt = null;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyID",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@InvLookupTypeId",
				Value = obj.Id
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvLookUp>("Proc_InvLookUpType_GenerateCodeByLookUpType", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["Code"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.InvLookUp> GetLookUpByLookUpTypeId(Architecture.Model.Inventory.InvLookUp obj)
	{
		DataTable dt = null;
		try
		{
			List<Architecture.Model.Inventory.InvLookUp> lst = new List<Architecture.Model.Inventory.InvLookUp>();
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
				ParameterName = "@InvLookupTypeId",
				Value = obj.InvLookupTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByInvlookTypeId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvLookUp>("Sp_InvLookup_GetAllMethod", p);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				Architecture.Model.Inventory.InvLookUp up = new Architecture.Model.Inventory.InvLookUp();
				up.LookupName = dt.Rows[i]["LookupName"].ToString();
				up.Id = Conversion.ToInt(dt.Rows[i]["Id"]);
				lst.Add(up);
			}
			return lst;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLookupsByTypeIdDt(Architecture.Model.Inventory.InvLookUp obj)
	{
		try
		{
			new List<Architecture.Model.Inventory.InvLookUp>();
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
				ParameterName = "@InvLookupTypeId",
				Value = obj.InvLookupTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByInvlookTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvLookUp>("Sp_InvLookup_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Inventory.InvLookUp obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@organizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvLookUp>("Sp_InvLookup_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLookUpsERP(int lookUpTypeId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@LookUpTypeId",
				Value = lookUpTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvLookUp>("USP_GetLookUpsERP", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
