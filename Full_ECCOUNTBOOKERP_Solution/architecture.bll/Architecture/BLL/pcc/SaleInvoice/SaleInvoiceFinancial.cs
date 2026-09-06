using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc.SaleInvoice;

public static class SaleInvoiceFinancial
{
	public static VoucherHead MakeVoucherForSaleInvoice(Architecture.Model.pcc.InvSaleInvoice obj)
	{
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
			dtItemOther = CommonServies.GetOtherItemForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				if (obj.CommissionAmount > 0m)
				{
					if (obj.CommissionAgentId <= 0)
					{
						throw new Exception("Commission Agent GLAccountId not Found");
					}
					List<SupplierCustomerGlAccountId> Cmgnt = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.CommissionAgentId)).ToList();
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
			commonIds.CompanyName = SupplierName;
			obj.CommonIdsForFinancials = commonIds;
			foreach (VoucherDetail item5 in SaleInvoiceGeneralFinancialMethods.ItemAndCustomerByWeightFinancial(obj, dtitem).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item5);
			}
			if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861) && obj.FrieghtAmountHeader > 0m)
			{
				VoucherDetail vd9 = new VoucherDetail();
				vd9.AccountId = Conversion.ToInt(vh.RefAccountId);
				vd9.AgainstAccountId = Conversion.ToInt(obj.TransporterId);
				vd9.Comments = "Carriage / Freight";
				vd9.DebitAmount = Conversion.ToDouble(obj.FrieghtAmountHeader);
				vd9.SubsidiaryTypeId = 1;
				vd9.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd9);
				VoucherDetail vd14 = new VoucherDetail();
				vd14.AccountId = Conversion.ToInt(obj.TransporterId);
				vd14.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd14.Comments = "Carriage / Freight";
				vd14.CreditAmount = Conversion.ToDouble(obj.FrieghtAmountHeader);
				vd14.DebitAmount = Conversion.ToDouble(0);
				vd14.SubsidiaryAgainstTypeId = 1;
				vd14.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd14);
			}
			if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861) && obj.OtherWagesHeader > 0m)
			{
				VoucherDetail vd8 = new VoucherDetail();
				vd8.AccountId = Conversion.ToInt(vh.RefAccountId);
				vd8.AgainstAccountId = Conversion.ToInt(obj.OtherWagesAccountId);
				vd8.Comments = "Labour / Wages";
				vd8.DebitAmount = Conversion.ToDouble(obj.OtherWagesHeader);
				vd8.SubsidiaryTypeId = 1;
				vd8.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd8);
				VoucherDetail vd13 = new VoucherDetail();
				vd13.AccountId = Conversion.ToInt(obj.OtherWagesAccountId);
				vd13.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				vd13.Comments = "Labour / Wages";
				vd13.CreditAmount = Conversion.ToDouble(obj.OtherWagesHeader);
				vd13.DebitAmount = Conversion.ToDouble(0);
				vd13.SubsidiaryAgainstTypeId = 1;
				vd13.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd13);
			}
			foreach (VoucherDetail item4 in SaleInvoiceGeneralFinancialMethods.SupplierAddLessFinancial(obj).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item4);
			}
			foreach (VoucherDetail item3 in SaleInvoiceGeneralFinancialMethods.OtherExpenseFinancial(obj, dtItemOther).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item3);
			}
			if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861 || obj.DocumentTypeId == 1862) && obj.CommissionAmount > 0m)
			{
				if (obj.DocumentTypeId != 1862)
				{
					VoucherDetail vd6 = new VoucherDetail();
					vd6.AccountId = Conversion.ToInt(obj.CommissionDebitAcId);
					vd6.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
					vd6.Comments = Conversion.ToString(obj.CommissionRemarks);
					vd6.DebitAmount = Conversion.ToDouble(obj.CommissionAmount);
					vd6.SubsidiaryAgainstTypeId = 1;
					vd6.SubsidiaryAgainstAccountId = Conversion.ToInt(obj.CommissionAgentId);
					vh.voucherDetailList.Add(vd6);
					VoucherDetail vd11 = new VoucherDetail();
					vd11.AccountId = Conversion.ToInt(vh.AgainstAccountId);
					vd11.AgainstAccountId = Conversion.ToInt(obj.CommissionDebitAcId);
					vd11.Comments = Conversion.ToString(obj.CommissionRemarks);
					vd11.CreditAmount = Conversion.ToDouble(obj.CommissionAmount);
					vd11.SubsidiaryTypeId = 1;
					vd11.SubsidiaryAccountId = Conversion.ToInt(obj.CommissionAgentId);
					vd11.SupplierCustomerId = Conversion.ToInt(obj.CommissionAgentId);
					vh.voucherDetailList.Add(vd11);
				}
				else
				{
					VoucherDetail vd12 = new VoucherDetail();
					vd12.AccountId = Conversion.ToInt(vh.AgainstAccountId);
					vd12.AgainstAccountId = Conversion.ToInt(obj.CommissionDebitAcId);
					vd12.Comments = Conversion.ToString(obj.CommissionRemarks);
					vd12.DebitAmount = Conversion.ToDouble(obj.CommissionAmount);
					vd12.SubsidiaryTypeId = 1;
					vd12.SubsidiaryAccountId = Conversion.ToInt(obj.CommissionAgentId);
					vd12.SupplierCustomerId = Conversion.ToInt(obj.CommissionAgentId);
					vh.voucherDetailList.Add(vd12);
					VoucherDetail vd7 = new VoucherDetail();
					vd7.AccountId = Conversion.ToInt(obj.CommissionDebitAcId);
					vd7.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
					vd7.Comments = Conversion.ToString(obj.CommissionRemarks);
					vd7.CreditAmount = Conversion.ToDouble(obj.CommissionAmount);
					vh.voucherDetailList.Add(vd7);
				}
			}
			if ((obj.DocumentTypeId == 1856 || obj.DocumentTypeId == 1861) && obj.SettlementDiscountHeader > 0m)
			{
				if (obj.SettlementDiscountAccountId > 0)
				{
					VoucherDetail vd10 = new VoucherDetail();
					vd10.AccountId = Conversion.ToInt(obj.SettlementDiscountAccountId);
					vd10.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd10.Comments = "Settle Discount " + commonIds.CompanyName;
					vd10.DebitAmount = Conversion.ToDouble(obj.SettlementDiscountHeader);
					vd10.SubsidiaryTypeId = 1;
					vd10.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
					vh.voucherDetailList.Add(vd10);
				}
				VoucherDetail vd5 = new VoucherDetail();
				vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
				if (obj.SettlementDiscountAccountId > 0)
				{
					vd5.AgainstAccountId = Conversion.ToInt(obj.SettlementDiscountAccountId);
				}
				else
				{
					vd5.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
				}
				vd5.Comments = "Settle Discount";
				vd5.CreditAmount = Conversion.ToDouble(obj.SettlementDiscountHeader);
				vd5.SubsidiaryTypeId = 1;
				vd5.SubsidiaryAccountId = Conversion.ToInt(obj.SupplierCustomerId);
				vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
				vh.voucherDetailList.Add(vd5);
			}
			foreach (VoucherDetail item2 in SaleInvoiceGeneralFinancialMethods.CGSTransactions(obj, dtitem, dtSupplier).voucherDetailList.ToList())
			{
				vh.voucherDetailList.Add(item2);
			}
			bool Exists = obj.InvSaleInvoiceDetaillist.Any((Architecture.Model.pcc.InvSaleInvoiceDetail rr) => rr.InvGdnId > 0 && rr.ActionTypeId != 3);
			if (obj.DocumentTypeId == 1861 && !Exists)
			{
				foreach (VoucherDetail item in SaleInvoiceGeneralFinancialMethods.ContractWagesFinancial(obj, dtSupplier).voucherDetailList.ToList())
				{
					vh.voucherDetailList.Add(item);
				}
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
