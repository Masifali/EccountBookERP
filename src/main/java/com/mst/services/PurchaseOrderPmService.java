package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.PurchaseOrderPmRequest;
import com.mst.repositories.InventoryOpeningRepository;
import com.mst.repositories.PurchaseOrderHeaderRepository;
import com.mst.repositories.PurchaseOrderPmRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

import static com.mst.repositories.PurchaseOrderPmRepository.*;

/**
 * Screen 498 "1002 Purchsae Order" - Packing Material module 54. Desktop form
 * {@code Architecture.WinApp.PackingMaterial_Store.PurchsaeOrderPmNew}.
 *
 * Save follows {@code Insert()} :1722 line by line: "Grid Record Not Found", FormValidation
 * (:1285), the per-row refusals inside the loop (:1797-1857), then BLL {@code PurchaseOrder.Save}
 * (DateLock, the detail-id guard) and DAL {@code SetData}: header procedure, one
 * Sp_PurchaseOrderDetail_Insert per row, then on UPDATE the removed-row procedure and
 * PoWeightAndGpWeightValidation, on INSERT the approval-detail row. One transaction.
 *
 * Amounts are recomputed here with the form's own formulas (CalculateWeight :2622,
 * CalculateItemAount :2643, CalculateTaxAmount :2686, txtExchangeRate_TextChanged :557 which
 * Insert() calls first), so a request that did not come from the page cannot store figures the
 * page could never have produced.
 */
@Service
public class PurchaseOrderPmService {

    public static final int SCREEN_ID = 498;

    private final PurchaseOrderPmRepository repo;
    private final PurchaseOrderHeaderRepository header;
    private final InventoryOpeningRepository shared;
    private final DesktopAttachmentStore store;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public PurchaseOrderPmService(PurchaseOrderPmRepository repo, PurchaseOrderHeaderRepository header,
                                  InventoryOpeningRepository shared, DesktopAttachmentStore store, JdbcTemplate jdbc,
                                  CurrentUserContext context, DesktopReportRights rights) {
        this.repo = repo; this.header = header; this.shared = shared; this.store = store; this.jdbc = jdbc;
        this.context = context; this.rights = rights;
    }

    private UserAccount user(String action) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int financialYear(UserAccount u) {
        var years = shared.years(u);
        if (years.isEmpty()) throw new IllegalArgumentException("No active financial year allocated to this company");
        return intOf(years.get(0).get("Id"));
    }

    // ================================================================== load

    /** InitializeComponentMethod :473 and its continuation :494-553. */
    public Map<String, Object> lookups() {
        UserAccount u = user("View");
        Set<Integer> f = repo.features(u);
        boolean subsidiary = f.contains(4), multi = f.contains(6), mrp = f.contains(23);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("suppliers", repo.suppliers(u, subsidiary));
        r.put("paymentTerms", repo.paymentTerms(u));
        r.put("currencies", repo.currencies(u));
        r.put("items", repo.items(u));
        r.put("cropYears", repo.cropYears(u));
        r.put("uoms", repo.uoms(u));
        r.put("refDocumentTypes", repo.refDocumentTypes());
        r.put("history", repo.historyCombos(u));

        Map<String, Object> fe = new LinkedHashMap<>();
        fe.put("subsidiaryAccounts", subsidiary);
        fe.put("multiCurrency", multi);
        fe.put("mrpPlanning", mrp);
        r.put("features", fe);

        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("itemSearchByCode", bool(repo.config(u, "ItemSearchByCode")));
        cfg.put("defaultDaysToLessFromHistoryFromDate", intOf(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        cfg.put("purchaseOrderBranchWise", bool(repo.config(u, "PurchaseOrderBranchWise")));
        cfg.put("defaultCropYearId", intOf(repo.config(u, "Default Crop Year")));
        cfg.put("taxPercentEditable", bool(repo.config(u, "TaxPercentEditable")));
        cfg.put("baseCurrencyId", intOf(repo.config(u, "Base Currency")));          // :792-797
        cfg.put("baseCurrencyRate", PurchaseOrderPmRepository.dbl(repo.config(u, "BaseCurrencyRate")));   // :798-803
        cfg.put("configuredCurrencyId", intOf(repo.configByDefinition(u, "1")));    // multiCurrencyFeature :610-620
        cfg.put("configuredCurrencyRate", PurchaseOrderPmRepository.dbl(repo.configByDefinition(u, "160")));
        cfg.put("amountDecimals", amountRoundDigits(u));
        cfg.put("amountFormatDecimals", formatDigits(intOf(repo.config(u, "Default NoofDecimal Points For Amount")), 0));
        cfg.put("rateFormatDecimals", formatDigits(intOf(repo.config(u, "Default NoofDecimal Points For Rate")), 2));
        cfg.put("fcyDecimals", fcyRoundDigits(u));
        cfg.put("fcyFormatDecimals", formatDigits(intOf(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")), 0));
        r.put("configuration", cfg);

        Map<String, Boolean> p = new LinkedHashMap<>();
        for (String a : List.of("Save", "Update", "Print", "CanView AllRecord")) p.put(a, allowed(u, a));
        r.put("permissions", p);

        r.put("userBranchId", u.getBranchesId());       // cmbBranchName.Text = UserAccount.BranchName (:759)
        r.putAll(numbers());
        return r;
    }

    /** UpdateDocumentNoUI / UpdateBranchSrNoUI - the numbers shown on New. The procedure allocates the real one. */
    public Map<String, Object> numbers() {
        UserAccount u = user("View");
        int fy = financialYear(u);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", header.nextDocNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy));
        m.put("branchSrNo", header.nextBranchSrNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, u.getBranchesId(), fy));
        return m;
    }

    /** combitem_Leave_1 :2839 / combsuppname_Leave :2911 - tax schedule and lead time. */
    public Map<String, Object> itemDefaults(int itemId, int supplierId, String docDate) {
        UserAccount u = user("View");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("leadTime", repo.leadTime(u, supplierId, itemId));
        List<Map<String, Object>> taxes = new ArrayList<>();
        if (itemId > 0) {
            for (var t : repo.taxes(u, itemId, date(docDate, "Doc Date"))) {
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", intOf(col(t, "TaxNameId")));
                x.put("TaxType", col(t, "TaxName"));
                x.put("TaxPrcnt", PurchaseOrderPmRepository.dbl(col(t, "TaxPercent")));
                taxes.add(x);
                break;                                           // :2888 - row 0 only
            }
        }
        m.put("taxes", taxes);
        return m;
    }

    public List<Map<String, Object>> refDocs(int refDocumentTypeId, int recId) {
        UserAccount u = user("View");
        if (recId > 0) repo.header(u, recId);
        return repo.refDocs(u, refDocumentTypeId, recId);
    }

    /** cmbCurrency_Leave :655. */
    public Map<String, Object> exchangeRate(int currencyId) {
        UserAccount u = user("View");
        Map<String, Object> m = new LinkedHashMap<>();
        int base = intOf(repo.config(u, "Base Currency"));
        if (currencyId == 0) return m;
        if (currencyId != base) {
            Double last = repo.lastExchangeRate(u, currencyId);
            m.put("exchangeRate", last == null ? 0d : last);
        } else {
            m.put("exchangeRate", PurchaseOrderPmRepository.dbl(repo.config(u, "BaseCurrencyRate")));
        }
        return m;
    }

    // ================================================================== history

    public List<Map<String, Object>> history(String dateMode, String from, String to, int fromDocNo, int toDocNo,
                                             int supplierId, String branchIds) {
        UserAccount u = user("View");
        if (branchIds == null || branchIds.isBlank()) throw new IllegalArgumentException("Select Branch First");   // :2305
        HistoryFilter f = new HistoryFilter();
        f.canViewAll = allowed(u, "CanView AllRecord");
        f.financialYearId = financialYear(u);
        f.dateMode = dateMode == null ? "doc" : dateMode;
        f.from = (from == null || from.isBlank()) ? null : date(from, "From Date");
        f.to = (to == null || to.isBlank()) ? null : date(to, "To Date");
        f.fromDocNo = fromDocNo;
        f.toDocNo = toDocNo;
        f.supplierId = supplierId;
        return repo.history(u, f);
    }

    /** ReadById :1931 - header, the grid rows as the form builds them (:1970), attachments. */
    public Map<String, Object> record(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = repo.header(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        List<Map<String, Object>> lines = new ArrayList<>();
        for (var d : repo.details(id)) {
            Map<String, Object> l = new LinkedHashMap<>();
            l.put("Id", intOf(col(d, "Id")));
            l.put("ItemId", intOf(col(d, "OrderItemId")));
            l.put("ItemCode", col(d, "ItemCode"));
            l.put("ItemName", col(d, "ItemName"));
            l.put("CropYearId", intOf(col(d, "CropYearId")));
            l.put("CropYear", col(d, "Crop"));
            l.put("WeightCapacity", col(d, "WeightCapacity"));
            l.put("PackingDate", col(d, "PackingDate"));
            l.put("ExpiryDate", col(d, "ExpiryDate"));
            l.put("PackUomId", intOf(col(d, "OrderItemUOMId")));
            l.put("PackUom", col(d, "UOMCode"));
            l.put("PackEquivalent", col(d, "UOMDescription"));         // :1970 puts UOMDescription here
            l.put("ItemQty", col(d, "OrderItemQty"));
            l.put("WeightPerQty", col(d, "WeightPerQty"));
            l.put("TotalWeight", col(d, "NetWeight"));
            l.put("ItemRate", col(d, "OrderItemRate"));
            l.put("RateUomId", intOf(col(d, "OrderItemRateUOMId")));
            l.put("RateUom", col(d, "RateUom"));
            l.put("RateEquivalent", col(d, "EquivalentRate"));
            l.put("Amount", col(d, "Amount"));
            l.put("FcyAmount", col(d, "FcyAmount"));
            l.put("TaxNameId", intOf(col(d, "TaxNameId")));
            l.put("TaxName", col(d, "TaxName"));
            l.put("TaxPercent", col(d, "TaxPercent"));
            l.put("TaxAmount", col(d, "TaxAmount"));
            l.put("TotalAmount", col(d, "TotalAmount"));
            l.put("LeadTime", intOf(col(d, "LeadTime")));
            l.put("RefDocumentTypeId", intOf(col(d, "RefDocumentTypeId")));
            l.put("RefDocumentType", col(d, "RefDocumentType"));
            l.put("RefDocId", intOf(col(d, "RefDocId")));
            l.put("RefDocNo", col(d, "RefDocNo"));
            l.put("RefDocInvoiceId", intOf(col(d, "RefDocInvoiceId")));
            l.put("RefDocInvoiceNo", col(d, "RefDocInvoiceNo"));
            l.put("Remarks", col(d, "OrderRemarks"));
            lines.add(l);
        }
        out.put("lines", lines);
        out.put("attachments", repo.attachments(u, id));
        return out;
    }

    // ================================================================== save

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(PurchaseOrderPmRequest r) {
        if (r == null || r.Id < 0) throw new IllegalArgumentException("Invalid Purchase Order");
        boolean insert = r.Id == 0;
        UserAccount u = user(insert ? "Save" : "Update");
        Map<String, Object> old = insert ? null : repo.header(u, r.Id);

        if (r.lines == null || r.lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");   // :1732

        Set<Integer> f = repo.features(u);
        boolean multi = f.contains(6);

        // -------------------------------------------------------- FormValidation :1285
        if (r.OrderSupCustId == 0 || repo.suppliers(u, f.contains(4)).stream().noneMatch(s -> intOf(s.get("Id")) == r.OrderSupCustId)) {
            throw new IllegalArgumentException("Supplier Name Field is Required");
        }
        if (r.PaymentTermsId == 0 || repo.paymentTerms(u).stream().noneMatch(s -> intOf(s.get("Id")) == r.PaymentTermsId)) {
            throw new IllegalArgumentException("Payment Term Field is Required");
        }
        String deliveryTerm = switch (r.DeliveryTermId) { case 1 -> "Load"; case 2 -> "Ponch"; default -> null; };
        if (deliveryTerm == null) throw new IllegalArgumentException("Delivery Term Field is Required");
        BigDecimal exRate = dec(r.ExchangeRate);
        if (multi) {
            if (r.CurrencyId == 0 || repo.currencies(u).stream().noneMatch(c -> intOf(c.get("Id")) == r.CurrencyId)) {
                throw new IllegalArgumentException("Fcy Code Field is Required");
            }
            if (exRate.signum() == 0) throw new IllegalArgumentException("Exchange Rate Field is Required");
        } else {
            if (r.CurrencyId == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (exRate.signum() == 0) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        if (r.BillCalculateTypeId != 1 && r.BillCalculateTypeId != 2) throw new IllegalArgumentException("Bill Type Field is Required");

        LocalDate docDate = date(r.DocDate, "Doc Date").toLocalDate();
        LocalDate deliveryStart = r.DeliveryStartDate == null || r.DeliveryStartDate.isBlank()
                ? docDate : date(r.DeliveryStartDate, "Delivery Start Date").toLocalDate();
        if (deliveryStart.isBefore(docDate)) throw new IllegalArgumentException("Delivery Start Date Can't Be Less Than Doc Date");

        // -------------------------------------------------------- the rows :1792-1866
        int amountDigits = amountRoundDigits(u);
        int fcyDigits = fcyRoundDigits(u);
        boolean taxEditable = bool(repo.config(u, "TaxPercentEditable"));
        Map<Integer, Map<String, Object>> items = new HashMap<>();
        for (var it : repo.items(u)) items.put(intOf(it.get("Id")), it);
        Map<Integer, String> crops = new HashMap<>();
        for (var c : repo.cropYears(u)) crops.put(intOf(c.get("Id")), Objects.toString(c.get("Description"), ""));
        List<Map<String, Object>> uoms = repo.uoms(u);
        Set<Integer> refTypes = new HashSet<>();
        for (var t : repo.refDocumentTypes()) refTypes.add(intOf(col(t, "Id")));
        Set<Integer> existingDetailIds = new HashSet<>();
        if (!insert) for (var d : repo.details(r.Id)) existingDetailIds.add(intOf(col(d, "Id")));
        Map<Integer, List<Map<String, Object>>> refDocCache = new HashMap<>();

        List<Map<String, Object>> details = new ArrayList<>();
        BigDecimal totalQty = BigDecimal.ZERO, totalAmount = BigDecimal.ZERO, totalFcy = BigDecimal.ZERO;
        int rowNo = 0;
        for (PurchaseOrderPmRequest.Line l : r.lines) {
            rowNo++;
            if (l == null) throw new IllegalArgumentException("Grid Record Not Found");
            if (l.RefDocumentTypeId > 0 && l.RefDocId == 0 && l.RefDocInvoiceId == 0) {
                throw new IllegalArgumentException("RefDocNo or RefDocInvoiceNo field Required in Detail grid Row No : " + rowNo);
            }
            Map<String, Object> item = items.get(l.ItemId);
            if (l.ItemId == 0 || item == null) throw new IllegalArgumentException("Item Required in grid row#" + rowNo);
            if (l.CropYearId == 0 || !crops.containsKey(l.CropYearId)) throw new IllegalArgumentException("Crop Year Required in grid row#" + rowNo);
            Map<String, Object> pack = uom(uoms, l.ItemId, l.PackUomId);
            if (l.PackUomId == 0 || pack == null) throw new IllegalArgumentException("Pack Uom Required in grid row#" + rowNo);

            // CalculateWeight (grid) :3638
            double totalWeight = (l.ItemQty > 0 && l.WeightPerQty > 0) ? l.WeightPerQty * l.ItemQty : 0d;
            if (totalWeight == 0d) throw new IllegalArgumentException("Total Weight Required in grid row#" + rowNo);
            if (l.ItemRate == 0d) throw new IllegalArgumentException("Item Rate Required in grid row#" + rowNo);
            Map<String, Object> rateUom = uom(uoms, l.ItemId, l.RateUomId);
            if (l.RateUomId == 0 || rateUom == null) throw new IllegalArgumentException("Rate Uom Required in grid row#" + rowNo);
            double rateEq = PurchaseOrderPmRepository.dbl(rateUom.get("Equivalent"));

            // CalculateAmount :3648
            double amount = 0d;
            if (l.ItemQty > 0 && rateEq > 0 && l.ItemRate > 0 && totalWeight > 0) {
                amount = r.BillCalculateTypeId == 1 ? totalWeight / rateEq * l.ItemRate : l.ItemQty / rateEq * l.ItemRate;
            }
            amount = round(amount, amountDigits);
            if (amount == 0d) throw new IllegalArgumentException("Amount Required in grid row#" + rowNo);

            // tax - the schedule's percent unless TaxPercentEditable (CalculateTaxAmount :2694-2707)
            double taxPercent = l.TaxPercent;
            String taxName = null;
            if (l.TaxNameId > 0) {
                Map<String, Object> sched = null;
                for (var t : repo.taxes(u, l.ItemId, Date.valueOf(docDate))) {
                    if (intOf(col(t, "TaxNameId")) == l.TaxNameId) { sched = t; break; }
                }
                if (sched == null) throw new IllegalArgumentException("Tax Name is not scheduled for this item on the Doc Date in grid row#" + rowNo);
                taxName = Objects.toString(col(sched, "TaxName"), null);
                double schedPct = PurchaseOrderPmRepository.dbl(col(sched, "TaxPercent"));
                if (!taxEditable || taxPercent == 0d) taxPercent = schedPct;
            }
            if (taxPercent > 100d) throw new IllegalArgumentException("Tax% cannot be greater than 100. in grid row#" + rowNo);
            double taxAmount = 0d, total = amount;
            if (taxPercent > 0d && amount > 0d) { taxAmount = amount * taxPercent / 100d; total = amount + taxAmount; }
            else taxPercent = 0d;
            taxAmount = round(taxAmount, amountDigits);
            total = round(total, amountDigits);
            if (taxPercent > 0d && taxAmount > 0d && l.TaxNameId == 0) throw new IllegalArgumentException("Tax Name required in grid row#" + rowNo);
            if (taxPercent == 0d && taxAmount == 0d && l.TaxNameId > 0) throw new IllegalArgumentException("Tax percent required in grid row#" + rowNo);

            // txtExchangeRate_TextChanged :557 - Insert() runs it first, so this is the stored FcyAmount.
            BigDecimal fcy = exRate.signum() > 0
                    ? BigDecimal.valueOf(amount).divide(exRate, fcyDigits, RoundingMode.HALF_EVEN)
                    : BigDecimal.ZERO;

            if (l.RefDocumentTypeId > 0) {
                if (!refTypes.contains(l.RefDocumentTypeId)) throw new IllegalArgumentException("Select a Ref Document Type from the list in grid row#" + rowNo);
                var refs = refDocCache.computeIfAbsent(l.RefDocumentTypeId, t -> repo.refDocs(u, t, r.Id));
                if (l.RefDocId > 0 && refs.stream().noneMatch(x -> intOf(col(x, "RefDocId")) == l.RefDocId)) {
                    throw new IllegalArgumentException("Ref Doc No is not available for this document in grid row#" + rowNo);
                }
                if (l.RefDocInvoiceId > 0 && refs.stream().noneMatch(x -> intOf(col(x, "RefDocInvoiceId")) == l.RefDocInvoiceId)) {
                    throw new IllegalArgumentException("Ref Doc Invoice No is not available for this document in grid row#" + rowNo);
                }
            }
            if (insert && l.Id > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
            if (!insert && l.Id > 0 && !existingDetailIds.contains(l.Id)) throw new IllegalArgumentException("Detail row does not belong to this order in grid row#" + rowNo);

            Map<String, Object> d = detailDefaults();
            d.put("Id", insert ? 0 : l.Id);                                          // :1796
            d.put("OrderItemId", l.ItemId);
            d.put("CropYearId", l.CropYearId);
            d.put("Crop", crops.get(l.CropYearId));
            d.put("ExpiryDate", optionalDate(l.ExpiryDate));
            d.put("PackingDate", optionalDate(l.PackingDate));
            d.put("WeightCapacity", l.WeightCapacity == null ? "" : l.WeightCapacity);
            d.put("OrderItemUOMId", l.PackUomId);
            d.put("OrderItemQty", l.ItemQty);
            d.put("WeightPerQty", l.WeightPerQty);
            d.put("NetWeight", totalWeight);
            d.put("OrderItemRate", l.ItemRate);
            d.put("OrderItemRateUOMId", l.RateUomId);
            d.put("Amount", amount);
            d.put("CurrencyId", r.CurrencyId);
            d.put("ExchangeRate", exRate);
            d.put("FcyAmount", fcy);
            d.put("TaxPercent", taxPercent);
            d.put("TaxAmount", taxAmount);
            d.put("TaxNameId", l.TaxNameId);
            d.put("TotalAmount", total);
            d.put("LeadTime", l.LeadTime);
            d.put("TaxableStatus", (l.TaxNameId > 0 && taxPercent > 0 && taxAmount > 0) ? "true" : "false");   // :1860
            d.put("RefDocumentTypeId", l.RefDocumentTypeId);
            d.put("RefDocId", l.RefDocumentTypeId > 0 ? l.RefDocId : 0);
            d.put("RefDocInvoiceId", l.RefDocumentTypeId > 0 ? l.RefDocInvoiceId : 0);
            d.put("OrderRemarks", l.Remarks == null ? "" : l.Remarks.trim());
            details.add(d);

            totalQty = totalQty.add(BigDecimal.valueOf(l.ItemQty));
            totalAmount = totalAmount.add(BigDecimal.valueOf(amount));
            totalFcy = totalFcy.add(fcy);
        }
        if (multi && totalFcy.signum() == 0) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");

        // removed rows - grd_ColumnButtonClick :1465-1471
        List<Integer> removed = new ArrayList<>();
        if (!insert && r.removedDetailIds != null) {
            for (Integer id : new LinkedHashSet<>(r.removedDetailIds)) {
                if (id == null || id <= 0) continue;
                if (!existingDetailIds.contains(id)) throw new IllegalArgumentException("Removed row does not belong to this order");
                removed.add(id);
            }
            if (!removed.isEmpty() && r.lines.stream().noneMatch(l -> l.Id > 0)) {
                throw new IllegalArgumentException("All rows Can not be Deleted in Update Mode");
            }
        }

        // -------------------------------------------------------- header :1740-1787
        int fy = financialYear(u);
        Timestamp now = new Timestamp(System.currentTimeMillis());
        int dueDays = r.PaymentTermsId == 1 ? 0 : intOf(r.OrderDueDays);           // combpttrm_TextChanged :3022
        Timestamp docTs = Timestamp.valueOf(docDate.atStartOfDay());
        Timestamp dueTs = Timestamp.valueOf(docDate.plusDays(dueDays).atStartOfDay());  // txtduedays_TextChanged :2934

        Map<String, Object> po = PurchaseOrderHeaderRepository.blankModel();
        po.put("Id", insert ? 0 : r.Id);
        po.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        po.put("BranchesId", u.getBranchesId());
        po.put("ProjectsId", u.getBranchesId());                                  // :1756
        po.put("DocDate", docTs);
        po.put("DocNo", insert ? header.nextDocNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy) : intOf(col(old, "DocNo")));
        po.put("BranchSrNo", insert
                ? header.nextBranchSrNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, u.getBranchesId(), fy)
                : intOf(col(old, "BranchSrNo")));
        po.put("OrderCatagoryId", 2);
        po.put("BaseDocumentTypeId", BASE_DOCUMENT_TYPE_ID);
        po.put("OrderSupCustId", r.OrderSupCustId);
        po.put("SupplierRefNo", r.SupplierRefNo == null ? "" : r.SupplierRefNo);
        po.put("RemarksHeader", r.RemarksHeader == null ? "" : r.RemarksHeader.trim());
        po.put("PaymentTermsId", r.PaymentTermsId);
        po.put("OrderDueDays", dueDays);
        po.put("OrderDueDate", dueTs);
        po.put("OrderExpiryDate", dueTs);                                          // :1768 - the due date, as written
        po.put("DeliveryTerm", deliveryTerm);
        po.put("DeliveryStartDate", Timestamp.valueOf(deliveryStart.atStartOfDay()));
        po.put("DeliveryDays", intOf(r.DeliveryDays));
        po.put("OrderStatus", "Open");
        po.put("BrokerAgentSupCustId", 0);                                         // combsalesman is never bound (:1773)
        po.put("OrganizationId", u.getOrganizationId());
        po.put("FinancialYearId", fy);
        po.put("CompanyId", u.getCompanyId());
        po.put("EntryUser", u.getId());
        po.put("ModifyUser", u.getId());
        po.put("EntryDate", now);
        po.put("ModifyDate", now);
        po.put("BillCalculateTypeId", r.BillCalculateTypeId);
        po.put("OrderQty", totalQty.setScale(2, RoundingMode.HALF_EVEN));            // Math.Round(TotalQty, 2) :638
        po.put("OrderAmount", totalAmount.setScale(formatDigits(intOf(repo.config(u, "Default NoofDecimal Points For Amount")), 0), RoundingMode.HALF_UP));
        po.put("CurrencyId", r.CurrencyId);
        po.put("ExchangeRate", exRate);
        po.put("FcyAmount", totalFcy.setScale(formatDigits(intOf(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")), 0), RoundingMode.HALF_UP));

        // attachments are written to disk before the header so AttachmentsValues can carry them (DAL :46-49)
        List<Map<String, Object>> newFiles = storeFiles(u, r);
        List<Map<String, Object>> keptFiles = new ArrayList<>();
        if (!insert) {
            Set<Integer> drop = new HashSet<>(r.removeAttachmentIds == null ? List.of() : r.removeAttachmentIds);
            for (var a : repo.attachments(u, r.Id)) if (!drop.contains(intOf(col(a, "Id")))) keptFiles.add(a);
        }
        List<String> names = new ArrayList<>(), custom = new ArrayList<>();
        for (var a : keptFiles) { names.add(Objects.toString(col(a, "Attachment"), "")); custom.add(Objects.toString(col(a, "UploadedFileCustomName"), "")); }
        for (var a : newFiles) { names.add((String) a.get("name")); custom.add((String) a.get("stored")); }
        if (!names.isEmpty()) {
            po.put("AttachmentsValues", String.join(",", names));
            po.put("CustomAttachmentsValues", String.join(",", custom));
        }

        // BLL Save :17 - lock date first
        header.assertNotDateLocked(u.getOrganizationId(), u.getCompanyId(), Date.valueOf(docDate));

        // DAL SetData :51-64
        int id;
        try {
            id = header.save(po);
            BigDecimal limit = BigDecimal.ZERO;
            for (Map<String, Object> d : details) {
                d.put("PurchaseOrderId", id);
                limit = limit.add(BigDecimal.valueOf(PurchaseOrderPmRepository.dbl(d.get("Amount"))));
                repo.saveDetail(d);
            }
            attachments(u, id, r, newFiles);
            if (!insert) {
                if (!removed.isEmpty()) {
                    StringBuilder ids = new StringBuilder();
                    for (int x : removed) ids.append(',').append(x);           // OrderDetailRemoveIds = "," + id ...
                    repo.deleteRemovedDetails(u, id, ids.toString());
                }
                repo.weightValidation(u, id);
            } else {
                repo.approvalDetail(u, id, limit);
            }
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }

        int docNo = intOf(col(repo.header(u, id), "DocNo"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", docNo);
        out.put("message", (insert ? "Data Save Successfully....[" : "Data Update Successfully....[") + docNo + "]");
        return out;
    }

    // ================================================================== files

    private List<Map<String, Object>> storeFiles(UserAccount u, PurchaseOrderPmRequest r) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (r.files == null) return out;
        if (r.files.size() > 10) throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
        for (var file : r.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(file);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("name", file.name);
            m.put("stored", store.store(u, file.name, bytes));
            m.put("size", bytes.length / 1048576d);
            out.add(m);
        }
        return out;
    }

    /** DAL :115-139 - RefAccountId = supplier, RefDocumentTypeId = 700, RefDocumentNo = the order. */
    private void attachments(UserAccount u, int id, PurchaseOrderPmRequest r, List<Map<String, Object>> newFiles) {
        var existing = repo.attachments(u, id);
        for (Integer removed : new LinkedHashSet<>(r.removeAttachmentIds == null ? List.<Integer>of() : r.removeAttachmentIds)) {
            if (removed == null || existing.stream().noneMatch(a -> intOf(col(a, "Id")) == removed)) {
                throw new IllegalArgumentException("Attachment does not belong to this order");
            }
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?, @Activity=?", removed, "AttachmentDeleteById");
        }
        for (var f : newFiles) {
            Timestamp now = new Timestamp(System.currentTimeMillis());
            Object[] v = {0, r.OrderSupCustId, 0, DOCUMENT_TYPE_ID, id, f.get("name"), now, u.getId(), now, u.getId(),
                    u.getOrganizationId(), u.getCompanyId(), 0, SCREEN, false, f.get("stored"), f.get("size"), 0};
            ProcExec.run(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?, @RefAccountId=?, @DMSFoldersLabelsId=?, @RefDocumentTypeId=?, @RefDocumentNo=?, @Attachment=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchId=?, @ScreenName=?, @DetailWiseAttachment=?, @UploadedFileCustomName=?, @UploadedFileSizeMb=?, @LineId=?", v);
        }
    }

    public DesktopInventoryItemFileService.Download attachment(int id, int attachmentId) {
        UserAccount u = user("View");
        repo.header(u, id);
        var row = repo.attachments(u, id).stream().filter(a -> intOf(col(a, "Id")) == attachmentId).findFirst()
                .orElseThrow(() -> new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND, "Attachment not found"));
        String stored = Objects.toString(col(row, "UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = Objects.toString(col(row, "Attachment"), "");
        return new DesktopInventoryItemFileService.Download(basename(Objects.toString(col(row, "Attachment"), stored)),
                store.read(u, basename(stored)), "application/octet-stream");
    }

    // ================================================================== helpers

    /** Architecture.Model.Inventory.PurchaseOrderDetail with every property at its CLR default. */
    private static Map<String, Object> detailDefaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String k : List.of("Amount", "BagPrice", "BagWeight", "NetWeight", "OrderItemQty", "OrderItemRate", "RetailRate",
                "TaxAmount", "TaxPercent", "TotalAmount", "WeightPerQty")) m.put(k, 0d);
        for (String k : List.of("Id", "JobLotId", "LineId", "OrderItemId", "OrderItemRateUOMId", "OrderItemUOMId", "PurchaseOrderId",
                "TaxNameId", "RefDocumentTypeId", "RefDocId", "RefDocInvoiceId", "CityId", "InvLabSampleAnalysisHeaderId",
                "LabAnalysisStandardScheduleId", "CurrencyId", "CropYearId", "LeadTime")) m.put(k, 0);
        for (String k : List.of("AmountCalcType", "CityArea", "Crop", "LabSampleNo", "Moisture", "OrderRemarks", "TaxableStatus", "WeightCapacity"))
            m.put(k, null);
        m.put("ExchangeRate", BigDecimal.ZERO);
        m.put("FcyAmount", BigDecimal.ZERO);
        m.put("PackingDate", null);
        m.put("ExpiryDate", null);
        return m;
    }

    private static Map<String, Object> uom(List<Map<String, Object>> uoms, int itemId, int uomId) {
        for (var m : uoms) if (intOf(m.get("ItemId")) == itemId && intOf(m.get("Id")) == uomId) return m;
        return null;
    }

    /** clsGlobalVariables.DefaultNoofDecimalPointsForAmount - the raw configured number (GetDecimalConfiguration :5380). */
    private int amountRoundDigits(UserAccount u) {
        return Math.max(0, Math.min(15, intOf(repo.config(u, "Default NoofDecimal Points For Amount"))));
    }

    private int fcyRoundDigits(UserAccount u) {
        return Math.max(0, Math.min(15, intOf(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount"))));
    }

    /** The "#,##0." + N format suffix - only 1..4 are honoured, otherwise the stated default (:5382-5397). */
    private static int formatDigits(int n, int fallback) {
        return (n >= 1 && n <= 4) ? n : fallback;
    }

    /** Math.Round(x, n, MidpointRounding.AwayFromZero). */
    private static double round(double v, int digits) {
        return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue();
    }

    private static Date date(String v, String name) {
        try { return Date.valueOf(LocalDate.parse(v.trim().substring(0, 10))); }
        catch (Exception e) { throw new IllegalArgumentException(name + " is not a valid date"); }
    }

    private static Timestamp optionalDate(String v) {
        // A DateTimePicker always holds a value - today until the operator changes it.
        if (v == null || v.isBlank()) return Timestamp.valueOf(LocalDate.now().atStartOfDay());
        try { return Timestamp.valueOf(LocalDate.parse(v.trim().substring(0, 10)).atStartOfDay()); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid packing / expiry date"); }
    }

    private static BigDecimal dec(String v) {
        if (v == null || v.isBlank()) return BigDecimal.ZERO;
        try { return new BigDecimal(v.replace(",", "").trim()); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    private static boolean bool(String v) {
        return v != null && Set.of("true", "1", "yes").contains(v.trim().toLowerCase(Locale.ROOT));
    }

    private static String basename(String name) {
        String n = name.replace('\\', '/');
        String r = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(r);
        return r;
    }
}
