using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.TaxProject.Common;
using Architecture.Common;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Inventory;
using Newtonsoft.Json;

namespace Architecture.BLL.TaxProject.Inventory.Financials.PurchaseInvoice;

public class PurchaseInvoiceGeneralFinancialMethods
{
	public static VoucherHead EmptyBagsFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int> { 56, 57, 98, 138, 166, 168, 172 }.Contains(obj.DocumentTypeId) && obj.InvPurchaseInvoiceEmptyBagslist.Count > 0)
		{
			foreach (InvPurchaseInvoiceEmptyBags item in obj.InvPurchaseInvoiceEmptyBagslist.ToList())
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
						vd.BranchesId = obj.BranchesId;
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
						vd3.BranchesId = obj.BranchesId;
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
						throw new Exception("Credit Account Not Found against Packing Material");
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
						vd2.BranchesId = obj.BranchesId;
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
						vd4.BranchesId = obj.BranchesId;
						vh.voucherDetailList.Add(vd4);
					}
				}
			}
		}
		return vh;
	}

	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int>
		{
			56, 57, 58, 59, 61, 63, 64, 131, 166, 168,
			172, 702, 1603, 1604, 1804, 245, 98
		}.Contains(obj.DocumentTypeId) && obj.InvPurchaseInvoicejournalList.Count > 0)
		{
			foreach (InvPurchaseInvoiceJournal item in obj.InvPurchaseInvoicejournalList.ToList())
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
						vd.Comments = Conversion.ToString(item.JvRemarks);
					}
					vd.DebitAmount = Conversion.ToDouble(item.JvDebit);
					vd.CreditAmount = Conversion.ToDouble(item.JvCredit);
					if (item.SupplierCustomerId > 0)
					{
						vd.SupplierCustomerId = item.SupplierCustomerId;
						vd.SubsidiaryAccountId = item.SupplierCustomerId;
						vd.SubsidiaryTypeId = 1;
					}
					vd.BranchesId = obj.BranchesId;
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
						vd2.Comments = Conversion.ToString(item.JvRemarks);
					}
					vd2.DebitAmount = Conversion.ToDouble(item.JvCredit);
					vd2.CreditAmount = Conversion.ToDouble(item.JvDebit);
					vd2.SupplierCustomerId = obj.SupplierCustomerId;
					vd2.SubsidiaryAccountId = obj.SupplierCustomerId;
					vd2.SubsidiaryTypeId = 1;
					vd2.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		return vh;
	}

	public static VoucherHead SalesTaxFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int>
		{
			56, 57, 58, 63, 64, 131, 702, 1603, 1604, 1653,
			1654
		}.Contains(obj.DocumentTypeId) && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			double TaxAmount = obj.invPurchaseInvoiceDetailList.Sum((InvPurchaseInvoiceDetail x) => x.TaxAmount);
			double TaxPercent = obj.invPurchaseInvoiceDetailList.Average((InvPurchaseInvoiceDetail x) => x.TaxPercent);
			if (TaxAmount > 0.0 && obj.TaxAccountId > 0)
			{
				VoucherDetail vd2 = new VoucherDetail();
				vd2.AccountId = obj.TaxAccountId;
				vd2.Comments = "Sale Tax";
				vd2.AgainstAccountId = vh.RefAccountId;
				vd2.DebitAmount = TaxAmount;
				vd2.TaxesTotalAmount = TaxAmount;
				vd2.TaxPrcnt = TaxPercent;
				vd2.IsTaxable = "True";
				vd2.BranchesId = obj.BranchesId;
				vh.voucherDetailList.Add(vd2);
				VoucherDetail vd1 = new VoucherDetail();
				vd1.AccountId = vh.RefAccountId;
				vd1.AgainstAccountId = obj.TaxAccountId;
				vd1.Comments = "Sale Tax";
				vd1.CreditAmount = TaxAmount;
				vd1.TaxesTotalAmount = TaxAmount;
				vd1.IsTaxable = "True";
				vd1.TaxPrcnt = TaxPercent;
				if (obj.DocumentTypeId == 1603 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1653 || obj.DocumentTypeId == 1654)
				{
					vd1.SupplierCustomerId = obj.SupplierCustomerId;
					vd1.SubsidiaryAccountId = obj.SupplierCustomerId;
					vd1.SubsidiaryTypeId = 1;
				}
				vd1.BranchesId = obj.BranchesId;
				vh.voucherDetailList.Add(vd1);
			}
		}
		return vh;
	}

	public static VoucherHead FreightFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int>
		{
			56, 57, 58, 61, 63, 64, 98, 131, 166, 168,
			172, 245, 702, 1603, 1604, 1653, 1654, 1804
		}.Contains(obj.DocumentTypeId))
		{
			int ItemGlId = 0;
			if (obj.invPurchaseInvoiceDetailList.Select((InvPurchaseInvoiceDetail x) => x.PurchaseGLAC).Distinct().Count() == 1)
			{
				ItemGlId = obj.invPurchaseInvoiceDetailList.Max((InvPurchaseInvoiceDetail x) => x.PurchaseGLAC);
			}
			if (obj.InvPurchaseInvoiceFreightList.Count > 0)
			{
				foreach (InvPurchaseInvoiceFreight r2 in obj.InvPurchaseInvoiceFreightList.ToList())
				{
					if (Conversion.ToInt(r2.TansporterId) > 0 && Conversion.ToDouble(r2.Debit) > 0.0)
					{
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = r2.TansporterId;
						vd3.AgainstAccountId = ((ItemGlId > 0) ? ItemGlId : vd3.AccountId);
						if (obj.CommonIdsForFinancials.AutoRemarksStatus)
						{
							vd3.Comments = obj.CommonIdsForFinancials.Remarks;
						}
						else if (r2.Remarks == null || r2.Remarks == string.Empty)
						{
							vd3.Comments = "PartyName :  " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + "  Freight : " + Conversion.ToString(r2.FreightAmount);
						}
						else
						{
							vd3.Comments = Conversion.ToString(r2.Remarks);
						}
						vd3.DebitAmount = Conversion.ToDouble(r2.Debit);
						vd3.CreditAmount = 0.0;
						if (r2.SupplierCustomerId > 0)
						{
							vd3.SupplierCustomerId = r2.SupplierCustomerId;
							vd3.SubsidiaryAccountId = r2.SupplierCustomerId;
							vd3.SubsidiaryTypeId = 1;
						}
						vd3.BranchesId = obj.BranchesId;
						vh.voucherDetailList.Add(vd3);
					}
					if (Conversion.ToInt(r2.TansporterId) > 0 && Conversion.ToDouble(r2.FreightAmount) > 0.0)
					{
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = r2.TansporterId;
						vd2.AgainstAccountId = ((ItemGlId > 0) ? ItemGlId : vd2.AccountId);
						if (obj.CommonIdsForFinancials.AutoRemarksStatus)
						{
							vd2.Comments = obj.CommonIdsForFinancials.Remarks;
						}
						else if (r2.Remarks == null || r2.Remarks == string.Empty)
						{
							vd2.Comments = "SupplierName :  " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + "  Freight : " + Conversion.ToString(r2.FreightAmount);
						}
						else
						{
							vd2.Comments = Conversion.ToString(r2.Remarks);
						}
						vd2.DebitAmount = 0.0;
						vd2.CreditAmount = Conversion.ToDouble(r2.FreightAmount);
						if (r2.SupplierCustomerId > 0)
						{
							vd2.SupplierCustomerId = r2.SupplierCustomerId;
							vd2.SubsidiaryAccountId = r2.SupplierCustomerId;
							vd2.SubsidiaryTypeId = 1;
						}
						vd2.BranchesId = obj.BranchesId;
						vh.voucherDetailList.Add(vd2);
					}
				}
			}
		}
		if (obj.DocumentTypeId == 59 && obj.InvPurchaseInvoiceFreightList.Count > 0)
		{
			foreach (InvPurchaseInvoiceFreight r in obj.InvPurchaseInvoiceFreightList.ToList())
			{
				if (Conversion.ToInt(r.TansporterId) > 0 && Conversion.ToDouble(r.FreightAmount) > 0.0)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = r.TansporterId;
					vd.AgainstAccountId = obj.CommonIdsForFinancials.OffsetAccountId;
					vd.Comments = Conversion.ToString(r.Remarks);
					vd.CreditAmount = 0.0;
					vd.DebitAmount = Conversion.ToDouble(r.FreightAmount);
					vd.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd);
				}
			}
		}
		return vh;
	}

	public static VoucherHead BrokeryBeHalfOnPartyFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 56 || obj.DocumentTypeId == 57 || obj.DocumentTypeId == 172) && obj.BrokerAgentId > 0 && obj.BrokeryAmount > 0.0)
		{
			VoucherDetail vd5 = new VoucherDetail();
			vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
			vd5.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
			if (obj.CommonIdsForFinancials.AutoRemarksStatus)
			{
				vd5.Comments = obj.CommonIdsForFinancials.Remarks;
			}
			else
			{
				vd5.Comments = "BrokerAgent: " + obj.CommonIdsForFinancials.BrokerAgentName + "  Brokery Rate" + obj.BrokeryRate + " BrokeryType" + obj.BrokeryType + "  BrokeryAmount" + obj.BrokeryAmount;
			}
			vd5.DebitAmount = Conversion.ToDouble(obj.BrokeryAmount);
			vd5.CreditAmount = 0.0;
			vd5.SupplierCustomerId = obj.SupplierCustomerId;
			vd5.SubsidiaryAccountId = obj.SupplierCustomerId;
			vd5.SubsidiaryTypeId = 1;
			vd5.BranchesId = obj.BranchesId;
			vh.voucherDetailList.Add(vd5);
			VoucherDetail vd2 = new VoucherDetail();
			vd2.AccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
			vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
			if (obj.CommonIdsForFinancials.AutoRemarksStatus)
			{
				vd2.Comments = obj.CommonIdsForFinancials.Remarks;
			}
			else
			{
				vd2.Comments = "SupplierName: " + obj.CommonIdsForFinancials.CompanyName + "  Brokery Rate" + obj.BrokeryRate + " BrokeryType" + obj.BrokeryType + "  BrokeryAmount" + obj.BrokeryAmount;
			}
			vd2.CreditAmount = Conversion.ToDouble(obj.BrokeryAmount);
			vd2.DebitAmount = 0.0;
			vd2.SubsidiaryAgainstTypeId = 1;
			vd2.SubsidiaryAgainstAccountId = obj.BrokerAgentId;
			vd2.BranchesId = obj.BranchesId;
			vh.voucherDetailList.Add(vd2);
		}
		if (obj.DocumentTypeId == 138)
		{
			if (obj.BrokerAgentId > 0 && obj.BrokeryAmount > 0.0)
			{
				VoucherDetail vd3 = new VoucherDetail();
				vd3.AccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
				vd3.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.ItemStockAccountId);
				vd3.Comments = "SupplierName: " + obj.CommonIdsForFinancials.CompanyName + "  Brokery%" + obj.BrokeryRate + "  BrokeryAmount" + obj.BrokeryAmount;
				vd3.DebitAmount = 0.0;
				vd3.CreditAmount = Conversion.ToDouble(obj.BrokeryAmount);
				vd3.BranchesId = obj.BranchesId;
				vh.voucherDetailList.Add(vd3);
			}
			if (obj.BrokerAgentId > 0 && obj.BrokeryAmount < 0.0)
			{
				VoucherDetail vd4 = new VoucherDetail();
				vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
				vd4.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
				vd4.Comments = "BrokerAgent: " + obj.CommonIdsForFinancials.BrokerAgentName + "  Brokery Rate" + obj.BrokeryRate + "  BrokeryAmount" + obj.BrokeryAmount;
				vd4.DebitAmount = Conversion.ToDouble(Math.Abs(obj.BrokeryAmount));
				vd4.CreditAmount = 0.0;
				vd4.SupplierCustomerId = obj.SupplierCustomerId;
				vd4.SubsidiaryAccountId = obj.SupplierCustomerId;
				vd4.SubsidiaryTypeId = 1;
				vd4.BranchesId = obj.BranchesId;
				vh.voucherDetailList.Add(vd4);
				VoucherDetail vd = new VoucherDetail();
				vd.AccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
				vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd.Comments = "SupplierName: " + obj.CommonIdsForFinancials.CompanyName + "  Brokery Rate" + obj.BrokeryRate + "  BrokeryAmount" + obj.BrokeryAmount;
				vd.CreditAmount = Conversion.ToDouble(Math.Abs(obj.BrokeryAmount));
				vd.DebitAmount = 0.0;
				vd.SubsidiaryAgainstTypeId = 1;
				vd.SubsidiaryAgainstAccountId = obj.BrokerAgentId;
				vd.BranchesId = obj.BranchesId;
				vh.voucherDetailList.Add(vd);
			}
		}
		return vh;
	}

	public static VoucherHead ItemAndSupplierByWeightFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int>
		{
			56, 57, 63, 98, 138, 166, 168, 172, 1603, 1604,
			1653, 1654, 1804, 245
		}.Contains(obj.DocumentTypeId) && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			bool CommissionDebitToExpenses = false;
			CommissionDebitToExpenses = obj.DocumentTypeId == 98 && Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "DebitAmountChargetoExpenseAcOfCommission"));
			bool AutoRemarksFormatII = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "AutoRemarksFormatII"));
			List<InvPurchaseInvoiceDetail> itemListWithUniqueItems = new List<InvPurchaseInvoiceDetail>();
			List<InvPurchaseInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<InvPurchaseInvoiceDetail>>(JsonConvert.SerializeObject(obj.invPurchaseInvoiceDetailList.ToList()));
			for (int j = 0; j < dummyArray.Count; j++)
			{
				if (j == 0)
				{
					itemListWithUniqueItems.Add(dummyArray[j]);
				}
				if (j <= 0)
				{
					continue;
				}
				bool flagForListWithUniqueItem = false;
				for (int l = 0; l < itemListWithUniqueItems.Count; l++)
				{
					if (itemListWithUniqueItems[l].ItemId == dummyArray[j].ItemId && itemListWithUniqueItems[l].ItemRate == dummyArray[j].ItemRate && itemListWithUniqueItems[l].WarehouseId == dummyArray[j].WarehouseId)
					{
						flagForListWithUniqueItem = true;
						break;
					}
				}
				if (!flagForListWithUniqueItem)
				{
					itemListWithUniqueItems.Add(dummyArray[j]);
				}
			}
			for (int k = 0; k < itemListWithUniqueItems.ToList().Count; k++)
			{
				double itemAmountAtI = 0.0;
				double itemExpenseAmountAtI = 0.0;
				double itemFreightAmountAtI = 0.0;
				double itemCommissionAmountAtI = 0.0;
				double itemQtyAtI = 0.0;
				double itemWeightAtI = 0.0;
				double itemWeightCutAtI = 0.0;
				double itemRateCutAtI = 0.0;
				double itemEmptyBagsDudectionAtI = 0.0;
				double itemGrossWeightAtI = 0.0;
				double itemBrokeryAmountAtI = 0.0;
				for (int m = 0; m < dummyArray.Count; m++)
				{
					if (itemListWithUniqueItems[k].ItemId == dummyArray[m].ItemId && itemListWithUniqueItems[k].ItemRate == dummyArray[m].ItemRate && itemListWithUniqueItems[k].WarehouseId == dummyArray[m].WarehouseId)
					{
						itemAmountAtI += dummyArray[m].ItemAmount;
						itemFreightAmountAtI += dummyArray[m].FreightAmount;
						itemExpenseAmountAtI += dummyArray[m].ExpenseAmount;
						itemCommissionAmountAtI += dummyArray[m].CommissionAmount;
						itemBrokeryAmountAtI += dummyArray[m].Brokery;
						itemQtyAtI += dummyArray[m].ItemQty;
						itemWeightAtI += dummyArray[m].NetBillWeight;
						itemWeightCutAtI += dummyArray[m].WeightCut;
						itemRateCutAtI += dummyArray[m].RateCut;
						itemEmptyBagsDudectionAtI += dummyArray[m].EBTotalWt;
						itemGrossWeightAtI += dummyArray[m].GrossWeight;
					}
				}
				itemListWithUniqueItems[k].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[k].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[k].FreightAmount = itemFreightAmountAtI;
				itemListWithUniqueItems[k].CommissionAmount = itemCommissionAmountAtI;
				itemListWithUniqueItems[k].Brokery = itemBrokeryAmountAtI;
				itemListWithUniqueItems[k].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[k].NetBillWeight = itemWeightAtI;
				itemListWithUniqueItems[k].WeightCut = itemWeightCutAtI;
				itemListWithUniqueItems[k].RateCut = itemRateCutAtI;
				itemListWithUniqueItems[k].EBTotalWt = itemEmptyBagsDudectionAtI;
				itemListWithUniqueItems[k].GrossWeight = itemGrossWeightAtI;
			}
			string Remarks = string.Empty;
			string VehicleNo = string.Empty;
			string InvoiceHeaderRemarksAuto = "";
			int getOrderNo = 0;
			int getItemId = 0;
			double getCommissionAmount = 0.0;
			double getFreightAmount = 0.0;
			int RowNo = 1;
			foreach (InvPurchaseInvoiceDetail item3 in itemListWithUniqueItems.ToList())
			{
				Remarks = "";
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item3.ItemId)).ToList();
					if (find3.Count > 0)
					{
						if (RowNo == 1)
						{
							getOrderNo = item3.PurchaseOrder;
							getItemId = item3.ItemId;
							VehicleNo = item3.VehicleNo;
						}
						double Mond = Conversion.ToDouble(item3.NetBillWeight) / 40.0;
						item3.ItemName = Conversion.ToString(find3[0].ItemName);
						VoucherDetail vd3 = new VoucherDetail();
						if (obj.DocumentTypeId == 63 || obj.DocumentTypeId == 1603 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1653 || obj.DocumentTypeId == 1654)
						{
							if (obj.RemarksHeader != string.Empty)
							{
								Remarks = obj.RemarksHeader;
							}
							Remarks = Remarks + "   Item: " + Conversion.ToString(find3[0].ItemName) + ",   Qty: " + item3.ItemQty + ",   Rate:" + item3.ItemRate;
							if (item3.ExpenseAmount > 0.0)
							{
								Remarks = Remarks + "    Expense Amount: " + item3.ExpenseAmount;
							}
							if (item3.CommissionAmount > 0.0)
							{
								Remarks = Remarks + "    Commission Amount: " + item3.CommissionAmount;
							}
							if (item3.FreightAmount > 0.0)
							{
								Remarks = Remarks + "   Freight Amount: " + item3.FreightAmount;
							}
						}
						else
						{
							if (item3.CommissionAmount > 0.0)
							{
								getCommissionAmount += item3.CommissionAmount;
							}
							if (item3.FreightAmount > 0.0)
							{
								getFreightAmount += item3.FreightAmount;
							}
							string TransType = "Purchase ";
							if (obj.DocumentTypeId == 98)
							{
								TransType = "Sale Return ";
							}
							if (AutoRemarksFormatII)
							{
								Remarks = TransType + Conversion.ToString(item3.ItemQty) + " Bags  ( " + Conversion.ToString(item3.GrossWeight) + " , " + Conversion.ToString(item3.NetBillWeight) + ")  of " + Conversion.ToString(find3[0].ItemName) + "  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item3.ItemRate - item3.RateCut) + "   = Rs. " + Conversion.ToString(item3.ItemAmount);
								if (item3.CommissionAmount > 0.0)
								{
									Remarks = Remarks + " Commission paid @ " + Conversion.ToString(obj.CommRate) + "% = Rs. " + Math.Round(item3.CommissionAmount);
								}
								if (obj.BrokeryAmount > 0.0)
								{
									Remarks = Remarks + " Commission deduct @ " + obj.BrokeryRate + "% = Rs. " + Math.Round(obj.BrokeryAmount);
								}
								if (item3.FreightAmount > 0.0)
								{
									Remarks = Remarks + " Freight Inward = Rs. " + Math.Round(item3.FreightAmount) + " Vehicle # " + item3.VehicleNo;
								}
								if (item3.PurchaseOrder > 0)
								{
									Remarks = Remarks + " Under contract # " + item3.PurchaseOrder;
								}
								if (obj.ManualBillNo != string.Empty)
								{
									Remarks = Remarks + " Manual # " + obj.ManualBillNo;
								}
								Remarks = Remarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
								obj.CommonIdsForFinancials.AutoRemarksStatus = AutoRemarksFormatII;
								obj.CommonIdsForFinancials.Remarks = Remarks;
							}
							else
							{
								if (obj.RemarksHeader != string.Empty)
								{
									Remarks = obj.RemarksHeader;
								}
								Remarks = Remarks + "   NoOfBags: " + Conversion.ToString(item3.ItemQty) + "   " + Conversion.ToString(find3[0].ItemName) + "  GrossWeight: " + Conversion.ToString(item3.GrossWeight) + "  Net Weight: " + Conversion.ToString(item3.NetBillWeight) + "  Rate:" + Conversion.ToString(item3.ItemRate);
								if (item3.RateCut > 0.0 || item3.RateCut < 0.0)
								{
									double NetRate = 0.0;
									if (item3.RateCut > 0.0)
									{
										NetRate = item3.ItemRate - item3.RateCut;
									}
									else if (item3.RateCut < 0.0)
									{
										NetRate = item3.ItemRate + item3.RateCut;
									}
									Remarks = Remarks + "   RateCut:" + Conversion.ToString(item3.RateCut) + " NetRate " + Conversion.ToString(NetRate);
								}
								if (item3.EBTotalWt > 0.0)
								{
									Remarks = Remarks + "   BagsDeduction: " + Conversion.ToString(item3.EBTotalWt);
								}
								if (item3.WeightCutTotal > 0.0)
								{
									Remarks = Remarks + "   WeightCut: " + Conversion.ToString(item3.WeightCutTotal);
								}
								if (item3.AdLsWeight > 0.0 || item3.AdLsWeight < 0.0)
								{
									Remarks = Remarks + "   AdLsWeight: " + Conversion.ToString(item3.AdLsWeight);
								}
								if (item3.ExpenseAmount > 0.0)
								{
									Remarks = Remarks + "    Expense Amount: " + Math.Round(item3.ExpenseAmount);
								}
								if (item3.CommissionAmount > 0.0)
								{
									Remarks = Remarks + "    Commission Amount: " + Math.Round(item3.CommissionAmount);
								}
								if (item3.FreightAmount > 0.0)
								{
									Remarks = Remarks + "    Freight Amount: " + Math.Round(item3.FreightAmount);
								}
								if (item3.Brokery > 0.0)
								{
									Remarks = Remarks + "    Brokery Amount: " + Math.Round(item3.Brokery);
								}
							}
							if (getItemId == item3.ItemId && RowNo == 1)
							{
								if (obj.RemarksHeader != string.Empty)
								{
									InvoiceHeaderRemarksAuto = obj.RemarksHeader + " ";
								}
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + TransType + Conversion.ToString(item3.ItemQty) + " Bags  ( " + Conversion.ToString(item3.GrossWeight) + " , " + Conversion.ToString(item3.NetBillWeight) + ")  of " + Conversion.ToString(find3[0].ItemName) + "  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item3.ItemRate - item3.RateCut) + "   = Rs. " + Conversion.ToString(item3.ItemAmount);
							}
							else if (getItemId == item3.ItemId && RowNo > 1)
							{
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " , " + Conversion.ToString(item3.ItemQty) + " Bags  ( " + Conversion.ToString(item3.GrossWeight) + " , " + Conversion.ToString(item3.NetBillWeight) + ")  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item3.ItemRate - item3.RateCut) + "   = Rs. " + Conversion.ToString(item3.ItemAmount);
							}
							else if (getItemId != item3.ItemId)
							{
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + TransType + Conversion.ToString(item3.ItemQty) + " Bags  ( " + Conversion.ToString(item3.GrossWeight) + " , " + Conversion.ToString(item3.NetBillWeight) + ")  of " + Conversion.ToString(find3[0].ItemName) + "  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item3.ItemRate - item3.RateCut) + "   = Rs. " + Conversion.ToString(item3.ItemAmount);
							}
						}
						JobLot objlot3 = new JobLot();
						if (item3.JobLotId > 0)
						{
							objlot3 = CommonServies.JobLotGetByID(item3.JobLotId);
						}
						if (objlot3 != null && objlot3.AccountId > 0)
						{
							vd3.AccountId = objlot3.AccountId;
						}
						else
						{
							vd3.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						}
						vd3.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.DebitAmount = Conversion.ToDouble(item3.ItemAmount);
						vd3.CreditAmount = Conversion.ToDouble(0);
						vd3.ItemId = Conversion.ToInt(item3.ItemId);
						vd3.QtyIn = Conversion.ToDouble(item3.ItemQty);
						vd3.ItemRate = Conversion.ToDouble(item3.ItemRate);
						vd3.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
						vd3.RateCut = Conversion.ToDouble(item3.RateCut);
						vd3.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
						vd3.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
						vd3.Freight = Conversion.ToDouble(item3.FreightAmount);
						vd3.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
						vd3.Journal = Conversion.ToDouble(item3.Brokery);
						vd3.JobLotId = Conversion.ToInt(item3.JobLotId);
						vd3.Commission = Conversion.ToDouble(item3.CommissionAmount);
						vd3.OrderNo = Conversion.ToInt(item3.PurchaseOrder);
						vd3.GpNo = Conversion.ToInt(item3.GpNo);
						vd3.VehicleNo = item3.VehicleNo;
						vd3.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
						vd3.SubsidiaryAgainstTypeId = 1;
						vd3.DMultiCurrencyId = Conversion.ToInt(item3.CurrencyId);
						vd3.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
						vd3.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
						vd3.BranchesId = item3.BranchId;
						vh.voucherDetailList.Add(vd3);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
						if (objlot3 != null && objlot3.AccountId > 0)
						{
							vd4.AgainstAccountId = objlot3.AccountId;
						}
						else
						{
							vd4.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						}
						vd4.Comments = Conversion.ToString(Remarks);
						vd4.CreditAmount = Conversion.ToDouble(item3.ItemAmount);
						vd4.DebitAmount = Conversion.ToDouble(0);
						vd4.ItemId = Conversion.ToInt(item3.ItemId);
						vd4.QtyIn = Conversion.ToDouble(item3.ItemQty);
						vd4.ItemRate = Conversion.ToDouble(item3.ItemRate);
						vd4.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
						vd4.RateCut = Conversion.ToDouble(item3.RateCut);
						vd4.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
						vd4.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
						vd4.Freight = Conversion.ToDouble(item3.FreightAmount);
						vd4.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
						vd4.Journal = Conversion.ToDouble(item3.Brokery);
						vd4.JobLotId = Conversion.ToInt(item3.JobLotId);
						vd4.Commission = Conversion.ToDouble(item3.CommissionAmount);
						vd4.OrderNo = Conversion.ToInt(item3.PurchaseOrder);
						vd4.GpNo = Conversion.ToInt(item3.GpNo);
						vd4.VehicleNo = item3.VehicleNo;
						vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						if (obj.DocumentTypeId == 56 || obj.DocumentTypeId == 57 || obj.DocumentTypeId == 1603 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1653 || obj.DocumentTypeId == 1654)
						{
							vd4.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd4.SubsidiaryTypeId = 1;
						}
						vd4.DMultiCurrencyId = Conversion.ToInt(item3.CurrencyId);
						vd4.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
						vd4.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
						vd4.BranchesId = item3.BranchId;
						vh.voucherDetailList.Add(vd4);
						if (item3.ExpenseAmount > 0.0)
						{
							VoucherDetail vd5 = new VoucherDetail();
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd5.AccountId = objlot3.AccountId;
							}
							else
							{
								vd5.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							vd5.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							if (AutoRemarksFormatII)
							{
								vd5.Comments = Remarks;
							}
							else
							{
								vd5.Comments = Conversion.ToString("Other Expenses");
							}
							vd5.DebitAmount = Conversion.ToDouble(item3.ExpenseAmount);
							vd5.CreditAmount = Conversion.ToDouble(0);
							vd5.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd5.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd5);
							VoucherDetail vd8 = new VoucherDetail();
							vd8.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd8.AgainstAccountId = objlot3.AccountId;
							}
							else
							{
								vd8.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							if (AutoRemarksFormatII)
							{
								vd8.Comments = Remarks;
							}
							else
							{
								vd8.Comments = Conversion.ToString("Other Expenses");
							}
							vd8.CreditAmount = Conversion.ToDouble(item3.ExpenseAmount);
							vd8.DebitAmount = Conversion.ToDouble(0);
							vd8.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd8.SupplierCustomerId = obj.SupplierCustomerId;
							vd8.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd8.SubsidiaryTypeId = 1;
							vd8.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd8);
						}
						if (obj.DocumentTypeId == 98)
						{
							if (item3.CommissionAmount > 0.0)
							{
								VoucherDetail vd6 = new VoucherDetail();
								vd6.AccountId = Conversion.ToInt(vh.AgainstAccountId);
								if (objlot3 != null && objlot3.AccountId > 0)
								{
									vd6.AgainstAccountId = objlot3.AccountId;
								}
								else
								{
									vd6.AgainstAccountId = (CommissionDebitToExpenses ? obj.CommissionCreditAccountId : Conversion.ToInt(find3[0].PurchaseGLAC));
								}
								vd6.Comments = Conversion.ToString(obj.CommissionRemarks);
								vd6.DebitAmount = Conversion.ToDouble(item3.CommissionAmount);
								vd6.CreditAmount = Conversion.ToDouble(0);
								vd6.SupplierCustomerId = obj.CommissionAgentId;
								vd6.SubsidiaryAccountId = obj.CommissionAgentId;
								vd6.SubsidiaryTypeId = 1;
								vd6.BranchesId = item3.BranchId;
								vh.voucherDetailList.Add(vd6);
								VoucherDetail vd9 = new VoucherDetail();
								if (objlot3 != null && objlot3.AccountId > 0)
								{
									vd9.AccountId = objlot3.AccountId;
								}
								else
								{
									vd9.AccountId = (CommissionDebitToExpenses ? obj.CommissionCreditAccountId : Conversion.ToInt(find3[0].PurchaseGLAC));
								}
								vd9.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
								vd9.Comments = Conversion.ToString(obj.CommissionRemarks);
								vd9.CreditAmount = Conversion.ToDouble(item3.CommissionAmount);
								vd9.SubsidiaryAgainstTypeId = 1;
								vd9.SubsidiaryAgainstAccountId = obj.CommissionAgentId;
								vd9.BranchesId = item3.BranchId;
								vh.voucherDetailList.Add(vd9);
							}
						}
						else if (item3.CommissionAmount > 0.0)
						{
							string RemarksCommission = "";
							VoucherDetail vd7 = new VoucherDetail();
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd7.AccountId = objlot3.AccountId;
							}
							else
							{
								vd7.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							vd7.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
							if (AutoRemarksFormatII)
							{
								vd7.Comments = Remarks;
							}
							else if (obj.CommissionRemarks == string.Empty || obj.CommissionRemarks == null)
							{
								RemarksCommission = (vd7.Comments = "ItemName " + item3.ItemName + " ItemQty " + item3.ItemQty + " Amount " + item3.CommissionAmount);
							}
							else
							{
								vd7.Comments = Conversion.ToString(obj.CommissionRemarks);
							}
							vd7.DebitAmount = Conversion.ToDouble(item3.CommissionAmount);
							vd7.CreditAmount = Conversion.ToDouble(0);
							vd7.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd7.SubsidiaryAgainstTypeId = 1;
							vd7.SubsidiaryAgainstAccountId = obj.CommissionAgentId;
							vd7.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd7);
							VoucherDetail vd10 = new VoucherDetail();
							vd10.AccountId = Conversion.ToInt(vh.AgainstAccountId);
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd10.AgainstAccountId = objlot3.AccountId;
							}
							else
							{
								vd10.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							if (AutoRemarksFormatII)
							{
								vd10.Comments = Remarks;
							}
							else if (obj.CommissionRemarks == string.Empty || obj.CommissionRemarks == null)
							{
								vd10.Comments = RemarksCommission;
							}
							else
							{
								vd10.Comments = Conversion.ToString(obj.CommissionRemarks);
							}
							vd10.DebitAmount = Conversion.ToDouble(0);
							vd10.CreditAmount = Conversion.ToDouble(item3.CommissionAmount);
							vd10.SupplierCustomerId = obj.CommissionAgentId;
							vd10.SubsidiaryAccountId = obj.CommissionAgentId;
							vd10.SubsidiaryTypeId = 1;
							vd10.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd10.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd10);
						}
					}
					getItemId = item3.ItemId;
					RowNo++;
					continue;
				}
				throw new Exception("ItemGlAccount Not found");
			}
			if (getCommissionAmount > 0.0)
			{
				InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " Commission paid @ " + Conversion.ToString(obj.CommRate) + "% = Rs. " + Math.Round(getCommissionAmount);
			}
			if (obj.BrokeryAmount > 0.0)
			{
				InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " Brokery deduct @ " + obj.BrokeryRate + "% = Rs. " + Math.Round(obj.BrokeryAmount);
			}
			if (getFreightAmount > 0.0)
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
			double TotalWeightInDetailList = 0.0;
			for (int i = 0; i < itemListWithUniqueItems.ToList().Count; i++)
			{
				TotalWeightInDetailList += itemListWithUniqueItems[i].NetBillWeight;
			}
			int transId = 0;
			double TotalFreightAmountInTransporterGrid = obj.InvPurchaseInvoiceFreightList.Sum((InvPurchaseInvoiceFreight x) => x.FreightAmount);
			if (obj.InvPurchaseInvoiceFreightList.Select((InvPurchaseInvoiceFreight x) => x.TansporterId).Distinct().Count() == 1)
			{
				transId = obj.InvPurchaseInvoiceFreightList.Max((InvPurchaseInvoiceFreight x) => x.TansporterId);
			}
			foreach (InvPurchaseInvoiceDetail item2 in itemListWithUniqueItems.ToList())
			{
				if (!new HashSet<int>
				{
					56, 57, 63, 98, 138, 166, 168, 172, 1603, 1604,
					1653, 1654, 1804, 245
				}.Contains(obj.DocumentTypeId) || dtitem.Count <= 0)
				{
					continue;
				}
				JobLot objlot2 = new JobLot();
				if (item2.JobLotId > 0)
				{
					objlot2 = CommonServies.JobLotGetByID(item2.JobLotId);
				}
				List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
				if (find2.Count <= 0)
				{
					continue;
				}
				if (item2.FreightAmount > 0.0 && obj.DocumentTypeId != 138)
				{
					VoucherDetail vd14 = new VoucherDetail();
					if (objlot2 != null && objlot2.AccountId > 0)
					{
						vd14.AccountId = objlot2.AccountId;
					}
					else
					{
						vd14.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						item2.PurchaseGLAC = vd14.AccountId;
					}
					vd14.AgainstAccountId = ((obj.TransportAccountId > 0) ? obj.TransportAccountId : ((transId > 0) ? transId : vd14.AccountId));
					vd14.DebitAmount = Conversion.ToDouble(item2.FreightAmount);
					vd14.CreditAmount = 0.0;
					if (AutoRemarksFormatII)
					{
						vd14.Comments = Remarks;
					}
					else
					{
						vd14.Comments = "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item2.NetBillWeight + " = " + Math.Round(item2.FreightAmount);
					}
					vd14.BranchesId = item2.BranchId;
					vh.voucherDetailList.Add(vd14);
				}
				if (item2.Brokery > 0.0 && obj.BrokeryAmount > 0.0)
				{
					VoucherDetail vd13 = new VoucherDetail();
					if (objlot2 != null && objlot2.AccountId > 0)
					{
						vd13.AccountId = objlot2.AccountId;
					}
					else
					{
						vd13.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					}
					vd13.AgainstAccountId = vd13.AccountId;
					vd13.DebitAmount = Conversion.ToDouble(item2.Brokery);
					vd13.CreditAmount = 0.0;
					if (AutoRemarksFormatII)
					{
						vd13.Comments = Remarks;
					}
					else
					{
						vd13.Comments = "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + obj.BrokeryAmount + " / " + TotalWeightInDetailList + " * " + item2.NetBillWeight + " = " + Math.Round(item2.Brokery);
					}
					vd13.BranchesId = item2.BranchId;
					vh.voucherDetailList.Add(vd13);
				}
			}
			if (obj.DocumentTypeId == 138)
			{
				if (obj.DeliveryTerm == "Load")
				{
					foreach (InvPurchaseInvoiceDetail item in obj.invPurchaseInvoiceDetailList.ToList())
					{
						if (dtitem.Count <= 0)
						{
							continue;
						}
						JobLot objlot = new JobLot();
						if (item.JobLotId > 0)
						{
							objlot = CommonServies.JobLotGetByID(item.JobLotId);
						}
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0 && item.FreightAmount > 0.0)
						{
							VoucherDetail vd12 = new VoucherDetail();
							if (objlot != null && objlot.AccountId > 0)
							{
								vd12.AccountId = objlot.AccountId;
							}
							else
							{
								vd12.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							}
							vd12.AgainstAccountId = ((obj.TransportAccountId > 0) ? obj.TransportAccountId : vd12.AccountId);
							vd12.DebitAmount = Conversion.ToDouble(item.FreightAmount);
							vd12.CreditAmount = 0.0;
							vd12.Comments = "SupplierName : " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + " Weight " + item.NetBillWeight + " Freight " + Math.Round(item.FreightAmount) + " VehicleNo " + item.VehicleNo + " GrnNo " + item.GrnNo;
							vd12.BranchesId = item.BranchId;
							vh.voucherDetailList.Add(vd12);
							VoucherDetail vd2 = new VoucherDetail();
							vd2.AccountId = obj.TransportAccountId;
							if (objlot != null && objlot.AccountId > 0)
							{
								vd2.AgainstAccountId = objlot.AccountId;
							}
							else
							{
								vd2.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							}
							vd2.Comments = "SupplierName :  " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + "  Freight : " + Conversion.ToString(item.FreightAmount) + " VehicleNo " + item.VehicleNo + " GrnNo " + item.GrnNo;
							vd2.DebitAmount = 0.0;
							vd2.CreditAmount = Conversion.ToDouble(item.FreightAmount);
							vd2.BranchesId = item.BranchId;
							vh.voucherDetailList.Add(vd2);
						}
					}
				}
				if (obj.DeliveryTerm == "Ponch")
				{
					string freightRemarks = "SupplierName : " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + " Freight Inward = Rs. " + Math.Round(obj.FreightAmount) + " Vehicle # " + VehicleNo;
					VoucherDetail vd11 = new VoucherDetail();
					vd11.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd11.AgainstAccountId = obj.TransportAccountId;
					vd11.DebitAmount = Conversion.ToDouble(obj.FreightAmount);
					vd11.CreditAmount = 0.0;
					vd11.Comments = freightRemarks;
					vd11.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd11);
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = obj.TransportAccountId;
					vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd.Comments = freightRemarks;
					vd.DebitAmount = 0.0;
					vd.CreditAmount = Conversion.ToDouble(obj.FreightAmount);
					vd.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd);
				}
			}
			Dictionary<int, int> itemDictionary = dtitem.ToDictionary((GetItemGlIdsandItemName x) => x.Id, (GetItemGlIdsandItemName x) => x.PurchaseGLAC);
			foreach (InvPurchaseInvoiceDetail detail in obj.invPurchaseInvoiceDetailList)
			{
				if (itemDictionary.TryGetValue(detail.ItemId, out var purchaseGlId))
				{
					detail.PurchaseGLAC = purchaseGlId;
				}
			}
		}
		return vh;
	}

	public static VoucherHead ItemAndSupplierTradingFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int> { 58, 61, 64, 104, 131, 702, 245 }.Contains(obj.DocumentTypeId))
		{
			string Remarks = string.Empty;
			if (obj.invPurchaseInvoiceDetailList.Count > 0)
			{
				double TotalFreightAmountInTransporterGrid = 0.0;
				double TotalWeightInDetailList = 0.0;
				foreach (InvPurchaseInvoiceDetail item2 in obj.invPurchaseInvoiceDetailList.ToList())
				{
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName w) => w.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count > 0)
					{
						JobLot objlot2 = new JobLot();
						if (item2.JobLotId > 0)
						{
							objlot2 = CommonServies.JobLotGetByID(item2.JobLotId);
						}
						VoucherDetail vd2 = new VoucherDetail();
						if (obj.DocumentTypeId == 702)
						{
							Remarks = obj.RemarksHeader + " ItemName: " + Conversion.ToString(find2[0].ItemName) + "  Rate:" + item2.ItemRate + " Qty: " + item2.ItemQty + " Weight: " + item2.NetBillWeight + " Item Amount: " + item2.ItemAmount;
						}
						else if (obj.DocumentTypeId == 61 || obj.DocumentTypeId == 64 || obj.DocumentTypeId == 245)
						{
							Remarks = Conversion.ToString(find2[0].ItemName) + "  " + Conversion.ToString(item2?.JobLotDescription) + "  " + Conversion.ToString(item2?.UOMCodeItem) + "  " + item2.NetBillWeight + " @" + item2.ItemRate + "/- ";
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
						else if (obj.DocumentTypeId != 61 && obj.DocumentTypeId != 245 && obj.DocumentTypeId != 64 && obj.DocumentTypeId != 702)
						{
							Remarks = obj.RemarksHeader + " ItemName: " + Conversion.ToString(find2[0].ItemName) + "  Rate:" + item2.ItemRate + "Qty: " + item2.ItemQty + "Item Amount: " + item2.ItemAmount;
						}
						if (item2.FreightAmount > 0.0)
						{
							Remarks = Remarks + " Freight Amount: " + Math.Round(item2.FreightAmount, 0);
						}
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd2.AccountId = objlot2.AccountId;
						}
						else
						{
							vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
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
						vd2.BranchesId = item2.BranchId;
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd3.AgainstAccountId = objlot2.AccountId;
						}
						else
						{
							vd3.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
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
						if (obj.DocumentTypeId == 58 || obj.DocumentTypeId == 61 || obj.DocumentTypeId == 245 || obj.DocumentTypeId == 64 || obj.DocumentTypeId == 104 || obj.DocumentTypeId == 131 || obj.DocumentTypeId == 702)
						{
							vd3.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd3.SubsidiaryTypeId = 1;
						}
						vd3.BranchesId = item2.BranchId;
						vh.voucherDetailList.Add(vd3);
						TotalFreightAmountInTransporterGrid += item2.FreightAmount;
						TotalWeightInDetailList += item2.NetBillWeight;
						continue;
					}
					throw new Exception("Item Purchase GL Account Not found");
				}
				if (TotalFreightAmountInTransporterGrid > 0.0)
				{
					foreach (InvPurchaseInvoiceDetail item in obj.invPurchaseInvoiceDetailList.ToList())
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName w) => w.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0)
						{
							JobLot objlot = new JobLot();
							if (item.JobLotId > 0)
							{
								objlot = CommonServies.JobLotGetByID(item.JobLotId);
							}
							VoucherDetail vd4 = new VoucherDetail();
							if (objlot != null && objlot.AccountId > 0)
							{
								vd4.AccountId = objlot.AccountId;
							}
							else
							{
								vd4.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							}
							vd4.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
							vd4.DebitAmount = Conversion.ToDouble(item.FreightAmount);
							vd4.Comments = "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item.NetBillWeight + " = " + Math.Round(item.FreightAmount);
							vd4.CreditAmount = Conversion.ToDouble(0);
							vd4.BranchesId = item.BranchId;
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

	public static VoucherHead PurchaseReturnFinancial(Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.DocumentTypeId == 59 && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			List<InvPurchaseInvoiceDetail> itemListWithUniqueItems = new List<InvPurchaseInvoiceDetail>();
			List<InvPurchaseInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<InvPurchaseInvoiceDetail>>(JsonConvert.SerializeObject(obj.invPurchaseInvoiceDetailList.ToList()));
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
				double itemAmountAtI = 0.0;
				double itemExpenseAmountAtI = 0.0;
				double itemFreightAmountAtI = 0.0;
				double itemCommissionAmountAtI = 0.0;
				double itemQtyAtI = 0.0;
				double itemWeightAtI = 0.0;
				double itemWeightCutAtI = 0.0;
				double itemRateCutAtI = 0.0;
				double itemEmptyBagsDudectionAtI = 0.0;
				double itemGrossWeightAtI = 0.0;
				for (int n = 0; n < dummyArray.Count; n++)
				{
					if (itemListWithUniqueItems[l].ItemId == dummyArray[n].ItemId && itemListWithUniqueItems[l].ItemRate == dummyArray[n].ItemRate && itemListWithUniqueItems[l].WarehouseId == dummyArray[n].WarehouseId)
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
			foreach (InvPurchaseInvoiceDetail item2 in itemListWithUniqueItems.ToList())
			{
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count <= 0)
					{
						continue;
					}
					VoucherDetail vd2 = new VoucherDetail();
					if (obj.RemarksHeader != string.Empty)
					{
						Remarks = obj.RemarksHeader;
					}
					Remarks = Remarks + "   NoOfBags: " + item2.ItemQty + "   " + Conversion.ToString(find2[0].ItemName) + "  GrossWeight: " + item2.GrossWeight + "  Net Weight: " + item2.NetBillWeight + "  Rate:" + item2.ItemRate;
					if (item2.EBTotalWt > 0.0)
					{
						Remarks = Remarks + "   EmptyBagsDeduction: " + item2.EBTotalWt;
					}
					if (item2.RateCut > 0.0)
					{
						Remarks = Remarks + "   RateCut:" + item2.RateCut;
					}
					if (item2.ExpenseAmount > 0.0)
					{
						Remarks = Remarks + "    Expense Amount: " + item2.ExpenseAmount;
					}
					if (item2.CommissionAmount > 0.0)
					{
						Remarks = Remarks + "    Commission Amount: " + item2.CommissionAmount;
					}
					if (item2.FreightAmount > 0.0)
					{
						Remarks = Remarks + "    Freight Amount: " + item2.FreightAmount;
					}
					JobLot objlot2 = new JobLot();
					if (item2.JobLotId > 0)
					{
						objlot2 = CommonServies.JobLotGetByID(item2.JobLotId);
					}
					VoucherDetail vd3 = new VoucherDetail();
					vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
					if (objlot2 != null && objlot2.AccountId > 0)
					{
						vd3.AgainstAccountId = objlot2.AccountId;
					}
					else
					{
						vd3.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					}
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
					vd3.BranchesId = item2.BranchId;
					vh.voucherDetailList.Add(vd3);
					if (objlot2 != null && objlot2.AccountId > 0)
					{
						vd2.AccountId = objlot2.AccountId;
					}
					else
					{
						vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					}
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
					vd2.BranchesId = item2.BranchId;
					vh.voucherDetailList.Add(vd2);
					if (item2.ExpenseAmount > 0.0)
					{
						VoucherDetail vd6 = new VoucherDetail();
						vd6.AccountId = Conversion.ToInt(vh.RefAccountId);
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd6.AgainstAccountId = objlot2.AccountId;
						}
						else
						{
							vd6.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
						vd6.Comments = Conversion.ToString("Other Expenses");
						vd6.DebitAmount = Conversion.ToDouble(item2.ExpenseAmount);
						vd6.CreditAmount = Conversion.ToDouble(0);
						vd6.BranchesId = item2.BranchId;
						vh.voucherDetailList.Add(vd6);
						VoucherDetail vd4 = new VoucherDetail();
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd4.AccountId = objlot2.AccountId;
						}
						else
						{
							vd4.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
						vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd4.Comments = Conversion.ToString("Other Expenses");
						vd4.CreditAmount = Conversion.ToDouble(item2.ExpenseAmount);
						vd4.DebitAmount = Conversion.ToDouble(0);
						vd4.BranchesId = item2.BranchId;
						vh.voucherDetailList.Add(vd4);
					}
					if (item2.CommissionAmount > 0.0)
					{
						VoucherDetail vd5 = new VoucherDetail();
						vd5.AccountId = Conversion.ToInt(vh.AgainstAccountId);
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd5.AgainstAccountId = objlot2.AccountId;
						}
						else
						{
							vd5.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
						vd5.Comments = Conversion.ToString(obj.CommissionRemarks);
						vd5.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
						vd5.CreditAmount = Conversion.ToDouble(0);
						vd5.BranchesId = item2.BranchId;
						vh.voucherDetailList.Add(vd5);
						VoucherDetail vd7 = new VoucherDetail();
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd7.AccountId = objlot2.AccountId;
						}
						else
						{
							vd7.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
						vd7.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
						vd7.Comments = Conversion.ToString(obj.CommissionRemarks);
						vd7.DebitAmount = Conversion.ToDouble(0);
						vd7.CreditAmount = Conversion.ToDouble(item2.CommissionAmount);
						vd7.BranchesId = item2.BranchId;
						vh.voucherDetailList.Add(vd7);
					}
					continue;
				}
				throw new Exception("ItemGlAccount Not found");
			}
			double TotalWeightInDetailList = 0.0;
			for (int j = 0; j < itemListWithUniqueItems.ToList().Count; j++)
			{
				TotalWeightInDetailList += itemListWithUniqueItems[j].NetBillWeight;
			}
			double TotalFreightAmountInTransporterGrid = 0.0;
			for (int i = 0; i < obj.InvPurchaseInvoiceFreightList.ToList().Count; i++)
			{
				TotalFreightAmountInTransporterGrid += obj.InvPurchaseInvoiceFreightList[i].FreightAmount;
			}
			foreach (InvPurchaseInvoiceDetail item in itemListWithUniqueItems.ToList())
			{
				if (obj.DocumentTypeId != 59 || dtitem.Count <= 0)
				{
					continue;
				}
				JobLot objlot = new JobLot();
				if (item.JobLotId > 0)
				{
					objlot = CommonServies.JobLotGetByID(item.JobLotId);
				}
				List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
				if (find.Count > 0 && item.FreightAmount > 0.0)
				{
					VoucherDetail vd8 = new VoucherDetail();
					if (objlot != null && objlot.AccountId > 0)
					{
						vd8.AccountId = objlot.AccountId;
					}
					else
					{
						vd8.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
					}
					vd8.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
					vd8.CreditAmount = Conversion.ToDouble(item.FreightAmount);
					vd8.DebitAmount = Conversion.ToDouble(0);
					vd8.Comments = "LessToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item.NetBillWeight + " = " + item.FreightAmount;
					vd8.BranchesId = item.BranchId;
					vh.voucherDetailList.Add(vd8);
				}
			}
		}
		return vh;
	}
}
