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

public class WsRmWareHouseToWareHouseStocTransferHeader
{
	public static int Save(Architecture.Model.POS.WsRmWareHouseToWareHouseStocTransferHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Pos.WsRmWareHouseToWareHouseStocTransferHeader().SetData(obj, "Sp_WsRmWareHouseToWareHouseStocTransferHeader_Insert");
			}
			return new Architecture.DAL.Pos.WsRmWareHouseToWareHouseStocTransferHeader().SetData(obj, "Sp_WsRmWareHouseToWareHouseStocTransferHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocumentNumber(Architecture.Model.POS.WsRmWareHouseToWareHouseStocTransferHeader obj)
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
				Value = "GenerateDocumentNumber"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmRequisitionPo>("Sp_WsRmWareHouseToWareHouseStocTransferHeader_GetAll", p);
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

	public static Architecture.Model.POS.WsRmWareHouseToWareHouseStocTransferHeader GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmWareHouseToWareHouseStocTransferHeader>();
			return new Architecture.DAL.Pos.WsRmWareHouseToWareHouseStocTransferHeader().GetData("Sp_WsRmWareHouseToWareHouseStocTransferHeader_GetAll", p)[0];
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmWareHouseToWareHouseStocTransferHeader>("Sp_WsRmWareHouseToWareHouseStocTransferHeader_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
