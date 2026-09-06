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
using Architecture.Model.WeightBridge;

namespace Architecture.BLL.PartyProcessing;

public class GatePassPartyProcessing
{
	public static int Save(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.PartyProcessing.GatePassPartyProcessing().SetData(obj, "Sp_GatePassPartyProcessing_Insert");
			}
			obj.ActionId = 2;
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.PartyProcessing.GatePassPartyProcessing().SetData(obj, "Sp_GatePassPartyProcessing_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PartyProcessing.GatePassPartyProcessing GetByID(int Id)
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
			new List<Architecture.Model.PartyProcessing.GatePassPartyProcessing>();
			return new Architecture.DAL.PartyProcessing.GatePassPartyProcessing().GetAll("Sp_GatePassPartyProcessing_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GatepassHistory(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@organizationId",
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
			if (obj.StockPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StockPartyId",
					Value = obj.StockPartyId
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
			if (obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("[dbo].[USP_GatePassPartyProcessing_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj)
	{
		try
		{
			int code = 0;
			DataTable dt = new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_GatePassPartyProcessing_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["GpSrNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static int GenerateGPTypeCode(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj)
	{
		try
		{
			int Code = 0;
			DataTable dt = new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateGPTypeCode"
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GatepassType",
				Value = obj.GatepassType
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_GatePassPartyProcessing_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["GpTypeSrNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GpNoPandingforInvGrnPartyProcessing(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj)
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
				ParameterName = "@Activity",
				Value = "GpNoPandingforInvGrnPartyProcessing"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_GatePassPartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGatePassReferenceType(int OrganizationId = 0, int CompanyId = 0)
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
			return GenericProvider.GetDataTableProc<GatepassType>("Sp_GatePassReferenceTypes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGatePassPartyProcessingForWeighBridge(ReportsParameters obj)
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
				ParameterName = "@WeighBridgeStatus",
				Value = obj.WeighBridgeStatus
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetGatePassPartyProcessingForWeighBridge"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_GatePassPartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByGpNoForWeighBridge(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.GpSrNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNo",
					Value = obj.GpSrNo
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
				Value = "ReadByGpNoForWeighBridge"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_GatePassPartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByGpNoForInvGrnPartyProcessing(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.GpSrNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNo",
					Value = obj.GpSrNo
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
				Value = "ReadByGpNoForInvGrnPartyProcessing"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_GatePassPartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingGatePassForInvGrnGdnStorePartyProcessing(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj)
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
				Value = "GetPendingGatePassForInvGrnGdnStorePartyProcessing"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_GatePassPartyProcessing_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromGatePass(int OrganizationId, int CompanyId, string DocumentTypeIds = null, string Activity = null)
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
			if (DocumentTypeIds != null && DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = DocumentTypeIds
				});
			}
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_GetDataForDropDownFromGatePassPartyProcessing]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVehicleWeight(WbTransactions obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVehicleWeight"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
