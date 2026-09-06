using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.WeightBridge;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.WeightBridge;

namespace Architecture.BLL.WeightBridge;

public class WbTransactions
{
	public static VoucherHead MakeVoucher(Architecture.Model.WeightBridge.WbTransactions obj, int CashWithWeighbridgeAccount, int OtherIncomeWithWeighbridgeAccount)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			vh.DocumentTypeId = obj.DocTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.TicketNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.WbRemarks);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchId;
			vh.ProjectId = obj.ProjectId;
			vh.BillAmount = 0.0;
			vh.ManualBillNo = "";
			vh.DueDate = obj.DocDate;
			vh.DueDays = 0;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			string Remarks = $"Vehicle #{obj.VehicleNo} | Charges: {obj.WbCharges} | First Weight: {obj.FirstWeight}";
			if (obj.SecondWeight > 0.0)
			{
				Remarks += $" | Second Weight: {obj.SecondWeight} | Net Weight: {obj.NetWbWeight}";
			}
			VoucherDetail vd = new VoucherDetail();
			vd.AccountId = CashWithWeighbridgeAccount;
			vd.AgainstAccountId = OtherIncomeWithWeighbridgeAccount;
			vd.Comments = Remarks;
			vd.DebitAmount = Conversion.ToDouble(obj.WbCharges);
			vh.voucherDetailList.Add(vd);
			VoucherDetail vd2 = new VoucherDetail();
			vd2.AccountId = OtherIncomeWithWeighbridgeAccount;
			vd2.AgainstAccountId = CashWithWeighbridgeAccount;
			vd2.Comments = Remarks;
			vd2.CreditAmount = Conversion.ToDouble(obj.WbCharges);
			vh.voucherDetailList.Add(vd2);
			return vh;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int Save(Architecture.Model.WeightBridge.WbTransactions obj)
	{
		try
		{
			int CashWithWeighbridgeAccount = Conversion.ToInt(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CashWithWeighbridgeAccount"));
			int OtherIncomeWithWeighbridgeAccount = Conversion.ToInt(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "OtherIncomeWithWeighbridgeAccount"));
			if (obj.WbCharges > 0.0 && OtherIncomeWithWeighbridgeAccount > 0 && CashWithWeighbridgeAccount > 0)
			{
				obj.VoucherHeadWeighBridge = MakeVoucher(obj, CashWithWeighbridgeAccount, OtherIncomeWithWeighbridgeAccount);
			}
			if (obj.Id == 0)
			{
				obj.ModifyUser = 0;
				if (obj.ReferenceDocTypeId == 74 || obj.ReferenceDocTypeId == 75)
				{
					obj.ReferenceDocNoId = obj.TicketNo;
				}
				return new Architecture.DAL.WeightBridge.WbTransactions().SetDate(obj, "Sp_WbTransactions_Insert");
			}
			return new Architecture.DAL.WeightBridge.WbTransactions().SetDate(obj, "Sp_WbTransactions_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.WeightBridge.WbTransactions obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchId",
					Value = obj.BranchId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["TicketNo"]);
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchId",
				Value = obj.BranchesId
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
					ParameterName = "@TicketNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TicketNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoFrom",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoTo",
					Value = obj.GpSrNoT
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WeighBridgeType",
					Value = obj.Activity
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
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FirstWeighBridgeId",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingWeighBridgeForSecondWeight(ReportsParameters obj)
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
					ParameterName = "@BranchId",
					Value = obj.BranchesId
				});
			}
			if (obj.FirstWeighBridgeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FirstWeighBridgeId",
					Value = obj.FirstWeighBridgeId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateF))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDateF",
					Value = obj.GpDateF
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateT))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDateT",
					Value = obj.GpDateT
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoFrom",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoTo",
					Value = obj.GpSrNoT
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("USP_PendingWeighBridgeForSecondWeight", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetTicketNoGrnForPurchaseFromPartyProcessing(int OrganizationId, int CompanyId, int DocumentTypeId = 0, int RecId = 0)
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
			if (RecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = DocumentTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("usp_getTicketNoGrnForPurchase", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByID(int Id)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetIdbyTicketNo(Architecture.Model.WeightBridge.WbTransactions obj)
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
				ParameterName = "@TicketNo",
				Value = obj.TicketNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdbyTicketNo"
			});
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchId
				});
			}
			dt = GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByTicketNoByRefDocId(Architecture.Model.WeightBridge.WbTransactions obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ReferenceDocNoId",
				Value = obj.ReferenceDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.ReferenceDocTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetTicketNoByRefDocId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByGatePassId(Architecture.Model.WeightBridge.WbTransactions obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ReferenceDocNoId",
				Value = obj.ReferenceDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = obj.ReferenceDocTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWbTransactionByGatePassId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WbTransationByGPID_SubReport(int GpId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GpId",
				Value = GpId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("[dbo].[USP_WbTransationByGPID_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WbTransationByOutwardGPID_SubReport(int GpId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GpId",
				Value = GpId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("[dbo].[USP_WbTransationByOutwardGPID_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllPendingTicketNo(Architecture.Model.WeightBridge.WbTransactions obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeIdGpIn",
				Value = obj.RefDocumentTypeIdGpIn
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeIdGpOut",
				Value = obj.RefDocumentTypeIdGpOut
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeIdSt",
				Value = obj.RefDocumentTypeIdSt
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PendingTicketsforContratorWages"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetNetWeightFromWbTransactions(Architecture.Model.WeightBridge.WbTransactions obj)
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
				Value = obj.ReferenceDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = obj.ReferenceDocTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetNetWeightFromWbTransactions"
			});
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadWBHistory(ReportsParameters obj)
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
					ParameterName = "@BranchId",
					Value = obj.BranchesId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@mId",
					Value = obj.Id
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TicketNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TicketNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoFrom",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoTo",
					Value = obj.GpSrNoT
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
			if (obj.RefDocumentTypeIds != null && obj.RefDocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeIds",
					Value = obj.RefDocumentTypeIds
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WeighBridgeType",
					Value = obj.Activity
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.OrderNoFrom != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderNoFrom",
					Value = obj.OrderNoFrom
				});
			}
			if (obj.OrderNoTo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderNoTo",
					Value = obj.OrderNoTo
				});
			}
			if (obj.CompanyName != null && obj.CompanyName != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WbPartyName",
					Value = obj.CompanyName
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadHistory"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetWeighBridgeDataForRejectedTicketNos(Architecture.Model.WeightBridge.WbTransactions obj)
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.ReferenceDocTypeId
			});
			if (obj.BranchId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWeighBridgeDataForRejectedTicketNos"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void WeighBridgeRejectedStatusUpdate(Architecture.Model.WeightBridge.WbTransactions obj)
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.ReferenceDocTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = obj.EntryUser
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RejectedDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "WeighBridgeRejectedStatusUpdate"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("Sp_WbTransation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GatepassPendingForWeighBridge(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.GpDateF))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDateF",
					Value = obj.GpDateF
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateT))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDateT",
					Value = obj.GpDateT
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoFrom",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpNoTo",
					Value = obj.GpSrNoT
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WbTransactions>("SP_GatepassPendingForWeighBridge", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void WbTransactions_WeightUpdation(ReportsParameters obj)
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
					sqlCommand.CommandText = "[dbo].[USP_WbTransactions_WeightUpdation]";
					sqlCommand.Parameters.AddWithValue("@Id", item.Id);
					sqlCommand.Parameters.AddWithValue("@UserId", item.EntryUser);
					sqlCommand.Parameters.AddWithValue("@ActionId", item.ActionId);
					sqlCommand.Parameters.AddWithValue("@Comments", item.Remarks);
					sqlCommand.Parameters.AddWithValue("@FirstWeight", item.FirstWeight);
					sqlCommand.Parameters.AddWithValue("@SecondWeight ", item.SecondWeight);
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
}
