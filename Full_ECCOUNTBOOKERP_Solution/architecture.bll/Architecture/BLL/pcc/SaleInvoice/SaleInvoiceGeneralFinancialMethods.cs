using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc.SaleInvoice;

public class SaleInvoiceGeneralFinancialMethods
{
	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.pcc.InvSaleInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861 || obj.DocumentTypeId == 1862) && obj.InvSaleInvoiceJournalList.Count > 0)
		{
			foreach (Architecture.Model.pcc.InvSaleInvoiceJournal item in obj.InvSaleInvoiceJournalList.ToList())
			{
				if (Conversion.ToInt(item.ChartofAccountId) != 0)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(item.ChartofAccountId);
					vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd.Comments = Conversion.ToString(item.JvRemarks);
					vd.DebitAmount = Conversion.ToDouble(item.JvDebit);
					vd.CreditAmount = Conversion.ToDouble(item.JvCredit);
					vd.SubsidiaryTypeId = 1;
					vd.SubsidiaryAccountId = Conversion.ToInt(item.TransporterSupCustId);
					vd.SupplierCustomerId = Conversion.ToInt(item.TransporterSupCustId);
					vh.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd2.AgainstAccountId = Conversion.ToInt(item.ChartofAccountId);
					vd2.Comments = Conversion.ToString(item.JvRemarks);
					vd2.CreditAmount = Conversion.ToDouble(item.JvDebit);
					vd2.DebitAmount = Conversion.ToDouble(item.JvCredit);
					vd2.SubsidiaryTypeId = 1;
					vd2.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		return vh;
	}

	public static VoucherHead ItemAndCustomerByWeightFinancial(Architecture.Model.pcc.InvSaleInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861 || obj.DocumentTypeId == 1862) && obj.InvSaleInvoiceDetaillist.Count > 0)
		{
			foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist.ToList())
			{
				string Remarks = string.Empty;
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						if (item.ActionTypeId == 3)
						{
							continue;
						}
						if (obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861)
						{
							if (obj.RemarksHeader != string.Empty)
							{
								Remarks = obj.RemarksHeader;
							}
							Remarks = Remarks + "  Sale " + Conversion.ToString(Math.Round(item.ItemQty)) + " Qty ( " + Conversion.ToString(Math.Round(item.VarientEquivalent));
							Remarks = Remarks + "  " + Conversion.ToString(Math.Round(item.ItemNetWeight)) + ")  of " + Conversion.ToString(find[0].ItemName) + " @Rate. " + Conversion.ToString(Math.Round(item.ItemRate)) + "   = Rs. " + Conversion.ToString(Math.Round(item.ItemAmount));
							if (item.ItemDiscountAmount > 0m)
							{
								Remarks = Remarks + " Discount " + Math.Round(item.ItemDiscountAmount, 3);
							}
							if (obj.PaymentTermsId == 2)
							{
								Remarks = Remarks + " Term " + obj.DueDays + " days Credit DueDate " + Conversion.ToDateTime(obj.DueDate).ToString("dd-MMM-yyyy");
							}
							else if (obj.PaymentTermsId == 1)
							{
								Remarks += " Term Cash";
							}
							if (item.RateCut != 0m)
							{
								Remarks = Remarks + " RateCut " + Conversion.ToString(Math.Round(item.RateCut)) + " RateCutAmount " + Math.Round(item.RateCutAmount);
							}
							if (item.VehicleNo != null && item.VehicleNo != string.Empty)
							{
								Remarks = Remarks + " Vehicle # " + item.VehicleNo;
							}
							if (item.GpNo != 0)
							{
								Remarks = Remarks + " Gp # " + item.GpNo;
							}
							if (item.ExpenseAmount > 0m)
							{
								Remarks = Remarks + " Expense Amount " + Math.Round(item.ExpenseAmount);
							}
							if (item.SaleOrderNo > 0)
							{
								Remarks = Remarks + " Under contract # " + item.SaleOrderNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								Remarks = Remarks + " Manual # " + obj.ManualBillNo;
							}
							Remarks = Remarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
						}
						else if (obj.DocumentTypeId == 1862)
						{
							if (obj.RemarksHeader != string.Empty)
							{
								Remarks = obj.RemarksHeader;
							}
							Remarks = Remarks + "  Sale Return " + Conversion.ToString(Math.Round(item.ItemQty)) + " Qty ( " + Conversion.ToString(Math.Round(item.VarientEquivalent));
							Remarks = Remarks + "  " + Conversion.ToString(Math.Round(item.ItemNetWeight)) + ")  of " + Conversion.ToString(find[0].ItemName) + " @Rate. " + Conversion.ToString(Math.Round(item.ItemRate)) + "   = Rs. " + Conversion.ToString(Math.Round(item.ItemAmount));
							if (item.ItemDiscountAmount > 0m)
							{
								Remarks = Remarks + " Discount " + Math.Round(item.ItemDiscountAmount, 3);
							}
							if (obj.PaymentTermsId == 2)
							{
								Remarks = Remarks + " Term " + obj.DueDays + " days Credit DueDate " + Conversion.ToDateTime(obj.DueDate).ToString("dd-MMM-yyyy");
							}
							else if (obj.PaymentTermsId == 1)
							{
								Remarks += " Term Cash";
							}
							if (item.RateCut != 0m)
							{
								Remarks = Remarks + " RateCut " + Conversion.ToString(Math.Round(item.RateCut)) + " RateCutAmount " + Math.Round(item.RateCutAmount);
							}
							if (item.VehicleNo != null && item.VehicleNo != string.Empty)
							{
								Remarks = Remarks + " Vehicle # " + item.VehicleNo;
							}
							if (item.GpNo != 0)
							{
								Remarks = Remarks + " Gp # " + item.GpNo;
							}
							if (item.ExpenseAmount > 0m)
							{
								Remarks = Remarks + " Expense Amount " + Math.Round(item.ExpenseAmount);
							}
							if (item.SaleOrderNo > 0)
							{
								Remarks = Remarks + " Under contract # " + item.SaleOrderNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								Remarks = Remarks + " Manual # " + obj.ManualBillNo;
							}
							Remarks = Remarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
						}
						if (!item.IsFOC)
						{
							if (obj.DocumentTypeId != 1862)
							{
								VoucherDetail vd5 = new VoucherDetail();
								vd5.LineId = 0;
								vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
								vd5.AgainstAccountId = Conversion.ToInt(find[0].SaleGLAC);
								vd5.Comments = Conversion.ToString(Remarks);
								vd5.DebitAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd5.ItemId = Conversion.ToInt(item.ItemId);
								vd5.QtyOut = Conversion.ToDouble(item.ItemQty);
								vd5.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd5.WeightOut = Conversion.ToDouble(item.ItemNetWeight);
								vd5.RateCut = Conversion.ToDouble(item.RateCut);
								vd5.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
								vd5.ItemAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd5.Expenses = Conversion.ToDouble(item.ExpenseAmount);
								vd5.Freight = Conversion.ToDouble(item.FreightAmount);
								vd5.Commission = Conversion.ToDouble(item.CommissionAmount);
								vd5.OrderNo = Conversion.ToInt(item.SaleOrderNo);
								vd5.GpNo = Conversion.ToInt(item.GpNo);
								vd5.JobLotId = Conversion.ToInt(item.JobLotId);
								vd5.VehicleNo = item.VehicleNo;
								vd5.DMultiCurrencyId = item.CurrencyId;
								vd5.DExchangeCurrencyRate = Conversion.ToDouble(item.ExchangeRate);
								vd5.DCurrencyAmount = Conversion.ToDouble(item.FcyAmount);
								vd5.SubsidiaryTypeId = 1;
								vd5.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd5);
								VoucherDetail vd3 = new VoucherDetail();
								vd3.LineId = 0;
								vd3.AccountId = Conversion.ToInt(find[0].SaleGLAC);
								vd3.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd3.Comments = Conversion.ToString(Remarks);
								vd3.CreditAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd3.ItemId = Conversion.ToInt(item.ItemId);
								vd3.QtyOut = Conversion.ToDouble(item.ItemQty);
								vd3.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd3.WeightOut = Conversion.ToDouble(item.ItemNetWeight);
								vd3.RateCut = Conversion.ToDouble(item.RateCut);
								vd3.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
								vd3.ItemAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd3.Expenses = Conversion.ToDouble(item.ExpenseAmount);
								vd3.Commission = Conversion.ToDouble(item.CommissionAmount);
								vd3.Freight = Conversion.ToDouble(item.FreightAmount);
								vd3.OrderNo = Conversion.ToInt(item.SaleOrderNo);
								vd3.GpNo = Conversion.ToInt(item.GpNo);
								vd3.VehicleNo = item.VehicleNo;
								vd3.JobLotId = Conversion.ToInt(item.JobLotId);
								vd3.SubsidiaryAgainstTypeId = 1;
								vd3.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd3.DMultiCurrencyId = item.CurrencyId;
								vd3.DExchangeCurrencyRate = Conversion.ToDouble(item.ExchangeRate);
								vd3.DCurrencyAmount = Conversion.ToDouble(item.FcyAmount);
								vh.voucherDetailList.Add(vd3);
							}
							if (obj.DocumentTypeId == 1862)
							{
								VoucherDetail vd2 = new VoucherDetail();
								vd2.LineId = 0;
								vd2.AccountId = Conversion.ToInt(find[0].SaleGLAC);
								vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd2.Comments = Conversion.ToString(Remarks);
								vd2.DebitAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd2.ItemId = Conversion.ToInt(item.ItemId);
								vd2.QtyIn = Conversion.ToDouble(item.ItemQty);
								vd2.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd2.WeightIn = Conversion.ToDouble(item.ItemNetWeight);
								vd2.RateCut = Conversion.ToDouble(item.RateCut);
								vd2.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
								vd2.ItemAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd2.Expenses = Conversion.ToDouble(item.ExpenseAmount);
								vd2.Commission = Conversion.ToDouble(item.CommissionAmount);
								vd2.Freight = Conversion.ToDouble(item.FreightAmount);
								vd2.OrderNo = Conversion.ToInt(item.SaleOrderNo);
								vd2.GpNo = Conversion.ToInt(item.GpNo);
								vd2.VehicleNo = item.VehicleNo;
								vd2.JobLotId = Conversion.ToInt(item.JobLotId);
								vd2.DMultiCurrencyId = item.CurrencyId;
								vd2.DExchangeCurrencyRate = Conversion.ToDouble(item.ExchangeRate);
								vd2.DCurrencyAmount = Conversion.ToDouble(item.FcyAmount);
								vd2.SubsidiaryAgainstTypeId = 1;
								vd2.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd2);
								VoucherDetail vd4 = new VoucherDetail();
								vd4.LineId = 0;
								vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
								vd4.AgainstAccountId = Conversion.ToInt(find[0].SaleGLAC);
								vd4.Comments = Conversion.ToString(Remarks);
								vd4.CreditAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd4.ItemId = Conversion.ToInt(item.ItemId);
								vd4.QtyIn = Conversion.ToDouble(item.ItemQty);
								vd4.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd4.WeightIn = Conversion.ToDouble(item.ItemNetWeight);
								vd4.RateCut = Conversion.ToDouble(item.RateCut);
								vd4.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
								vd4.ItemAmount = Conversion.ToDouble(item.ItemAmountWithDisc);
								vd4.Expenses = Conversion.ToDouble(item.ExpenseAmount);
								vd4.Freight = Conversion.ToDouble(item.FreightAmount);
								vd4.Commission = Conversion.ToDouble(item.CommissionAmount);
								vd4.OrderNo = Conversion.ToInt(item.SaleOrderNo);
								vd4.GpNo = Conversion.ToInt(item.GpNo);
								vd4.JobLotId = Conversion.ToInt(item.JobLotId);
								vd4.VehicleNo = item.VehicleNo;
								vd4.DMultiCurrencyId = item.CurrencyId;
								vd4.DExchangeCurrencyRate = Conversion.ToDouble(item.ExchangeRate);
								vd4.DCurrencyAmount = Conversion.ToDouble(item.FcyAmount);
								vd4.SubsidiaryTypeId = 1;
								vd4.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd4);
							}
						}
						if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861) && !Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")) && item.ExpenseAmount > 0m)
						{
							VoucherDetail vd7 = new VoucherDetail();
							vd7.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd7.AgainstAccountId = Conversion.ToInt(find[0].SaleGLAC);
							vd7.DebitAmount = Conversion.ToDouble(item.ExpenseAmount);
							vd7.Comments = Remarks;
							vd7.JobLotId = Conversion.ToInt(item.JobLotId);
							vd7.SubsidiaryTypeId = 1;
							vd7.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd7);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.AccountId = Conversion.ToInt(find[0].SaleGLAC);
							vd6.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd6.CreditAmount = Conversion.ToDouble(item.ExpenseAmount);
							vd6.Comments = Remarks;
							vd6.JobLotId = Conversion.ToInt(item.JobLotId);
							vd6.SubsidiaryAgainstTypeId = 1;
							vd6.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd6);
						}
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				throw new Exception("Item Record Not found");
			}
		}
		return vh;
	}

	public static VoucherHead CGSTransactions(Architecture.Model.pcc.InvSaleInvoice obj, List<GetItemGlIdsandItemName> dtitem, List<SupplierCustomerGlAccountId> dtSupplier)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861)
		{
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item3 in obj.InvSaleInvoiceDetaillist.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
						if (find2.Count > 0)
						{
							if (item3.ActionTypeId != 3 && item3.JournalAmount > 0m && obj.SettlementDiscountAccountId == 0)
							{
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = Conversion.ToInt(find2[0].COGSGLAC);
								vd5.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd5.Comments = "Settle Discount " + find2[0].ItemName.ToString();
								vd5.DebitAmount = Conversion.ToDouble(item3.JournalAmount);
								vh.voucherDetailList.Add(vd5);
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
					throw new Exception("Item Record Not found");
				}
				foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item2 in obj.InvSaleInvoiceDetaillist.ToList())
				{
					_ = string.Empty;
					double CgsRate = 0.0;
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
						if (find.Count > 0)
						{
							if (item2.ActionTypeId == 3 || (obj.DocumentTypeId != 1856 && obj.DocumentTypeId != 1861))
							{
								continue;
							}
							bool ItemWiseCGS = CommonRepository.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 1);
							if (Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow")) && ItemWiseCGS)
							{
								ReportsParameters parameters = new ReportsParameters();
								parameters.OrganizationId = obj.OrganizationId;
								parameters.CompanyId = obj.CompanyId;
								parameters.ItemId = item2.ItemId;
								parameters.DocDate = obj.DocDate;
								parameters.JobLotId = item2.JobLotId;
								parameters.WarehouseId = item2.WarehouseId;
								if (obj.Id > 0)
								{
									parameters.DocumentTypeId = obj.DocumentTypeId;
									parameters.Id = obj.Id;
								}
								CgsRate = CommonServies.AvgRateOnlyForCGS(parameters);
								if (CgsRate <= 0.0)
								{
									throw new Exception("CGS Rate not found");
								}
								decimal CgsAmount = item2.ItemQty * item2.VarientEquivalent * Conversion.ToDecimal(CgsRate);
								string RemarksCgs = "Qty: " + item2.ItemQty + "   " + Conversion.ToString(find[0].ItemName) + " Varient: " + item2.VarientEquivalent + "  Rate:" + CgsRate;
								VoucherDetail vd8 = new VoucherDetail();
								vd8.LineId = item2.LineId;
								vd8.AccountId = Conversion.ToInt(find[0].COGSGLAC);
								vd8.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd8.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd8.DebitAmount = Conversion.ToDouble(CgsAmount);
								vd8.ItemId = Conversion.ToInt(item2.ItemId);
								vd8.QtyOut = Conversion.ToDouble(item2.ItemQty);
								vd8.WeightOut = Conversion.ToDouble(item2.ItemNetWeight);
								vd8.ItemCgsRate = Conversion.ToDouble(CgsRate);
								vd8.RateCut = Conversion.ToDouble(item2.RateCut);
								vd8.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
								vd8.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd8.Expenses = Conversion.ToDouble(item2.ExpenseAmount);
								vd8.Commission = Conversion.ToDouble(item2.CommissionAmount);
								vd8.Freight = Conversion.ToDouble(item2.FreightAmount);
								vd8.OrderNo = Conversion.ToInt(item2.SaleOrderNo);
								vd8.GpNo = Conversion.ToInt(item2.GpNo);
								vd8.VehicleNo = item2.VehicleNo;
								vd8.JobLotId = Conversion.ToInt(item2.JobLotId);
								vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd8.IsCGS = 1;
								vh.voucherDetailList.Add(vd8);
								VoucherDetail vd6 = new VoucherDetail();
								vd6.LineId = item2.LineId;
								vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
								vd6.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd6.CreditAmount = Conversion.ToDouble(CgsAmount);
								vd6.ItemId = Conversion.ToInt(item2.ItemId);
								vd6.QtyOut = Conversion.ToDouble(item2.ItemQty);
								vd6.WeightOut = Conversion.ToDouble(item2.ItemNetWeight);
								vd6.ItemCgsRate = Conversion.ToDouble(CgsRate);
								vd6.RateCut = Conversion.ToDouble(item2.RateCut);
								vd6.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
								vd6.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd6.Expenses = Conversion.ToDouble(item2.ExpenseAmount);
								vd6.Commission = Conversion.ToDouble(item2.CommissionAmount);
								vd6.Freight = Conversion.ToDouble(item2.FreightAmount);
								vd6.OrderNo = Conversion.ToInt(item2.SaleOrderNo);
								vd6.GpNo = Conversion.ToInt(item2.GpNo);
								vd6.VehicleNo = item2.VehicleNo;
								vd6.JobLotId = Conversion.ToInt(item2.JobLotId);
								vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd6.IsCGS = 1;
								vh.voucherDetailList.Add(vd6);
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
					throw new Exception("Item Record Not found");
				}
			}
		}
		else if (obj.DocumentTypeId == 1862)
		{
			bool FIFOCgs = CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 5);
			if (Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow")) || FIFOCgs)
			{
				foreach (Architecture.Model.pcc.InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find3.Count > 0)
						{
							item.ItemName = Conversion.ToString(find3[0].ItemName);
							string RemarksCgs2 = "";
							decimal CgsAmount2 = default(decimal);
							ReportsParameters parameters2 = new ReportsParameters();
							parameters2.OrganizationId = obj.OrganizationId;
							parameters2.CompanyId = obj.CompanyId;
							parameters2.ItemId = item.ItemId;
							parameters2.DocDate = obj.DocDate;
							if (obj.Id > 0)
							{
								parameters2.RecId = obj.Id;
								parameters2.DocumentTypeId = obj.DocumentTypeId;
							}
							DataTable dtcgs = CommonRepository.getRatefromLastPurchase(parameters2);
							if (dtcgs != null && dtcgs.Rows.Count > 0)
							{
								item.CgsRate = Conversion.ToDecimal(dtcgs.Rows[0]["CgsRate"]);
							}
							if (item.CgsRate <= 0m)
							{
								throw new Exception("CGS Rate not found against " + item.ItemName + " and row qty is " + item.ItemQty);
							}
							RemarksCgs2 = "Bags: " + item.ItemQty + "   " + Conversion.ToString(find3[0].ItemName) + " Rate: " + item.CgsRate;
							CgsAmount2 = item.ItemQty * item.VarientEquivalent * item.CgsRate;
							VoucherDetail vd7 = new VoucherDetail();
							vd7.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							vd7.AgainstAccountId = Conversion.ToInt(find3[0].COGSGLAC);
							vd7.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd7.DebitAmount = Conversion.ToDouble(CgsAmount2);
							vd7.ItemId = Conversion.ToInt(item.ItemId);
							vd7.QtyIn = Conversion.ToDouble(item.ItemQty);
							vd7.WeightIn = Conversion.ToDouble(item.ItemWeight);
							vd7.ItemCgsRate = Conversion.ToDouble(item.CgsRate);
							vd7.ItemAmount = Conversion.ToDouble(CgsAmount2);
							vd7.Commission = Conversion.ToDouble(item.CommissionAmount);
							vd7.GpNo = Conversion.ToInt(item.GpNo);
							vd7.VehicleNo = item.VehicleNo;
							vd7.JobLotId = Conversion.ToInt(item.JobLotId);
							vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd7);
							VoucherDetail vd9 = new VoucherDetail();
							vd9.AccountId = Conversion.ToInt(find3[0].COGSGLAC);
							vd9.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							vd9.Comments = Conversion.ToString(RemarksCgs2 + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd9.CreditAmount = Conversion.ToDouble(CgsAmount2);
							vd9.ItemId = Conversion.ToInt(item.ItemId);
							vd9.QtyIn = Conversion.ToDouble(item.ItemQty);
							vd9.WeightIn = Conversion.ToDouble(item.ItemWeight);
							vd9.ItemCgsRate = Conversion.ToDouble(item.CgsRate);
							vd9.ItemAmount = Conversion.ToDouble(CgsAmount2);
							vd9.Commission = Conversion.ToDouble(item.CommissionAmount);
							vd9.GpNo = Conversion.ToInt(item.GpNo);
							vd9.VehicleNo = item.VehicleNo;
							vd9.JobLotId = Conversion.ToInt(item.JobLotId);
							vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd9);
							continue;
						}
						throw new Exception("Item GlAccount Not found");
					}
					throw new Exception("Item GlAccount Not found");
				}
			}
		}
		return vh;
	}

	public static VoucherHead OtherExpenseFinancial(Architecture.Model.pcc.InvSaleInvoice obj, List<InventoryItemsOther> dtItemOther)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861) && obj.InvSaleInvoiceExpenseList.Count > 0 && Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")))
		{
			foreach (Architecture.Model.pcc.InvSaleInvoiceExpense item in obj.InvSaleInvoiceExpenseList.ToList())
			{
				if (item.Amount > 0m)
				{
					if (dtItemOther.Count <= 0)
					{
						throw new Exception("Other Items not found");
					}
					List<InventoryItemsOther> find = dtItemOther.Where((InventoryItemsOther x) => x.Id == Convert.ToInt32(item.InvOtherItemId)).ToList();
					if (find.Count <= 0)
					{
						throw new Exception("Other Items Sale GL Account not found");
					}
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd2.AgainstAccountId = Conversion.ToInt(find[0].SaleGLAcId);
					vd2.Comments = Conversion.ToString(find[0].OtherItemName + " " + item.Remarks);
					vd2.DebitAmount = Conversion.ToDouble(item.Amount);
					vd2.SubsidiaryTypeId = 1;
					vd2.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd2);
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(find[0].SaleGLAcId);
					vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd.Comments = Conversion.ToString(find[0].OtherItemName + " " + item.Remarks);
					vd.CreditAmount = Conversion.ToDouble(item.Amount);
					vd.SubsidiaryAgainstTypeId = 1;
					vd.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd);
				}
			}
		}
		return vh;
	}

	public static VoucherHead ContractWagesFinancial(Architecture.Model.pcc.InvSaleInvoice obj, List<SupplierCustomerGlAccountId> dtSupplier)
	{
		VoucherHead vh = new VoucherHead();
		if (obj.DocumentTypeId == 1856 && obj.InvSaleInvoiceWagesDetailList.Count > 0)
		{
			foreach (InvSaleInvoiceWagesDetail item in obj.InvSaleInvoiceWagesDetailList.ToList())
			{
				if (item.NetAmount > 0m)
				{
					if (dtSupplier.Count <= 0)
					{
						throw new Exception("Contractor record not found");
					}
					List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(item.ContractorId)).ToList();
					if (find.Count <= 0)
					{
						throw new Exception("Contractor GlAccountId not found");
					}
					DataTable dtAct = new DataTable();
					dtAct = ((item.WagesTypeId != 2) ? CommonServies.GetActivityGlIdAndName(item.ContractorWagesRateScheduleId) : CommonServies.GetActivityGlIdAndNameForExtra(item.ContractorWagesRateScheduleId));
					if (dtAct == null || dtAct.Rows.Count == 0)
					{
						throw new Exception("Activity Record Not Found Against ScheduleId " + item.ContractorWagesRateScheduleId);
					}
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(dtAct.Rows[0]["GlAccountId"]);
					vd2.AgainstAccountId = Conversion.ToInt(find[0].GlAccountId);
					vd2.Comments = item.Remarks;
					vd2.DebitAmount = Conversion.ToDouble(item.NetAmount);
					vd2.SubsidiaryAgainstTypeId = 1;
					vd2.SubsidiaryAgainstAccountId = Conversion.ToInt(item.ContractorId);
					vh.voucherDetailList.Add(vd2);
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(find[0].GlAccountId);
					vd.AgainstAccountId = Conversion.ToInt(dtAct.Rows[0]["GlAccountId"]);
					vd.Comments = Conversion.ToString(item.Remarks);
					vd.CreditAmount = Conversion.ToDouble(item.NetAmount);
					vd.SubsidiaryTypeId = 1;
					vd.SubsidiaryAccountId = Conversion.ToInt(item.ContractorId);
					vd.SupplierCustomerId = Conversion.ToInt(item.ContractorId);
					vh.voucherDetailList.Add(vd);
				}
			}
		}
		return vh;
	}
}
