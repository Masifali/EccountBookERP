using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Model.Production;

namespace Architecture.BLL.PartyProcessing;

public class InvFoodProductionPartyProcessing
{
	public static int Save(Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PartyProcessing.InvFoodProductionPartyProcessing().SetData(obj, "Sp_InvFoodProductionPartyProcessing_Insert");
			}
			return new Architecture.DAL.PartyProcessing.InvFoodProductionPartyProcessing().SetData(obj, "Sp_InvFoodProductionPartyProcessing_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing GetByID(int Id)
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
			new List<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing>();
			return new Architecture.DAL.PartyProcessing.InvFoodProductionPartyProcessing().GetDate("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p)[0];
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
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
			if (obj.InvJobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvJobOrderId",
					Value = obj.InvJobOrderId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing>("USP_InvFoodProductionPartyProcessing_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing>("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p);
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

	public static DataTable PendingWorkDashboardPartyProcessing(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancaialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.ReportType != null && obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportType",
					Value = obj.ReportType
				});
			}
			if (obj.DepartmentName != null && obj.DepartmentName != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingWorkDepartment",
					Value = obj.DepartmentName
				});
			}
			if (obj.PendingWorkName != null && obj.PendingWorkName != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingWorkCaption",
					Value = obj.PendingWorkName
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrder>("Sp_Dashboards_PendingWork_PartyProcessing", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSummeryValues(ReportsParameters obj)
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
				ParameterName = "@JobOrderId",
				Value = obj.JobOrderId
			});
			if (obj.NetWeight != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FgNetWeight",
					Value = obj.NetWeight
				});
			}
			return GenericProvider.GetDataTableProc<InvFoodProduction>("Sp_InvFoodProductionPartyProcessing_GetSummeryValues", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInPutTotalQtyandWeightByJobOrderId(Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing obj)
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
				ParameterName = "@InvJobOrderId",
				Value = obj.InvJobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInPutTotalQtyandWeightByJobOrderId"
			});
			return GenericProvider.GetDataTableProc<InvFoodProduction>("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FoodProductionSlip(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
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
			if (obj.JobOrderFrom != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderFrom",
					Value = obj.JobOrderFrom
				});
			}
			if (obj.JobOrderTo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderTo",
					Value = obj.JobOrderTo
				});
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = obj.JobOrderId
				});
			}
			if (obj.ReqType != null && obj.ReqType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryType",
					Value = obj.ReqType
				});
			}
			return GenericProvider.GetDataTableProc<InvFoodProduction>("[dbo].[Sp_InvFoodProductionPartyProcessing_Rpt]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromInvFoodProductionPartyProcessing(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (!string.IsNullOrEmpty(obj.DocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<InvProductionJobOrder>("USP_GetDataForDropDownFromInvFoodProductionPartyProcessing", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionRegisterPartyProcessing(ReportsParameters obj)
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
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvJobOrderId",
					Value = obj.JobOrderId
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
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoriesId",
					Value = obj.ParentCategoryId
				});
			}
			if (obj.StockPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StockPartyId",
					Value = obj.StockPartyId
				});
			}
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryType ",
					Value = obj.ReportType
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = obj.Activity
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_ProductionPartyProcessingRegisterWithActivity]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
