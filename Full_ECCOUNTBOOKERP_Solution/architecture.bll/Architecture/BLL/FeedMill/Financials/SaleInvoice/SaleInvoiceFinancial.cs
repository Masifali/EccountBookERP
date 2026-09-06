using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.BLL.FeedMill.Sales.Financials.SaleInvoice;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.Sales;
using Architecture.Model.Inventory;

namespace Architecture.BLL.FeedMill.Financials.SaleInvoice;

public static class SaleInvoiceFinancial
{
	public static VoucherHead MakeVoucherForSaleInvoice(Architecture.Model.FeedMill.Sales.InvSaleInvoice obj)
	{
		string SupplierName = "";
		string CommissionAgentname = "";
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
		List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
		VoucherHead vh = new VoucherHead();
		try
		{
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.RemarksHeader);
			vh.RemarksOtherLingo = "";
			vh.VoucherAmount = obj.BillAmount;
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = obj.BillAmount;
			vh.ManualBillNo = Conversion.ToString(obj.ManualBillNo);
			vh.DueDate = obj.DueDate;
			vh.DueDays = Conversion.ToInt(obj.DueDays);
			vh.MultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
			vh.ExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
			vh.FcAmount = Conversion.ToDouble(obj.FcyAmount);
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				if (obj.CommAmount > 0.0)
				{
					if (obj.CommissionAgentId <= 0)
					{
						throw new Exception("Commission Agent GLAccountId not Found");
					}
					List<SupplierCustomerGlAccountId> Cmgnt = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.CommissionAgentId)).ToList();
					if (Cmgnt.Count > 0)
					{
						vh.AgainstAccountId = Conversion.ToInt(Cmgnt[0].GlAccountId);
						CommissionAgentname = Conversion.ToString(Cmgnt[0].CompanyName);
					}
				}
				List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find.Count <= 0)
				{
					throw new Exception("Supplier GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find[0].GlAccountId);
				SupplierName = Conversion.ToString(find[0].CompanyName);
			}
			CommonIdsForFinancials commonIds = new CommonIdsForFinancials();
			commonIds.SupplierGlAccountId = vh.RefAccountId;
			commonIds.CommissionGlAccountId = vh.AgainstAccountId;
			commonIds.CompanyName = SupplierName;
			commonIds.BrokerAgentName = CommissionAgentname;
			obj.CommonIdsForFinancials = commonIds;
			foreach (VoucherDetail item in SaleInvoiceGeneralFinancialMethods.ItemAndCustomerByWeightFinancial(obj, dtitem, dtSupplier).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		vh.Remarks = obj.RemarksHeader;
		return vh;
	}
}
