using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model.Inventory;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.WinApp.DataSyncing.Mapping;

public class PurchaseInvoiceMapping
{
	public static InvPurchaseInvoice ConvertLocalSaleObjectToTaxSaleObject(InvPurchaseInvoice Obj, DataTable TaxData = null)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Expected O, but got Unknown
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
			InvPurchaseInvoice TaxObj = new InvPurchaseInvoice
			{
				OrganizationId = TaxOrganizationId,
				CompanyId = TaxCompanyId,
				FinancialYearId = TaxFinancialYearId,
				BranchesId = TaxBranchesId,
				ProjectsId = Obj.ProjectsId,
				EntryUser = TaxDefaultUserId,
				ModifyUser = TaxDefaultUserId,
				PostUser = TaxDefaultUserId,
				EntryDate = DateTime.Now,
				ModifyDate = DateTime.Now,
				PostDate = DateTime.Now,
				IsApproved = false,
				CustomAccounts = true,
				DocDate = Obj.DocDate,
				SupplierInvoiceDate = Obj.SupplierInvoiceDate,
				CashReceived = Obj.CashReceived,
				FreightAmount = Obj.FreightAmount,
				TransporterDebitGLId = 0,
				TransporterDebitPartyId = 0,
				TransporterCreditPartyId = 0,
				DueDays = Obj.DueDays,
				DueDate = Obj.DueDate,
				PaymentTermsId = Obj.PaymentTermsId,
				DeliveryTerm = Obj.DeliveryTerm,
				CommissionAgentId = 0,
				CommissionType = "",
				UomScheduleIdCmRate = "",
				CommRate = 0.0,
				CommAmount = 0.0,
				CommissionRemarks = "",
				SalesTaxNo = Obj.SalesTaxNo,
				DocumentTypeSrNo = Obj.DocumentTypeSrNo,
				ReferencePartyId = Obj.ReferencePartyId,
				StockPartyId = Obj.StockPartyId,
				SupplierCustomerId = Obj.SupplierCustomerId,
				SupplierInvoiceNo = Obj.SupplierInvoiceNo,
				SupplierReferenceNo = Conversion.ToString((object)Obj.SupplierReferenceNo),
				TransportAccountId = Obj.TransportAccountId,
				CommissionCreditAccountId = 0,
				DiscountAccountId = 0,
				InvoiceQty = Obj.InvoiceQty,
				InvoiceWeight = Obj.InvoiceWeight,
				CurrencyId = Obj.CurrencyId,
				ExchangeRate = Obj.ExchangeRate,
				DiscountAmount = 0.0,
				FcyAmount = Obj.FcyAmount,
				IsAttachments = Obj.IsAttachments,
				ManualBillNo = Obj.ManualBillNo,
				OtherRemarks = Obj.OtherRemarks,
				RemarksHeader = Obj.RemarksHeader,
				ScreenName = Obj.ScreenName,
				FreightRemark = "",
				IsTaxable = (Obj.TaxAccountId > 0),
				TaxAccountId = Obj.TaxAccountId,
				BillAmount = Obj.BillAmount,
				Id = 0,
				DocNo = 0,
				DocumentTypeId = 57,
				BranchSrNo = 0,
				AttachmentsValues = string.Empty,
				CustomAttachmentsValues = string.Empty,
				AttachmentsList = Obj.AttachmentsList,
				DeleteAttachmentsList = Obj.DeleteAttachmentsList
			};
			TaxObj.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			TaxObj.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
			TaxObj.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
			TaxObj.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
			TaxObj.InvPurchaseInvoiceEmptyBagslist = new List<InvPurchaseInvoiceEmptyBags>();
			TaxObj.InvPurchaseInvoicePaymentTermsDetailList = new List<InvPurchaseInvoicePaymentTermsDetail>();
			ConvertEmptyBagDetail(Obj, TaxObj);
			ConvertDetailEntry(Obj, TaxObj, TaxData);
			double TItemAmount = TaxObj.invPurchaseInvoiceDetailList.Sum((InvPurchaseInvoiceDetail r) => r.ItemAmount);
			double TTaxAmount = TaxObj.invPurchaseInvoiceDetailList.Sum((InvPurchaseInvoiceDetail r) => r.TaxAmount);
			TaxObj.BillAmount = TItemAmount + TTaxAmount;
			TaxObj.FcyAmount = ((TaxObj.ExchangeRate > 0m) ? (Conversion.ToDecimal((object)TaxObj.BillAmount) / TaxObj.ExchangeRate) : 0m);
			return TaxObj;
		}
		catch (Exception ex)
		{
			throw new Exception("Error converting sale invoice: " + ex.Message, ex);
		}
	}

	private static void ConvertEmptyBagDetail(InvPurchaseInvoice source, InvPurchaseInvoice Destination)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		foreach (InvPurchaseInvoiceEmptyBags item in source.InvPurchaseInvoiceEmptyBagslist)
		{
			InvPurchaseInvoiceEmptyBags List = new InvPurchaseInvoiceEmptyBags
			{
				Id = 0,
				InvPurchaseInvoiceId = item.InvPurchaseInvoiceId,
				ItemId = item.ItemId,
				TypeId = item.TypeId,
				PurchaseQty = item.PurchaseQty,
				ReceivedQty = item.ReceivedQty,
				Rate = item.Rate,
				Amount = item.Amount,
				CreditAccountId = item.CreditAccountId,
				Remarks = item.Remarks
			};
			Destination.InvPurchaseInvoiceEmptyBagslist.Add(List);
		}
	}

	private static void ConvertJournalEntries(InvPurchaseInvoice Obj, InvPurchaseInvoice Destination)
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
		foreach (InvPurchaseInvoiceJournal source in Obj.InvPurchaseInvoicejournalList)
		{
			InvPurchaseInvoiceJournal item = new InvPurchaseInvoiceJournal
			{
				JvCredit = source.JvCredit,
				JvDebit = source.JvDebit,
				JvPrcnt = source.JvPrcnt,
				JvQty = source.JvQty,
				JvRate = source.JvRate,
				ChartofAccountId = source.ChartofAccountId,
				TransporterSupCustId = source.SupplierCustomerId,
				Id = 0,
				InvPurchaseInvoiceId = source.InvPurchaseInvoiceId,
				JvRemarks = source.JvRemarks
			};
			Destination.InvPurchaseInvoicejournalList.Add(item);
		}
	}

	private static void ConvertExpenseEntries(InvPurchaseInvoice Obj, InvPurchaseInvoice Destination)
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
		foreach (InvPurchaseInvoiceExpense source in Obj.InvPurchaseInvoiceExpList)
		{
			InvPurchaseInvoiceExpense item = new InvPurchaseInvoiceExpense
			{
				Amount = source.Amount,
				Qty = source.Qty,
				Rate = source.Rate,
				Id = 0,
				InvRevExpItemId = source.InvRevExpItemId,
				InvPurchaseInvoiceId = source.InvPurchaseInvoiceId,
				Remarks = source.Remarks,
				CustomRemarks = string.Empty
			};
			Destination.InvPurchaseInvoiceExpList.Add(item);
		}
	}

	private static void ConvertFreightEntries(InvPurchaseInvoice Obj, InvPurchaseInvoice Destination)
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
		foreach (InvPurchaseInvoiceFreight source in Obj.InvPurchaseInvoiceFreightList)
		{
			InvPurchaseInvoiceFreight item = new InvPurchaseInvoiceFreight
			{
				Debit = source.Debit,
				FreightAmount = source.FreightAmount,
				FrQty = source.FrQty,
				FrRate = source.FrRate,
				FrWeight = source.FrWeight,
				Id = 0,
				InvGrnId = 0,
				InvPurchaseInvoiceId = source.InvPurchaseInvoiceId,
				TansporterId = source.TansporterId,
				SupplierCustomerId = source.SupplierCustomerId,
				Remarks = source.Remarks
			};
			Destination.InvPurchaseInvoiceFreightList.Add(item);
		}
	}

	private static void ConvertDetailEntry(InvPurchaseInvoice source, InvPurchaseInvoice Destination, DataTable TaxData = null)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		int TaxNameId = 0;
		double TaxPercent = 0.0;
		if (TaxData != null && TaxData.Rows.Count > 0)
		{
			TaxNameId = Conversion.ToInt(TaxData.Rows[0]["TaxNameId"]);
			TaxPercent = Conversion.ToDouble(TaxData.Rows[0]["TaxPercent"]);
		}
		foreach (InvPurchaseInvoiceDetail item in source.invPurchaseInvoiceDetailList)
		{
			double TaxAmount = 0.0;
			if (TaxNameId > 0 && TaxPercent > 0.0)
			{
				TaxAmount = Conversion.ToDouble((object)(item.ItemAmount * TaxPercent / 100.0));
			}
			InvPurchaseInvoiceDetail List = new InvPurchaseInvoiceDetail
			{
				Id = 0,
				InvGrnDetailId = 0,
				InvGrnId = 0,
				PurchaseOrderId = 0,
				PurchaseOrderDetailId = 0,
				InvPurchaseInvoiceId = item.InvPurchaseInvoiceId,
				WarehouseId = item.WarehouseId,
				JobLotId = item.JobLotId,
				CropYear = item.CropYear,
				ItemId = item.ItemId,
				ItemUOMId = item.ItemUOMId,
				PackingTypeId = item.PackingTypeId,
				ItemQty = item.ItemQty,
				GrossWeight = item.GrossWeight,
				WeightCut = item.WeightCut,
				WeightCutTotal = item.WeightCutTotal,
				EBTotalWt = item.EBTotalWt,
				EBWeight = item.EBWeight,
				AdLsWeight = item.AdLsWeight,
				NetBillWeight = item.NetBillWeight,
				NetStockWeight = item.NetStockWeight,
				RateCut = item.RateCut,
				RateCutAmount = item.RateCutAmount,
				ItemRate = item.ItemRate,
				ItemAmount = item.ItemAmount,
				GpDate = item.GpDate,
				AvgCgsRate = item.AvgCgsRate,
				CommissionAmount = 0.0,
				ExpenseAmount = 0.0,
				FreightAmount = 0.0,
				ItemCgsRate = item.ItemCgsRate,
				JournalAmount = 0.0,
				Brokery = item.Brokery,
				DiscountTypeId = 0,
				DiscountPercent = 0.0,
				DiscountAmount = 0.0,
				EbPurAgainstWeight = item.EbPurAgainstWeight,
				ExchangeRate = item.ExchangeRate,
				FcyAmount = item.FcyAmount,
				CurrencyId = item.CurrencyId,
				GpNo = item.GpNo,
				UomScheduleIdRate = item.UomScheduleIdRate,
				RefDocumentTypeId = 0,
				RefDocId = 0,
				RefDocSubId = 0,
				LineId = item.LineId,
				CityId = 0,
				IsTaxable = item.IsTaxable,
				LabAnalisysNo = item.LabAnalisysNo,
				RemarksDetail = item.RemarksDetail,
				TaxDescriptions = item.TaxDescriptions,
				VehicleNo = item.VehicleNo,
				TaxNameId = ((item.TaxNameId > 0) ? item.TaxNameId : TaxNameId),
				TaxPercent = ((item.TaxPercent > 0.0) ? item.TaxPercent : TaxPercent),
				TaxAmount = ((item.TaxPercent > 0.0) ? item.TaxAmount : TaxAmount),
				BillAmount = item.BillAmount
			};
			Destination.invPurchaseInvoiceDetailList.Add(List);
		}
	}
}
