using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Import.Transactions;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Import.Transaction;

namespace Architecture.BLL.Import.Transaction;

public class ShipmentBooking
{
	public static long Save(Architecture.Model.Import.Transaction.ShipmentBooking obj)
	{
		try
		{
			obj.appActionId = ((obj.ShipmentBookingId <= 0) ? 1 : 2);
			string Activity = ((obj.ShipmentBookingId > 0) ? "UPDATE" : "INSERT");
			return new Architecture.DAL.Import.Transactions.ShipmentBooking().SetData(obj, "[ImEx].[usp_Set_ShipmentBooking]", Activity);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Import.Transaction.ShipmentBooking GetByID(long Id)
	{
		try
		{
			List<Architecture.Model.Import.Transaction.ShipmentBooking> lst = new List<Architecture.Model.Import.Transaction.ShipmentBooking>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ShipmentBookingId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ID"
			});
			lst = new Architecture.DAL.Import.Transactions.ShipmentBooking().GetData("[ImEx].[usp_Get_ShipmentBooking]", p);
			return (lst != null && lst.Count > 0) ? lst[0] : null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable History(ReportsParameters obj)
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShipmentBookingId",
					Value = obj.RecId
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
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryFromDate",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryToDate",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyFromDate",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyToDate",
					Value = obj.ModifyDateTo
				});
			}
			if (obj.ShippingLineId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShippingLineId",
					Value = obj.ShippingLineId
				});
			}
			if (obj.LoadingPortId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LoadingPortId",
					Value = obj.LoadingPortId
				});
			}
			if (obj.DestinationPortId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@dischargePortId",
					Value = obj.DestinationPortId
				});
			}
			if (obj.InvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@invoiceMasterId",
					Value = obj.InvoiceId
				});
			}
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@createdUserId",
					Value = obj.EntryUser
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SEARCH"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.Transaction.ShipmentBooking>("[ImEx].[usp_Get_ShipmentBooking]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoForBookingInfo(int OrganizationId, int CompanyId, int DocumentTypeId, int FinancialYearId, long RecId = 0L)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			if (RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.invoiceMaster>("[ImEx].[usp_Get_InvoiceMasterNoForShipmentBooking]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromShipmentBookingInfo(ReportsParameters obj)
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.Transaction.ShipmentBooking>("[ImEx].[USP_GetDataForDropDownFrom_ShipmentBooking]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ShipmentBookingReport(ReportsParameters obj)
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
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShipmentBookingId",
					Value = obj.RecId
				});
			}
			if (obj.DestinationPortId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DestinationPortId",
					Value = obj.DestinationPortId
				});
			}
			if (obj.ShippingLineId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@shippingLIneId",
					Value = obj.ShippingLineId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<ExImExportShipingLineBooking>("[Imex].[USP_Get_ShipmentBookingReport]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
