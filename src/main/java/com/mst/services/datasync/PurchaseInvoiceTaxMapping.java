package com.mst.services.datasync;

import java.math.BigDecimal;
import java.math.MathContext;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Architecture.WinApp.DataSyncing.Mapping.PurchaseInvoiceMapping.ConvertLocalSaleObjectToTaxSaleObject(Obj, TaxData)
 * - the local purchase invoice (Architecture.Model.Inventory.InvPurchaseInvoice read by GetByID) turned into the
 * Architecture.Model.TaxProject.Inventory.InvPurchaseInvoice that BLL.TaxProject InvPurchaseInvoice.Save writes to the
 * tax server. Field for field, in the mapping's order; property names are the model's (and the column names the
 * read procedures return). frmPendingPurchaseInvoiceForUpload calls it without TaxData (:576), so TaxNameId and
 * TaxPercent are 0 here unless a caller passes them.
 *
 * Only the header, the detail list and the empty bags are copied: the mapping's ConvertJournalEntries /
 * ConvertExpenseEntries / ConvertFreightEntries exist but are never called, and the expense, freight, journal,
 * empty-bag (re-filled) and payment-terms lists start empty.
 */
public final class PurchaseInvoiceTaxMapping {

    private PurchaseInvoiceTaxMapping() { }

    /** The tax-server invoice: "header" (property -> value), "details" and "emptyBags" (lists of property maps). */
    public record TaxPurchaseInvoice(Map<String, Object> header, List<Map<String, Object>> details, List<Map<String, Object>> emptyBags) { }

    /**
     * @param h           the ReadById header row
     * @param details     invPurchaseInvoiceDetailList (never empty when called from the upload)
     * @param emptyBags   InvPurchaseInvoiceEmptyBagslist
     * @param taxSetup    USP_GetTaxCompanySetupByOrgAndCompany rows for Obj.OrganizationId / Obj.CompanyId
     * @param taxNameId   TaxData.Rows[0]["TaxNameId"] (0 when TaxData is null)
     * @param taxPercent  TaxData.Rows[0]["TaxPercent"] (0 when TaxData is null)
     */
    public static TaxPurchaseInvoice convert(Map<String, Object> h, List<Map<String, Object>> details, List<Map<String, Object>> emptyBags,
                                             List<Map<String, Object>> taxSetup, int taxNameId, double taxPercent) {
        try {
            if (taxSetup == null || taxSetup.isEmpty())
                throw new IllegalArgumentException("Tax company setup not found for the given company.");
            Map<String, Object> s = taxSetup.get(0);
            int taxOrganizationId = toInt32(s.get("TaxOrganizationId"));
            int taxCompanyId = toInt32(s.get("TaxCompanyId"));
            int taxFinancialYearId = toInt32(s.get("TaxFinancialYearId"));
            int taxBranchesId = toInt32(s.get("TaxBranchesId"));
            int taxDefaultUserId = toInt32(s.get("TaxDefaultUserId"));
            LocalDateTime now = LocalDateTime.now();

            Map<String, Object> t = new LinkedHashMap<>();
            t.put("OrganizationId", taxOrganizationId);
            t.put("CompanyId", taxCompanyId);
            t.put("FinancialYearId", taxFinancialYearId);
            t.put("BranchesId", taxBranchesId);
            t.put("ProjectsId", h.get("ProjectsId"));
            t.put("EntryUser", taxDefaultUserId);
            t.put("ModifyUser", taxDefaultUserId);
            t.put("PostUser", taxDefaultUserId);
            t.put("EntryDate", now);
            t.put("ModifyDate", now);
            t.put("PostDate", now);
            t.put("IsApproved", false);
            t.put("CustomAccounts", true);
            t.put("DocDate", h.get("DocDate"));
            t.put("SupplierInvoiceDate", h.get("SupplierInvoiceDate"));
            t.put("CashReceived", h.get("CashReceived"));
            t.put("FreightAmount", h.get("FreightAmount"));
            t.put("TransporterDebitGLId", 0);
            t.put("TransporterDebitPartyId", 0);
            t.put("TransporterCreditPartyId", 0);
            t.put("DueDays", h.get("DueDays"));
            t.put("DueDate", h.get("DueDate"));
            t.put("PaymentTermsId", h.get("PaymentTermsId"));
            t.put("DeliveryTerm", h.get("DeliveryTerm"));
            t.put("CommissionAgentId", 0);
            t.put("CommissionType", "");
            t.put("UomScheduleIdCmRate", "");
            t.put("CommRate", 0.0);
            t.put("CommAmount", 0.0);
            t.put("CommissionRemarks", "");
            t.put("SalesTaxNo", h.get("SalesTaxNo"));
            t.put("DocumentTypeSrNo", h.get("DocumentTypeSrNo"));
            t.put("ReferencePartyId", h.get("ReferencePartyId"));
            t.put("StockPartyId", h.get("StockPartyId"));
            t.put("SupplierCustomerId", h.get("SupplierCustomerId"));
            t.put("SupplierInvoiceNo", h.get("SupplierInvoiceNo"));
            t.put("SupplierReferenceNo", h.get("SupplierReferenceNo") == null ? "" : String.valueOf(h.get("SupplierReferenceNo")));
            t.put("TransportAccountId", h.get("TransportAccountId"));
            t.put("CommissionCreditAccountId", 0);
            t.put("DiscountAccountId", 0);
            t.put("InvoiceQty", h.get("InvoiceQty"));
            t.put("InvoiceWeight", h.get("InvoiceWeight"));
            t.put("CurrencyId", h.get("CurrencyId"));
            t.put("ExchangeRate", h.get("ExchangeRate"));
            t.put("DiscountAmount", 0.0);
            t.put("FcyAmount", h.get("FcyAmount"));
            t.put("IsAttachments", h.get("IsAttachments"));
            t.put("ManualBillNo", h.get("ManualBillNo"));
            t.put("OtherRemarks", h.get("OtherRemarks"));
            t.put("RemarksHeader", h.get("RemarksHeader"));
            t.put("ScreenName", h.get("ScreenName"));
            t.put("FreightRemark", "");
            t.put("IsTaxable", num(h.get("TaxAccountId")) > 0);
            t.put("TaxAccountId", h.get("TaxAccountId"));
            t.put("BillAmount", h.get("BillAmount"));
            t.put("Id", 0);
            t.put("DocNo", 0);
            t.put("DocumentTypeId", 57);
            t.put("BranchSrNo", 0);
            t.put("AttachmentsValues", "");
            t.put("CustomAttachmentsValues", "");
            // GetByID does not load AttachmentsList / DeleteAttachmentsList (DAL 0434 GetDate), so both are null.
            t.put("AttachmentsList", null);
            t.put("DeleteAttachmentsList", h.get("DeleteAttachmentsList"));

            // ConvertEmptyBagDetail
            List<Map<String, Object>> bags = new ArrayList<>();
            for (Map<String, Object> b : emptyBags == null ? List.<Map<String, Object>>of() : emptyBags) {
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", 0);
                for (String k : new String[] { "InvPurchaseInvoiceId", "ItemId", "TypeId", "PurchaseQty", "ReceivedQty", "Rate",
                        "Amount", "CreditAccountId", "Remarks" }) x.put(k, b.get(k));
                bags.add(x);
            }

            // ConvertDetailEntry
            List<Map<String, Object>> lines = new ArrayList<>();
            for (Map<String, Object> d : details == null ? List.<Map<String, Object>>of() : details) {
                double taxAmount = 0.0;
                if (taxNameId > 0 && taxPercent > 0.0) taxAmount = num(d.get("ItemAmount")) * taxPercent / 100.0;
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", 0);
                x.put("InvGrnDetailId", 0);
                x.put("InvGrnId", 0);
                x.put("PurchaseOrderId", 0);
                x.put("PurchaseOrderDetailId", 0);
                copy(d, x, "InvPurchaseInvoiceId", "WarehouseId", "JobLotId", "CropYear", "ItemId", "ItemUOMId", "PackingTypeId",
                        "ItemQty", "GrossWeight", "WeightCut", "WeightCutTotal", "EBTotalWt", "EBWeight", "AdLsWeight", "NetBillWeight",
                        "NetStockWeight", "RateCut", "RateCutAmount", "ItemRate", "ItemAmount", "GpDate", "AvgCgsRate");
                x.put("CommissionAmount", 0.0);
                x.put("ExpenseAmount", 0.0);
                x.put("FreightAmount", 0.0);
                copy(d, x, "ItemCgsRate");
                x.put("JournalAmount", 0.0);
                copy(d, x, "Brokery");
                x.put("DiscountTypeId", 0);
                x.put("DiscountPercent", 0.0);
                x.put("DiscountAmount", 0.0);
                copy(d, x, "EbPurAgainstWeight", "ExchangeRate", "FcyAmount", "CurrencyId", "GpNo", "UomScheduleIdRate");
                x.put("RefDocumentTypeId", 0);
                x.put("RefDocId", 0);
                x.put("RefDocSubId", 0);
                copy(d, x, "LineId");
                x.put("CityId", 0);
                copy(d, x, "IsTaxable", "LabAnalisysNo", "RemarksDetail", "TaxDescriptions", "VehicleNo");
                x.put("TaxNameId", num(d.get("TaxNameId")) > 0 ? d.get("TaxNameId") : taxNameId);
                boolean ownPercent = num(d.get("TaxPercent")) > 0.0;
                x.put("TaxPercent", ownPercent ? d.get("TaxPercent") : taxPercent);
                x.put("TaxAmount", ownPercent ? d.get("TaxAmount") : taxAmount);
                copy(d, x, "BillAmount");
                lines.add(x);
            }

            double itemAmount = lines.stream().mapToDouble(r -> num(r.get("ItemAmount"))).sum();
            double tax = lines.stream().mapToDouble(r -> num(r.get("TaxAmount"))).sum();
            t.put("BillAmount", itemAmount + tax);
            BigDecimal exchangeRate = decimal(t.get("ExchangeRate"));
            t.put("FcyAmount", exchangeRate.signum() > 0
                    ? BigDecimal.valueOf(itemAmount + tax).divide(exchangeRate, MathContext.DECIMAL128) : BigDecimal.ZERO);
            return new TaxPurchaseInvoice(t, lines, bags);
        } catch (RuntimeException ex) {
            throw new IllegalArgumentException("Error converting sale invoice: " + ex.getMessage(), ex);
        }
    }

    private static void copy(Map<String, Object> from, Map<String, Object> to, String... keys) {
        for (String k : keys) to.put(k, from.get(k));
    }

    /** Convert.ToInt32(object): DBNull throws, as on the desktop. */
    private static int toInt32(Object v) {
        if (v == null) throw new IllegalArgumentException("Object cannot be cast from DBNull to other types.");
        if (v instanceof Number n) return n.intValue();
        return Integer.parseInt(String.valueOf(v).trim());
    }

    private static double num(Object v) {
        if (v instanceof Number n) return n.doubleValue();
        if (v instanceof Boolean b) return b ? 1 : 0;
        try { return v == null ? 0 : Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static BigDecimal decimal(Object v) {
        if (v instanceof BigDecimal b) return b;
        return BigDecimal.valueOf(num(v));
    }
}
