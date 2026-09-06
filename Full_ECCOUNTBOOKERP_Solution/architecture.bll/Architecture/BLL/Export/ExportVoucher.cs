using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.Export;

public class ExportVoucher
{
	public static VoucherHead MakeVoucherForExportVoucher(Architecture.Model.Export.ExportVoucher obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.ExImInvoiceId;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.Remarks);
			vh.RemarksOtherLingo = "";
			vh.ManualBillNo = Conversion.ToString(obj.InvoiceNo);
			vh.VoucherAmount = Conversion.ToDouble(obj.LcyAmount);
			vh.FcAmount = Conversion.ToDouble(obj.FcyAmount);
			vh.ExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
			vh.MultiCurrencyId = obj.CurrencyId;
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = 0;
			vh.BillAmount = Conversion.ToDouble(obj.LcyAmount);
			vh.DueDate = obj.DueDate;
			vh.DueDays = obj.DueDays;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.ActionId = 1;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				List<SupplierCustomerGlAccountId> find2 = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find2.Count <= 0)
				{
					throw new Exception("Customer GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find2[0].GlAccountId);
			}
			if (obj.DocumentTypeId == 27)
			{
				bool FinancialisActiveonCommercialInvoice = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "FinancialisActiveonCommercialInvoice"));
				bool ExportAddlessAmountChargeToSalesAccount = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ExportAddlessAmountChargeToSalesAccount"));
				bool CgsAllow = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow"));
				bool FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
				int LineId = 1;
				foreach (ExportVoucherDetail item4 in obj.ExportVoucherDetailslist.ToList())
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
					if (find.Count > 0)
					{
						if (!FinancialisActiveonCommercialInvoice)
						{
							decimal LcyAddLessAmount = item4.AddLessAmount * Conversion.ToDecimal(obj.ExchangeRate);
							decimal LcyAmount = (ExportAddlessAmountChargeToSalesAccount ? (item4.LcyAmount + LcyAddLessAmount) : item4.LcyAmount);
							item4.LcyAmount = LcyAmount;
							item4.LineId = LineId;
							VoucherDetail vd1 = new VoucherDetail();
							vd1.AccountId = vh.RefAccountId;
							if (obj.CreditAccounId > 0)
							{
								vd1.AgainstAccountId = obj.CreditAccounId;
							}
							else
							{
								vd1.AgainstAccountId = find[0].SaleGLAC;
							}
							vd1.DebitAmount = Conversion.ToDouble(item4.LcyAmount);
							vd1.JobLotId = item4.LotJobId;
							vd1.Comments = item4.Remarks;
							vd1.IsTaxable = "False";
							vd1.QtyOut = Conversion.ToDouble(item4.Qty);
							vd1.WeightOut = Conversion.ToDouble(item4.NetWeight);
							vd1.ItemRate = Conversion.ToDouble(item4.RatePrice);
							vd1.ItemId = item4.ItemId;
							vd1.ItemAmount = Conversion.ToDouble(item4.LcyAmount);
							vd1.DMultiCurrencyId = item4.CurrencyId;
							vd1.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd1.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount) + Conversion.ToDouble(item4.AddLessAmount);
							vd1.SupplierCustomerId = obj.SupplierCustomerId;
							vd1.LineId = LineId;
							vh.voucherDetailList.Add(vd1);
							VoucherDetail vd2 = new VoucherDetail();
							if (obj.CreditAccounId > 0)
							{
								vd2.AccountId = obj.CreditAccounId;
							}
							else
							{
								vd2.AccountId = find[0].SaleGLAC;
							}
							vd2.AgainstAccountId = vh.RefAccountId;
							vd2.CreditAmount = Conversion.ToDouble(item4.LcyAmount);
							vd2.JobLotId = item4.LotJobId;
							vd2.Comments = item4.Remarks;
							vd2.IsTaxable = "False";
							vd2.QtyOut = Conversion.ToDouble(item4.Qty);
							vd2.WeightOut = Conversion.ToDouble(item4.NetWeight);
							vd2.ItemRate = Conversion.ToDouble(item4.RatePrice);
							vd2.ItemId = item4.ItemId;
							vd2.ItemAmount = Conversion.ToDouble(item4.LcyAmount);
							vd2.DMultiCurrencyId = item4.CurrencyId;
							vd2.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd2.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount) + Conversion.ToDouble(item4.AddLessAmount);
							vd2.SupplierCustomerId = obj.SupplierCustomerId;
							vd2.LineId = LineId;
							vh.voucherDetailList.Add(vd2);
						}
						double CgsAmount = 0.0;
						string RemarksCgs = "";
						bool SaleCostingJobOrderWise = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "SaleCostingJobOrderWise"));
						if (CgsAllow || SaleCostingJobOrderWise)
						{
							if (SaleCostingJobOrderWise)
							{
								ReportsParameters parameters = new ReportsParameters();
								parameters.OrganizationId = obj.OrganizationId;
								parameters.CompanyId = obj.CompanyId;
								parameters.DocDate = obj.DocDate;
								parameters.ItemId = item4.ItemId;
								parameters.JobLotId = item4.LotJobId;
								if (obj.Id > 0)
								{
									parameters.DocumentTypeId = obj.DocumentTypeId;
									parameters.RecId = obj.Id;
								}
								item4.CostRate = Conversion.ToDecimal(CommonServies.getAvgRateByJobOrder(parameters));
								if (item4.CostRate <= 0m)
								{
									item4.CostRate = 1m;
								}
							}
							else
							{
								ReportsParameters cgs = new ReportsParameters();
								cgs.OrganizationId = obj.OrganizationId;
								cgs.CompanyId = obj.CompanyId;
								cgs.DocDate = obj.DocDate;
								cgs.ItemId = item4.ItemId;
								cgs.JobLotId = item4.LotJobId;
								cgs.CropYearId = item4.CropYearId;
								cgs.WarehouseId = item4.WarehouseId;
								if (obj.Id > 0)
								{
									cgs.DocumentTypeId = obj.DocumentTypeId;
									cgs.Id = obj.Id;
								}
								item4.CostRate = Conversion.ToDecimal(CommonServies.AvgRateOnlyForCGS(cgs));
							}
						}
						if (item4.CostRate > 0m && !FIFOCgs)
						{
							CgsAmount = Conversion.ToDouble(item4.StockWeight) * Conversion.ToDouble(item4.CostRate);
							item4.StockValue = Conversion.ToDecimal(CgsAmount);
							RemarksCgs = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item4.Qty + ",   Weight: " + item4.StockWeight + ",   Rate:" + item4.CostRate + "  Item Amount: " + CgsAmount;
							VoucherDetail vd4 = new VoucherDetail();
							if (CgsAllow || SaleCostingJobOrderWise)
							{
								vd4.AccountId = Conversion.ToInt(find[0].COGSGLAC);
							}
							else
							{
								vd4.AccountId = ((obj.CreditAccounId > 0) ? obj.CreditAccounId : Conversion.ToInt(find[0].COGSGLAC));
							}
							vd4.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd4.Comments = Conversion.ToString(RemarksCgs);
							vd4.DebitAmount = Conversion.ToDouble(CgsAmount);
							vd4.ItemId = Conversion.ToInt(item4.ItemId);
							vd4.JobLotId = Conversion.ToInt(item4.LotJobId);
							vd4.QtyOut = Conversion.ToDouble(item4.Qty);
							vd4.OrderNo = Conversion.ToInt(item4.ContractId);
							vd4.WeightOut = Conversion.ToDouble(item4.StockWeight);
							vd4.ItemRate = Conversion.ToDouble(item4.CostRate);
							vd4.ItemCgsRate = Conversion.ToDouble(item4.CostRate);
							vd4.ItemAmount = Conversion.ToDouble(CgsAmount);
							vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd4.RefDocSubIdNo = Conversion.ToInt(item4.Id);
							vd4.IsCGS = 1;
							vd4.LineId = LineId;
							vh.voucherDetailList.Add(vd4);
							VoucherDetail vd3 = new VoucherDetail();
							vd3.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							if (CgsAllow || SaleCostingJobOrderWise)
							{
								vd3.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
							}
							else
							{
								vd3.AgainstAccountId = ((obj.CreditAccounId > 0) ? obj.CreditAccounId : Conversion.ToInt(find[0].COGSGLAC));
							}
							vd3.Comments = Conversion.ToString(RemarksCgs);
							vd3.CreditAmount = Conversion.ToDouble(CgsAmount);
							vd3.ItemId = Conversion.ToInt(item4.ItemId);
							vd3.JobLotId = Conversion.ToInt(item4.LotJobId);
							vd3.QtyOut = Conversion.ToDouble(item4.Qty);
							vd3.OrderNo = Conversion.ToInt(item4.ContractId);
							vd3.WeightOut = Conversion.ToDouble(item4.StockWeight);
							vd3.ItemRate = Conversion.ToDouble(item4.CostRate);
							vd3.ItemCgsRate = Conversion.ToDouble(item4.CostRate);
							vd3.ItemAmount = Conversion.ToDouble(CgsAmount);
							vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd3.RefDocSubIdNo = Conversion.ToInt(item4.Id);
							vd3.IsCGS = 1;
							vd3.LineId = LineId;
							vh.voucherDetailList.Add(vd3);
						}
					}
					LineId++;
				}
				if (!FinancialisActiveonCommercialInvoice)
				{
					if (!ExportAddlessAmountChargeToSalesAccount)
					{
						foreach (VoucherDetail item3 in ExportOtherChargesFinancial(obj, vh.RefAccountId).voucherDetailList.ToList())
						{
							vh.voucherDetailList.Add(item3);
						}
					}
					foreach (VoucherDetail item2 in ExportPaymentDetailFinancial(obj, vh.RefAccountId).voucherDetailList.ToList())
					{
						vh.voucherDetailList.Add(item2);
					}
					foreach (VoucherDetail item in ExportCommissionAgentFinancial(obj, dtSupplier).voucherDetailList.ToList())
					{
						vh.voucherDetailList.Add(item);
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static VoucherHead ExportOtherChargesFinancial(Architecture.Model.Export.ExportVoucher obj, int RefAccountId)
	{
		VoucherHead vh = new VoucherHead();
		foreach (ExImInvoiceOtherChargesDetail item in obj.ExImInvoiceOtherChargesDetailList)
		{
			if (item.AddAmount != 0.0 || item.LessAmount != 0.0)
			{
				if (item.ChargesAccountId == 0)
				{
					throw new Exception("Charges Account not found");
				}
				double LcyAmount = ((item.AddAmount > 0.0) ? item.AddAmount : item.LessAmount) * Conversion.ToDouble(obj.ExchangeRate);
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = ((item.AddAmount > 0.0) ? RefAccountId : item.ChargesAccountId),
					AgainstAccountId = ((item.AddAmount > 0.0) ? item.ChargesAccountId : RefAccountId),
					Comments = ((!string.IsNullOrEmpty(item.Remarks)) ? (item.Remarks + " " + item.ChargesAccount) : ("Export Charges " + item.ChargesAccount)),
					DebitAmount = LcyAmount,
					DMultiCurrencyId = obj.CurrencyId,
					DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate),
					DCurrencyAmount = ((item.AddAmount > 0.0) ? item.AddAmount : item.LessAmount),
					SubsidiaryTypeId = 1,
					SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId),
					SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId),
					BranchesId = obj.BranchesId
				});
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = ((item.AddAmount > 0.0) ? item.ChargesAccountId : RefAccountId),
					AgainstAccountId = ((item.AddAmount > 0.0) ? RefAccountId : item.ChargesAccountId),
					Comments = ((!string.IsNullOrEmpty(item.Remarks)) ? (item.Remarks + " " + item.ChargesAccount) : ("Export Charges " + item.ChargesAccount)),
					CreditAmount = LcyAmount,
					DMultiCurrencyId = obj.CurrencyId,
					DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate),
					DCurrencyAmount = ((item.AddAmount > 0.0) ? item.AddAmount : item.LessAmount),
					SubsidiaryTypeId = 1,
					SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId),
					SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId),
					BranchesId = obj.BranchesId
				});
			}
		}
		return vh;
	}

	public static VoucherHead ExportPaymentDetailFinancial(Architecture.Model.Export.ExportVoucher obj, int refAccountId)
	{
		VoucherHead voucherHead = new VoucherHead();
		foreach (ExImInvoicePaymentTermsDetail item in obj.ExImInvoicePaymentTermsDetailList.Where((ExImInvoicePaymentTermsDetail r) => r.PaymentTermId == 1))
		{
			DataTable exchangeRateAndGainLossAccountFcyReceipts = getExchangeRateAndGainLossAccountFcyReceipts(obj.OrganizationId, obj.CompanyId, item.DocumentTypeId, item.ExImEFormRegistrationId);
			if (exchangeRateAndGainLossAccountFcyReceipts != null && exchangeRateAndGainLossAccountFcyReceipts.Rows.Count == 0)
			{
				throw new Exception("ExchangeRate or GainLossAccountId not found Against This FI " + item.FinancialInstrumentNo);
			}
			int gainLossAccountId = Conversion.ToInt(exchangeRateAndGainLossAccountFcyReceipts.Rows[0]["GainLossAccountId"]);
			decimal fcyRate = Conversion.ToDecimal(exchangeRateAndGainLossAccountFcyReceipts.Rows[0]["ExchangeRate"]);
			if (fcyRate == 0m)
			{
				throw new Exception("ExchangeRate not found Against This FI " + item.FinancialInstrumentNo);
			}
			if (gainLossAccountId == 0)
			{
				throw new Exception("GainLossAccountId not found Against This FI " + item.FinancialInstrumentNo);
			}
			decimal rateDiff = obj.ExchangeRate - fcyRate;
			if (!(rateDiff == 0m))
			{
				double lcyAmount = Conversion.ToDouble(rateDiff) * item.FcyAmount;
				int subsidiaryId = Conversion.ToInt(obj.SupplierCustomerId);
				string remarks = "Export Invoice ExchangeRate is " + obj.ExchangeRate + " and Fcy Receipts ExchangeRate is " + fcyRate + " " + item.PaymentRemarks;
				if (rateDiff > 0m)
				{
					AddVoucherEntry(voucherHead, gainLossAccountId, refAccountId, lcyAmount, remarks, subsidiaryId, obj.BranchesId);
				}
				else
				{
					AddVoucherEntry(voucherHead, refAccountId, gainLossAccountId, lcyAmount, remarks, subsidiaryId, obj.BranchesId);
				}
			}
		}
		return voucherHead;
	}

	public static VoucherHead ExportCommissionAgentFinancial(Architecture.Model.Export.ExportVoucher obj, List<SupplierCustomerGlAccountId> dtSupplier)
	{
		VoucherHead vh = new VoucherHead();
		foreach (ExImInvoiceSaleManCommission item in obj.ExImInvoiceSaleManCommissionList)
		{
			int SalesPersonAccountId = 0;
			if (item.SalesPersonId == 0)
			{
				throw new Exception("Sales person Account not found");
			}
			if (obj.CommissionExpenseAccountId == 0)
			{
				throw new Exception("Commission Expenses Account not found");
			}
			List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.SalesPersonId)).ToList();
			if (find.Count > 0)
			{
				SalesPersonAccountId = Conversion.ToInt(find[0].GlAccountId);
				double LcyAmount = item.fcyAmount * Conversion.ToDouble(obj.ExchangeRate);
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = obj.CommissionExpenseAccountId,
					AgainstAccountId = SalesPersonAccountId,
					Comments = ((!string.IsNullOrEmpty(item.Remarks)) ? (item.Remarks + " " + item.SalesPerson) : ("CommissionType " + item.CommissionType + " Commission Rate " + item.commissionRate + " " + item.SalesPerson)),
					DebitAmount = LcyAmount,
					DMultiCurrencyId = obj.CurrencyId,
					DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate),
					DCurrencyAmount = item.fcyAmount,
					BranchesId = obj.BranchesId
				});
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = SalesPersonAccountId,
					AgainstAccountId = obj.CommissionExpenseAccountId,
					Comments = ((!string.IsNullOrEmpty(item.Remarks)) ? (item.Remarks + " " + item.SalesPerson) : ("CommissionType " + item.CommissionType + " Commission Rate " + item.commissionRate + " " + item.SalesPerson)),
					CreditAmount = LcyAmount,
					DMultiCurrencyId = obj.CurrencyId,
					DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate),
					DCurrencyAmount = item.fcyAmount,
					SubsidiaryTypeId = 1,
					SubsidiaryAccountId = Conversion.ToInt(item.SalesPersonId),
					SupplierCustomerId = Conversion.ToInt(item.SalesPersonId),
					BranchesId = obj.BranchesId
				});
				continue;
			}
			throw new Exception("Sales person GLAccountId not Found");
		}
		return vh;
	}

	private static void AddVoucherEntry(VoucherHead voucherHead, int debitAccountId, int creditAccountId, double amount, string remarks, int subsidiaryId, int branchId)
	{
		voucherHead.voucherDetailList.Add(new VoucherDetail
		{
			AccountId = debitAccountId,
			AgainstAccountId = creditAccountId,
			Comments = remarks,
			DebitAmount = amount,
			SubsidiaryTypeId = 1,
			SubsidiaryAccountId = subsidiaryId,
			SupplierCustomerId = subsidiaryId,
			BranchesId = branchId
		});
		voucherHead.voucherDetailList.Add(new VoucherDetail
		{
			AccountId = creditAccountId,
			AgainstAccountId = debitAccountId,
			Comments = remarks,
			CreditAmount = amount,
			SubsidiaryTypeId = 1,
			SubsidiaryAccountId = subsidiaryId,
			SupplierCustomerId = subsidiaryId,
			BranchesId = branchId
		});
	}

	public static int Save(Architecture.Model.Export.ExportVoucher obj)
	{
		try
		{
			obj.VoucherHeadExImInvoices = MakeVoucherForExportVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ModifyUserId = 0;
				return new Architecture.DAL.Export.ExportVoucher().SetDate(obj, "USP_ExportVoucher_Insert");
			}
			obj.EntryUserId = 0;
			return new Architecture.DAL.Export.ExportVoucher().SetDate(obj, "USP_ExportVoucher_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getExchangeRateAndGainLossAccountFcyReceipts(int OrganizationId, int CompanyId, int DocumentTypeId, int FcyReceiptsId)
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
				ParameterName = "@FcyReceiptsId",
				Value = FcyReceiptsId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportVoucher>("[dbo].[usp_getExchangeRateAndGainLossAccountFcyReceipts]", p);
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
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
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportVoucher>("Sp_ExportVoucher_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryNew(ReportsParameters obj)
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
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
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved ",
					Value = obj.IsApproved
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
			if (obj.IsSpecialApprovalRequired != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsSpecialApprovalRequired",
					Value = obj.IsSpecialApprovalRequired
				});
			}
			return GenericProvider.GetDataTableProc<VoucherHead>("USP_ExportInvoiceVoucher_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromExportInvoiceVoucher(ReportsParameters obj)
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
			if (obj.NotUploaded != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NotUploaded",
					Value = obj.NotUploaded
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityType ",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<PdcInventory>("[dbo].[USP_GetDataForDropDownFromExportInvoiceVoucher]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Export.ExportVoucher obj)
	{
		DataTable dt = null;
		try
		{
			int DocNo = 0;
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
				Value = "GenerateDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportVoucher>("Sp_ExportVoucher_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				DocNo = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return DocNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExportVoucher GetByID(int Id)
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
			new List<Architecture.Model.Export.ExportVoucher>();
			return new Architecture.DAL.Export.ExportVoucher().GetData("Sp_ExportVoucher_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportVoucherPendingForUpload(ReportsParameters obj)
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
			if (obj.ContractId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.ContractId
				});
			}
			if (obj.CurrencyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CurrencyId",
					Value = obj.CurrencyId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportVoucher>("USP_ExportVoucherPendingForUpload", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExportVoucher GetByIDForUpload(int Id)
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
			new List<Architecture.Model.Export.ExportVoucher>();
			return new Architecture.DAL.Export.ExportVoucher().GetDataForUpload("Sp_ExportVoucher_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateUploadStatus(int UploadedById, int Id, bool IsUploaded = true)
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
			sqlCommand.CommandText = "[dbo].[USP_ExportVoucher_UpdateUploadStatus]";
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@UploadedById", UploadedById);
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

	public static DataTable GetExportVoucherIdsForAutoUpdation(int OrganizationId, int CompanyId, int FinancialYearId, int DocumentTypeId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvStockTransferHeader>("[dbo].[USP_GetExportVoucherIdsForAutoUpdation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
