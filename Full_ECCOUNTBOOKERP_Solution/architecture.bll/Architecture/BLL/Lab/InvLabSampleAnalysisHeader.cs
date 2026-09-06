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

public class InvLabSampleAnalysisHeader
{
	public static int Save(Architecture.Model.Lab.InvLabSampleAnalysisHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.InvLabSampleAnalysisHeader().SetData(obj, "Sp_InvLabSampleAnalysisHeader_Insert");
			}
			return new Architecture.DAL.Lab.InvLabSampleAnalysisHeader().SetData(obj, "Sp_InvLabSampleAnalysisHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Lab.InvLabSampleAnalysisHeader obj)
	{
		DataTable dt = null;
		try
		{
			int code = 0;
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
				Value = "GenerateDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
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
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
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
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.AnalysisGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AnalysisGroupId",
					Value = obj.AnalysisGroupId
				});
			}
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Lab.InvLabSampleAnalysisHeader GetByID(int Id)
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
			List<Architecture.Model.Lab.InvLabSampleAnalysisHeader> lst = new List<Architecture.Model.Lab.InvLabSampleAnalysisHeader>();
			lst = new Architecture.DAL.Lab.InvLabSampleAnalysisHeader().GetData("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
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

	public static DataTable ReadByAnalysisGroupId(Architecture.Model.Lab.InvLabSampleAnalysisHeader obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetBySampleGroupNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBySampleId(Architecture.Model.Lab.InvLabSampleAnalysisHeader obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetBySampleNoId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPrintSlipAndReport(ReportsParameters obj)
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromSampleNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToSampleNo",
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleLogRegister>("Sp_InvLabSampleAnalysisHeader_RiceSlipAndRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetIdFromDocNo(Architecture.Model.Lab.InvLabSampleAnalysisHeader obj)
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
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByDocNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSampleNoFromItemIdNPONo(ReportsParameters obj)
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
				Value = obj.OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByPONoAndItemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSampleNoBySupplierCustomerIdAndITemId(Architecture.Model.Lab.InvLabSampleAnalysisHeader obj)
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
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			if (obj.CommissionAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CommissionAgentId",
					Value = obj.CommissionAgentId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSampleNoBySupplierCustomerIdAndITemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForLabSampleAnalysisSubReport(int Id)
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
				Value = "GetDataForLabSampleAnalysisSubReport"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSampleLogForSampleAnalysis(ReportsParameters obj)
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
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDataForSampleAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleLogRegister_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemFromSampleAnalysis(int OrganizationId, int CompanyId)
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
				Value = "GetItemFromSampleAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetParentCategoryFromSampleAnalysis(int OrganizationId, int CompanyId)
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
				Value = "GetParentCategoryFromSampleAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseOrderFromSampleAnalysis(int OrganizationId, int CompanyId)
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
				Value = "GetPurchaseOrderFromSampleAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSuppliersFromSampleAnalysis(int OrganizationId, int CompanyId)
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
				Value = "GetSuppliersFromSampleAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabSampleAnalysisHeader>("Sp_InvLabSampleAnalysisHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LabSampleAnalysisRegister(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisPurchaseHeader>("[USP_LabSampleAnalysis_Register]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromInvLabSampleAnalysisHeader(ReportsParameters obj)
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
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryId",
					Value = obj.ParentCategoryId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.LabInProcessAnalysisRawHeader>("[dbo].[USP_GetDataForDropDownFromInvLabSampleAnalysisHeader]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
