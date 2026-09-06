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

public class ItemDiscounting
{
	public static int Save(Architecture.Model.POS.ItemDiscounting obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Pos.ItemDiscounting().SetData(obj, "Sp_ItemDiscountingSchedule_Insert");
			}
			return new Architecture.DAL.Pos.ItemDiscounting().SetData(obj, "Sp_ItemDiscountingSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GenerateDiscountAgainstItemIdAndRate(ReportsParameters obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemRate",
				Value = obj.ItemRate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemQty",
				Value = obj.ItemQty
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MasterDiscountTypeId",
				Value = obj.MasterDiscountTypeId
			});
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.CurrentDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CurrentDate",
					Value = obj.CurrentDate
				});
			}
			return GenericProvider.GetDataTableProc<PosSaleInvoiceDetail>("GenerateDiscountAgainstItemIdAndRate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DiscountingHistory(Architecture.Model.POS.ItemDiscounting obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.DiscountStatus != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.DiscountStatus
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DiscountingHistory"
			});
			return GenericProvider.GetDataTableProc<PosSaleInvoiceDetail>("Sp_ItemDiscountingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateDiscountStatus(Architecture.Model.POS.ItemDiscounting obj)
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
				ParameterName = "@Status",
				Value = obj.DiscountStatus
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "UpdateDiscountStatus"
			});
			GenericProvider.GetDataTableProc<PosSaleInvoiceDetail>("Sp_ItemDiscountingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet GenerateDisacountForWholeSale(ReportsParameters obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemRate",
				Value = obj.ItemRate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemQty",
				Value = obj.ItemQty
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PaymenetTermId",
				Value = obj.PaymenetTermId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RequiredForFlag",
				Value = obj.ReqType
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			return GenericProvider.GetDataSetProc<PosSaleInvoiceDetail>("SP_GenerateDisacountForWholeSale", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDiscountNames(Architecture.Model.POS.ItemDiscounting obj)
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
				Value = "GetDiscountNames"
			});
			return GenericProvider.GetDataTableProc<PosSaleInvoiceDetail>("Sp_ItemDiscountingSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetRegularItemDiscount(ReportsParameters obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PriceTypeId",
				Value = obj.PriceTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MasterDiscountTypeId",
				Value = obj.MasterDiscountTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CurrentDate",
				Value = obj.CurrentDate
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			return GenericProvider.GetDataTableProc<PosSaleInvoiceDetail>("USP_GetRegularItemDiscount", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
