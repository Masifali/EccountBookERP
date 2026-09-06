using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Model.Production;

namespace Architecture.BLL.PartyProcessing;

public class InvProductionProcessingBill
{
	public static VoucherHead MakeVoucher(Architecture.Model.PartyProcessing.InvProductionProcessingBill obj)
	{
		List<GetItemGlIdsandItemName> dtItem = new List<GetItemGlIdsandItemName>();
		List<GetItemGlIdsandItemName> PartyProcessingItemlst = new List<GetItemGlIdsandItemName>();
		List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
		VoucherHead vh = new VoucherHead();
		try
		{
			DataTable dt = CommonServies.GetActiveFinancialYear(obj.OrganizationId, obj.CompanyId);
			if (dt.Rows.Count > 0)
			{
				Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.BillNo);
			vh.VoucherDate = obj.BillDate;
			vh.Remarks = Conversion.ToString(obj.RemarksHeader);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtItem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				List<SupplierCustomerGlAccountId> find3 = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.StockPartyId)).ToList();
				if (find3.Count <= 0)
				{
					throw new Exception("StockParty GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find3[0].GlAccountId);
				Conversion.ToString(find3[0].CompanyName);
			}
			if (obj.DocumentTypeId == 123)
			{
				VoucherDetail vd3 = new VoucherDetail();
				vd3.LineId = 0;
				vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
				vd3.AgainstAccountId = Conversion.ToInt(obj.CreditAccountId);
				vd3.Comments = Conversion.ToString(obj.RemarksHeader);
				vd3.DebitAmount = Conversion.ToDouble(obj.Amount);
				vd3.QtyIn = Conversion.ToDouble(obj.Qty);
				vd3.ItemRate = Conversion.ToDouble(obj.Rate);
				vd3.WeightIn = Conversion.ToDouble(obj.Weight);
				vd3.ItemAmount = Conversion.ToDouble(obj.Amount);
				vd3.SupplierCustomerId = Conversion.ToInt(obj.StockPartyId);
				vh.voucherDetailList.Add(vd3);
				VoucherDetail vd2 = new VoucherDetail();
				vd2.LineId = 0;
				vd2.AccountId = Conversion.ToInt(obj.CreditAccountId);
				vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd2.Comments = Conversion.ToString(obj.RemarksHeader);
				vd2.CreditAmount = Conversion.ToDouble(obj.Amount);
				vd2.QtyIn = Conversion.ToDouble(obj.Qty);
				vd2.ItemRate = Conversion.ToDouble(obj.Rate);
				vd2.ItemAmount = Conversion.ToDouble(obj.Amount);
				vd2.SupplierCustomerId = Conversion.ToInt(obj.StockPartyId);
				vh.voucherDetailList.Add(vd2);
				if (obj.InvProductionProcessingBillOHslist.Count > 0)
				{
					foreach (InvProductionProcessingBillOH item3 in obj.InvProductionProcessingBillOHslist.ToList())
					{
						VoucherDetail vd6 = new VoucherDetail();
						vd6.LineId = 0;
						vd6.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd6.AgainstAccountId = Conversion.ToInt(item3.AccountId);
						vd6.Comments = Conversion.ToString(item3.OHRemarks);
						vd6.DebitAmount = Conversion.ToDouble(item3.Amount);
						vd6.QtyIn = Conversion.ToDouble(item3.Qty);
						vd6.ItemRate = Conversion.ToDouble(item3.Rate);
						vd6.ItemAmount = Conversion.ToDouble(item3.Amount);
						vh.voucherDetailList.Add(vd6);
						VoucherDetail vd9 = new VoucherDetail();
						vd9.LineId = 0;
						vd9.AccountId = Conversion.ToInt(item3.AccountId);
						vd9.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd9.Comments = Conversion.ToString(item3.OHRemarks);
						vd9.CreditAmount = Conversion.ToDouble(item3.Amount);
						vd9.QtyIn = Conversion.ToDouble(item3.Qty);
						vd9.ItemRate = Conversion.ToDouble(item3.Rate);
						vd9.ItemAmount = Conversion.ToDouble(item3.Amount);
						vh.voucherDetailList.Add(vd9);
					}
				}
				if (obj.InvProductionProcessingBillPMslist.Count > 0)
				{
					foreach (InvProductionProcessingBillPM item2 in obj.InvProductionProcessingBillPMslist.ToList())
					{
						if (dtItem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find2 = dtItem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find2.Count > 0)
							{
								VoucherDetail vd5 = new VoucherDetail();
								vd5.LineId = 0;
								vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
								vd5.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd5.Comments = Conversion.ToString(item2.Remarks);
								vd5.DebitAmount = Conversion.ToDouble(item2.Amount);
								vd5.QtyIn = Conversion.ToDouble(item2.Qty);
								vd5.ItemRate = Conversion.ToDouble(item2.Rate);
								vd5.ItemAmount = Conversion.ToDouble(item2.Amount);
								vh.voucherDetailList.Add(vd5);
								VoucherDetail vd8 = new VoucherDetail();
								vd8.LineId = 0;
								vd8.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
								vd8.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd8.Comments = Conversion.ToString(item2.Remarks);
								vd8.CreditAmount = Conversion.ToDouble(item2.Amount);
								vd8.QtyIn = Conversion.ToDouble(item2.Qty);
								vd8.ItemRate = Conversion.ToDouble(item2.Rate);
								vd8.ItemAmount = Conversion.ToDouble(item2.Amount);
								vh.voucherDetailList.Add(vd8);
								continue;
							}
							throw new Exception("Item PurchaseGLAccountId not Found");
						}
						throw new Exception("Item PurchaseGLAccountId not Found");
					}
				}
				if (obj.InvProductionProcessingBillOutPutslist.Count > 0)
				{
					PartyProcessingItemlst = CommonServies.GetPartyProcessingItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
					foreach (InvProductionProcessingBillOutPut item in obj.InvProductionProcessingBillOutPutslist.ToList())
					{
						if (item.ItemId > 0 && item.Amount > 0m)
						{
							string Remarks = "";
							List<GetItemGlIdsandItemName> find = PartyProcessingItemlst.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
							VoucherDetail vd4 = new VoucherDetail();
							vd4.LineId = 0;
							vd4.AccountId = Conversion.ToInt(item.DebitAccountId);
							vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							Remarks = ((!(item.Remarks != string.Empty)) ? (Conversion.ToString(find[0].ItemName) + " Qty " + item.Qty + " Weight " + item.Weight + " Rate " + item.Rate + " Amount " + item.Amount) : (item.Remarks + " " + Conversion.ToString(find[0].ItemName) + " Qty " + item.Qty + " Weight " + item.Weight + " Rate " + item.Rate + " Amount " + item.Amount));
							vd4.Comments = Conversion.ToString(Remarks);
							vd4.DebitAmount = Conversion.ToDouble(item.Amount);
							vd4.QtyIn = Conversion.ToDouble(item.Qty);
							vd4.ItemRate = Conversion.ToDouble(item.Rate);
							vd4.ItemAmount = Conversion.ToDouble(item.Amount);
							vh.voucherDetailList.Add(vd4);
							VoucherDetail vd7 = new VoucherDetail();
							vd7.LineId = 0;
							vd7.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd7.AgainstAccountId = Conversion.ToInt(item.DebitAccountId);
							vd7.Comments = Conversion.ToString(Remarks);
							vd7.CreditAmount = Conversion.ToDouble(item.Amount);
							vd7.QtyIn = Conversion.ToDouble(item.Qty);
							vd7.ItemRate = Conversion.ToDouble(item.Rate);
							vd7.ItemAmount = Conversion.ToDouble(item.Amount);
							vh.voucherDetailList.Add(vd7);
							continue;
						}
						throw new Exception("By Product ItemId or Item Amount not found");
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	public static int Save(Architecture.Model.PartyProcessing.InvProductionProcessingBill obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.PartyProcessing.InvProductionProcessingBill().SetData(obj, "USP_InvProductionProcessingBill_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.PartyProcessing.InvProductionProcessingBill().SetData(obj, "USP_InvProductionProcessingBill_Update");
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteById"
			});
			GenericProvider.GetDataTableProc<InvProductionJobOrder>("USP_InvProductionProcessingBill_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PartyProcessing.InvProductionProcessingBill GetByID(int Id)
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
			new List<Architecture.Model.PartyProcessing.InvProductionProcessingBill>();
			return new Architecture.DAL.PartyProcessing.InvProductionProcessingBill().GetDate("USP_InvProductionProcessingBill_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.PartyProcessing.InvProductionProcessingBill obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvProductionProcessingBill>("USP_InvProductionProcessingBill_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["BillNo"]);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing>("[dbo].[USP_InvProductionProcessingBill_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromInvProductionProcessingBill(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != string.Empty || obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_GetDataForDropDownFromInvProductionProcessingBill]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInPutForProcessingBill(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@InvJobOrderId",
				Value = obj.JobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInPutForProcessingBill"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvProductionProcessingBill>("USP_InvProductionProcessingBill_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOutPutForProcessingBillByJobOrder(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@InvJobOrderId",
				Value = obj.JobOrderId
			});
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryType",
					Value = obj.Activity
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetOutPutForProcessingBillByJobOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvProductionProcessingBill>("USP_InvProductionProcessingBill_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvProductionProcessingBill_PartyProcessingModule_Report_611(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			return GenericProvider.GetDataTableProc<InvFoodProduction>("spInvProductionProcessingBill_PartyProcessingModule_Report", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvProductionProcessingBill_SubReport_611(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<InvFoodProduction>("spInvProductionProcessingBill_PartyProcessingModule_SubReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvProductionProcessingBillByProduct_SubReport_611(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<InvFoodProduction>("Usp_ProductionProcessingBillOutPut_SubReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProductionProcessingBill_Register(ReportsParameters obj)
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
			if (obj.InvJobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvJobOrderId",
					Value = obj.InvJobOrderId
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
			if (obj.CreditAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CreditAccountId",
					Value = obj.CreditAccountId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PartyProcessing.InvFoodProductionPartyProcessing>("[dbo].[USP_ProductionProcessingBill_Register]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
