package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleBookingOrderRepository;
import com.mst.repositories.SaleInvoiceDirect139Repository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleBookingOrderRepository.m;

/**
 * Screen 140 - Architecture.WinApp.Sale.BookingOrder (DocumentTypeId 127).
 * Save = BLL 0596 SaleOrder.Save -> DAL 0449 SetData, in one transaction: header, every detail (removed rows with ActionTypeId 3),
 * then the document approval row. Messages and their order are the form's. Document, branch and category numbers are regenerated
 * on insert (web deviation: two users cannot collide on a number).
 */
@Service
public class SaleBookingOrderService {
    private final SaleBookingOrderRepository repo;
    private final SaleInvoiceDirect139Repository r139;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final SaleInvoiceRepository si;

    public SaleBookingOrderService(SaleBookingOrderRepository repo, SaleInvoiceDirect139Repository r139, CurrentUserContext ctx,
                                   StoreScreenRights rights, SaleInvoiceRepository si) {
        this.repo = repo; this.r139 = r139; this.ctx = ctx; this.rights = rights; this.si = si;
    }

    private UserAccount user() { return ctx.requireAccountingUser(); }

    // ------------------------------------------------------------------------------------------ helpers
    private static int I(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).replace(",", "").trim()); } catch (Exception e) { return 0; }
    }
    private static double D(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).replace(",", "").trim()); } catch (Exception e) { return 0d; }
    }
    private static String S(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static Timestamp ts(Object o) {
        String s = S(o);
        if (s.isEmpty() || "null".equals(s)) return null;
        try { return Timestamp.valueOf(s.length() <= 10 ? s + " 00:00:00" : s.replace('T', ' ').substring(0, Math.min(19, s.replace('T', ' ').length()))); }
        catch (Exception e) { return null; }
    }
    private static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now()); }
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object o) { return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>(); }

    // ------------------------------------------------------------------------------------------ lookups
    public Map<String, Object> lookups() {
        UserAccount u = user();
        int year = ctx.currentFinancialYearId();
        Map<String, Object> o = repo.initial(u, year);
        o.put("dueTerms", r139.dueTerms(u));
        o.put("rights", rights.of(SaleBookingOrderRepository.SCREEN));
        o.put("today", LocalDate.now().toString());
        LocalDateTime start = si.financialYearStart(u, year);
        o.put("yearStart", start == null ? null : start.toLocalDate().toString());
        o.put("userBranchId", u.getBranchesId());
        boolean bw = Boolean.TRUE.equals(((Map<?, ?>) o.get("config")).get("branchWise"));
        o.put("historyBranches", repo.historyBranches(u, bw, bw ? si.branchName(u) : ""));
        o.put("historyCustomers", repo.historyCustomers(u, bw ? String.valueOf(u.getBranchesId()) : ""));
        return o;
    }
    public Map<String, Object> numbers(int categoryId) {
        UserAccount u = user(); int year = ctx.currentFinancialYearId();
        return m("docNo", repo.docNo(u, year), "branchSr", repo.branchSr(u, year), "catSr", categoryId > 0 ? repo.categorySr(u, year, categoryId) : "");
    }
    public List<Map<String, Object>> uoms(int itemId) {
        if (itemId <= 0) throw new IllegalArgumentException("ItemName Field is Required");
        return repo.uoms(user(), itemId);
    }
    public List<Map<String, Object>> cropYears(int itemId, String date) {
        Timestamp t = ts(date); return repo.cropYears(user(), itemId, t == null ? now() : t);
    }
    /** CmbCropyr_Leave: rate lookup, customer discount when a row is found, regular item discount, commission-on-sale flag. */
    public Map<String, Object> pricing(int itemId, int cropYearId, int customerId, String date) {
        UserAccount u = user(); Timestamp t = ts(date); if (t == null) t = now();
        Map<String, Object> out = new LinkedHashMap<>();
        var rows = repo.rate(u, itemId, cropYearId, t);
        if (!rows.isEmpty()) {
            var r = rows.get(0);
            out.put("found", true);
            out.put("customerDiscount", repo.customerDiscount(u, customerId, itemId, t));
            out.put("scheduleId", r.get("Id")); out.put("rateUomId", r.get("RateUomId")); out.put("itemPrice", r.get("ItemPrice")); out.put("rateUom", r.get("PackingUom"));
        } else {
            out.put("found", false); out.put("customerDiscount", 0); out.put("scheduleId", 0); out.put("rateUomId", 0); out.put("itemPrice", 0); out.put("rateUom", 0);
        }
        out.put("itemDiscount", repo.itemDiscount(u, itemId, t));
        out.put("commOnSale", repo.commOnSale(u, itemId));
        return out;
    }
    /** GetCustomerDiscount: CustomerDiscountPolicy.GetCustomerDiscountByEffectiveDate. */
    public Map<String, Object> customerDiscount(int customerId, int itemId, String date) {
        Timestamp t = ts(date); return m("discount", repo.customerDiscount(user(), customerId, itemId, t == null ? now() : t));
    }
    public Map<String, Object> packingAddLess(int uomId, int customerId, String date) {
        Timestamp t = ts(date); return m("packingAddLess", repo.packingAddLess(user(), uomId, customerId, t == null ? now() : t));
    }
    public Map<String, Object> record(int id) { return repo.record(user(), id); }

    public List<Map<String, Object>> history(String mode, String from, String to, double fromNo, double toNo, int customerId, String branchIds) {
        UserAccount u = user();
        return repo.history(u, ctx.currentFinancialYearId(), repo.canViewAllRecords(u, ctx.currentRoleName()), mode, ts(from), ts(to), fromNo, toNo, customerId, branchIds);
    }
    public List<Map<String, Object>> historyCustomers(String branchIds) { return repo.historyCustomers(user(), branchIds); }

    // ------------------------------------------------------------------------------------------ validation
    /** Insert pre-check, FormValidation, commission checks and the per-row chain, in the form's order. */
    private void validate(Map<String, Object> b, boolean insert) {
        var rows = list(b.get("details"));
        if (insert && rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        if (S(b.get("docNo")).isEmpty()) throw new IllegalArgumentException("Doc No Field is Required");
        if (I(b.get("orderCategoryId")) <= 0) throw new IllegalArgumentException("Order Category Field is Required");
        if (I(b.get("customerId")) <= 0) throw new IllegalArgumentException("Customer Name Field is Required");
        if (S(b.get("orderType")).isEmpty()) throw new IllegalArgumentException("OrderType Field is Required");
        if (I(b.get("paymentTermId")) <= 0) throw new IllegalArgumentException("Payment Term Field is Required");
        if (S(b.get("deliveryTerm")).isEmpty()) throw new IllegalArgumentException("Delivery Term Field is Required");
        int agent = I(b.get("salesmanId"));
        if (agent <= 0 && (D(b.get("commAmount")) > 0 || D(b.get("commRate")) > 0))
            throw new IllegalArgumentException("Please Select Commission Agent Required when Commission Amount or Rate is Present...");
        if (agent > 0 && D(b.get("commAmount")) <= 0) throw new IllegalArgumentException("Commission Amount Required when Commission Agent is Selected...");
        if (agent > 0 && D(b.get("commRate")) <= 0) throw new IllegalArgumentException("Commission Rate Required when Commission Agent is Selected...");
        int n = 0;
        for (var d : rows) {
            n++;
            String at = " in Detail Grid Row No: " + n;
            if (I(d.get("scheduleId")) <= 0) throw new IllegalArgumentException("PriceScheduleId Field Required" + at);
            if (D(d.get("itemPrice")) <= 0) throw new IllegalArgumentException("Item Price Field Required" + at);
            if (I(d.get("itemId")) <= 0) throw new IllegalArgumentException("Item Field Required" + at);
            if (I(d.get("packingTypeId")) <= 0) throw new IllegalArgumentException("PackingType Field Required" + at);
            if (I(d.get("packUomId")) <= 0) throw new IllegalArgumentException("Item UOM Field Required" + at);
            if (D(d.get("qty")) <= 0) throw new IllegalArgumentException("Item Qty Field Required" + at);
            if (D(d.get("weight")) <= 0) throw new IllegalArgumentException("Weight Field Required" + at);
            if (D(d.get("rate")) <= 0) throw new IllegalArgumentException("Rate Field Required" + at);
            if (I(d.get("rateUomId")) <= 0) throw new IllegalArgumentException("RateUom Field Required" + at);
            if (D(d.get("amount")) <= 0) throw new IllegalArgumentException("Amount Field Required" + at);
        }
    }

    private Map<String, Object> modelDetail(Map<String, Object> d) {
        Map<String, Object> x = new LinkedHashMap<>();
        x.put("Id", I(d.get("id"))); x.put("RefDocId", I(d.get("preOrderId"))); x.put("RefDocDetailId", I(d.get("preOrderDetailId")));
        x.put("PriceScheduleId", I(d.get("scheduleId"))); x.put("OrderItemId", I(d.get("itemId"))); x.put("CropYearId", I(d.get("cropYearId")));
        x.put("PackingTypeID", I(d.get("packingTypeId"))); x.put("InvPackingTypeId", I(d.get("packingTypeId")));
        x.put("OrderItemUOMId", I(d.get("packUomId"))); x.put("PackScheduleUnitId", I(d.get("packScheduleUnitId")));
        x.put("OrderItemQty", D(d.get("qty"))); x.put("NetWeight", D(d.get("weight"))); x.put("ItemPrice", D(d.get("itemPrice")));
        x.put("PackingAddLessOnRate", D(d.get("packingAddLess"))); x.put("RateDiscount", D(d.get("rateDiscount")));
        x.put("OrderItemRate", D(d.get("rate"))); x.put("OrderItemRateUOMId", I(d.get("rateUomId")));
        x.put("Amount", D(d.get("amount"))); x.put("ItemDiscount", D(d.get("itemDiscPrct"))); x.put("ItemDiscountAmount", D(d.get("itemDiscAmt")));
        x.put("TotalAmount", D(d.get("totalAmount"))); x.put("CityId", I(d.get("cityId"))); x.put("ReferencePartyId", I(d.get("refPartyId")));
        x.put("OrderRemarks", S(d.get("remarks"))); x.put("CommOnSale", Boolean.TRUE.equals(d.get("commOnSale")) || "true".equalsIgnoreCase(S(d.get("commOnSale"))));
        x.put("CostCenterId", I(d.get("costCenterId")));
        x.put("CityArea", S(d.get("cityName"))); x.put("Crop", S(d.get("cropYear")));
        return x;
    }

    private double billAmount(Map<String, Object> b) {
        double sum = 0; for (var d : list(b.get("details"))) sum += D(d.get("amount"));
        if (I(b.get("salesmanId")) > 0 && I(b.get("salesmanId")) == I(b.get("customerId"))) sum += D(b.get("commAmount"));
        return sum;
    }

    /**
     * The Yes/No prompts the form raises before it saves: USP_SaleOrderValidations per row (first warning only) and the customer
     * debit limit (SP_CHECKSUPPLIERCUSTOMERLIMITS, ActionId 2). Validation errors are thrown first, as the form validates first.
     */
    public Map<String, Object> checks(Map<String, Object> b) {
        UserAccount u = user();
        int id = I(b.get("id"));
        validate(b, id == 0);
        List<String> confirms = new ArrayList<>();
        Timestamp docDate = ts(b.get("docDate")); if (docDate == null) docDate = now();
        int cust = I(b.get("customerId"));
        for (var d : list(b.get("details"))) {
            String w = repo.rowWarning(u, id, docDate, cust, modelDetail(d));
            if (w != null) { confirms.add(w); break; }
        }
        Double limit = repo.availableLimit(u, cust, billAmount(b));
        if (limit != null) {
            if (limit > 0) confirms.add("Debit Limit of Customer Exceeds Define Debit Limit & remaining limit value is   " + String.format("%,.0f", limit));
            else if (limit < 0) confirms.add("Debit Limit of Customer already Exceeds Define Debit Limit");
        }
        return m("confirms", confirms);
    }

    // ------------------------------------------------------------------------------------------ save
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = user();
        int year = ctx.currentFinancialYearId();
        int id = I(b.get("id"));
        boolean insert = id == 0 && !"update".equals(S(b.get("mode")));
        if (!insert && id == 0) throw new IllegalArgumentException("Record Not Update because RecId Not Found");
        if (insert && list(b.get("details")).stream().anyMatch(d -> I(d.get("id")) > 0))
            throw new IllegalArgumentException("Record cannot be inserted because ActionTypeId greater than 1");
        validate(b, insert);

        String docNo = S(b.get("docNo")), branchSr = S(b.get("branchSr")), catSr = S(b.get("catSr"));
        int category = I(b.get("orderCategoryId"));
        if (insert) {
            docNo = String.valueOf(repo.docNo(u, year)); branchSr = String.valueOf(repo.branchSr(u, year)); catSr = String.valueOf(repo.categorySr(u, year, category));
        }
        Timestamp now = now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", id); h.put("OrganizationId", u.getOrganizationId()); h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", u.getBranchesId()); h.put("ProjectsId", u.getBranchesId()); h.put("FinancialYearId", year); h.put("DocumentTypeId", SaleBookingOrderRepository.DOC);
        h.put("EntryUser", u.getId()); h.put("ModifyUser", u.getId()); h.put("EntryDate", now); h.put("ModifyDate", now);
        h.put("DocDate", ts(b.get("docDate"))); h.put("DocNo", I(docNo)); h.put("BranchSrNo", I(branchSr));
        h.put("OrderCatagoryId", category); h.put("CatagorySrNo", I(catSr)); h.put("OrderSupCustId", I(b.get("customerId")));
        h.put("SupplierRefNo", S(b.get("supplierRefNo"))); h.put("RemarksHeader", S(b.get("remarks")));
        h.put("PaymentTermsId", I(b.get("paymentTermId"))); h.put("OrderDueDays", I(b.get("dueDays"))); h.put("OrderDueDate", ts(b.get("dueDate")));
        h.put("OrderExpiryDate", now); h.put("OrderType", S(b.get("orderType"))); h.put("OrderStatus", "Open");
        h.put("DeliveryTerm", S(b.get("deliveryTerm"))); h.put("DeliveryStartDate", ts(b.get("deliveryStartDate"))); h.put("DeliveryDays", I(b.get("deliveryDays")));
        if (I(b.get("salesmanId")) > 0) {
            h.put("BrokerAgentSupCustId", I(b.get("salesmanId"))); h.put("CommissionType", S(b.get("commType"))); h.put("CommRate", D(b.get("commRate")));
            h.put("UomScheduleIdCmRate", I(b.get("commUom"))); h.put("CommAmount", D(b.get("commAmount"))); h.put("CommissionRemarks", S(b.get("commRemarks")));
        }
        int saved = repo.saveHeader(insert, h);
        if (saved <= 0) saved = id;
        if (saved <= 0) throw new IllegalStateException("Sale order identity was not returned");

        double limit = 0;
        for (var d : list(b.get("details"))) {
            Map<String, Object> x = modelDetail(d);
            x.put("SaleOrderId", saved); x.put("ActionTypeId", insert ? 1 : (I(d.get("id")) > 0 ? 2 : 1));
            addCommon(x, u, now);
            repo.saveDetail(x);
            limit += D(d.get("amount"));
        }
        for (var d : list(b.get("removed"))) {
            if (I(d.get("id")) <= 0) continue;
            Map<String, Object> x = modelDetail(d);
            x.put("SaleOrderId", saved); x.put("ActionTypeId", 3);
            addCommon(x, u, now);
            repo.saveDetail(x);
        }
        repo.approval(u, saved, limit);
        return m("success", true, "id", saved, "docNo", docNo,
                "message", (insert ? "Data Save Successfully....  " : "Data Update Successfully....  ") + docNo);
    }

    private void addCommon(Map<String, Object> x, UserAccount u, Timestamp now) {
        x.put("OrganizationId", u.getOrganizationId()); x.put("CompanyId", u.getCompanyId()); x.put("EntryUser", u.getId()); x.put("ModifyUser", u.getId());
        x.put("EntryDate", now); x.put("ModifyDate", now);
    }
}
