using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.lgstcm;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.lgstcm;

namespace Architecture.BLL.lgstcm;

public class ServicesBillHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.lgstcm.ServicesBillHeader obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		VoucherHead voucher = new VoucherHead();
		try
		{
			voucher.DocumentTypeId = obj.DocumentTypeId;
			voucher.DocumentTypeSrNo = obj.ServicesBillHeaderId;
			voucher.RefDocNoId = obj.ServicesBillHeaderId;
			voucher.VoucherCode = Conversion.ToInt(obj.DocumentNo);
			voucher.VoucherDate = obj.DocumentDate;
			voucher.BranchId = obj.BranchesId;
			voucher.ProjectId = obj.ProjectsId;
			voucher.ManualBillNo = Conversion.ToString(obj.ReferenceNo);
			voucher.DueDate = obj.DocumentDate.AddDays(obj.DueDays);
			voucher.OrganizationId = obj.OrganizationId;
			voucher.CompanyId = obj.CompanyId;
			voucher.FinancialYearId = obj.FinancialYearId;
			voucher.EntryUser = obj.EntryUserId;
			voucher.ModifyUser = obj.ModifyUserId;
			voucher.EntryDate = DateTime.Now;
			voucher.ModifyDate = DateTime.Now;
			voucher.IncludeWHT = false;
			voucher.Remarks = string.Empty;
			voucher.RemarksOtherLingo = string.Empty;
			voucher.ChequeDate = DateTime.Today;
			voucher.voucherDetailList = new List<VoucherDetail>();
			SupplierCustomerGlAccountId party = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId)?.FirstOrDefault((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.BilltoPartyId));
			if (party == null)
			{
				throw new Exception("Party GL Account not found.");
			}
			voucher.RefAccountId = Conversion.ToInt(party.GlAccountId);
			List<GetItemGlIdsandItemName> itemGlList = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (itemGlList == null || itemGlList.Count == 0)
			{
				throw new Exception("Item GL mapping not found.");
			}
			double totalAmount = 0.0;
			foreach (ServicesBillDetail item in obj.ServicesBillDetailList)
			{
				int debitAccountId = ResolveDebitAccount(item, itemGlList);
				string itemName = ((itemGlList != null && itemGlList.Count > 0) ? itemGlList[0].ItemName : string.Empty);
				string VoucherComments = "";
				string baseComment = string.Join(" ", new string[6]
				{
					string.IsNullOrWhiteSpace(obj.ReferenceNo) ? null : ("Reference No: " + obj.ReferenceNo),
					string.IsNullOrWhiteSpace(itemName) ? null : ("Service Item: " + itemName),
					(item.ServiceRate == 0.0) ? null : $"Service Rate: {item.ServiceRate}",
					(item.lcyAmount == 0.0) ? null : $"Amount: {item.lcyAmount}",
					string.IsNullOrWhiteSpace(obj.ExportInvoiceNo) ? null : ("ExportInvoiceNo: " + obj.ExportInvoiceNo),
					string.IsNullOrWhiteSpace(obj.ExportInvoiceCustomerName) ? null : ("ExportInvoice_Customer: " + obj.ExportInvoiceCustomerName)
				}.Where((string x) => !string.IsNullOrWhiteSpace(x)));
				VoucherComments = (string.IsNullOrWhiteSpace(item.Remarks) ? baseComment : (item.Remarks + " " + baseComment));
				AddVoucherDetail(voucher, debitAccountId, voucher.RefAccountId, item, isDebit: true, VoucherComments);
				AddVoucherDetail(voucher, voucher.RefAccountId, debitAccountId, item, isDebit: false, VoucherComments);
				totalAmount += Conversion.ToDouble(item.TotalAmount);
			}
			voucher.VoucherAmount = totalAmount;
			voucher.BillAmount = totalAmount;
			return voucher;
		}
		catch
		{
			throw;
		}
	}

	private static int ResolveDebitAccount(ServicesBillDetail item, List<GetItemGlIdsandItemName> itemGlList)
	{
		if (item.DebitAccountId > 0)
		{
			return Conversion.ToInt(item.DebitAccountId);
		}
		return Conversion.ToInt((itemGlList.FirstOrDefault((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.serviceItemId)) ?? throw new Exception("Item GL Account not found.")).PurchaseGLAC);
	}

	private static void AddVoucherDetail(VoucherHead voucher, int accountId, int againstAccountId, ServicesBillDetail item, bool isDebit, string AutoRemarks)
	{
		VoucherDetail detail = new VoucherDetail
		{
			AccountId = accountId,
			AgainstAccountId = againstAccountId,
			Comments = (string.IsNullOrWhiteSpace(AutoRemarks) ? Conversion.ToString(item.Remarks) : AutoRemarks),
			DebitAmount = (isDebit ? Conversion.ToDouble(item.lcyAmount) : 0.0),
			CreditAmount = (isDebit ? 0.0 : Conversion.ToDouble(item.lcyAmount)),
			ItemId = Conversion.ToInt(item.serviceItemId),
			ItemRate = Conversion.ToDouble(item.ServiceRate),
			ItemAmount = Conversion.ToDouble(item.lcyAmount + item.TaxAmount * item.transactionExchangeRate),
			DMultiCurrencyId = Conversion.ToInt(item.transactionCurrencyId),
			DExchangeCurrencyRate = Conversion.ToDouble(item.transactionExchangeRate),
			DCurrencyAmount = Conversion.ToDouble(item.TotalAmount),
			ThirdCurrencyId = Conversion.ToInt(item.GlcyCurrencyId),
			ThirdCurrencyFcyExchangeRate = Conversion.ToDouble(item.GlcyExchangeRate),
			ThirdCurrencyAmount = Conversion.ToDouble(item.GlcyAmount),
			BaseFcyId = Conversion.ToInt(item.FcyCurrencyId),
			BaseFcyExchangeRate = Conversion.ToDouble(item.FcyExchangeRate),
			BaseFcyAmount = Conversion.ToDouble(item.fcyAmount)
		};
		voucher.voucherDetailList.Add(detail);
	}

	public static int Save(Architecture.Model.lgstcm.ServicesBillHeader obj)
	{
		try
		{
			obj.ActionId = ((obj.ServicesBillHeaderId == 0) ? 1 : 2);
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.lgstcm.ServicesBillHeader().SetData(obj, "[lgstcm].[USP_ServicesBillHeader_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.lgstcm.ServicesBillHeader ReadById(int Id)
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
			new List<Architecture.Model.lgstcm.ServicesBillHeader>();
			return new Architecture.DAL.lgstcm.ServicesBillHeader().GetData("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.lgstcm.ServicesBillHeader obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.ServicesBillHeader>("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocumentNo"]);
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
			sqlCommand.CommandText = "[lgstcm].[USP_ServicesBillHeader_GetAllMethod]";
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.TypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ServiceTypeId",
					Value = obj.TypeId
				});
			}
			if (obj.InvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExportInvoiceId",
					Value = obj.InvoiceId
				});
			}
			if (obj.BrokerAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BrokerAgentId",
					Value = obj.BrokerAgentId
				});
			}
			if (obj.BrokerOrServiceProviderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BrokerOrServiceProviderId",
					Value = obj.BrokerOrServiceProviderId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.ServicesBillHeader>("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromServicesBillHeader(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != string.Empty && obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[lgstcm].[USP_GetDataForDropDownFromServicesBillHeader]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ServicesBillHeader_Slip(ReportsParameters obj)
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
			DataSet ds = GenericProvider.GetDataSetProc<Architecture.Model.lgstcm.ServicesBillHeader>("[lgstcm].[USP_ServicesBillHeader_Slip]", p);
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

	public static DataTable ServicesBill_DataForApproval(ReportsParameters obj)
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
			if (obj.TypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ServiceTypeId",
					Value = obj.TypeId
				});
			}
			if (obj.InvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExportInvoiceId",
					Value = obj.InvoiceId
				});
			}
			if (obj.BillToPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillToPartyId",
					Value = obj.BillToPartyId
				});
			}
			if (obj.BrokerOrServiceProviderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BrokerOrServiceProviderId",
					Value = obj.BrokerOrServiceProviderId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.ServicesBillHeader>("[lgstcm].[USP_ServicesBill_DataForApproval]", p);
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
			using (SqlCommand cmd = new SqlCommand("[lgstcm].[USP_ServicesBillHeader_Approval]", sqlcon, sqlTrn))
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

	public static DataTable ServicesBill_Report(ReportsParameters obj)
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
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.ItemTypeOfTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeOfTypeId",
					Value = obj.ItemTypeOfTypeId
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
			if (obj.MasterItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MasterItemId",
					Value = obj.MasterItemId
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
			if (obj.ServiceTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ServiceTypeId",
					Value = obj.ServiceTypeId
				});
			}
			if (obj.EximInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExportInvoiceId",
					Value = obj.EximInvoiceId
				});
			}
			if (obj.BillToPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillToPartyId",
					Value = obj.BillToPartyId
				});
			}
			if (obj.BrokerOrServiceProviderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BrokerOrServiceProviderId",
					Value = obj.BrokerOrServiceProviderId
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
			if (!string.IsNullOrWhiteSpace(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			DataSet ds = GenericProvider.GetDataSetProc<Architecture.Model.lgstcm.ServicesBillHeader>("[lgstcm].[USP_ServicesBill_Report]", p);
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
}
