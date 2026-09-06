using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture.BLL.Inventory;
using Architecture.BLL.TaxProject.Inventory;
using Architecture.Common;
using Architecture.Model.Inventory;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.WinApp.DataSyncing.Mapping;

public class SaleInvoiceMapping
{
	public static InvSaleInvoice ConvertLocalSaleObjectToTaxSaleObject(InvSaleInvoice Obj, DataTable TaxData = null)
	{
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany(Obj.OrganizationId, Obj.CompanyId);
			if (dt == null || dt.Rows.Count == 0)
			{
				throw new Exception("Tax company setup not found for the given company.");
			}
			int TaxOrganizationId = Convert.ToInt32(dt.Rows[0]["TaxOrganizationId"]);
			int TaxCompanyId = Convert.ToInt32(dt.Rows[0]["TaxCompanyId"]);
			int TaxFinancialYearId = Convert.ToInt32(dt.Rows[0]["TaxFinancialYearId"]);
			int TaxBranchesId = Convert.ToInt32(dt.Rows[0]["TaxBranchesId"]);
			int TaxDefaultUserId = Convert.ToInt32(dt.Rows[0]["TaxDefaultUserId"]);
			DataTable dtTaxData = new DataTable();
			if (Obj.TaxAccountId <= 0)
			{
				dtTaxData = InvSaleInvoice.TaxScheduleMain_GetLatestSchedule(TaxOrganizationId, TaxCompanyId, Obj.DocDate);
				Obj.TaxAccountId = ((dtTaxData.Rows.Count > 0) ? Convert.ToInt32(dtTaxData.Rows[0]["TaxGLAccountId"]) : 0);
			}
			InvSaleInvoice TaxObj = new InvSaleInvoice
			{
				OrganizationId = TaxOrganizationId,
				CompanyId = TaxCompanyId,
				FinancialYearId = TaxFinancialYearId,
				BranchesId = TaxBranchesId,
				EntryUser = TaxDefaultUserId,
				ModifyUser = TaxDefaultUserId,
				PostUser = TaxDefaultUserId,
				EntryDate = DateTime.Now,
				ModifyDate = DateTime.Now,
				PostDate = DateTime.Now,
				IsApproved = false,
				CustomAccounts = true,
				DocDate = Obj.DocDate,
				DueDate = Obj.DueDate,
				SupplierInvoiceDate = Obj.SupplierInvoiceDate,
				CashReceived = Obj.CashReceived,
				CommAmount = 0.0,
				CommRate = 0.0,
				FreightAmount = 0.0,
				TransporterDebitGLId = 0,
				TransporterDebitPartyId = 0,
				TransporterCreditPartyId = 0,
				DueDays = Obj.DueDays,
				CommissionAgentId = 0,
				SalesTaxNo = Obj.SalesTaxNo,
				DocumentTypeSrNo = Obj.DocumentTypeSrNo,
				ProjectsId = Obj.ProjectsId,
				ReferencePartyId = Obj.ReferencePartyId,
				StockPartyId = Obj.StockPartyId,
				SupplierCustomerId = Obj.SupplierCustomerId,
				SupplierInvoiceNo = Obj.SupplierInvoiceNo,
				SupplierReferenceNo = Obj.SupplierReferenceNo,
				TransporterId = 0,
				CommissionDebitAcGLId = 0,
				PaymentTermId = Obj.PaymentTermId,
				DiscountAccountId = Obj.DiscountAccountId,
				InvoiceQty = Obj.InvoiceQty,
				InvoiceWeight = Obj.InvoiceWeight,
				CurrencyId = Obj.CurrencyId,
				OtherCategoryId = Obj.OtherCategoryId,
				ExchangeRate = Obj.ExchangeRate,
				DiscountAmount = Obj.DiscountAmount,
				FcyAmount = Obj.FcyAmount,
				CommissionRemarks = "",
				CommissionType = "",
				IsAttachments = Obj.IsAttachments,
				IsReserve = Obj.IsReserve,
				ManualBillNo = Obj.ManualBillNo,
				OtherRemarks = Obj.OtherRemarks,
				RemarksHeader = Obj.RemarksHeader,
				UomScheduleIdCmRate = "",
				DeliveryTerm = Obj.DeliveryTerm,
				ScreenName = Obj.ScreenName,
				FreightRemark = "",
				IsTaxable = (Obj.TaxAccountId > 0),
				TaxAccountId = Obj.TaxAccountId,
				BillAmount = Obj.BillAmount,
				Id = 0,
				DocNo = 0,
				DocumentTypeId = 99,
				BranchSrNo = 0,
				AutoUpdate = 0,
				LocationTypeId = 0,
				TransTypeId = 0,
				CustomgroupId = 0,
				AttachmentsValues = string.Empty,
				CustomAttachmentsValues = string.Empty,
				AttachmentsList = Obj.AttachmentsList,
				DeleteAttachmentsList = Obj.DeleteAttachmentsList
			};
			TaxObj.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			TaxObj.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			TaxObj.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			TaxObj.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			TaxObj.SaleInvoicePaymentTermsDetailslist = new List<SaleInvoicePaymentTermsDetail>();
			TaxObj.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			ConvertPaymentTermDetail(Obj, TaxObj);
			ConvertDetailEntry(Obj, TaxObj, TaxData);
			double TItemAmount = TaxObj.InvSaleInvoiceDetaillist.Sum((InvSaleInvoiceDetail r) => r.ItemAmount);
			double TTaxAmount = TaxObj.InvSaleInvoiceDetaillist.Sum((InvSaleInvoiceDetail r) => r.TaxAmount);
			TaxObj.BillAmount = TItemAmount + TTaxAmount;
			TaxObj.FcyAmount = Conversion.ToDecimal((object)TaxObj.BillAmount) / TaxObj.ExchangeRate;
			return TaxObj;
		}
		catch (Exception ex)
		{
			throw new Exception("Error converting sale invoice: " + ex.Message, ex);
		}
	}

	private static void ConvertPaymentTermDetail(InvSaleInvoice source, InvSaleInvoice Destination)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		foreach (SaleInvoicePaymentTermsDetail item in source.SaleInvoicePaymentTermsDetailslist)
		{
			SaleInvoicePaymentTermsDetail List = new SaleInvoicePaymentTermsDetail
			{
				Amount = item.Amount,
				PrcntOfTotal = item.PrcntOfTotal,
				DueDate = item.DueDate,
				DueDays = item.DueDays,
				Id = 0,
				InvSaleInvoiceId = item.InvSaleInvoiceId,
				PaymentTermId = item.PaymentTermId,
				SortNo = item.SortNo,
				PaymentRemarks = item.PaymentRemarks
			};
			Destination.SaleInvoicePaymentTermsDetailslist.Add(List);
		}
	}

	private static void ConvertCommissionEntries(InvSaleInvoice Obj, InvSaleInvoice Destination)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		foreach (InvSaleInvoiceCommission source in Obj.InvSaleInvoiceCommission)
		{
			InvSaleInvoiceCommission item = new InvSaleInvoiceCommission
			{
				CommAmount = source.CommAmount,
				CommType = source.CommType,
				Rate = source.Rate,
				RateUom = source.RateUom,
				CommissionAgentId = source.CommissionAgentId,
				Id = 0,
				InvSaleInvoiceId = source.InvSaleInvoiceId,
				CommDebitSupCustId = source.CommDebitSupCustId,
				DebitAccountId = source.DebitAccountId,
				SaleOrderId = 0,
				Remarks = source.Remarks
			};
			Destination.InvSaleInvoiceCommission.Add(item);
		}
	}

	private static void ConvertJournalEntries(InvSaleInvoice Obj, InvSaleInvoice Destination)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		foreach (InvSaleInvoiceJournal source in Obj.InvSaleInvoiceJournalList)
		{
			InvSaleInvoiceJournal item = new InvSaleInvoiceJournal
			{
				JvCredit = source.JvCredit,
				JvDebit = source.JvDebit,
				JvPrcnt = source.JvPrcnt,
				JvQty = source.JvQty,
				JvRate = source.JvRate,
				ChartofAccountId = source.ChartofAccountId,
				TransporterSupCustId = source.TransporterSupCustId,
				Id = 0,
				InvSaleInvoiceId = source.InvSaleInvoiceId,
				JvRemarks = source.JvRemarks
			};
			Destination.InvSaleInvoiceJournalList.Add(item);
		}
	}

	private static void ConvertExpenseEntries(InvSaleInvoice Obj, InvSaleInvoice Destination)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		foreach (InvSaleInvoiceExpense source in Obj.InvSaleInvoiceExpenseList)
		{
			InvSaleInvoiceExpense item = new InvSaleInvoiceExpense
			{
				Amount = source.Amount,
				Qty = source.Qty,
				Rate = source.Rate,
				Id = 0,
				InvRevExpItemId = source.InvRevExpItemId,
				InvSaleInvoiceId = source.InvSaleInvoiceId,
				Remarks = source.Remarks,
				CustomRemarks = string.Empty
			};
			Destination.InvSaleInvoiceExpenseList.Add(item);
		}
	}

	private static void ConvertFreightEntries(InvSaleInvoice Obj, InvSaleInvoice Destination)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		foreach (InvSaleInvoiceFreight source in Obj.InvSaleInvoiceFreightList)
		{
			InvSaleInvoiceFreight item = new InvSaleInvoiceFreight
			{
				Debit = source.Debit,
				FreightAmount = source.FreightAmount,
				FrQty = source.FrQty,
				FrRate = source.FrRate,
				FrWeight = source.FrWeight,
				Id = 0,
				InvGdnId = 0,
				InvSaleInvoiceId = source.InvSaleInvoiceId,
				TansporterId = source.TansporterId,
				TransporterSupCustId = source.TransporterSupCustId,
				Remarks = source.Remarks
			};
			Destination.InvSaleInvoiceFreightList.Add(item);
		}
	}

	private static void ConvertDetailEntry(InvSaleInvoice source, InvSaleInvoice Destination, DataTable TaxData = null)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		int TaxNameId = 0;
		double TaxPercent = 0.0;
		if (TaxData != null && TaxData.Rows.Count > 0)
		{
			TaxNameId = Conversion.ToInt(TaxData.Rows[0]["TaxNameId"]);
			TaxPercent = Conversion.ToDouble(TaxData.Rows[0]["TaxPercent"]);
		}
		foreach (InvSaleInvoiceDetail item in source.InvSaleInvoiceDetaillist)
		{
			double TaxAmount = 0.0;
			if (TaxNameId > 0 && TaxPercent > 0.0)
			{
				TaxAmount = Conversion.ToDouble((object)(item.ItemAmount * TaxPercent / 100.0));
			}
			InvSaleInvoiceDetail List = new InvSaleInvoiceDetail
			{
				GpDate = item.GpDate,
				DueDate = item.DueDate,
				AdLsWeight = item.AdLsWeight,
				AvgCgsRate = item.AvgCgsRate,
				CommissionAmount = 0.0,
				EBTotalWt = item.EBTotalWt,
				EBWeight = item.EBWeight,
				ExpenseAmount = 0.0,
				FreightAmount = 0.0,
				GrossWeight = item.GrossWeight,
				ItemAmount = ((source.DiscountAccountId > 0) ? item.ItemAmount : (item.ItemAmount - item.ItemDiscountAmount)),
				ItemCgsRate = item.ItemCgsRate,
				ItemQty = item.ItemQty,
				ItemRate = item.ItemRate,
				JournalAmount = 0.0,
				NetBillWeight = item.NetBillWeight,
				NetStockWeight = item.NetStockWeight,
				RateCut = item.RateCut,
				RateCutAmount = item.RateCutAmount,
				WeightCut = item.WeightCut,
				WeightCutTotal = item.WeightCutTotal,
				ItemDiscount = item.ItemDiscount,
				DiscountTypeId = item.DiscountTypeId,
				ItemDiscountAmount = item.ItemDiscountAmount,
				ItemRateWOExp = item.ItemRateWOExp,
				PackingAddLess = item.PackingAddLess,
				ExchangeRate = item.ExchangeRate,
				FcyAmount = item.FcyAmount,
				CurrencyId = item.CurrencyId,
				GpNo = item.GpNo,
				Id = 0,
				InvSaleInvoiceId = item.InvSaleInvoiceId,
				InvGdnDetailId = 0,
				InvGdnId = 0,
				ItemId = item.ItemId,
				ItemUOMId = item.ItemUOMId,
				JobLotId = item.JobLotId,
				PackingTypeId = item.PackingTypeId,
				SaleOrderId = 0,
				SaleOrderDetailId = 0,
				UomScheduleIdRate = item.UomScheduleIdRate,
				WarehouseId = item.WarehouseId,
				InvForwardingId = item.InvForwardingId,
				InvForwardingDetailId = item.InvForwardingDetailId,
				RefRefDocumentTypeId = item.RefRefDocumentTypeId,
				RefRefDocIdNo = item.RefRefDocIdNo,
				RefDocSubId = item.RefDocSubId,
				LineId = item.LineId,
				CityId = item.CityId,
				ReserveWareHouse = item.ReserveWareHouse,
				PaymentTermId = item.PaymentTermId,
				BillCalculateTypeId = item.BillCalculateTypeId,
				DueDays = item.DueDays,
				CgsRateUomId = item.CgsRateUomId,
				CropYear = item.CropYear,
				IsTaxable = item.IsTaxable,
				LabAnalisysNo = item.LabAnalisysNo,
				RemarksDetail = item.RemarksDetail,
				TaxDescriptions = item.TaxDescriptions,
				VehicleNo = item.VehicleNo,
				ReferenceNo = item.ReferenceNo,
				CommOnSale = item.CommOnSale,
				TaxNameId = ((item.TaxNameId > 0) ? item.TaxNameId : TaxNameId),
				TaxPercent = ((item.TaxPercent > 0.0) ? item.TaxPercent : TaxPercent),
				TaxAmount = ((item.TaxPercent > 0.0) ? item.TaxAmount : TaxAmount),
				BillAmount = item.BillAmount
			};
			Destination.InvSaleInvoiceDetaillist.Add(List);
		}
	}
}
