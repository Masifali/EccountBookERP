using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory.Financials.SaleInvoice;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.ModernHistoryModels;
using Architecture.Model.POS;
using Architecture.Model.Steel.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.Inventory;

public class InvSaleInvoice
{
	public static DataTable GetTaxCompanySetupByOrgAndCompany(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.AccountNotes>("USP_GetTaxCompanySetupByOrgAndCompany", p);
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
			sqlCommand.CommandText = "[dbo].[USP_InvSaleInvoice_UpdateInvoiceUploadStatus]";
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

	public static int Save(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				if (obj.InvSaleInvoiceDetaillist.Exists((Architecture.Model.Inventory.InvSaleInvoiceDetail p) => p.Id > 0))
				{
					throw new Exception("Record cannot be inserted because detailId greater than zero");
				}
				obj.ModifyUser = 0;
				obj.VoucherHeadInvoices = SaleInvoiceFinancial.MakeVoucherForSaleInvoice(obj);
				return new Architecture.DAL.Inventory.InvSaleInvoice().SetData(obj, "Sp_InvSaleInvoice_Insert");
			}
			obj.EntryUser = 0;
			obj.VoucherHeadInvoices = SaleInvoiceFinancial.MakeVoucherForSaleInvoice(obj);
			return new Architecture.DAL.Inventory.InvSaleInvoice().SetData(obj, "Sp_InvSaleInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvSaleInvoice GetByID(int Id)
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
			return new Architecture.DAL.Inventory.InvSaleInvoice().GetData("Sp_InvSaleInvoice_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateInvSaleInvoiceCode(Architecture.Model.Inventory.InvSaleInvoice obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
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

	public static int GenerateInvSaleInvoiceCodeBranch(Architecture.Model.Inventory.InvSaleInvoice obj)
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
				Value = "GenerateBranchCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["BranchSrNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartiesFromSaleInvoiceWithGlAccount(int OrganizationId, int CompanyId, string DocumentTypeIds = "95,99,186")
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
				ParameterName = "@DocumentTypeIds",
				Value = DocumentTypeIds
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[USP_GetPartiesFromSaleInvoiceWithGlAccount]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromSaleInvoiceAgainstPartyId(int OrganizationId, int CompanyId, int PartyId, string DocumentTypeIds = "95,99,186")
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeIds",
				Value = DocumentTypeIds
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USP_GetItemsFromSaleInvoiceAgainstPartyId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateSalesTaxNo(Architecture.Model.Inventory.InvSaleInvoice obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
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

	public static int GetIdbyDocNoSaleInvoice(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
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
				Value = "GetInvSaleInvoiceIdByDocNo"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
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

	public static int GetLastSaleTaxAcId(ReportsParameters obj)
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
				Value = "GetLastSaleTaxAcId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["SaleTaxAcId"]);
			}
			return Code;
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
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

	public static DataTable GetByGdnIdForSaleInvoiceTrading(string Id)
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
				Value = "GdnIdForSaleInvoiceTrading"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvGdnNoForInvoice(Architecture.Model.Inventory.InvSaleInvoice obj)
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
				Value = "GetInvGdnNotInInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoiceLoadForSaleReturn(Architecture.Model.Inventory.InvSaleInvoice obj)
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
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SaleInvoiceLoadForSaleReturn"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoBySupplierCustomerId(Architecture.Model.Inventory.InvSaleInvoice obj)
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
				ParameterName = "@Activity",
				Value = "GetInvoiceNoBySupplierCustomerId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.PaymenetTermId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PaymentTermId",
					Value = obj.PaymenetTermId
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
			if (!string.IsNullOrEmpty(obj.ScreenName))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.ScreenName
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GdnLoadForSaleInvoice(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[usp_getGdnDataForSaleInvoiceByIds]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GdnLoadForSaleInvoiceWithoutWeighBridge(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[usp_getGdnDataForSaleInvoiceWithoutWeightBridgeByIds]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GdnLoadForSaleInvoiceDirect(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GdnLoadForSaleInvoiceDirect"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGdnPartyProcessingDetailByGdnIds(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("usp_getGdnPartyProcessingDetailByGdnIds", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GrnLoadForPurchaseInvoiceTrading(int Id)
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
				Value = "GrnLoadForPurchaseInvoiceTrading"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvPurchaseInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetTransporterAndFreightFromGdn(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetTransporterAndFreightFromGdn"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExpensesFromGdn(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[dbo].[USP_InvGdnExpensesByInvGdnIdIds]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemQtyAndItemRateFromSO(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetItemQtyAndItemRateFromSO"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoiceExpense_ByIds(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SaleInvoiceExpense_ByIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoiceFreight_ByIds(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "SaleInvoiceFreight_ByIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvSaleInvoiceJournal_ByIds(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "InvSaleInvoiceJournal_ByIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWeightCurrStockByItem(ReportsParameters obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
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
			if (obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWeightCurrStockByItem"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetQtyCurrStockByItem(ReportsParameters obj)
	{
		DataTable dt = null;
		try
		{
			double AvailableStock = 0.0;
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
				ParameterName = "@WarehouseId",
				Value = obj.WarehouseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobLotId",
				Value = obj.JobLotId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetQtyCurrStockByItem"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				AvailableStock = Conversion.ToDouble(dt.Rows[0]["AvailableStock"]);
			}
			return AvailableStock;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetQtyAvgRateByItem(ReportsParameters obj)
	{
		DataTable dt = null;
		try
		{
			double AvgRate = 0.0;
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
				ParameterName = "@WarehouseId",
				Value = obj.WarehouseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobLotId",
				Value = obj.JobLotId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetQtyAvgRateByItem"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				AvgRate = Conversion.ToDouble(dt.Rows[0]["AvgRate"]);
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingGdnforLoadingInvoice(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
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
			if (obj.GdnIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GdnIds",
					Value = obj.GdnIds
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingGdnForSaleInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingGdnforLoadingInvoiceRice(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingGdnForSaleInvoiceRice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("Sp_InvSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGdnDataForSaleInvoiceStoreAndPm(ReportsParameters obj)
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
			if (obj.OrderCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@OrderCategoryId",
					Value = obj.OrderCategoryId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("usp_getGdndataForSaleInvoice_Store", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingTradeGdnForTradeAndSaleInvoice(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("usp_PendingTradeGdnForTradeAndSaleInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingGDNForSaleInvoice_Engr(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USP_GetPendingGDNForSaleInvoice_Engr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingInvSaleInvoiceForApproval(ReportsParameters obj)
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
			if (obj.BaseDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BaseDocumentTypeId",
					Value = obj.BaseDocumentTypeId
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USp_PendingInvSaleInvoiceForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvSaleInvoiceandVoucherApprove(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "InvSaleInvoiceandVoucherApprove"
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
				GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_InvSaleInvoice_GetAllMethod", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleInvoiceReturnForApproval(ReportsParameters obj)
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
				Value = "GetSaleInvoiceReturnForApproval"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_InvSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoiceTradeProRpt(Architecture.Model.Inventory.InvSaleInvoiceSlip obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("sp_InvSaleInvoiceTradePro_Slip", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingGdnForSaleInvoiceTrading(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingGdnForSaleInvoiceTrading"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TradingFormHistory(ReportsParameters obj)
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
				Value = "TradingFormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", p);
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
			DataSet ds = GenericProvider.GetDataSetProc<PosSaleInvoiceDetail>("Sp_InvSaleInvoice_GetAllMethod", p);
			List<Purchase_Sale_InvoiceHistory> History = new List<Purchase_Sale_InvoiceHistory>();
			List<Prucahse_Slae_InvoiceHistoryHeader> HistoryHeaderRecords = Conversion.ConvertDataTableToList<Prucahse_Slae_InvoiceHistoryHeader>(ds.Tables[0]);
			List<Puchase_Sale_InvoiceHistoryDetail> HistoryDetailRecords = Conversion.ConvertDataTableToList<Puchase_Sale_InvoiceHistoryDetail>(ds.Tables[1]);
			List<ChargeToProductDetailForInvoiceHistory> ChargeToProductRecords = Conversion.ConvertDataTableToList<ChargeToProductDetailForInvoiceHistory>(ds.Tables[2]);
			List<Customer_Supplier_AddLessDetailForInvoiceHistory> Customer_SupplierAddLessRecords = Conversion.ConvertDataTableToList<Customer_Supplier_AddLessDetailForInvoiceHistory>(ds.Tables[3]);
			List<OtherExpensedDetailForInvoiceHistory> OtherExpensesRecords = Conversion.ConvertDataTableToList<OtherExpensedDetailForInvoiceHistory>(ds.Tables[4]);
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
				Record.EmptyBagsDetailPresent = false;
				History.Add(Record);
			}
			return History;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHistoryForPdfReport_HeaderData(ReportsParameters obj)
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
			if (obj.Id > 0)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetHistoryForPdfReport_HeaderData"
			});
			return GenericProvider.GetDataTableProc<PosSaleInvoiceDetail>("Sp_InvSaleInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllComboBindAgainstSaleInvoice(ReportsParameters obj)
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
				ParameterName = "@AppId",
				Value = obj.AppId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[Usp_AllComboAgainstSaleInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBranchsAllocatedToUserFromSaleInvoice(int OrganizationId, int CompanyId, int UserId, int DocumentTypeId = 0)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.BranchesAllocationToUser>("[dbo].[USP_GetBranchsAllocatedToUserFromSaleInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoiceSlip_Engr(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Steel.Inventory.InvSaleInvoice>("[dbo].[Sp_InvSaleInvoice_Slip_Eng]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGdnLoad_Eng(string Ids)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@GdnIds",
				Value = Ids
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[USP_GetGdnLoad_Eng]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveByID(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		try
		{
			DataTable lst = Architecture.BLL.Accounts.VoucherHead.RecordCheckTables(new ReportsParameters
			{
				OrganizationId = obj.OrganizationId,
				CompanyId = obj.CompanyId,
				DocumentTypeId = obj.DocumentTypeId,
				Id = obj.Id
			});
			if (lst.Rows.Count > 0)
			{
				if (Conversion.ToInt(lst.Rows[0]["RefId"].ToString()) > 0)
				{
					throw new Exception("Record Not Update beacuase Record has Exist");
				}
			}
			else
			{
				new Architecture.DAL.Inventory.InvSaleInvoice().AccountandInventoryRemoveById(obj);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleOrderPaymentTermDetailBySoIds(string Ids)
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
				Value = "SaleOrderPaymentTermDetailBySoIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[Sp_SaleOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCommissionRateByCustomerAndItemIdSFromCommPolicy(ReportsParameters obj)
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
				ParameterName = "@CustomerId",
				Value = obj.CustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemIds",
				Value = obj.ItemIds
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EffectiveDate",
				Value = obj.EffectedDate
			});
			if (obj.PolicyTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PolicyTypeId",
					Value = obj.PolicyTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[USP_GetCommissionRateByCustomerAndItemIdFromCommPolicy]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getSaleDataForMobileApp(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGdn>("USP_getSaleDataForMobileApp", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getPendingDeliveryForBills(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGdn>("USP_getPendingDeliveryForBills", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleAnalyticsA_Report(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter("@SupplierCustomerId", obj.SupplierCustomerId));
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchId", obj.BranchesId));
			}
			p.Add(new SqlParameter("@FromDate", obj.FromDate));
			p.Add(new SqlParameter("@ToDate", obj.ToDate));
			p.Add(new SqlParameter("@SeasonStart", obj.StartDate));
			p.Add(new SqlParameter("@SeasonEnd", obj.EndDate));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_SaleAnalyticsA_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleAnalyticsAB_Report(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_SaleAnalyticsAB_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvoiceStatusUpdate(int Id, string CustomerRemarks)
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
				ParameterName = "@CustomerRemarks",
				Value = CustomerRemarks
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGdn>("USP_InvoiceStatusUpdate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet getSaleDataForVoucherAndStockEffects(int OrganizationId, int CompanyId, int DocumentTypeId, int FinancialYearId, DateTime AsOnDate)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@AsOnDate",
				Value = AsOnDate
			});
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.InvSaleInvoice>("usp_getSaleDataForVoucherAndStockEffects", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet getSaleDataForCgsUpdate(int OrganizationId, int CompanyId, int FinancialYearId, DateTime FromDate, DateTime ToDate, int ActionId)
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
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FromDate",
				Value = FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = ToDate
			});
			if (ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = ActionId
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.InvSaleInvoice>("usp_getSaleDataForCgsUpdate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet getExportDataForCgsUpdate(int OrganizationId, int CompanyId, int FinancialYearId, DateTime FromDate, DateTime ToDate, int ActionId)
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
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FromDate",
				Value = FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = ToDate
			});
			if (ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = ActionId
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[usp_getEXportDataForCgsUpdate]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AutoVoucherAndStockEffects(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		try
		{
			new Architecture.DAL.Inventory.InvSaleInvoice().AutoVoucherAndStockEffects(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingSaleInvoiceForReturnInvoice(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USP_PendingSaleInvoiceForReturnInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoice_PendingForReturnEngr(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Mfg].USP_SaleInvoice_PendingForReturnEngr", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSaleInvoiceIdsForAutoUpdation(int OrganizationId, int CompanyId, int FinancialYearId, int DocumentTypeId)
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
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvStockTransferHeader>("[dbo].[USP_GetSaleInvoiceIdsForAutoUpdation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockRateUpdateFromJobOrder(List<InvoiceInfo> lst)
	{
		try
		{
			foreach (InvoiceInfo item in lst.ToList())
			{
				List<SqlParameter> p = new List<SqlParameter>();
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
					ParameterName = "@RefDocumentTypeId",
					Value = item.DocumentTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocIdNo",
					Value = item.Id
				});
				if (item.ActionId != 0)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@ActionId",
						Value = item.ActionId
					});
				}
				GenericProvider.GetDataTableProc<object>("[dbo].[usp_StockRateUpdateFromJobOrder]", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingReservedSaleInvoiceForAdvanceDeliveryOrder(ReportsParameters obj)
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
			if (obj.InvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceId",
					Value = obj.InvoiceId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[dbo].[usp_PendingReservedSaleInvoiceForAdvanceDeliveryOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoicePendingForUpload(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("USP_SaleInvoicePendingForUpload", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
