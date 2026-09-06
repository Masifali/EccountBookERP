using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.POS;

namespace Architecture.BLL.Pos.Financials;

public class BookingSaleInvoiceFinancialMethods
{
	public static VoucherHead BookingSaleInvoiceReturnCustomerAndSalesGL(Architecture.Model.POS.WsRmSaleInvoiceReturn obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		vh.RefAccountId = obj.CommonIdsForFinancials.SupplierGlAccountId;
		if (obj.WsRmSaleInvoiceReturnDetailslist.Count > 0)
		{
			string Remarks = string.Empty;
			foreach (WsRmSaleInvoiceReturnDetail item in obj.WsRmSaleInvoiceReturnDetailslist.ToList())
			{
				if (dtitem.Count > 0)
				{
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						VoucherDetail vd2 = new VoucherDetail();
						Remarks = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item.ItemQty + ",   Rate:" + item.ItemRate + "  Item Amount: " + item.ItemAmount;
						vd2.AccountId = Conversion.ToInt(find[0].SaleGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.Comments = Conversion.ToString(Remarks);
						vd2.DebitAmount = Conversion.ToDouble(item.ItemAmount);
						vd2.ItemId = Conversion.ToInt(item.ItemId);
						vd2.QtyIn = Conversion.ToDouble(item.ItemQty);
						vd2.ItemRate = Conversion.ToDouble(item.ItemRate);
						vd2.ItemAmount = Conversion.ToDouble(item.ItemAmount);
						vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(find[0].SaleGLAC);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.CreditAmount = Conversion.ToDouble(item.ItemAmount);
						vd3.ItemId = Conversion.ToInt(item.ItemId);
						vd3.QtyIn = Conversion.ToDouble(item.ItemQty);
						vd3.ItemRate = Conversion.ToDouble(item.ItemRate);
						vd3.WeightIn = Conversion.ToDouble(item.ItemQty);
						vd3.ItemAmount = Conversion.ToDouble(item.ItemAmount);
						vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd3);
					}
					continue;
				}
				throw new Exception("Item Record Not found");
			}
		}
		return vh;
	}

	public static VoucherHead BookingSaleInvoiceReturnCgsAndStock(Architecture.Model.POS.WsRmSaleInvoiceReturn obj, List<GetItemGlIdsandItemName> dtitem)
	{
		VoucherHead vh = new VoucherHead();
		if (obj.WsRmSaleInvoiceReturnDetailslist.Count > 0)
		{
			foreach (WsRmSaleInvoiceReturnDetail item in obj.WsRmSaleInvoiceReturnDetailslist.ToList())
			{
				if (item.ItemCgsRate > 0.0)
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item Record Not found");
					}
					List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						double CgsAmount = 0.0;
						string RemarksCgs = "";
						CgsAmount = item.ItemQty * item.ItemCgsRate;
						RemarksCgs = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item.ItemQty + ",   Rate:" + item.ItemCgsRate + "  Item Amount: " + CgsAmount;
						VoucherDetail vd6 = new VoucherDetail();
						vd6.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd6.Comments = Conversion.ToString(RemarksCgs);
						vd6.DebitAmount = Conversion.ToDouble(CgsAmount);
						vd6.ItemId = Conversion.ToInt(item.ItemId);
						vd6.QtyIn = Conversion.ToDouble(item.ItemQty);
						vd6.WeightIn = Conversion.ToDouble(item.ItemQty);
						vd6.ItemRate = Conversion.ToDouble(item.ItemCgsRate);
						vd6.ItemAmount = Conversion.ToDouble(CgsAmount);
						vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd6);
						VoucherDetail vd7 = new VoucherDetail();
						vd7.AccountId = Conversion.ToInt(find[0].COGSGLAC);
						vd7.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
						vd7.Comments = Conversion.ToString(RemarksCgs);
						vd7.CreditAmount = Conversion.ToDouble(CgsAmount);
						vd7.ItemId = Conversion.ToInt(item.ItemId);
						vd7.QtyIn = Conversion.ToDouble(item.ItemQty);
						vd7.WeightIn = Conversion.ToDouble(item.ItemQty);
						vd7.ItemRate = Conversion.ToDouble(item.ItemCgsRate);
						vd7.ItemAmount = Conversion.ToDouble(CgsAmount);
						vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vh.voucherDetailList.Add(vd7);
					}
				}
			}
		}
		return vh;
	}
}
