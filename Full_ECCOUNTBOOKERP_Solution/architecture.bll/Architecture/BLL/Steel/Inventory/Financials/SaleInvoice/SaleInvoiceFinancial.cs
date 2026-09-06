using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;

namespace Architecture.BLL.Steel.Inventory.Financials.SaleInvoice;

public static class SaleInvoiceFinancial
{
	public static VoucherHead MakeVoucherForSaleInvoice(Architecture.Model.Steel.Inventory.InvSaleInvoice obj)
	{
		int StockAgainstAcId = 0;
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
			vh.VoucherAmount = Conversion.ToDouble(obj.BillAmount);
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = Conversion.ToDouble(obj.BillAmount);
			vh.ManualBillNo = Conversion.ToString(obj.ManualBillNo);
			vh.DueDate = obj.DueDate;
			vh.DueDays = Conversion.ToInt(obj.DueDays);
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.voucherDetailList = new List<VoucherDetail>();
			string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "StockAgainstAccount");
			if (ConfigKey != string.Empty)
			{
				StockAgainstAcId = Conversion.ToInt(ConfigKey);
				dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				dtItemOther = CommonServies.GetOtherItemForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				if (dtSupplier.Count > 0)
				{
					if (obj.CommAmount > 0m)
					{
						if (obj.CommAgentId <= 0)
						{
							throw new Exception("Commission Agent GLAccountId not Found");
						}
						List<SupplierCustomerGlAccountId> Cmgnt = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.CommAgentId)).ToList();
						if (Cmgnt.Count > 0)
						{
							vh.AgainstAccountId = Conversion.ToInt(Cmgnt[0].GlAccountId);
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
				commonIds.OffsetAccountId = StockAgainstAcId;
				commonIds.CompanyName = SupplierName;
				obj.CommonIdsForFinancials = commonIds;
				foreach (VoucherDetail item4 in SaleInvoiceGeneralFinancialMethods.ItemAndCustomerByWeightFinancial(obj, dtitem).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item4);
				}
				foreach (VoucherDetail item3 in SaleInvoiceGeneralFinancialMethods.FreightFinancial(obj).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item3);
				}
				foreach (VoucherDetail item2 in SaleInvoiceGeneralFinancialMethods.SupplierAddLessFinancial(obj).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item2);
				}
				{
					foreach (VoucherDetail item in SaleInvoiceGeneralFinancialMethods.OtherExpenseFinancial(obj, dtItemOther, dtitem).voucherDetailList.ToList())
					{
						vh.voucherDetailList.Add(item);
					}
					return vh;
				}
			}
			throw new Exception("Stock AgainstAc Configuration Not Found");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
