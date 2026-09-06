using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImExportShipingLineBooking
{
	public static int Save(Architecture.Model.Export.ExImExportShipingLineBooking obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImExportShipingLineBooking().SetDate(obj, "Sp_ExImExportShipingLineBooking_Insert");
			}
			return new Architecture.DAL.Export.ExImExportShipingLineBooking().SetDate(obj, "Sp_ExImExportShipingLineBooking_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImExportShipingLineBooking GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImExportShipingLineBooking>();
			return new Architecture.DAL.Export.ExImExportShipingLineBooking().GetData("Sp_ExImExportShipingLineBooking_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(ReportsParameters obj)
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoTo",
					Value = obj.ToDocNo
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ShippingAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShippingAgentId",
					Value = obj.ShippingAgentId
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
			if (obj.ForwarderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransporterId",
					Value = obj.ForwarderId
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
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CroNo",
					Value = obj.Status
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("Sp_ExImExportShipingLineBooking_GetAllMethod", p);
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ShippingAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShippingAgentId",
					Value = obj.ShippingAgentId
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
			if (obj.ForwarderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransporterId",
					Value = obj.ForwarderId
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
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("[dbo].[USP_ExImExportShipingLineBooking_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByInvoiceId(Architecture.Model.Export.ExImExportShipingLineBooking obj)
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
				ParameterName = "@ExImInvoiceId",
				Value = obj.ExImInvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByInvoiceId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("Sp_ExImExportShipingLineBooking_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CustomerExistInExportInvoices(ReportsParameters obj)
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
				Value = "GetCustomerExistInExportInvoices"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PrintSlipandRegister(ReportsParameters obj)
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
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExIminvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetDataForDropDownFromShippingBookingInfo(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("USP_GetDataForDropDownFromShippingBookingInfo", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static int GetBookingCROIdByInvoiceId(int OrganizationId, int CompanyId, int InvoiceId)
	{
		try
		{
			int Id = 0;
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
				ParameterName = "@Id",
				Value = InvoiceId
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("usp_getBookingCROIdByInvoiceId", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				Id = Conversion.ToInt(dt.Rows[0][0]);
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
