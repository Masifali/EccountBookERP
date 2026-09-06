using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.BLL.TaxProject.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.TaxProject.Export;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Export;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.BLL.TaxProject.Export;

public class ExportVoucher
{
	public static VoucherHead MakeVoucherForExportVoucher(Architecture.Model.TaxProject.Export.ExportVoucher obj, int OrgId, int CompId)
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
			dtitem = Architecture.BLL.TaxProject.Common.CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtSupplier = Architecture.BLL.TaxProject.Common.CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
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
				bool FinancialisActiveonCommercialInvoice = Conversion.ToBool(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "FinancialisActiveonCommercialInvoice"));
				bool CgsAllow = Conversion.ToBool(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow"));
				bool FIFOCgs = Architecture.BLL.TaxProject.Common.CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 5);
				int LineId = 1;
				foreach (Architecture.Model.TaxProject.Export.ExportVoucherDetail item4 in obj.ExportVoucherDetailslist.ToList())
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
					if (find.Count > 0)
					{
						if (!FinancialisActiveonCommercialInvoice)
						{
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
							vd1.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
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
							vd2.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd2.SupplierCustomerId = obj.SupplierCustomerId;
							vd2.LineId = LineId;
							vh.voucherDetailList.Add(vd2);
						}
						double CgsAmount = 0.0;
						string RemarksCgs = "";
						bool SaleCostingJobOrderWise = Conversion.ToBool(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "SaleCostingJobOrderWise"));
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
								item4.CostRate = Conversion.ToDecimal(Architecture.BLL.Common.CommonServies.getAvgRateByJobOrder(parameters));
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
								item4.CostRate = Conversion.ToDecimal(Architecture.BLL.TaxProject.Common.CommonServies.AvgRateOnlyForCGS(cgs));
							}
						}
						if ((CgsAllow || SaleCostingJobOrderWise) && item4.CostRate > 0m && !FIFOCgs)
						{
							CgsAmount = Conversion.ToDouble(item4.StockWeight) * Conversion.ToDouble(item4.CostRate);
							item4.StockValue = Conversion.ToDecimal(CgsAmount);
							RemarksCgs = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item4.Qty + ",   Weight: " + item4.StockWeight + ",   Rate:" + item4.CostRate + "  Item Amount: " + CgsAmount;
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = ((obj.CreditAccounId > 0) ? obj.CreditAccounId : Conversion.ToInt(find[0].COGSGLAC));
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
							vd3.AgainstAccountId = ((obj.CreditAccounId > 0) ? obj.CreditAccounId : Conversion.ToInt(find[0].COGSGLAC));
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
					foreach (VoucherDetail item3 in ExportOtherChargesFinancial(obj, vh.RefAccountId).voucherDetailList.ToList())
					{
						vh.voucherDetailList.Add(item3);
					}
					foreach (VoucherDetail item2 in ExportPaymentDetailFinancial(obj, vh.RefAccountId, OrgId, CompId).voucherDetailList.ToList())
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

	public static VoucherHead ExportOtherChargesFinancial(Architecture.Model.TaxProject.Export.ExportVoucher obj, int RefAccountId)
	{
		VoucherHead vh = new VoucherHead();
		foreach (Architecture.Model.TaxProject.Export.ExImInvoiceOtherChargesDetail item in obj.ExImInvoiceOtherChargesDetailList)
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

	public static VoucherHead ExportPaymentDetailFinancial(Architecture.Model.TaxProject.Export.ExportVoucher obj, int refAccountId, int OrgId, int CompId)
	{
		VoucherHead voucherHead = new VoucherHead();
		foreach (Architecture.Model.TaxProject.Export.ExImInvoicePaymentTermsDetail item in obj.ExImInvoicePaymentTermsDetailList.Where((Architecture.Model.TaxProject.Export.ExImInvoicePaymentTermsDetail r) => r.PaymentTermId == 1))
		{
			DataTable exchangeRateAndGainLossAccountFcyReceipts = getExchangeRateAndGainLossAccountFcyReceipts(OrgId, CompId, item.DocumentTypeId, item.ExImEFormRegistrationId);
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

	public static VoucherHead ExportCommissionAgentFinancial(Architecture.Model.TaxProject.Export.ExportVoucher obj, List<SupplierCustomerGlAccountId> dtSupplier)
	{
		VoucherHead vh = new VoucherHead();
		foreach (Architecture.Model.TaxProject.Export.ExImInvoiceSaleManCommission item in obj.ExImInvoiceSaleManCommissionList)
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

	public static int Save(Architecture.Model.TaxProject.Export.ExportVoucher obj, int OrgId, int CompId)
	{
		try
		{
			obj.VoucherHeadExImInvoices = MakeVoucherForExportVoucher(obj, OrgId, CompId);
			if (obj.Id == 0)
			{
				obj.ModifyUserId = 0;
				return new Architecture.DAL.TaxProject.Export.ExportVoucher().SetDate(obj, "USP_ExportVoucher_Insert");
			}
			obj.EntryUserId = 0;
			return new Architecture.DAL.TaxProject.Export.ExportVoucher().SetDate(obj, "USP_ExportVoucher_Update");
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
}
