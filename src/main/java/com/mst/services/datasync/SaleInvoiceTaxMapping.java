package com.mst.services.datasync;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

/**
 * Architecture.WinApp.DataSyncing.Mapping.SaleInvoiceMapping.ConvertLocalSaleObjectToTaxSaleObject(Obj, TaxData) - the local
 * sale invoice (InvSaleInvoice read by GetByID) turned into the InvSaleInvoice that BLL.TaxProject InvSaleInvoice.Save writes
 * to the tax server. Field for field, in the mapping's order (SaleInvoiceMapping.cs :17-517). frmPendingSaleInvoiceForUpload
 * calls it without TaxData (:553), so TaxNameId and TaxPercent are 0 unless the line already carries them.
 *
 * Unlike the purchase mapping this one also copies the payment-term lines (ConvertPaymentTermDetail) and the detail lines;
 * ConvertCommissionEntries / ConvertJournalEntries / ConvertExpenseEntries / ConvertFreightEntries exist in the C# class but
 * ConvertLocalSaleObjectToTaxSaleObject never calls them, so those four lists stay empty.
 *
 * When the local invoice has no TaxAccountId the desktop first asks the TAX SERVER for the latest tax schedule
 * (InvSaleInvoice.TaxScheduleMain_GetLatestSchedule -> GetDataTableProcForServer). That connection does not exist in the web
 * application, so the caller refuses at that point (see {@link #needsTaxServerSchedule}).
 */
public final class SaleInvoiceTaxMapping {

    private SaleInvoiceTaxMapping() { }

    public record TaxSaleInvoice(Map<String, Object> header, List<Map<String, Object>> details, List<Map<String, Object>> paymentTerms) { }

    /** SaleInvoiceMapping :40-48. */
    public static void requireTaxSetup(List<Map<String, Object>> taxSetup) {
        if (taxSetup == null || taxSetup.isEmpty()) throw new IllegalArgumentException("Error converting sale invoice: Tax company setup not found for the given company.");
    }

    /** :50 - the TaxAccountId of the local invoice is not above zero, so the tax server's latest schedule is required. */
    public static boolean needsTaxServerSchedule(Map<String, Object> h) { return num(ci(h).get("TaxAccountId")) <= 0; }

    public static TaxSaleInvoice convert(Map<String, Object> header, List<Map<String, Object>> details, List<Map<String, Object>> paymentTerms,
                                         List<Map<String, Object>> taxSetup) {
        requireTaxSetup(taxSetup);
        Map<String, Object> h = ci(header);
        Map<String, Object> s = ci(taxSetup.get(0));
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> t = new LinkedHashMap<>();
        t.put("OrganizationId", num(s.get("TaxOrganizationId")));
        t.put("CompanyId", num(s.get("TaxCompanyId")));
        t.put("FinancialYearId", num(s.get("TaxFinancialYearId")));
        t.put("BranchesId", num(s.get("TaxBranchesId")));
        int user = num(s.get("TaxDefaultUserId"));
        t.put("EntryUser", user); t.put("ModifyUser", user); t.put("PostUser", user);
        t.put("EntryDate", now); t.put("ModifyDate", now); t.put("PostDate", now);
        t.put("IsApproved", false);
        t.put("CustomAccounts", true);
        for (String k : new String[] {"DocDate", "DueDate", "SupplierInvoiceDate", "CashReceived"}) t.put(k, h.get(k));
        t.put("CommAmount", 0.0); t.put("CommRate", 0.0); t.put("FreightAmount", 0.0);
        t.put("TransporterDebitGLId", 0); t.put("TransporterDebitPartyId", 0); t.put("TransporterCreditPartyId", 0);
        t.put("DueDays", h.get("DueDays"));
        t.put("CommissionAgentId", 0);
        for (String k : new String[] {"SalesTaxNo", "DocumentTypeSrNo", "ProjectsId", "ReferencePartyId", "StockPartyId", "SupplierCustomerId",
                "SupplierInvoiceNo", "SupplierReferenceNo"}) t.put(k, h.get(k));
        t.put("TransporterId", 0); t.put("CommissionDebitAcGLId", 0);
        for (String k : new String[] {"PaymentTermId", "DiscountAccountId", "InvoiceQty", "InvoiceWeight", "CurrencyId", "OtherCategoryId",
                "ExchangeRate", "DiscountAmount", "FcyAmount"}) t.put(k, h.get(k));
        t.put("CommissionRemarks", ""); t.put("CommissionType", "");
        for (String k : new String[] {"IsAttachments", "IsReserve", "ManualBillNo", "OtherRemarks", "RemarksHeader"}) t.put(k, h.get(k));
        t.put("UomScheduleIdCmRate", "");
        t.put("DeliveryTerm", h.get("DeliveryTerm"));
        t.put("ScreenName", h.get("ScreenName"));
        t.put("FreightRemark", "");
        // :50-53 - TaxAccountId stays the local value here (the tax-server schedule is only read when it is <= 0).
        int taxAccountId = num(h.get("TaxAccountId"));
        t.put("IsTaxable", taxAccountId > 0);
        t.put("TaxAccountId", taxAccountId);
        t.put("BillAmount", h.get("BillAmount"));
        t.put("Id", 0); t.put("DocNo", 0); t.put("DocumentTypeId", 99); t.put("BranchSrNo", 0); t.put("AutoUpdate", 0);
        t.put("LocationTypeId", 0); t.put("TransTypeId", 0); t.put("CustomgroupId", 0);
        t.put("AttachmentsValues", ""); t.put("CustomAttachmentsValues", "");
        // GetByID does not load AttachmentsList / DeleteAttachmentsList (DAL 0433 GetData), so both are null.
        t.put("AttachmentsList", null); t.put("DeleteAttachmentsList", null);

        // ConvertPaymentTermDetail
        List<Map<String, Object>> terms = new ArrayList<>();
        for (Map<String, Object> p0 : paymentTerms == null ? List.<Map<String, Object>>of() : paymentTerms) {
            Map<String, Object> p = ci(p0);
            Map<String, Object> x = new LinkedHashMap<>();
            for (String k : new String[] {"Amount", "PrcntOfTotal", "DueDate", "DueDays"}) x.put(k, p.get(k));
            x.put("Id", 0);
            for (String k : new String[] {"InvSaleInvoiceId", "PaymentTermId", "SortNo", "PaymentRemarks"}) x.put(k, p.get(k));
            terms.add(x);
        }

        // ConvertDetailEntry (TaxData == null -> TaxNameId = 0, TaxPercent = 0)
        double taxNameId = 0, taxPercent = 0;
        boolean discountAccount = num(h.get("DiscountAccountId")) > 0;
        List<Map<String, Object>> lines = new ArrayList<>();
        double itemTotal = 0, taxTotal = 0;
        for (Map<String, Object> d0 : details == null ? List.<Map<String, Object>>of() : details) {
            Map<String, Object> d = ci(d0);
            double taxAmount = taxNameId > 0 && taxPercent > 0 ? dbl(d.get("ItemAmount")) * taxPercent / 100.0 : 0.0;
            Map<String, Object> x = new LinkedHashMap<>();
            for (String k : new String[] {"GpDate", "DueDate", "AdLsWeight", "AvgCgsRate"}) x.put(k, d.get(k));
            x.put("CommissionAmount", 0.0);
            x.put("EBTotalWt", d.get("EBTotalWt")); x.put("EBWeight", d.get("EBWeight"));
            x.put("ExpenseAmount", 0.0); x.put("FreightAmount", 0.0);
            x.put("GrossWeight", d.get("GrossWeight"));
            double amount = discountAccount ? dbl(d.get("ItemAmount")) : dbl(d.get("ItemAmount")) - dbl(d.get("ItemDiscountAmount"));
            x.put("ItemAmount", amount);
            for (String k : new String[] {"ItemCgsRate", "ItemQty", "ItemRate"}) x.put(k, d.get(k));
            x.put("JournalAmount", 0.0);
            for (String k : new String[] {"NetBillWeight", "NetStockWeight", "RateCut", "RateCutAmount", "WeightCut", "WeightCutTotal", "ItemDiscount",
                    "DiscountTypeId", "ItemDiscountAmount", "ItemRateWOExp", "PackingAddLess", "ExchangeRate", "FcyAmount", "CurrencyId", "GpNo"}) x.put(k, d.get(k));
            x.put("Id", 0);
            x.put("InvSaleInvoiceId", d.get("InvSaleInvoiceId"));
            x.put("InvGdnDetailId", 0); x.put("InvGdnId", 0);
            for (String k : new String[] {"ItemId", "ItemUOMId", "JobLotId", "PackingTypeId"}) x.put(k, d.get(k));
            x.put("SaleOrderId", 0); x.put("SaleOrderDetailId", 0);
            for (String k : new String[] {"UomScheduleIdRate", "WarehouseId", "InvForwardingId", "InvForwardingDetailId", "RefRefDocumentTypeId",
                    "RefRefDocIdNo", "RefDocSubId", "LineId", "CityId", "ReserveWareHouse", "PaymentTermId", "BillCalculateTypeId", "DueDays",
                    "CgsRateUomId", "CropYear", "IsTaxable", "LabAnalisysNo", "RemarksDetail", "TaxDescriptions", "VehicleNo", "ReferenceNo", "CommOnSale"})
                x.put(k, d.get(k));
            x.put("TaxNameId", num(d.get("TaxNameId")) > 0 ? d.get("TaxNameId") : (Object) 0);
            x.put("TaxPercent", dbl(d.get("TaxPercent")) > 0 ? d.get("TaxPercent") : (Object) 0.0);
            x.put("TaxAmount", dbl(d.get("TaxPercent")) > 0 ? d.get("TaxAmount") : (Object) taxAmount);
            x.put("BillAmount", d.get("BillAmount"));
            itemTotal += amount; taxTotal += dbl(x.get("TaxAmount"));
            lines.add(x);
        }
        double bill = itemTotal + taxTotal;
        t.put("BillAmount", bill);
        double rate = dbl(h.get("ExchangeRate"));
        t.put("FcyAmount", rate == 0 ? null : bill / rate);
        return new TaxSaleInvoice(t, lines, terms);
    }

    private static Map<String, Object> ci(Map<String, Object> m) {
        Map<String, Object> out = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        if (m != null) out.putAll(m);
        return out;
    }

    private static int num(Object v) { return (int) Math.rint(dbl(v)); }

    private static double dbl(Object v) {
        if (v instanceof Number n) return n.doubleValue();
        if (v instanceof Boolean b) return b ? 1 : 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }
}
