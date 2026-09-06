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

public class ExportContractSchedule
{
	public static int Save(Architecture.Model.Export.ExportContractSchedule obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			_ = obj.Id;
			return new Architecture.DAL.Export.ExportContractSchedule().SetData(obj, "USP_ExportContractSchedule_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Export.ExportContractSchedule> GetByID(int Id)
	{
		try
		{
			new List<Architecture.Model.Export.ExportContractSchedule>();
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
			return new Architecture.DAL.Export.ExportContractSchedule().GetData("USP_ExportContractSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateRecordIdentityId(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GenerateRecordIdentityId"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportContractSchedule>("USP_ExportContractSchedule_GetAllMethod", p).Rows[0][0]);
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
			new DataTable();
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
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUserId",
					Value = obj.UserId
				});
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromLoadingDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToLoadingDate",
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
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("USP_ExportContractSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetDataForDropDownFromSchedule(int OrganizationId, int CompanyId, string Activity = null, string DocumentTypeIds = null)
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
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			if (DocumentTypeIds != null && DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = DocumentTypeIds
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportContractSchedule>("USP_GetDataForDropDownFromExportContractSchedule", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet ExportContractScheduleSlipAndRegister(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			if (obj.RecordIdentityId != 0)
			{
				p.Add(new SqlParameter("@RecordIdentityId", obj.RecordIdentityId));
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter("@SupplierCustomerId", obj.SupplierCustomerId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter("@FromLoadingDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter("@ToLoadingDate", obj.ToDate));
			}
			if (!string.IsNullOrEmpty(obj.Ids))
			{
				p.Add(new SqlParameter("@Ids", obj.Ids));
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter("@FromDocNo", obj.FromDocNo));
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter("@ToDocNo", obj.ToDocNo));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter("@ItemCategoryId", obj.ItemCategoryId));
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter("@ItemTypeId", obj.ItemTypeId));
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter("@IsApproved", obj.IsApproved));
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Export.ExportContractSchedule>("USP_ExportContractSchedule_SlipAndRegisterNew", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetPendingContractsForSchedule(ReportsParameters obj)
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportContractSchedule>("USP_ExImLcOrder_PendingForExportContractSchedule", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByIdentityId(int Id, int EntryUserId)
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
			sqlCommand.CommandText = "[dbo].[USP_ExportContractSchedule_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@Id", Id);
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

	public static DataTable ExportContractSchedule_GetPending(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportContractSchedule>("[dbo].[usp_ExportContractSchedule_GetPending]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CustomGroupDocumentSave(int DocumentTypeId, int Id, int CustomGroupId)
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
			sqlCommand.CommandText = "[sdt].[USP_ShipmentDocumentSchedule_AutoInsert]";
			sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", DocumentTypeId);
			sqlCommand.Parameters.AddWithValue("@RefDocId", Id);
			sqlCommand.Parameters.AddWithValue("@CustomGroupId", CustomGroupId);
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
