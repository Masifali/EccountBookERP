using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model;
using Architecture.Model.Lab;

namespace Architecture.BLL.Lab;

public class InvLabAnalysisPurchaseHeader
{
	public static DataTable LabPurchaseAnalyticsPartyWiseByItemId(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SeasonStart",
				Value = obj.StartDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SeasonEnd",
				Value = obj.EndDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			if (obj.SortNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SortNo",
					Value = obj.SortNo
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("[dbo].[usp_LabPurchaseAnalyticsPartyWiseByItemId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LabPurchaseAnalyticsItemWise(ReportsParameters obj)
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
				ParameterName = "@InventoryParentId",
				Value = obj.InventoryParentCategories
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SeasonStart",
				Value = obj.StartDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SeasonEnd",
				Value = obj.EndDate
			});
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("[dbo].[usp_LabPurchaseAnalytics_ItemWise]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LabPurchaseAnalyticsA(ReportsParameters obj)
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
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SeasonStart",
				Value = obj.StartDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SeasonEnd",
				Value = obj.EndDate
			});
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("usp_LabPurchaseAnalyticsA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int Save(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.EntryDate = DateTime.Now;
				return new Architecture.DAL.Lab.InvLabAnalysisPurchaseHeader().SetData(obj, "Sp_InvLabAnalysisPurchaseHeader_Insert");
			}
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.Lab.InvLabAnalysisPurchaseHeader().SetData(obj, "Sp_InvLabAnalysisPurchaseHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int UpdateDuringApproval(LabAnalysisPurchaseUpdateDuringApproval obj)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (LabAnalysisPurchaseUpdateDuringApproval item in obj.LabAnalysisPurchaseUpdateDuringApprovalList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "[dbo].[USP_InvLabAnalysisPurchaseHeader_UpdateDuringApproval]"));
			}
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static int GenerateCode(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj)
	{
		DataTable dt = null;
		try
		{
			int Code = 0;
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNo"
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(ReportsParameters obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
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
					ParameterName = "@ToDate ",
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
			if (obj.GpId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InwardGatePassId",
					Value = obj.GpId
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
			if (obj.AnalystStatus != null && obj.AnalystStatus != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AnaylstStatus",
					Value = obj.AnalystStatus
				});
			}
			if (obj.ApprovedFilter != null && obj.ApprovedFilter != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedStatus",
					Value = obj.ApprovedFilter
				});
			}
			if (obj.VehicleNo != null && obj.VehicleNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
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
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
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
			if (obj.PestResultId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PestResultId",
					Value = obj.PestResultId
				});
			}
			if (obj.PestStatusId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PestStatusId",
					Value = obj.PestStatusId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Lab.InvLabAnalysisPurchaseHeader ReadById(int Id)
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
			List<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader> lst = new List<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>();
			lst = new Architecture.DAL.Lab.InvLabAnalysisPurchaseHeader().GetData("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
			if (lst.Count > 0)
			{
				return lst[0];
			}
			return null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetById(int Id)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseRegister(ReportsParameters obj)
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
					ParameterName = "@Datefrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo ",
					Value = obj.ToDate
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId ",
					Value = obj.ItemId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId ",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PurchaseOrderId",
					Value = obj.OrderId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategorId",
					Value = obj.InventoryParentCategories
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchase_RegisterReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromLabPurchaseAnaylsis(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.ParentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentId",
					Value = obj.ParentId
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			if (obj.Keyword != null && obj.Keyword != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Keyword",
					Value = obj.Keyword
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("[dbo].[USP_GetDataForDropDownFromLabPurchaseAnaylsis]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseReport(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate ",
					Value = obj.ToDate
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id ",
					Value = obj.Id
				});
			}
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderId ",
					Value = obj.OrderId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierId ",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromGpNo ",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToGpNo  ",
					Value = obj.GpSrNoT
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchase_RiceHistory_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable RptInvLabPurchaseAnalysisSlip653(ReportsParameters obj)
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
					ParameterName = "@DateF",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateT",
					Value = obj.ToDate
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoF",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoT",
					Value = obj.GpSrNoT
				});
			}
			if (obj.GpId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpId",
					Value = obj.GpId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabAnalysisPurchaseSlip_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvLabAnalysisInProcessHeader_Slip659(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("USp_InvLabAnalysisInProcessHeader_Slip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForLabInProcessGroupAnalysisSubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PihId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("USp_InvLabAnalysisGroup_SubReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForLabAnalysisStep_SubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PihId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("USp_InvLabAnalysisStep_SubReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGatePassIdandNoforWeighBridge(ReportsParameters obj)
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
				ParameterName = "@WeighBridgeStatus",
				Value = obj.WeighBridgeStatus
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("USP_GetGatePassIdandNoforWeighBridge", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseAnalysisDetailByPo(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj)
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
				ParameterName = "@PurchaseOrderId",
				Value = obj.PurchaseOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPurchaseAnalysisDetailByPo"
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSampleAnalysisDetailByPo(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj)
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
				ParameterName = "@PurchaseOrderId",
				Value = obj.PurchaseOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSampleAnalysisDetailByPo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGpDetailByGatepassNo(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj)
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
				ParameterName = "@InwardGatePassId",
				Value = obj.GatePassInwardId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetGpDetailByGatepassNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetMoistureFromOrderByItemIdAndOrder(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj)
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
				ParameterName = "@OrderItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PurchaseOrderId",
				Value = obj.PurchaseOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetMoistureFromOrderByItemIdAndOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLabDataForApproval(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDate",
					Value = obj.GpDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.BiltyDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BiltyDate",
					Value = obj.BiltyDate
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
			if (obj.VehicleNo != null && obj.VehicleNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Usp_LabPurchasenAalysisDataForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RecordApprovedByLabHeaderId(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@IsAcceptedApproved",
					Value = item.ActionTypeId
				});
				if (item.Rate != 0.0)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@DeductedRate",
						Value = item.Rate
					});
				}
				if (item.Weight != 0.0)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@DeductedWeight",
						Value = item.Weight
					});
				}
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "RecordApprovedByLabHeaderId"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForLabPurchaseAnalysisSubReport(int Id)
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
				Value = "GetDataForLabPurchaseAnalysisSubReport"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemFromPurchaseAnalysis(int OrganizationId, int CompanyId)
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
				ParameterName = "@Activity",
				Value = "GetItemFromPurchaseAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetParentCategoryFromPurchaseAnalysis(int OrganizationId, int CompanyId, int BranchesId = 0)
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
			if (BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetParentCategoryFromPurchaseAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseOrderFromPurchaseAnalysis(int OrganizationId, int CompanyId, int BranchesId = 0)
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
			if (BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPurchaseOrderFromPurchaseAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSuppliersFromPurchaseAnalysis(int OrganizationId, int CompanyId, int BranchesId = 0)
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
			if (BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSuppliersFromPurchaseAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RecordApprovedBySampleLabHeaderId(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@IsAcceptedApproved",
					Value = item.ActionTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "RecordApprovedByLabSampleId"
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLabDataForSampleApproval(Architecture.Model.Lab.InvLabAnalysisPurchaseHeader obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Usp_LabSamplenAalysisDataForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LabPurchaseAnalysisRegister(ReportsParameters obj)
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
					ParameterName = "@Datefrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo ",
					Value = obj.ToDate
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoFrom ",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoTo ",
					Value = obj.GpSrNoT
				});
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId ",
					Value = obj.ItemId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId ",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PurchaseOrderId",
					Value = obj.OrderId
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsAccepted",
					Value = obj.IsApproved
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategorId",
					Value = obj.InventoryParentCategories
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("USP_LabPurchaseAnalysis_Register", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGatePassIdandNoforWeighBridge_Engr(ReportsParameters obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("USP_GetGatePassIdandNoforWeighBridge_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LabAnalysisPurchaseByGP_SubReport(ReportsParameters obj)
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
				ParameterName = "@GpId",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("USP_InvLabAnalysisPurchaseByGP_SubReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseAnalysisDataForGrnByGpIdAndItemId(ReportsParameters obj)
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
				ParameterName = "@GatePassInwardId",
				Value = obj.GpId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPurchaseAnalysisDataForGrnByGpIdAndItemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("Sp_InvLabAnalysisPurchaseHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getLabAnaylsisAgainstMasterParamsById(int OrganizationId, int CompanyId, int LabId)
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
				ParameterName = "@Id",
				Value = LabId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("usp_getLabAnaylsisAgainstMasterParamsById", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet LabDataVehicleWiseByParent(ReportsParameters obj)
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
				ParameterName = "@ParentCategoryId",
				Value = obj.InventoryParentCategories
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
					ParameterName = "@ToDate ",
					Value = obj.ToDate
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId ",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoisuteTo ",
					Value = obj.ToDocNo
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoisuteFrom ",
					Value = obj.FromDocNo
				});
			}
			if (Conversion.ToInt(obj.OrderNo) != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderNo ",
					Value = obj.OrderNo
				});
			}
			if (obj.SlabFrom1 != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoistureSlab1From ",
					Value = obj.SlabFrom1
				});
			}
			if (obj.SlabFrom2 != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoistureSlab2From ",
					Value = obj.SlabFrom2
				});
			}
			if (obj.SlabFrom3 != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoistureSlab3From ",
					Value = obj.SlabFrom3
				});
			}
			if (obj.SlabTo1 != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoistureSlab1To",
					Value = obj.SlabTo1
				});
			}
			if (obj.SlabTo2 != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoistureSlab2To ",
					Value = obj.SlabTo2
				});
			}
			if (obj.SlabTo3 != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MoistureSlab3To",
					Value = obj.SlabTo3
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("usp_getLabDataVehicleWiseByParentId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdatePurchaseAnalysisParameterResultValue(ReportsParameters obj)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (ReportsParameters item in obj.ReportsParameterslist)
				{
					new List<SqlParameter>();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "USP_UpdatePurchaseAnalysisParameterResultValue";
					sqlCommand.Parameters.AddWithValue("@Id", item.Id);
					sqlCommand.Parameters.AddWithValue("@ResultValue", item.ResultValue);
					sqlCommand.Parameters.AddWithValue("@Remarks", item.Remarks);
					sqlCommand.ExecuteNonQuery();
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				throw ex;
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	public static void LabAnalysisPurchase_CutsUpdatationById(int Id, int ModifyUserId, double DeductedRate, double DeductedWeight)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			new List<SqlParameter>();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[USP_LabAnalysisPurchase_CutsUpdatationById]";
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@ModifyUserId", ModifyUserId);
			sqlCommand.Parameters.AddWithValue("@DeductedRate", DeductedRate);
			sqlCommand.Parameters.AddWithValue("@DeductedWeight", DeductedWeight);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable GetItemsForUnloading(ReportsParameters obj)
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
				ParameterName = "@GpId",
				Value = obj.GpId
			});
			if (obj.LabId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LabId",
					Value = obj.LabId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("USP_InvLabAnalysisPurchaseHeader_GetItemsForUnloading", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPestResult()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("usp_PestResult", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPestStatus()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("usp_PestStatus", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPMLabStatus()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("[dbo].[usp_PMLabStatus]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
