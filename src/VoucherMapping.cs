using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture.BLL.Inventory;
using Architecture.Model.Accounts;
using Architecture.Model.TaxProject.Accounts;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.Model.Mappings
{

public static class VoucherMapping
{
	public static VoucherHead MapToTaxProjectVoucher(VoucherHead source)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		DataTable dt = InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany(source.OrganizationId, source.CompanyId);
		if (dt == null || dt.Rows.Count == 0)
		{
			throw new Exception("Tax company setup not found for the given company.");
		}
		int TaxOrganizationId = Convert.ToInt32(dt.Rows[0]["TaxOrganizationId"]);
		int TaxCompanyId = Convert.ToInt32(dt.Rows[0]["TaxCompanyId"]);
		int TaxFinancialYearId = Convert.ToInt32(dt.Rows[0]["TaxFinancialYearId"]);
		int TaxBranchesId = Convert.ToInt32(dt.Rows[0]["TaxBranchesId"]);
		int TaxDefaultUserId = Convert.ToInt32(dt.Rows[0]["TaxDefaultUserId"]);
		VoucherHead destination = new VoucherHead
		{
			OrganizationId = TaxOrganizationId,
			CompanyId = TaxCompanyId,
			FinancialYearId = TaxFinancialYearId,
			BranchId = TaxBranchesId,
			DocumentTypeId = source.DocumentTypeId,
			BaseDocumentTypeId = source.BaseDocumentTypeId,
			Id = 0,
			EntryUser = TaxDefaultUserId,
			ModifyUser = TaxDefaultUserId,
			PostUser = TaxDefaultUserId,
			EntryDate = DateTime.Now,
			ModifyDate = DateTime.Now,
			PostDate = DateTime.Now,
			PostState = source.PostState,
			IsApproved = source.IsApproved,
			IncludeWHT = source.IncludeWHT,
			ChequeDate = source.ChequeDate,
			DueDate = source.DueDate,
			VoucherDate = source.VoucherDate,
			BillAmount = source.BillAmount,
			ExchangeCurrencyRate = source.ExchangeCurrencyRate,
			FcAmount = source.FcAmount,
			VoucherAmount = source.VoucherAmount,
			AgainstAccountId = source.AgainstAccountId,
			CheqId = source.CheqId,
			ChequePrintId = source.ChequePrintId,
			RefDocumentTypeId = source.RefDocumentTypeId,
			DocumentTypeSrNo = source.DocumentTypeSrNo,
			DueDays = source.DueDays,
			MultiCurrencyId = source.MultiCurrencyId,
			ProjectId = source.ProjectId,
			RefAccountId = source.RefAccountId,
			RefDocNoId = source.RefDocNoId,
			VoucherCode = source.VoucherCode,
			ActionId = source.ActionId,
			BankBranch = source.BankBranch,
			ChequeNo = source.ChequeNo,
			ConversionFormula = source.ConversionFormula,
			DrCrNoteType = source.DrCrNoteType,
			ManualBillNo = source.ManualBillNo,
			PayTitle = source.PayTitle,
			Remarks = source.Remarks,
			RemarksOtherLingo = source.RemarksOtherLingo,
			Source = source.Source,
			CustomAccounts = false,
			AdvanceTaxAccountId = 0,
			AdvanceTaxAmount = 0.0,
			OtherChargesAccountId = 0,
			OtherChargesAmount = 0.0,
			CostCenterAmount = source.costCenterDetailList.Sum((voucherCostCenterDetail c) => c.costAmount),
			CustomAttachmentsValues = "",
			AttachmentsValues = "",
			AttachmentsList = source.AttachmentsList,
			DeleteAttachmentsList = source.DeleteAttachmentsList,
			DetailAttachmentsList = new List<DMSAttachments>()
		};
		MapVoucherDetail(source, destination);
		MapPaymentDueSchedule(source, destination);
		MapCostCenterDetail(source, destination);
		return destination;
	}

	private static void MapVoucherDetail(VoucherHead sources, VoucherHead Destination)
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
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		foreach (VoucherDetail source in sources.voucherDetailList)
		{
			VoucherDetail List = new VoucherDetail
			{
				DCheqDate = source.DCheqDate,
				GpDate = source.GpDate,
				Adjustment = source.Adjustment,
				AdvanceAmount = source.AdvanceAmount,
				Commission = source.Commission,
				CreditAmount = source.CreditAmount,
				DCurrencyAmount = source.DCurrencyAmount,
				DebitAmount = source.DebitAmount,
				DExchangeCurrencyRate = source.DExchangeCurrencyRate,
				Expenses = source.Expenses,
				ExTax = source.ExTax,
				Freight = source.Freight,
				ItemAmount = source.ItemAmount,
				ItemRate = source.ItemRate,
				Journal = source.Journal,
				QtyIn = source.QtyIn,
				QtyOut = source.QtyOut,
				RateCut = source.RateCut,
				RateCutAmount = source.RateCutAmount,
				SaleTax = source.SaleTax,
				TaxesTotalAmount = source.TaxesTotalAmount,
				TaxPrcnt = source.TaxPrcnt,
				WeightIn = source.WeightIn,
				WeightOut = source.WeightOut,
				WhtHolding = source.WhtHolding,
				ItemCgsRate = source.ItemCgsRate,
				TotalDebitAmount = source.TotalDebitAmount,
				TotalCreditAmount = source.TotalCreditAmount,
				ThirdCurrencyFcyExchangeRate = source.ThirdCurrencyFcyExchangeRate,
				ThirdCurrencyHcyExchangeRate = source.ThirdCurrencyHcyExchangeRate,
				ThirdCurrencyAmount = source.ThirdCurrencyAmount,
				ThirdCurrencyReceiverExchangeRate = source.ThirdCurrencyReceiverExchangeRate,
				ThirdCurrencyReceiverFcyAmount = source.ThirdCurrencyReceiverFcyAmount,
				ThirdCurrencyId = source.ThirdCurrencyId,
				AccountId = source.AccountId,
				AgainstAccountId = source.AgainstAccountId,
				DMultiCurrencyId = source.DMultiCurrencyId,
				DocumentTypeIdRef = source.DocumentTypeIdRef,
				GpNo = source.GpNo,
				InvoiceNoRefId = source.InvoiceNoRefId,
				ItemId = source.ItemId,
				JobLotId = source.JobLotId,
				OrderNo = source.OrderNo,
				SupplierCustomerId = source.SupplierCustomerId,
				EmployeeId = source.EmployeeId,
				SubsidiaryTypeId = source.SubsidiaryTypeId,
				SubsidiaryAccountId = source.SubsidiaryAccountId,
				SubsidiaryAgainstTypeId = source.SubsidiaryAgainstTypeId,
				SubsidiaryAgainstAccountId = source.SubsidiaryAgainstAccountId,
				TaxTypeId = source.TaxTypeId,
				ActionId = source.ActionId,
				VoucherHeadId = source.VoucherHeadId,
				RefDocSubIdNo = source.RefDocSubIdNo,
				LineId = source.LineId,
				InstrumentTypeId = source.InstrumentTypeId,
				SubNo = source.SubNo,
				SortNo = source.SortNo,
				IsCGS = source.IsCGS,
				CheqNoDetail = source.CheqNoDetail,
				Comments = source.Comments,
				CommentsOtherLingo = source.CommentsOtherLingo,
				DConversionFormula = source.DConversionFormula,
				IsTaxable = source.IsTaxable,
				PaymentType = source.PaymentType,
				RefInvoiceNo = source.RefInvoiceNo,
				TaxesRemarks = source.TaxesRemarks,
				VehicleNo = source.VehicleNo,
				PayeeTitle = source.PayeeTitle,
				Id = source.Id,
				PaymentTypeId = 0,
				ChequeTypeId = source.ChequeTypeId,
				BranchesId = 0,
				CostCenterId = 0,
				SBRTaxAmount = 0.0,
				DiscountPercent = 0.0,
				DiscountAmount = 0.0,
				ReferenceAccountId = 0,
				LocationTypeId = 0,
				AccountCode = source.AccountCode,
				AccountTitle = source.AccountTitle,
				CheqPartyName = source.CheqPartyName,
				JobLotDescription = source.JobLotDescription,
				SubsidiaryAccountTitle = source.SubsidiaryAccountTitle,
				InstrumentType = source.InstrumentType,
				BranchName = "",
				Location = "",
				CostCenterName = "",
				AgainstAccount = "",
				ReferenceAccount = "",
				AccountTypeId = 0
			};
			Destination.voucherDetailList.Add(List);
		}
	}

	private static void MapPaymentDueSchedule(VoucherHead sources, VoucherHead Destination)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		foreach (PaymentDueSchedule source in sources.PaymentDuesSchedules)
		{
			PaymentDueSchedule List = new PaymentDueSchedule
			{
				DueDate = source.DueDate,
				Amount = source.Amount,
				DuePercentage = source.DuePercentage,
				DueDays = source.DueDays,
				RefdocNoId = source.RefdocNoId,
				refDocumentTypeId = source.refDocumentTypeId,
				PaymentDueScheduleId = source.PaymentDueScheduleId
			};
			Destination.PaymentDuesSchedules.Add(List);
		}
	}

	private static void MapCostCenterDetail(VoucherHead sources, VoucherHead Destination)
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
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		foreach (voucherCostCenterDetail source in sources.costCenterDetailList)
		{
			voucherCostCenterDetail List = new voucherCostCenterDetail
			{
				costPrcent = source.costPrcent,
				costAmount = source.costAmount,
				ActionTypeId = source.ActionTypeId,
				CostCenterId = source.CostCenterId,
				Id = source.Id,
				SortNo = source.SortNo,
				VoucherDetailId = source.VoucherDetailId,
				VoucherHeaderId = source.VoucherHeaderId,
				AccountTitle = source.AccountTitle,
				ParentCostCenterId = source.ParentCostCenterId,
				CostCenterName = source.CostCenterName
			};
			Destination.costCenterDetailList.Add(List);
		}
	}
}

}
