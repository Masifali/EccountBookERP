using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.TaxProject.Common;
using Architecture.Common;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.BLL.TaxProject.Inventory.Financials.SaleInvoice;

public static class SaleInvoiceFinancial
{
	public static VoucherHead MakeVoucherForSaleInvoice(Architecture.Model.TaxProject.Inventory.InvSaleInvoice obj)
	{
		int StockPartyGlAccountId = 0;
		string SupplierName = "";
		List<InventoryItemsOther> dtItemOther = new List<InventoryItemsOther>();
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
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtItemOther = CommonServies.GetOtherItemForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			Architecture.Model.TaxProject.Inventory.InvSaleInvoice invSaleInvoice = obj;
			if (invSaleInvoice != null && invSaleInvoice.CommAmount > 0.0 && obj.CommissionAgentId > 0)
			{
				SupplierCustomerGlAccountId Cmgnt = dtSupplier.FirstOrDefault((SupplierCustomerGlAccountId x) => x.Id == obj.CommissionAgentId);
				if (Cmgnt == null)
				{
					throw new Exception("Commission Agent GLAccountId not Found");
				}
				vh.AgainstAccountId = Cmgnt.GlAccountId;
			}
			SupplierCustomerGlAccountId supplier = dtSupplier.FirstOrDefault((SupplierCustomerGlAccountId x) => x.Id == obj.SupplierCustomerId);
			if (supplier == null)
			{
				throw new Exception("Party GLAccountId not Found");
			}
			vh.RefAccountId = supplier.GlAccountId;
			SupplierName = supplier.CompanyName;
			if (obj.DocumentTypeId == 222)
			{
				SupplierCustomerGlAccountId stockParty = dtSupplier.FirstOrDefault((SupplierCustomerGlAccountId x) => x.Id == obj.StockPartyId);
				if (stockParty != null)
				{
					StockPartyGlAccountId = stockParty.GlAccountId;
				}
			}
			int CostCenterId = 0;
			if (obj.DocumentTypeId == 95)
			{
				CostCenterId = obj.InvSaleInvoiceDetaillist.Max((InvSaleInvoiceDetail x) => x.CostCenterId);
			}
			bool Foc = false;
			bool Zakat = false;
			if (obj.DocumentTypeId == 99)
			{
				Foc = obj.OtherCategoryId == 55;
				Zakat = obj.OtherCategoryId == 56;
			}
			CommonIdsForFinancials commonIds = new CommonIdsForFinancials();
			commonIds.SupplierGlAccountId = vh.RefAccountId;
			commonIds.OffsetAccountId = vh.RefAccountId;
			commonIds.CommissionGlAccountId = vh.AgainstAccountId;
			commonIds.CompanyName = SupplierName;
			commonIds.StockPartyGlAccountId = StockPartyGlAccountId;
			commonIds.CostCenterId = CostCenterId;
			obj.CommonIdsForFinancials = commonIds;
			foreach (VoucherDetail item5 in SaleInvoiceGeneralFinancialMethods.ItemAndCustomerByWeightFinancial(obj, dtitem, dtSupplier).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item5);
			}
			if (!Foc && !Zakat)
			{
				foreach (VoucherDetail item4 in SaleInvoiceGeneralFinancialMethods.FreightFinancial(obj).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item4);
				}
			}
			if (!Foc && !Zakat)
			{
				foreach (VoucherDetail item3 in SaleInvoiceGeneralFinancialMethods.SupplierAddLessFinancial(obj).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item3);
				}
			}
			foreach (VoucherDetail item2 in SaleInvoiceGeneralFinancialMethods.OtherExpenseFinancial(obj, dtItemOther).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item2);
			}
			if (!Foc && !Zakat)
			{
				foreach (VoucherDetail item in SaleInvoiceGeneralFinancialMethods.SalesTaxFinancial(obj).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		if (obj.DocumentTypeId == 1608 || obj.DocumentTypeId == 1609 || obj.DocumentTypeId == 1611)
		{
			vh.Remarks = obj.OtherRemarks;
		}
		else
		{
			vh.Remarks = obj.RemarksHeader;
		}
		return vh;
	}
}
