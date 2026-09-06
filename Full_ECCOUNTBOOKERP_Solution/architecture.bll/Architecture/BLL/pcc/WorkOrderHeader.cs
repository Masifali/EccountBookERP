using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class WorkOrderHeader
{
	public static int Save(Architecture.Model.pcc.WorkOrderHeader obj)
	{
		try
		{
			obj.ActionId = ((obj.WorkOrderHeaderId == 0) ? 1 : 2);
			return new Architecture.DAL.pcc.WorkOrderHeader().SetData(obj, "[pcc].[USP_WorkOrderHeader_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.WorkOrderHeader ReadById(int Id)
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
			new List<Architecture.Model.pcc.WorkOrderHeader>();
			return new Architecture.DAL.pcc.WorkOrderHeader().GetData("[pcc].[USP_WorkOrderHeader_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.pcc.WorkOrderHeader obj)
	{
		DataTable dt = null;
		int code = 0;
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.pcc.WorkOrderHeader>("[pcc].[USP_WorkOrderHeader_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int EntryUserId, int Id)
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
			sqlCommand.CommandText = "[pcc].[USP_WorkOrderHeader_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
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

	public static DataTable FormHistory(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
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
			if (!string.IsNullOrEmpty(obj.WorkOrderNo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WorkOrderNo",
					Value = obj.WorkOrderNo
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
			if (obj.ProductionTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductionTypeId
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.WorkOrderHeader>("[pcc].[USP_WorkOrderHeader_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingWorkOrderForProduction(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryId",
					Value = obj.ParentCategoryId
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
			if (obj.ProductionTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductionTypeId
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
			if (!string.IsNullOrEmpty(obj.OrderDetailIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderDetailIds",
					Value = obj.OrderDetailIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.WorkOrderHeader>("[pcc].[USP_PendingWorkOrderForProduction]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromWorkOrderHeader(ReportsParameters obj)
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[pcc].[USP_GetDataForDropDownFromWorkOrderHeader]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WorkOrderHeader_Slip(ReportsParameters obj)
	{
		DataTable dtMain = null;
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			DataSet ds = GenericProvider.GetDataSetProc<Architecture.Model.pcc.WorkOrderHeader>("[pcc].[USP_WorkOrderHeader_Slip]", p);
			if (ds != null && ds.Tables.Count > 0)
			{
				dtMain = ds.Tables[0];
				CommonServices.ReplaceCompLogoColumn(dtMain, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			}
			return dtMain;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WorkOrder_InputSubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@WorkOrderHeaderId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.WorkOrderHeader>("[pcc].[USP_WorkOrder_InputSubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WorkOrder_Report(ReportsParameters obj)
	{
		DataTable dtMain = null;
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			if (obj.ProductionTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionTypeId",
					Value = obj.ProductionTypeId
				});
			}
			if (obj.BOMHeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BOMHeaderId",
					Value = obj.BOMHeaderId
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (!string.IsNullOrWhiteSpace(obj.CustomerName))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerName",
					Value = obj.CustomerName
				});
			}
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderId",
					Value = obj.OrderId
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
			if (!string.IsNullOrWhiteSpace(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			DataSet ds = GenericProvider.GetDataSetProc<Architecture.Model.pcc.WorkOrderHeader>("[pcc].[USP_WorkOrder_Report]", p);
			if (ds != null && ds.Tables.Count > 0)
			{
				dtMain = ds.Tables[0];
				CommonServices.ReplaceCompLogoColumn(dtMain, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			}
			return dtMain;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WorkOrderHeader_ForApproval(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
			if (obj.ProductinTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductinTypeId",
					Value = obj.ProductinTypeId
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (obj.BOMHeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BOMHeaderId",
					Value = obj.BOMHeaderId
				});
			}
			if (!string.IsNullOrEmpty(obj.CustomerName))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerName",
					Value = obj.CustomerName
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.WorkOrderHeader>("[pcc].[USP_WorkOrderHeader_ForApproval]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateApprovalStatus(ApprovalList obj)
	{
		if (obj == null || obj.AllApprovalLists == null || obj.AllApprovalLists.Count == 0)
		{
			return;
		}
		DataTable dt = new DataTable();
		dt.Columns.Add("OrganizationId", typeof(int));
		dt.Columns.Add("CompanyId", typeof(int));
		dt.Columns.Add("FinancialYearId", typeof(int));
		dt.Columns.Add("DocumentTypeId", typeof(int));
		dt.Columns.Add("ApprovedUserId", typeof(int));
		dt.Columns.Add("ReqType", typeof(string));
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("ApprovalRemarks", typeof(string));
		foreach (ApprovalList item in obj.AllApprovalLists)
		{
			dt.Rows.Add(item.OrganizationId, item.CompanyId, item.FinancialYearId, item.DocumentTypeId, item.EntryUser, ((object)item.ReqType) ?? ((object)DBNull.Value), item.Id, ((object)item.Comments) ?? ((object)DBNull.Value));
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand cmd = new SqlCommand("[pcc].[USP_WorkOrderHeader_Approval]", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@ApprovalData", dt).SqlDbType = SqlDbType.Structured;
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
}
