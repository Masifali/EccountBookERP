using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmShiftDayCashInOut
{
	public static int Save(Architecture.Model.POS.WsRmShiftDayCashInOut obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Pos.WsRmShiftDayCashInOut().SetData(obj, "Sp_WsRmShiftDayCashInOut_Insert");
			}
			return new Architecture.DAL.Pos.WsRmShiftDayCashInOut().SetData(obj, "Sp_WsRmShiftDayCashInOut_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmShiftDayCashInOut GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmShiftDayCashInOut>();
			return new Architecture.DAL.Pos.WsRmShiftDayCashInOut().GetData("Sp_WsRmShiftDayCashInOut_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.POS.WsRmShiftDayCashInOut obj)
	{
		try
		{
			int DocNo = 0;
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
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmShiftDayCashInOut>("Sp_WsRmShiftDayCashInOut_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				DocNo = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return DocNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.POS.WsRmShiftDayCashInOut obj)
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
				ParameterName = "@ShiftDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmShiftDayCashInOut>("Sp_WsRmShiftDayCashInOut_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
