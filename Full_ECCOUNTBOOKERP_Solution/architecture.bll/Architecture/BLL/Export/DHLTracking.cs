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

public class DHLTracking
{
	public static long Save(Architecture.Model.Export.DHLTracking obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			if (obj.DHLTrackingId == 0L)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.Export.DHLTracking().SetData(obj, "USP_DHLTracking_InsertAndUpdate");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Export.DHLTracking().SetData(obj, "USP_DHLTracking_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.DHLTracking GetByID(long Id)
	{
		try
		{
			new List<Architecture.Model.Export.DHLTracking>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DHLTrackingId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			return new Architecture.DAL.Export.DHLTracking().GetAll("USP_DHLTracking_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet FormHistory(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUserId",
					Value = obj.UserId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DisPatchedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DispatchedFromDate",
					Value = obj.DisPatchedDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DisPatchedToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DispatchedToDate",
					Value = obj.DisPatchedToDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ETADate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinalETAFromDate",
					Value = obj.ETADate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ETAToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinalETAToDate",
					Value = obj.ETAToDate
				});
			}
			if (obj.AWBBillNo != null && obj.AWBBillNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AWBBillNumber",
					Value = obj.AWBBillNo
				});
			}
			if (obj.StatusId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CurrentStatusId",
					Value = obj.StatusId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataSetProc<Architecture.Model.Export.DHLTracking>("USP_DHLTracking_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SlipAndRegister(ReportsParameters obj)
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
			if (obj.EximInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@",
					Value = obj.EximInvoiceId
				});
			}
			if (obj.ExImEFormRegistrationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@",
					Value = obj.ExImEFormRegistrationId
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImBillOfLading>("USP_DHLTracking_SlipAndRegister", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetDataForDropDown(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImExportShipingLineBooking>("USP_GetDataForDropDownFromDHLTracking", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void DeleteById(long Id, int EntryUserId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[USP_DHLTracking_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@DHLTrackingId", Id);
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
