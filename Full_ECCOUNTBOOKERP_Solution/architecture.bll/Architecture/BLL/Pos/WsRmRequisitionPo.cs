using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmRequisitionPo
{
	public static int Save(Architecture.Model.POS.WsRmRequisitionPo obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Pos.WsRmRequisitionPo().SetData(obj, "Sp_WsRmRequisitionPo_Insert");
			}
			return new Architecture.DAL.Pos.WsRmRequisitionPo().SetData(obj, "Sp_WsRmRequisitionPo_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int RequisitionApproval(Architecture.Model.POS.WsRmRequisitionPo obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id > 0)
			{
				return new Architecture.DAL.Pos.WsRmRequisitionPo().SetDataForApproval(obj, "Sp_WsRmRequisitionPo_Approve");
			}
			return 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocCode(Architecture.Model.POS.WsRmRequisitionPo obj)
	{
		DataTable dt = null;
		try
		{
			int Code = 0;
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
				Value = "GenerateDocNoByDocumentTypeId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmRequisitionPo>("Sp_WsRmRequisitionPo_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmRequisitionPo GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmRequisitionPo>();
			return new Architecture.DAL.Pos.WsRmRequisitionPo().GetData("Sp_WsRmRequisitionPo_GetAllMethod", p)[0];
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
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmRequisitionPo>("Sp_WsRmRequisitionPo_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUnApprovedRequisitionOrders(ReportsParameters obj)
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
				Value = "GetUnApprovedRequisitionOrders"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmRequisitionPo>("Sp_WsRmRequisitionPo_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
