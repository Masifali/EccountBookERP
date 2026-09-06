using System;
using System.Collections.Generic;
using System.Data;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model.Export;
using Architecture.Model.TaxProject.Export;

namespace Architecture.WinApp.DataSyncing.Mapping;

public class ExportVoucherMapping
{
	public static ExportVoucher ConvertLocalObjectToTaxObject(ExportVoucher Obj, DataTable TaxData = null)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
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
			ExportVoucher TaxObj = new ExportVoucher
			{
				OrganizationId = TaxOrganizationId,
				CompanyId = TaxCompanyId,
				FinancialYearId = TaxFinancialYearId,
				BranchesId = TaxBranchesId,
				EntryUserId = TaxDefaultUserId,
				ModifyUserId = TaxDefaultUserId,
				EntryDate = DateTime.Now,
				ModifyDate = DateTime.Now,
				IsApproved = false,
				DocumentTypeId = Obj.DocumentTypeId,
				DocDate = Obj.DocDate,
				DueDate = Obj.DueDate,
				ExchangeRate = Obj.ExchangeRate,
				FcyAmount = Obj.FcyAmount,
				LcyAmount = Obj.LcyAmount,
				NetWeight = Obj.NetWeight,
				DueDays = Obj.DueDays,
				CreditAccounId = Obj.CreditAccounId,
				CommissionExpenseAccountId = Obj.CommissionExpenseAccountId,
				CurrencyId = Obj.CurrencyId,
				ExImInvoiceId = Obj.ExImInvoiceId,
				InvoiceNo = Obj.InvoiceNo,
				SupplierCustomerId = Obj.SupplierCustomerId,
				AddLessAmount = Obj.AddLessAmount,
				FcyAmountWithoutAddLess = Obj.FcyAmountWithoutAddLess,
				Remarks = Obj.Remarks,
				Id = 0,
				DocNo = 0
			};
			TaxObj.ExportVoucherDetailslist = new List<ExportVoucherDetail>();
			TaxObj.ExImInvoiceOtherChargesDetailList = new List<ExImInvoiceOtherChargesDetail>();
			TaxObj.ExImInvoicePaymentTermsDetailList = new List<ExImInvoicePaymentTermsDetail>();
			TaxObj.ExImInvoiceSaleManCommissionList = new List<ExImInvoiceSaleManCommission>();
			ConvertDetailEntry(Obj, TaxObj, TaxData);
			ConvertOtherChargesDetail(Obj, TaxObj);
			ConvertPaymentTermDetailEntries(Obj, TaxObj);
			ConvertExpenseEntries(Obj, TaxObj);
			return TaxObj;
		}
		catch (Exception ex)
		{
			throw new Exception("Error converting sale invoice: " + ex.Message, ex);
		}
	}

	private static void ConvertOtherChargesDetail(ExportVoucher source, ExportVoucher Destination)
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
		//IL_0086: Expected O, but got Unknown
		foreach (ExImInvoiceOtherChargesDetail item in source.ExImInvoiceOtherChargesDetailList)
		{
			ExImInvoiceOtherChargesDetail List = new ExImInvoiceOtherChargesDetail
			{
				Id = 0,
				ExImInvoiceId = item.ExImInvoiceId,
				ChargesItemId = item.ChargesItemId,
				AddAmount = item.AddAmount,
				LessAmount = item.LessAmount,
				ContractId = item.ContractId,
				ChargesAccountId = item.ChargesAccountId,
				ContractOtherChargesDetailId = item.ContractOtherChargesDetailId,
				Remarks = item.Remarks
			};
			Destination.ExImInvoiceOtherChargesDetailList.Add(List);
		}
	}

	private static void ConvertPaymentTermDetailEntries(ExportVoucher Obj, ExportVoucher Destination)
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
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		foreach (ExImInvoicePaymentTermsDetail source in Obj.ExImInvoicePaymentTermsDetailList)
		{
			ExImInvoicePaymentTermsDetail item = new ExImInvoicePaymentTermsDetail
			{
				Id = 0,
				FcyAmount = source.FcyAmount,
				PrcntOfTotal = source.PrcntOfTotal,
				DueDays = source.DueDays,
				ExImEFormRegistrationId = source.ExImEFormRegistrationId,
				ExImInvoiceId = source.ExImInvoiceId,
				RefDocumentTypeId = source.RefDocumentTypeId,
				PaymentTermId = source.PaymentTermId,
				DocumentTypeId = source.DocumentTypeId,
				SortNo = source.SortNo,
				FinancialInstrumentNo = source.FinancialInstrumentNo,
				PaymentRemarks = source.PaymentRemarks
			};
			Destination.ExImInvoicePaymentTermsDetailList.Add(item);
		}
	}

	private static void ConvertExpenseEntries(ExportVoucher Obj, ExportVoucher Destination)
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
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		foreach (ExImInvoiceSaleManCommission source in Obj.ExImInvoiceSaleManCommissionList)
		{
			ExImInvoiceSaleManCommission item = new ExImInvoiceSaleManCommission
			{
				exImInvoiceSaleManCommissionId = 0,
				exImInvoiceId = source.exImInvoiceId,
				SalesPersonId = source.SalesPersonId,
				ActionTypeId = source.ActionTypeId,
				commissionTypeId = source.commissionTypeId,
				rateUom = source.rateUom,
				commissionRate = source.commissionRate,
				exchangeRate = source.exchangeRate,
				fcyAmount = source.fcyAmount,
				lcyAmount = source.lcyAmount,
				Remarks = source.Remarks
			};
			Destination.ExImInvoiceSaleManCommissionList.Add(item);
		}
	}

	private static void ConvertDetailEntry(ExportVoucher source, ExportVoucher Destination, DataTable TaxData = null)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		if (TaxData != null && TaxData.Rows.Count > 0)
		{
			Conversion.ToInt(TaxData.Rows[0]["TaxNameId"]);
			Conversion.ToDouble(TaxData.Rows[0]["TaxPercent"]);
		}
		foreach (ExportVoucherDetail item in source.ExportVoucherDetailslist)
		{
			ExportVoucherDetail List = new ExportVoucherDetail
			{
				Id = 0,
				CostRate = item.CostRate,
				ExchangeRate = item.ExchangeRate,
				FcyAmount = item.FcyAmount,
				LcyAmount = item.LcyAmount,
				Mton = item.Mton,
				NetWeight = item.NetWeight,
				Qty = item.Qty,
				RatePrice = item.RatePrice,
				StockValue = item.StockValue,
				StockWeight = item.StockWeight,
				CurrencyId = item.CurrencyId,
				EntryTypeId = item.EntryTypeId,
				ExImInvoiceDetailId = item.ExImInvoiceDetailId,
				ContractId = item.ContractId,
				WarehouseId = item.WarehouseId,
				CropYear = item.CropYear,
				ItemId = item.ItemId,
				ItemUomId = item.ItemUomId,
				LotJobId = item.LotJobId,
				PackingTypeId = item.PackingTypeId,
				RateUomId = item.RateUomId,
				LineId = item.LineId
			};
			Destination.ExportVoucherDetailslist.Add(List);
		}
	}
}
