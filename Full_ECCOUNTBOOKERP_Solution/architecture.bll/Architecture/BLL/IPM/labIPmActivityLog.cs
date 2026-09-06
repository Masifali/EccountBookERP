using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.IPM;
using Architecture.Model;
using Architecture.Model.IPM;

namespace Architecture.BLL.IPM;

public class labIPmActivityLog
{
	public static int save(Architecture.Model.IPM.labIPmActivityLog ctry)
	{
		try
		{
			return new Architecture.DAL.IPM.labIPmActivityLog().SetData(ctry, "USP_labIPmActivityLog_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@BranchesId", obj.BranchesId));
			p.Add(new SqlParameter("@FinancialYearId", obj.FinancialYearId));
			if (obj.DocumentTypeId > 0)
			{
				p.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			}
			if (obj.StatusId > 0)
			{
				p.Add(new SqlParameter("@stockReleaseHoldStatusId", obj.StatusId));
			}
			p.Add(new SqlParameter("@CanViewAllRecord", obj.CanViewAllRecord));
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter("@EntryUserId", obj.EntryUser));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter("@EntryFromDate", obj.EntryDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter("@EntryToDate", obj.EntryDateTo));
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter("@ModifyFromDate", obj.ModifyDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter("@ModifyToDate", obj.ModifyDateTo));
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter("@ApprovedFromDate", obj.ApprovedDateFrom));
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter("@ApprovedToDate", obj.ApprovedDateTo));
			}
			if (obj.ApprovedStatus.HasValue)
			{
				p.Add(new SqlParameter("@IsApproved", obj.ApprovedStatus));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.IPM.labIPmActivityLog>("[dbo].[USP_labIPmActivityLog_FormHistory]", p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static List<Architecture.Model.IPM.labIPmActivityLog> GetById(int Id)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@Id", Id)
			};
			return GenericProvider.GetAllDetail<Architecture.Model.IPM.labIPmActivityLog>(sqlcon, "USP_labIPmActivityLog_ReadById", parameters);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable CommonCombo(string ComboType = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@ComboType", ComboType));
			return GenericProvider.GetDataTableProc<Architecture.Model.IPM.labIPmActivityLog>("[dbo].[USP_IPM_CommonCombo]", p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static int GenerateMasterId(int OrganizationId, int CompanyId, int BranchId, int FinancialYearId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@BranchId", BranchId));
			p.Add(new SqlParameter("@FinancialYearId", FinancialYearId));
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.IPM.labIPmActivityLog>("[dbo].[USP_labIPmActivityLog_GenerateMasterId]", p).Rows[0]["labIPmActivityLogMasterId"]);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static string LabIPmActivityLog_GenerateLotNo(int OrganizationId, int CompanyId, int BranchId, int FinancialYearId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@BranchId", BranchId));
			p.Add(new SqlParameter("@FinancialYearId", FinancialYearId));
			return Conversion.ToString(GenericProvider.GetDataTableProc<Architecture.Model.IPM.labIPmActivityLog>("[dbo].[USP_labIPmActivityLog_GenerateLotNo]", p).Rows[0]["FumigationLotNo"]);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable HoldStockForFumigation(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo",
					Value = obj.ToDate
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
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.RefWarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefWarehouseId",
					Value = obj.RefWarehouseId
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferenceDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
				});
			}
			if (obj.ItemUomId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.ItemUomId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (!string.IsNullOrEmpty(obj.CropYear))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
			if (!string.IsNullOrEmpty(obj.WarehouseIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseIds",
					Value = obj.WarehouseIds
				});
			}
			if (!string.IsNullOrEmpty(obj.ItemIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemIds",
					Value = obj.ItemIds
				});
			}
			if (!string.IsNullOrEmpty(obj.ParentCategoryIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryIds",
					Value = obj.ParentCategoryIds
				});
			}
			if (obj.StockReleaseFromFumigation != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StockReleaseFromFumigation",
					Value = obj.StockReleaseFromFumigation
				});
			}
			if (obj.AvailableForFumigation != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AvailableForFumigation",
					Value = obj.AvailableForFumigation
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[getHoldStockForFumigation]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void StockHoldData(ApprovalList obj)
	{
		if (obj == null || obj.AllApprovalLists == null || obj.AllApprovalLists.Count == 0)
		{
			return;
		}
		DataTable dt = new DataTable();
		dt.Columns.Add("OrganizationId", typeof(int));
		dt.Columns.Add("CompanyId", typeof(int));
		dt.Columns.Add("DocumentTypeId", typeof(int));
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("DetailId", typeof(int));
		dt.Columns.Add("UserId", typeof(int));
		dt.Columns.Add("ReqType", typeof(string));
		dt.Columns.Add("DateForUpdate", typeof(DateTime));
		dt.Columns.Add("Remarks", typeof(string));
		foreach (ApprovalList item in obj.AllApprovalLists)
		{
			dt.Rows.Add(item.OrganizationId, item.CompanyId, item.DocumentTypeId, item.Id, item.DetailId, item.EntryUser, ((object)item.ReqType) ?? ((object)DBNull.Value), DateTime.Now, ((object)item.Comments) ?? ((object)DBNull.Value));
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand cmd = new SqlCommand("usp_StockHoldForFumigation_UpdateStatus", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@StockHoldData", dt).SqlDbType = SqlDbType.Structured;
				cmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static DataTable GetDataForDropDownFromlabIPmActivityLog(int OrganizationId, int CompanyId, string DocumentTypeIds = "", string Activity = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			if (DocumentTypeIds != null && DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", DocumentTypeIds));
			}
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter("@Activity", Activity));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.IPM.labIPmActivityLog>("[dbo].[USP_GetDataForDropDownFromlabIPmActivityLog]", p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable LabIPmActivityLogRegister(ReportsParameters obj)
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
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
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
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferenceDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			if (obj.ItemUomId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.ItemUomId
				});
			}
			if (!string.IsNullOrEmpty(obj.WarehouseIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseIds",
					Value = obj.WarehouseIds
				});
			}
			if (!string.IsNullOrEmpty(obj.ItemIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemIds",
					Value = obj.ItemIds
				});
			}
			if (!string.IsNullOrEmpty(obj.ParentCategoryIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryIds",
					Value = obj.ParentCategoryIds
				});
			}
			if (obj.StockReleaseHoldStatusId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@stockReleaseHoldStatusId",
					Value = obj.StockReleaseHoldStatusId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_labIPmActivityLog_Register]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
