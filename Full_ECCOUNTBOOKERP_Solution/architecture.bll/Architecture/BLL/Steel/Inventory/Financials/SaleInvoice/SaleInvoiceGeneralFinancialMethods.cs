using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;

namespace Architecture.BLL.Steel.Inventory.Financials.SaleInvoice;

public class SaleInvoiceGeneralFinancialMethods
{
	public static VoucherHead SupplierAddLessFinancial(Architecture.Model.Steel.Inventory.InvSaleInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1510) && obj.InvSaleInvoiceJournalList.Count > 0)
		{
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceJournal item in obj.InvSaleInvoiceJournalList.ToList())
			{
				if (Conversion.ToInt(item.ChartofAccountId) != 0)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(item.ChartofAccountId);
					vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd.Comments = Conversion.ToString(item.JvRemarks);
					vd.DebitAmount = Conversion.ToDouble(item.JvDebit);
					vd.CreditAmount = Conversion.ToDouble(item.JvCredit);
					vh.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd2.AgainstAccountId = Conversion.ToInt(item.ChartofAccountId);
					vd2.Comments = Conversion.ToString(item.JvRemarks);
					vd2.CreditAmount = Conversion.ToDouble(item.JvDebit);
					vd2.DebitAmount = Conversion.ToDouble(item.JvCredit);
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		return vh;
	}

	public static VoucherHead FreightFinancial(Architecture.Model.Steel.Inventory.InvSaleInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1510) && obj.InvSaleInvoiceFreightList.Count > 0)
		{
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceFreight r in obj.InvSaleInvoiceFreightList.ToList())
			{
				if (Conversion.ToInt(r.TansporterId) != 0)
				{
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = r.TansporterId;
					vd2.AgainstAccountId = obj.CommonIdsForFinancials.OffsetAccountId;
					vd2.Comments = obj.RemarksHeader;
					vd2.DebitAmount = 0.0;
					vd2.CreditAmount = Conversion.ToDouble(r.CreditAmount);
					vh.voucherDetailList.Add(vd2);
				}
			}
		}
		if (obj.FreightAmount > 0m && (obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1509))
		{
			VoucherDetail vd = new VoucherDetail();
			vd.AccountId = Conversion.ToInt(obj.TransporterId);
			vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
			vd.Comments = obj.RemarksHeader;
			vd.DebitAmount = Conversion.ToDouble(obj.FreightAmount);
			vd.CreditAmount = Conversion.ToDouble(0);
			vh.voucherDetailList.Add(vd);
			VoucherDetail vd3 = new VoucherDetail();
			vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
			vd3.AgainstAccountId = Conversion.ToInt(obj.TransporterId);
			vd3.Comments = obj.RemarksHeader;
			vd3.CreditAmount = Conversion.ToDouble(obj.FreightAmount);
			vd3.DebitAmount = Conversion.ToDouble(0);
			vh.voucherDetailList.Add(vd3);
		}
		return vh;
	}

	public static VoucherHead ItemAndCustomerByWeightFinancial(Architecture.Model.Steel.Inventory.InvSaleInvoice obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1510) && obj.InvSaleInvoiceDetaillist.Count > 0)
		{
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail item in obj.InvSaleInvoiceDetaillist.ToList())
			{
				if (item.ActionTypeId == 3)
				{
					continue;
				}
				string Remarks = obj.RemarksHeader;
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find2.Count > 0)
					{
						VoucherDetail vd2 = new VoucherDetail();
						if (obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1510)
						{
							Remarks = Conversion.ToString(find2[0].ItemName) + "  " + Conversion.ToString(item?.JobLotDescription) + "  " + Conversion.ToString(item?.UOMCode) + "  " + item.NetBillWeight + " @" + item.ItemRate + "/- ";
							if (item.GpNo > 0)
							{
								Remarks = Remarks + "GP# " + item.GpNo;
							}
							if (item.VehicleNo != null && item.VehicleNo != string.Empty)
							{
								Remarks = Remarks + " V# " + item.VehicleNo.ToString();
							}
							if (item.RemarksDetail != null && item.RemarksDetail != string.Empty)
							{
								Remarks = Remarks + " " + item.RemarksDetail.ToString();
							}
						}
						VoucherDetail vd3 = new VoucherDetail();
						vd3.LineId = 0;
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						if (obj.DocumentTypeId == 60)
						{
							vd3.LineId = item.LineId;
							vd3.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
						else
						{
							vd3.AgainstAccountId = Conversion.ToInt(find2[0].SaleGLAC);
						}
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.DebitAmount = Conversion.ToDouble(item.ItemAmount);
						vd3.ItemId = Conversion.ToInt(item.ItemId);
						vd3.QtyOut = Conversion.ToDouble(item.ItemQty);
						vd3.ItemRate = Conversion.ToDouble(item.ItemRate);
						vd3.WeightOut = Conversion.ToDouble(item.NetBillWeight);
						vd3.RateCut = Conversion.ToDouble(item.RateCut);
						vd3.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
						vd3.ItemAmount = Conversion.ToDouble(item.ItemAmount);
						vd3.Expenses = Conversion.ToDouble(item.ExpenseAmount);
						vd3.Freight = Conversion.ToDouble(item.FreightAmount);
						vd3.Commission = Conversion.ToDouble(item.CommissionAmount);
						vd3.OrderNo = Conversion.ToInt(item.SaleOrder);
						vd3.GpNo = Conversion.ToInt(item.GpNo);
						vd3.JobLotId = Conversion.ToInt(item.JobLotId);
						vd3.VehicleNo = item.VehicleNo;
						vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd3);
						vd2.LineId = 0;
						if (obj.DocumentTypeId == 60)
						{
							vd2.LineId = item.LineId;
							vd2.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						}
						else
						{
							vd2.AccountId = Conversion.ToInt(find2[0].SaleGLAC);
						}
						vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.Comments = Conversion.ToString(Remarks);
						vd2.CreditAmount = Conversion.ToDouble(item.ItemAmount);
						vd2.ItemId = Conversion.ToInt(item.ItemId);
						vd2.QtyOut = Conversion.ToDouble(item.ItemQty);
						vd2.ItemRate = Conversion.ToDouble(item.ItemRate);
						vd2.WeightOut = Conversion.ToDouble(item.NetBillWeight);
						vd2.RateCut = Conversion.ToDouble(item.RateCut);
						vd2.RateCutAmount = Conversion.ToDouble(item.RateCutAmount);
						vd2.ItemAmount = Conversion.ToDouble(item.ItemAmount);
						vd2.Expenses = Conversion.ToDouble(item.ExpenseAmount);
						vd2.Commission = Conversion.ToDouble(item.CommissionAmount);
						vd2.Freight = Conversion.ToDouble(item.FreightAmount);
						vd2.OrderNo = Conversion.ToInt(item.SaleOrder);
						vd2.GpNo = Conversion.ToInt(item.GpNo);
						vd2.VehicleNo = item.VehicleNo;
						vd2.JobLotId = Conversion.ToInt(item.JobLotId);
						vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd2);
						if ((obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1510) && item.CommissionAmount > 0m)
						{
							VoucherDetail vd5 = new VoucherDetail();
							vd5.AccountId = Conversion.ToInt(find2[0].SaleGLAC);
							vd5.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
							vd5.Comments = Conversion.ToString(obj.CommRemarks);
							vd5.DebitAmount = Conversion.ToDouble(item.CommissionAmount);
							vd5.JobLotId = Conversion.ToInt(item.JobLotId);
							vh.voucherDetailList.Add(vd5);
							VoucherDetail vd6 = new VoucherDetail();
							vd6.AccountId = Conversion.ToInt(vh.AgainstAccountId);
							vd6.AgainstAccountId = Conversion.ToInt(find2[0].SaleGLAC);
							vd6.Comments = Conversion.ToString(obj.CommRemarks);
							vd6.DebitAmount = Conversion.ToDouble(0);
							vd6.CreditAmount = Conversion.ToDouble(item.CommissionAmount);
							vd6.JobLotId = Conversion.ToInt(item.JobLotId);
							vh.voucherDetailList.Add(vd6);
						}
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				throw new Exception("Item Record Not found");
			}
			foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail itemFreight in obj.InvSaleInvoiceDetaillist.ToList())
			{
				if ((obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1510) && itemFreight.FreightAmount > 0m)
				{
					_ = obj.RemarksHeader;
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item Record Not found");
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(itemFreight.ItemId)).ToList();
					if (find.Count <= 0)
					{
						throw new Exception("Item Record Not found");
					}
					VoucherDetail vd4 = new VoucherDetail();
					vd4.AccountId = Conversion.ToInt(find[0].SaleGLAC);
					vd4.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
					vd4.Comments = itemFreight.RemarksDetail;
					vd4.DebitAmount = Conversion.ToDouble(itemFreight.FreightAmount);
					vd4.CreditAmount = Conversion.ToDouble(0);
					vd4.JobLotId = Conversion.ToInt(itemFreight.JobLotId);
					vh.voucherDetailList.Add(vd4);
				}
			}
		}
		return vh;
	}

	public static VoucherHead OtherExpenseFinancial(Architecture.Model.Steel.Inventory.InvSaleInvoice obj, List<InventoryItemsOther> dtItemOther, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		vh.AgainstAccountId = obj.CommonIdsForFinancials.CommissionGlAccountId;
		if ((obj.DocumentTypeId == 1509 || obj.DocumentTypeId == 1510) && obj.InvSaleInvoiceExpenseList.Count > 0)
		{
			if (Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CreditAmountInItemSaleGL")))
			{
				foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceExpense item in obj.InvSaleInvoiceExpenseList.ToList())
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
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = Conversion.ToInt(find[0].SaleGLAcId);
						vd.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd.Comments = Conversion.ToString(find[0].OtherItemName + " " + item.Remarks);
						vd.CreditAmount = Conversion.ToDouble(item.Amount);
						vh.voucherDetailList.Add(vd);
					}
				}
			}
			else
			{
				string OtherItemRemarks = "";
				foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceExpense item2 in obj.InvSaleInvoiceExpenseList.ToList())
				{
					if (item2.Amount > 0m)
					{
						if (dtItemOther.Count <= 0)
						{
							throw new Exception("Other Items not found");
						}
						List<InventoryItemsOther> find3 = dtItemOther.Where((InventoryItemsOther x) => x.Id == Convert.ToInt32(item2.InvOtherItemId)).ToList();
						if (find3.Count <= 0)
						{
							throw new Exception("Other Items Sale GL Account not found");
						}
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd4.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
						vd4.Comments = Conversion.ToString(find3[0].OtherItemName + " " + item2.Remarks);
						vd4.DebitAmount = Conversion.ToDouble(item2.Amount);
						vh.voucherDetailList.Add(vd4);
						OtherItemRemarks += vd4.Comments;
					}
				}
				foreach (Architecture.Model.Steel.Inventory.InvSaleInvoiceDetail item3 in obj.InvSaleInvoiceDetaillist.ToList())
				{
					if (item3.ExpenseAmount > 0m)
					{
						if (dtitem.Count <= 0)
						{
							throw new Exception("Items not found");
						}
						List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
						if (find2.Count <= 0)
						{
							throw new Exception("Items Sale GL Account not found");
						}
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(find2[0].SaleGLAC);
						vd3.AgainstAccountId = Conversion.ToInt(obj.CommonIdsForFinancials.OffsetAccountId);
						vd3.Comments = Conversion.ToString(OtherItemRemarks);
						vd3.CreditAmount = Conversion.ToDouble(item3.ExpenseAmount);
						vh.voucherDetailList.Add(vd3);
					}
				}
			}
		}
		return vh;
	}
}
