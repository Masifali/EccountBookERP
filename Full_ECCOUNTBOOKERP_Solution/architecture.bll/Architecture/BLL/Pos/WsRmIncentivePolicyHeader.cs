using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model.Accounts;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmIncentivePolicyHeader
{
	public static int Save(Architecture.Model.POS.WsRmIncentivePolicyHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Pos.WsRmIncentivePolicyHeader().SetData(obj, "Sp_WsRmIncentivePolicyHeader_Insert");
			}
			return new Architecture.DAL.Pos.WsRmIncentivePolicyHeader().SetData(obj, "Sp_WsRmIncentivePolicyHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmIncentivePolicyHeader GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmIncentivePolicyHeader>();
			return new Architecture.DAL.Pos.WsRmIncentivePolicyHeader().GetData("Sp_WsRmIncentivePolicyHeader_GetAllMetod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WsrmIncentivePolicyFormHistory(Architecture.Model.POS.WsRmIncentivePolicyHeader obj)
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<COAAllocation>("Sp_WsRmIncentivePolicyHeader_GetAllMetod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryOnlyHeader(Architecture.Model.POS.WsRmIncentivePolicyHeader obj)
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
				Value = "FormHistoryOnlyHeader"
			});
			return GenericProvider.GetDataTableProc<COAAllocation>("Sp_WsRmIncentivePolicyHeader_GetAllMetod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CalculateIncentivesForCreditNotes(Architecture.Model.POS.WsRmIncentivePolicyHeader obj)
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
				ParameterName = "@FromDate",
				Value = obj.SchemeDateFrom
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.SchemeDateTo
			});
			return GenericProvider.GetDataTableProc<COAAllocation>("USP_CalculateIncentivesForCreditNotes", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
