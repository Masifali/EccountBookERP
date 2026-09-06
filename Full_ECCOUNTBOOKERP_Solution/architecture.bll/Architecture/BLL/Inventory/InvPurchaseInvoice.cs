using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.ModernHistoryModels;
using Architecture.Model.POS;

namespace Architecture.BLL.Inventory;

public class InvPurchaseInvoice
{
	public static int Save(Architecture.Model.Inventory.InvPurchaseInvoice obj)
	{
		try
		{
			obj.VoucherHeadInvoices = PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice(obj);
			if (obj.Id == 0)
			{
				if (obj.invPurchaseInvoiceDetailList.Exists((InvPurchaseInvoiceDetail p) => p.Id > 0))
				{
					throw new Exception("Record cannot be inserted because detailId greater than zero");
				}
				obj.ActionId = 1;
				return new Architecture.DAL.Inventory.InvPurchaseInvoice().SetData(obj, "Sp_InvPurchaseInvoice_Insert");
			}
			obj.PreviousDate = GetRecordsById(obj.Id);
			obj.ActionId = 2;
			return new Architecture.DAL.Inventory.InvPurchaseInvoice().SetData(obj, "Sp_InvPurchaseInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvPurchaseInvoice GetByID(int Id)
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
			return new Architecture.DAL.Inventory.InvPurchaseInvoice().GetDate("Sp_InvPurchaseInvoice_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockInReferenceValidationReferredOrNot(int OrganizationId, int CompanyId, int DocumentTypeId, int Id, int DetailId)
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
			sqlCommand.CommandText = "[dbo].[usp_StockInReferenceValidationReferredOrNot]";
			sqlCommand.Parameters.AddWithValue("@OrganizationId", OrganizationId);
			sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
			sqlCommand.Parameters.AddWithValue("@DocumentTypeId ", DocumentTypeId);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@DetailId", DetailId);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static void SoftDeleteByID(int Id, int EntryUserId)
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
			sqlCommand.CommandText = "[dbo].[USP_InvPurchaseInvoice_DeleteById]";
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable GetPurchaseInvoiceDetailById(int Id)
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
				Value = "PurchaseDetailReadByInvPurchaseInvoiceId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateInvPurchaseInvoiceCode(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
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

	public static int GenerateInvPurchaseInvoiceCodeBranch(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateBranchSrNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["BranchSrNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateBranchCode(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateBranchSrNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["BranchSrNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateSalesTaxNo(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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
				ParameterName = "@IsTaxable",
				Value = obj.IsTaxable
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateSalesTaxNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["SalesTaxNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetIdbyDocNoPurchaseInvoice(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvPurhcaseInvoiceIdByDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvPurchaseInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return Code;
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoice_PM_FormHistory(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("USP_PurchaseInvoice_PM_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExpenseDetailFromPOAndPreBill(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[USP_GetExpenseDetailFromPOAndPreBill]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GrnLoadForPurchaseInvoice(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GrnIds",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("usp_GrnLoadForPurchaseInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWagesAmountByRefDocumentTypeIdandRefIds(int OrganizationId, int CompanyId, int RefDocumentTypeId, string RefIds)
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
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefIds",
				Value = RefIds
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("USP_GetWagesAmountByRefDocumentTypeIdandRefIds", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GrnLoadForPaddyPurchaseByOrder_GrnId(int GrnId, int OrderId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GrnId",
				Value = GrnId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderId",
				Value = OrderId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[USP_GrnLoadForPaddyPurchaseByOrder_GrnId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DirectGrnLoadForPurchaseInvoiceDirect(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DirectGrnLoadForPurchaseInvoiceDirect"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GrnLoadForPurchaseInvoiceTrading(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GrnLoadForPurchaseInvoiceTrading"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetTransporterAndFreightFromGrn(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetTransporterAndFreightFromGrn"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvGrn_GetTransporterAndFreight(string GdnIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = GdnIds
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[dbo].[USP_InvGrn_GetTransporterAndFreight]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataRateCutInCaseOfAccessWeightReceived(int OrganizationId, int CompanyId, string Id)
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
				ParameterName = "@GdnIds",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[dbo].[USP_GetDataRateCutInCaseOfAccessWeightReceived]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetEmptyBagsFromGrn(string Id, int OrderId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderId",
				Value = OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEmptyBagsFromGrn"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseOrderExpensesChargToProdut(int PurchaseOrderId, string PoIds = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (PurchaseOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = PurchaseOrderId
				});
			}
			if (PoIds != null && PoIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@POIds",
					Value = PoIds
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPurchaseOrderExpensesChargToProdut"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemQtyAndItemRateFromPO(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemQtyAndItemRateFromPO"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseOrderSupplierExpenseByGdnIds(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadPurchaseOrderSupplierExpenseByGdnIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseOrderSupplierExpenseByOrderIds(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PurchaseOrderSupplierExpenseByOrderIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingGrnforLoadingInvoice(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("usp_getGrnLoaderDataForPurchaseInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingMarketGrnForPurchaseInvoice(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingMarketGrnForPurchaseInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingGrnForPurchaseInvoiceTrading(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingGrnForPurchaseInvoiceTrading"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingGhallaMandiGrnForInvoice(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.GhallaMandiId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GhallaMandiId",
					Value = obj.GhallaMandiId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingGhallaMandiGrnForInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GrnGhallaMandiLoadForInvoice(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GrnGhallaMandiLoadForInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseOrderLoadForPurchaseInvoice(ReportsParameters obj)
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.DocNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNo",
					Value = obj.DocNo
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
			p.Add(new SqlParameter
			{
				ParameterName = "@POIds",
				Value = obj.Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PurchaseOrderLoadForPurchaseInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseDetailInvoicesForProductionIssuence(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.CompanyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoicesDetailForProductionIssence"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvPurchaseInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInventoryStockEvalautionDetail_GetAvgRates(ReportsParameters inv)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (inv.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = inv.BranchesId
				});
			}
			if (inv.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = inv.ProjectsId
				});
			}
			if (inv.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = inv.ItemId
				});
			}
			if (inv.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = inv.CropYear
				});
			}
			if (inv.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = inv.JobLotId
				});
			}
			if (!Conversion.CheckDateTimeNull(inv.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateFrom",
					Value = inv.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(inv.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo",
					Value = inv.ToDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = inv.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = inv.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAvgRates"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InventoryStockEvalautionDetail_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingInvPurchaseInvoiceForApproval(ReportsParameters obj)
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
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
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("USP_PendingInvPurchaseInvoiceForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvPurchaseInvoiceandVoucherApprove(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "InvPurchaseInvoiceandVoucherApprove"
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = item.DocumentTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@PostDate",
					Value = item.PostDate
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqType",
					Value = item.ReqType
				});
				GenericProvider.GetDataTableProc<VoucherHead>("Sp_InvPurchaseInvoice_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartiesFromPurchaseInvoiceStoreWithGlAccount(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[USP_GetPartiesFromPurchaseInvoiceStoreWithGlAccount]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromPurchaseInvoiceStoreAgainstPartyId(int OrganizationId, int CompanyId, int PartyId, string DocumentTypeIds = null)
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
				ParameterName = "@SupplierCustomerId ",
				Value = PartyId
			});
			if (DocumentTypeIds != null && DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[USP_GetItemsFromPurchaseInvoiceStoreAgainstPartyId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartiesAndItemFromPurchaseInvoiceStoreOrPM(int OrganizationId, int CompanyId, string DocumentTypeIds = null)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[USP_GetPartiesAndItemFromPurchaseInvoiceStoreOrPM]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LoadGrnForStorePurchaseInvoiceByGrnIds(string Id, DateTime EffectedDate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			if (!Conversion.CheckDateTimeNull(EffectedDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EffectedDate",
					Value = EffectedDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "LoadGrnForStorePurchaseInvoiceByGrnIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable LoadGrnForPackingMaterialInvoiceByGrnIds(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("USP_LoadGrnForPackingMaterialInvoiceByGrnIds", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveByID(Architecture.Model.Inventory.InvPurchaseInvoice obj)
	{
		try
		{
			new Architecture.DAL.Inventory.InvPurchaseInvoice().AccountandInventoryRemoveById(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryTradePro(ReportsParameters obj)
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
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistoryTradePro"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForPurchaseInvoiceRegisterDropdownBind(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GetDataForPurchaseInvoiceRegisterDropdownBind"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseInvoiceReturnForApproval(ReportsParameters obj)
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
				Value = "GetPurchaseInvoiceReturnForApproval"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvPurchaseInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GLItemQuantativeLedger(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_GeneralLedger_GLItemQuantative", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Purchase_Sale_InvoiceHistory> InoviceHistoryForCustomPdfReport(int Id)
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
				Value = "GetHistoryForPdfReport"
			});
			DataSet ds = GenericProvider.GetDataSetProc<PosSaleInvoiceDetail>("Sp_InvPurchaseInvoice_GetAllMethod", p);
			List<Purchase_Sale_InvoiceHistory> History = new List<Purchase_Sale_InvoiceHistory>();
			List<Prucahse_Slae_InvoiceHistoryHeader> HistoryHeaderRecords = Conversion.ConvertDataTableToList<Prucahse_Slae_InvoiceHistoryHeader>(ds.Tables[0]);
			List<Puchase_Sale_InvoiceHistoryDetail> HistoryDetailRecords = Conversion.ConvertDataTableToList<Puchase_Sale_InvoiceHistoryDetail>(ds.Tables[1]);
			List<ChargeToProductDetailForInvoiceHistory> ChargeToProductRecords = Conversion.ConvertDataTableToList<ChargeToProductDetailForInvoiceHistory>(ds.Tables[2]);
			List<Customer_Supplier_AddLessDetailForInvoiceHistory> Customer_SupplierAddLessRecords = Conversion.ConvertDataTableToList<Customer_Supplier_AddLessDetailForInvoiceHistory>(ds.Tables[3]);
			List<OtherExpensedDetailForInvoiceHistory> OtherExpensesRecords = Conversion.ConvertDataTableToList<OtherExpensedDetailForInvoiceHistory>(ds.Tables[4]);
			List<EmtpyBagsDetailForInvoiceHistory> EmptyBagsRecords = Conversion.ConvertDataTableToList<EmtpyBagsDetailForInvoiceHistory>(ds.Tables[5]);
			int i;
			for (i = 0; i < HistoryHeaderRecords.Count; i++)
			{
				Purchase_Sale_InvoiceHistory Record = new Purchase_Sale_InvoiceHistory();
				if (HistoryHeaderRecords[i].ComissionAgentId > 0 || HistoryHeaderRecords[i].ComissionAmount > 0.0)
				{
					HistoryHeaderRecords[i].ComissionPresent = true;
				}
				else
				{
					HistoryHeaderRecords[i].ComissionPresent = false;
				}
				if (HistoryHeaderRecords[i].BrokerAgentId > 0 || HistoryHeaderRecords[i].BrokeryAmount > 0.0)
				{
					HistoryHeaderRecords[i].BorkeryPresent = true;
				}
				else
				{
					HistoryHeaderRecords[i].BorkeryPresent = false;
				}
				Record.InvoiceHeader = HistoryHeaderRecords[i];
				Record.InvoiceDetail = HistoryDetailRecords.FindAll((Puchase_Sale_InvoiceHistoryDetail item) => item.InvoiceId == HistoryHeaderRecords[i].InvoiceId);
				Record.ChargeToProductDetail = ChargeToProductRecords.FindAll((ChargeToProductDetailForInvoiceHistory item) => item.InvoiceId == HistoryHeaderRecords[i].InvoiceId);
				if (Record.ChargeToProductDetail.Count > 0)
				{
					Record.ChargeToProductDetailPresent = true;
				}
				else
				{
					Record.ChargeToProductDetailPresent = false;
				}
				Record.Customer_Supplier_AddLessDetail = Customer_SupplierAddLessRecords.FindAll((Customer_Supplier_AddLessDetailForInvoiceHistory item) => item.InvoiceId == HistoryHeaderRecords[i].InvoiceId);
				if (Record.Customer_Supplier_AddLessDetail.Count > 0)
				{
					Record.Customer_Supplier_AddLessDetailPresent = true;
				}
				else
				{
					Record.Customer_Supplier_AddLessDetailPresent = false;
				}
				Record.OtherExpensesDetail = OtherExpensesRecords.FindAll((OtherExpensedDetailForInvoiceHistory item) => item.InvoiceId == HistoryHeaderRecords[i].InvoiceId);
				if (Record.OtherExpensesDetail.Count > 0)
				{
					Record.OtherExpensesDetailPresent = true;
				}
				else
				{
					Record.OtherExpensesDetailPresent = false;
				}
				Record.EmptyBagsDetail = EmptyBagsRecords.FindAll((EmtpyBagsDetailForInvoiceHistory item) => item.InvoiceId == HistoryHeaderRecords[i].InvoiceId);
				if (Record.EmptyBagsDetail.Count > 0)
				{
					Record.EmptyBagsDetailPresent = true;
				}
				else
				{
					Record.EmptyBagsDetailPresent = false;
				}
				History.Add(Record);
			}
			return History;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLastInvoiceTransportAndBrokerAgentId(ReportsParameters obj)
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
				Value = "GetLastInvoiceTransportAndBrokerAgentId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvoiceHistoryHeaderForCustomPdfReport(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetHistoryForPdfReport_HeaderData"
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.NoOfRecords > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[dbo].[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DateTime GetRecordsById(int Id)
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
				Value = "GetRecordId"
			});
			return Conversion.ToDateTime(GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvPurchaseInvoice_GetAllMethod", p).Rows[0][0]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllComboBindAgainstPurchaseInvoice(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[dbo].[Usp_AllComboAgainstPurchaseInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoice_GetVendorsAndCustomers(int OrganizationId, int CompanyId, int PartyTypeId = 0, string DocumentTypeIds = null)
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
			if (PartyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyTypeId",
					Value = PartyTypeId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[dbo].[USP_PurchaseInvoice_GetVendorsAndCustomers]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet InventoryPurchsaeComparisonsMonthWise(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Organizationid",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@companyId",
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
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.MonthValue > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ByMonth",
					Value = obj.MonthValue
				});
			}
			if (obj.InventoryParentCategories > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.ItemClassGroupId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemClassGroupId",
					Value = obj.ItemClassGroupId
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.InvPurchaseInvoice>("SPU_InventoryPurchsaeComparisonsMonthWise", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GrnLoadForPurchaseInvoice_Engr(string Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GrnIds",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("UPS_GrnLoadForPurchaseInvoice_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetLastPurchaseTaxAcId(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GetLastPurchaseTaxAcId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvPurchaseInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["PurchaseTaxAcId"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountsWithSubsidoryForInvoice(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			return GenericProvider.GetDataTableProc<ChartofAccount>("[dbo].[Usp_GetAccountsWithSubsidoryForInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoice_WithActivitiesRegister_Engr(ReportsParameters obj)
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
			if (obj.ItemClassGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ClassGroupId",
					Value = obj.ItemClassGroupId
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.ParentCategoryId
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.ApprovedFilter == "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipReturn",
					Value = obj.IsApproved
				});
			}
			if (obj.RateUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackUomId",
					Value = obj.RateUOM
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
			if (obj.PaymenetTermId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PaymentTermId",
					Value = obj.PaymenetTermId
				});
			}
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityName",
					Value = obj.ReportType
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PurchaseReportWithActivitiesFromEvaulations(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.ParentCategoryId
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
			if (obj.ItemClassGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemClassGroupId",
					Value = obj.ItemClassGroupId
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
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
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
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.DistrictId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DistrictId",
					Value = obj.DistrictId
				});
			}
			if (obj.AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsCityAdd",
					Value = obj.AppId
				});
			}
			if (obj.PackUomTo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsPackSizeAdd",
					Value = obj.PackUomTo
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
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityName",
					Value = obj.ReportType
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PurchaseStockAccountId",
					Value = obj.AccountId
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
			if (obj.ParentCategoryIds != null && obj.ParentCategoryIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryIds",
					Value = obj.ParentCategoryIds
				});
			}
			if (obj.CustomGroupIds != null && obj.CustomGroupIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomGroupIds",
					Value = obj.CustomGroupIds
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PurchaseAnalyticsA_Report(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchId", obj.BranchesId));
			}
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			p.Add(new SqlParameter("@SeasonStart", obj.StartDate));
			p.Add(new SqlParameter("@SeasonEnd", obj.EndDate));
			return GenericProvider.GetDataTableProc<ChartofAccount>("USP_PurchaseAnalyticsA_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseAnalyticsAB_Report(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchId", obj.BranchesId));
			}
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			p.Add(new SqlParameter("@SeasonStart", obj.StartDate));
			p.Add(new SqlParameter("@SeasonEnd", obj.EndDate));
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter("@ParentCategoryIdsForReports", obj.Ids));
			}
			if (obj.SortNo != 0)
			{
				p.Add(new SqlParameter("@SortNo", obj.SortNo));
			}
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			p.Add(new SqlParameter("@Activity", obj.Activity));
			return GenericProvider.GetDataTableProc<ChartofAccount>("USP_PurchaseAnalyticsAB_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoice_ReportSalt(ReportsParameters obj)
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
					ParameterName = "@datefrom",
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
			if (obj.BillNoFrom != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillNoFrom",
					Value = obj.BillNoFrom
				});
			}
			if (obj.BillNoTo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillNoTo",
					Value = obj.BillNoTo
				});
			}
			if (obj.ItemClassId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemClassId",
					Value = obj.ItemClassId
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
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.CommissionAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CommissionAgentId",
					Value = obj.CommissionAgentId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@ReportType",
				Value = obj.ReportType
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_PurchaseInvoice_ReportSalt]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable PurchaseFromEvaulationsWithActivitiesReportSalt(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.ParentCategoryId
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
			if (obj.ItemClassGroupId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemClassGroupId",
					Value = obj.ItemClassGroupId
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
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
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
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.DistrictId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DistrictId",
					Value = obj.DistrictId
				});
			}
			if (obj.AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsCityAdd",
					Value = obj.AppId
				});
			}
			if (obj.PackUomTo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsPackSizeAdd",
					Value = obj.PackUomTo
				});
			}
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityName",
					Value = obj.ReportType
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetBranchsAllocatedToUserFromPurchaseInvoice(int OrganizationId, int CompanyId, int UserId, int DocumentTypeId = 0)
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
				ParameterName = "@UserId",
				Value = UserId
			});
			if (DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.BranchesAllocationToUser>("[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetLastDiscountAcId(ReportsParameters obj)
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
				ParameterName = "@Activity",
				Value = "GetLastDiscountAcId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvPurchaseInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["DiscountAccountId"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingPurchaseInvoiceForReturnInvoice(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.DocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USP_PendingPurchaseInvoiceForReturnInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoice_GetLastRecordsByItem(ReportsParameters obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[USP_InvPurchaseInvoice_GetLastRecordsByItem]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoicePendingForUpload(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USP_PurchaseInvoicePendingForUpload", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateInvoiceUploadStatus(int UploadedById, int Id, bool IsUploaded = true)
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
			sqlCommand.CommandText = "[dbo].[USP_InvPurchaseInvoice_UpdateUploadStatus]";
			sqlCommand.Parameters.AddWithValue("@UploadedById", UploadedById);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@IsUploaded", IsUploaded);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable PaymentTermDetailByPoIds(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PurchaseOrderPaymentTermDetailByPoIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingPurchaseInvoiceStoreForReturnInvoice(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.DocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USP_PendingPurchaseInvoiceStoreForReturnInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
