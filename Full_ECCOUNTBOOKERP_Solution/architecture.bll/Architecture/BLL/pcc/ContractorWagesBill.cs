using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class ContractorWagesBill
{
	public static VoucherHead MakeVoucher(Architecture.Model.pcc.ContractorWagesBill obj)
	{
		VoucherHead vh = new VoucherHead
		{
			DocumentTypeId = obj.DocumentTypeId,
			DocumentTypeSrNo = obj.Id,
			RefDocNoId = obj.Id,
			VoucherCode = Conversion.ToInt(obj.DocNo),
			VoucherDate = obj.DocDate,
			Remarks = Conversion.ToString(obj.RemarksHeader),
			RemarksOtherLingo = string.Empty,
			ChequeDate = DateTime.Today,
			IncludeWHT = false,
			BranchId = obj.BranchId,
			ProjectId = obj.ProjectId,
			BillAmount = 0.0,
			ManualBillNo = Conversion.ToString(obj.RefDocNo),
			DueDate = obj.DocDate,
			DueDays = 0,
			OrganizationId = obj.OrganizationId,
			CompanyId = obj.CompanyId,
			FinancialYearId = obj.FinancialYearId,
			EntryUser = obj.EntryUserId,
			EntryDate = DateTime.Now,
			ModifyDate = DateTime.Now,
			ModifyUser = obj.ModifyUserId,
			voucherDetailList = new List<VoucherDetail>()
		};
		try
		{
			List<SupplierCustomerGlAccountId> dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			List<Architecture.Model.pcc.ContractorWagesActivity> dtitem = null;
			List<GetItemGlIdsandItemName> itemlst = null;
			bool isManualWages = obj.DocumentTypeId == 1868 || obj.DocumentTypeId == 1869;
			bool num = obj.RefDocumentTypeId == 1855 && obj.DocumentTypeId == 1869;
			bool chargeToProduct = false;
			if (num)
			{
				itemlst = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				chargeToProduct = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractWagesChargetoProduct"));
			}
			if (dtitem == null)
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
				parameters.Add(new SqlParameter("@CompanyId", obj.CompanyId));
				parameters.Add(new SqlParameter("@BranchesId", obj.BranchId));
				parameters.Add(new SqlParameter("@Activity", "FormHistory"));
				dtitem = new Architecture.DAL.pcc.ContractorWagesActivity().GetData("pcc.USP_ContractorWagesActivity_GetAllMethod", parameters);
			}
			IEnumerable<ContractorWagesBillDetail> DetailList = obj.ContractorWagesBillDetailList.Where((ContractorWagesBillDetail rrr) => rrr.ActionTypeId != 3);
			if (isManualWages && DetailList.Any())
			{
				foreach (ContractorWagesBillDetail item in DetailList)
				{
					int debitAccountId = 0;
					if (chargeToProduct)
					{
						GetItemGlIdsandItemName itemGl = itemlst?.FirstOrDefault((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId));
						if (itemGl == null)
						{
							throw new Exception($"ItemId [{item.ItemId}] GL Account not found.");
						}
						debitAccountId = Conversion.ToInt(itemGl.PurchaseGLAC);
					}
					if (debitAccountId == 0)
					{
						int ActivityId = item.ContractorWagesActivityId;
						Architecture.Model.pcc.ContractorWagesActivity act = dtitem.FirstOrDefault((Architecture.Model.pcc.ContractorWagesActivity u) => u.Id == ActivityId);
						if (act == null)
						{
							throw new Exception("ContractorWagesActivity [" + item.ContractorWagesActivity + "] GL Account not found.");
						}
						debitAccountId = Conversion.ToInt(act.GlAccountId);
					}
					if (debitAccountId == 0)
					{
						throw new Exception("Debit AccountId not resolved.");
					}
					SupplierCustomerGlAccountId supplier = dtSupplier.FirstOrDefault((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.ContractorId));
					if (supplier == null)
					{
						throw new Exception("Contractor AccountId not resolved.");
					}
					string remarks = $"{obj.RefDocumentTypeId}: {item.ContractorWagesActivity} | " + $"Item: {item.ItemName} | Qty: {item.Qty} | Rate: {item.Rate} | Amount: {item.Amount}";
					if (item.AddLess > 0m)
					{
						remarks += $" | AddLess: {item.AddLess} | NetAmount: {item.NetAmount}";
					}
					double amount = (item.FreeOfCost ? 0.0 : Conversion.ToDouble(item.NetAmount));
					double rate = Conversion.ToDouble(item.Rate);
					vh.voucherDetailList.Add(new VoucherDetail
					{
						AccountId = debitAccountId,
						AgainstAccountId = Conversion.ToInt(supplier.GlAccountId),
						Comments = remarks,
						DebitAmount = amount,
						CreditAmount = 0.0,
						ItemAmount = amount,
						ItemRate = rate
					});
					vh.voucherDetailList.Add(new VoucherDetail
					{
						AccountId = Conversion.ToInt(supplier.GlAccountId),
						AgainstAccountId = debitAccountId,
						Comments = remarks,
						DebitAmount = 0.0,
						CreditAmount = amount,
						ItemAmount = amount,
						ItemRate = rate,
						SubsidiaryTypeId = 1,
						SubsidiaryAccountId = Conversion.ToInt(item.ContractorId),
						SupplierCustomerId = Conversion.ToInt(item.ContractorId)
					});
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("MakeVoucher Error: " + ex.Message, ex);
		}
		return vh;
	}

	public static int Save(Architecture.Model.pcc.ContractorWagesBill obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				obj.VoucherHeadForContractorWages = MakeVoucher(obj);
				return new Architecture.DAL.pcc.ContractorWagesBill().SetData(obj, "pcc.USP_ContractorWagesBill_InsertAndUpdate");
			}
			obj.ActionId = 2;
			obj.VoucherHeadForContractorWages = MakeVoucher(obj);
			return new Architecture.DAL.pcc.ContractorWagesBill().SetData(obj, "pcc.USP_ContractorWagesBill_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.pcc.ContractorWagesBill obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("pcc.USP_ContractorWagesBill_GetAllMethod", p);
			if (dt.Rows.Count > 0)
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

	public static Architecture.Model.pcc.ContractorWagesBill ReadById(int Id)
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
			new List<Architecture.Model.pcc.ContractorWagesBill>();
			return new Architecture.DAL.pcc.ContractorWagesBill().GetData("pcc.USP_ContractorWagesBill_GetAllMethod", p)[0];
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
			sqlCommand.CommandText = "[pcc].[USP_ContractorWagesBill_GetAllMethod]";
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

	public static DataTable RefDocNoBySupplierCustomerIdAndRefDocNoId(ReportsParameters obj)
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.SaleOrder>("[pcc].[USP_GetRefDocNoBySupplierCustomerIdAndRefDocNoIdForWages]", p);
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
				ParameterName = "@BranchId",
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[pcc].[USP_ContractorWagesBill_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllComboAgainstContractorWagesBill(ReportsParameters obj)
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
			if (obj.Activity != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[pcc].[Usp_AllComboAgainstContractorWagesBill]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractorWagesBill_Slip(ReportsParameters obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.SaleOrder>("[pcc].[USP_ContractorWagesBill_Slip]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContractorWagesBillIdsForAutoUpdation(int OrganizationId, int CompanyId, int DocumentTypeId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvStockTransferHeader>("[pcc].[USP_GetContractorWagesBillIdsForAutoUpdation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
