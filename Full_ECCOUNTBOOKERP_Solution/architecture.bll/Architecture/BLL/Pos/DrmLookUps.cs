using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model.Inventory;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class DrmLookUps
{
	public static int Save(Architecture.Model.POS.drmLookUps obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Pos.drmLookUps().SetData(obj, "Sp_drmLookUps_Insert");
			}
			return new Architecture.DAL.Pos.drmLookUps().SetData(obj, "Sp_drmLookUps_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.drmLookUps GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.POS.drmLookUps>();
			return new Architecture.DAL.Pos.drmLookUps().GetData("Sp_drmLookUpsGetAllMethods", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByLookUpsTypeId(int Id)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetByLookUpsTypeId"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_drmLookUpsGetAllMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDiscountsPolicyByLookupTypeIds()
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDiscountsPolicyByLookupTypeIds"
			});
			return GenericProvider.GetDataTableProc<Item>("Sp_drmLookUpsGetAllMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
