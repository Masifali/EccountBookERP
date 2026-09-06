using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;
using Newtonsoft.Json;

namespace Architecture.BLL.Steel.Inventory.Financials.PurchaseInvoice;

public class PurchaseInvoiceGeneralFinancialMethods
{
	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.Steel.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1502 || obj.DocumentTypeId == 1503) && obj.InvPurchaseInvoicejournalList.Count > 0)
		{
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceJournal item in obj.InvPurchaseInvoicejournalList.ToList())
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
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		return vh;
	}

	public static VoucherHead FreightFinancial(Architecture.Model.Steel.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1502 || obj.DocumentTypeId == 1503) && obj.InvPurchaseInvoiceFreightList.Count > 0)
		{
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceFreight r in obj.InvPurchaseInvoiceFreightList.ToList())
			{
				if (Conversion.ToInt(r.TansporterId) > 0 && Conversion.ToDouble(r.CreditAmount) > 0.0)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = r.TansporterId;
					vd.AgainstAccountId = obj.CommonIdsForFinancials.OffsetAccountId;
					if (obj.CommonIdsForFinancials.AutoRemarksStatus)
					{
						vd.Comments = obj.RemarksHeader;
					}
					else
					{
						vd.Comments = Conversion.ToString(r.Remarks);
					}
					vd.DebitAmount = 0.0;
					vd.CreditAmount = Conversion.ToDouble(r.CreditAmount);
					vh.voucherDetailList.Add(vd);
				}
			}
		}
		return vh;
	}

	public static VoucherHead BrokeryBeHalfOnPartyFinancial(Architecture.Model.Steel.Inventory.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1502 || obj.DocumentTypeId == 1503) && obj.BrokerAgentId > 0 && obj.BrokeryAmount > 0m)
		{
			VoucherDetail vd2 = new VoucherDetail();
			vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
			vd2.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
			vd2.Comments = obj.RemarksHeader;
			vd2.DebitAmount = Conversion.ToDouble(obj.BrokeryAmount);
			vd2.CreditAmount = 0.0;
			vh.voucherDetailList.Add(vd2);
			VoucherDetail vd = new VoucherDetail();
			vd.AccountId = Conversion.ToInt(obj.CommonIdsForFinancials.BrokerGlAccountId);
			vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
			vd.Comments = obj.RemarksHeader;
			vd.DebitAmount = 0.0;
			vd.CreditAmount = Conversion.ToDouble(obj.BrokeryAmount);
			vh.voucherDetailList.Add(vd);
		}
		return vh;
	}

	public static VoucherHead ItemAndSupplierByWeightFinancial(Architecture.Model.Steel.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1502 || obj.DocumentTypeId == 1503) && obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "AutoRemarksFormatII"));
			List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail> itemListWithUniqueItems = new List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail>();
			List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail>>(JsonConvert.SerializeObject(obj.invPurchaseInvoiceDetailList.ToList()));
			for (int l = 0; l < dummyArray.Count; l++)
			{
				if (l == 0)
				{
					itemListWithUniqueItems.Add(dummyArray[l]);
				}
				if (l <= 0)
				{
					continue;
				}
				bool flagForListWithUniqueItem = false;
				for (int n = 0; n < itemListWithUniqueItems.Count; n++)
				{
					if (itemListWithUniqueItems[n].ItemId == dummyArray[l].ItemId && itemListWithUniqueItems[n].ItemRate == dummyArray[l].ItemRate && itemListWithUniqueItems[n].JobLotId == dummyArray[l].JobLotId && itemListWithUniqueItems[n].ActionTypeId != 3)
					{
						flagForListWithUniqueItem = true;
						break;
					}
				}
				if (!flagForListWithUniqueItem)
				{
					itemListWithUniqueItems.Add(dummyArray[l]);
				}
			}
			for (int k = 0; k < itemListWithUniqueItems.ToList().Count; k++)
			{
				decimal itemAmountAtI = default(decimal);
				decimal itemExpenseAmountAtI = default(decimal);
				decimal itemFreightAmountAtI = default(decimal);
				decimal itemCommissionAmountAtI = default(decimal);
				decimal itemQtyAtI = default(decimal);
				decimal itemWeightAtI = default(decimal);
				decimal itemWeightCutAtI = default(decimal);
				decimal itemRateCutAtI = default(decimal);
				decimal itemGrossWeightAtI = default(decimal);
				decimal itemBrokeryAmountAtI = default(decimal);
				for (int m = 0; m < dummyArray.Count; m++)
				{
					if (itemListWithUniqueItems[k].ItemId == dummyArray[m].ItemId && itemListWithUniqueItems[k].ItemRate == dummyArray[m].ItemRate && itemListWithUniqueItems[k].WarehouseId == dummyArray[m].WarehouseId && itemListWithUniqueItems[k].JobLotId == dummyArray[m].JobLotId && itemListWithUniqueItems[k].ActionTypeId != 3)
					{
						itemAmountAtI += dummyArray[m].ItemAmount;
						itemFreightAmountAtI += dummyArray[m].FreightAmount;
						itemExpenseAmountAtI += dummyArray[m].ExpenseAmount;
						itemCommissionAmountAtI += dummyArray[m].CommissionAmount;
						itemBrokeryAmountAtI += dummyArray[m].BrokeryAmount;
						itemQtyAtI += dummyArray[m].ItemQty;
						itemWeightAtI += dummyArray[m].NetBillWeight;
						itemWeightCutAtI += dummyArray[m].WcutTotal;
						itemRateCutAtI += dummyArray[m].RateCut;
						itemGrossWeightAtI += dummyArray[m].GrossWeight;
					}
				}
				itemListWithUniqueItems[k].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[k].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[k].FreightAmount = itemFreightAmountAtI;
				itemListWithUniqueItems[k].CommissionAmount = itemCommissionAmountAtI;
				itemListWithUniqueItems[k].BrokeryAmount = itemBrokeryAmountAtI;
				itemListWithUniqueItems[k].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[k].NetBillWeight = itemWeightAtI;
				itemListWithUniqueItems[k].WcutTotal = itemWeightCutAtI;
				itemListWithUniqueItems[k].RateCut = itemRateCutAtI;
				itemListWithUniqueItems[k].GrossWeight = itemGrossWeightAtI;
			}
			string Remarks = string.Empty;
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail item2 in obj.invPurchaseInvoiceDetailList.ToList())
			{
				if (item2.ActionTypeId == 3)
				{
					continue;
				}
				Remarks = "";
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count <= 0)
					{
						continue;
					}
					item2.ItemName = Conversion.ToString(find2[0].ItemName);
					VoucherDetail vd2 = new VoucherDetail();
					if (obj.DocumentTypeId == 1502 || obj.DocumentTypeId == 1503)
					{
						Remarks = Conversion.ToString(item2.ItemName) + "   " + Conversion.ToString(item2.JobLotDescription) + "   " + Conversion.ToString(item2?.UOMCode) + "   " + Conversion.ToString(item2.NetBillWeight) + " @" + Conversion.ToString(item2.ItemRate) + "/- ";
						if (item2.GpNo > 0)
						{
							Remarks = Remarks + " GP# " + Conversion.ToString(item2.GpNo);
						}
						if (item2.VehicleNo != null && item2.VehicleNo != string.Empty)
						{
							Remarks = Remarks + " V# " + Conversion.ToString(item2.VehicleNo);
						}
						if (item2.RemarksDetail != null && item2.RemarksDetail != string.Empty)
						{
							Remarks = Remarks + " " + Conversion.ToString(item2.RemarksDetail);
						}
					}
					vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd2.Comments = Conversion.ToString(Remarks);
					vd2.DebitAmount = Conversion.ToSingle(item2.ItemAmount);
					vd2.CreditAmount = Conversion.ToSingle(0);
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
					vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd2);
					VoucherDetail vd3 = new VoucherDetail();
					vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd3.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
					vd3.Comments = Conversion.ToString(Remarks);
					vd3.CreditAmount = Conversion.ToSingle(item2.ItemAmount);
					vd3.DebitAmount = Conversion.ToSingle(0);
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
					vh.voucherDetailList.Add(vd3);
					if (item2.ExpenseAmount > 0m)
					{
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd4.Comments = Remarks;
						vd4.DebitAmount = Conversion.ToDouble(item2.ExpenseAmount);
						vd4.CreditAmount = Conversion.ToDouble(0);
						vd4.JobLotId = Conversion.ToInt(item2.JobLotId);
						vh.voucherDetailList.Add(vd4);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd6.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd6.Comments = Remarks;
						vd6.CreditAmount = Conversion.ToDouble(item2.ExpenseAmount);
						vd6.DebitAmount = Conversion.ToDouble(0);
						vd6.JobLotId = Conversion.ToInt(item2.JobLotId);
						vh.voucherDetailList.Add(vd6);
					}
					if (item2.CommissionAmount > 0m)
					{
						VoucherDetail vd5 = new VoucherDetail();
						vd5.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd5.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
						vd5.Comments = Conversion.ToString(obj.CommissionRemarks);
						vd5.DebitAmount = Conversion.ToDouble(item2.CommissionAmount);
						vd5.CreditAmount = Conversion.ToDouble(0);
						vd5.JobLotId = Conversion.ToInt(item2.JobLotId);
						vh.voucherDetailList.Add(vd5);
						VoucherDetail vd7 = new VoucherDetail();
						vd7.AccountId = Conversion.ToInt(vh.AgainstAccountId);
						vd7.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd7.Comments = Conversion.ToString(obj.CommissionRemarks);
						vd7.DebitAmount = Conversion.ToSingle(0);
						vd7.CreditAmount = Conversion.ToSingle(item2.CommissionAmount);
						vd7.JobLotId = Conversion.ToInt(item2.JobLotId);
						vh.voucherDetailList.Add(vd7);
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
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail item in itemListWithUniqueItems.ToList())
			{
				if ((obj.DocumentTypeId != 1502 && obj.DocumentTypeId != 1503) || item.ActionTypeId == 3 || dtitem.Count <= 0)
				{
					continue;
				}
				List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
				if (find.Count > 0)
				{
					if (item.FreightAmount > 0m)
					{
						VoucherDetail vd9 = new VoucherDetail();
						vd9.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd9.AgainstAccountId = obj.TransportAccountId;
						vd9.DebitAmount = Conversion.ToDouble(item.FreightAmount);
						vd9.CreditAmount = 0.0;
						vd9.Comments = Remarks;
						vh.voucherDetailList.Add(vd9);
					}
					if (item.BrokeryAmount > 0m && obj.BrokeryAmount > 0m)
					{
						VoucherDetail vd8 = new VoucherDetail();
						vd8.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd8.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
						vd8.DebitAmount = Conversion.ToDouble(item.BrokeryAmount);
						vd8.CreditAmount = 0.0;
						vd8.Comments = Remarks;
						vh.voucherDetailList.Add(vd8);
					}
				}
			}
		}
		return vh;
	}

	public static VoucherHead PurchaseReturnFinancial(Architecture.Model.Steel.Inventory.InvPurchaseInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if (obj.invPurchaseInvoiceDetailList.Count > 0)
		{
			List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail> itemListWithUniqueItems = new List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail>();
			List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail> dummyArray = JsonConvert.DeserializeObject<List<Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail>>(JsonConvert.SerializeObject(obj.invPurchaseInvoiceDetailList.ToList()));
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
				decimal itemGrossWeightAtI = default(decimal);
				for (int n = 0; n < dummyArray.Count; n++)
				{
					if (itemListWithUniqueItems[l].ItemId == dummyArray[n].ItemId && itemListWithUniqueItems[l].ItemRate == dummyArray[n].ItemRate)
					{
						itemAmountAtI += dummyArray[n].ItemAmount;
						itemFreightAmountAtI += dummyArray[n].FreightAmount;
						itemExpenseAmountAtI += dummyArray[n].ExpenseAmount;
						itemCommissionAmountAtI += dummyArray[n].CommissionAmount;
						itemQtyAtI += dummyArray[n].ItemQty;
						itemWeightAtI += dummyArray[n].NetBillWeight;
						itemWeightCutAtI += dummyArray[n].WcutTotal;
						itemRateCutAtI += dummyArray[n].RateCut;
						itemGrossWeightAtI += dummyArray[n].GrossWeight;
					}
				}
				itemListWithUniqueItems[l].ItemAmount = itemAmountAtI;
				itemListWithUniqueItems[l].ExpenseAmount = itemExpenseAmountAtI;
				itemListWithUniqueItems[l].FreightAmount = itemFreightAmountAtI;
				itemListWithUniqueItems[l].CommissionAmount = itemCommissionAmountAtI;
				itemListWithUniqueItems[l].ItemQty = itemQtyAtI;
				itemListWithUniqueItems[l].NetBillWeight = itemWeightAtI;
				itemListWithUniqueItems[l].WcutTotal = itemWeightCutAtI;
				itemListWithUniqueItems[l].RateCut = itemRateCutAtI;
				itemListWithUniqueItems[l].GrossWeight = itemGrossWeightAtI;
			}
			string Remarks = string.Empty;
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail item2 in itemListWithUniqueItems.ToList())
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
						vd3.DebitAmount = Conversion.ToSingle(item2.ItemAmount);
						vd3.CreditAmount = Conversion.ToSingle(0);
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
						vd2.CreditAmount = Conversion.ToSingle(item2.ItemAmount);
						vd2.DebitAmount = Conversion.ToSingle(0);
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
			foreach (Architecture.Model.Steel.Inventory.InvPurchaseInvoiceDetail item in itemListWithUniqueItems.ToList())
			{
				if (dtitem.Count > 0)
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
