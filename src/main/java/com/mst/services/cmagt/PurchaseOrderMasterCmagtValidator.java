package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderMasterCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Server-side reproduction of the save-time refusals that frmSupplierOfferCmagt and
 * frmPurchaseOrderCmagt perform before they call purchaseOrderMaster.Save(obj).
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS IS SHARED BETWEEN THE TWO SCREENS
 * ---------------------------------------------------------------------------------------------
 * Not because the field names look alike. The two forms' `formvalidation()` bodies were diffed
 * line by line and are character-for-character identical - same order, same conditions, same
 * message strings, including the desktop's own "Paymrnt" typo:
 *
 *     frmSupplierOfferCmagt.cs:2891-2933   ==   frmPurchaseOrderCmagt.cs (same method)
 *
 * They also build the SAME model (Architecture.Model.CommissionAgent.purchaseOrderMaster), run
 * the same BLL, and write through the same procedures; only DocumentTypeId differs (1051 vs
 * 1052). A screen whose rules later diverge must stop using this class rather than have a flag
 * added to it.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY SERVER-SIDE AND NOT ONLY IN THE PAGE
 * ---------------------------------------------------------------------------------------------
 * The browser already refuses these, but a crafted POST to /save would otherwise walk straight
 * past every one of them into a procedure that accepts whatever it is given. Desktop business
 * rules must not be bypassable by not using the desktop.
 *
 * Every check below cites the desktop line that establishes it. Nothing here is invented: where
 * the desktop tolerates something (a zero weight-cut when the FOC term is chosen, an empty
 * payment grid) this tolerates it too.
 */
@Component
public class PurchaseOrderMasterCmagtValidator {

    @Autowired private JdbcTemplate jdbcTemplate;

    /** Desktop compares with these exact tolerances - they are not rounding guesses. */
    private static final BigDecimal AMOUNT_TOLERANCE  = new BigDecimal("0.3");   // :3210
    private static final BigDecimal PERCENT_TOLERANCE = new BigDecimal("0.01");  // :3218
    private static final BigDecimal HUNDRED           = new BigDecimal("100");

    /** EBWeightDeductionTermId 3 = "Bag FOC and weight cut NOT applied" (Insert():3051). */
    private static final int EB_TERM_FOC_NO_WEIGHT_CUT = 3;

    public void validate(PurchaseOrderMasterCmagtDto dto) {
        header(dto);
        detail(dto);
        emptyBags(dto);
        payment(dto);
        mapping(dto);
        saleOrderMapping(dto);
    }

    /* ------------------------------------------------------------------ formvalidation() */

    /**
     * frmSupplierOfferCmagt.cs:2891-2933 / frmPurchaseOrderCmagt.cs, in the desktop's order.
     *
     * Note the payment-term branch: the desktop only demands a header payment term when the
     * payment SCHEDULE GRID sums to zero (:2905). A document that carries its own schedule is
     * not required to have one, so this does not demand it either.
     */
    private void header(PurchaseOrderMasterCmagtDto dto) {
        if (zero(dto.getCommissionAgentId())) fail("Please Select Commission Agent / Broker");  // :2893
        if (zero(dto.getSupplierId()))        fail("Please Select Supplier Name");              // :2899

        if (scheduleAmountSum(dto).compareTo(BigDecimal.ZERO) == 0) {                            // :2905
            Integer term = headerPaymentTermId(dto);
            if (zero(term)) fail("Please Select Paymrnt Term");                                  // :2909 (desktop's spelling)
            if (term != null && term == 2 && zero(headerDueDays(dto)))
                fail("Due Days field is required");                                             // :2915
        }

        if (zero(dto.getDeliveryTermId())) fail("Please Select Delivery Term");                  // :2921
        if (zero(dto.getDeliveryDays()))   fail("Delivery Days field is required");              // :2927

        /* Insert():2987 - the grid must not be empty, checked before the model is even built. */
        if (dto.getPurchaseOrderDetailList() == null || liveDetail(dto).isEmpty())
            fail("Detail record not found. Please check!");
    }

    /* ---------------------------------------------------------------------------- detail */

    /**
     * Insert():3067-3084 - FormHelper.ValidateField per column, per row, in that order. The
     * desktop names the row by its grid index, so "row#1" here is the first grid row.
     *
     * The tax trio is conditional exactly as the desktop has it (:3078): a row with no tax at
     * all is legal; a row with ANY of name / percent / amount must have all three.
     */
    private void detail(PurchaseOrderMasterCmagtDto dto) {
        List<PurchaseOrderMasterCmagtDto.DetailDto> rows = liveDetail(dto);
        Set<Integer> seenItems = new HashSet<>();

        for (int i = 0; i < rows.size(); i++) {
            PurchaseOrderMasterCmagtDto.DetailDto d = rows.get(i);
            int n = i + 1;
            req(d.getInventoryParentCategoryId(), "Parent Category", n);   // :3068
            req(d.getItemId(),                    "Item Name",       n);   // :3069
            req(d.getCropYearId(),                "Crop Year",       n);   // :3070
            req(d.getPackingTypeId(),             "Packing Type",    n);   // :3071
            req(d.getPackUomId(),                 "Pack Uom",        n);   // :3072
            req(d.getItemQty(),                   "Qty",             n);   // :3073
            req(d.getItemWeight(),                "Weight",          n);   // :3074
            req(d.getItemRate(),                  "Rate",            n);   // :3075
            req(d.getRateUomId(),                 "Rate Uom",        n);   // :3076
            req(d.getItemAmount(),                "Amount",          n);   // :3077

            boolean anyTax = !zero(d.getTaxNameId())
                    || pos(d.getTaxPercent())
                    || pos(d.getTaxAmount());
            if (anyTax) {                                                   // :3078-3083
                req(d.getTaxNameId(),  "Tax Name",    n);
                req(d.getTaxPercent(), "Tax Percent", n);
                req(d.getTaxAmount(),  "Tax Amount",  n);
            }
            req(d.getTotalAmount(), "Total Amount", n);                     // :3084

            /* BtnAdd_Click:1943 / BtnUpdateDetail_Click:2021 - one row per item. The desktop
               enforces this as the row is added; a crafted payload could otherwise carry the
               same item twice, which the grid cannot produce. */
            if (d.getItemId() != null && !seenItems.add(d.getItemId()))
                fail("Item in row#" + n + " already in detail. So can't add this item");
        }
    }

    /* ------------------------------------------------------------------------- empty bags */

    /**
     * Insert():3088-3125.
     *
     * entryTypeId 1 rows (the weight-cut grid) are checked ONLY when the chosen deduction term
     * is not "Bag FOC and weight cut not applied" (:3091) - under that term the desktop skips
     * the whole block, so a zero weight-cut is legal there.
     *
     * The min/max range comes from InvPackingType for a SUBSTITUTED packing type: ids 1, 2 and 5
     * validate against themselves, everything else validates against id 2 (:3097-3102). That
     * substitution is the desktop's, not a simplification.
     */
    private void emptyBags(PurchaseOrderMasterCmagtDto dto) {
        if (dto.getPurchaseOrderEmptyBagDetailList() == null) return;
        boolean focTerm = dto.getEBWeightDeductionTermId() != null
                && dto.getEBWeightDeductionTermId() == EB_TERM_FOC_NO_WEIGHT_CUT;

        Map<Integer, Map<String, Object>> packTypes = packingTypeRanges();

        for (PurchaseOrderMasterCmagtDto.EmptyBagDto b : dto.getPurchaseOrderEmptyBagDetailList()) {
            int entryType = b.getEntryTypeId() == null ? 1 : b.getEntryTypeId();

            if (entryType == 2) {
                /* :3111-3123 - a packing-material row only counts when both ids are set, and
                   then its rate is mandatory. */
                if (zero(b.getEmptyBagPackingMaterialItemId()) || zero(b.getPackingTypeId())) continue;
                if (!pos(b.getRate())) fail("Rate filed required In Empty bags Pm Grid...");
                continue;
            }

            if (focTerm) continue;                                           // :3091

            if (!pos(b.getWeightCutKg()))
                fail("WeightCut filed required In Empty bags Grid...");       // :3095

            int declared = b.getPackingTypeId() == null ? 0 : b.getPackingTypeId();
            int validationTypeId = (declared == 1 || declared == 2 || declared == 5) ? declared : 2;  // :3097-3102
            Map<String, Object> pt = packTypes.get(validationTypeId);
            if (pt == null) continue;                                        // desktop: packType == null -> no check

            double cut = b.getWeightCutKg().doubleValue();
            double min = dbl(pt.get("MinEbWeight"));
            double max = dbl(pt.get("MaxEbWeight"));
            if (cut < min || cut > max) {                                    // :3103
                fail("Weight Cut Should be in Range of: " + trim(min) + " to " + trim(max) + "\n"
                   + "For Packing Type:" + str(pt.get("PackTypeDesc")));
            }
        }
    }

    /* ---------------------------------------------------------------------------- payment */

    /**
     * Insert():3168-3223.
     *
     * Per row (:3175-3190) then the two reconciliations (:3210-3223). The comparison base is
     * the sum of the detail rows' TotalAmount - DetailSumAmount is accumulated at :3086 from
     * vd.TotalAmount, NOT from ItemAmount, so tax is included on both sides.
     */
    private void payment(PurchaseOrderMasterCmagtDto dto) {
        List<PurchaseOrderMasterCmagtDto.PaymentDto> rows =
                dto.getPurchaseOrderPaymentDetailList() == null
                        ? new ArrayList<>() : dto.getPurchaseOrderPaymentDetailList();

        BigDecimal detailSum = BigDecimal.ZERO;
        for (PurchaseOrderMasterCmagtDto.DetailDto d : liveDetail(dto)) {
            detailSum = detailSum.add(nz(d.getTotalAmount()));               // :3086
        }

        BigDecimal paid = BigDecimal.ZERO;
        BigDecimal pct  = BigDecimal.ZERO;
        for (int i = 0; i < rows.size(); i++) {
            PurchaseOrderMasterCmagtDto.PaymentDto p = rows.get(i);
            int n = i + 1;
            if (zero(p.getPaymentTermId()))
                fail("Payment Term Required in row#" + n);                    // :3178
            if (p.getPaymentTermId() == 2 && zero(p.getDueDays()))
                fail("Due Days Required In case Of Credit row in row#" + n);  // :3184
            paid = paid.add(nz(p.getDueAmount()));
            pct  = pct.add(nz(p.getPctOfTotal()));
        }

        if (rows.isEmpty()) return;   /* :3192 - the desktop synthesizes one 100% row instead */

        if (paid.subtract(detailSum).abs().compareTo(AMOUNT_TOLERANCE) > 0) { // :3210
            fail("Payment Detail Amount:" + money(paid)
               + " Not Equal to Total Amount:" + money(detailSum));
        }
        BigDecimal rounded = pct.setScale(4, RoundingMode.HALF_UP);           // :3216
        if (HUNDRED.subtract(rounded).abs().compareTo(PERCENT_TOLERANCE) > 0) {
            fail("Payment Detail Total% not near to 100");                    // :3220
        }
    }

    /* ---------------------------------------------------------------------------- mapping */

    /**
     * Insert():3226-3300 - the Buyer-Inquiry mapping grid (Supplier Offer) and the Sale-Order
     * mapping grid (Purchase Order) are validated by the same rules.
     *
     * Order matters: the desktop reports a missing id per row first, then the supplier/buyer
     * clash, then the unknown-item list, and only then the per-item weight ceiling.
     */
    private void mapping(PurchaseOrderMasterCmagtDto dto) {
        List<PurchaseOrderMasterCmagtDto.OfferInquiryMappingDto> map =
                dto.getSupplierOfferBuyerInquiryMappingDetailList();
        if (map == null || map.isEmpty()) return;

        Map<Integer, BigDecimal> offerWeight = new HashMap<>();
        for (PurchaseOrderMasterCmagtDto.DetailDto d : liveDetail(dto)) {
            if (d.getItemId() == null) continue;
            offerWeight.merge(d.getItemId(), nz(d.getItemWeight()), BigDecimal::add);
        }

        Set<String> unknownItems = new LinkedHashSet<>();
        Map<Integer, BigDecimal> mapped = new HashMap<>();

        for (int i = 0; i < map.size(); i++) {
            PurchaseOrderMasterCmagtDto.OfferInquiryMappingDto m = map.get(i);
            /* rows queued for deletion carry actionTypeId 3 and are not re-validated */
            if (m.getActionTypeId() != null && m.getActionTypeId() == 3) continue;

            if (zero(m.getInquiryBookingMasterId())) fail("Inquiry Id not found. please check");        // :3233
            if (zero(m.getInquiryBookingDetailId())) fail("Inquiry detail Id not found. please check"); // :3239
            if (zero(m.getBuyerId()))                fail("buyer not found. please check");             // :3246
            if (zero(m.getItemId()))                 fail("Item not found. please check");              // :3252
            if (!pos(m.getItemNetWeight()))
                fail("Allocation weight should be greater than '0'. please check");                     // :3257

            /* :3283 - a party cannot be both sides of its own deal. */
            if (dto.getSupplierId() != null && m.getBuyerId().equals(dto.getSupplierId()))
                fail("Row " + (i + 1) + ": Supplier and buyer cannot be the same in mapping grid.");

            if (!offerWeight.containsKey(m.getItemId())) {                                              // :3261
                unknownItems.add("ItemId " + m.getItemId());
            } else {
                mapped.merge(m.getItemId(), nz(m.getItemNetWeight()), BigDecimal::add);
            }
        }

        if (!unknownItems.isEmpty()) {                                                                  // :3290
            fail("Following item(s) are not present in Purchase Order Detail: "
               + String.join(", ", unknownItems)
               + ". Please delete those rows which contain these items from mapping.");
        }

        for (Map.Entry<Integer, BigDecimal> e : mapped.entrySet()) {                                    // :3297
            BigDecimal offer = offerWeight.get(e.getKey());
            if (offer != null && e.getValue().compareTo(offer) > 0) {
                fail("Mapped weight (" + money(e.getValue())
                   + ") cannot be greater than Offer weight (" + money(offer)
                   + ") for Item " + e.getKey());
            }
        }
    }

    /* ------------------------------------------------------------------ sale-order mapping */
    /**
     * frmPurchaseOrderCmagt Insert():3225-3300 - the Purchase Order's own mapping grid
     * (grdSaleOrderMapping -> purchaseOrderSaleOrderMappingList). Supplier Offer never sends
     * this list, so for 1051 the method returns at once.
     *
     * Desktop order: per-row id/weight refusals inside the loop (:3232-3262), rows whose item is
     * not in the live detail are collected rather than kept (:3263-3272); then, over the KEPT
     * rows only, the expiry check (:3274-3281) and the supplier = buyer check (:3282-3289); then
     * the unknown-item list (:3290); then per live detail row "No mapping found" and the weight
     * ceiling (:3292-3300). Rows the operator removed (lstRemoveRecordMappingDetail,
     * actionTypeId 3) are appended only after all of this (:3302), so they are skipped here.
     */
    private void saleOrderMapping(PurchaseOrderMasterCmagtDto dto) {
        List<PurchaseOrderMasterCmagtDto.SaleOrderMappingDto> all = dto.getPurchaseOrderSaleOrderMappingList();
        if (all == null || all.isEmpty()) return;
        List<PurchaseOrderMasterCmagtDto.DetailDto> live = liveDetail(dto);
        Map<Integer, String> validItems = new HashMap<>();
        for (PurchaseOrderMasterCmagtDto.DetailDto d : live) {
            validItems.put(d.getItemId() == null ? 0 : d.getItemId(), d.getItemName());
        }
        List<PurchaseOrderMasterCmagtDto.SaleOrderMappingDto> kept = new ArrayList<>();
        Set<String> invalidItems = new LinkedHashSet<>();
        for (PurchaseOrderMasterCmagtDto.SaleOrderMappingDto m : all) {
            if (m.getActionTypeId() != null && m.getActionTypeId() == 3) continue;
            if (zero(m.getSaleOrderMasterId())) fail("Sale Order Id not found. please check");          // :3235
            if (zero(m.getSaleOrderDetailId())) fail("Sale Order detail Id not found. please check");   // :3240
            if (zero(m.getBuyerId()))           fail("buyer not found. please check");                  // :3245
            if (zero(m.getItemId()))            fail("Item not found. please check");                   // :3250
            if (!pos(m.getItemNetWeight()))
                fail("Allocation weight should be greater than '0'. please check");                     // :3255
            if (!validItems.containsKey(m.getItemId())) {                                               // :3265
                String name = m.getItemName();
                invalidItems.add(name != null && !name.trim().isEmpty() ? name : "ItemId " + m.getItemId());
            } else {
                kept.add(m);
            }
        }
        java.time.LocalDate poExpiry = day(dto.getValidityDate());
        for (int i = 0; i < kept.size(); i++) {                                                          // :3274-3281
            java.time.LocalDate soExpiry = day(kept.get(i).getValidityDate());
            if (poExpiry != null && (soExpiry == null || soExpiry.isBefore(poExpiry)))
                fail("Row " + (i + 1) + ": Sale order Expiry Date is Less Than Po Expiry Date in mapping grid.");
        }
        for (int i = 0; i < kept.size(); i++) {                                                          // :3282-3289
            if (dto.getSupplierId() != null && dto.getSupplierId().equals(kept.get(i).getBuyerId()))
                fail("Row " + (i + 1) + ": Supplier and buyer cannot be the same in mapping grid.");
        }
        if (!invalidItems.isEmpty()) {                                                                   // :3290
            fail("Following item(s) are not present in Purchase Order Detail: "
               + String.join(", ", invalidItems)
               + ". Please delete those rows which contain these items from mapping.");
        }
        if (live.isEmpty() || kept.isEmpty()) return;                                                    // :3292
        for (PurchaseOrderMasterCmagtDto.DetailDto d : live) {
            BigDecimal sum = BigDecimal.ZERO;
            boolean any = false;
            for (PurchaseOrderMasterCmagtDto.SaleOrderMappingDto m : kept) {
                if (m.getItemId() != null && m.getItemId().equals(d.getItemId())) {
                    any = true;
                    sum = sum.add(nz(m.getItemNetWeight()));
                }
            }
            String name = d.getItemName() == null ? "" : d.getItemName();
            if (!any) fail("No mapping found for Item " + name);                                         // :3296
            if (sum.compareTo(nz(d.getItemWeight())) > 0)                                                // :3299
                fail("Mapped weight (" + sum.stripTrailingZeros().toPlainString()
                   + ") cannot be greater than PO weight (" + nz(d.getItemWeight()).stripTrailingZeros().toPlainString()
                   + ") for Item " + name);
        }
    }

    /** yyyy-MM-dd prefix of a date string, or null. */
    private static java.time.LocalDate day(String s) {
        if (s == null || s.trim().length() < 10) return null;
        try { return java.time.LocalDate.parse(s.trim().substring(0, 10)); }
        catch (Exception e) { return null; }
    }

    /* ---------------------------------------------------------------------------- sources */

    /**
     * clsGlobalVariables.globalInvPackingType, which the desktop loads once at start-up and
     * then searches in memory (:3102). Read here per save: the range is a business rule and a
     * stale cached copy would silently widen or narrow it.
     */
    private Map<Integer, Map<String, Object>> packingTypeRanges() {
        Map<Integer, Map<String, Object>> out = new HashMap<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?", "ReadAll")) {
                Object id = col(r, "Id");
                if (id instanceof Number) out.put(((Number) id).intValue(), r);
            }
        } catch (Exception e) {
            /* The desktop treats a missing InvPackingType row as "no range to check" (:3102
               `packType != null`). An unreadable list is the same situation, and must not be
               turned into a blanket refusal of every save. */
            return out;
        }
        return out;
    }

    /* ---------------------------------------------------------------------------- helpers */

    /** Rows queued for deletion (actionTypeId 3) are not part of the document being validated. */
    private static List<PurchaseOrderMasterCmagtDto.DetailDto> liveDetail(PurchaseOrderMasterCmagtDto dto) {
        List<PurchaseOrderMasterCmagtDto.DetailDto> out = new ArrayList<>();
        if (dto.getPurchaseOrderDetailList() == null) return out;
        for (PurchaseOrderMasterCmagtDto.DetailDto d : dto.getPurchaseOrderDetailList()) {
            if (d.getActionTypeId() != null && d.getActionTypeId() == 3) continue;
            out.add(d);
        }
        return out;
    }

    private static BigDecimal scheduleAmountSum(PurchaseOrderMasterCmagtDto dto) {
        BigDecimal s = BigDecimal.ZERO;
        if (dto.getPurchaseOrderPaymentDetailList() == null) return s;
        for (PurchaseOrderMasterCmagtDto.PaymentDto p : dto.getPurchaseOrderPaymentDetailList()) {
            s = s.add(nz(p.getDueAmount()));
        }
        return s;
    }

    /* The header payment term / due days are not master columns on this model - the desktop
       reads them straight off CmbPaymentTerm and txtDueDays and uses them only to synthesize
       the single fallback schedule row (:3193-3200). The page therefore sends them as that row,
       and it is that row we read here. */
    private static Integer headerPaymentTermId(PurchaseOrderMasterCmagtDto dto) {
        if (dto.getPurchaseOrderPaymentDetailList() == null
                || dto.getPurchaseOrderPaymentDetailList().isEmpty()) return 0;
        return dto.getPurchaseOrderPaymentDetailList().get(0).getPaymentTermId();
    }

    private static Integer headerDueDays(PurchaseOrderMasterCmagtDto dto) {
        if (dto.getPurchaseOrderPaymentDetailList() == null
                || dto.getPurchaseOrderPaymentDetailList().isEmpty()) return 0;
        return dto.getPurchaseOrderPaymentDetailList().get(0).getDueDays();
    }

    private static void req(Integer v, String field, int row) {
        if (zero(v)) fail(field + " Field Required in row#" + row);
    }

    private static void req(BigDecimal v, String field, int row) {
        if (!pos(v)) fail(field + " Field Required in row#" + row);
    }

    private static void fail(String message) { throw new IllegalArgumentException(message); }

    private static boolean zero(Integer v) { return v == null || v == 0; }
    private static boolean pos(BigDecimal v) { return v != null && v.compareTo(BigDecimal.ZERO) > 0; }
    private static BigDecimal nz(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }

    private static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0d; }
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    private static String trim(double d) {
        String s = new BigDecimal(String.valueOf(d)).stripTrailingZeros().toPlainString();
        return s;
    }

    /** "#,##0.####" as the desktop formats these two messages (:3212). */
    private static String money(BigDecimal v) {
        return new java.text.DecimalFormat("#,##0.####").format(nz(v));
    }

    /** SQL Server result keys are case-insensitive to the desktop; JdbcTemplate's are not. */
    private static Object col(Map<String, Object> row, String name) {
        Object v = row.get(name);
        if (v != null || row.containsKey(name)) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }
}
