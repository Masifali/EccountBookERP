using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.SDT;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.SDT;

namespace Architecture.BLL.SDT;

public class ShipmentDocumentSchedule
{
	public static int Save(Architecture.Model.SDT.ShipmentDocumentSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.SDT.ShipmentDocumentSchedule().SetData(obj, "[sdt].[USP_ShipmentDocumentSchedule_Insert]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateDocStatusAndReadyDate(Architecture.Model.SDT.ShipmentDocumentSchedule obj)
	{
		if (obj?.ShipmentDocumentScheduleList == null || obj.ShipmentDocumentScheduleList.Count == 0)
		{
			return;
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.SDT.ShipmentDocumentSchedule item in obj.ShipmentDocumentScheduleList)
			{
				if (item.shipmentDocumentScheduleId > 0)
				{
					using SqlCommand sqlCommand = new SqlCommand("[sdt].[USP_ShipmentDocumentSchedule_GetAllMethod]", sqlcon, sqlTrn);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.AddWithValue("@Activity", "UpdateDocStatusAndReadyDate");
					sqlCommand.Parameters.AddWithValue("@Id", item.shipmentDocumentScheduleId);
					sqlCommand.Parameters.AddWithValue("@DocStatusId", item.DocStatus);
					sqlCommand.Parameters.AddWithValue("@ReadyDate", item.ReadyDate);
					sqlCommand.Parameters.AddWithValue("@EntryUserId", item.ModifyUserId);
					sqlCommand.ExecuteNonQuery();
				}
			}
			sqlTrn.Commit();
		}
		catch (Exception)
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static void AutoSaveByRefDocumentType(int RefDocumentTypeId, int RefDocId, int CustomGroupId, string CODIds)
	{
		try
		{
			new Architecture.DAL.SDT.ShipmentDocumentSchedule().AutoSaveByRefDocumentType(RefDocumentTypeId, RefDocId, CustomGroupId, CODIds);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateRefDocument(int RefDocumentTypeId, int RefDocId, int RefDocCustomGroupId, bool RefDocLockStatus)
	{
		try
		{
			new Architecture.DAL.SDT.ShipmentDocumentSchedule().UpdateRefDocument(RefDocumentTypeId, RefDocId, RefDocCustomGroupId, RefDocLockStatus);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByRefDocument(int RefDocId, int RefDocumentTypeId, int CustomGroupId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocId",
				Value = RefDocId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByRefDocument"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ShipmentDocumentSchedule>("[sdt].[USP_ShipmentDocumentSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetChartOfDocumentForShipmentScheduleByRefDocument(int RefDocId, int RefDocumentTypeId, int CustomGroupId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocId",
				Value = RefDocId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupId",
				Value = CustomGroupId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ShipmentDocumentSchedule>("[sdt].[USP_GetChartOfDocumentForShipmentScheduleByRefDocument]", p);
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
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
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedFromDate",
					Value = obj.ApprovedDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
				});
			}
			if (obj.OtherDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@chartOfDocumentId",
					Value = obj.OtherDocumentTypeId
				});
			}
			if (obj.RefRefDocIdNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocId",
					Value = obj.RefRefDocIdNo
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ShipmentDocumentSchedule>("[sdt].[USP_ShipmentDocumentSchedule_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportDocumentTrackingReportGroupWiseI(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueFrom",
					Value = obj.DueDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueTo",
					Value = obj.DueDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ReadyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDateFrom",
					Value = obj.ReadyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ReadyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDateTo",
					Value = obj.ReadyDateTo
				});
			}
			if (obj.DocumentProviderId != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentProviderId",
					Value = obj.DocumentProviderId
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@chartOfDocumentId",
					Value = obj.RefDocumentTypeId
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
			if (obj.SalesPersonId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SalesPersonId",
					Value = obj.SalesPersonId
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
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDocsOnly",
					Value = obj.SkipZero
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingInvoice",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ShipmentDocumentSchedule>("SDT.USP_ExportDocumentTrackingReportI", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportDocumentTrackingReportGroupWiseII(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueFrom",
					Value = obj.DueDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueTo",
					Value = obj.DueDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ReadyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDateFrom",
					Value = obj.ReadyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ReadyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDateTo",
					Value = obj.ReadyDateTo
				});
			}
			if (obj.DocumentProviderId != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentProviderId",
					Value = obj.DocumentProviderId
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@chartOfDocumentId",
					Value = obj.RefDocumentTypeId
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
			if (obj.SalesPersonId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SalesPersonId",
					Value = obj.SalesPersonId
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
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDocsOnly",
					Value = obj.SkipZero
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingInvoice",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ShipmentDocumentSchedule>("SDT.USP_ExportDocumentTrackingReportII", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportDocumentTrackingReport(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueFrom",
					Value = obj.DueDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DueTo",
					Value = obj.DueDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ReadyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDateFrom",
					Value = obj.ReadyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ReadyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDateTo",
					Value = obj.ReadyDateTo
				});
			}
			if (obj.DocumentProviderId != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentProviderId",
					Value = obj.DocumentProviderId
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@chartOfDocumentId",
					Value = obj.RefDocumentTypeId
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
			if (obj.SalesPersonId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SalesPersonId",
					Value = obj.SalesPersonId
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
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReadyDocsOnly",
					Value = obj.SkipZero
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingInvoice",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.SDT.ShipmentDocumentSchedule>("SDT.USP_ExportDocumentTrackingReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByIds(string Ids, string CODIds, int RefDocumentTypeId, int EntryUserId)
	{
		try
		{
			new Architecture.DAL.SDT.ShipmentDocumentSchedule().DeleteData(Ids, CODIds, RefDocumentTypeId, EntryUserId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromShipmentDocumentSchedule(int OrganizationId, int CompanyId, string Activity = null)
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
			if (Activity != string.Empty || Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[sdt].[USP_DropDownFillFromShipmentDocumentSchedule]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
