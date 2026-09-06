using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Purchase;
using Architecture.Model.Inventory;

namespace Architecture.BLL.FeedMill.Financials.PurchaseInvoice;

public static class PurchaseInvoiceFinancial
{
	public static VoucherHead MakeVoucherForPurchaseInvoice(Architecture.Model.FeedMill.Purchase.InvPurchaseInvoice obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.RemarksOtherLingo = "";
			vh.VoucherAmount = Conversion.ToDouble(obj.BillAmount);
			vh.MultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
			vh.ExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
			vh.FcAmount = Conversion.ToDouble(obj.FcyAmount);
			vh.voucherDetailList = new List<VoucherDetail>();
			CommonIdsForFinancials commonIds = new CommonIdsForFinancials();
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				if (obj.CommAmount > 0m)
				{
					if (obj.CommissionAgentId <= 0)
					{
						throw new Exception("CommissionAgent Account Not Found");
					}
					List<SupplierCustomerGlAccountId> Cmgent2 = dtSupplier.Where((SupplierCustomerGlAccountId y) => y.Id == Conversion.ToInt(obj.CommissionAgentId)).ToList();
					if (Cmgent2.Count > 0)
					{
						vh.AgainstAccountId = Conversion.ToInt(Cmgent2[0].GlAccountId);
					}
				}
				if (obj.BrokeryAmount != 0m)
				{
					if (obj.BrokerAgentId <= 0)
					{
						throw new Exception("BrokerAgent Account Not Found");
					}
					List<SupplierCustomerGlAccountId> Cmgent = dtSupplier.Where((SupplierCustomerGlAccountId y) => y.Id == Conversion.ToInt(obj.BrokerAgentId)).ToList();
					if (Cmgent.Count > 0)
					{
						commonIds.BrokerGlAccountId = Conversion.ToInt(Cmgent[0].GlAccountId);
						commonIds.BrokerAgentName = Conversion.ToString(Cmgent[0].CompanyName);
					}
				}
				List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find.Count <= 0)
				{
					throw new Exception("Supplier GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find[0].GlAccountId);
				commonIds.CompanyName = Conversion.ToString(find[0].CompanyName);
			}
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			commonIds.SupplierGlAccountId = vh.RefAccountId;
			commonIds.CommissionGlAccountId = vh.AgainstAccountId;
			obj.CommonIdsForFinancials = commonIds;
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = Conversion.ToDouble(obj.BillAmount);
			vh.ManualBillNo = obj.ManualBillNo;
			vh.DueDate = obj.DueDate;
			vh.DueDays = Conversion.ToInt(obj.DueDays);
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			foreach (VoucherDetail item8 in PurchaseInvoiceGeneralFinancialMethods.ItemAndSupplierByWeightFinancial(obj, dtitem).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item8);
			}
			foreach (VoucherDetail item7 in PurchaseInvoiceGeneralFinancialMethods.ItemAndSupplierTradingFinancial(obj, dtitem).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item7);
			}
			if (obj.DocumentTypeId == 59)
			{
				foreach (VoucherDetail item6 in PurchaseInvoiceGeneralFinancialMethods.PurchaseReturnFinancial(obj, dtitem).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item6);
				}
			}
			foreach (VoucherDetail item5 in PurchaseInvoiceGeneralFinancialMethods.FreightFinancial(obj).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item5);
			}
			foreach (VoucherDetail item4 in PurchaseInvoiceGeneralFinancialMethods.SupplierAddLessFinancial(obj).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item4);
			}
			foreach (VoucherDetail item3 in PurchaseInvoiceGeneralFinancialMethods.BrokeryBeHalfOnPartyFinancial(obj).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item3);
			}
			foreach (VoucherDetail item2 in PurchaseInvoiceGeneralFinancialMethods.EmptyBagsFinancial(obj, dtitem).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item2);
			}
			foreach (VoucherDetail item in PurchaseInvoiceGeneralFinancialMethods.SalesTaxFinancial(obj).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		if (obj.RemarksHeader != string.Empty)
		{
			vh.Remarks = obj.RemarksHeader.ToString();
		}
		return vh;
	}
}
