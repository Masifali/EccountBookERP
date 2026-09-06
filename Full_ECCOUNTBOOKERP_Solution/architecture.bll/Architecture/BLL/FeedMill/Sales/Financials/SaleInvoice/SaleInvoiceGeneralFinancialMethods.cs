using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Sales;
using Architecture.Model.Inventory;

namespace Architecture.BLL.FeedMill.Sales.Financials.SaleInvoice;

public class SaleInvoiceGeneralFinancialMethods
{
	public static VoucherHead ItemAndCustomerByWeightFinancial(Architecture.Model.FeedMill.Sales.InvSaleInvoice obj, List<GetItemGlIdsandItemName> dtitem, List<SupplierCustomerGlAccountId> dtSupplier)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1115 && obj.InvSaleInvoiceDetaillist.Count > 0)
		{
			double OtherExpensesAmount = 0.0;
			double TaxTotalAmount = 0.0;
			foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail item4 in obj.InvSaleInvoiceDetaillist.ToList())
			{
				if (item4.ActionTypeId == 3)
				{
					continue;
				}
				string Remarks = string.Empty;
				TaxTotalAmount += item4.TaxAmount;
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find4 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
					if (find4.Count > 0)
					{
						if (obj.DocumentTypeId == 1115)
						{
							if (item4.SaleOrder > 0)
							{
								Remarks = "Contract No. " + item4.SaleOrder + "  ";
							}
							Remarks = Remarks + Conversion.ToString(find4[0].ItemName) + " Bags " + Conversion.ToString(item4.ItemQty) + " @Rate. " + Conversion.ToString(item4.ItemRate) + " " + item4.RateUom.ToString();
							if (obj.PaymentTermId == 2)
							{
								Remarks = Remarks + "  Term " + obj.DueDays + " days Credit   DueDate " + Conversion.ToDateTime(obj.DueDate).ToString("dd-MMM-yyyy");
							}
							else if (obj.PaymentTermId == 1)
							{
								Remarks += " Term Cash";
							}
							if (item4.GpNo != 0)
							{
								Remarks = Remarks + " Gp # " + item4.GpNo;
							}
							if (item4.VehicleNo != null && Conversion.ToString(item4.VehicleNo) != string.Empty && Conversion.ToString(item4.VehicleNo) != "0")
							{
								Remarks = Remarks + " Vehicle # " + item4.VehicleNo;
							}
						}
						else
						{
							Remarks = "  Sale " + Conversion.ToString(item4.ItemQty) + " Bags  ( " + Conversion.ToString(item4.GrossWeight);
							if (item4.EBTotalWt > 0.0)
							{
								Remarks = Remarks + " , " + Math.Round(item4.EBTotalWt, 3);
							}
							Remarks = Remarks + " , " + Conversion.ToString(item4.NetBillWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + " @NetRate. " + Conversion.ToString(item4.ItemRate) + "   = Rs. " + Conversion.ToString(item4.ItemAmount);
							if (item4.ItemDiscount > 0.0 && item4.ItemDiscountAmount > 0.0)
							{
								Remarks = Remarks + " Discount% " + Conversion.ToString(item4.ItemDiscount) + " DiscountAmount " + Math.Round(item4.ItemDiscountAmount);
							}
							if (item4.RateCut != 0.0)
							{
								Remarks = Remarks + " RateCut " + Conversion.ToString(item4.RateCut) + " RateCutAmount " + Math.Round(item4.RateCutAmount);
							}
							if (item4.CommissionAmount > 0.0)
							{
								Remarks = Remarks + " CommType " + Conversion.ToString(obj.CommissionType) + " Commission paid @ " + Conversion.ToString(obj.CommRate) + " = Rs. " + Math.Round(item4.CommissionAmount);
							}
							if (item4.FreightAmount > 0.0)
							{
								Remarks = Remarks + " Freight Outward = Rs. " + Math.Round(item4.FreightAmount);
							}
							if (item4.VehicleNo != null && Conversion.ToString(item4.VehicleNo) != string.Empty && Conversion.ToString(item4.VehicleNo) != "0")
							{
								Remarks = Remarks + " Vehicle # " + item4.VehicleNo;
							}
							if (item4.GpNo != 0)
							{
								Remarks = Remarks + " Gp # " + item4.GpNo;
							}
							if (item4.ExpenseAmount > 0.0)
							{
								Remarks = Remarks + " Expense Amount = Rs. " + Math.Round(item4.ExpenseAmount);
							}
							if (item4.SaleOrder > 0)
							{
								Remarks = Remarks + " Under contract # " + item4.SaleOrder;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								Remarks = Remarks + " Manual # " + obj.ManualBillNo;
							}
							if (obj.RemarksHeader != string.Empty)
							{
								Remarks = Remarks + "  " + obj.RemarksHeader;
							}
							Remarks = Remarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
						}
						if (obj.DocumentTypeId != 1611)
						{
							VoucherDetail vd17 = new VoucherDetail();
							vd17.LineId = 0;
							vd17.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd17.AgainstAccountId = Conversion.ToInt(find4[0].SaleGLAC);
							vd17.Comments = Conversion.ToString(Remarks);
							vd17.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
							vd17.ItemId = Conversion.ToInt(item4.ItemId);
							vd17.QtyOut = Conversion.ToDouble(item4.ItemQty);
							vd17.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd17.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
							vd17.RateCut = Conversion.ToDouble(item4.RateCut);
							vd17.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
							vd17.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd17.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd17.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd17.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd17.OrderNo = Conversion.ToInt(item4.SaleOrder);
							vd17.GpNo = Conversion.ToInt(item4.GpNo);
							vd17.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd17.VehicleNo = item4.VehicleNo;
							vd17.DMultiCurrencyId = item4.CurrencyId;
							vd17.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd17.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd17.SubsidiaryTypeId = 1;
							vd17.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd17.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd17);
							VoucherDetail vd16 = new VoucherDetail();
							vd16.LineId = 0;
							vd16.AccountId = Conversion.ToInt(find4[0].SaleGLAC);
							vd16.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd16.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd16.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
							vd16.ItemId = Conversion.ToInt(item4.ItemId);
							vd16.QtyOut = Conversion.ToDouble(item4.ItemQty);
							vd16.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd16.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
							vd16.RateCut = Conversion.ToDouble(item4.RateCut);
							vd16.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
							vd16.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd16.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd16.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd16.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd16.OrderNo = Conversion.ToInt(item4.SaleOrder);
							vd16.GpNo = Conversion.ToInt(item4.GpNo);
							vd16.VehicleNo = item4.VehicleNo;
							vd16.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd16.SubsidiaryAgainstTypeId = 1;
							vd16.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd16.DMultiCurrencyId = item4.CurrencyId;
							vd16.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd16.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vh.voucherDetailList.Add(vd16);
						}
						if (obj.DocumentTypeId == 1116)
						{
							VoucherDetail vd22 = new VoucherDetail();
							vd22.LineId = 0;
							vd22.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							vd22.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd22.Comments = Conversion.ToString(Remarks);
							vd22.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
							vd22.ItemId = Conversion.ToInt(item4.ItemId);
							vd22.QtyIn = Conversion.ToDouble(item4.ItemQty);
							vd22.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd22.WeightIn = Conversion.ToDouble(item4.NetBillWeight);
							vd22.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd22.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd22.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd22.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd22.OrderNo = Conversion.ToInt(item4.SaleOrder);
							vd22.GpNo = Conversion.ToInt(item4.GpNo);
							vd22.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd22.VehicleNo = item4.VehicleNo;
							vd22.DMultiCurrencyId = item4.CurrencyId;
							vd22.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd22.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd22.SubsidiaryAgainstTypeId = 1;
							vd22.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd22);
							OtherExpensesAmount += vd22.Expenses;
							VoucherDetail vd15 = new VoucherDetail();
							vd15.LineId = 0;
							vd15.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd15.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							vd15.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd15.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
							vd15.ItemId = Conversion.ToInt(item4.ItemId);
							vd15.QtyIn = Conversion.ToDouble(item4.ItemQty);
							vd15.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd15.WeightIn = Conversion.ToDouble(item4.NetBillWeight);
							vd15.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd15.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd15.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd15.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd15.OrderNo = Conversion.ToInt(item4.SaleOrder);
							vd15.GpNo = Conversion.ToInt(item4.GpNo);
							vd15.VehicleNo = item4.VehicleNo;
							vd15.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd15.DMultiCurrencyId = item4.CurrencyId;
							vd15.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd15.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd15.SubsidiaryTypeId = 1;
							vd15.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd15.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd15);
						}
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				throw new Exception("Item Record Not found");
			}
			if (obj.FreightAmount > 0.0 && obj.DocumentTypeId == 1115)
			{
				VoucherDetail vd5 = new VoucherDetail();
				vd5.AccountId = Conversion.ToInt(obj.TransporterDebitGLId);
				vd5.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd5.Comments = "Freight " + obj.CommonIdsForFinancials.CompanyName.ToString();
				vd5.DebitAmount = Conversion.ToDouble(obj.FreightAmount);
				vd5.CreditAmount = Conversion.ToDouble(0);
				if (obj.TransporterDebitGLId == vh.RefAccountId)
				{
					throw new Exception("Freight Account not equal to Party Account Please Check");
				}
				vd5.SubsidiaryTypeId = 1;
				vd5.SubsidiaryAccountId = Conversion.ToInt(obj.TransporterDebitPartyId);
				vd5.SupplierCustomerId = Conversion.ToInt(obj.TransporterDebitPartyId);
				vd5.SubsidiaryAgainstTypeId = vd5.SubsidiaryTypeId;
				vd5.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd5);
				VoucherDetail vd11 = new VoucherDetail();
				vd11.AccountId = Conversion.ToInt(vh.RefAccountId);
				vd11.AgainstAccountId = Conversion.ToInt(obj.TransporterDebitGLId);
				vd11.Comments = "Freight " + obj.CommonIdsForFinancials.CompanyName.ToString();
				vd11.CreditAmount = Conversion.ToDouble(obj.FreightAmount);
				vd11.DebitAmount = Conversion.ToDouble(0);
				vd11.SubsidiaryTypeId = 1;
				vd11.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd11.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
				vd11.SubsidiaryAgainstTypeId = vd11.SubsidiaryTypeId;
				vd11.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.TransporterDebitPartyId);
				vh.voucherDetailList.Add(vd11);
			}
			if (obj.UnloadingDisc > 0.0 && obj.UnloadingDiscAccountId > 0 && obj.DocumentTypeId == 1115)
			{
				VoucherDetail vd4 = new VoucherDetail();
				vd4.AccountId = Conversion.ToInt(obj.UnloadingDiscAccountId);
				vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd4.Comments = "Unloading " + obj.CommonIdsForFinancials.CompanyName.ToString();
				vd4.DebitAmount = Conversion.ToDouble(obj.UnloadingDisc);
				if (obj.UnloadingDiscAccountId == vh.RefAccountId)
				{
					throw new Exception("Unloading Account not equal to Party Account Please Check");
				}
				vh.voucherDetailList.Add(vd4);
				VoucherDetail vd10 = new VoucherDetail();
				vd10.AccountId = Conversion.ToInt(vh.RefAccountId);
				vd10.AgainstAccountId = Conversion.ToInt(obj.UnloadingDiscAccountId);
				vd10.Comments = "Unloading " + obj.CommonIdsForFinancials.CompanyName.ToString();
				vd10.CreditAmount = Conversion.ToDouble(obj.UnloadingDisc);
				vd10.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd10);
			}
			if (obj.CommAmount > 0.0 && obj.CommissionAgentId > 0 && obj.CommissionDebitAcGLId > 0 && obj.DocumentTypeId == 1115)
			{
				if (obj.CommissionDebitAcGLId == vh.AgainstAccountId)
				{
					throw new Exception("Commission Agent Account not equal to Commission Debit Account Please Check");
				}
				VoucherDetail vd3 = new VoucherDetail();
				vd3.AccountId = Conversion.ToInt(obj.CommissionDebitAcGLId);
				vd3.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
				vd3.Comments = "Commission " + obj.CommonIdsForFinancials.BrokerAgentName;
				vd3.DebitAmount = Conversion.ToDouble(obj.CommAmount);
				vd3.ItemRate = Conversion.ToDouble(obj.CommRate);
				vh.voucherDetailList.Add(vd3);
				VoucherDetail vd9 = new VoucherDetail();
				vd9.AccountId = Conversion.ToInt(vh.AgainstAccountId);
				vd9.AgainstAccountId = Conversion.ToInt(obj.CommissionDebitAcGLId);
				vd9.Comments = "Commission " + obj.CommonIdsForFinancials.BrokerAgentName;
				vd9.CreditAmount = Conversion.ToDouble(obj.CommAmount);
				vd9.SupplierCustomerId = Conversion.ToInt(obj.CommissionAgentId);
				vd9.ItemRate = Conversion.ToDouble(obj.CommRate);
				vh.voucherDetailList.Add(vd9);
			}
			if (obj.DiscountAmount > 0m && obj.DocumentTypeId == 1115)
			{
				if (obj.DiscountAccountId == 0)
				{
					throw new Exception("Discount debit account fields is required");
				}
				VoucherDetail vd12 = new VoucherDetail();
				vd12.LineId = 0;
				vd12.AccountId = Conversion.ToInt(obj.DiscountAccountId);
				vd12.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd12.Comments = "Discount to " + obj.CommonIdsForFinancials.CompanyName;
				vd12.DebitAmount = Conversion.ToDouble(obj.DiscountAmount);
				vd12.DMultiCurrencyId = obj.CurrencyId;
				vd12.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
				vd12.DCurrencyAmount = Conversion.ToDouble(obj.FcyAmount);
				vd12.SubsidiaryAgainstTypeId = 1;
				vd12.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd12);
				VoucherDetail vd13 = new VoucherDetail();
				vd13.LineId = 0;
				vd13.AccountId = Conversion.ToInt(vh.RefAccountId);
				vd13.AgainstAccountId = Conversion.ToInt(obj.DiscountAccountId);
				vd13.Comments = "Discount to " + obj.CommonIdsForFinancials.CompanyName;
				vd13.CreditAmount = Conversion.ToDouble(obj.DiscountAmount);
				vd13.DMultiCurrencyId = obj.CurrencyId;
				vd13.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
				vd13.DCurrencyAmount = Conversion.ToDouble(obj.FcyAmount);
				vd13.SubsidiaryTypeId = 1;
				vd13.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd13.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd13);
			}
			if (obj.InvSaleInvoiceExpenseList.Count > 0 && OtherExpensesAmount > 0.0 && obj.DocumentTypeId == 1115)
			{
				if (!Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")))
				{
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail item3 in obj.InvSaleInvoiceDetaillist.ToList())
					{
						if (item3.ActionTypeId == 3)
						{
							continue;
						}
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
							if (find2.Count > 0)
							{
								if (item3.ExpenseAmount > 0.0)
								{
									string ExpeneseRemarks = "";
									ExpeneseRemarks = Conversion.ToString(find2[0].ItemName) + " Bags " + Conversion.ToString(item3.ItemQty);
									if (item3.SaleOrder > 0)
									{
										ExpeneseRemarks = ExpeneseRemarks + " Under contract # " + item3.SaleOrder;
									}
									if (item3.VehicleNo != null && item3.VehicleNo != string.Empty)
									{
										ExpeneseRemarks = ExpeneseRemarks + " Vehicle # " + item3.VehicleNo;
									}
									if (item3.GpNo != 0)
									{
										ExpeneseRemarks = ExpeneseRemarks + " Gp # " + item3.GpNo;
									}
									if (obj.ManualBillNo != string.Empty)
									{
										ExpeneseRemarks = ExpeneseRemarks + " Manual # " + obj.ManualBillNo;
									}
									ExpeneseRemarks = ExpeneseRemarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
									VoucherDetail vd19 = new VoucherDetail();
									vd19.AccountId = Conversion.ToInt(vh.RefAccountId);
									vd19.AgainstAccountId = Conversion.ToInt(find2[0].SaleGLAC);
									vd19.DebitAmount = Conversion.ToDouble(item3.ExpenseAmount);
									vd19.Comments = ExpeneseRemarks;
									vd19.JobLotId = Conversion.ToInt(item3.JobLotId);
									vd19.SubsidiaryTypeId = 1;
									vd19.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
									vd19.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd19);
									VoucherDetail vd18 = new VoucherDetail();
									vd18.AccountId = Conversion.ToInt(find2[0].SaleGLAC);
									vd18.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
									vd18.CreditAmount = Conversion.ToDouble(item3.ExpenseAmount);
									vd18.Comments = ExpeneseRemarks;
									vd18.JobLotId = Conversion.ToInt(item3.JobLotId);
									vd18.SubsidiaryAgainstTypeId = 1;
									vd18.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd18);
								}
								continue;
							}
							throw new Exception("Item Record Not found");
						}
						throw new Exception("Item Record Not found");
					}
				}
				else
				{
					List<InventoryItemsOther> dtItemOther = CommonServies.GetOtherItemForFinancialEffects(obj.OrganizationId, obj.CompanyId);
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceExpense item2 in obj.InvSaleInvoiceExpenseList.ToList())
					{
						if (item2.Amount > 0.0)
						{
							if (dtItemOther.Count <= 0)
							{
								throw new Exception("Other Items not found");
							}
							List<InventoryItemsOther> find3 = dtItemOther.Where((InventoryItemsOther x) => x.Id == Convert.ToInt32(item2.InvRevExpItemId)).ToList();
							if (find3.Count <= 0)
							{
								throw new Exception("Other Items Sale GL Account not found");
							}
							VoucherDetail vd8 = new VoucherDetail();
							vd8.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd8.AgainstAccountId = Conversion.ToInt(find3[0].SaleGLAcId);
							vd8.Comments = Conversion.ToString(find3[0].OtherItemName + " " + item2.Remarks);
							vd8.DebitAmount = item2.Amount;
							vd8.SubsidiaryTypeId = 1;
							vd8.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd8);
							VoucherDetail vd2 = new VoucherDetail();
							vd2.AccountId = Conversion.ToInt(find3[0].SaleGLAcId);
							vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd2.Comments = Conversion.ToString(find3[0].OtherItemName + " " + item2.Remarks);
							vd2.CreditAmount = item2.Amount;
							vd2.SubsidiaryAgainstTypeId = 1;
							vd2.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd2);
						}
					}
				}
			}
			if (TaxTotalAmount > 0.0)
			{
				VoucherDetail vd7 = new VoucherDetail();
				vd7.AccountId = vh.RefAccountId;
				vd7.Comments = "Sales Tax " + obj.CommonIdsForFinancials.CompanyName.ToString();
				vd7.AgainstAccountId = obj.TaxAccountId;
				vd7.DebitAmount = TaxTotalAmount;
				vd7.TaxesTotalAmount = TaxTotalAmount;
				vd7.IsTaxable = "True";
				vd7.TaxTypeId = 2;
				vd7.SubsidiaryTypeId = 1;
				vd7.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd7.SupplierCustomerId = obj.SupplierCustomerId;
				vh.voucherDetailList.Add(vd7);
				VoucherDetail vd14 = new VoucherDetail();
				vd14.AccountId = obj.TaxAccountId;
				vd14.Comments = "Sales Tax " + obj.CommonIdsForFinancials.CompanyName.ToString();
				vd14.AgainstAccountId = vh.RefAccountId;
				vd14.CreditAmount = TaxTotalAmount;
				vd14.TaxesTotalAmount = TaxTotalAmount;
				vd14.IsTaxable = "True";
				vd14.TaxTypeId = 2;
				vd14.SubsidiaryAgainstTypeId = 1;
				vd14.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd14);
			}
			if (obj.InvSaleInvoiceJournalList.Count > 0 && obj.InvSaleInvoiceJournalList != null)
			{
				foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceJournal item5 in obj.InvSaleInvoiceJournalList.ToList())
				{
					if (Conversion.ToInt(item5.ChartofAccountId) != 0)
					{
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = Conversion.ToInt(item5.ChartofAccountId);
						vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd.Comments = Conversion.ToString(item5.JvRemarks);
						vd.DebitAmount = Conversion.ToDouble(item5.JvDebit);
						vd.CreditAmount = Conversion.ToDouble(item5.JvCredit);
						vd.SubsidiaryTypeId = 1;
						vd.SubsidiaryAccountId = Conversion.ToInt(item5.TransporterSupCustId);
						vd.SupplierCustomerId = Conversion.ToInt(item5.TransporterSupCustId);
						vd.SubsidiaryAgainstTypeId = vd.SubsidiaryTypeId;
						vd.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd6.AgainstAccountId = Conversion.ToInt(item5.ChartofAccountId);
						vd6.Comments = Conversion.ToString(item5.JvRemarks);
						vd6.CreditAmount = Conversion.ToDouble(item5.JvDebit);
						vd6.DebitAmount = Conversion.ToDouble(item5.JvCredit);
						vd6.SubsidiaryTypeId = 1;
						vd6.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
						vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd6.SubsidiaryAgainstTypeId = vd.SubsidiaryTypeId;
						vd6.SubsidiaryAgainstAccountId = Conversion.ToInt(item5.TransporterSupCustId);
						vh.voucherDetailList.Add(vd6);
					}
				}
			}
			if (obj.DocumentTypeId != 1116)
			{
				bool ItemWiseCGS = CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 1);
				if (Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow")) || ItemWiseCGS)
				{
					foreach (Architecture.Model.FeedMill.Sales.InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist.ToList())
					{
						if (item.ActionTypeId == 3 || dtitem.Count <= 0)
						{
							continue;
						}
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count <= 0)
						{
							continue;
						}
						ReportsParameters parameters = new ReportsParameters();
						parameters.OrganizationId = obj.OrganizationId;
						parameters.CompanyId = obj.CompanyId;
						parameters.ItemId = item.ItemId;
						parameters.DocDate = obj.DocDate;
						if (obj.Id > 0)
						{
							parameters.DocumentTypeId = obj.DocumentTypeId;
							parameters.Id = obj.Id;
						}
						item.ItemCgsRate = GetAvgRatesAndStockInHand.getAvgRateByItemId(parameters);
						if (item.ItemCgsRate <= 0.0)
						{
							throw new Exception("CGS Rate not found against " + find[0].ItemName + " and row bags is " + item.ItemQty + " and row rate is " + item.ItemRate);
						}
						double CgsAmount = 0.0;
						string RemarksCgs = "";
						if (obj.DocumentTypeId == 1115)
						{
							if (item.NetBillWeight == 0.0)
							{
								throw new Exception("NetBillWeight Not Found Please Check");
							}
							if (item.RateEquivalent == 0.0)
							{
								throw new Exception("RateUOM Not Found Please Check");
							}
							item.ItemCgsRate = item.RateEquivalent * item.ItemCgsRate;
							CgsAmount = item.NetStockWeight / item.RateEquivalent * item.ItemCgsRate;
							RemarksCgs = Conversion.ToString(find[0].ItemName) + " Bags: " + item.ItemQty + " Stock Weight: " + item.NetStockWeight + " Rate: " + item.ItemCgsRate;
							if (item.RateEquivalent > 0.0)
							{
								RemarksCgs = RemarksCgs + " RateUom " + item.RateEquivalent;
							}
						}
						if (CgsAmount == 0.0)
						{
							throw new Exception("CGS Amount cannot be equal to zero");
						}
						VoucherDetail vd21 = new VoucherDetail();
						vd21.LineId = item.LineId;
						vd21.AccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd21.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd21.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd21.DebitAmount = Conversion.ToDouble(CgsAmount);
						vd21.ItemId = Conversion.ToInt(item.ItemId);
						vd21.QtyOut = Conversion.ToDouble(item.ItemQty);
						vd21.WeightOut = Conversion.ToDouble(item.NetStockWeight);
						vd21.ItemCgsRate = Conversion.ToDouble(item.ItemCgsRate);
						vd21.RateCut = Conversion.ToDouble(item.RateCut);
						vd21.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
						vd21.ItemAmount = Conversion.ToDouble(CgsAmount);
						vd21.Expenses = Conversion.ToDouble(item.ExpenseAmount);
						vd21.Commission = Conversion.ToDouble(item.CommissionAmount);
						vd21.Freight = Conversion.ToDouble(item.FreightAmount);
						vd21.OrderNo = Conversion.ToInt(item.SaleOrder);
						vd21.GpNo = Conversion.ToInt(item.GpNo);
						vd21.VehicleNo = item.VehicleNo;
						vd21.JobLotId = Conversion.ToInt(item.JobLotId);
						vd21.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd21.IsCGS = 1;
						vh.voucherDetailList.Add(vd21);
						VoucherDetail vd20 = new VoucherDetail();
						vd20.LineId = item.LineId;
						vd20.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd20.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd20.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
						vd20.CreditAmount = Conversion.ToDouble(CgsAmount);
						vd20.ItemId = Conversion.ToInt(item.ItemId);
						vd20.QtyOut = Conversion.ToDouble(item.ItemQty);
						vd20.WeightOut = Conversion.ToDouble(item.NetStockWeight);
						vd20.ItemCgsRate = Conversion.ToDouble(item.ItemCgsRate);
						vd20.RateCut = Conversion.ToDouble(item.RateCut);
						vd20.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
						vd20.ItemAmount = Conversion.ToDouble(CgsAmount);
						vd20.Expenses = Conversion.ToDouble(item.ExpenseAmount);
						vd20.Commission = Conversion.ToDouble(item.CommissionAmount);
						vd20.Freight = Conversion.ToDouble(item.FreightAmount);
						vd20.OrderNo = Conversion.ToInt(item.SaleOrder);
						vd20.GpNo = Conversion.ToInt(item.GpNo);
						vd20.VehicleNo = item.VehicleNo;
						vd20.JobLotId = Conversion.ToInt(item.JobLotId);
						vd20.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd20.IsCGS = 1;
						vh.voucherDetailList.Add(vd20);
					}
				}
			}
		}
		return vh;
	}
}
