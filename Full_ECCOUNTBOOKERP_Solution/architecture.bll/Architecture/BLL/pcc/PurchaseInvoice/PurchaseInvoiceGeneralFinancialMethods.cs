using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;
using Newtonsoft.Json;

namespace Architecture.BLL.pcc.PurchaseInvoice;

public class PurchaseInvoiceGeneralFinancialMethods
{
	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.pcc.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1859 || obj.DocumentTypeId == 1865) && obj.InvPurchaseInvoiceJournalList.Count > 0)
		{
			foreach (Architecture.Model.pcc.InvPurchaseInvoiceJournal item in obj.InvPurchaseInvoiceJournalList.ToList())
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
					vd.SubsidiaryAccountId = Conversion.ToInt(item.SupplierCustomerId);
					vd.SupplierCustomerId = Conversion.ToInt(item.SupplierCustomerId);
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

	public static VoucherHead ItemAndCustomerByWeightFinancial(Architecture.Model.pcc.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1859 || obj.DocumentTypeId == 1865) && obj.InvPurchaseInvoiceDetailList.Count > 0)
		{
			List<Architecture.Model.pcc.InvPurchaseInvoiceDetail> itemListWithUniqueItems = new List<Architecture.Model.pcc.InvPurchaseInvoiceDetail>();
			List<Architecture.Model.pcc.InvPurchaseInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<Architecture.Model.pcc.InvPurchaseInvoiceDetail>>(JsonConvert.SerializeObject(obj.InvPurchaseInvoiceDetailList.ToList()));
			for (int i = 0; i < dummyArray.Count; i++)
			{
				if (i == 0)
				{
					itemListWithUniqueItems.Add(dummyArray[i]);
				}
				if (i <= 0)
				{
					continue;
				}
				bool flagForListWithUniqueItem = false;
				for (int k = 0; k < itemListWithUniqueItems.Count; k++)
				{
					if (itemListWithUniqueItems[k].ItemId == dummyArray[i].ItemId)
					{
						flagForListWithUniqueItem = true;
						break;
					}
				}
				if (!flagForListWithUniqueItem)
				{
					itemListWithUniqueItems.Add(dummyArray[i]);
				}
			}
			for (int j = 0; j < itemListWithUniqueItems.ToList().Count; j++)
			{
				decimal itemAmountAtI = default(decimal);
				decimal itemExpenseAmountAtI = default(decimal);
				decimal itemQtyAtI = default(decimal);
				decimal itemWeightAtI = default(decimal);
				decimal JournalAmountAti = default(decimal);
				decimal CommissionAmountAtI = default(decimal);
				decimal FreightAmountAtI = default(decimal);
				for (int l = 0; l < dummyArray.Count; l++)
				{
					if (itemListWithUniqueItems[j].ItemId == dummyArray[l].ItemId)
					{
						itemQtyAtI += dummyArray[l].ItemQty;
						itemWeightAtI += dummyArray[l].ItemNetWeight;
						itemAmountAtI += dummyArray[l].ItemAmount;
						itemExpenseAmountAtI += dummyArray[l].ExpenseAmount;
						JournalAmountAti += dummyArray[l].JournalAmount;
						CommissionAmountAtI += dummyArray[l].CommissionAmount;
						FreightAmountAtI += dummyArray[l].FreightAmount;
					}
				}
				itemListWithUniqueItems[j].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[j].ItemNetWeight = itemWeightAtI;
				itemListWithUniqueItems[j].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[j].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[j].JournalAmount = JournalAmountAti;
				itemListWithUniqueItems[j].CommissionAmount = CommissionAmountAtI;
				itemListWithUniqueItems[j].FreightAmount = FreightAmountAtI;
			}
			decimal TaxAmount = default(decimal);
			decimal FreightAmount = default(decimal);
			decimal ExpAmount = default(decimal);
			decimal CommAmount = default(decimal);
			foreach (Architecture.Model.pcc.InvPurchaseInvoiceDetail item4 in obj.InvPurchaseInvoiceDetailList.ToList())
			{
				string Remarks = string.Empty;
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find4 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
					if (find4.Count > 0)
					{
						if (item4.ActionTypeId == 3)
						{
							continue;
						}
						TaxAmount += item4.TaxAmount;
						FreightAmount += item4.FreightAmount;
						ExpAmount += item4.ExpenseAmount;
						CommAmount += item4.CommissionAmount;
						if (obj.DocumentTypeId == 1859)
						{
							if (obj.RemarksHeader != string.Empty)
							{
								Remarks = obj.RemarksHeader;
							}
							Remarks = Remarks + "  Purchase " + Conversion.ToString(item4.ItemQty) + " Qty ( " + Conversion.ToString(item4.VarientEquivalent);
							Remarks = Remarks + "  " + Conversion.ToString(item4.ItemNetWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + " @NetRate. " + Conversion.ToString(item4.ItemRate) + "  = Rs. " + Conversion.ToString(item4.ItemAmount);
							if (item4.TaxAmount > 0m)
							{
								Remarks = Remarks + " Tax " + Math.Round(item4.TaxAmount, 3);
							}
							if (obj.PaymentTermsId == 2)
							{
								Remarks = Remarks + " Term " + obj.DueDays + " days Credit DueDate " + Conversion.ToDateTime(obj.DueDate).ToString("dd-MMM-yyyy");
							}
							else if (obj.PaymentTermsId == 1)
							{
								Remarks += " Term Cash";
							}
							if (item4.VehicleNo != null && item4.VehicleNo != string.Empty && item4.VehicleNo != "0")
							{
								Remarks = Remarks + " Vehicle # " + item4.VehicleNo;
							}
							if (item4.GpNo != 0)
							{
								Remarks = Remarks + " Gp # " + item4.GpNo;
							}
							if (item4.OrderNo > 0)
							{
								Remarks = Remarks + " Under contract # " + item4.OrderNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								Remarks = Remarks + " Manual # " + obj.ManualBillNo;
							}
							Remarks = Remarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
						}
						else if (obj.DocumentTypeId == 1865)
						{
							if (obj.RemarksHeader != string.Empty)
							{
								Remarks = obj.RemarksHeader;
							}
							Remarks = Remarks + "  Purchase Return" + Conversion.ToString(item4.ItemQty) + " Qty ( " + Conversion.ToString(item4.VarientEquivalent);
							Remarks = Remarks + "  " + Conversion.ToString(item4.ItemNetWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + " @NetRate. " + Conversion.ToString(item4.ItemRate) + "  = Rs. " + Conversion.ToString(item4.ItemAmount);
							if (item4.TaxAmount > 0m)
							{
								Remarks = Remarks + " Tax " + Math.Round(item4.TaxAmount, 3);
							}
							if (obj.PaymentTermsId == 2)
							{
								Remarks = Remarks + " Term " + obj.DueDays + " days Credit DueDate " + Conversion.ToDateTime(obj.DueDate).ToString("dd-MMM-yyyy");
							}
							else if (obj.PaymentTermsId == 1)
							{
								Remarks += " Term Cash";
							}
							if (item4.VehicleNo != null && item4.VehicleNo != string.Empty && item4.VehicleNo != "0")
							{
								Remarks = Remarks + " Vehicle # " + item4.VehicleNo;
							}
							if (item4.GpNo != 0)
							{
								Remarks = Remarks + " Gp # " + item4.GpNo;
							}
							if (item4.OrderNo > 0)
							{
								Remarks = Remarks + " Under contract # " + item4.OrderNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								Remarks = Remarks + " Manual # " + obj.ManualBillNo;
							}
							Remarks = Remarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
						}
						if (obj.DocumentTypeId != 1865)
						{
							VoucherDetail vd3 = new VoucherDetail();
							vd3.LineId = 0;
							vd3.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							vd3.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd3.Comments = Conversion.ToString(Remarks);
							vd3.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
							vd3.ItemId = Conversion.ToInt(item4.ItemId);
							vd3.QtyIn = Conversion.ToDouble(item4.ItemQty);
							vd3.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd3.WeightIn = Conversion.ToDouble(item4.ItemNetWeight);
							vd3.RateCut = Conversion.ToDouble(item4.RateCut);
							vd3.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd3.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd3.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd3.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd3.OrderNo = Conversion.ToInt(item4.OrderNo);
							vd3.GpNo = Conversion.ToInt(item4.GpNo);
							vd3.VehicleNo = item4.VehicleNo;
							vd3.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd3.SubsidiaryAgainstTypeId = 1;
							vd3.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd3.DMultiCurrencyId = item4.CurrencyId;
							vd3.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd3.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vh.voucherDetailList.Add(vd3);
							VoucherDetail vd5 = new VoucherDetail();
							vd5.LineId = 0;
							vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd5.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							vd5.Comments = Conversion.ToString(Remarks);
							vd5.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
							vd5.ItemId = Conversion.ToInt(item4.ItemId);
							vd5.QtyIn = Conversion.ToDouble(item4.ItemQty);
							vd5.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd5.WeightIn = Conversion.ToDouble(item4.ItemNetWeight);
							vd5.RateCut = Conversion.ToDouble(item4.RateCut);
							vd5.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd5.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd5.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd5.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd5.OrderNo = Conversion.ToInt(item4.OrderNo);
							vd5.GpNo = Conversion.ToInt(item4.GpNo);
							vd5.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd5.VehicleNo = item4.VehicleNo;
							vd5.DMultiCurrencyId = item4.CurrencyId;
							vd5.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd5.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd5.SubsidiaryTypeId = 1;
							vd5.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd5);
						}
						else if (obj.DocumentTypeId == 1865)
						{
							VoucherDetail vd4 = new VoucherDetail();
							vd4.LineId = 0;
							vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd4.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							vd4.Comments = Conversion.ToString(Remarks);
							vd4.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
							vd4.ItemId = Conversion.ToInt(item4.ItemId);
							vd4.QtyOut = Conversion.ToDouble(item4.ItemQty);
							vd4.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd4.WeightOut = Conversion.ToDouble(item4.ItemNetWeight);
							vd4.RateCut = Conversion.ToDouble(item4.RateCut);
							vd4.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd4.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd4.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd4.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd4.OrderNo = Conversion.ToInt(item4.OrderNo);
							vd4.GpNo = Conversion.ToInt(item4.GpNo);
							vd4.VehicleNo = item4.VehicleNo;
							vd4.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd4.SubsidiaryTypeId = 1;
							vd4.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd4.DMultiCurrencyId = item4.CurrencyId;
							vd4.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd4.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vh.voucherDetailList.Add(vd4);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.LineId = 0;
							vd6.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							vd6.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd6.Comments = Conversion.ToString(Remarks);
							vd6.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
							vd6.ItemId = Conversion.ToInt(item4.ItemId);
							vd6.QtyOut = Conversion.ToDouble(item4.ItemQty);
							vd6.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd6.WeightOut = Conversion.ToDouble(item4.ItemNetWeight);
							vd6.RateCut = Conversion.ToDouble(item4.RateCut);
							vd6.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd6.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd6.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd6.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd6.OrderNo = Conversion.ToInt(item4.OrderNo);
							vd6.GpNo = Conversion.ToInt(item4.GpNo);
							vd6.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd6.VehicleNo = item4.VehicleNo;
							vd6.DMultiCurrencyId = item4.CurrencyId;
							vd6.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd6.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
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
			if (TaxAmount > 0m && obj.TaxAccountId > 0)
			{
				if (obj.DocumentTypeId == 1859)
				{
					VoucherDetail vd16 = new VoucherDetail();
					vd16.AccountId = Conversion.ToInt(obj.TaxAccountId);
					vd16.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd16.DebitAmount = Conversion.ToDouble(TaxAmount);
					vd16.Comments = "Tax " + TaxAmount;
					vd16.SubsidiaryAgainstTypeId = 1;
					vd16.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd16);
					VoucherDetail vd11 = new VoucherDetail();
					vd11.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd11.AgainstAccountId = Conversion.ToInt(obj.TaxAccountId);
					vd11.CreditAmount = Conversion.ToDouble(TaxAmount);
					vd11.Comments = "Tax " + TaxAmount;
					vd11.SubsidiaryTypeId = 1;
					vd11.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vd11.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd11);
				}
				else if (obj.DocumentTypeId == 1865)
				{
					VoucherDetail vd17 = new VoucherDetail();
					vd17.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd17.AgainstAccountId = Conversion.ToInt(obj.TaxAccountId);
					vd17.DebitAmount = Conversion.ToDouble(TaxAmount);
					vd17.Comments = "Tax " + TaxAmount;
					vd17.SubsidiaryTypeId = 1;
					vd17.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vd17.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd17);
					VoucherDetail vd12 = new VoucherDetail();
					vd12.AccountId = Conversion.ToInt(obj.TaxAccountId);
					vd12.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd12.CreditAmount = Conversion.ToDouble(TaxAmount);
					vd12.Comments = "Tax " + TaxAmount;
					vd12.SubsidiaryAgainstTypeId = 1;
					vd12.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd12);
				}
			}
			if (ExpAmount > 0m && obj.DocumentTypeId == 1859)
			{
				string ExpRemarks = "";
				foreach (Architecture.Model.pcc.InvPurchaseInvoiceDetail item3 in itemListWithUniqueItems.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
						if (find3.Count > 0)
						{
							if (item3.ActionTypeId != 3)
							{
								ExpRemarks = Conversion.ToString(find3[0].ItemName) + " Qty " + Conversion.ToString(item3.ItemQty) + "  = Rs. " + Conversion.ToString(item3.ItemAmount);
								if (item3.VehicleNo != null && item3.VehicleNo != string.Empty && item3.VehicleNo != "0")
								{
									ExpRemarks = ExpRemarks + " Vehicle # " + item3.VehicleNo;
								}
								if (item3.GpNo != 0)
								{
									ExpRemarks = ExpRemarks + " Gp # " + item3.GpNo;
								}
								if (item3.OrderNo > 0)
								{
									ExpRemarks = ExpRemarks + " Under contract # " + item3.OrderNo;
								}
								if (item3.ExpenseAmount > 0m)
								{
									VoucherDetail vd15 = new VoucherDetail();
									vd15.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
									vd15.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
									vd15.DebitAmount = Conversion.ToDouble(item3.ExpenseAmount);
									vd15.Comments = ExpRemarks;
									vd15.JobLotId = Conversion.ToInt(item3.JobLotId);
									vd15.SubsidiaryAgainstTypeId = 1;
									vd15.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd15);
									VoucherDetail vd10 = new VoucherDetail();
									vd10.AccountId = Conversion.ToInt(vh.RefAccountId);
									vd10.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
									vd10.CreditAmount = Conversion.ToDouble(item3.ExpenseAmount);
									vd10.Comments = ExpRemarks;
									vd10.JobLotId = Conversion.ToInt(item3.JobLotId);
									vd10.SubsidiaryTypeId = 1;
									vd10.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
									vd10.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd10);
								}
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
					throw new Exception("Item Record Not found");
				}
			}
			if (CommAmount > 0m && obj.CommissionAgentId > 0)
			{
				string CommRemarks = "";
				foreach (Architecture.Model.pcc.InvPurchaseInvoiceDetail item2 in itemListWithUniqueItems.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
						if (find2.Count > 0)
						{
							if (item2.ActionTypeId == 3)
							{
								continue;
							}
							CommRemarks = Conversion.ToString(find2[0].ItemName) + " Qty " + Conversion.ToString(item2.ItemQty) + "  = Rs. " + Conversion.ToString(item2.ItemAmount);
							if (item2.VehicleNo != null && item2.VehicleNo != string.Empty && item2.VehicleNo != "0")
							{
								CommRemarks = CommRemarks + " Vehicle # " + item2.VehicleNo;
							}
							if (item2.GpNo != 0)
							{
								CommRemarks = CommRemarks + " Gp # " + item2.GpNo;
							}
							if (item2.OrderNo > 0)
							{
								CommRemarks = CommRemarks + " Under contract # " + item2.OrderNo;
							}
							if (item2.CommissionAmount > 0m)
							{
								if (obj.DocumentTypeId == 1859)
								{
									VoucherDetail vd13 = new VoucherDetail();
									vd13.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd13.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
									vd13.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
									vd13.Comments = CommRemarks + " " + obj.CommissionRemarks;
									vd13.JobLotId = Conversion.ToInt(item2.JobLotId);
									vd13.SubsidiaryAgainstTypeId = 1;
									vd13.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.CommissionAgentId);
									vh.voucherDetailList.Add(vd13);
									VoucherDetail vd8 = new VoucherDetail();
									vd8.AccountId = Conversion.ToInt(vh.AgainstAccountId);
									vd8.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd8.CreditAmount = Conversion.ToDouble(item2.CommissionAmount);
									vd8.Comments = CommRemarks + " " + obj.CommissionRemarks;
									vd8.JobLotId = Conversion.ToInt(item2.JobLotId);
									vd8.SubsidiaryTypeId = 1;
									vd8.SubsidiaryAccountId = Conversion.ToInt(obj.CommissionAgentId);
									vd8.SupplierCustomerId = Conversion.ToInt(obj.CommissionAgentId);
									vh.voucherDetailList.Add(vd8);
								}
								else if (obj.DocumentTypeId == 1865)
								{
									VoucherDetail vd14 = new VoucherDetail();
									vd14.AccountId = Conversion.ToInt(vh.AgainstAccountId);
									vd14.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd14.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
									vd14.Comments = CommRemarks + " " + obj.CommissionRemarks;
									vd14.JobLotId = Conversion.ToInt(item2.JobLotId);
									vd14.SubsidiaryTypeId = 1;
									vd14.SubsidiaryAccountId = Conversion.ToInt(obj.CommissionAgentId);
									vd14.SupplierCustomerId = Conversion.ToInt(obj.CommissionAgentId);
									vh.voucherDetailList.Add(vd14);
									VoucherDetail vd9 = new VoucherDetail();
									vd9.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
									vd9.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
									vd9.CreditAmount = Conversion.ToDouble(item2.CommissionAmount);
									vd9.Comments = CommRemarks + " " + obj.CommissionRemarks;
									vd9.JobLotId = Conversion.ToInt(item2.JobLotId);
									vd9.SubsidiaryAgainstTypeId = 1;
									vd9.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.CommissionAgentId);
									vh.voucherDetailList.Add(vd9);
								}
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
					throw new Exception("Item Record Not found");
				}
			}
			if (FreightAmount > 0m && obj.TransporterId > 0)
			{
				string TransRemarks = "";
				foreach (Architecture.Model.pcc.InvPurchaseInvoiceDetail item in itemListWithUniqueItems.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0)
						{
							if (item.ActionTypeId != 3)
							{
								TransRemarks = Conversion.ToString(find[0].ItemName) + " Qty " + Conversion.ToString(item.ItemQty) + "  = Rs. " + Conversion.ToString(item.ItemAmount);
								if (item.VehicleNo != null && item.VehicleNo != string.Empty && item.VehicleNo != "0")
								{
									TransRemarks = TransRemarks + " Vehicle # " + item.VehicleNo;
								}
								if (item.GpNo != 0)
								{
									TransRemarks = TransRemarks + " Gp # " + item.GpNo;
								}
								if (item.OrderNo > 0)
								{
									TransRemarks = TransRemarks + " Under contract # " + item.OrderNo;
								}
								if (item.FreightAmount > 0m)
								{
									VoucherDetail vd2 = new VoucherDetail();
									vd2.LineId = 0;
									vd2.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
									vd2.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.TransporterGlAccountId);
									vd2.Comments = obj.TransporterRemarks + " " + TransRemarks;
									vd2.DebitAmount = Conversion.ToDouble(item.FreightAmount);
									vh.voucherDetailList.Add(vd2);
									VoucherDetail vd7 = new VoucherDetail();
									vd7.AccountId = Conversion.ToInt(obj.CommonIdsForFinancials.TransporterGlAccountId);
									vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
									vd7.CreditAmount = Conversion.ToDouble(item.FreightAmount);
									vd7.Comments = obj.TransporterRemarks + " " + TransRemarks;
									vh.voucherDetailList.Add(vd7);
								}
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
					throw new Exception("Item Record Not found");
				}
			}
		}
		return vh;
	}
}
