using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model;
using Architecture.Model.PartyProcessing;
using Architecture.Model.Production;

namespace Architecture.BLL.PartyProcessing;

public class StockConversionPartyProcessing
{
	public static int Save(Architecture.Model.PartyProcessing.StockConversionPartyProcessing obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.PartyProcessing.StockConversionPartyProcessing().SetData(obj, "[USP_InvStockConversionPartyProcessing_Insert]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.PartyProcessing.StockConversionPartyProcessing().SetData(obj, "[USP_InvStockConversionPartyProcessing_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PartyProcessing.StockConversionPartyProcessing ReadById(int Id)
	{
		try
		{
			new List<Architecture.Model.PartyProcessing.StockConversionPartyProcessing>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return new Architecture.DAL.PartyProcessing.StockConversionPartyProcessing().GetData("USP_InvStockConversionPartyProcessing_ReadById", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetGenerateCode(Architecture.Model.PartyProcessing.StockConversionPartyProcessing obj)
	{
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchedId",
				Value = obj.BranchedId
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.StockConversionPartyProcessing>("USP_InvStockConversionPartyProcessing_GenerateCode", p);
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
				ParameterName = "@BranchedId",
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
					Value = clsGlobalVariables.UserAccount.ID
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.StockConversionPartyProcessing>("USP_InvStockConversionPartyProcessing_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable StockConversionPartyProcessingSummeryRpt(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.StockConversionPartyProcessing>("SpInvStockConversionPartyProcessing_Summery_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GellAllReferencePartiesStockPartyWise(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@StockPartyId", obj.StockPartyId));
			p.Add(new SqlParameter("@Activity", "GellAllReferencePartiesStockPartyWise"));
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.JobLotPartyProcessing>("[Sp_ReferenceParties_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int EntryUserId, int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUserId",
				Value = EntryUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			GenericProvider.GetDataTableProc<InvProductionJobOrder>("usp_InvStockConversionPartyProcessingDeleteById", p);
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
			if (obj.RefPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferencePartyId",
					Value = obj.RefPartyId
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_StockConversionPartyProcessingRegisterWithActivity]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromInvStockConversionPartyProcessing(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<InvProductionJobOrder>("USP_GetDataForDropDownFromInvStockConversionPartyProcessing", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
