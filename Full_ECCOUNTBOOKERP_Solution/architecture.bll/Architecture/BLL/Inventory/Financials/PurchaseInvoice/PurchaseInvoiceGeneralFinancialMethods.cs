using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Newtonsoft.Json;

namespace Architecture.BLL.Inventory.Financials.PurchaseInvoice;

public class PurchaseInvoiceGeneralFinancialMethods
{
	public static VoucherHead EmptyBagsFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int> { 56, 57, 98, 138, 166, 168, 172 }.Contains(obj.DocumentTypeId) && obj.InvPurchaseInvoiceEmptyBagslist.Count > 0)
		{
			double PurAgainstWeight = 0.0;
			bool emptyBagPurAgWtCreditAcExists = obj.InvPurchaseInvoiceEmptyBagslist.Any((InvPurchaseInvoiceEmptyBags r) => (r.TypeId == 2 || r.TypeId == 3) && r.CreditAccountId > 0);
			foreach (InvPurchaseInvoiceEmptyBags item2 in obj.InvPurchaseInvoiceEmptyBagslist.ToList())
			{
				if (item2.TypeId == 1)
				{
					if (Conversion.ToInt(item2.ItemId) <= 0 || Conversion.ToInt(item2.PurchaseQty) <= 0)
					{
						continue;
					}
					if (dtitem.Count <= 0)
					{
						throw new Exception("ItemId Not found against Packing Material");
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName u) => u.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find.Count > 0)
					{
						double Amount = Conversion.ToDouble(item2.PurchaseQty) * Conversion.ToDouble(item2.Rate);
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						if (item2.Remarks == string.Empty || item2.Remarks == null)
						{
							vd.Comments = "ItemName: " + find[0].ItemName + " Rate: " + item2.Rate + " Amount: " + Amount;
						}
						else
						{
							vd.Comments = Conversion.ToString(item2.Remarks);
						}
						if (Amount == 0.0)
						{
							throw new Exception("Packing Material Amount Field Required");
						}
						vd.DebitAmount = Amount;
						vd.CreditAmount = Conversion.ToDouble(0);
						vd.ItemId = Conversion.ToInt(item2.ItemId);
						vd.BranchesId = obj.BranchesId;
						vh.voucherDetailList.Add(vd);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd4.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						if (item2.Remarks == string.Empty || item2.Remarks == null)
						{
							vd4.Comments = "ItemName: " + find[0].ItemName + " Rate: " + item2.Rate + " Amount: " + Amount;
						}
						else
						{
							vd4.Comments = Conversion.ToString(item2.Remarks);
						}
						vd4.DebitAmount = Conversion.ToDouble(0);
						vd4.CreditAmount = Amount;
						vd4.ItemId = Conversion.ToInt(item2.ItemId);
						vd4.SupplierCustomerId = obj.SupplierCustomerId;
						vd4.SubsidiaryAccountId = obj.SupplierCustomerId;
						vd4.SubsidiaryTypeId = 1;
						vd4.BranchesId = obj.BranchesId;
						vh.voucherDetailList.Add(vd4);
					}
				}
				else
				{
					if ((item2.TypeId != 2 && item2.TypeId != 3) || Conversion.ToInt(item2.ItemId) <= 0 || Conversion.ToInt(item2.PurchaseQty) <= 0)
					{
						continue;
					}
					if (item2.CreditAccountId == 0 && emptyBagPurAgWtCreditAcExists)
					{
						throw new Exception("Credit Account Not Found against Packing Material");
					}
					if (dtitem.Count <= 0)
					{
						throw new Exception("ItemId Not found against Packing Material");
					}
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName u) => u.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count <= 0)
					{
						continue;
					}
					double Amount2 = Conversion.ToDouble(item2.PurchaseQty) * Conversion.ToDouble(item2.Rate);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					vd2.AgainstAccountId = (emptyBagPurAgWtCreditAcExists ? Conversion.ToInt(item2.CreditAccountId) : Conversion.ToInt(find2[0].PurchaseGLAC));
					if (item2.Remarks == string.Empty || item2.Remarks == null)
					{
						vd2.Comments = "ItemName: " + find2[0].ItemName + " Rate: " + item2.Rate + " Amount: " + Amount2;
					}
					else
					{
						vd2.Comments = Conversion.ToString(item2.Remarks);
					}
					if (Amount2 == 0.0)
					{
						throw new Exception("Packing Material Amount Field Required");
					}
					vd2.DebitAmount = Amount2;
					vd2.ItemId = Conversion.ToInt(item2.ItemId);
					vd2.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd2);
					PurAgainstWeight += vd2.DebitAmount;
					if (emptyBagPurAgWtCreditAcExists)
					{
						VoucherDetail vd5 = new VoucherDetail();
						vd5.AccountId = Conversion.ToInt(item2.CreditAccountId);
						vd5.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						if (item2.Remarks == string.Empty || item2.Remarks == null)
						{
							vd5.Comments = "ItemName: " + find2[0].ItemName + " Rate: " + item2.Rate + " Amount: " + Amount2;
						}
						else
						{
							vd5.Comments = Conversion.ToString(item2.Remarks);
						}
						vd5.CreditAmount = Amount2;
						vd5.ItemId = Conversion.ToInt(item2.ItemId);
						vd5.BranchesId = obj.BranchesId;
						vh.voucherDetailList.Add(vd5);
					}
				}
			}
			if (PurAgainstWeight > 0.0 && !emptyBagPurAgWtCreditAcExists)
			{
				foreach (KeyValuePair<int, double> item in (from x in obj.invPurchaseInvoiceDetailList
					group x by x.ItemId).ToDictionary((IGrouping<int, InvPurchaseInvoiceDetail> g) => g.Key, (IGrouping<int, InvPurchaseInvoiceDetail> g) => g.Sum((InvPurchaseInvoiceDetail x) => x.EbPurAgainstWeightAmount)))
				{
					if (!(item.Value <= 0.0))
					{
						GetItemGlIdsandItemName matchedItem = dtitem.FirstOrDefault((GetItemGlIdsandItemName u) => u.Id == Conversion.ToInt(item.Key));
						if (matchedItem != null)
						{
							VoucherDetail vd3 = new VoucherDetail
							{
								AccountId = Conversion.ToInt(matchedItem.PurchaseGLAC),
								AgainstAccountId = Conversion.ToInt(matchedItem.PurchaseGLAC),
								Comments = "Empty Bags Purchase against " + matchedItem.ItemName + " or Free of Cost Empty Bags",
								ItemId = Conversion.ToInt(matchedItem.Id),
								CreditAmount = item.Value
							};
							vh.voucherDetailList.Add(vd3);
						}
					}
				}
			}
		}
		return vh;
	}

	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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
				if (Conversion.ToInt(item.ChartofAccountId) > 0 && (item.JvCredit > 0.0 || item.JvDebit > 0.0))
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

	public static VoucherHead SalesTaxFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int> { 58, 63, 64, 131, 702, 1603, 1604, 1653, 1654 }.Contains(obj.DocumentTypeId) && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			foreach (InvPurchaseInvoiceDetail item in obj.invPurchaseInvoiceDetailList.ToList())
			{
				if (item.TaxAmount > 0.0)
				{
					VoucherDetail vd2 = new VoucherDetail();
					if (obj.DocumentTypeId == 1603 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1653 || obj.DocumentTypeId == 1654 || obj.DocumentTypeId == 702)
					{
						vd2.AccountId = obj.TaxAccountId;
					}
					else
					{
						vd2.AccountId = obj.ReferencePartyId;
					}
					vd2.Comments = item.TaxPercent + "Tax %  " + item.TaxAmount;
					vd2.AgainstAccountId = vh.RefAccountId;
					vd2.DebitAmount = item.TaxAmount;
					vd2.TaxesTotalAmount = item.TaxAmount;
					vd2.TaxPrcnt = item.TaxPercent;
					vd2.IsTaxable = "True";
					vd2.TaxTypeId = item.TaxNameId;
					vd2.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd2);
					VoucherDetail vd1 = new VoucherDetail();
					vd1.AccountId = vh.RefAccountId;
					if (obj.DocumentTypeId == 1603 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1653 || obj.DocumentTypeId == 1654 || obj.DocumentTypeId == 702)
					{
						vd1.AgainstAccountId = obj.TaxAccountId;
					}
					else
					{
						vd1.AgainstAccountId = obj.ReferencePartyId;
					}
					vd1.Comments = item.TaxPercent + "Tax %  " + item.TaxAmount;
					vd1.CreditAmount = item.TaxAmount;
					vd1.TaxesTotalAmount = item.TaxAmount;
					vd1.IsTaxable = "True";
					vd1.TaxPrcnt = item.TaxPercent;
					vd1.TaxTypeId = item.TaxNameId;
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
		}
		return vh;
	}

	public static VoucherHead FreightFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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

	public static VoucherHead BrokeryBeHalfOnPartyFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj)
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

	public static VoucherHead ItemAndSupplierByWeightFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		bool InventoryFinancialsEffectsInActive = false;
		InventoryFinancialsEffectsInActive = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive"));
		if (new HashSet<int>
		{
			56, 57, 63, 98, 138, 166, 168, 172, 1603, 1604,
			1653, 1654, 1804
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
			foreach (InvPurchaseInvoiceDetail item4 in itemListWithUniqueItems.ToList())
			{
				Remarks = "";
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find4 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item4.ItemId)).ToList();
					if (find4.Count > 0)
					{
						if (RowNo == 1)
						{
							getOrderNo = item4.PurchaseOrder;
							getItemId = item4.ItemId;
							VehicleNo = item4.VehicleNo;
						}
						double Mond = Conversion.ToDouble(item4.NetBillWeight) / 40.0;
						item4.ItemName = Conversion.ToString(find4[0].ItemName);
						VoucherDetail vd3 = new VoucherDetail();
						if (obj.DocumentTypeId == 63 || obj.DocumentTypeId == 1603 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1653 || obj.DocumentTypeId == 1654)
						{
							if (obj.RemarksHeader != string.Empty)
							{
								Remarks = obj.RemarksHeader;
							}
							Remarks = Remarks + "   Item: " + Conversion.ToString(find4[0].ItemName) + ",   Qty: " + item4.ItemQty + ",   Rate:" + item4.ItemRate;
							if (item4.ExpenseAmount > 0.0)
							{
								Remarks = Remarks + "    Expense Amount: " + item4.ExpenseAmount;
							}
							if (item4.CommissionAmount > 0.0)
							{
								Remarks = Remarks + "    Commission Amount: " + item4.CommissionAmount;
							}
							if (item4.FreightAmount > 0.0)
							{
								Remarks = Remarks + "   Freight Amount: " + item4.FreightAmount;
							}
						}
						else
						{
							if (item4.CommissionAmount > 0.0)
							{
								getCommissionAmount += item4.CommissionAmount;
							}
							if (item4.FreightAmount > 0.0)
							{
								getFreightAmount += item4.FreightAmount;
							}
							string TransType = "Purchase ";
							if (obj.DocumentTypeId == 98)
							{
								TransType = "Sale Return ";
							}
							if (AutoRemarksFormatII)
							{
								Remarks = TransType + Conversion.ToString(item4.ItemQty) + " Bags  ( " + Conversion.ToString(item4.GrossWeight) + " , " + Conversion.ToString(item4.NetBillWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + "  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item4.ItemRate - item4.RateCut) + "   = Rs. " + Conversion.ToString(item4.ItemAmount);
								if (item4.CommissionAmount > 0.0)
								{
									Remarks = Remarks + " Commission paid @ " + Conversion.ToString(obj.CommRate) + "% = Rs. " + Math.Round(item4.CommissionAmount);
								}
								if (obj.BrokeryAmount > 0.0)
								{
									Remarks = Remarks + " Commission deduct @ " + obj.BrokeryRate + "% = Rs. " + Math.Round(obj.BrokeryAmount);
								}
								if (item4.FreightAmount > 0.0)
								{
									Remarks = Remarks + " Freight Inward = Rs. " + Math.Round(item4.FreightAmount) + " Vehicle # " + item4.VehicleNo;
								}
								if (item4.PurchaseOrder > 0)
								{
									Remarks = Remarks + " Under contract # " + item4.PurchaseOrder;
								}
								if (obj.ManualBillNo != string.Empty)
								{
									Remarks = Remarks + " Manual # " + obj.ManualBillNo;
								}
								if (!string.IsNullOrEmpty(item4.WareHouseName))
								{
									Remarks = Remarks + " Warehouse " + item4.WareHouseName;
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
								Remarks = Remarks + "   NoOfBags: " + Conversion.ToString(item4.ItemQty) + "   " + Conversion.ToString(find4[0].ItemName) + "  GrossWeight: " + Conversion.ToString(item4.GrossWeight) + "  Net Weight: " + Conversion.ToString(item4.NetBillWeight) + "  Rate:" + Conversion.ToString(item4.ItemRate);
								if (item4.RateCut > 0.0 || item4.RateCut < 0.0)
								{
									double NetRate = 0.0;
									if (item4.RateCut > 0.0)
									{
										NetRate = item4.ItemRate - item4.RateCut;
									}
									else if (item4.RateCut < 0.0)
									{
										NetRate = item4.ItemRate + item4.RateCut;
									}
									Remarks = Remarks + "   RateCut:" + Conversion.ToString(item4.RateCut) + " NetRate " + Conversion.ToString(NetRate);
								}
								if (item4.EBTotalWt > 0.0)
								{
									Remarks = Remarks + "   BagsDeduction: " + Conversion.ToString(item4.EBTotalWt);
								}
								if (item4.WeightCutTotal > 0.0)
								{
									Remarks = Remarks + "   WeightCut: " + Conversion.ToString(item4.WeightCutTotal);
								}
								if (item4.AdLsWeight > 0.0 || item4.AdLsWeight < 0.0)
								{
									Remarks = Remarks + "   AdLsWeight: " + Conversion.ToString(item4.AdLsWeight);
								}
								if (item4.ExpenseAmount > 0.0)
								{
									Remarks = Remarks + "    Expense Amount: " + Math.Round(item4.ExpenseAmount);
								}
								if (item4.CommissionAmount > 0.0)
								{
									Remarks = Remarks + "    Commission Amount: " + Math.Round(item4.CommissionAmount);
								}
								if (item4.FreightAmount > 0.0)
								{
									Remarks = Remarks + "    Freight Amount: " + Math.Round(item4.FreightAmount);
								}
								if (item4.Brokery > 0.0)
								{
									Remarks = Remarks + "    Brokery Amount: " + Math.Round(item4.Brokery);
								}
								if (!string.IsNullOrEmpty(item4.WareHouseName))
								{
									Remarks = Remarks + " Warehouse " + item4.WareHouseName;
								}
							}
							if (getItemId == item4.ItemId && RowNo == 1)
							{
								if (obj.RemarksHeader != string.Empty)
								{
									InvoiceHeaderRemarksAuto = obj.RemarksHeader + " ";
								}
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + TransType + Conversion.ToString(item4.ItemQty) + " Bags  ( " + Conversion.ToString(item4.GrossWeight) + " , " + Conversion.ToString(item4.NetBillWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + "  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item4.ItemRate - item4.RateCut) + "   = Rs. " + Conversion.ToString(item4.ItemAmount);
							}
							else if (getItemId == item4.ItemId && RowNo > 1)
							{
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + " , " + Conversion.ToString(item4.ItemQty) + " Bags  ( " + Conversion.ToString(item4.GrossWeight) + " , " + Conversion.ToString(item4.NetBillWeight) + ")  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item4.ItemRate - item4.RateCut) + "   = Rs. " + Conversion.ToString(item4.ItemAmount);
							}
							else if (getItemId != item4.ItemId)
							{
								InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + TransType + Conversion.ToString(item4.ItemQty) + " Bags  ( " + Conversion.ToString(item4.GrossWeight) + " , " + Conversion.ToString(item4.NetBillWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + "  " + Math.Round(Mond, 2) + " Mond @ Rs. " + Conversion.ToString(item4.ItemRate - item4.RateCut) + "   = Rs. " + Conversion.ToString(item4.ItemAmount);
							}
						}
						JobLot objlot3 = new JobLot();
						if (item4.JobLotId > 0)
						{
							objlot3 = jobLot.GetByID(item4.JobLotId);
						}
						if (objlot3 != null && objlot3.AccountId > 0)
						{
							vd3.AccountId = objlot3.AccountId;
						}
						else
						{
							vd3.AccountId = ((obj.DocumentTypeId == 98) ? Conversion.ToInt(find4[0].SaleGLAC) : Conversion.ToInt(find4[0].PurchaseGLAC));
						}
						vd3.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
						vd3.CreditAmount = Conversion.ToDouble(0);
						vd3.ItemId = Conversion.ToInt(item4.ItemId);
						vd3.QtyIn = Conversion.ToDouble(item4.ItemQty);
						vd3.ItemRate = Conversion.ToDouble(item4.ItemRate);
						vd3.WeightIn = Conversion.ToDouble(item4.NetBillWeight);
						vd3.RateCut = Conversion.ToDouble(item4.RateCut);
						vd3.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
						vd3.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
						vd3.Freight = Conversion.ToDouble(item4.FreightAmount);
						vd3.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
						vd3.Journal = Conversion.ToDouble(item4.Brokery);
						vd3.JobLotId = Conversion.ToInt(item4.JobLotId);
						vd3.Commission = Conversion.ToDouble(item4.CommissionAmount);
						vd3.OrderNo = Conversion.ToInt(item4.PurchaseOrder);
						vd3.GpNo = Conversion.ToInt(item4.GpNo);
						vd3.VehicleNo = item4.VehicleNo;
						vd3.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
						vd3.SubsidiaryAgainstTypeId = 1;
						vd3.DMultiCurrencyId = Conversion.ToInt(item4.CurrencyId);
						vd3.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
						vd3.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
						vd3.BranchesId = item4.BranchId;
						vh.voucherDetailList.Add(vd3);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
						if (objlot3 != null && objlot3.AccountId > 0)
						{
							vd4.AgainstAccountId = objlot3.AccountId;
						}
						else
						{
							vd4.AgainstAccountId = ((obj.DocumentTypeId == 98) ? Conversion.ToInt(find4[0].SaleGLAC) : Conversion.ToInt(find4[0].PurchaseGLAC));
						}
						vd4.Comments = Conversion.ToString(Remarks);
						vd4.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
						vd4.DebitAmount = Conversion.ToDouble(0);
						vd4.ItemId = Conversion.ToInt(item4.ItemId);
						vd4.QtyIn = Conversion.ToDouble(item4.ItemQty);
						vd4.ItemRate = Conversion.ToDouble(item4.ItemRate);
						vd4.WeightIn = Conversion.ToDouble(item4.NetBillWeight);
						vd4.RateCut = Conversion.ToDouble(item4.RateCut);
						vd4.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
						vd4.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
						vd4.Freight = Conversion.ToDouble(item4.FreightAmount);
						vd4.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
						vd4.Journal = Conversion.ToDouble(item4.Brokery);
						vd4.JobLotId = Conversion.ToInt(item4.JobLotId);
						vd4.Commission = Conversion.ToDouble(item4.CommissionAmount);
						vd4.OrderNo = Conversion.ToInt(item4.PurchaseOrder);
						vd4.GpNo = Conversion.ToInt(item4.GpNo);
						vd4.VehicleNo = item4.VehicleNo;
						vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						if (obj.DocumentTypeId == 56 || obj.DocumentTypeId == 57 || obj.DocumentTypeId == 1603 || obj.DocumentTypeId == 1604 || obj.DocumentTypeId == 1653 || obj.DocumentTypeId == 1654)
						{
							vd4.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd4.SubsidiaryTypeId = 1;
						}
						vd4.DMultiCurrencyId = Conversion.ToInt(item4.CurrencyId);
						vd4.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
						vd4.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
						vd4.BranchesId = item4.BranchId;
						vh.voucherDetailList.Add(vd4);
						if (item4.ExpenseAmount > 0.0)
						{
							VoucherDetail vd5 = new VoucherDetail();
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd5.AccountId = objlot3.AccountId;
							}
							else
							{
								vd5.AccountId = ((obj.DocumentTypeId == 98) ? Conversion.ToInt(find4[0].SaleGLAC) : Conversion.ToInt(find4[0].PurchaseGLAC));
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
							vd5.DebitAmount = Conversion.ToDouble(item4.ExpenseAmount);
							vd5.CreditAmount = Conversion.ToDouble(0);
							vd5.ItemId = Conversion.ToInt(item4.ItemId);
							vd5.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd5.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd5);
							VoucherDetail vd8 = new VoucherDetail();
							vd8.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd8.AgainstAccountId = objlot3.AccountId;
							}
							else
							{
								vd8.AgainstAccountId = ((obj.DocumentTypeId == 98) ? Conversion.ToInt(find4[0].SaleGLAC) : Conversion.ToInt(find4[0].PurchaseGLAC));
							}
							if (AutoRemarksFormatII)
							{
								vd8.Comments = Remarks;
							}
							else
							{
								vd8.Comments = Conversion.ToString("Other Expenses");
							}
							vd8.CreditAmount = Conversion.ToDouble(item4.ExpenseAmount);
							vd8.DebitAmount = Conversion.ToDouble(0);
							vd8.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd8.ItemId = Conversion.ToInt(item4.ItemId);
							vd8.SupplierCustomerId = obj.SupplierCustomerId;
							vd8.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd8.SubsidiaryTypeId = 1;
							vd8.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd8);
						}
						if (obj.DocumentTypeId == 98)
						{
							if (item4.CommissionAmount > 0.0)
							{
								VoucherDetail vd6 = new VoucherDetail();
								vd6.AccountId = Conversion.ToInt(vh.AgainstAccountId);
								if (objlot3 != null && objlot3.AccountId > 0)
								{
									vd6.AgainstAccountId = objlot3.AccountId;
								}
								else
								{
									vd6.AgainstAccountId = (CommissionDebitToExpenses ? obj.CommissionCreditAccountId : ((obj.DocumentTypeId == 98) ? Conversion.ToInt(find4[0].SaleGLAC) : Conversion.ToInt(find4[0].PurchaseGLAC)));
								}
								vd6.Comments = Conversion.ToString(obj.CommissionRemarks);
								vd6.DebitAmount = Conversion.ToDouble(item4.CommissionAmount);
								vd6.CreditAmount = Conversion.ToDouble(0);
								vd6.SupplierCustomerId = obj.CommissionAgentId;
								vd6.SubsidiaryAccountId = obj.CommissionAgentId;
								vd6.SubsidiaryTypeId = 1;
								vd6.ItemId = Conversion.ToInt(item4.ItemId);
								vd6.JobLotId = Conversion.ToInt(item4.JobLotId);
								vd6.BranchesId = item4.BranchId;
								vh.voucherDetailList.Add(vd6);
								VoucherDetail vd10 = new VoucherDetail();
								if (objlot3 != null && objlot3.AccountId > 0)
								{
									vd10.AccountId = objlot3.AccountId;
								}
								else
								{
									vd10.AccountId = (CommissionDebitToExpenses ? obj.CommissionCreditAccountId : ((obj.DocumentTypeId == 98) ? Conversion.ToInt(find4[0].SaleGLAC) : Conversion.ToInt(find4[0].PurchaseGLAC)));
								}
								vd10.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
								vd10.Comments = Conversion.ToString(obj.CommissionRemarks);
								vd10.CreditAmount = Conversion.ToDouble(item4.CommissionAmount);
								vd10.ItemId = Conversion.ToInt(item4.ItemId);
								vd10.JobLotId = Conversion.ToInt(item4.JobLotId);
								vd10.SubsidiaryAgainstTypeId = 1;
								vd10.SubsidiaryAgainstAccountId = obj.CommissionAgentId;
								vd10.BranchesId = item4.BranchId;
								vh.voucherDetailList.Add(vd10);
							}
						}
						else if (item4.CommissionAmount > 0.0)
						{
							string RemarksCommission = "";
							VoucherDetail vd7 = new VoucherDetail();
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd7.AccountId = objlot3.AccountId;
							}
							else
							{
								vd7.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							}
							vd7.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
							if (AutoRemarksFormatII)
							{
								vd7.Comments = Remarks;
							}
							else if (obj.CommissionRemarks == string.Empty || obj.CommissionRemarks == null)
							{
								RemarksCommission = (vd7.Comments = "ItemName " + item4.ItemName + " ItemQty " + item4.ItemQty + " Amount " + item4.CommissionAmount);
							}
							else
							{
								vd7.Comments = Conversion.ToString(obj.CommissionRemarks);
							}
							vd7.DebitAmount = Conversion.ToDouble(item4.CommissionAmount);
							vd7.CreditAmount = Conversion.ToDouble(0);
							vd7.ItemId = Conversion.ToInt(item4.ItemId);
							vd7.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd7.SubsidiaryAgainstTypeId = 1;
							vd7.SubsidiaryAgainstAccountId = obj.CommissionAgentId;
							vd7.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd7);
							VoucherDetail vd11 = new VoucherDetail();
							vd11.AccountId = Conversion.ToInt(vh.AgainstAccountId);
							if (objlot3 != null && objlot3.AccountId > 0)
							{
								vd11.AgainstAccountId = objlot3.AccountId;
							}
							else
							{
								vd11.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							}
							if (AutoRemarksFormatII)
							{
								vd11.Comments = Remarks;
							}
							else if (obj.CommissionRemarks == string.Empty || obj.CommissionRemarks == null)
							{
								vd11.Comments = RemarksCommission;
							}
							else
							{
								vd11.Comments = Conversion.ToString(obj.CommissionRemarks);
							}
							vd11.DebitAmount = Conversion.ToDouble(0);
							vd11.CreditAmount = Conversion.ToDouble(item4.CommissionAmount);
							vd11.SupplierCustomerId = obj.CommissionAgentId;
							vd11.SubsidiaryAccountId = obj.CommissionAgentId;
							vd11.SubsidiaryTypeId = 1;
							vd11.ItemId = Conversion.ToInt(item4.ItemId);
							vd11.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd11.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd11);
						}
					}
					getItemId = item4.ItemId;
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
			if (obj.DocumentTypeId == 98)
			{
				bool SaleCostingJobOrderWise = false;
				bool CgsAllow = false;
				bool FIFOCgs = CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 5);
				SaleCostingJobOrderWise = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "SaleCostingJobOrderWise"));
				CgsAllow = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow"));
				if (!InventoryFinancialsEffectsInActive && (CgsAllow || FIFOCgs || SaleCostingJobOrderWise))
				{
					foreach (InvPurchaseInvoiceDetail item3 in obj.invPurchaseInvoiceDetailList.ToList())
					{
						if (dtitem.Count > 0)
						{
							List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item3.ItemId)).ToList();
							if (find3.Count > 0)
							{
								item3.ItemName = Conversion.ToString(find3[0].ItemName);
								string RemarksCgs = "";
								double CgsAmount = 0.0;
								ReportsParameters parameters = new ReportsParameters();
								parameters.OrganizationId = obj.OrganizationId;
								parameters.CompanyId = obj.CompanyId;
								parameters.ItemId = item3.ItemId;
								parameters.DocDate = obj.DocDate;
								parameters.RateUOM = item3.UomScheduleIdRate;
								if (obj.Id > 0)
								{
									parameters.DocumentTypeId = obj.DocumentTypeId;
									parameters.Id = obj.Id;
								}
								parameters.RefDocumentTypeId = item3.RefDocumentTypeId;
								parameters.RefDocIdNo = item3.RefDocId;
								parameters.RefDocSubId = item3.RefDocSubId;
								DataTable dtcgs = CommonRepository.getAvgRateOrlastPurchaseRate(parameters);
								if (dtcgs != null && dtcgs.Rows.Count > 0)
								{
									item3.ItemCgsRate = Conversion.ToDouble(dtcgs.Rows[0]["CgsRate"]);
									item3.EquivalentPoRate = Conversion.ToDouble(dtcgs.Rows[0]["RateUom"]);
								}
								if (item3.ItemCgsRate <= 0.0)
								{
									throw new Exception("CGS Rate not found against " + item3.ItemName + " and row qty is " + item3.ItemQty);
								}
								RemarksCgs = "Bags: " + item3.ItemQty + "   " + Conversion.ToString(find3[0].ItemName) + " Net Weight: " + item3.NetBillWeight + " Rate: " + item3.ItemCgsRate;
								if (item3.CommissionAmount > 0.0)
								{
									RemarksCgs = RemarksCgs + "    Commission Amount: " + item3.CommissionAmount;
								}
								CgsAmount = item3.NetBillWeight / item3.EquivalentPoRate * item3.ItemCgsRate;
								VoucherDetail vd9 = new VoucherDetail();
								vd9.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
								vd9.AgainstAccountId = Conversion.ToInt(find3[0].COGSGLAC);
								vd9.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd9.DebitAmount = Conversion.ToDouble(CgsAmount);
								vd9.ItemId = Conversion.ToInt(item3.ItemId);
								vd9.QtyIn = Conversion.ToDouble(item3.ItemQty);
								vd9.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
								vd9.ItemCgsRate = Conversion.ToDouble(item3.ItemCgsRate);
								vd9.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd9.Commission = Conversion.ToDouble(item3.CommissionAmount);
								vd9.GpNo = Conversion.ToInt(item3.GpNo);
								vd9.VehicleNo = item3.VehicleNo;
								vd9.JobLotId = Conversion.ToInt(item3.JobLotId);
								vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd9);
								VoucherDetail vd12 = new VoucherDetail();
								vd12.AccountId = Conversion.ToInt(find3[0].COGSGLAC);
								vd12.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
								vd12.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd12.CreditAmount = Conversion.ToDouble(CgsAmount);
								vd12.ItemId = Conversion.ToInt(item3.ItemId);
								vd12.QtyIn = Conversion.ToDouble(item3.ItemQty);
								vd12.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
								vd12.ItemCgsRate = Conversion.ToDouble(item3.ItemCgsRate);
								vd12.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd12.Commission = Conversion.ToDouble(item3.CommissionAmount);
								vd12.GpNo = Conversion.ToInt(item3.GpNo);
								vd12.VehicleNo = item3.VehicleNo;
								vd12.JobLotId = Conversion.ToInt(item3.JobLotId);
								vd12.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vh.voucherDetailList.Add(vd12);
								continue;
							}
							throw new Exception("Item GlAccount Not found");
						}
						throw new Exception("Item GlAccount Not found");
					}
				}
			}
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
					objlot2 = jobLot.GetByID(item2.JobLotId);
				}
				List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
				if (find2.Count <= 0)
				{
					continue;
				}
				if (item2.FreightAmount > 0.0 && obj.DocumentTypeId != 138)
				{
					VoucherDetail vd17 = new VoucherDetail();
					if (objlot2 != null && objlot2.AccountId > 0)
					{
						vd17.AccountId = objlot2.AccountId;
					}
					else
					{
						vd17.AccountId = ((obj.DocumentTypeId == 98) ? Conversion.ToInt(find2[0].SaleGLAC) : Conversion.ToInt(find2[0].PurchaseGLAC));
						item2.PurchaseGLAC = vd17.AccountId;
					}
					vd17.AgainstAccountId = ((obj.TransportAccountId > 0) ? obj.TransportAccountId : ((transId > 0) ? transId : vd17.AccountId));
					vd17.DebitAmount = Conversion.ToDouble(item2.FreightAmount);
					vd17.CreditAmount = 0.0;
					if (AutoRemarksFormatII)
					{
						vd17.Comments = Remarks;
					}
					else
					{
						vd17.Comments = "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item2.NetBillWeight + " = " + Math.Round(item2.FreightAmount);
					}
					vd17.JobLotId = Conversion.ToInt(item2.JobLotId);
					vd17.ItemId = Conversion.ToInt(item2.ItemId);
					vd17.BranchesId = item2.BranchId;
					vh.voucherDetailList.Add(vd17);
				}
				if (obj.FreightAmount > 0.0 && obj.TransportAccountId > 0 && obj.DocumentTypeId == 56)
				{
					if (item2.FreightDeduction == 0.0)
					{
						throw new Exception("The proportionate freight deduction amount is zero. Please check.");
					}
					VoucherDetail vd16 = new VoucherDetail();
					vd16.AccountId = Conversion.ToInt(obj.TransportAccountId);
					vd16.AgainstAccountId = find2[0].PurchaseGLAC;
					vd16.DebitAmount = Conversion.ToDouble(item2.FreightDeduction);
					vd16.CreditAmount = 0.0;
					vd16.Comments = "Freight Deduction " + Remarks;
					vd16.JobLotId = Conversion.ToInt(item2.JobLotId);
					vd16.ItemId = Conversion.ToInt(item2.ItemId);
					vh.voucherDetailList.Add(vd16);
					VoucherDetail vd18 = new VoucherDetail();
					vd18.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					vd18.AgainstAccountId = obj.TransportAccountId;
					vd18.CreditAmount = Conversion.ToDouble(item2.FreightDeduction);
					vd18.DebitAmount = 0.0;
					vd18.Comments = "Freight Deduction " + Remarks;
					vd18.JobLotId = Conversion.ToInt(item2.JobLotId);
					vd18.ItemId = Conversion.ToInt(item2.ItemId);
					vh.voucherDetailList.Add(vd18);
				}
				if (item2.Brokery > 0.0 && obj.BrokeryAmount > 0.0)
				{
					VoucherDetail vd15 = new VoucherDetail();
					if (objlot2 != null && objlot2.AccountId > 0)
					{
						vd15.AccountId = objlot2.AccountId;
					}
					else
					{
						vd15.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					}
					vd15.AgainstAccountId = vd15.AccountId;
					vd15.DebitAmount = Conversion.ToDouble(item2.Brokery);
					vd15.CreditAmount = 0.0;
					if (AutoRemarksFormatII)
					{
						vd15.Comments = Remarks;
					}
					else
					{
						vd15.Comments = "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + obj.BrokeryAmount + " / " + TotalWeightInDetailList + " * " + item2.NetBillWeight + " = " + Math.Round(item2.Brokery);
					}
					vd15.JobLotId = Conversion.ToInt(item2.JobLotId);
					vd15.ItemId = Conversion.ToInt(item2.ItemId);
					vd15.BranchesId = item2.BranchId;
					vh.voucherDetailList.Add(vd15);
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
							objlot = jobLot.GetByID(item.JobLotId);
						}
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0 && item.FreightAmount > 0.0)
						{
							VoucherDetail vd14 = new VoucherDetail();
							if (objlot != null && objlot.AccountId > 0)
							{
								vd14.AccountId = objlot.AccountId;
							}
							else
							{
								vd14.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							}
							vd14.AgainstAccountId = ((obj.TransportAccountId > 0) ? obj.TransportAccountId : vd14.AccountId);
							vd14.DebitAmount = Conversion.ToDouble(item.FreightAmount);
							vd14.CreditAmount = 0.0;
							vd14.Comments = "SupplierName : " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + " Weight " + item.NetBillWeight + " Freight " + Math.Round(item.FreightAmount) + " VehicleNo " + item.VehicleNo + " GrnNo " + item.GrnNo;
							vd14.JobLotId = Conversion.ToInt(item.JobLotId);
							vd14.ItemId = Conversion.ToInt(item.ItemId);
							vd14.BranchesId = item.BranchId;
							vh.voucherDetailList.Add(vd14);
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
							vd2.JobLotId = Conversion.ToInt(item.JobLotId);
							vd2.ItemId = Conversion.ToInt(item.ItemId);
							vd2.BranchesId = item.BranchId;
							vh.voucherDetailList.Add(vd2);
						}
					}
				}
				if (obj.DeliveryTerm == "Ponch")
				{
					string freightRemarks = "SupplierName : " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName) + " Freight Inward = Rs. " + Math.Round(obj.FreightAmount) + " Vehicle # " + VehicleNo;
					VoucherDetail vd13 = new VoucherDetail();
					vd13.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd13.AgainstAccountId = obj.TransportAccountId;
					vd13.DebitAmount = Conversion.ToDouble(obj.FreightAmount);
					vd13.CreditAmount = 0.0;
					vd13.Comments = freightRemarks;
					vd13.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd13);
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

	public static VoucherHead ItemAndSupplierTradingFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		string Partyname = obj.CommonIdsForFinancials.CompanyName;
		if (new HashSet<int> { 58, 61, 64, 104, 131, 702, 245 }.Contains(obj.DocumentTypeId))
		{
			string Remarks = string.Empty;
			if (obj.invPurchaseInvoiceDetailList.Count > 0)
			{
				double TotalFreightAmountInTransporterGrid = 0.0;
				double TotalWeightInDetailList = 0.0;
				foreach (InvPurchaseInvoiceDetail item3 in obj.invPurchaseInvoiceDetailList.ToList())
				{
					if (item3.ActionTypeId == 3)
					{
						continue;
					}
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName w) => w.Id == Conversion.ToInt(item3.ItemId)).ToList();
					if (find3.Count > 0)
					{
						JobLot objlot2 = new JobLot();
						if (item3.JobLotId > 0)
						{
							objlot2 = jobLot.GetByID(item3.JobLotId);
						}
						VoucherDetail vd2 = new VoucherDetail();
						StringBuilder sb = new StringBuilder();
						string itemName = find3.FirstOrDefault()?.ItemName ?? string.Empty;
						string header = obj.RemarksHeader ?? string.Empty;
						switch (obj.DocumentTypeId)
						{
						case 702:
							sb.Append(header).Append(" ItemName: ").Append(itemName)
								.Append(" Rate: ")
								.Append(item3.ItemRate)
								.Append(" Qty: ")
								.Append(item3.ItemQty)
								.Append(" Weight: ")
								.Append(item3.NetBillWeight)
								.Append(" Item Amount: ")
								.Append(item3.ItemAmount);
							break;
						case 61:
						case 64:
						case 245:
							sb.Append(itemName).Append(" ").Append(item3?.JobLotDescription)
								.Append(" ")
								.Append(item3?.UOMCodeItem)
								.Append(" ")
								.Append(item3.NetBillWeight)
								.Append(" @")
								.Append(item3.ItemRate)
								.Append("/-");
							if (item3.GpNo > 0)
							{
								sb.Append(" GP# ").Append(item3.GpNo);
							}
							if (!string.IsNullOrWhiteSpace(item3.VehicleNo))
							{
								sb.Append(" V# ").Append(item3.VehicleNo);
							}
							if (!string.IsNullOrWhiteSpace(item3.RemarksDetail))
							{
								sb.Append(" ").Append(item3.RemarksDetail);
							}
							break;
						default:
							sb.Append(header).Append(" ItemName: ").Append(itemName)
								.Append(" Rate: ")
								.Append(item3.ItemRate)
								.Append(" Qty: ")
								.Append(item3.ItemQty)
								.Append(" Item Amount: ")
								.Append(item3.ItemAmount);
							break;
						}
						if (item3.FreightAmount > 0.0)
						{
							sb.Append(" Freight Amount: ").Append(Math.Round(item3.FreightAmount, 0));
						}
						Remarks = sb.ToString();
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd2.AccountId = objlot2.AccountId;
						}
						else
						{
							vd2.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						}
						vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.Comments = Conversion.ToString(Remarks);
						vd2.DebitAmount = Conversion.ToDouble(item3.ItemAmount);
						vd2.CreditAmount = Conversion.ToDouble(0);
						vd2.ItemId = Conversion.ToInt(item3.ItemId);
						vd2.JobLotId = Conversion.ToInt(item3.JobLotId);
						vd2.QtyIn = Conversion.ToDouble(item3.ItemQty);
						vd2.ItemRate = Conversion.ToDouble(item3.ItemRate);
						vd2.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
						vd2.RateCut = Conversion.ToDouble(item3.RateCut);
						vd2.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
						vd2.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
						vd2.Freight = Conversion.ToDouble(item3.FreightAmount);
						vd2.Commission = Conversion.ToDouble(item3.CommissionAmount);
						vd2.OrderNo = Conversion.ToInt(item3.PurchaseOrder);
						vd2.GpNo = Conversion.ToInt(item3.GpNo);
						vd2.VehicleNo = item3.VehicleNo;
						vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd2.BranchesId = item3.BranchId;
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
						if (objlot2 != null && objlot2.AccountId > 0)
						{
							vd4.AgainstAccountId = objlot2.AccountId;
						}
						else
						{
							vd4.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						}
						vd4.Comments = Conversion.ToString(Remarks);
						vd4.CreditAmount = Conversion.ToDouble(item3.ItemAmount);
						vd4.DebitAmount = Conversion.ToDouble(0);
						vd4.ItemId = Conversion.ToInt(item3.ItemId);
						vd4.JobLotId = Conversion.ToInt(item3.JobLotId);
						vd4.QtyIn = Conversion.ToDouble(item3.ItemQty);
						vd4.ItemRate = Conversion.ToDouble(item3.ItemRate);
						vd4.RateCut = Conversion.ToDouble(item3.RateCut);
						vd4.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
						vd4.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
						vd4.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
						vd4.Freight = Conversion.ToDouble(item3.FreightAmount);
						vd4.Commission = Conversion.ToDouble(item3.CommissionAmount);
						vd4.OrderNo = Conversion.ToInt(item3.PurchaseOrder);
						vd4.GpNo = Conversion.ToInt(item3.GpNo);
						vd4.VehicleNo = item3.VehicleNo;
						vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd4.SubsidiaryAccountId = obj.SupplierCustomerId;
						vd4.SubsidiaryTypeId = 1;
						vd4.BranchesId = item3.BranchId;
						vh.voucherDetailList.Add(vd4);
						TotalFreightAmountInTransporterGrid += item3.FreightAmount;
						TotalWeightInDetailList += item3.NetBillWeight;
						continue;
					}
					throw new Exception("Item Purchase GL Account Not found");
				}
				if (TotalFreightAmountInTransporterGrid > 0.0)
				{
					foreach (InvPurchaseInvoiceDetail item2 in obj.invPurchaseInvoiceDetailList.ToList())
					{
						if (item2.ActionTypeId != 3)
						{
							List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName w) => w.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find2.Count <= 0)
							{
								throw new Exception("Item Purchase GL Account Not found");
							}
							JobLot objlot = new JobLot();
							if (item2.JobLotId > 0)
							{
								objlot = jobLot.GetByID(item2.JobLotId);
							}
							VoucherDetail vd7 = new VoucherDetail();
							if (objlot != null && objlot.AccountId > 0)
							{
								vd7.AccountId = objlot.AccountId;
							}
							else
							{
								vd7.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							}
							vd7.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
							vd7.DebitAmount = Conversion.ToDouble(item2.FreightAmount);
							vd7.Comments = "ChargeToProduct/TotalWeightInDetail * Qty = Proportionated Charges : " + TotalFreightAmountInTransporterGrid + " / " + TotalWeightInDetailList + " * " + item2.ItemQty + " = " + Math.Round(item2.FreightAmount);
							vd7.CreditAmount = Conversion.ToDouble(0);
							vd7.ItemId = Conversion.ToInt(item2.ItemId);
							vd7.JobLotId = Conversion.ToInt(item2.JobLotId);
							vd7.BranchesId = item2.BranchId;
							vh.voucherDetailList.Add(vd7);
						}
					}
				}
				if (obj.InvPurchaseInvoiceExpList.Count > 0)
				{
					List<Architecture.Model.Inventory.InventoryItemsOther> dtItemOther = CommonServies.GetOtherItemForFinancialEffects(obj.OrganizationId, obj.CompanyId);
					foreach (InvPurchaseInvoiceExpense item in obj.InvPurchaseInvoiceExpList.ToList())
					{
						List<Architecture.Model.Inventory.InventoryItemsOther> find = dtItemOther.Where((Architecture.Model.Inventory.InventoryItemsOther w) => w.Id == Conversion.ToInt(item.InvRevExpItemId)).ToList();
						if (find.Count > 0)
						{
							Architecture.Model.Inventory.InventoryItemsOther record = find?.FirstOrDefault() ?? throw new Exception("Item mapping not found.");
							item.OtherItemName = record.OtherItemName;
							if (record.StockGLAcId <= 0)
							{
								throw new Exception("GL Account mapping missing for '" + record.OtherItemName + "'.");
							}
							int otherItemGlId = record.StockGLAcId;
							List<string> parts = new List<string>();
							if (!string.IsNullOrWhiteSpace(item.Remarks))
							{
								parts.Add(item.Remarks.Trim());
							}
							if (!string.IsNullOrWhiteSpace(item.OtherItemName))
							{
								parts.Add(item.OtherItemName.Trim());
							}
							if (item.Qty > 0.0)
							{
								parts.Add($"Qty {item.Qty}");
							}
							if (item.Rate > 0.0)
							{
								parts.Add($"Rate {item.Rate}");
							}
							string remarks = string.Join(" ", parts);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.AccountId = otherItemGlId;
							vd6.AgainstAccountId = vh.RefAccountId;
							vd6.DebitAmount = Conversion.ToDouble(item.Amount);
							vd6.Comments = remarks;
							vd6.BranchesId = obj.BranchesId;
							vh.voucherDetailList.Add(vd6);
							VoucherDetail vd8 = new VoucherDetail();
							vd8.AccountId = vh.RefAccountId;
							vd8.AgainstAccountId = otherItemGlId;
							vd8.CreditAmount = Conversion.ToDouble(item.Amount);
							vd8.Comments = remarks;
							vd8.BranchesId = obj.BranchesId;
							vh.voucherDetailList.Add(vd8);
							continue;
						}
						throw new Exception("Other Item Purchase GL Account Not found");
					}
				}
				if (obj.DiscountAccountId > 0 && obj.DiscountAmount > 0.0)
				{
					double discount = Convert.ToDouble(obj.DiscountAmount);
					string partyName = Partyname ?? "";
					string invoiceNo = obj.DocNo.ToString() ?? "";
					string commonRemark = $"Discount {discount:0.##}";
					VoucherDetail voucherDetail = new VoucherDetail();
					voucherDetail.AccountId = vh.RefAccountId;
					voucherDetail.AgainstAccountId = obj.DiscountAccountId;
					voucherDetail.Comments = commonRemark + " received from " + partyName + " (Inv: " + invoiceNo + ")";
					voucherDetail.DebitAmount = discount;
					voucherDetail.BranchesId = obj.BranchesId;
					VoucherDetail vd3 = voucherDetail;
					vh.voucherDetailList.Add(vd3);
					VoucherDetail vd5 = new VoucherDetail
					{
						AccountId = obj.DiscountAccountId,
						AgainstAccountId = vh.RefAccountId,
						Comments = commonRemark + " allowed (Inv: " + invoiceNo + ")",
						CreditAmount = discount,
						SupplierCustomerId = Convert.ToInt32(obj.SupplierCustomerId),
						SubsidiaryAccountId = obj.SupplierCustomerId,
						SubsidiaryTypeId = 1,
						BranchesId = obj.BranchesId
					};
					vh.voucherDetailList.Add(vd5);
				}
			}
		}
		return vh;
	}

	public static VoucherHead PurchaseReturnFinancial(Architecture.Model.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
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
						objlot2 = jobLot.GetByID(item2.JobLotId);
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
						vd6.ItemId = Conversion.ToInt(item2.ItemId);
						vd6.JobLotId = Conversion.ToInt(item2.JobLotId);
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
						vd4.ItemId = Conversion.ToInt(item2.ItemId);
						vd4.JobLotId = Conversion.ToInt(item2.JobLotId);
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
						vd5.ItemId = Conversion.ToInt(item2.ItemId);
						vd5.JobLotId = Conversion.ToInt(item2.JobLotId);
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
						vd7.ItemId = Conversion.ToInt(item2.ItemId);
						vd7.JobLotId = Conversion.ToInt(item2.JobLotId);
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
					objlot = jobLot.GetByID(item.JobLotId);
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
					vd8.ItemId = Conversion.ToInt(item.ItemId);
					vd8.JobLotId = Conversion.ToInt(item.JobLotId);
					vd8.BranchesId = item.BranchId;
					vh.voucherDetailList.Add(vd8);
				}
			}
		}
		return vh;
	}
}
