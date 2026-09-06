using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Purchase;
using Architecture.Model.Inventory;
using Newtonsoft.Json;

namespace Architecture.BLL.FeedMill.Financials.PurchaseInvoice;

public class PurchaseInvoiceGeneralFinancialMethods
{
	public static VoucherHead EmptyBagsFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1104 && obj.InvPurchaseInvoiceEmptyBagslist.Count > 0)
		{
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceEmptyBags item in obj.InvPurchaseInvoiceEmptyBagslist.ToList())
			{
				if (item.TypeId == 1)
				{
					if (Conversion.ToInt(item.ItemId) <= 0 || Conversion.ToInt(item.PurchaseQty) <= 0)
					{
						continue;
					}
					if (dtitem.Count <= 0)
					{
						throw new Exception("ItemId Not found against Packing Material");
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName u) => u.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						double Amount = Conversion.ToDouble(item.PurchaseQty) * Conversion.ToDouble(item.Rate);
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						if (item.Remarks == string.Empty || item.Remarks == null)
						{
							vd.Comments = "ItemName: " + find[0].ItemName + " Rate: " + item.Rate + " Amount: " + Amount;
						}
						else
						{
							vd.Comments = Conversion.ToString(item.Remarks);
						}
						if (Amount == 0.0)
						{
							throw new Exception("Packing Material Amount Field Required");
						}
						vd.DebitAmount = Amount;
						vd.CreditAmount = Conversion.ToDouble(0);
						vh.voucherDetailList.Add(vd);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						if (item.Remarks == string.Empty || item.Remarks == null)
						{
							vd3.Comments = "ItemName: " + find[0].ItemName + " Rate: " + item.Rate + " Amount: " + Amount;
						}
						else
						{
							vd3.Comments = Conversion.ToString(item.Remarks);
						}
						vd3.DebitAmount = Conversion.ToDouble(0);
						vd3.CreditAmount = Amount;
						vd3.SupplierCustomerId = obj.SupplierCustomerId;
						vd3.SubsidiaryAccountId = obj.SupplierCustomerId;
						vd3.SubsidiaryTypeId = 1;
						vh.voucherDetailList.Add(vd3);
					}
				}
				else
				{
					if ((item.TypeId != 2 && item.TypeId != 3) || Conversion.ToInt(item.ItemId) <= 0 || Conversion.ToInt(item.PurchaseQty) <= 0)
					{
						continue;
					}
					if (item.CreditAccountId == 0)
					{
						throw new Exception("Credit Account Not Found aginst Packing Material");
					}
					if (dtitem.Count <= 0)
					{
						throw new Exception("ItemId Not found against Packing Material");
					}
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName u) => u.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find2.Count > 0)
					{
						double Amount2 = Conversion.ToDouble(item.PurchaseQty) * Conversion.ToDouble(item.Rate);
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(item.CreditAccountId);
						if (item.Remarks == string.Empty || item.Remarks == null)
						{
							vd2.Comments = "ItemName: " + find2[0].ItemName + " Rate: " + item.Rate + " Amount: " + Amount2;
						}
						else
						{
							vd2.Comments = Conversion.ToString(item.Remarks);
						}
						if (Amount2 == 0.0)
						{
							throw new Exception("Packing Material Amount Field Required");
						}
						vd2.DebitAmount = Amount2;
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(item.CreditAccountId);
						vd4.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						if (item.Remarks == string.Empty || item.Remarks == null)
						{
							vd4.Comments = "ItemName: " + find2[0].ItemName + " Rate: " + item.Rate + " Amount: " + Amount2;
						}
						else
						{
							vd4.Comments = Conversion.ToString(item.Remarks);
						}
						vd4.CreditAmount = Amount2;
						vh.voucherDetailList.Add(vd4);
					}
				}
			}
		}
		return vh;
	}

	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1104 && obj.InvPurchaseInvoicejournalList.Count > 0)
		{
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceJournal item in obj.InvPurchaseInvoicejournalList.ToList())
			{
				if (Conversion.ToInt(item.ChartofAccountId) != 0)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(item.ChartofAccountId);
					vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					if (obj.CommonIdsForFinancials.AutoRemarksStatus)
					{
						vd.Comments = obj.CommonIdsForFinancials.Remarks;
					}
					else
					{
						vd.Comments = Conversion.ToString(item.Remarks);
					}
					vd.DebitAmount = Conversion.ToDouble(item.Debit);
					vd.CreditAmount = Conversion.ToDouble(item.Credit);
					if (item.SupplierCustomerId > 0)
					{
						vd.SupplierCustomerId = item.SupplierCustomerId;
						vd.SubsidiaryAccountId = item.SupplierCustomerId;
						vd.SubsidiaryTypeId = 1;
					}
					vh.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd2.AgainstAccountId = Conversion.ToInt(item.ChartofAccountId);
					if (obj.CommonIdsForFinancials.AutoRemarksStatus)
					{
						vd2.Comments = obj.CommonIdsForFinancials.Remarks;
					}
					else
					{
						vd2.Comments = Conversion.ToString(item.Remarks);
					}
					vd2.DebitAmount = Conversion.ToDouble(item.Credit);
					vd2.CreditAmount = Conversion.ToDouble(item.Debit);
					vd2.SupplierCustomerId = obj.SupplierCustomerId;
					vd2.SubsidiaryAccountId = obj.SupplierCustomerId;
					vd2.SubsidiaryTypeId = 1;
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		return vh;
	}

	public static VoucherHead SalesTaxFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1104 && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item in obj.invPurchaseInvoiceDetailList.ToList())
			{
				if (item.TaxAmount > 0m)
				{
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = obj.TaxAccountId;
					vd2.Comments = item.TaxPercent + "Tax %  " + item.TaxAmount;
					vd2.AgainstAccountId = vh.RefAccountId;
					vd2.DebitAmount = Conversion.ToDouble(item.TaxAmount);
					vd2.TaxesTotalAmount = Conversion.ToDouble(item.TaxAmount);
					vd2.TaxPrcnt = Conversion.ToDouble(item.TaxPercent);
					vd2.IsTaxable = "True";
					vd2.TaxTypeId = item.TaxNameId;
					vh.voucherDetailList.Add(vd2);
					VoucherDetail vd1 = new VoucherDetail();
					vd1.AccountId = vh.RefAccountId;
					vd1.AgainstAccountId = obj.TaxAccountId;
					vd1.Comments = item.TaxPercent + "Tax %  " + item.TaxAmount;
					vd1.CreditAmount = Conversion.ToDouble(item.TaxAmount);
					vd1.TaxesTotalAmount = Conversion.ToDouble(item.TaxAmount);
					vd1.IsTaxable = "True";
					vd1.TaxPrcnt = Conversion.ToDouble(item.TaxPercent);
					vd1.TaxTypeId = item.TaxNameId;
					if (obj.DocumentTypeId == 1104 || obj.DocumentTypeId == 1103)
					{
						vd1.SupplierCustomerId = obj.SupplierCustomerId;
						vd1.SubsidiaryAccountId = obj.SupplierCustomerId;
						vd1.SubsidiaryTypeId = 1;
					}
					vh.voucherDetailList.Add(vd1);
				}
			}
		}
		return vh;
	}

	public static VoucherHead FreightFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1104 && obj.InvPurchaseInvoiceFreightList.Count > 0)
		{
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceFreight r in obj.InvPurchaseInvoiceFreightList.ToList())
			{
				if (Conversion.ToInt(r.TansporterId) > 0 && Conversion.ToDouble(r.DebitAmount) > 0.0)
				{
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = r.TansporterId;
					vd2.AgainstAccountId = obj.CommonIdsForFinancials.OffsetAccountId;
					if (obj.CommonIdsForFinancials.AutoRemarksStatus)
					{
						vd2.Comments = obj.CommonIdsForFinancials.Remarks;
					}
					else if (r.Remarks == null || r.Remarks == string.Empty)
					{
						vd2.Comments = "PartyName :  " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + "  Freight : " + Conversion.ToString(r.CreditAmount);
					}
					else
					{
						vd2.Comments = Conversion.ToString(r.Remarks);
					}
					vd2.DebitAmount = Conversion.ToDouble(r.DebitAmount);
					vd2.CreditAmount = 0.0;
					if (r.SupplierCustomerId > 0)
					{
						vd2.SupplierCustomerId = r.SupplierCustomerId;
						vd2.SubsidiaryAccountId = r.SupplierCustomerId;
						vd2.SubsidiaryTypeId = 1;
					}
					vh.voucherDetailList.Add(vd2);
				}
				if (Conversion.ToInt(r.TansporterId) > 0 && Conversion.ToDouble(r.CreditAmount) > 0.0)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = r.TansporterId;
					vd.AgainstAccountId = obj.CommonIdsForFinancials.OffsetAccountId;
					if (obj.CommonIdsForFinancials.AutoRemarksStatus)
					{
						vd.Comments = obj.CommonIdsForFinancials.Remarks;
					}
					else if (r.Remarks == null || r.Remarks == string.Empty)
					{
						vd.Comments = "SupplierName :  " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + "  Freight : " + Conversion.ToString(r.CreditAmount);
					}
					else
					{
						vd.Comments = Conversion.ToString(r.Remarks);
					}
					vd.DebitAmount = 0.0;
					vd.CreditAmount = Conversion.ToDouble(r.CreditAmount);
					if (r.SupplierCustomerId > 0)
					{
						vd.SupplierCustomerId = r.SupplierCustomerId;
						vd.SubsidiaryAccountId = r.SupplierCustomerId;
						vd.SubsidiaryTypeId = 1;
					}
					vh.voucherDetailList.Add(vd);
				}
			}
		}
		return vh;
	}

	public static VoucherHead BrokeryBeHalfOnPartyFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1104 && obj.BrokerAgentId > 0 && obj.BrokeryAmount > 0m)
		{
			if (obj.SupplierCustomerId == obj.BrokerAgentId)
			{
				throw new Exception("Broker Agent should not be same to Supplier");
			}
			VoucherDetail vd2 = new VoucherDetail();
			vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
			vd2.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
			if (obj.CommonIdsForFinancials.AutoRemarksStatus)
			{
				vd2.Comments = obj.CommonIdsForFinancials.Remarks;
			}
			else
			{
				vd2.Comments = "BrokerAgent: " + obj.CommonIdsForFinancials.BrokerAgentName + "  Brokery Rate" + obj.BrokeryRate + " BrokeryType" + obj.BrokeryType + "  BrokeryAmount" + obj.BrokeryAmount;
			}
			vd2.DebitAmount = Conversion.ToDouble(obj.BrokeryAmount);
			vd2.CreditAmount = 0.0;
			vd2.SupplierCustomerId = obj.SupplierCustomerId;
			vd2.SubsidiaryAccountId = obj.SupplierCustomerId;
			vd2.SubsidiaryTypeId = 1;
			vh.voucherDetailList.Add(vd2);
			VoucherDetail vd = new VoucherDetail();
			vd.AccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
			vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
			if (obj.CommonIdsForFinancials.AutoRemarksStatus)
			{
				vd.Comments = obj.CommonIdsForFinancials.Remarks;
			}
			else
			{
				vd.Comments = "SupplierName: " + obj.CommonIdsForFinancials.CompanyName + "  Brokery Rate" + obj.BrokeryRate + " BrokeryType" + obj.BrokeryType + "  BrokeryAmount" + obj.BrokeryAmount;
			}
			vd.CreditAmount = Conversion.ToDouble(obj.BrokeryAmount);
			vd.DebitAmount = 0.0;
			vd.SubsidiaryAgainstTypeId = 1;
			vd.SubsidiaryAgainstAccountId = obj.BrokerAgentId;
			vh.voucherDetailList.Add(vd);
		}
		return vh;
	}

	public static VoucherHead ItemAndSupplierByWeightFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 1104 && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			bool AutoRemarksFormatII = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "AutoRemarksFormatII"));
			List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail> itemListWithUniqueItems = new List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail>();
			List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail>>(JsonConvert.SerializeObject(obj.invPurchaseInvoiceDetailList.ToList()));
			for (int k = 0; k < dummyArray.Count; k++)
			{
				if (k == 0)
				{
					itemListWithUniqueItems.Add(dummyArray[k]);
				}
				if (k <= 0)
				{
					continue;
				}
				bool flagForListWithUniqueItem = false;
				for (int m = 0; m < itemListWithUniqueItems.Count; m++)
				{
					if (itemListWithUniqueItems[m].ItemId == dummyArray[k].ItemId && itemListWithUniqueItems[m].ItemRate == dummyArray[k].ItemRate && itemListWithUniqueItems[m].WarehouseId == dummyArray[k].WarehouseId)
					{
						flagForListWithUniqueItem = true;
						break;
					}
				}
				if (!flagForListWithUniqueItem)
				{
					itemListWithUniqueItems.Add(dummyArray[k]);
				}
			}
			for (int l = 0; l < itemListWithUniqueItems.ToList().Count; l++)
			{
				if (itemListWithUniqueItems[l].ActionTypeId == 3)
				{
					continue;
				}
				decimal itemAmountAtI = default(decimal);
				decimal itemExpenseAmountAtI = default(decimal);
				decimal itemFreightAmountAtI = default(decimal);
				decimal itemCommissionAmountAtI = default(decimal);
				decimal itemQtyAtI = default(decimal);
				decimal itemWeightAtI = default(decimal);
				decimal itemWeightCutAtI = default(decimal);
				decimal itemRateCutAtI = default(decimal);
				decimal itemEmptyBagsDudectionAtI = default(decimal);
				decimal itemGrossWeightAtI = default(decimal);
				decimal itemBrokeryAmountAtI = default(decimal);
				for (int n = 0; n < dummyArray.Count; n++)
				{
					if (itemListWithUniqueItems[l].ItemId == dummyArray[n].ItemId && itemListWithUniqueItems[l].ItemRate == dummyArray[n].ItemRate && itemListWithUniqueItems[l].WarehouseId == dummyArray[n].WarehouseId)
					{
						itemAmountAtI += dummyArray[n].ItemAmount;
						itemFreightAmountAtI += dummyArray[n].FreightAmount;
						itemExpenseAmountAtI += dummyArray[n].ExpenseAmount;
						itemCommissionAmountAtI += dummyArray[n].CommissionAmount;
						itemBrokeryAmountAtI += dummyArray[n].BrokeryAmount;
						itemQtyAtI += dummyArray[n].ItemQty;
						itemWeightAtI += dummyArray[n].NetBillWeight;
						itemWeightCutAtI += dummyArray[n].WeightCut;
						itemRateCutAtI += dummyArray[n].RateCut;
						itemEmptyBagsDudectionAtI += dummyArray[n].EBTotalWt;
						itemGrossWeightAtI += dummyArray[n].GrossWeight;
					}
				}
				itemListWithUniqueItems[l].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[l].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[l].FreightAmount = itemFreightAmountAtI;
				itemListWithUniqueItems[l].CommissionAmount = itemCommissionAmountAtI;
				itemListWithUniqueItems[l].BrokeryAmount = itemBrokeryAmountAtI;
				itemListWithUniqueItems[l].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[l].NetBillWeight = itemWeightAtI;
				itemListWithUniqueItems[l].WeightCut = itemWeightCutAtI;
				itemListWithUniqueItems[l].RateCut = itemRateCutAtI;
				itemListWithUniqueItems[l].EBTotalWt = itemEmptyBagsDudectionAtI;
				itemListWithUniqueItems[l].GrossWeight = itemGrossWeightAtI;
			}
			string Remarks = string.Empty;
			string VehicleNo = string.Empty;
			string InvoiceHeaderRemarksAuto = "";
			int getOrderNo = 0;
			int getItemId = 0;
			decimal getCommissionAmount = default(decimal);
			decimal getFreightAmount = default(decimal);
			int RowNo = 1;
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item2 in itemListWithUniqueItems.ToList())
			{
				if (item2.ActionTypeId == 3)
				{
					continue;
				}
				Remarks = "";
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find3.Count > 0)
					{
						if (RowNo == 1)
						{
							getOrderNo = item2.PurchaseOrder;
							getItemId = item2.ItemId;
							VehicleNo = item2.VehicleNo;
						}
						item2.ItemName = Conversion.ToString(find3[0].ItemName);
						VoucherDetail vd2 = new VoucherDetail();
						if (obj.DocumentTypeId == 1104)
						{
							if (item2.CommissionAmount > 0m)
							{
								getCommissionAmount += item2.CommissionAmount;
							}
							if (item2.FreightAmount > 0m)
							{
								getFreightAmount += item2.FreightAmount;
							}
							Remarks = Conversion.ToString(find3[0].ItemName) + " ,Purchased Qty " + Conversion.ToDouble(item2.NetBillWeight) + " kg, " + Conversion.ToDouble(item2.ItemQty) + " bags, @Rate " + Conversion.ToDouble(item2.ItemRate - item2.RateCut) + "  " + item2.RateUom.ToString() + " ,from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
							if (item2.VehicleNo != string.Empty)
							{
								Remarks = Remarks + " ,Vehicle # " + item2.VehicleNo;
							}
							if (item2.EBTotalWt > 0m)
							{
								Remarks = Remarks + " ,Bags Weight: " + Conversion.ToDouble(item2.EBTotalWt);
							}
							if (item2.WeightCutTotal > 0m)
							{
								Remarks = Remarks + " ,Deduction: " + Conversion.ToDouble(item2.WeightCutTotal);
							}
							if (item2.CommissionAmount > 0m)
							{
								Remarks = Remarks + " Commission paid @ " + Conversion.ToString(obj.CommRate) + "% = Rs. " + Math.Round(item2.CommissionAmount);
								if (obj.CommissionRemarks != null && obj.CommissionRemarks != string.Empty)
								{
									Remarks = Remarks + " " + obj.CommissionRemarks;
								}
							}
							if (obj.BrokeryAmount > 0m)
							{
								Remarks = Remarks + " Commission deduct @ " + obj.BrokeryRate + "% = Rs. " + Math.Round(obj.BrokeryAmount);
							}
							if (item2.FreightAmount > 0m)
							{
								Remarks = Remarks + " Freight Inward = Rs. " + Math.Round(item2.FreightAmount);
							}
							if (item2.PurchaseOrder > 0)
							{
								Remarks = Remarks + " Under contract # " + item2.PurchaseOrder;
							}
							if (obj.SupplierReferenceNo > 0)
							{
								Remarks = Remarks + " Ref # " + obj.SupplierReferenceNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								Remarks = Remarks + " Manual # " + obj.ManualBillNo;
							}
							obj.CommonIdsForFinancials.AutoRemarksStatus = AutoRemarksFormatII;
							obj.CommonIdsForFinancials.Remarks = Remarks;
							if (getItemId == item2.ItemId && RowNo == 1)
							{
								if (obj.RemarksHeader != string.Empty)
								{
									InvoiceHeaderRemarksAuto = obj.RemarksHeader + " ";
								}
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + Conversion.ToString(find3[0].ItemName) + " Purchased Qty " + Conversion.ToDouble(item2.NetBillWeight) + " kg " + Conversion.ToDouble(item2.ItemQty) + " bags " + Conversion.ToDouble(item2.ItemRate - item2.RateCut) + " Rate";
							}
							else if (getItemId == item2.ItemId && RowNo > 1)
							{
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " , " + Conversion.ToString(find3[0].ItemName) + " Purchased Qty " + Conversion.ToDouble(item2.NetBillWeight) + " kg " + Conversion.ToDouble(item2.ItemQty) + " bags " + Conversion.ToDouble(item2.ItemRate - item2.RateCut) + " Rate";
							}
							else if (getItemId != item2.ItemId)
							{
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + Conversion.ToString(find3[0].ItemName) + " Purchased Qty " + Conversion.ToDouble(item2.NetBillWeight) + " kg " + Conversion.ToDouble(item2.ItemQty) + " bags " + Conversion.ToDouble(item2.ItemRate - item2.RateCut) + " Rate";
							}
						}
						string StockRemarks = Conversion.ToString(find3[0].ItemName) + " ,Purchased Qty " + Conversion.ToDouble(item2.NetStockWeight) + " kg, " + Conversion.ToDouble(item2.ItemQty) + " bags, @Rate " + Conversion.ToDouble(item2.ItemRate - item2.RateCut) + "  " + item2.RateUom.ToString() + " ,from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
						if (item2.VehicleNo != string.Empty)
						{
							StockRemarks = StockRemarks + " ,Vehicle # " + item2.VehicleNo;
						}
						if (item2.EBTotalWt > 0m)
						{
							StockRemarks = StockRemarks + " ,Bags Weight: " + Conversion.ToDouble(item2.EBTotalWt);
						}
						if (item2.WeightCutTotal > 0m)
						{
							StockRemarks = StockRemarks + " ,Deduction: " + Conversion.ToDouble(item2.WeightCutTotal);
						}
						if (item2.CommissionAmount > 0m)
						{
							StockRemarks = StockRemarks + " Commission paid @ " + Conversion.ToString(obj.CommRate) + "% = Rs. " + Math.Round(item2.CommissionAmount);
							if (obj.CommissionRemarks != null && obj.CommissionRemarks != string.Empty)
							{
								StockRemarks = StockRemarks + " " + obj.CommissionRemarks;
							}
						}
						if (obj.BrokeryAmount > 0m)
						{
							StockRemarks = StockRemarks + " Commission deduct @ " + obj.BrokeryRate + "% = Rs. " + Math.Round(obj.BrokeryAmount);
						}
						if (item2.FreightAmount > 0m)
						{
							StockRemarks = StockRemarks + " Freight Inward = Rs. " + Math.Round(item2.FreightAmount);
						}
						if (item2.PurchaseOrder > 0)
						{
							StockRemarks = StockRemarks + " Under contract # " + item2.PurchaseOrder;
						}
						if (obj.SupplierReferenceNo > 0)
						{
							StockRemarks = StockRemarks + " Ref # " + obj.SupplierReferenceNo;
						}
						if (obj.ManualBillNo != string.Empty)
						{
							StockRemarks = StockRemarks + " Manual # " + obj.ManualBillNo;
						}
						vd2.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.Comments = Conversion.ToString(StockRemarks);
						vd2.DebitAmount = Conversion.ToDouble(item2.ItemAmount);
						vd2.CreditAmount = Conversion.ToDouble(0);
						vd2.ItemId = Conversion.ToInt(item2.ItemId);
						vd2.QtyIn = Conversion.ToDouble(item2.ItemQty);
						vd2.ItemRate = Conversion.ToDouble(item2.ItemRate);
						vd2.WeightIn = Conversion.ToDouble(item2.NetBillWeight);
						vd2.RateCut = Conversion.ToDouble(item2.RateCut);
						vd2.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
						vd2.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
						vd2.Freight = Conversion.ToDouble(item2.FreightAmount);
						vd2.Expenses = Conversion.ToDouble(item2.ExpenseAmount);
						vd2.Journal = Conversion.ToDouble(item2.BrokeryAmount);
						vd2.JobLotId = Conversion.ToInt(item2.JobLotId);
						vd2.Commission = Conversion.ToDouble(item2.CommissionAmount);
						vd2.OrderNo = Conversion.ToInt(item2.PurchaseOrder);
						vd2.GpNo = Conversion.ToInt(item2.GpNo);
						vd2.VehicleNo = item2.VehicleNo;
						vd2.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
						vd2.SubsidiaryAgainstTypeId = 1;
						vd2.DMultiCurrencyId = Conversion.ToInt(item2.CurrencyId);
						vd2.DExchangeCurrencyRate = Conversion.ToDouble(item2.ExchangeRate);
						vd2.DCurrencyAmount = Conversion.ToDouble(item2.FcyAmount);
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.CreditAmount = Conversion.ToDouble(item2.ItemAmount);
						vd3.DebitAmount = Conversion.ToDouble(0);
						vd3.ItemId = Conversion.ToInt(item2.ItemId);
						vd3.QtyIn = Conversion.ToDouble(item2.ItemQty);
						vd3.ItemRate = Conversion.ToDouble(item2.ItemRate);
						vd3.WeightIn = Conversion.ToDouble(item2.NetBillWeight);
						vd3.RateCut = Conversion.ToDouble(item2.RateCut);
						vd3.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
						vd3.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
						vd3.Freight = Conversion.ToDouble(item2.FreightAmount);
						vd3.Expenses = Conversion.ToDouble(item2.ExpenseAmount);
						vd3.Journal = Conversion.ToDouble(item2.BrokeryAmount);
						vd3.JobLotId = Conversion.ToInt(item2.JobLotId);
						vd3.Commission = Conversion.ToDouble(item2.CommissionAmount);
						vd3.OrderNo = Conversion.ToInt(item2.PurchaseOrder);
						vd3.GpNo = Conversion.ToInt(item2.GpNo);
						vd3.VehicleNo = item2.VehicleNo;
						vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd3.SubsidiaryAccountId = obj.SupplierCustomerId;
						vd3.SubsidiaryTypeId = 1;
						vd3.DMultiCurrencyId = Conversion.ToInt(item2.CurrencyId);
						vd3.DExchangeCurrencyRate = Conversion.ToDouble(item2.ExchangeRate);
						vd3.DCurrencyAmount = Conversion.ToDouble(item2.FcyAmount);
						vh.voucherDetailList.Add(vd3);
						if (item2.ExpenseAmount > 0m)
						{
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							if (AutoRemarksFormatII)
							{
								vd4.Comments = Remarks;
							}
							else
							{
								vd4.Comments = Conversion.ToString("Other Expenses");
							}
							vd4.DebitAmount = Conversion.ToDouble(item2.ExpenseAmount);
							vd4.CreditAmount = Conversion.ToDouble(0);
							vd4.JobLotId = Conversion.ToInt(item2.JobLotId);
							vh.voucherDetailList.Add(vd4);
							VoucherDetail vd7 = new VoucherDetail();
							vd7.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd7.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							if (AutoRemarksFormatII)
							{
								vd7.Comments = Remarks;
							}
							else
							{
								vd7.Comments = Conversion.ToString("Other Expenses");
							}
							vd7.CreditAmount = Conversion.ToDouble(item2.ExpenseAmount);
							vd7.DebitAmount = Conversion.ToDouble(0);
							vd7.JobLotId = Conversion.ToInt(item2.JobLotId);
							vd7.SupplierCustomerId = obj.SupplierCustomerId;
							vd7.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd7.SubsidiaryTypeId = 1;
							vh.voucherDetailList.Add(vd7);
						}
						if (obj.DocumentTypeId == 98)
						{
							if (item2.CommissionAmount > 0m)
							{
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = Conversion.ToInt(vh.AgainstAccountId);
								vd5.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
								vd5.Comments = Conversion.ToString(obj.CommissionRemarks);
								vd5.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
								vd5.CreditAmount = Conversion.ToDouble(0);
								vd5.SupplierCustomerId = obj.CommissionAgentId;
								vd5.SubsidiaryAccountId = obj.CommissionAgentId;
								vd5.SubsidiaryTypeId = 1;
								vh.voucherDetailList.Add(vd5);
								VoucherDetail vd8 = new VoucherDetail();
								vd8.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
								vd8.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
								vd8.Comments = Conversion.ToString(obj.CommissionRemarks);
								vd8.CreditAmount = Conversion.ToDouble(item2.CommissionAmount);
								vd8.SubsidiaryAgainstTypeId = 1;
								vd8.SubsidiaryAgainstAccountId = obj.CommissionAgentId;
								vh.voucherDetailList.Add(vd8);
							}
						}
						else if (item2.CommissionAmount > 0m)
						{
							string RemarksCommission = "";
							VoucherDetail vd6 = new VoucherDetail();
							vd6.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							vd6.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
							if (AutoRemarksFormatII)
							{
								vd6.Comments = Remarks;
							}
							else if (obj.CommissionRemarks == string.Empty || obj.CommissionRemarks == null)
							{
								RemarksCommission = (vd6.Comments = "ItemName " + item2.ItemName + " ItemQty " + item2.ItemQty + " Amount " + item2.CommissionAmount);
							}
							else
							{
								vd6.Comments = Conversion.ToString(obj.CommissionRemarks);
							}
							vd6.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
							vd6.CreditAmount = Conversion.ToDouble(0);
							vd6.JobLotId = Conversion.ToInt(item2.JobLotId);
							vd6.SubsidiaryAgainstTypeId = 1;
							vd6.SubsidiaryAgainstAccountId = obj.CommissionAgentId;
							vh.voucherDetailList.Add(vd6);
							VoucherDetail vd9 = new VoucherDetail();
							vd9.AccountId = Conversion.ToInt(vh.AgainstAccountId);
							vd9.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							if (AutoRemarksFormatII)
							{
								vd9.Comments = Remarks;
							}
							else if (obj.CommissionRemarks == string.Empty || obj.CommissionRemarks == null)
							{
								vd9.Comments = RemarksCommission;
							}
							else
							{
								vd9.Comments = Conversion.ToString(obj.CommissionRemarks);
							}
							vd9.DebitAmount = Conversion.ToDouble(0);
							vd9.CreditAmount = Conversion.ToDouble(item2.CommissionAmount);
							vd9.SupplierCustomerId = obj.CommissionAgentId;
							vd9.SubsidiaryAccountId = obj.CommissionAgentId;
							vd9.SubsidiaryTypeId = 1;
							vd9.JobLotId = Conversion.ToInt(item2.JobLotId);
							vh.voucherDetailList.Add(vd9);
						}
					}
					getItemId = item2.ItemId;
					RowNo++;
					continue;
				}
				throw new Exception("ItemGlAccount Not found");
			}
			if (getCommissionAmount > 0m)
			{
				InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " Commission paid @ " + Conversion.ToString(obj.CommRate) + "% = Rs. " + Math.Round(getCommissionAmount);
			}
			if (obj.BrokeryAmount > 0m)
			{
				InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " Brokery deduct @ " + obj.BrokeryRate + "% = Rs. " + Math.Round(obj.BrokeryAmount);
			}
			if (getFreightAmount > 0m)
			{
				InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " Freight Inward = Rs. " + Math.Round(getFreightAmount) + " Vehicle # " + VehicleNo;
			}
			if (getOrderNo > 0)
			{
				InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " Under contract # " + getOrderNo;
			}
			if (obj.ManualBillNo != string.Empty)
			{
				InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " Manual # " + obj.ManualBillNo;
			}
			obj.RemarksHeader = InvoiceHeaderRemarksAuto;
			decimal TotalWeightInDetailList = default(decimal);
			for (int j = 0; j < itemListWithUniqueItems.ToList().Count; j++)
			{
				TotalWeightInDetailList += itemListWithUniqueItems[j].NetBillWeight;
			}
			decimal TotalFreightAmountInTransporterGrid = default(decimal);
			for (int i = 0; i < obj.InvPurchaseInvoiceFreightList.ToList().Count; i++)
			{
				TotalFreightAmountInTransporterGrid += obj.InvPurchaseInvoiceFreightList[i].CreditAmount;
			}
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item in itemListWithUniqueItems.ToList())
			{
				if (obj.DocumentTypeId != 1104)
				{
					continue;
				}
				if (item.FreightAmount > 0m && dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find2.Count > 0)
					{
						VoucherDetail vd11 = new VoucherDetail();
						vd11.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd11.AgainstAccountId = obj.TransportAccountId;
						vd11.DebitAmount = Conversion.ToDouble(item.FreightAmount);
						vd11.CreditAmount = 0.0;
						if (AutoRemarksFormatII)
						{
							vd11.Comments = Remarks;
						}
						else
						{
							vd11.Comments = "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item.NetBillWeight + " = " + Math.Round(item.FreightAmount);
						}
						vh.voucherDetailList.Add(vd11);
					}
				}
				if (obj.FreightDeduction > 0m && obj.FreightDebitAccountId > 0 && dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						VoucherDetail vd10 = new VoucherDetail();
						vd10.AccountId = Conversion.ToInt(obj.FreightDebitAccountId);
						vd10.AgainstAccountId = find[0].PurchaseGLAC;
						vd10.DebitAmount = Conversion.ToDouble(item.FreightDeduction);
						vd10.CreditAmount = 0.0;
						vd10.Comments = "Freight Deduction " + Remarks;
						vh.voucherDetailList.Add(vd10);
						VoucherDetail vd12 = new VoucherDetail();
						vd12.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd12.AgainstAccountId = obj.FreightDebitAccountId;
						vd12.CreditAmount = Conversion.ToDouble(obj.FreightDeduction);
						vd12.DebitAmount = 0.0;
						vd12.Comments = "Freight Deduction " + Remarks;
						vh.voucherDetailList.Add(vd12);
					}
				}
			}
		}
		return vh;
	}

	public static VoucherHead ItemAndSupplierTradingFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 58 || obj.DocumentTypeId == 61 || obj.DocumentTypeId == 64 || obj.DocumentTypeId == 104 || obj.DocumentTypeId == 131 || obj.DocumentTypeId == 702)
		{
			string Remarks = string.Empty;
			if (obj.invPurchaseInvoiceDetailList.Count > 0)
			{
				decimal TotalFreightAmountInTransporterGrid = default(decimal);
				decimal TotalWeightInDetailList = default(decimal);
				foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item2 in obj.invPurchaseInvoiceDetailList.ToList())
				{
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName w) => w.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count > 0)
					{
						VoucherDetail vd2 = new VoucherDetail();
						if (obj.DocumentTypeId == 702)
						{
							Remarks = obj.RemarksHeader + " ItemName: " + Conversion.ToString(find2[0].ItemName) + "  Rate:" + item2.ItemRate + " Qty: " + item2.ItemQty + " Weight: " + item2.NetBillWeight + " Item Amount: " + item2.ItemAmount;
						}
						else if (obj.DocumentTypeId == 61 || obj.DocumentTypeId == 64)
						{
							Remarks = Conversion.ToString(find2[0].ItemName) + "  " + Conversion.ToString(item2?.JobLotDescription) + "  " + Conversion.ToString(item2?.ItemUom) + "  " + item2.NetBillWeight + " @" + item2.ItemRate + "/- ";
							if (item2.GpNo > 0)
							{
								Remarks = Remarks + "GP# " + item2.GpNo;
							}
							if (item2.VehicleNo != null && item2.VehicleNo != string.Empty)
							{
								Remarks = Remarks + " V# " + item2.VehicleNo.ToString();
							}
							if (item2.RemarksDetail != null && item2.RemarksDetail != string.Empty)
							{
								Remarks = Remarks + " " + item2.RemarksDetail.ToString();
							}
						}
						else if (obj.DocumentTypeId != 61 && obj.DocumentTypeId != 64 && obj.DocumentTypeId != 702)
						{
							Remarks = obj.RemarksHeader + " ItemName: " + Conversion.ToString(find2[0].ItemName) + "  Rate:" + item2.ItemRate + "Qty: " + item2.ItemQty + "Item Amount: " + item2.ItemAmount;
						}
						if (item2.FreightAmount > 0m)
						{
							Remarks = Remarks + " Freight Amount: " + Math.Round(item2.FreightAmount, 0);
						}
						vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.Comments = Conversion.ToString(Remarks);
						vd2.DebitAmount = Conversion.ToDouble(item2.ItemAmount);
						vd2.CreditAmount = Conversion.ToDouble(0);
						vd2.ItemId = Conversion.ToInt(item2.ItemId);
						vd2.QtyIn = Conversion.ToDouble(item2.ItemQty);
						vd2.ItemRate = Conversion.ToDouble(item2.ItemRate);
						vd2.WeightIn = Conversion.ToDouble(item2.NetBillWeight);
						vd2.RateCut = Conversion.ToDouble(item2.RateCut);
						vd2.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
						vd2.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
						vd2.Freight = Conversion.ToDouble(item2.FreightAmount);
						vd2.Commission = Conversion.ToDouble(item2.CommissionAmount);
						vd2.OrderNo = Conversion.ToInt(item2.PurchaseOrder);
						vd2.GpNo = Conversion.ToInt(item2.GpNo);
						vd2.VehicleNo = item2.VehicleNo;
						vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.CreditAmount = Conversion.ToDouble(item2.ItemAmount);
						vd3.DebitAmount = Conversion.ToDouble(0);
						vd3.ItemId = Conversion.ToInt(item2.ItemId);
						vd3.QtyIn = Conversion.ToDouble(item2.ItemQty);
						vd3.ItemRate = Conversion.ToDouble(item2.ItemRate);
						vd3.RateCut = Conversion.ToDouble(item2.RateCut);
						vd3.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
						vd3.WeightIn = Conversion.ToDouble(item2.NetBillWeight);
						vd3.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
						vd3.Freight = Conversion.ToDouble(item2.FreightAmount);
						vd3.Commission = Conversion.ToDouble(item2.CommissionAmount);
						vd3.OrderNo = Conversion.ToInt(item2.PurchaseOrder);
						vd3.GpNo = Conversion.ToInt(item2.GpNo);
						vd3.VehicleNo = item2.VehicleNo;
						vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						if (obj.DocumentTypeId == 58 || obj.DocumentTypeId == 61 || obj.DocumentTypeId == 64 || obj.DocumentTypeId == 104 || obj.DocumentTypeId == 131 || obj.DocumentTypeId == 702)
						{
							vd3.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd3.SubsidiaryTypeId = 1;
						}
						vh.voucherDetailList.Add(vd3);
						TotalFreightAmountInTransporterGrid += item2.FreightAmount;
						TotalWeightInDetailList += item2.NetBillWeight;
						continue;
					}
					throw new Exception("Item Purchase GL Account Not found");
				}
				if (TotalFreightAmountInTransporterGrid > 0m)
				{
					foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item in obj.invPurchaseInvoiceDetailList.ToList())
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName w) => w.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0)
						{
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd4.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
							vd4.DebitAmount = Conversion.ToDouble(item.FreightAmount);
							vd4.Comments = "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item.NetBillWeight + " = " + Math.Round(item.FreightAmount);
							vd4.CreditAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd4);
							continue;
						}
						throw new Exception("Item Purchase GL Account Not found");
					}
				}
			}
		}
		return vh;
	}

	public static VoucherHead PurchaseReturnFinancial(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 59 && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail> itemListWithUniqueItems = new List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail>();
			List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail>>(JsonConvert.SerializeObject(obj.invPurchaseInvoiceDetailList.ToList()));
			for (int k = 0; k < dummyArray.Count; k++)
			{
				if (k == 0)
				{
					itemListWithUniqueItems.Add(dummyArray[k]);
				}
				if (k <= 0)
				{
					continue;
				}
				bool flagForListWithUniqueItem = false;
				for (int m = 0; m < itemListWithUniqueItems.Count; m++)
				{
					if (itemListWithUniqueItems[m].ItemId == dummyArray[k].ItemId)
					{
						flagForListWithUniqueItem = true;
						break;
					}
				}
				if (!flagForListWithUniqueItem)
				{
					itemListWithUniqueItems.Add(dummyArray[k]);
				}
			}
			for (int l = 0; l < itemListWithUniqueItems.ToList().Count; l++)
			{
				decimal itemAmountAtI = default(decimal);
				decimal itemExpenseAmountAtI = default(decimal);
				decimal itemFreightAmountAtI = default(decimal);
				decimal itemCommissionAmountAtI = default(decimal);
				decimal itemQtyAtI = default(decimal);
				decimal itemWeightAtI = default(decimal);
				decimal itemWeightCutAtI = default(decimal);
				decimal itemRateCutAtI = default(decimal);
				decimal itemEmptyBagsDudectionAtI = default(decimal);
				decimal itemGrossWeightAtI = default(decimal);
				for (int n = 0; n < dummyArray.Count; n++)
				{
					if (itemListWithUniqueItems[l].ItemId == dummyArray[n].ItemId)
					{
						itemAmountAtI += dummyArray[n].ItemAmount;
						itemFreightAmountAtI += dummyArray[n].FreightAmount;
						itemExpenseAmountAtI += dummyArray[n].ExpenseAmount;
						itemCommissionAmountAtI += dummyArray[n].CommissionAmount;
						itemQtyAtI += dummyArray[n].ItemQty;
						itemWeightAtI += dummyArray[n].NetBillWeight;
						itemWeightCutAtI += dummyArray[n].WeightCut;
						itemRateCutAtI += dummyArray[n].RateCut;
						itemEmptyBagsDudectionAtI += dummyArray[n].EBTotalWt;
						itemGrossWeightAtI += dummyArray[n].GrossWeight;
					}
				}
				itemListWithUniqueItems[l].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[l].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[l].FreightAmount = itemFreightAmountAtI;
				itemListWithUniqueItems[l].CommissionAmount = itemCommissionAmountAtI;
				itemListWithUniqueItems[l].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[l].NetBillWeight = itemWeightAtI;
				itemListWithUniqueItems[l].WeightCut = itemWeightCutAtI;
				itemListWithUniqueItems[l].RateCut = itemRateCutAtI;
				itemListWithUniqueItems[l].EBTotalWt = itemEmptyBagsDudectionAtI;
				itemListWithUniqueItems[l].GrossWeight = itemGrossWeightAtI;
			}
			string Remarks = string.Empty;
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item2 in itemListWithUniqueItems.ToList())
			{
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count > 0)
					{
						VoucherDetail vd2 = new VoucherDetail();
						if (obj.RemarksHeader != string.Empty)
						{
							Remarks = obj.RemarksHeader;
						}
						Remarks = Remarks + "   NoOfBags: " + item2.ItemQty + "   " + Conversion.ToString(find2[0].ItemName) + "  GrossWeight: " + item2.GrossWeight + "  Net Weight: " + item2.NetBillWeight + "  Rate:" + item2.ItemRate;
						if (item2.EBTotalWt > 0m)
						{
							Remarks = Remarks + "   EmptyBagsDeduction: " + item2.EBTotalWt;
						}
						if (item2.RateCut > 0m)
						{
							Remarks = Remarks + "   RateCut:" + item2.RateCut;
						}
						if (item2.ExpenseAmount > 0m)
						{
							Remarks = Remarks + "    Expense Amount: " + item2.ExpenseAmount;
						}
						if (item2.CommissionAmount > 0m)
						{
							Remarks = Remarks + "    Commission Amount: " + item2.CommissionAmount;
						}
						if (item2.FreightAmount > 0m)
						{
							Remarks = Remarks + "    Freight Amount: " + item2.FreightAmount;
						}
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.DebitAmount = Conversion.ToDouble(item2.ItemAmount);
						vd3.CreditAmount = Conversion.ToDouble(0);
						vd3.ItemId = Conversion.ToInt(item2.ItemId);
						vd3.QtyOut = Conversion.ToDouble(item2.ItemQty);
						vd3.ItemRate = Conversion.ToDouble(item2.ItemRate);
						vd3.WeightOut = Conversion.ToDouble(item2.NetBillWeight);
						vd3.RateCut = Conversion.ToDouble(item2.RateCut);
						vd3.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
						vd3.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
						vd3.Freight = Conversion.ToDouble(item2.FreightAmount);
						vd3.Expenses = Conversion.ToDouble(item2.ExpenseAmount);
						vd3.Journal = 0.0;
						vd3.Commission = Conversion.ToDouble(item2.CommissionAmount);
						vd3.OrderNo = Conversion.ToInt(item2.PurchaseOrder);
						vd3.GpNo = Conversion.ToInt(item2.GpNo);
						vd3.VehicleNo = item2.VehicleNo;
						vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd3);
						vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.Comments = Conversion.ToString(Remarks);
						vd2.CreditAmount = Conversion.ToDouble(item2.ItemAmount);
						vd2.DebitAmount = Conversion.ToDouble(0);
						vd2.ItemId = Conversion.ToInt(item2.ItemId);
						vd2.QtyOut = Conversion.ToDouble(item2.ItemQty);
						vd2.ItemRate = Conversion.ToDouble(item2.ItemRate);
						vd2.WeightOut = Conversion.ToDouble(item2.NetBillWeight);
						vd2.RateCut = Conversion.ToDouble(item2.RateCut);
						vd2.RateCutAmount = Conversion.ToDouble(item2.RateCutAmount);
						vd2.ItemAmount = Conversion.ToDouble(item2.ItemAmount);
						vd2.Freight = Conversion.ToDouble(item2.FreightAmount);
						vd2.Expenses = Conversion.ToDouble(item2.ExpenseAmount);
						vd2.Journal = 0.0;
						vd2.Commission = Conversion.ToDouble(item2.CommissionAmount);
						vd2.OrderNo = Conversion.ToInt(item2.PurchaseOrder);
						vd2.GpNo = Conversion.ToInt(item2.GpNo);
						vd2.VehicleNo = item2.VehicleNo;
						vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd2);
						if (item2.ExpenseAmount > 0m)
						{
							VoucherDetail vd6 = new VoucherDetail();
							vd6.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd6.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd6.Comments = Conversion.ToString("Other Expenses");
							vd6.DebitAmount = Conversion.ToDouble(item2.ExpenseAmount);
							vd6.CreditAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd6);
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd4.Comments = Conversion.ToString("Other Expenses");
							vd4.CreditAmount = Conversion.ToDouble(item2.ExpenseAmount);
							vd4.DebitAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd4);
						}
						if (item2.CommissionAmount > 0m)
						{
							VoucherDetail vd5 = new VoucherDetail();
							vd5.AccountId = Conversion.ToInt(vh.AgainstAccountId);
							vd5.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd5.Comments = Conversion.ToString(obj.CommissionRemarks);
							vd5.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
							vd5.CreditAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd5);
							VoucherDetail vd7 = new VoucherDetail();
							vd7.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							vd7.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
							vd7.Comments = Conversion.ToString(obj.CommissionRemarks);
							vd7.DebitAmount = Conversion.ToDouble(0);
							vd7.CreditAmount = Conversion.ToDouble(item2.CommissionAmount);
							vh.voucherDetailList.Add(vd7);
						}
					}
					continue;
				}
				throw new Exception("ItemGlAccount Not found");
			}
			decimal TotalWeightInDetailList = default(decimal);
			for (int j = 0; j < itemListWithUniqueItems.ToList().Count; j++)
			{
				TotalWeightInDetailList += itemListWithUniqueItems[j].NetBillWeight;
			}
			decimal TotalFreightAmountInTransporterGrid = default(decimal);
			for (int i = 0; i < obj.InvPurchaseInvoiceFreightList.ToList().Count; i++)
			{
				TotalFreightAmountInTransporterGrid += obj.InvPurchaseInvoiceFreightList[i].CreditAmount;
			}
			foreach (Architecture.Model.FeedMill.Purchase.InvPurchaseInvoiceDetail item in itemListWithUniqueItems.ToList())
			{
				if (obj.DocumentTypeId == 59 && dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0 && item.FreightAmount > 0m)
					{
						VoucherDetail vd8 = new VoucherDetail();
						vd8.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd8.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
						vd8.CreditAmount = Conversion.ToDouble(item.FreightAmount);
						vd8.DebitAmount = Conversion.ToDouble(0);
						vd8.Comments = "LessToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item.NetBillWeight + " = " + item.FreightAmount;
						vh.voucherDetailList.Add(vd8);
					}
				}
			}
		}
		return vh;
	}
}
