using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.BLL.TaxProject.Common;
using Architecture.Common;
using Architecture.DAL.TaxProject.Common;
using Architecture.Model;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Inventory;
using Newtonsoft.Json;

namespace Architecture.BLL.TaxProject.Inventory.Financials.SaleInvoice;

public class SaleInvoiceGeneralFinancialMethods
{
	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj)
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

	public static VoucherHead SalesTaxFinancial(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = ((obj.CommonIdsForFinancials.CommissionGlAccountId > 0) ? obj.CommonIdsForFinancials.CommissionGlAccountId : vh.RefAccountId);
		if (new HashSet<int>
		{
			95, 99, 96, 133, 1608, 1609, 1661, 1660, 1611, 145,
			1662
		}.Contains(obj.DocumentTypeId) && obj.InvSaleInvoiceDetaillist.Count > 0)
		{
			double TaxTotalAmount = 0.0;
			foreach (InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist.ToList())
			{
				TaxTotalAmount += item.TaxAmount;
			}
			if (TaxTotalAmount > 0.0 && obj.TaxAccountId > 0)
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
					vd2.AgainstAccountId = obj.TaxAccountId;
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
					vd4.AccountId = obj.TaxAccountId;
					vd4.AgainstAccountId = vh.RefAccountId;
					vd4.Comments = "Tax " + TaxTotalAmount;
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

	public static VoucherHead FreightFinancial(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj)
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
							AgainstAccountId = r.TansporterId,
							Comments = r.Remarks,
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
					Comments = "FreightAmount",
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
					Comments = "FreightAmount",
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
						Comments = obj.FreightRemark,
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
					Comments = obj.FreightRemark,
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

	public static VoucherHead ItemAndCustomerByWeightFinancial(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj, List<GetItemGlIdsandItemName> dtitem, List<SupplierCustomerGlAccountId> dtSupplier)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		bool DebitAmountChargetoExpenseAcOfCommission = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "DebitAmountChargetoExpenseAcOfCommission"));
		bool Foc = false;
		bool Zakat = false;
		if (obj.DocumentTypeId == 99)
		{
			Foc = obj.OtherCategoryId == 55;
			Zakat = obj.OtherCategoryId == 56;
		}
		HashSet<int> obj2 = new HashSet<int>
		{
			95, 96, 99, 103, 60, 126, 128, 133, 139, 171,
			208, 1608, 1609, 1611, 184, 186, 1809, 145, 222, 1661,
			1660, 1662
		};
		HashSet<int> returnDocumentTypeIds = new HashSet<int> { 1611, 1662 };
		if (obj2.Contains(obj.DocumentTypeId) && obj.InvSaleInvoiceDetaillist.Count > 0)
		{
			List<InvSaleInvoiceDetail> itemListWithUniqueItems = new List<InvSaleInvoiceDetail>();
			List<InvSaleInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<InvSaleInvoiceDetail>>(JsonConvert.SerializeObject(obj.InvSaleInvoiceDetaillist.ToList()));
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
					if (itemListWithUniqueItems[k].ItemId == dummyArray[i].ItemId && itemListWithUniqueItems[k]?.BranchId == dummyArray[i]?.BranchId)
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
				for (int l = 0; l < dummyArray.Count; l++)
				{
					if (itemListWithUniqueItems[j].ItemId == dummyArray[l].ItemId && itemListWithUniqueItems[j]?.BranchId == dummyArray[l]?.BranchId)
					{
						itemAmountAtI += dummyArray[l].ItemAmount;
						itemFreightAmountAtI += dummyArray[l].FreightAmount;
						itemExpenseAmountAtI += dummyArray[l].ExpenseAmount;
						itemCommissionAmountAtI += dummyArray[l].CommissionAmount;
						itemQtyAtI += dummyArray[l].ItemQty;
						itemWeightAtI += dummyArray[l].NetBillWeight;
						itemWeightCutAtI += dummyArray[l].WeightCut;
						itemRateCutAtI += dummyArray[l].RateCut;
						itemEmptyBagsDudectionAtI += dummyArray[l].EBTotalWt;
						itemGrossWeightAtI += dummyArray[l].GrossWeight;
					}
				}
				itemListWithUniqueItems[j].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[j].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[j].FreightAmount = itemFreightAmountAtI;
				itemListWithUniqueItems[j].CommissionAmount = itemCommissionAmountAtI;
				itemListWithUniqueItems[j].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[j].NetBillWeight = itemWeightAtI;
				itemListWithUniqueItems[j].WeightCut = itemWeightCutAtI;
				itemListWithUniqueItems[j].RateCut = itemRateCutAtI;
				itemListWithUniqueItems[j].EBTotalWt = itemEmptyBagsDudectionAtI;
				itemListWithUniqueItems[j].GrossWeight = itemGrossWeightAtI;
			}
			string InvoiceHeaderRemarksAuto = "";
			int getOrderNo = 0;
			DateTime getOrderDate = DateTime.Now;
			int RowNo = 1;
			foreach (InvSaleInvoiceDetail item4 in itemListWithUniqueItems)
			{
				if (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660)
				{
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item4.ItemId)).ToList();
					InvoiceHeaderRemarksAuto = InvoiceHeaderRemarksAuto + Conversion.ToString(find3[0].ItemName) + "  Qty " + Conversion.ToString(item4.ItemQty) + "  ";
				}
				_ = item4.ItemId;
				getOrderNo = item4.SaleOrder;
				getOrderDate = item4.OrderDate;
				_ = item4.GpNo;
				_ = item4.VehicleNo;
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
			bool LoaderFlag = false;
			foreach (InvSaleInvoiceDetail item3 in obj.InvSaleInvoiceDetaillist.ToList())
			{
				if (item3.RefRefDocumentTypeId > 0 && item3.RefRefDocIdNo > 0 && item3.RefDocSubId > 0)
				{
					LoaderFlag = true;
				}
				string Remarks = string.Empty;
				double Mond2 = Conversion.ToDouble(item3.NetBillWeight) / 40.0;
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find4 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
					if (find4.Count > 0)
					{
						JobLot objlot2 = new JobLot();
						if (item3.JobLotId > 0)
						{
							objlot2 = Architecture.BLL.TaxProject.Common.CommonServies.JobLotGetByID(item3.JobLotId);
						}
						if (obj.DocumentTypeId == 103 || obj.DocumentTypeId == 60 || obj.DocumentTypeId == 128 || obj.DocumentTypeId == 184 || obj.DocumentTypeId == 186 || obj.DocumentTypeId == 145)
						{
							if (obj.DocumentTypeId == 186 && obj.ManualBillNo != null && obj.ManualBillNo != string.Empty)
							{
								Remarks = Conversion.ToString(obj.ManualBillNo);
							}
							Remarks = Remarks + obj.RemarksHeader + " Item: " + Conversion.ToString(find4[0].ItemName) + " Qty: " + item3.ItemQty + " Rate: " + item3.ItemRate;
							if (item3.ItemDiscountAmount > 0.0)
							{
								Remarks = Remarks + " Discount Amount: " + item3.ItemDiscountAmount;
							}
							if (item3.ExpenseAmount > 0.0)
							{
								Remarks = Remarks + " Expense Amount: " + item3.ExpenseAmount;
							}
							if (item3.CommissionAmount > 0.0)
							{
								Remarks = Remarks + " Commission Amount: " + item3.CommissionAmount;
							}
						}
						else if (obj.DocumentTypeId == 126 || obj.DocumentTypeId == 133)
						{
							Remarks = obj.RemarksHeader + ",  Item: " + Conversion.ToString(find4[0].ItemName) + ",   Qty: " + item3.ItemQty + ",   Rate:" + item3.ItemRate + ",  Credit Amount: " + item3.FreightAmount + ",  Commission Amount: " + item3.CommissionAmount;
						}
						else if (obj.DocumentTypeId != 60 && obj.DocumentTypeId != 103 && obj.DocumentTypeId != 126 && obj.DocumentTypeId != 128 && obj.DocumentTypeId != 133 && obj.DocumentTypeId != 184 && obj.DocumentTypeId != 186 && obj.DocumentTypeId != 145)
						{
							if (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660)
							{
								if (item3.SaleOrder > 0)
								{
									Remarks = "Contract No. " + item3.SaleOrder + "  " + Conversion.ToDateTime(item3.OrderDate).ToString("dd-MMM-yyyy") + "  ";
								}
								Remarks = Remarks + Conversion.ToString(find4[0].ItemName) + " Qty " + Conversion.ToString(item3.ItemQty) + " @Rate. " + Conversion.ToString(item3.ItemRate);
								if (item3.ItemDiscount > 0.0 && item3.ItemDiscountAmount > 0.0)
								{
									Remarks = Remarks + " Discount " + Math.Round(item3.ItemDiscountAmount);
								}
								Remarks = Remarks + " NetAmount " + Conversion.ToString(item3.ItemAmount);
								if (obj.PaymentTermId == 2)
								{
									Remarks = Remarks + "  Term " + obj.DueDays + " days Credit   DueDate " + Conversion.ToDateTime(obj.DueDate).ToString("dd-MMM-yyyy");
								}
								else if (obj.PaymentTermId == 1)
								{
									Remarks += " Term Cash";
								}
								if (item3.GpNo != 0)
								{
									Remarks = Remarks + " Gp # " + item3.GpNo;
								}
								if (item3.VehicleNo != null && Conversion.ToString(item3.VehicleNo) != string.Empty && Conversion.ToString(item3.VehicleNo) != "0")
								{
									Remarks = Remarks + " Vehicle # " + item3.VehicleNo;
								}
							}
							else if ((obj.DocumentTypeId != 1608 && obj.DocumentTypeId != 1609 && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1661) || obj.DocumentTypeId == 1660)
							{
								if (obj.RemarksHeader != string.Empty)
								{
									Remarks = obj.RemarksHeader + " ";
								}
								Remarks = Remarks + "Sale " + Conversion.ToString(item3.ItemQty) + " Bags  ( " + Conversion.ToString(item3.GrossWeight);
								if (item3.EBTotalWt > 0.0)
								{
									Remarks = Remarks + " , " + Math.Round(item3.EBTotalWt, 3);
								}
								Remarks = Remarks + " , " + Conversion.ToString(item3.NetBillWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + "  " + Math.Round(Mond2, 2) + " Mound @ NetRate. " + Conversion.ToString(item3.ItemRate) + "   = Rs. " + Conversion.ToString(item3.ItemAmount);
								if (item3.ItemDiscount > 0.0 && item3.ItemDiscountAmount > 0.0)
								{
									Remarks = Remarks + " Discount% " + Conversion.ToString(item3.ItemDiscount) + " DiscountAmount " + Math.Round(item3.ItemDiscountAmount);
								}
								if (item3.RateCut != 0.0)
								{
									Remarks = Remarks + " RateCut " + Conversion.ToString(item3.RateCut) + " RateCutAmount " + Math.Round(item3.RateCutAmount);
								}
								if (item3.CommissionAmount > 0.0)
								{
									Remarks = Remarks + " CommType " + Conversion.ToString(obj.CommissionType) + " Commission paid @ " + Conversion.ToString(obj.CommRate) + " = Rs. " + Math.Round(item3.CommissionAmount);
								}
								if (item3.FreightAmount > 0.0)
								{
									Remarks = Remarks + " Freight Outward = Rs. " + Math.Round(item3.FreightAmount);
								}
								if (item3.VehicleNo != null && Conversion.ToString(item3.VehicleNo) != string.Empty && Conversion.ToString(item3.VehicleNo) != "0")
								{
									Remarks = Remarks + " Vehicle # " + item3.VehicleNo;
								}
								if (item3.GpNo != 0)
								{
									Remarks = Remarks + " Gp # " + item3.GpNo;
								}
								if (item3.ExpenseAmount > 0.0)
								{
									Remarks = Remarks + " Expense Amount = Rs. " + Math.Round(item3.ExpenseAmount);
								}
								if (item3.SaleOrder > 0)
								{
									Remarks = Remarks + " Under contract # " + item3.SaleOrder;
								}
								if (obj.ManualBillNo != string.Empty)
								{
									Remarks = Remarks + " Manual # " + obj.ManualBillNo;
								}
								Remarks = Remarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
							}
						}
						if (!returnDocumentTypeIds.Contains(obj.DocumentTypeId))
						{
							if (!Foc && !Zakat)
							{
								VoucherDetail vd18 = new VoucherDetail();
								vd18.LineId = 0;
								vd18.AccountId = Conversion.ToInt(vh.RefAccountId);
								if (obj.DocumentTypeId == 60 || obj.DocumentTypeId == 145)
								{
									vd18.LineId = item3.LineId;
									vd18.AgainstAccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find4[0].PurchaseGLAC));
								}
								else if (obj.DocumentTypeId == 222)
								{
									vd18.AgainstAccountId = obj.CommonIdsForFinancials.StockPartyGlAccountId;
								}
								else
								{
									vd18.AgainstAccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find4[0].SaleGLAC));
								}
								vd18.Comments = Conversion.ToString(Remarks);
								vd18.DebitAmount = Conversion.ToDouble(item3.ItemAmount);
								vd18.ItemId = Conversion.ToInt(item3.ItemId);
								vd18.QtyOut = Conversion.ToDouble(item3.ItemQty);
								vd18.ItemRate = Conversion.ToDouble(item3.ItemRate);
								vd18.WeightOut = Conversion.ToDouble(item3.NetBillWeight);
								vd18.RateCut = Conversion.ToDouble(item3.RateCut);
								vd18.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
								vd18.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
								vd18.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
								vd18.Freight = Conversion.ToDouble(item3.FreightAmount);
								vd18.Commission = Conversion.ToDouble(item3.CommissionAmount);
								vd18.OrderNo = Conversion.ToInt(item3.SaleOrder);
								vd18.GpNo = Conversion.ToInt(item3.GpNo);
								vd18.JobLotId = Conversion.ToInt(item3.JobLotId);
								vd18.VehicleNo = item3.VehicleNo;
								vd18.DMultiCurrencyId = item3.CurrencyId;
								vd18.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
								vd18.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
								vd18.SubsidiaryTypeId = 1;
								vd18.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd18.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd18.BranchesId = item3.BranchId;
								vd18.CostCenterId = item3.CostCenterId;
								vh.voucherDetailList.Add(vd18);
								VoucherDetail vd17 = new VoucherDetail();
								vd17.LineId = 0;
								if (obj.DocumentTypeId == 60 || obj.DocumentTypeId == 145)
								{
									vd17.LineId = item3.LineId;
									vd17.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find4[0].PurchaseGLAC));
								}
								else if (obj.DocumentTypeId == 222)
								{
									vd17.AccountId = obj.CommonIdsForFinancials.StockPartyGlAccountId;
								}
								else
								{
									vd17.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find4[0].SaleGLAC));
								}
								vd17.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd17.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
								vd17.CreditAmount = Conversion.ToDouble(item3.ItemAmount);
								vd17.ItemId = Conversion.ToInt(item3.ItemId);
								vd17.QtyOut = Conversion.ToDouble(item3.ItemQty);
								vd17.ItemRate = Conversion.ToDouble(item3.ItemRate);
								vd17.WeightOut = Conversion.ToDouble(item3.NetBillWeight);
								vd17.RateCut = Conversion.ToDouble(item3.RateCut);
								vd17.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
								vd17.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
								vd17.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
								vd17.Commission = Conversion.ToDouble(item3.CommissionAmount);
								vd17.Freight = Conversion.ToDouble(item3.FreightAmount);
								vd17.OrderNo = Conversion.ToInt(item3.SaleOrder);
								vd17.GpNo = Conversion.ToInt(item3.GpNo);
								vd17.VehicleNo = item3.VehicleNo;
								vd17.JobLotId = Conversion.ToInt(item3.JobLotId);
								vd17.SubsidiaryAgainstTypeId = 1;
								vd17.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
								vd17.DMultiCurrencyId = item3.CurrencyId;
								vd17.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
								vd17.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
								vd17.BranchesId = item3.BranchId;
								vd17.CostCenterId = item3.CostCenterId;
								vh.voucherDetailList.Add(vd17);
							}
							if (obj.DocumentTypeId == 222 && obj.TransTypeId == 2)
							{
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find4[0].PurchaseGLAC), Conversion.ToInt(find4[0].SaleGLAC), item3.ItemAmount, null, isQtyOut: false, item3, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find4[0].SaleGLAC), Conversion.ToInt(find4[0].PurchaseGLAC), null, item3.ItemAmount, isQtyOut: false, item3, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find4[0].COGSGLAC), Conversion.ToInt(find4[0].PurchaseGLAC), item3.ItemAmount, null, isQtyOut: true, item3, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
								vh.voucherDetailList.Add(AddVoucherDetail(Conversion.ToInt(find4[0].PurchaseGLAC), Conversion.ToInt(find4[0].COGSGLAC), null, item3.ItemAmount, isQtyOut: true, item3, Remarks, obj.SupplierCustomerId, obj.CommonIdsForFinancials.CompanyName));
							}
						}
						if (returnDocumentTypeIds.Contains(obj.DocumentTypeId))
						{
							VoucherDetail vd30 = new VoucherDetail();
							vd30.LineId = 0;
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd30.AccountId = objlot2.AccountId;
							}
							else
							{
								vd30.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							}
							vd30.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd30.Comments = Conversion.ToString(Remarks);
							vd30.DebitAmount = Conversion.ToDouble(item3.ItemAmount);
							vd30.ItemId = Conversion.ToInt(item3.ItemId);
							vd30.QtyIn = Conversion.ToDouble(item3.ItemQty);
							vd30.ItemRate = Conversion.ToDouble(item3.ItemRate);
							vd30.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
							vd30.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
							vd30.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
							vd30.Freight = Conversion.ToDouble(item3.FreightAmount);
							vd30.Commission = Conversion.ToDouble(item3.CommissionAmount);
							vd30.OrderNo = Conversion.ToInt(item3.SaleOrder);
							vd30.GpNo = Conversion.ToInt(item3.GpNo);
							vd30.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd30.VehicleNo = item3.VehicleNo;
							vd30.DMultiCurrencyId = item3.CurrencyId;
							vd30.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
							vd30.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
							vd30.SubsidiaryAgainstTypeId = 1;
							vd30.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd30.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd30);
							VoucherDetail vd16 = new VoucherDetail();
							vd16.LineId = 0;
							vd16.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd16.AgainstAccountId = objlot2.AccountId;
							}
							else
							{
								vd16.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							}
							vd16.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd16.CreditAmount = Conversion.ToDouble(item3.ItemAmount);
							vd16.ItemId = Conversion.ToInt(item3.ItemId);
							vd16.QtyIn = Conversion.ToDouble(item3.ItemQty);
							vd16.ItemRate = Conversion.ToDouble(item3.ItemRate);
							vd16.WeightIn = Conversion.ToDouble(item3.NetBillWeight);
							vd16.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
							vd16.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
							vd16.Commission = Conversion.ToDouble(item3.CommissionAmount);
							vd16.Freight = Conversion.ToDouble(item3.FreightAmount);
							vd16.OrderNo = Conversion.ToInt(item3.SaleOrder);
							vd16.GpNo = Conversion.ToInt(item3.GpNo);
							vd16.VehicleNo = item3.VehicleNo;
							vd16.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd16.DMultiCurrencyId = item3.CurrencyId;
							vd16.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
							vd16.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
							vd16.SubsidiaryTypeId = 1;
							vd16.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd16.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd16.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd16);
						}
						if (item3.ItemDiscountAmount > 0.0 && obj.DiscountAccountId > 0 && obj.DocumentTypeId != 1608 && obj.DocumentTypeId != 1609 && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1661 && obj.DocumentTypeId != 1662 && obj.DocumentTypeId != 1660 && obj.DocumentTypeId != 184 && obj.DocumentTypeId != 186)
						{
							if (obj.DiscountAccountId == 0)
							{
								throw new Exception("Discount Allowed Account Not Found");
							}
							VoucherDetail vd13 = new VoucherDetail();
							vd13.LineId = 0;
							vd13.AccountId = Conversion.ToInt(obj.DiscountAccountId);
							vd13.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd13.Comments = Conversion.ToString(Remarks);
							vd13.DebitAmount = Conversion.ToDouble(item3.ItemDiscountAmount);
							vd13.ItemId = Conversion.ToInt(item3.ItemId);
							vd13.QtyOut = Conversion.ToDouble(item3.ItemQty);
							vd13.ItemRate = Conversion.ToDouble(item3.ItemRate);
							vd13.WeightOut = Conversion.ToDouble(item3.NetBillWeight);
							vd13.RateCut = Conversion.ToDouble(item3.RateCut);
							vd13.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
							vd13.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
							vd13.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
							vd13.Freight = Conversion.ToDouble(item3.FreightAmount);
							vd13.Commission = Conversion.ToDouble(item3.CommissionAmount);
							vd13.OrderNo = Conversion.ToInt(item3.SaleOrder);
							vd13.GpNo = Conversion.ToInt(item3.GpNo);
							vd13.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd13.VehicleNo = item3.VehicleNo;
							vd13.DMultiCurrencyId = item3.CurrencyId;
							vd13.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
							vd13.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
							vd13.SubsidiaryAgainstTypeId = 1;
							vd13.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd13.BranchesId = item3.BranchId;
							vd13.CostCenterId = item3.CostCenterId;
							vh.voucherDetailList.Add(vd13);
							VoucherDetail vd15 = new VoucherDetail();
							vd15.LineId = 0;
							vd15.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd15.AgainstAccountId = Conversion.ToInt(obj.DiscountAccountId);
							vd15.Comments = Conversion.ToString(Remarks + "  " + obj.CommonIdsForFinancials.CompanyName);
							vd15.CreditAmount = Conversion.ToDouble(item3.ItemDiscountAmount);
							vd15.ItemId = Conversion.ToInt(item3.ItemId);
							vd15.QtyOut = Conversion.ToDouble(item3.ItemQty);
							vd15.ItemRate = Conversion.ToDouble(item3.ItemRate);
							vd15.WeightOut = Conversion.ToDouble(item3.NetBillWeight);
							vd15.RateCut = Conversion.ToDouble(item3.RateCut);
							vd15.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
							vd15.ItemAmount = Conversion.ToDouble(item3.ItemAmount);
							vd15.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
							vd15.Commission = Conversion.ToDouble(item3.CommissionAmount);
							vd15.Freight = Conversion.ToDouble(item3.FreightAmount);
							vd15.OrderNo = Conversion.ToInt(item3.SaleOrder);
							vd15.GpNo = Conversion.ToInt(item3.GpNo);
							vd15.VehicleNo = item3.VehicleNo;
							vd15.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd15.DMultiCurrencyId = item3.CurrencyId;
							vd15.DExchangeCurrencyRate = Conversion.ToDouble(item3.ExchangeRate);
							vd15.DCurrencyAmount = Conversion.ToDouble(item3.FcyAmount);
							vd15.SubsidiaryTypeId = 1;
							vd15.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd15.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd15.BranchesId = item3.BranchId;
							vd15.CostCenterId = item3.CostCenterId;
							vh.voucherDetailList.Add(vd15);
						}
						if (obj.DocumentTypeId != 60 && obj.DocumentTypeId != 1611 && obj.DocumentTypeId != 1662 && obj.DocumentTypeId != 145 && obj.DocumentTypeId != 222 && objlot2 != null && objlot2.AccountId == 0)
						{
							bool SaleCostingJobOrderWise = false;
							bool FIFOCgs = Architecture.BLL.TaxProject.Common.CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 5);
							bool num = Conversion.ToBool(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow"));
							SaleCostingJobOrderWise = Conversion.ToBool(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "SaleCostingJobOrderWise"));
							int FOCInventoryExpenseAccount = (Foc ? Conversion.ToInt(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "FOCInventoryExpenseAccount")) : 0);
							int ZakatInventoryExpensesAccount = (Zakat ? Conversion.ToInt(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ZakatInventoryExpensesAccount")) : 0);
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
								bool ItemWiseCGS = Architecture.BLL.TaxProject.Common.CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 1);
								bool Item_Job_Wise = Architecture.BLL.TaxProject.Common.CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 2);
								bool CGSLoaderWise = Architecture.BLL.TaxProject.Common.CommonServies.GetERPFeatureById(obj.OrganizationId, obj.CompanyId, 3);
								if (FIFOCgs && LoaderFlag)
								{
									CGSLoaderWise = true;
								}
								if (CGSLoaderWise)
								{
									if (item3.RefRefDocumentTypeId <= 0 || item3.RefRefDocIdNo <= 0)
									{
										throw new Exception("RefIds Not Found");
									}
									DataTable dt = Architecture.BLL.TaxProject.Common.CommonServies.GetAvgRateFromAvaiblableLoaderStock(new ReportsParameters
									{
										OrganizationId = obj.OrganizationId,
										CompanyId = obj.CompanyId,
										RefDocumentTypeId = item3.RefRefDocumentTypeId,
										RefRefDocIdNo = item3.RefRefDocIdNo,
										RefDocSubId = item3.RefDocSubId
									});
									if (dt.Rows.Count > 0)
									{
										item3.ItemCgsRate = Conversion.ToDouble(dt.Rows[0]["AvgRate"]);
										item3.RateUOM = Conversion.ToDouble(dt.Rows[0]["RateUom"]);
										item3.CgsRateUomId = Conversion.ToInt(dt.Rows[0]["RateUomId"]);
									}
									if (item3.ItemCgsRate <= 0.0)
									{
										throw new Exception("CGS Rate not found");
									}
								}
								else if (ItemWiseCGS || Item_Job_Wise)
								{
									if (SaleCostingJobOrderWise)
									{
										ReportsParameters parameters = new ReportsParameters();
										parameters.OrganizationId = obj.OrganizationId;
										parameters.CompanyId = obj.CompanyId;
										parameters.DocDate = obj.DocDate;
										parameters.ItemId = item3.ItemId;
										parameters.JobLotId = item3.JobLotId;
										if (obj.Id > 0)
										{
											parameters.DocumentTypeId = obj.DocumentTypeId;
											parameters.RecId = obj.Id;
										}
										item3.ItemCgsRate = Architecture.BLL.TaxProject.Common.CommonServies.getAvgRateByJobOrder(parameters);
										if (item3.ItemCgsRate <= 0.0)
										{
											item3.ItemCgsRate = 1.0;
										}
									}
									else
									{
										ReportsParameters parameters2 = new ReportsParameters();
										parameters2.OrganizationId = obj.OrganizationId;
										parameters2.CompanyId = obj.CompanyId;
										parameters2.ItemId = item3.ItemId;
										parameters2.DocDate = obj.DocDate;
										parameters2.JobLotId = item3.JobLotId;
										parameters2.CropYear = item3.CropYear;
										parameters2.WarehouseId = item3.WarehouseId;
										if (obj.Id > 0)
										{
											parameters2.DocumentTypeId = obj.DocumentTypeId;
											parameters2.Id = obj.Id;
										}
										item3.ItemCgsRate = Architecture.BLL.Common.CommonServies.AvgRateOnlyForCGS(parameters2);
										if (item3.ItemCgsRate <= 0.0)
										{
											throw new Exception("CGS Rate not found.for Item " + item3.ItemName);
										}
									}
								}
								if (CGSLoaderWise || ItemWiseCGS || Item_Job_Wise)
								{
									double CgsAmount = 0.0;
									string RemarksCgs = "";
									if (obj.DocumentTypeId == 128)
									{
										RemarksCgs = "NoOfBags: " + item3.ItemQty + "   " + Conversion.ToString(find4[0].ItemName) + "  Net Weight: " + item3.NetBillWeight + "  Rate:" + item3.ItemCgsRate;
										if (item3.ExpenseAmount > 0.0)
										{
											RemarksCgs = RemarksCgs + "   Expense Amount: " + item3.ExpenseAmount;
										}
										if (item3.CommissionAmount > 0.0)
										{
											RemarksCgs = RemarksCgs + "    Commission Amount: " + item3.CommissionAmount;
										}
										double PackUom = Architecture.BLL.TaxProject.Common.CommonServies.GetEqvilentByItemId(new ReportsParameters
										{
											OrganizationId = obj.OrganizationId,
											CompanyId = obj.CompanyId,
											ItemId = item3.ItemId
										});
										item3.ItemCgsRate *= PackUom;
										CgsAmount = item3.ItemCgsRate * item3.ItemQty;
									}
									else if (obj.DocumentTypeId == 126 || obj.DocumentTypeId == 133 || obj.DocumentTypeId == 184)
									{
										RemarksCgs = "ItemQty: " + item3.ItemQty + "   " + Conversion.ToString(find4[0].ItemName) + " Net Weight: " + item3.NetBillWeight + " Rate: " + item3.ItemCgsRate;
										if (item3.ItemDiscountAmount > 0.0)
										{
											RemarksCgs = RemarksCgs + " Discount Amount: " + item3.ItemDiscountAmount;
										}
										if (item3.FreightAmount > 0.0)
										{
											RemarksCgs = RemarksCgs + " Credit Amount: " + item3.FreightAmount;
										}
										if (item3.CommissionAmount > 0.0)
										{
											RemarksCgs = RemarksCgs + " Commission Amount: " + item3.CommissionAmount;
										}
										CgsAmount = item3.ItemCgsRate * item3.ItemQty;
									}
									else if (obj.DocumentTypeId != 128 && obj.DocumentTypeId != 126 && obj.DocumentTypeId != 133 && obj.DocumentTypeId != 184)
									{
										RemarksCgs = "NoOfBags: " + item3.ItemQty + "   " + Conversion.ToString(find4[0].ItemName) + "  GrossWeight: " + item3.GrossWeight + "  Net Weight: " + item3.NetBillWeight + "  Rate:" + item3.ItemCgsRate;
										if (item3.EBTotalWt > 0.0)
										{
											RemarksCgs = RemarksCgs + "   EmptyBagsDeduction: " + item3.EBTotalWt;
										}
										if (item3.ExpenseAmount > 0.0)
										{
											RemarksCgs = RemarksCgs + "   Expense Amount: " + item3.ExpenseAmount;
										}
										if (item3.CommissionAmount > 0.0)
										{
											RemarksCgs = RemarksCgs + "    Commission Amount: " + item3.CommissionAmount;
										}
										if (item3.NetBillWeight == 0.0)
										{
											throw new Exception("NetBillWeight Not Found Please Check");
										}
										if (item3.RateUOM == 0.0)
										{
											throw new Exception("RateUOM Not Found Please Check");
										}
									}
									if (obj.DocumentTypeId != 128 && obj.DocumentTypeId != 126 && obj.DocumentTypeId != 133 && item3.ItemCgsRate > 0.0 && item3.NetBillWeight > 0.0)
									{
										if (!CGSLoaderWise)
										{
											ReportsParameters Uom = new ReportsParameters();
											Uom.OrganizationId = obj.OrganizationId;
											Uom.CompanyId = obj.CompanyId;
											Uom.ItemId = item3.ItemId;
											Uom.Id = item3.UomScheduleIdRate;
											item3.RateUOM = Architecture.BLL.TaxProject.Common.CommonServies.GetEqvilentByItemIdAndUomScheduleId(Uom);
											item3.ItemCgsRate = item3.RateUOM * item3.ItemCgsRate;
										}
										CgsAmount = item3.NetStockWeight / item3.RateUOM * item3.ItemCgsRate;
									}
									VoucherDetail vd29 = new VoucherDetail();
									vd29.LineId = item3.LineId;
									vd29.AccountId = ((FOCInventoryExpenseAccount > 0) ? FOCInventoryExpenseAccount : ((ZakatInventoryExpensesAccount > 0) ? ZakatInventoryExpensesAccount : Conversion.ToInt(find4[0].COGSGLAC)));
									vd29.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
									vd29.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
									vd29.DebitAmount = Conversion.ToDouble(CgsAmount);
									vd29.DocumentTypeIdRef = Conversion.ToInt(item3.RefRefDocumentTypeId);
									vd29.InvoiceNoRefId = Conversion.ToInt(item3.RefRefDocIdNo);
									vd29.RefInvoiceNo = Conversion.ToString(item3.RefDocSubId);
									vd29.ItemId = Conversion.ToInt(item3.ItemId);
									vd29.QtyOut = Conversion.ToDouble(item3.ItemQty);
									vd29.WeightOut = Conversion.ToDouble(item3.NetBillWeight);
									vd29.ItemCgsRate = Conversion.ToDouble(item3.ItemCgsRate);
									vd29.RateCut = Conversion.ToDouble(item3.RateCut);
									vd29.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
									vd29.ItemAmount = Conversion.ToDouble(CgsAmount);
									vd29.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
									vd29.Commission = Conversion.ToDouble(item3.CommissionAmount);
									vd29.Freight = Conversion.ToDouble(item3.FreightAmount);
									vd29.OrderNo = Conversion.ToInt(item3.SaleOrder);
									vd29.GpNo = Conversion.ToInt(item3.GpNo);
									vd29.VehicleNo = item3.VehicleNo;
									vd29.JobLotId = Conversion.ToInt(item3.JobLotId);
									vd29.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vd29.BranchesId = item3.BranchId;
									vd29.IsCGS = 1;
									vd29.CostCenterId = item3.CostCenterId;
									vh.voucherDetailList.Add(vd29);
									VoucherDetail vd28 = new VoucherDetail();
									vd28.LineId = item3.LineId;
									vd28.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
									vd28.AgainstAccountId = ((FOCInventoryExpenseAccount > 0) ? FOCInventoryExpenseAccount : ((ZakatInventoryExpensesAccount > 0) ? ZakatInventoryExpensesAccount : Conversion.ToInt(find4[0].COGSGLAC)));
									vd28.Comments = Conversion.ToString(RemarksCgs + "  " + obj.CommonIdsForFinancials.CompanyName);
									vd28.CreditAmount = Conversion.ToDouble(CgsAmount);
									vd28.DocumentTypeIdRef = Conversion.ToInt(item3.RefRefDocumentTypeId);
									vd28.InvoiceNoRefId = Conversion.ToInt(item3.RefRefDocIdNo);
									vd28.RefInvoiceNo = Conversion.ToString(item3.RefDocSubId);
									vd28.ItemId = Conversion.ToInt(item3.ItemId);
									vd28.QtyOut = Conversion.ToDouble(item3.ItemQty);
									vd28.WeightOut = Conversion.ToDouble(item3.NetBillWeight);
									vd28.ItemCgsRate = Conversion.ToDouble(item3.ItemCgsRate);
									vd28.RateCut = Conversion.ToDouble(item3.RateCut);
									vd28.RateCutAmount = Conversion.ToDouble(item3.RateCutAmount);
									vd28.ItemAmount = Conversion.ToDouble(CgsAmount);
									vd28.Expenses = Conversion.ToDouble(item3.ExpenseAmount);
									vd28.Commission = Conversion.ToDouble(item3.CommissionAmount);
									vd28.Freight = Conversion.ToDouble(item3.FreightAmount);
									vd28.OrderNo = Conversion.ToInt(item3.SaleOrder);
									vd28.GpNo = Conversion.ToInt(item3.GpNo);
									vd28.VehicleNo = item3.VehicleNo;
									vd28.JobLotId = Conversion.ToInt(item3.JobLotId);
									vd28.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vd28.BranchesId = item3.BranchId;
									vd28.IsCGS = 1;
									vd28.CostCenterId = item3.CostCenterId;
									vh.voucherDetailList.Add(vd28);
								}
							}
						}
						if ((obj.DocumentTypeId == 95 || obj.DocumentTypeId == 96 || obj.DocumentTypeId == 103 || obj.DocumentTypeId == 126 || obj.DocumentTypeId == 128 || obj.DocumentTypeId == 133 || obj.DocumentTypeId == 171 || obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660 || obj.DocumentTypeId == 145 || obj.DocumentTypeId == 99 || obj.DocumentTypeId == 222) && item3.FreightAmount > 0.0 && Conversion.ToInt(obj.TransporterDebitGLId) == 0 && !Foc && !Zakat)
						{
							string FreightRemarks = "";
							FreightRemarks = ((obj.DocumentTypeId != 145) ? ("Sale " + Conversion.ToString(item3.ItemQty) + " Qty  ( " + Conversion.ToString(item3.NetBillWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + "  " + Math.Round(Mond2, 2) + " Mond @ NetRate. " + Conversion.ToString(item3.ItemRate) + "   = Rs. " + Conversion.ToString(item3.ItemAmount)) : ("Purchase Return " + Conversion.ToString(item3.ItemQty) + " Qty of " + Conversion.ToString(find4[0].ItemName) + " @ NetRate. " + Conversion.ToString(item3.ItemRate) + " = Rs. " + Conversion.ToString(item3.ItemAmount)));
							FreightRemarks = FreightRemarks + " Freight " + Conversion.ToString(item3.FreightAmount);
							if (item3.SaleOrder > 0)
							{
								FreightRemarks = FreightRemarks + " Under contract # " + item3.SaleOrder;
							}
							if (item3.VehicleNo != string.Empty)
							{
								FreightRemarks = FreightRemarks + " Vehicle # " + item3.VehicleNo;
							}
							if (item3.GpNo != 0)
							{
								FreightRemarks = FreightRemarks + " Gp # " + item3.GpNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								FreightRemarks = FreightRemarks + " Manual # " + obj.ManualBillNo;
							}
							FreightRemarks = FreightRemarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
							VoucherDetail vd25 = new VoucherDetail();
							if (obj.DocumentTypeId == 1611 || obj.DocumentTypeId == 145)
							{
								vd25.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find4[0].PurchaseGLAC));
							}
							else
							{
								vd25.AccountId = ((objlot2 != null && objlot2.AccountId > 0) ? objlot2.AccountId : Conversion.ToInt(find4[0].SaleGLAC));
							}
							vd25.AgainstAccountId = vd25.AccountId;
							vd25.Comments = FreightRemarks;
							vd25.DebitAmount = Conversion.ToDouble(item3.FreightAmount);
							vd25.CreditAmount = Conversion.ToDouble(0);
							vd25.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd25.BranchesId = item3.BranchId;
							vd25.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
							vh.voucherDetailList.Add(vd25);
						}
						if ((obj.DocumentTypeId == 95 || obj.DocumentTypeId == 99 || obj.DocumentTypeId == 103 || obj.DocumentTypeId == 128 || obj.DocumentTypeId == 133 || obj.DocumentTypeId == 139 || obj.DocumentTypeId == 171 || obj.DocumentTypeId == 184 || obj.DocumentTypeId == 186 || obj.DocumentTypeId == 1809 || obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1661 || obj.DocumentTypeId == 1660 || obj.DocumentTypeId == 222) && !Conversion.ToBool(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")) && item3.ExpenseAmount > 0.0)
						{
							string ExpeneseRemarks2 = "";
							ExpeneseRemarks2 = "Sale " + Conversion.ToString(item3.ItemQty) + " Bags  ( " + Conversion.ToString(item3.NetBillWeight) + ")  of " + Conversion.ToString(find4[0].ItemName) + "  " + Math.Round(Mond2, 2) + " Mond @ NetRate. " + Conversion.ToString(item3.ItemRate) + "   = Rs. " + Conversion.ToString(item3.ItemAmount);
							ExpeneseRemarks2 = ExpeneseRemarks2 + " Other Exp " + Conversion.ToString(item3.ExpenseAmount);
							if (item3.SaleOrder > 0)
							{
								ExpeneseRemarks2 = ExpeneseRemarks2 + " Under contract # " + item3.SaleOrder;
							}
							if (item3.VehicleNo != string.Empty)
							{
								ExpeneseRemarks2 = ExpeneseRemarks2 + " Vehicle # " + item3.VehicleNo;
							}
							if (item3.GpNo != 0)
							{
								ExpeneseRemarks2 = ExpeneseRemarks2 + " Gp # " + item3.GpNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								ExpeneseRemarks2 = ExpeneseRemarks2 + " Manual # " + obj.ManualBillNo;
							}
							ExpeneseRemarks2 = ExpeneseRemarks2 + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
							VoucherDetail vd24 = new VoucherDetail();
							vd24.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd24.AgainstAccountId = objlot2.AccountId;
							}
							else
							{
								vd24.AgainstAccountId = Conversion.ToInt(find4[0].SaleGLAC);
							}
							vd24.DebitAmount = Conversion.ToDouble(item3.ExpenseAmount);
							vd24.Comments = ExpeneseRemarks2;
							vd24.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd24.SubsidiaryTypeId = 1;
							vd24.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd24.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd24.BranchesId = item3.BranchId;
							vd24.CostCenterId = item3.CostCenterId;
							vh.voucherDetailList.Add(vd24);
							VoucherDetail vd20 = new VoucherDetail();
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd20.AccountId = objlot2.AccountId;
							}
							else
							{
								vd20.AccountId = Conversion.ToInt(find4[0].SaleGLAC);
							}
							vd20.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd20.CreditAmount = Conversion.ToDouble(item3.ExpenseAmount);
							vd20.Comments = ExpeneseRemarks2;
							vd20.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd20.SubsidiaryAgainstTypeId = 1;
							vd20.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd20.BranchesId = item3.BranchId;
							vd20.CostCenterId = item3.CostCenterId;
							vh.voucherDetailList.Add(vd20);
						}
						if (obj.DocumentTypeId == 145 && item3.ExpenseAmount > 0.0)
						{
							string ExpeneseRemarks = "";
							ExpeneseRemarks = "Purchase Return " + Conversion.ToString(find4[0].ItemName) + " Qty " + Conversion.ToString(item3.ItemQty) + " @ NetRate. " + Conversion.ToString(item3.ItemRate) + "   = Rs. " + Conversion.ToString(item3.ItemAmount);
							ExpeneseRemarks = ExpeneseRemarks + " Other Exp " + Conversion.ToString(item3.ExpenseAmount);
							if (item3.SaleOrder > 0)
							{
								ExpeneseRemarks = ExpeneseRemarks + " Under contract # " + item3.SaleOrder;
							}
							if (item3.VehicleNo != string.Empty)
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
							VoucherDetail vd23 = new VoucherDetail();
							vd23.AccountId = Conversion.ToInt(vh.RefAccountId);
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd23.AgainstAccountId = objlot2.AccountId;
							}
							else
							{
								vd23.AgainstAccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							}
							vd23.DebitAmount = Conversion.ToDouble(item3.ExpenseAmount);
							vd23.Comments = ExpeneseRemarks;
							vd23.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd23.SubsidiaryTypeId = 1;
							vd23.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd23.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vd23.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd23);
							VoucherDetail vd19 = new VoucherDetail();
							if (objlot2 != null && objlot2.AccountId > 0)
							{
								vd19.AccountId = objlot2.AccountId;
							}
							else
							{
								vd19.AccountId = Conversion.ToInt(find4[0].PurchaseGLAC);
							}
							vd19.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd19.CreditAmount = Conversion.ToDouble(item3.ExpenseAmount);
							vd19.Comments = ExpeneseRemarks;
							vd19.JobLotId = Conversion.ToInt(item3.JobLotId);
							vd19.SubsidiaryAgainstTypeId = 1;
							vd19.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
							vd19.BranchesId = item3.BranchId;
							vh.voucherDetailList.Add(vd19);
						}
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				throw new Exception("Item Record Not found");
			}
			if (((obj.InvSaleInvoiceCommission.Count > 0 && obj.DocumentTypeId == 95) || obj.DocumentTypeId == 99 || obj.DocumentTypeId == 103 || obj.DocumentTypeId == 60 || obj.DocumentTypeId == 126 || obj.DocumentTypeId == 128 || obj.DocumentTypeId == 133 || obj.DocumentTypeId == 139 || obj.DocumentTypeId == 171 || obj.DocumentTypeId == 1809 || obj.DocumentTypeId == 222) && !Foc && !Zakat)
			{
				foreach (InvSaleInvoiceDetail item2 in obj.InvSaleInvoiceDetaillist.ToList())
				{
					_ = string.Empty;
					double Mond = Conversion.ToDouble(item2.NetBillWeight) / 40.0;
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
						if (find2.Count > 0)
						{
							JobLot objlot = new JobLot();
							if (item2.JobLotId > 0)
							{
								objlot = Architecture.BLL.TaxProject.Common.CommonServies.JobLotGetByID(item2.JobLotId);
							}
							string CommissionRemarks2 = "";
							string CommissionType = "";
							string CommissionRate = "";
							if (item2.CommissionAmount > 0.0 && obj.CommissionDebitAcGLId == 0 && !DebitAmountChargetoExpenseAcOfCommission)
							{
								CommissionRemarks2 = "Sale " + Conversion.ToString(item2.ItemQty) + " Bags  ( " + Conversion.ToString(item2.NetBillWeight) + ")  of " + Conversion.ToString(find2[0].ItemName) + "  " + Math.Round(Mond, 2) + " Mond @ NetRate. " + Conversion.ToString(item2.ItemRate) + "   = Rs. " + Conversion.ToString(item2.ItemAmount);
								List<InvSaleInvoiceCommission> Cmgnt2 = obj.InvSaleInvoiceCommission.Where((InvSaleInvoiceCommission x) => x.SaleOrderId == Conversion.ToInt(item2.SaleOrderId)).ToList();
								if (Cmgnt2.Count > 0)
								{
									CommissionType = Conversion.ToString(Cmgnt2[0].CommType);
									CommissionRate = Conversion.ToString(Cmgnt2[0].Rate);
								}
								CommissionRemarks2 = CommissionRemarks2 + " CommType " + CommissionType + " Commission paid @ " + CommissionRate + " = Rs. " + Math.Round(item2.CommissionAmount);
								if (item2.SaleOrder > 0)
								{
									CommissionRemarks2 = CommissionRemarks2 + " Under contract # " + item2.SaleOrder;
								}
								if (item2.VehicleNo != string.Empty)
								{
									CommissionRemarks2 = CommissionRemarks2 + " Vehicle # " + item2.VehicleNo;
								}
								if (item2.GpNo != 0)
								{
									CommissionRemarks2 = CommissionRemarks2 + " Gp # " + item2.GpNo;
								}
								if (obj.ManualBillNo != string.Empty)
								{
									CommissionRemarks2 = CommissionRemarks2 + " Manual # " + obj.ManualBillNo;
								}
								CommissionRemarks2 = CommissionRemarks2 + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
								VoucherDetail vd22 = new VoucherDetail();
								if (objlot != null && objlot.AccountId > 0)
								{
									vd22.AccountId = objlot.AccountId;
								}
								else
								{
									vd22.AccountId = Conversion.ToInt(find2[0].SaleGLAC);
								}
								vd22.AgainstAccountId = vd22.AccountId;
								if (obj.CommissionRemarks != string.Empty)
								{
									vd22.Comments = CommissionRemarks2;
								}
								else
								{
									vd22.Comments = CommissionRemarks2 + Conversion.ToString(obj.CommissionRemarks);
								}
								vd22.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
								vd22.JobLotId = Conversion.ToInt(item2.JobLotId);
								vd22.BranchesId = item2.BranchId;
								vh.voucherDetailList.Add(vd22);
							}
							continue;
						}
						throw new Exception("Item Record Not found");
					}
					throw new Exception("Item Record Not found");
				}
				foreach (InvSaleInvoiceCommission item in obj.InvSaleInvoiceCommission.ToList())
				{
					_ = string.Empty;
					List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(item.CommissionAgentId)).ToList();
					if (find.Count > 0)
					{
						string CommissionRemarks = "";
						string OrderNo = "";
						string VehicleNo = "";
						string GpNo = "";
						double ItemQty = 0.0;
						double NetBillWeight = 0.0;
						double ItemAmount = 0.0;
						if (item.SaleOrderId > 0)
						{
							List<InvSaleInvoiceDetail> Cmgnt = obj.InvSaleInvoiceDetaillist.Where((InvSaleInvoiceDetail x) => x.SaleOrderId == Conversion.ToInt(item.SaleOrderId)).ToList();
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
						if (item.CommAmount > 0.0 && item.CommissionAgentId > 0 && DebitAmountChargetoExpenseAcOfCommission)
						{
							if (item.DebitAccountId == 0)
							{
								throw new Exception("Commission Debit Account Not Found in Commission Agent Grid");
							}
							string CommType2 = "";
							if (item.CommType == 1.0)
							{
								CommType2 = "Flat";
							}
							else if (item.CommType == 2.0)
							{
								CommType2 = "Percent";
							}
							else if (item.CommType == 3.0)
							{
								CommType2 = "OnWeight";
							}
							CommissionRemarks = "Sale  ItemQty " + Conversion.ToString(ItemQty) + " BillWeight " + Conversion.ToString(NetBillWeight) + " ItemAmount " + Conversion.ToString(ItemAmount);
							CommissionRemarks = CommissionRemarks + " CommType " + CommType2 + " Commission paid @ " + item.Rate + " = Rs. " + Math.Round(item.CommAmount);
							if (OrderNo != string.Empty)
							{
								CommissionRemarks = CommissionRemarks + " Under contract # " + OrderNo;
							}
							if (VehicleNo != string.Empty)
							{
								CommissionRemarks = CommissionRemarks + " Vehicle # " + VehicleNo;
							}
							if (GpNo != string.Empty)
							{
								CommissionRemarks = CommissionRemarks + " Gp # " + GpNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								CommissionRemarks = CommissionRemarks + " Manual # " + obj.ManualBillNo;
							}
							CommissionRemarks = CommissionRemarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
							VoucherDetail vd21 = new VoucherDetail();
							vd21.AccountId = Conversion.ToInt(item.DebitAccountId);
							vd21.AgainstAccountId = Conversion.ToInt(find[0].GlAccountId);
							if (item.Remarks == string.Empty)
							{
								vd21.Comments = CommissionRemarks;
							}
							else
							{
								vd21.Comments = CommissionRemarks + Conversion.ToString(item.Remarks);
							}
							vd21.DebitAmount = Conversion.ToDouble(item.CommAmount);
							vd21.SubsidiaryTypeId = 1;
							vd21.SubsidiaryAccountId = Conversion.ToInt(item.CommDebitSupCustId);
							vd21.SupplierCustomerId = Conversion.ToInt(item.CommDebitSupCustId);
							vd21.SubsidiaryAgainstTypeId = vd21.SubsidiaryTypeId;
							vd21.SubsidiaryAgainstAccountId = Conversion.ToInt(item.CommissionAgentId);
							vd21.BranchesId = obj.BranchesId;
							vd21.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
							vh.voucherDetailList.Add(vd21);
							VoucherDetail vd27 = new VoucherDetail();
							vd27.AccountId = Conversion.ToInt(find[0].GlAccountId);
							vd27.AgainstAccountId = Conversion.ToInt(item.DebitAccountId);
							if (item.Remarks == string.Empty)
							{
								vd27.Comments = CommissionRemarks;
							}
							else
							{
								vd27.Comments = CommissionRemarks + Conversion.ToString(item.Remarks);
							}
							vd27.CreditAmount = Conversion.ToDouble(item.CommAmount);
							vd27.SubsidiaryTypeId = 1;
							vd27.SubsidiaryAccountId = Conversion.ToInt(item.CommissionAgentId);
							vd27.SupplierCustomerId = Conversion.ToInt(item.CommissionAgentId);
							vd27.SubsidiaryTypeId = vd27.SubsidiaryTypeId;
							vd27.SubsidiaryAccountId = Conversion.ToInt(item.CommDebitSupCustId);
							vd27.BranchesId = obj.BranchesId;
							vd27.CostCenterId = obj.CommonIdsForFinancials.CostCenterId;
							vh.voucherDetailList.Add(vd27);
						}
						if (item.CommAmount > 0.0 && item.CommissionAgentId > 0 && !DebitAmountChargetoExpenseAcOfCommission)
						{
							string CommType = "";
							if (item.CommType == 1.0)
							{
								CommType = "Flat";
							}
							else if (item.CommType == 2.0)
							{
								CommType = "Percent";
							}
							else if (item.CommType == 3.0)
							{
								CommType = "OnWeight";
							}
							CommissionRemarks = "Sale  ItemQty " + Conversion.ToString(ItemQty) + " BillWeight " + Conversion.ToString(NetBillWeight) + " ItemAmount " + Conversion.ToString(ItemAmount);
							CommissionRemarks = CommissionRemarks + " CommType " + CommType + " Commission paid @ " + item.Rate + " = Rs. " + Math.Round(item.CommAmount);
							if (OrderNo != string.Empty && OrderNo != "0")
							{
								CommissionRemarks = CommissionRemarks + " Under contract # " + OrderNo;
							}
							if (VehicleNo != string.Empty)
							{
								CommissionRemarks = CommissionRemarks + " Vehicle # " + VehicleNo;
							}
							if (GpNo != string.Empty)
							{
								CommissionRemarks = CommissionRemarks + " Gp # " + GpNo;
							}
							if (obj.ManualBillNo != string.Empty)
							{
								CommissionRemarks = CommissionRemarks + " Manual # " + obj.ManualBillNo;
							}
							CommissionRemarks = CommissionRemarks + " from " + Conversion.ToString(obj.CommonIdsForFinancials.CompanyName);
							VoucherDetail vd26 = new VoucherDetail();
							vd26.AccountId = Conversion.ToInt(find[0].GlAccountId);
							vd26.AgainstAccountId = vd26.AccountId;
							if (item.Remarks == string.Empty)
							{
								vd26.Comments = CommissionRemarks;
							}
							else
							{
								vd26.Comments = CommissionRemarks + Conversion.ToString(item.Remarks);
							}
							vd26.CreditAmount = Conversion.ToDouble(item.CommAmount);
							vd26.SubsidiaryTypeId = 1;
							vd26.SubsidiaryAccountId = Conversion.ToInt(item.CommissionAgentId);
							vd26.SupplierCustomerId = Conversion.ToInt(item.CommissionAgentId);
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

	public static VoucherHead OtherExpenseFinancial(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj, List<InventoryItemsOther> dtItemOther)
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
		if (!Conversion.ToBool(Architecture.BLL.TaxProject.Common.CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")))
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
				InventoryItemsOther find = dtItemOther.FirstOrDefault((InventoryItemsOther x) => x.Id == Convert.ToInt32(item.InvRevExpItemId));
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
