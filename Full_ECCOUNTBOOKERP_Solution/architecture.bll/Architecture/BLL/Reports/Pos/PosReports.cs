using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Reports.Pos;

public class PosReports
{
	public static DataTable WsRmStockTransferSlipAndRegister(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_WsrmStockTransferNotesSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable WsRmStockReceivingNotesSlipAndRegister(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_WsrmStockReceivedNotesSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PosSaleInvoiceSlip(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Id", obj.Id));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_PosSaleInvoiceRpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PosSaleInvoiceSlip_SubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Id", Id));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_PosSaleInvoiceDetailXGetYFree_SubRpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PosSaleInvoiceRegister(ReportsParameters obj)
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
			if (obj.EntryUser != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
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
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_PosSaleInvoiceRegister", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable WsrmSalesInvoiceSlipAndRegister(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter("@SupplierCustomerId", obj.SupplierCustomerId));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter("@RecordNo", obj.NoOfRecords));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_WsrmSalesInvoiceSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WsrmSalesOrderSlipAndRegister(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter("@SupplierCustomerId", obj.SupplierCustomerId));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_WsrmSalesOrderSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WsrmSaleInvoiceHistorydRegister(ReportsParameters obj)
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
			if (obj.Id > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			return GenericProvider.GetDataTableProc<SaleOrder>("Sp_WsRmSaleInvoice_SlipandRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CurrentItemPricesRpt(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_CurrentItemPricesRpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable WsRmSaleInvoiceReturnSlipAndRegister(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@GlAccountId", obj.AccountId));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_WsRmSaleInvoiceReturnSlipAndRegister", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable WsRmSaleInvoiceSlip_SubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Id", Id));
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_SaleInvoiceDetailXGetYFree_SubRpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FreeItemsDetailRegister(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			if (obj.CompanyId != 0)
			{
				p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter("@DocNoFrom", obj.FromDocNo));
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter("@DocNoTo", obj.ToDocNo));
			}
			if (obj.ReqType != null && obj.ReqType != string.Empty)
			{
				p.Add(new SqlParameter("@InvoiceType", obj.ReqType));
			}
			if (obj.DiscountName != null && obj.DiscountName != string.Empty)
			{
				p.Add(new SqlParameter("@DiscountName", obj.DiscountName));
			}
			if (obj.XGroupId != 0)
			{
				p.Add(new SqlParameter("@XGroupId", obj.XGroupId));
			}
			if (obj.YGroupId != 0)
			{
				p.Add(new SqlParameter("@YGroupId", obj.YGroupId));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@XItemId", obj.ItemId));
			}
			if (obj.YItemId != 0)
			{
				p.Add(new SqlParameter("@YItemId", obj.YItemId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_FreeItemsDetailRegister", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable WareHouseToWareHouseStocTransfer_710(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_WsRmWareHouseToWareHouseStocTransfer_Report", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
