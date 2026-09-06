using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Newtonsoft.Json;

namespace Architecture.BLL.Inventory.Financials.SaleInvoice;

public class SaleInvoiceGeneralFinancialMethods
{
	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		VoucherHead vh = new VoucherHead
		{
			RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId,
			AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId
		};
		if (new HashSet<int>
		{
			95, 96, 99, 103, 126, 133, 171, 208, 1608, 1609,
			1661, 1660, 1611, 1662, 1809, 145, 222
		}.Contains(obj.DocumentTypeId) && obj.InvSaleInvoiceJournalList.Count > 0)
		{
			foreach (InvSaleInvoiceJournal item in obj.InvSaleInvoiceJournalList)
			{
				int chartAccountId = Conversion.ToInt(item.ChartofAccountId);
				if (chartAccountId != 0)
				{
					VoucherDetail vd = new VoucherDetail
					{
						AccountId = chartAccountId,
						AgainstAccountId = Conversion.ToInt(vh.RefAccountId),
						Comments = Conversion.ToString(item.JvRemarks),
						DebitAmount = Conversion.ToDouble(item.JvDebit),
						CreditAmount = Conversion.ToDouble(item.JvCredit),
						SubsidiaryTypeId = 1,
						SubsidiaryAccountId = Conversion.ToInt(item.TransporterSupCustId),
						SupplierCustomerId = Conversion.ToInt(item.TransporterSupCustId),
						SubsidiaryAgainstTypeId = 1,
						SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId),
						BranchesId = obj.BranchesId
					};
					vh.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail
					{
						AccountId = Conversion.ToInt(vh.RefAccountId),
						AgainstAccountId = chartAccountId,
						Comments = Conversion.ToString(item.JvRemarks),
						CreditAmount = Conversion.ToDouble(item.JvDebit),
						DebitAmount = Conversion.ToDouble(item.JvCredit),
						SubsidiaryTypeId = 1,
						SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId),
						SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId),
						SubsidiaryAgainstTypeId = 1,
						SubsidiaryAgainstAccountId = Conversion.ToInt(item.TransporterSupCustId),
						BranchesId = obj.BranchesId,
						CostCenterId = obj.CommonIdsForFinancials.CostCenterId
					};
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		return vh;
	}

	public static VoucherHead SalesTaxFinancial(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (new HashSet<int>
		{
			96, 103, 126, 133, 1608, 1609, 1661, 1660, 1611, 145,
			1662
		}.Contains(obj.DocumentTypeId) && obj.InvSaleInvoiceDetaillist.Count > 0)
		{
			double TaxTotalAmount = 0.0;
			foreach (InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist.ToList())
			{
				TaxTotalAmount += item.TaxAmount;
			}
			if (TaxTotalAmount > 0.0)
			{
				if (obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 1662)
				{
					VoucherDetail vd1 = new VoucherDetail();
					vd1.AccountId = obj.TaxAccountId;
					vd1.AgainstAccountId = vh.RefAccountId;
					vd1.Comments = "Tax %  " + TaxTotalAmount;
					vd1.DebitAmount = TaxTotalAmount;
					vd1.TaxesTotalAmount = TaxTotalAmount;
					vd1.IsTaxable = "True";
					vd1.TaxTypeId = 2;
					vd1.SubsidiaryAgainstTypeId = 1;
					vd1.SubsidiaryAgainstAccountId = obj.SupplierCustomerId;
					vd1.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd1);
					VoucherDetail vd3 = new VoucherDetail();
					vd3.AccountId = vh.RefAccountId;
					vd3.AgainstAccountId = obj.TaxAccountId;
					vd3.Comments = "Tax %  " + TaxTotalAmount;
					vd3.CreditAmount = TaxTotalAmount;
					vd3.TaxesTotalAmount = TaxTotalAmount;
					vd3.TaxPrcnt = 17.0;
					vd3.IsTaxable = "True";
					vd3.TaxTypeId = 2;
					vd3.SubsidiaryTypeId = 1;
					vd3.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vd3.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd3);
				}
				else
				{
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = vh.RefAccountId;
					vd2.Comments = "Tax " + TaxTotalAmount;
					if (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660 || obj.DocumentTypeId == 145)
					{
						vd2.AgainstAccountId = obj.TaxAccountId;
					}
					else
					{
						vd2.AgainstAccountId = obj.ReferencePartyId;
					}
					vd2.DebitAmount = TaxTotalAmount;
					vd2.TaxesTotalAmount = TaxTotalAmount;
					vd2.IsTaxable = "True";
					vd2.TaxTypeId = 2;
					vd2.SubsidiaryTypeId = 1;
					vd2.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vd2.SupplierCustomerId = obj.SupplierCustomerId;
					vd2.BranchesId = obj.BranchesId;
					vd2.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
					vh.voucherDetailList.Add(vd2);
					VoucherDetail vd4 = new VoucherDetail();
					if (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660 || obj.DocumentTypeId == 145)
					{
						vd4.AccountId = obj.TaxAccountId;
					}
					else
					{
						vd4.AccountId = obj.ReferencePartyId;
					}
					vd4.Comments = "Tax " + TaxTotalAmount;
					vd4.AgainstAccountId = vh.RefAccountId;
					vd4.CreditAmount = TaxTotalAmount;
					vd4.TaxesTotalAmount = TaxTotalAmount;
					vd4.IsTaxable = "True";
					vd4.TaxTypeId = 2;
					vd4.SubsidiaryAgainstTypeId = 1;
					vd4.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vd4.BranchesId = obj.BranchesId;
					vh.voucherDetailList.Add(vd4);
				}
			}
		}
		return vh;
	}

	public static VoucherHead FreightFinancial(Architecture.Model.Inventory.InvSaleInvoice obj)
	{
		VoucherHead vh = new VoucherHead
		{
			RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId,
			AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId
		};
		if (new HashSet<int>
		{
			95, 96, 103, 126, 133, 171, 1608, 1609, 1661, 1660,
			1611, 145, 99, 222
		}.Contains(obj.DocumentTypeId))
		{
			List<InvSaleInvoiceFreight> invSaleInvoiceFreightList = obj.InvSaleInvoiceFreightList;
			if (invSaleInvoiceFreightList != null && invSaleInvoiceFreightList.Count > 0)
			{
				foreach (InvSaleInvoiceFreight r in obj.InvSaleInvoiceFreightList)
				{
					if (r.TansporterId > 0)
					{
						vh.voucherDetailList.Add(new VoucherDetail
						{
							AccountId = r.TansporterId,
							AgainstAccountId = ((obj.CommonIdsForFinancials.SaleGLAccountId > 0) ? obj.CommonIdsForFinancials.SaleGLAccountId : r.TansporterId),
							Comments = ((!string.IsNullOrWhiteSpace(r.Remarks)) ? r.Remarks : "Freight"),
							DebitAmount = Conversion.ToDouble(r.Debit),
							CreditAmount = Conversion.ToDouble(r.FreightAmount),
							SubsidiaryTypeId = 1,
							SubsidiaryAccountId = Conversion.ToInt((r.TansporterId == vh.RefAccountId) ? obj.SupplierCustomerId : r.TransporterSupCustId),
							SupplierCustomerId = Conversion.ToInt((r.TansporterId == vh.RefAccountId) ? obj.SupplierCustomerId : r.TransporterSupCustId),
							CostCenterId = Conversion.ToInt((r.TansporterId == vh.RefAccountId) ? obj.CommonIdsForFinancials.CostCenterId : 0),
							BranchesId = obj.BranchesId
						});
					}
				}
			}
		}
		if (obj.FreightAmount > 0.0)
		{
			if (obj.DocumentTypeId == 99 || obj.DocumentTypeId == 222)
			{
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = Conversion.ToInt(obj.TransporterId),
					AgainstAccountId = Conversion.ToInt(vh.RefAccountId),
					Comments = "Freight",
					DebitAmount = Conversion.ToDouble(obj.FreightAmount),
					CreditAmount = 0.0,
					SubsidiaryTypeId = 1,
					SubsidiaryAccountId = Conversion.ToInt((obj.TransporterId == vh.RefAccountId) ? obj.SupplierCustomerId : obj.TransporterCreditPartyId),
					SupplierCustomerId = Conversion.ToInt((obj.TransporterId == vh.RefAccountId) ? obj.SupplierCustomerId : obj.TransporterCreditPartyId),
					SubsidiaryAgainstTypeId = 1,
					SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId),
					CostCenterId = Conversion.ToInt((obj.TransporterId == vh.RefAccountId) ? obj.CommonIdsForFinancials.CostCenterId : 0),
					BranchesId = obj.BranchesId
				});
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = Conversion.ToInt(vh.RefAccountId),
					AgainstAccountId = Conversion.ToInt(obj.TransporterId),
					Comments = "Freight",
					DebitAmount = 0.0,
					CreditAmount = Conversion.ToDouble(obj.FreightAmount),
					SubsidiaryTypeId = 1,
					SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId),
					SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId),
					SubsidiaryAgainstTypeId = 1,
					SubsidiaryAgainstAccountId = Conversion.ToInt(obj.TransporterCreditPartyId),
					BranchesId = obj.BranchesId
				});
			}
			if (obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660 || obj.DocumentTypeId == 1662)
			{
				if (obj.TransporterDebitGLId > 0)
				{
					vh.voucherDetailList.Add(new VoucherDetail
					{
						AccountId = Conversion.ToInt(obj.TransporterDebitGLId),
						AgainstAccountId = Conversion.ToInt(obj.TransporterId),
						Comments = ((!string.IsNullOrWhiteSpace(obj.FreightRemark)) ? obj.FreightRemark : "Freight"),
						DebitAmount = Conversion.ToDouble(obj.FreightAmount),
						SubsidiaryTypeId = 1,
						SubsidiaryAccountId = Conversion.ToInt(obj.TransporterDebitPartyId),
						SupplierCustomerId = Conversion.ToInt(obj.TransporterDebitPartyId),
						SubsidiaryAgainstTypeId = 1,
						SubsidiaryAgainstAccountId = Conversion.ToInt(obj.TransporterCreditPartyId),
						BranchesId = obj.BranchesId
					});
				}
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = Conversion.ToInt(obj.TransporterId),
					AgainstAccountId = Conversion.ToInt(obj.TransporterDebitGLId),
					Comments = ((!string.IsNullOrWhiteSpace(obj.FreightRemark)) ? obj.FreightRemark : "Freight"),
					DebitAmount = 0.0,
					CreditAmount = Conversion.ToDouble(obj.FreightAmount),
					SubsidiaryTypeId = 1,
					SubsidiaryAccountId = Conversion.ToInt(obj.TransporterCreditPartyId),
					SupplierCustomerId = Conversion.ToInt(obj.TransporterCreditPartyId),
					SubsidiaryAgainstTypeId = 1,
					SubsidiaryAgainstAccountId = Conversion.ToInt(obj.TransporterDebitPartyId),
					BranchesId = obj.BranchesId
				});
			}
		}
		return vh;
	}

	public static VoucherHead ItemAndCustomerByWeightFinancial(Architecture.Model.Inventory.InvSaleInvoice obj, List<GetItemGlIdsandItemName> dtitem, List<SupplierCustomerGlAccountId> dtSupplier)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		bool DebitAmountChargetoExpenseAcOfCommission = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "DebitAmountChargetoExpenseAcOfCommission"));
		int DiscountAllowed = Conversion.ToInt(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "DiscountAllowed"));
		bool Foc = false;
		bool Zakat = false;
		if (obj.DocumentTypeId == 99)
		{
			Foc = obj.OtherCategoryId == 55;
			Zakat = obj.OtherCategoryId == 56;
		}
		HashSet<int> validDocumentTypes = new HashSet<int>
		{
			95, 96, 99, 103, 60, 126, 128, 133, 139, 171,
			208, 1608, 1609, 1611, 184, 186, 1809, 145, 222, 1661,
			1660, 1662
		};
		HashSet<int> returnDocumentTypeIds = new HashSet<int> { 1611, 1662 };
		bool PmTrans = new int[3] { 103, 126, 133 }.Contains(obj.DocumentTypeId);
		if (validDocumentTypes.Contains(obj.DocumentTypeId) && obj.InvSaleInvoiceDetaillist.Count > 0)
		{
			List<InvSaleInvoiceDetail> itemListWithUniqueItems = new List<InvSaleInvoiceDetail>();
			List<InvSaleInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<InvSaleInvoiceDetail>>(JsonConvert.SerializeObject(obj.InvSaleInvoiceDetaillist.ToList()));
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
					if (itemListWithUniqueItems[l].ItemId == dummyArray[j].ItemId && itemListWithUniqueItems[l]?.BranchId == dummyArray[j]?.BranchId)
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
			for (int i = 0; i < itemListWithUniqueItems.ToList().Count; i++)
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
				for (int k = 0; k < dummyArray.Count; k++)
				{
					if (itemListWithUniqueItems[i].ItemId == dummyArray[k].ItemId && itemListWithUniqueItems[i]?.BranchId == dummyArray[k]?.BranchId)
					{
						itemAmountAtI += dummyArray[k].ItemAmount;
						itemFreightAmountAtI += dummyArray[k].FreightAmount;
						itemExpenseAmountAtI += dummyArray[k].ExpenseAmount;
						itemCommissionAmountAtI += dummyArray[k].CommissionAmount;
						itemQtyAtI += dummyArray[k].ItemQty;
						itemWeightAtI += dummyArray[k].NetBillWeight;
						itemWeightCutAtI += dummyArray[k].WeightCut;
						itemRateCutAtI += dummyArray[k].RateCut;
						itemEmptyBagsDudectionAtI += dummyArray[k].EBTotalWt;
						itemGrossWeightAtI += dummyArray[k].GrossWeight;
					}
				}
				itemListWithUniqueItems[i].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[i].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[i].FreightAmount = itemFreightAmountAtI;
				itemListWithUniqueItems[i].CommissionAmount = itemCommissionAmountAtI;
				itemListWithUniqueItems[i].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[i].NetBillWeight = itemWeightAtI;
				itemListWithUniqueItems[i].WeightCut = itemWeightCutAtI;
				itemListWithUniqueItems[i].RateCut = itemRateCutAtI;
				itemListWithUniqueItems[i].EBTotalWt = itemEmptyBagsDudectionAtI;
				itemListWithUniqueItems[i].GrossWeight = itemGrossWeightAtI;
			}
			string InvoiceHeaderRemarksAuto = "";
			int getOrderNo = 0;
			DateTime getOrderDate = DateTime.Now;
			int RowNo = 1;
			foreach (InvSaleInvoiceDetail item5 in itemListWithUniqueItems)
			{
				if (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660)
				{
					List<GetItemGlIdsandItemName> find4 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item5.ItemId)).ToList();
					InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + Conversion.ToString(find4[0].ItemName) + "  Qty " + Conversion.ToString(item5.ItemQty) + "  ";
				}
				_ = item5.ItemId;
				getOrderNo = item5.SaleOrder;
				getOrderDate = item5.OrderDate;
				_ = item5.GpNo;
				_ = item5.VehicleNo;
				RowNo++;
			}
			if (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660)
			{
				obj.OtherRemarks = obj.RemarksHeader + " " + InvoiceHeaderRemarksAuto;
				if (getOrderNo > 0)
				{
					obj.OtherRemarks = obj.OtherRemarks + " ContractNo " + getOrderNo + "  " + Conversion.ToDateTime(getOrderDate).ToString("dd-MMM-yy");
				}
			}
			int transId = 0;
			if (((obj.InvSaleInvoiceFreightList != null) ? obj.InvSaleInvoiceFreightList.Sum((InvSaleInvoiceFreight x) => x.FreightAmount) : 0.0) > 0.0 && obj.InvSaleInvoiceFreightList.Select((InvSaleInvoiceFreight x) => x.TansporterId).Distinct().Count() == 1)
			{
				transId = obj.InvSaleInvoiceFreightList.Max((InvSaleInvoiceFreight x) => x.TansporterId);
			}
			bool LoaderFlag = false;
			double StockCreditAmount = 0.0;
			foreach (InvSaleInvoiceDetail item4 in obj.InvSaleInvoiceDetaillist.ToList())
			{
				if (item4.RefRefDocumentTypeId > 0 && item4.RefRefDocIdNo > 0 && item4.RefDocSubId > 0)
				{
					LoaderFlag = true;
				}
				string Remarks = string.Empty;
				double Mond2 = Conversion.ToDouble(item4.NetBillWeight) / 40.0;
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
					if (find3.Count > 0)
					{
						JobLot objlot2 = new JobLot();
						if (item4.JobLotId > 0)
						{
							objlot2 = jobLot.GetByID(item4.JobLotId);
						}
						List<string> parts4 = new List<string>();
						string ItemName = find3[0].ItemName;
						if (new int[5] { 60, 128, 184, 186, 145 }.Contains(obj.DocumentTypeId))
						{
							if (obj.DocumentTypeId == 186 && !string.IsNullOrWhiteSpace(obj.ManualBillNo))
							{
								parts4.Add(obj.ManualBillNo);
							}
							if (!string.IsNullOrWhiteSpace(obj.RemarksHeader))
							{
								parts4.Add(obj.RemarksHeader);
							}
							parts4.Add("Item: " + ItemName);
							parts4.Add($"Qty: {item4.ItemQty}");
							parts4.Add($"Rate: {item4.ItemRate}");
							if (item4.ItemDiscountAmount > 0.0)
							{
								parts4.Add($"Discount: {item4.ItemDiscountAmount}");
							}
							if (item4.ExpenseAmount > 0.0)
							{
								parts4.Add($"Expense: {item4.ExpenseAmount}");
							}
							if (item4.CommissionAmount > 0.0)
							{
								parts4.Add($"Commission: {item4.CommissionAmount}");
							}
						}
						else if (new int[3] { 103, 126, 133 }.Contains(obj.DocumentTypeId))
						{
							if (!string.IsNullOrWhiteSpace(obj.RemarksHeader))
							{
								parts4.Add(obj.RemarksHeader);
							}
							parts4.Add("Item: " + ItemName);
							parts4.Add($"Qty: {item4.ItemQty}");
							parts4.Add($"Rate: {item4.ItemRate}");
							if (item4.FreightAmount > 0.0)
							{
								parts4.Add($"Freight: {item4.FreightAmount}");
							}
							if (item4.CommissionAmount > 0.0)
							{
								parts4.Add($"Commission: {item4.CommissionAmount}");
							}
						}
						else if (new int[5] { 1608, 1609, 1611, 1661, 1660 }.Contains(obj.DocumentTypeId))
						{
							if (item4.SaleOrder > 0)
							{
								parts4.Add($"Contract No. {item4.SaleOrder} {item4.OrderDate:dd-MMM-yyyy}");
							}
							parts4.Add($"{ItemName} Qty {item4.ItemQty} @Rate {item4.ItemRate}");
							if (item4.ItemDiscount > 0.0 && item4.ItemDiscountAmount > 0.0)
							{
								parts4.Add($"Discount {Math.Round(item4.ItemDiscountAmount)}");
							}
							parts4.Add($"NetAmount {item4.ItemAmount}");
							if (obj.PaymentTermId == 2)
							{
								parts4.Add($"Term {obj.DueDays} days Credit DueDate {obj.DueDate:dd-MMM-yyyy}");
							}
							else if (obj.PaymentTermId == 1)
							{
								parts4.Add("Term Cash");
							}
							if (item4.GpNo != 0)
							{
								parts4.Add($"Gp # {item4.GpNo}");
							}
							if (!string.IsNullOrWhiteSpace(item4.VehicleNo) && item4.VehicleNo != "0")
							{
								parts4.Add("Vehicle # " + item4.VehicleNo);
							}
							if (!string.IsNullOrWhiteSpace(item4.VariantDescription) && item4.VariantDescription != "0")
							{
								parts4.Add("Modal Description # " + item4.VariantDescription);
							}
						}
						else
						{
							if (!string.IsNullOrWhiteSpace(obj.RemarksHeader))
							{
								parts4.Add(obj.RemarksHeader);
							}
							string weightPart = $"Sale {item4.ItemQty} Bags ({item4.GrossWeight}";
							if (item4.EBTotalWt > 0.0)
							{
								weightPart += $", {Math.Round(item4.EBTotalWt, 3)}";
							}
							weightPart += $", {item4.NetBillWeight})";
							parts4.Add(weightPart);
							parts4.Add("of " + ItemName);
							parts4.Add($"{Math.Round(Mond2, 2)} Mound");
							parts4.Add($"@ NetRate {item4.ItemRate}");
							parts4.Add($"= Rs. {item4.ItemAmount}");
							if (item4.ItemDiscount > 0.0 && item4.ItemDiscountAmount > 0.0)
							{
								parts4.Add($"Discount% {item4.ItemDiscount} Amount {Math.Round(item4.ItemDiscountAmount)}");
							}
							if (item4.RateCut != 0.0)
							{
								parts4.Add($"RateCut {item4.RateCut} Amount {Math.Round(item4.RateCutAmount)}");
							}
							if (item4.CommissionAmount > 0.0)
							{
								parts4.Add($"Commission @ {obj.CommRate} = Rs. {Math.Round(item4.CommissionAmount)}");
							}
							if (item4.FreightAmount > 0.0)
							{
								parts4.Add($"Freight = Rs. {Math.Round(item4.FreightAmount)}");
							}
							if (!string.IsNullOrWhiteSpace(item4.VehicleNo) && item4.VehicleNo != "0")
							{
								parts4.Add("Vehicle # " + item4.VehicleNo);
							}
							if (item4.GpNo != 0)
							{
								parts4.Add($"Gp # {item4.GpNo}");
							}
							if (item4.ExpenseAmount > 0.0)
							{
								parts4.Add($"Expense = Rs. {Math.Round(item4.ExpenseAmount)}");
							}
							if (item4.SaleOrder > 0)
							{
								parts4.Add($"Contract # {item4.SaleOrder}");
							}
							if (!string.IsNullOrWhiteSpace(obj.ManualBillNo))
							{
								parts4.Add("Manual # " + obj.ManualBillNo);
							}
							if (!string.IsNullOrWhiteSpace(item4.WareHouseName))
							{
								parts4.Add("Warehouse " + item4.WareHouseName);
							}
							if (!string.IsNullOrWhiteSpace(item4.VariantDescription) && item4.VariantDescription != "0")
							{
								parts4.Add("Modal Description # " + item4.VariantDescription);
							}
							parts4.Add("From " + obj.CommonIdsForFinancials.CompanyName);
						}
						Remarks = string.Join(", ", parts4);
						if (!returnDocumentTypeIds.Contains(obj.DocumentTypeId))
						{
							if (!Foc && !Zakat)
							{
								VoucherDetail vd18 = new VoucherDetail();
								vd18.LineId = 0;
								vd18.AccountId = Conversion.ToInt(vh.RefAccountId);
								if (obj.DocumentTypeId == 60 || obj.DocumentTypeId == 145)
								{
									vd18.LineId = item4.LineId;
									vd18.AgainstAccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find3[0].PurchaseGLAC));
								}
								else if (obj.DocumentTypeId == 222)
								{
									vd18.AgainstAccountId = obj.CommonIdsForFinancials.StockPartyGlAccountId;
								}
								else
								{
									vd18.AgainstAccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find3[0].SaleGLAC));
								}
								vd18.Comments = Conversion.ToString(Remarks);
								vd18.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
								vd18.ItemId = Conversion.ToInt(item4.ItemId);
								vd18.QtyOut = Conversion.ToDouble(item4.ItemQty);
								vd18.ItemRate = Conversion.ToDouble(item4.ItemRate);
								vd18.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
								vd18.RateCut = Conversion.ToDouble(item4.RateCut);
								vd18.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
								vd18.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
								vd18.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
								vd18.Freight = Conversion.ToDouble(item4.FreightAmount);
								vd18.Commission = Conversion.ToDouble(item4.CommissionAmount);
								vd18.OrderNo = Conversion.ToInt(item4.SaleOrder);
								vd18.GpNo = Conversion.ToInt(item4.GpNo);
								vd18.JobLotId = Conversion.ToInt(item4.JobLotId);
								vd18.VehicleNo = item4.VehicleNo;
								vd18.DMultiCurrencyId = item4.CurrencyId;
								vd18.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
								vd18.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
								vd18.SubsidiaryTypeId = 1;
								vd18.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd18.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd18.BranchesId = item4.BranchId;
								vd18.CostCenterId = item4.CostCenterId;
								vh.voucherDetailList.Add(vd18);
								VoucherDetail vd17 = new VoucherDetail();
								vd17.LineId = 0;
								if (obj.DocumentTypeId == 60 || obj.DocumentTypeId == 145)
								{
									vd17.LineId = item4.LineId;
									vd17.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find3[0].PurchaseGLAC));
								}
								else if (obj.DocumentTypeId == 222)
								{
									vd17.AccountId = obj.CommonIdsForFinancials.StockPartyGlAccountId;
								}
								else
								{
									vd17.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find3[0].SaleGLAC));
								}
								vd17.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd17.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd17.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
								vd17.ItemId = Conversion.ToInt(item4.ItemId);
								vd17.QtyOut = Conversion.ToDouble(item4.ItemQty);
								vd17.ItemRate = Conversion.ToDouble(item4.ItemRate);
								vd17.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
								vd17.RateCut = Conversion.ToDouble(item4.RateCut);
								vd17.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
								vd17.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
								vd17.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
								vd17.Commission = Conversion.ToDouble(item4.CommissionAmount);
								vd17.Freight = Conversion.ToDouble(item4.FreightAmount);
								vd17.OrderNo = Conversion.ToInt(item4.SaleOrder);
								vd17.GpNo = Conversion.ToInt(item4.GpNo);
								vd17.VehicleNo = item4.VehicleNo;
								vd17.JobLotId = Conversion.ToInt(item4.JobLotId);
								vd17.SubsidiaryAgainstTypeId = 1;
								vd17.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd17.DMultiCurrencyId = item4.CurrencyId;
								vd17.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
								vd17.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
								vd17.BranchesId = item4.BranchId;
								vd17.CostCenterId = item4.CostCenterId;
								vh.voucherDetailList.Add(vd17);
								StockCreditAmount = vd17.CreditAmount;
							}
							if (obj.DocumentTypeId == 222 && obj.TransTypeId == 2)
							{
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find3[0].PurchaseGLAC), Conversion.ToInt(find3[0].SaleGLAC), item4.ItemAmount, null, isQtyOut: false, item4, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find3[0].SaleGLAC), Conversion.ToInt(find3[0].PurchaseGLAC), null, item4.ItemAmount, isQtyOut: false, item4, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find3[0].COGSGLAC), Conversion.ToInt(find3[0].PurchaseGLAC), item4.ItemAmount, null, isQtyOut: true, item4, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find3[0].PurchaseGLAC), Conversion.ToInt(find3[0].COGSGLAC), null, item4.ItemAmount, isQtyOut: true, item4, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
							}
						}
						if (returnDocumentTypeIds.Contains(obj.DocumentTypeId))
						{
							VoucherDetail vd32 = new VoucherDetail();
							vd32.LineId = 0;
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd32.AccountId = objlot2.AccountId;
							}
							else
							{
								vd32.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							vd32.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd32.Comments = Conversion.ToString(Remarks);
							vd32.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
							vd32.ItemId = Conversion.ToInt(item4.ItemId);
							vd32.QtyIn = Conversion.ToDouble(item4.ItemQty);
							vd32.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd32.WeightIn = Conversion.ToDouble(item4.NetBillWeight);
							vd32.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd32.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd32.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd32.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd32.OrderNo = Conversion.ToInt(item4.SaleOrder);
							vd32.GpNo = Conversion.ToInt(item4.GpNo);
							vd32.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd32.VehicleNo = item4.VehicleNo;
							vd32.DMultiCurrencyId = item4.CurrencyId;
							vd32.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd32.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd32.SubsidiaryAgainstTypeId = 1;
							vd32.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd32.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd32);
							VoucherDetail vd16 = new VoucherDetail();
							vd16.LineId = 0;
							vd16.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd16.AgainstAccountId = objlot2.AccountId;
							}
							else
							{
								vd16.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							vd16.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd16.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
							vd16.ItemId = Conversion.ToInt(item4.ItemId);
							vd16.QtyIn = Conversion.ToDouble(item4.ItemQty);
							vd16.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd16.WeightIn = Conversion.ToDouble(item4.NetBillWeight);
							vd16.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd16.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd16.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd16.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd16.OrderNo = Conversion.ToInt(item4.SaleOrder);
							vd16.GpNo = Conversion.ToInt(item4.GpNo);
							vd16.VehicleNo = item4.VehicleNo;
							vd16.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd16.DMultiCurrencyId = item4.CurrencyId;
							vd16.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd16.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd16.SubsidiaryTypeId = 1;
							vd16.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd16.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd16.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd16);
						}
						if (item4.ItemDiscountAmount > 0.0 && obj.DocumentTypeId != 1608 && obj.DocumentTypeId != 1609 && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1661 && obj.DocumentTypeId != 1662 && obj.DocumentTypeId != 1660 && obj.DocumentTypeId != 184 && obj.DocumentTypeId != 186)
						{
							if (DiscountAllowed == 0)
							{
								throw new Exception("Discount Allowed Account Not Found");
							}
							VoucherDetail vd13 = new VoucherDetail();
							vd13.LineId = 0;
							vd13.AccountId = Conversion.ToInt(DiscountAllowed);
							vd13.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd13.Comments = Conversion.ToString(Remarks);
							vd13.DebitAmount = Conversion.ToDouble(item4.ItemAmount);
							vd13.ItemId = Conversion.ToInt(item4.ItemId);
							vd13.QtyOut = Conversion.ToDouble(item4.ItemQty);
							vd13.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd13.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
							vd13.RateCut = Conversion.ToDouble(item4.RateCut);
							vd13.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
							vd13.ItemAmount = Conversion.ToDouble(item4.ItemAmount);
							vd13.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
							vd13.Freight = Conversion.ToDouble(item4.FreightAmount);
							vd13.Commission = Conversion.ToDouble(item4.CommissionAmount);
							vd13.OrderNo = Conversion.ToInt(item4.SaleOrder);
							vd13.GpNo = Conversion.ToInt(item4.GpNo);
							vd13.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd13.VehicleNo = item4.VehicleNo;
							vd13.DMultiCurrencyId = item4.CurrencyId;
							vd13.DExchangeCurrencyRate = Conversion.ToDouble(item4.ExchangeRate);
							vd13.DCurrencyAmount = Conversion.ToDouble(item4.FcyAmount);
							vd13.SubsidiaryAgainstTypeId = 1;
							vd13.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd13.BranchesId = item4.BranchId;
							vd13.CostCenterId = item4.CostCenterId;
							vh.voucherDetailList.Add(vd13);
							VoucherDetail vd15 = new VoucherDetail();
							vd15.LineId = 0;
							vd15.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd15.AgainstAccountId = Conversion.ToInt(DiscountAllowed);
							vd15.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd15.CreditAmount = Conversion.ToDouble(item4.ItemAmount);
							vd15.ItemId = Conversion.ToInt(item4.ItemId);
							vd15.QtyOut = Conversion.ToDouble(item4.ItemQty);
							vd15.ItemRate = Conversion.ToDouble(item4.ItemRate);
							vd15.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
							vd15.RateCut = Conversion.ToDouble(item4.RateCut);
							vd15.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
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
							vd15.BranchesId = item4.BranchId;
							vd15.CostCenterId = item4.CostCenterId;
							vh.voucherDetailList.Add(vd15);
						}
						if (obj.DocumentTypeId != 60 && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1662 && obj.DocumentTypeId != 222)
						{
							bool InventoryFinancialsEffectsInActive = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive"));
							if (objlot2 != null && objlot2.AccountId == 0 && !InventoryFinancialsEffectsInActive)
							{
								bool SaleCostingJobOrderWise = false;
								bool FIFOCgs = CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 5);
								bool num = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow"));
								SaleCostingJobOrderWise = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "SaleCostingJobOrderWise"));
								int FOCInventoryExpenseAccount = (Foc ? Conversion.ToInt(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "FOCInventoryExpenseAccount")) : 0);
								int ZakatInventoryExpensesAccount = (Zakat ? Conversion.ToInt(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ZakatInventoryExpensesAccount")) : 0);
								if (Foc && FOCInventoryExpenseAccount == 0)
								{
									throw new Exception("Foc Inventory Expense account not found from configuration...");
								}
								if (Zakat && ZakatInventoryExpensesAccount == 0)
								{
									throw new Exception("Zakat Inventory Expense account not found from configuration...");
								}
								if (num || SaleCostingJobOrderWise || (FIFOCgs && LoaderFlag))
								{
									bool IsPurchaseReturn = obj.DocumentTypeId == 145;
									HashSet<int> PmDocumentTypeIds = new HashSet<int> { 103, 126, 133, 145 };
									bool ItemWiseCGS = PmDocumentTypeIds.Contains(obj.DocumentTypeId) || CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 1);
									bool Item_Job_Wise = CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 2);
									bool CGSLoaderWise = CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 3);
									if (FIFOCgs && LoaderFlag && !IsPurchaseReturn)
									{
										CGSLoaderWise = true;
									}
									if (CGSLoaderWise)
									{
										if (item4.RefRefDocumentTypeId <= 0 || item4.RefRefDocIdNo <= 0)
										{
											throw new Exception("RefIds Not Found");
										}
										DataTable dt = CommonServies.GetAvgRateFromAvaiblableLoaderStock(new ReportsParameters
										{
											OrganizationId = obj.OrganizationId,
											CompanyId = obj.CompanyId,
											RefDocumentTypeId = item4.RefRefDocumentTypeId,
											RefRefDocIdNo = item4.RefRefDocIdNo,
											RefDocSubId = item4.RefDocSubId
										});
										if (dt.Rows.Count > 0)
										{
											item4.ItemCgsRate = Conversion.ToDouble(dt.Rows[0]["AvgRate"]);
											item4.RateUOM = Conversion.ToDouble(dt.Rows[0]["RateUom"]);
											item4.CgsRateUomId = Conversion.ToInt(dt.Rows[0]["RateUomId"]);
										}
										if (item4.ItemCgsRate <= 0.0)
										{
											throw new Exception("CGS Rate not found");
										}
									}
									else if (ItemWiseCGS || Item_Job_Wise)
									{
										if (PmDocumentTypeIds.Contains(obj.DocumentTypeId))
										{
											ReportsParameters parameters = new ReportsParameters();
											parameters.OrganizationId = obj.OrganizationId;
											parameters.CompanyId = obj.CompanyId;
											parameters.DocDate = obj.DocDate;
											parameters.ItemId = item4.ItemId;
											parameters.ItemConditionId = item4.ItemConditionId;
											if (obj.Id > 0)
											{
												parameters.DocumentTypeId = obj.DocumentTypeId;
												parameters.RecId = obj.Id;
											}
											DataTable dt2 = GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand(parameters);
											if (dt2.Rows.Count > 0)
											{
												item4.ItemCgsRate = Math.Round(Conversion.ToDouble(dt2.Rows[0]["AvgRate"]), 3);
											}
											if (item4.ItemCgsRate <= 0.0)
											{
												throw new Exception("CGS Rate not found.for Item " + item4.ItemName);
											}
										}
										else if (SaleCostingJobOrderWise)
										{
											ReportsParameters parameters2 = new ReportsParameters();
											parameters2.OrganizationId = obj.OrganizationId;
											parameters2.CompanyId = obj.CompanyId;
											parameters2.DocDate = obj.DocDate;
											parameters2.ItemId = item4.ItemId;
											parameters2.JobLotId = item4.JobLotId;
											if (obj.Id > 0)
											{
												parameters2.DocumentTypeId = obj.DocumentTypeId;
												parameters2.RecId = obj.Id;
											}
											item4.ItemCgsRate = CommonServies.getAvgRateByJobOrder(parameters2);
											if (item4.ItemCgsRate <= 0.0)
											{
												item4.ItemCgsRate = 1.0;
											}
										}
										else
										{
											ReportsParameters parameters3 = new ReportsParameters();
											parameters3.OrganizationId = obj.OrganizationId;
											parameters3.CompanyId = obj.CompanyId;
											parameters3.ItemId = item4.ItemId;
											parameters3.DocDate = obj.DocDate;
											parameters3.JobLotId = item4.JobLotId;
											parameters3.CropYear = item4.CropYear;
											parameters3.WarehouseId = item4.WarehouseId;
											if (obj.Id > 0)
											{
												parameters3.DocumentTypeId = obj.DocumentTypeId;
												parameters3.Id = obj.Id;
											}
											item4.ItemCgsRate = CommonServies.AvgRateOnlyForCGS(parameters3);
											if (item4.ItemCgsRate <= 0.0)
											{
												throw new Exception("CGS Rate not found.for Item " + item4.ItemName);
											}
										}
									}
									if (CGSLoaderWise || ItemWiseCGS || Item_Job_Wise)
									{
										double CgsAmount = 0.0;
										parts4 = new List<string>();
										int stockGlAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
										int cgsGlAccountId = Conversion.ToInt(find3[0].COGSGLAC);
										string itemName4 = find3[0].ItemName;
										if (obj.DocumentTypeId == 128)
										{
											parts4.Add($"NoOfBags: {item4.ItemQty}");
											parts4.Add(itemName4);
											parts4.Add($"Net Weight: {item4.NetBillWeight}");
											parts4.Add($"Rate: {item4.ItemCgsRate}");
											if (item4.ExpenseAmount > 0.0)
											{
												parts4.Add($"Expense Amount: {item4.ExpenseAmount}");
											}
											if (item4.CommissionAmount > 0.0)
											{
												parts4.Add($"Commission Amount: {item4.CommissionAmount}");
											}
											double packUom = CommonServies.GetEqvilentByItemId(new ReportsParameters
											{
												OrganizationId = obj.OrganizationId,
												CompanyId = obj.CompanyId,
												ItemId = item4.ItemId
											});
											item4.ItemCgsRate *= packUom;
											CgsAmount = item4.ItemCgsRate * item4.ItemQty;
										}
										else if (new int[5] { 103, 126, 133, 184, 145 }.Contains(obj.DocumentTypeId))
										{
											parts4.Add($"ItemQty: {item4.ItemQty}");
											parts4.Add(itemName4);
											if (item4.NetBillWeight > 0.0)
											{
												parts4.Add($"Net Weight: {item4.NetBillWeight}");
											}
											parts4.Add($"Rate: {item4.ItemCgsRate}");
											if (item4.ItemDiscountAmount > 0.0)
											{
												parts4.Add($"Discount Amount: {item4.ItemDiscountAmount}");
											}
											if (item4.FreightAmount > 0.0)
											{
												parts4.Add($"Credit Amount: {item4.FreightAmount}");
											}
											if (item4.CommissionAmount > 0.0)
											{
												parts4.Add($"Commission Amount: {item4.CommissionAmount}");
											}
											CgsAmount = item4.ItemCgsRate * item4.ItemQty;
										}
										else
										{
											parts4.Add($"NoOfBags: {item4.ItemQty}");
											parts4.Add(itemName4);
											parts4.Add($"GrossWeight: {item4.GrossWeight}");
											parts4.Add($"Net Weight: {item4.NetBillWeight}");
											parts4.Add($"Rate: {item4.ItemCgsRate}");
											if (item4.EBTotalWt > 0.0)
											{
												parts4.Add($"EmptyBagsDeduction: {item4.EBTotalWt}");
											}
											if (item4.ExpenseAmount > 0.0)
											{
												parts4.Add($"Expense Amount: {item4.ExpenseAmount}");
											}
											if (item4.CommissionAmount > 0.0)
											{
												parts4.Add($"Commission Amount: {item4.CommissionAmount}");
											}
											if (item4.NetBillWeight == 0.0)
											{
												throw new Exception("Validation Error: Net Bill Weight is missing.");
											}
											if (item4.RateUOM == 0.0)
											{
												throw new Exception("Validation Error: Rate UOM is missing.");
											}
										}
										parts4.Add("Party: " + obj.CommonIdsForFinancials.CompanyName);
										string RemarksCgs = string.Join(", ", parts4);
										if (obj.DocumentTypeId != 128 && obj.DocumentTypeId != 103 && obj.DocumentTypeId != 126 && obj.DocumentTypeId != 133 && obj.DocumentTypeId != 145 && item4.ItemCgsRate > 0.0 && item4.NetBillWeight > 0.0)
										{
											if (!CGSLoaderWise)
											{
												ReportsParameters Uom = new ReportsParameters();
												Uom.OrganizationId = obj.OrganizationId;
												Uom.CompanyId = obj.CompanyId;
												Uom.ItemId = item4.ItemId;
												Uom.Id = item4.UomScheduleIdRate;
												item4.RateUOM = CommonServies.GetEqvilentByItemIdAndUomScheduleId(Uom);
												item4.ItemCgsRate = item4.RateUOM * item4.ItemCgsRate;
											}
											CgsAmount = item4.NetStockWeight / item4.RateUOM * item4.ItemCgsRate;
										}
										if (!IsPurchaseReturn)
										{
											VoucherDetail vd29 = new VoucherDetail();
											vd29.LineId = item4.LineId;
											vd29.AccountId = ((FOCInventoryExpenseAccount > 0) ? FOCInventoryExpenseAccount : ((ZakatInventoryExpensesAccount > 0) ? ZakatInventoryExpensesAccount : Conversion.ToInt(find3[0].COGSGLAC)));
											vd29.AgainstAccountId = stockGlAccountId;
											vd29.Comments = RemarksCgs;
											vd29.DebitAmount = Conversion.ToDouble(CgsAmount);
											vd29.DocumentTypeIdRef = Conversion.ToInt(item4.RefRefDocumentTypeId);
											vd29.InvoiceNoRefId = Conversion.ToInt(item4.RefRefDocIdNo);
											vd29.RefInvoiceNo = Conversion.ToString(item4.RefDocSubId);
											vd29.ItemId = Conversion.ToInt(item4.ItemId);
											vd29.QtyOut = Conversion.ToDouble(item4.ItemQty);
											vd29.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
											vd29.ItemCgsRate = Conversion.ToDouble(item4.ItemCgsRate);
											vd29.RateCut = Conversion.ToDouble(item4.RateCut);
											vd29.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
											vd29.ItemAmount = Conversion.ToDouble(CgsAmount);
											vd29.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
											vd29.Commission = Conversion.ToDouble(item4.CommissionAmount);
											vd29.Freight = Conversion.ToDouble(item4.FreightAmount);
											vd29.OrderNo = Conversion.ToInt(item4.SaleOrder);
											vd29.GpNo = Conversion.ToInt(item4.GpNo);
											vd29.VehicleNo = item4.VehicleNo;
											vd29.JobLotId = Conversion.ToInt(item4.JobLotId);
											vd29.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
											vd29.BranchesId = item4.BranchId;
											vd29.IsCGS = 1;
											vd29.CostCenterId = item4.CostCenterId;
											vh.voucherDetailList.Add(vd29);
											VoucherDetail vd28 = new VoucherDetail();
											vd28.LineId = item4.LineId;
											vd28.AccountId = stockGlAccountId;
											vd28.AgainstAccountId = ((FOCInventoryExpenseAccount > 0) ? FOCInventoryExpenseAccount : ((ZakatInventoryExpensesAccount > 0) ? ZakatInventoryExpensesAccount : Conversion.ToInt(find3[0].COGSGLAC)));
											vd28.Comments = RemarksCgs;
											vd28.CreditAmount = Conversion.ToDouble(CgsAmount);
											vd28.DocumentTypeIdRef = Conversion.ToInt(item4.RefRefDocumentTypeId);
											vd28.InvoiceNoRefId = Conversion.ToInt(item4.RefRefDocIdNo);
											vd28.RefInvoiceNo = Conversion.ToString(item4.RefDocSubId);
											vd28.ItemId = Conversion.ToInt(item4.ItemId);
											vd28.QtyOut = Conversion.ToDouble(item4.ItemQty);
											vd28.WeightOut = Conversion.ToDouble(item4.NetBillWeight);
											vd28.ItemCgsRate = Conversion.ToDouble(item4.ItemCgsRate);
											vd28.RateCut = Conversion.ToDouble(item4.RateCut);
											vd28.RateCutAmount = Conversion.ToDouble(item4.RateCutAmount);
											vd28.ItemAmount = Conversion.ToDouble(CgsAmount);
											vd28.Expenses = Conversion.ToDouble(item4.ExpenseAmount);
											vd28.Commission = Conversion.ToDouble(item4.CommissionAmount);
											vd28.Freight = Conversion.ToDouble(item4.FreightAmount);
											vd28.OrderNo = Conversion.ToInt(item4.SaleOrder);
											vd28.GpNo = Conversion.ToInt(item4.GpNo);
											vd28.VehicleNo = item4.VehicleNo;
											vd28.JobLotId = Conversion.ToInt(item4.JobLotId);
											vd28.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
											vd28.BranchesId = item4.BranchId;
											vd28.IsCGS = 1;
											vd28.CostCenterId = item4.CostCenterId;
											vh.voucherDetailList.Add(vd28);
										}
										else
										{
											double stockDiffAmount = StockCreditAmount - CgsAmount;
											if (stockDiffAmount != 0.0)
											{
												VoucherDetail vd30 = new VoucherDetail();
												vd30.IsCGS = 1;
												vd30.AccountId = stockGlAccountId;
												vd30.AgainstAccountId = cgsGlAccountId;
												vd30.Comments = "Stock Difference";
												if (stockDiffAmount > 0.0)
												{
													vd30.DebitAmount = stockDiffAmount;
												}
												else
												{
													vd30.CreditAmount = stockDiffAmount;
												}
												vd30.BranchesId = obj.BranchesId;
												vh.voucherDetailList.Add(vd30);
												VoucherDetail vd31 = new VoucherDetail();
												vd31.IsCGS = 1;
												vd31.AccountId = cgsGlAccountId;
												vd31.AgainstAccountId = stockGlAccountId;
												vd31.Comments = "Stock Difference";
												if (stockDiffAmount > 0.0)
												{
													vd31.CreditAmount = stockDiffAmount;
												}
												else
												{
													vd31.DebitAmount = stockDiffAmount;
												}
												vd31.BranchesId = obj.BranchesId;
												vh.voucherDetailList.Add(vd31);
											}
										}
									}
								}
							}
						}
						if (new HashSet<int>
						{
							95, 96, 103, 126, 128, 133, 171, 1608, 1609, 1611,
							1661, 1660, 145, 99, 222
						}.Contains(obj.DocumentTypeId) && item4.FreightAmount > 0.0 && Conversion.ToInt(obj.TransporterDebitGLId) == 0 && !Foc && !Zakat)
						{
							parts4 = new List<string>();
							string itemName3 = find3[0].ItemName;
							if (obj.DocumentTypeId == 145)
							{
								parts4.Add($"Purchase Return {item4.ItemQty} Qty of {itemName3}");
							}
							else
							{
								if (!PmTrans)
								{
									parts4.Add($"Sale {item4.ItemQty} Qty ({item4.NetBillWeight}) of {itemName3}");
								}
								if (PmTrans)
								{
									parts4.Add($"Sale {item4.ItemQty} of {itemName3}");
								}
								if (!PmTrans)
								{
									parts4.Add($"{Math.Round(Mond2, 2)} Mond");
								}
							}
							parts4.Add($"@ NetRate {item4.ItemRate}");
							parts4.Add($"= Rs. {item4.ItemAmount}");
							parts4.Add($"Freight {item4.FreightAmount}");
							if (item4.SaleOrder > 0)
							{
								parts4.Add($"Under contract # {item4.SaleOrder}");
							}
							if (!string.IsNullOrWhiteSpace(item4.VehicleNo))
							{
								parts4.Add("Vehicle # " + item4.VehicleNo);
							}
							if (item4.GpNo != 0)
							{
								parts4.Add($"Gp # {item4.GpNo}");
							}
							if (!string.IsNullOrWhiteSpace(obj.ManualBillNo))
							{
								parts4.Add("Manual # " + obj.ManualBillNo);
							}
							parts4.Add("From " + obj.CommonIdsForFinancials.CompanyName);
							string freightRemarks = string.Join(", ", parts4);
							VoucherDetail vd25 = new VoucherDetail();
							if (obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 145)
							{
								vd25.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find3[0].PurchaseGLAC));
							}
							else
							{
								vd25.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find3[0].SaleGLAC));
							}
							vd25.AgainstAccountId = ((transId > 0) ? transId : vd25.AccountId);
							vd25.Comments = freightRemarks;
							vd25.DebitAmount = Conversion.ToDouble(item4.FreightAmount);
							vd25.CreditAmount = Conversion.ToDouble(0);
							vd25.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd25.BranchesId = item4.BranchId;
							vd25.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
							vh.voucherDetailList.Add(vd25);
						}
						if (new HashSet<int>
						{
							95, 99, 103, 128, 133, 139, 171, 184, 186, 1809,
							1608, 1609, 1661, 1660, 222
						}.Contains(obj.DocumentTypeId) && !Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")) && item4.ExpenseAmount > 0.0)
						{
							parts4 = new List<string>();
							string itemName2 = find3[0].ItemName;
							parts4.Add($"Sale {item4.ItemQty} Bags ({item4.NetBillWeight}) of {itemName2}");
							parts4.Add($"{Math.Round(Mond2, 2)} Mond");
							parts4.Add($"@ NetRate {item4.ItemRate}");
							parts4.Add($"= Rs. {item4.ItemAmount}");
							parts4.Add($"Other Exp {item4.ExpenseAmount}");
							if (item4.SaleOrder > 0)
							{
								parts4.Add($"Under contract # {item4.SaleOrder}");
							}
							if (!string.IsNullOrWhiteSpace(item4.VehicleNo))
							{
								parts4.Add("Vehicle # " + item4.VehicleNo);
							}
							if (item4.GpNo != 0)
							{
								parts4.Add($"Gp # {item4.GpNo}");
							}
							if (!string.IsNullOrWhiteSpace(obj.ManualBillNo))
							{
								parts4.Add("Manual # " + obj.ManualBillNo);
							}
							parts4.Add("From " + obj.CommonIdsForFinancials.CompanyName);
							string expenseRemarks = string.Join(", ", parts4);
							VoucherDetail vd24 = new VoucherDetail();
							vd24.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd24.AgainstAccountId = objlot2.AccountId;
							}
							else
							{
								vd24.AgainstAccountId = Conversion.ToInt(find3[0].SaleGLAC);
							}
							vd24.DebitAmount = Conversion.ToDouble(item4.ExpenseAmount);
							vd24.Comments = expenseRemarks;
							vd24.JobLotId = item4.JobLotId;
							vd24.SubsidiaryTypeId = 1;
							vd24.SubsidiaryAccountId = obj.SupplierCustomerId;
							vd24.SupplierCustomerId = obj.SupplierCustomerId;
							vd24.BranchesId = item4.BranchId;
							vd24.CostCenterId = item4.CostCenterId;
							vh.voucherDetailList.Add(vd24);
							VoucherDetail vd20 = new VoucherDetail();
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd20.AccountId = objlot2.AccountId;
							}
							else
							{
								vd20.AccountId = Conversion.ToInt(find3[0].SaleGLAC);
							}
							vd20.AgainstAccountId = vh.RefAccountId;
							vd20.CreditAmount = Conversion.ToDouble(item4.ExpenseAmount);
							vd20.Comments = expenseRemarks;
							vd20.JobLotId = item4.JobLotId;
							vd20.SubsidiaryAgainstTypeId = 1;
							vd20.SubsidiaryAgainstAccountId = obj.SupplierCustomerId;
							vd20.BranchesId = item4.BranchId;
							vd20.CostCenterId = item4.CostCenterId;
							vh.voucherDetailList.Add(vd20);
						}
						if (obj.DocumentTypeId == 145 && item4.ExpenseAmount > 0.0)
						{
							string ExpeneseRemarks = "";
							ExpeneseRemarks = "Purchase Return " + Conversion.ToString(find3[0].ItemName) + " Qty " + Conversion.ToString(item4.ItemQty) + " @ NetRate. " + Conversion.ToString(item4.ItemRate) + "   = Rs. " + Conversion.ToString(item4.ItemAmount);
							ExpeneseRemarks = ExpeneseRemarks + " Other Exp " + Conversion.ToString(item4.ExpenseAmount);
							if (item4.SaleOrder > 0)
							{
								ExpeneseRemarks = ExpeneseRemarks + " Under contract # " + item4.SaleOrder;
							}
							if (item4.VehicleNo != string.Empty)
							{
								ExpeneseRemarks = ExpeneseRemarks + " Vehicle # " + item4.VehicleNo;
							}
							if (item4.GpNo != 0)
							{
								ExpeneseRemarks = ExpeneseRemarks + " Gp # " + item4.GpNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								ExpeneseRemarks = ExpeneseRemarks + " Manual # " + obj.ManualBillNo;
							}
							ExpeneseRemarks = ExpeneseRemarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
							VoucherDetail vd23 = new VoucherDetail();
							vd23.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd23.AgainstAccountId = objlot2.AccountId;
							}
							else
							{
								vd23.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							vd23.DebitAmount = Conversion.ToDouble(item4.ExpenseAmount);
							vd23.Comments = ExpeneseRemarks;
							vd23.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd23.SubsidiaryTypeId = 1;
							vd23.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd23.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd23.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd23);
							VoucherDetail vd19 = new VoucherDetail();
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd19.AccountId = objlot2.AccountId;
							}
							else
							{
								vd19.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
							}
							vd19.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd19.CreditAmount = Conversion.ToDouble(item4.ExpenseAmount);
							vd19.Comments = ExpeneseRemarks;
							vd19.JobLotId = Conversion.ToInt(item4.JobLotId);
							vd19.SubsidiaryAgainstTypeId = 1;
							vd19.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd19.BranchesId = item4.BranchId;
							vh.voucherDetailList.Add(vd19);
						}
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				throw new Exception("Item Record Not found");
			}
			HashSet<int> commDocumentTypeIds = new HashSet<int>
			{
				95, 99, 103, 60, 128, 126, 133, 139, 171, 1809,
				222
			};
			if (obj.InvSaleInvoiceCommission.Count > 0 && commDocumentTypeIds.Contains(obj.DocumentTypeId) && !Foc && !Zakat)
			{
				int CommissionAgentGLId = 0;
				if (!DebitAmountChargetoExpenseAcOfCommission && obj.InvSaleInvoiceCommission.Select((InvSaleInvoiceCommission x) => x.CommissionAgentId).Distinct().ToList()
					.Count == 1)
				{
					CommissionAgentGLId = obj.CommonIdsForFinancials.CommissionGlAccountId;
				}
				foreach (InvSaleInvoiceDetail item3 in obj.InvSaleInvoiceDetaillist.ToList())
				{
					_ = string.Empty;
					double Mond = (PmTrans ? 0.0 : (Conversion.ToDouble(item3.NetBillWeight) / 40.0));
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
						if (find2.Count > 0)
						{
							JobLot objlot = new JobLot();
							if (item3.JobLotId > 0)
							{
								objlot = jobLot.GetByID(item3.JobLotId);
							}
							string commissionRemarks2 = string.Empty;
							if (item3.CommissionAmount > 0.0 && obj.CommissionDebitAcGLId == 0 && !DebitAmountChargetoExpenseAcOfCommission)
							{
								List<string> parts3 = new List<string>();
								string itemName = find2[0].ItemName;
								if (PmTrans)
								{
									parts3.Add($"Sale {item3.ItemQty}) of {itemName}");
								}
								else
								{
									parts3.Add($"Sale {item3.ItemQty} Bags ({item3.NetBillWeight}) of {itemName}");
								}
								if (Mond > 0.0)
								{
									parts3.Add($"{Math.Round(Mond, 2)} Mond");
								}
								parts3.Add($"@ NetRate {item3.ItemRate}");
								parts3.Add($"= Rs. {item3.ItemAmount}");
								InvSaleInvoiceCommission cmgnt = obj.InvSaleInvoiceCommission.FirstOrDefault((InvSaleInvoiceCommission x) => x.SaleOrderId == item3.SaleOrderId);
								string commType3 = "";
								string commissionRate = "";
								if (cmgnt != null)
								{
									int commissionType = Conversion.ToInt(cmgnt.CommType);
									commissionRate = cmgnt?.Rate.ToString() ?? "";
									commType3 = commissionType switch
									{
										1 => "Flat", 
										2 => "Percent", 
										3 => "OnWeight", 
										_ => "", 
									};
								}
								parts3.Add("CommType " + commType3);
								parts3.Add($"Commission @ {commissionRate} = Rs. {Math.Round(item3.CommissionAmount)}");
								if (item3.SaleOrder > 0)
								{
									parts3.Add($"Under contract # {item3.SaleOrder}");
								}
								if (!string.IsNullOrWhiteSpace(item3.VehicleNo))
								{
									parts3.Add("Vehicle # " + item3.VehicleNo);
								}
								if (item3.GpNo != 0)
								{
									parts3.Add($"Gp # {item3.GpNo}");
								}
								if (!string.IsNullOrWhiteSpace(obj.ManualBillNo))
								{
									parts3.Add("Manual # " + obj.ManualBillNo);
								}
								parts3.Add("From " + obj.CommonIdsForFinancials.CompanyName);
								commissionRemarks2 = string.Join(", ", parts3);
								VoucherDetail vd22 = new VoucherDetail();
								if (objlot != null && objlot.AccountId > 0)
								{
									vd22.AccountId = objlot.AccountId;
								}
								else
								{
									vd22.AccountId = Conversion.ToInt(find2[0].SaleGLAC);
								}
								vd22.AgainstAccountId = ((CommissionAgentGLId > 0) ? CommissionAgentGLId : vd22.AccountId);
								if (obj.CommissionRemarks == string.Empty)
								{
									vd22.Comments = commissionRemarks2;
								}
								else
								{
									vd22.Comments = commissionRemarks2 + Conversion.ToString(obj.CommissionRemarks);
								}
								vd22.DebitAmount = Conversion.ToDouble(item3.CommissionAmount);
								vd22.JobLotId = Conversion.ToInt(item3.JobLotId);
								vd22.BranchesId = item3.BranchId;
								vh.voucherDetailList.Add(vd22);
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
					throw new Exception("Item Record Not found");
				}
				int SaleGlId = 0;
				if (!DebitAmountChargetoExpenseAcOfCommission)
				{
					List<int> distinctItemIds = obj.InvSaleInvoiceDetaillist.Select((InvSaleInvoiceDetail x) => x.ItemId).Distinct().ToList();
					if (distinctItemIds.Count == 1)
					{
						int itemId = distinctItemIds[0];
						GetItemGlIdsandItemName item = dtitem.FirstOrDefault((GetItemGlIdsandItemName x) => x.Id == itemId);
						if (item != null)
						{
							SaleGlId = item.SaleGLAC;
							obj.CommonIdsForFinancials.SaleGLAccountId = SaleGlId;
						}
					}
				}
				foreach (InvSaleInvoiceCommission item2 in obj.InvSaleInvoiceCommission.ToList())
				{
					_ = string.Empty;
					List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(item2.CommissionAgentId)).ToList();
					if (find.Count > 0)
					{
						string commissionRemarks = "";
						string OrderNo = "";
						string VehicleNo = "";
						string GpNo = "";
						double ItemQty = 0.0;
						double NetBillWeight = 0.0;
						double ItemAmount = 0.0;
						if (item2.SaleOrderId > 0)
						{
							List<InvSaleInvoiceDetail> Cmgnt = obj.InvSaleInvoiceDetaillist.Where((InvSaleInvoiceDetail x) => x.SaleOrderId == Conversion.ToInt(item2.SaleOrderId)).ToList();
							ItemQty = Cmgnt.Sum((InvSaleInvoiceDetail row) => row.ItemQty);
							NetBillWeight = Cmgnt.Sum((InvSaleInvoiceDetail row) => row.NetBillWeight);
							ItemAmount = Cmgnt.Sum((InvSaleInvoiceDetail row) => row.ItemAmount);
							if (Cmgnt.Count > 0)
							{
								OrderNo = Conversion.ToString(Cmgnt[0].SaleOrder);
								VehicleNo = Conversion.ToString(Cmgnt[0].VehicleNo);
							}
						}
						else
						{
							ItemQty = obj.InvSaleInvoiceDetaillist.Sum((InvSaleInvoiceDetail row) => row.ItemQty);
							NetBillWeight = obj.InvSaleInvoiceDetaillist.Sum((InvSaleInvoiceDetail row) => row.NetBillWeight);
							ItemAmount = obj.InvSaleInvoiceDetaillist.Sum((InvSaleInvoiceDetail row) => row.ItemAmount);
						}
						if (item2.CommAmount > 0.0 && item2.CommissionAgentId > 0 && DebitAmountChargetoExpenseAcOfCommission)
						{
							if (item2.DebitAccountId == 0)
							{
								throw new Exception("Commission Debit Account Not Found in Commission Agent Grid");
							}
							List<string> parts2 = new List<string>();
							string commType2 = "";
							double commType4 = item2.CommType;
							commType2 = ((commType4 == 1.0) ? "Flat" : ((commType4 == 2.0) ? "Percent" : ((commType4 != 3.0) ? "" : "OnWeight")));
							parts2.Add($"Sale ItemQty {ItemQty}");
							if (!PmTrans)
							{
								parts2.Add($"BillWeight {NetBillWeight}");
							}
							parts2.Add($"ItemAmount {ItemAmount}");
							parts2.Add("CommType " + commType2);
							parts2.Add($"Commission paid @ {item2.Rate} = Rs. {Math.Round(item2.CommAmount)}");
							if (!string.IsNullOrWhiteSpace(OrderNo))
							{
								parts2.Add("Under contract # " + OrderNo);
							}
							if (!string.IsNullOrWhiteSpace(VehicleNo))
							{
								parts2.Add("Vehicle # " + VehicleNo);
							}
							if (!string.IsNullOrWhiteSpace(GpNo))
							{
								parts2.Add("Gp # " + GpNo);
							}
							if (!string.IsNullOrWhiteSpace(obj.ManualBillNo))
							{
								parts2.Add("Manual # " + obj.ManualBillNo);
							}
							parts2.Add("From " + obj.CommonIdsForFinancials.CompanyName);
							commissionRemarks = string.Join(", ", parts2);
							VoucherDetail vd21 = new VoucherDetail();
							vd21.AccountId = Conversion.ToInt(item2.DebitAccountId);
							vd21.AgainstAccountId = Conversion.ToInt(find[0].GlAccountId);
							if (item2.Remarks == string.Empty)
							{
								vd21.Comments = commissionRemarks;
							}
							else
							{
								vd21.Comments = commissionRemarks + item2.Remarks;
							}
							vd21.DebitAmount = Conversion.ToDouble(item2.CommAmount);
							vd21.SubsidiaryTypeId = 1;
							vd21.SubsidiaryAccountId = Conversion.ToInt(item2.CommDebitSupCustId);
							vd21.SupplierCustomerId = Conversion.ToInt(item2.CommDebitSupCustId);
							vd21.SubsidiaryAgainstTypeId = vd21.SubsidiaryTypeId;
							vd21.SubsidiaryAgainstAccountId = Conversion.ToInt(item2.CommissionAgentId);
							vd21.BranchesId = obj.BranchesId;
							vd21.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
							vh.voucherDetailList.Add(vd21);
							VoucherDetail vd27 = new VoucherDetail();
							vd27.AccountId = Conversion.ToInt(find[0].GlAccountId);
							vd27.AgainstAccountId = Conversion.ToInt(item2.DebitAccountId);
							if (item2.Remarks == string.Empty)
							{
								vd27.Comments = commissionRemarks;
							}
							else
							{
								vd27.Comments = commissionRemarks + Conversion.ToString(item2.Remarks);
							}
							vd27.CreditAmount = Conversion.ToDouble(item2.CommAmount);
							vd27.SubsidiaryTypeId = 1;
							vd27.SubsidiaryAccountId = Conversion.ToInt(item2.CommissionAgentId);
							vd27.SupplierCustomerId = Conversion.ToInt(item2.CommissionAgentId);
							vd27.SubsidiaryTypeId = vd27.SubsidiaryTypeId;
							vd27.SubsidiaryAccountId = Conversion.ToInt(item2.CommDebitSupCustId);
							vd27.BranchesId = obj.BranchesId;
							vd27.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
							vh.voucherDetailList.Add(vd27);
						}
						if (item2.CommAmount > 0.0 && item2.CommissionAgentId > 0 && !DebitAmountChargetoExpenseAcOfCommission)
						{
							List<string> parts = new List<string>();
							string commType = "";
							double commType4 = item2.CommType;
							commType = ((commType4 == 1.0) ? "Flat" : ((commType4 == 2.0) ? "Percent" : ((commType4 != 3.0) ? "" : "OnWeight")));
							parts.Add($"Sale ItemQty {ItemQty}");
							if (!PmTrans)
							{
								parts.Add($"BillWeight {NetBillWeight}");
							}
							parts.Add($"ItemAmount {ItemAmount}");
							parts.Add("CommType " + commType);
							parts.Add($"Commission paid @ {item2.Rate} = Rs. {Math.Round(item2.CommAmount)}");
							if (!string.IsNullOrWhiteSpace(OrderNo))
							{
								parts.Add("Under contract # " + OrderNo);
							}
							if (!string.IsNullOrWhiteSpace(VehicleNo))
							{
								parts.Add("Vehicle # " + VehicleNo);
							}
							if (!string.IsNullOrWhiteSpace(GpNo))
							{
								parts.Add("Gp # " + GpNo);
							}
							if (!string.IsNullOrWhiteSpace(obj.ManualBillNo))
							{
								parts.Add("Manual # " + obj.ManualBillNo);
							}
							parts.Add("From " + obj.CommonIdsForFinancials.CompanyName);
							commissionRemarks = string.Join(", ", parts);
							VoucherDetail vd26 = new VoucherDetail();
							vd26.AccountId = Conversion.ToInt(find[0].GlAccountId);
							vd26.AgainstAccountId = ((SaleGlId > 0) ? SaleGlId : vd26.AccountId);
							if (item2.Remarks == string.Empty)
							{
								vd26.Comments = commissionRemarks;
							}
							else
							{
								vd26.Comments = commissionRemarks + Conversion.ToString(item2.Remarks);
							}
							vd26.CreditAmount = Conversion.ToDouble(item2.CommAmount);
							vd26.SubsidiaryTypeId = 1;
							vd26.SubsidiaryAccountId = Conversion.ToInt(item2.CommissionAgentId);
							vd26.SupplierCustomerId = Conversion.ToInt(item2.CommissionAgentId);
							vd26.BranchesId = obj.BranchesId;
							vd26.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
							vh.voucherDetailList.Add(vd26);
						}
						continue;
					}
					throw new Exception("Commission AgentGlId Not Found");
				}
			}
			if (obj.DiscountAmount > 0m && (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660 || obj.DocumentTypeId == 1662))
			{
				if (obj.DiscountAccountId == 0)
				{
					throw new Exception("Discount debit account fields is required");
				}
				VoucherDetail vd12 = new VoucherDetail();
				vd12.LineId = 0;
				vd12.AccountId = ((obj.DocumentTypeId == 1662) ? vh.RefAccountId : Conversion.ToInt(obj.DiscountAccountId));
				vd12.AgainstAccountId = ((obj.DocumentTypeId == 1662) ? obj.DiscountAccountId : Conversion.ToInt(vh.RefAccountId));
				vd12.Comments = "Discount to " + obj.CommonIdsForFinancials.CompanyName;
				vd12.DebitAmount = Conversion.ToDouble(obj.DiscountAmount);
				vd12.DMultiCurrencyId = obj.CurrencyId;
				vd12.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
				vd12.DCurrencyAmount = Conversion.ToDouble(obj.FcyAmount);
				vd12.SubsidiaryAgainstTypeId = 1;
				vd12.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd12.BranchesId = obj.BranchesId;
				vd12.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
				vh.voucherDetailList.Add(vd12);
				VoucherDetail vd14 = new VoucherDetail();
				vd14.LineId = 0;
				vd14.AccountId = ((obj.DocumentTypeId == 1662) ? obj.DiscountAccountId : Conversion.ToInt(vh.RefAccountId));
				vd14.AgainstAccountId = ((obj.DocumentTypeId == 1662) ? vh.RefAccountId : Conversion.ToInt(obj.DiscountAccountId));
				vd14.Comments = "Discount to " + obj.CommonIdsForFinancials.CompanyName;
				vd14.CreditAmount = Conversion.ToDouble(obj.DiscountAmount);
				vd14.DMultiCurrencyId = obj.CurrencyId;
				vd14.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
				vd14.DCurrencyAmount = Conversion.ToDouble(obj.FcyAmount);
				vd14.SubsidiaryTypeId = 1;
				vd14.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd14.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
				vd14.BranchesId = obj.BranchesId;
				vd14.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
				vh.voucherDetailList.Add(vd14);
			}
		}
		return vh;
	}

	public static VoucherHead OtherExpenseFinancial(Architecture.Model.Inventory.InvSaleInvoice obj, List<Architecture.Model.Inventory.InventoryItemsOther> dtItemOther)
	{
		VoucherHead vh = new VoucherHead
		{
			RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId,
			AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId
		};
		if (!new HashSet<int>
		{
			95, 99, 103, 139, 171, 184, 186, 1809, 1608, 1609,
			1661, 1660, 222, 1662
		}.Contains(obj.DocumentTypeId) || obj.InvSaleInvoiceExpenseList.Count == 0)
		{
			return vh;
		}
		if (!Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")))
		{
			return vh;
		}
		if (dtItemOther.Count == 0)
		{
			throw new Exception("Other Items not found");
		}
		foreach (InvSaleInvoiceExpense item in obj.InvSaleInvoiceExpenseList)
		{
			if (!(item.Amount <= 0.0))
			{
				Architecture.Model.Inventory.InventoryItemsOther find = dtItemOther.FirstOrDefault((Architecture.Model.Inventory.InventoryItemsOther x) => x.Id == Convert.ToInt32(item.InvRevExpItemId));
				if (find == null)
				{
					throw new Exception("Other Items Sale GL Account not found");
				}
				string comment = find.OtherItemName + " " + item.Remarks;
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = ((obj.DocumentTypeId == 1662) ? Conversion.ToInt(find.SaleGLAcId) : Conversion.ToInt(vh.RefAccountId)),
					AgainstAccountId = ((obj.DocumentTypeId == 1662) ? vh.RefAccountId : Conversion.ToInt(find.SaleGLAcId)),
					Comments = comment,
					DebitAmount = item.Amount,
					SubsidiaryTypeId = 1,
					SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId),
					SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId),
					BranchesId = obj.BranchesId,
					CostCenterId = obj.CommonIdsForFinancials.CostCenterId
				});
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = ((obj.DocumentTypeId == 1662) ? vh.RefAccountId : Conversion.ToInt(find.SaleGLAcId)),
					AgainstAccountId = ((obj.DocumentTypeId == 1662) ? Conversion.ToInt(find.SaleGLAcId) : Conversion.ToInt(vh.RefAccountId)),
					Comments = comment,
					CreditAmount = item.Amount,
					SubsidiaryAgainstTypeId = 1,
					SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId),
					BranchesId = obj.BranchesId,
					CostCenterId = obj.CommonIdsForFinancials.CostCenterId
				});
			}
		}
		return vh;
	}

	private static VoucherDetail AddVoucherDetail(int accountId, int againstAccountId, double? debitAmount, double? creditAmount, bool isQtyOut, InvSaleInvoiceDetail item, string remarks, int supplierCustomerId, string companyName)
	{
		return new VoucherDetail
		{
			LineId = item.LineId,
			AccountId = accountId,
			AgainstAccountId = againstAccountId,
			Comments = remarks + (creditAmount.HasValue ? ("  " + companyName) : ""),
			DebitAmount = debitAmount.GetValueOrDefault(),
			CreditAmount = creditAmount.GetValueOrDefault(),
			ItemId = Conversion.ToInt(item.ItemId),
			QtyIn = (isQtyOut ? 0.0 : Conversion.ToDouble(item.ItemQty)),
			QtyOut = (isQtyOut ? Conversion.ToDouble(item.ItemQty) : 0.0),
			ItemRate = Conversion.ToDouble(item.ItemRate),
			WeightIn = (isQtyOut ? 0.0 : Conversion.ToDouble(item.NetBillWeight)),
			WeightOut = (isQtyOut ? Conversion.ToDouble(item.NetBillWeight) : 0.0),
			RateCut = Conversion.ToDouble(item.RateCut),
			RateCutAmount = Conversion.ToDouble(item.RateCutAmount),
			ItemAmount = Conversion.ToDouble(item.ItemAmount),
			Expenses = Conversion.ToDouble(item.ExpenseAmount),
			Freight = Conversion.ToDouble(item.FreightAmount),
			Commission = Conversion.ToDouble(item.CommissionAmount),
			OrderNo = Conversion.ToInt(item.SaleOrder),
			GpNo = Conversion.ToInt(item.GpNo),
			JobLotId = Conversion.ToInt(item.JobLotId),
			VehicleNo = item.VehicleNo,
			DMultiCurrencyId = item.CurrencyId,
			DExchangeCurrencyRate = Conversion.ToDouble(item.ExchangeRate),
			DCurrencyAmount = Conversion.ToDouble(item.FcyAmount),
			SubsidiaryTypeId = (debitAmount.HasValue ? 1 : 0),
			SubsidiaryAccountId = (debitAmount.HasValue ? supplierCustomerId : 0),
			SubsidiaryAgainstTypeId = (creditAmount.HasValue ? 1 : 0),
			SubsidiaryAgainstAccountId = (creditAmount.HasValue ? supplierCustomerId : 0),
			SupplierCustomerId = supplierCustomerId,
			BranchesId = item.BranchId,
			IsCGS = 0,
			CostCenterId = item.CostCenterId
		};
	}
}
