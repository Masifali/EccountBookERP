using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Pos;
using Architecture.Model;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos;

public class WsRmXYItemGroupHeader
{
	public static int Save(Architecture.Model.POS.WsRmXYItemGroupHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Pos.WsRmXYItemGroupHeader().SetData(obj, "Sp_WsRmXYItemGroupHeader_Insert");
			}
			return new Architecture.DAL.Pos.WsRmXYItemGroupHeader().SetData(obj, "Sp_WsRmXYItemGroupHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.POS.WsRmXYItemGroupHeader GetByID(int Id)
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
			new List<Architecture.Model.POS.WsRmXYItemGroupHeader>();
			return new Architecture.DAL.Pos.WsRmXYItemGroupHeader().GetData("Sp_WsRmXYItemGroupHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetXYGroupsBydrmLookupTypeId(ReportsParameters obj)
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
				ParameterName = "@ActivityId",
				Value = obj.ActivityId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GroupTypeId",
				Value = obj.GroupAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetXYGroupsBydrmLookupTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockReceivedNotes>("Sp_WsRmXYItemGroupHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WsRmGetXFreeYDiscountPolicy(ReportsParameters obj)
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
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemIds",
				Value = obj.ItemIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ALL_ITEM_MINQTY_FLAG",
				Value = obj.ActionTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockReceivedNotes>("Sp_WsRmGetXFreeYDiscountPolicy", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetXYItemsRefferedInRetailAndWholeSaleInvoice(ReportsParameters obj)
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
				Value = "GetXYItemsRefferedInRetailAndWholeSaleInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.POS.WsRmStockReceivedNotes>("Sp_WsRmGetXFreeYDiscountPolicy", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
