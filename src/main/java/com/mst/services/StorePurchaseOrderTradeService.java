package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.GrnStoreRepository;
import com.mst.repositories.InventoryOpeningRepository;
import com.mst.repositories.PurchaseOrderHeaderRepository;
import com.mst.repositories.PurchaseOrderPmRepository;
import com.mst.repositories.StorePurchaseOrderTradeRepository;
import com.mst.repositories.StorePurchaseOrderTradeRepository.HistoryFilter;
import com.mst.security.CurrentUserContext;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.dao.DataAccessException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.dbl;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;
import static com.mst.repositories.StorePurchaseOrderTradeRepository.DOCUMENT_TYPE_ID;
import static com.mst.repositories.StorePurchaseOrderTradeRepository.SCREEN;

/**
 * Store Purchase (ModuleId 67) screen 328 "Purchase Order Store" - desktop form
 * {@code Architecture.WinApp.PurchaseTrading.PurchsaeOrderTrade}, ScreenName "PurchsaeOrderTrade",
 * DocumentTypeId 42, route /store/purchase-order-store.
 *
 * The form is the Purchase Order BLL ({@code PurchaseOrder.Save / GetByID / Getall}) run for document type 42 behind
 * a store-sized GUI: no crop year, currency, lab analysis, empty-bag or expense tabs (those belong to
 * {@code PurchsaeOrder}, screen 41, a different class). Header, detail, removed-row and weight-validation writes are
 * the shared PurchaseOrder repositories; only the document type, the validations and the page differ.
 *
 * Save follows {@code btnsave_Click :1690 -> Insert() :1499} in order: Complete/Cancel refused in Save mode,
 * FormValidation :1023, "Grid Record Not Found", then per row (:1561-1617) the refusals with their messages, then BLL
 * Save (date lock, detail-id guard) and DAL SetData in one transaction: header, one Sp_PurchaseOrderDetail_Insert per
 * row, on update the removed-row procedure and PoWeightAndGpWeightValidation, on insert the approval-detail row.
 *
 * WEB DEVIATIONS (each one stated in _claude_stage/sale_missing/reports/ST2.md):
 *  D1 Amounts are recomputed here from the stored UOM equivalents (CalculateWeight :1436 / CalculateAmount :1446). The
 *     desktop's ReadById :1763 loads Amount as Math.Round(Amount) (whole numbers), so an unedited Update re-saves the
 *     rounded figure there; the web does not reproduce that loss.
 *  D2 Attachments are not ported (the Attachment button is shown disabled, as on the other Store pages).
 *  D3 Rows loaded from a Purchase Demand (RefDocumentTypeId 141) are re-checked on the server against the pending
 *     loader's balance (+ this order's own stored quantity on update) - the desktop only checks in the grid cell.
 *  D4 The taxable run of the form (Tag "frmPurchaseOrderWithTax", type 132) is not part of screen 328.
 */
@Service
public class StorePurchaseOrderTradeService {

    private static final int DEMAND_DOCUMENT_TYPE_ID = 141;               // frmPendingPurchaseDemand :59

    private final StorePurchaseOrderTradeRepository repo;
    private final PurchaseOrderPmRepository shared;
    private final PurchaseOrderHeaderRepository header;
    private final GrnStoreRepository grn;
    private final InventoryOpeningRepository years;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StorePurchaseOrderTradeService(StorePurchaseOrderTradeRepository repo, PurchaseOrderPmRepository shared,
                                          PurchaseOrderHeaderRepository header, GrnStoreRepository grn,
                                          InventoryOpeningRepository years, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo; this.shared = shared; this.header = header; this.grn = grn;
        this.years = years; this.rights = rights; this.ctx = ctx;
    }

    // ================================================================== request shapes

    public static class Request {
        public int Id;
        public String DocDate;
        public int OrderSupCustId;
        public String SupplierRefNo;
        public String RemarksHeader;
        public int PaymentTermsId;
        public String OrderDueDays;
        public String OrderDueDate;
        public int DeliveryTermId;
        public String DeliveryStartDate;
        public String DeliveryDays;
        public int StatusId;
        public List<Integer> removedDetailIds;
        public List<Line> lines;
    }

    public static class Line {
        public int Id;
        public int RefDocumentTypeId;
        public int RefDocId;
        public int RefDocInvoiceId;
        public int ItemId;
        public int JobLotId;
        public int PackUomId;
        public double ItemQty;
        public double Rate;
        public int RateUomId;
        public String Remarks;
    }

    // ================================================================== load

    private UserAccount user() { return ctx.requireAccountingUser(); }

    private void need(String key, String message) {
        if (!rights.has(SCREEN, key)) throw new IllegalStateException(message);
    }

    private int financialYear() {
        UserAccount u = user();
        var list = years.years(u);
        if (list.isEmpty()) throw new IllegalArgumentException("No active financial year allocated to this company");
        return intOf(list.get(0).get("Id"));
    }

    private int amountRoundDigits(UserAccount u) {
        return Math.max(0, Math.min(15, intOf(shared.config(u, "Default NoofDecimal Points For Amount"))));
    }

    private static int formatDigits(int n, int fallback) { return (n >= 1 && n <= 4) ? n : fallback; }

    private static boolean bool(String v) {
        return v != null && Set.of("true", "1", "yes").contains(v.trim().toLowerCase(Locale.ROOT));
    }

    /** PurchsaeOrder_Load :401-531, the combo fills and the configuration reads the page needs. */
    public Map<String, Object> lookups() {
        UserAccount u = user();
        need("view", "You do not have the View right for this screen.");
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("rights", rights.of(SCREEN));
        r.put("suppliers", repo.suppliers(u));
        r.put("dueTerms", repo.dueTerms(u));
        boolean packingNotOnStore = bool(shared.config(u, "PackingMaterialItemsNotIncludeOnStore"));
        r.put("items", repo.items(u, packingNotOnStore ? "17" : "14,17"));          // ItemDetailFill :811
        r.put("uoms", shared.uoms(u));
        r.put("defaultJobLotId", intOf(shared.config(u, "Job/Lot")));
        r.put("jobLots", repo.jobLots(u));
        r.put("deliveryTerms", List.of(idName(1, "Load"), idName(2, "Ponch")));      // DeliveryTerm :702
        r.put("statuses", List.of(idName(1, "Open"), idName(2, "Complete"), idName(3, "Cancel")));   // OrderStatus :762
        boolean branchWise = bool(shared.config(u, "PurchaseOrderBranchWise"));
        r.put("branchImplemented", branchWise);
        int days = intOf(shared.config(u, "DefaultDaysToLessFromHistoryFromDate"));
        r.put("defaultDaysToLessFromHistoryFromDate", days);
        r.put("amountDecimals", amountRoundDigits(u));
        r.put("amountFormatDecimals", formatDigits(intOf(shared.config(u, "Default NoofDecimal Points For Amount")), 0));
        r.put("historyBranches", historyBranches(u, branchWise));
        r.put("userBranchId", u.getBranchesId());
        r.putAll(numbers());
        return r;
    }

    private List<Map<String, Object>> historyBranches(UserAccount u, boolean branchWise) {
        if (branchWise) {                                                             // HistoryBranchComboFill :2063
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", u.getBranchesId());
            m.put("BranchName", repo.branchName(u, u.getBranchesId()));
            return List.of(m);
        }
        return repo.branchesFromPurchaseOrder(u);
    }

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("Name", name);
        return m;
    }

    /** DocumentNoFill :794 and BranchSrNoFill :540 - the numbers shown on New; the save allocates the real ones. */
    public Map<String, Object> numbers() {
        UserAccount u = user();
        int fy = financialYear();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", header.nextDocNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy));
        m.put("branchSrNo", header.nextBranchSrNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, u.getBranchesId(), fy));
        return m;
    }

    /** HistoryComboFill :2010 for the branches ticked in the combo. */
    public List<Map<String, Object>> historySuppliers(List<Integer> branchIds) {
        UserAccount u = user();
        need("view", "You do not have the View right for this screen.");
        return repo.historySuppliers(u, branchString(u, branchIds));
    }

    private String branchString(UserAccount u, List<Integer> ids) {
        if (ids == null || ids.isEmpty()) throw new IllegalArgumentException("Select branch first");     // :2054 / :2224
        boolean branchWise = bool(shared.config(u, "PurchaseOrderBranchWise"));
        Set<Integer> allowed = new HashSet<>();
        for (var b : historyBranches(u, branchWise)) allowed.add(intOf(b.get("Id")));
        StringBuilder s = new StringBuilder();
        for (Integer id : new LinkedHashSet<>(ids)) {
            if (id == null || !allowed.contains(id)) continue;
            s.append(',').append(id);                                                 // BranchIds + "," + id
        }
        if (s.length() == 0) throw new IllegalArgumentException("Select branch first");
        return s.toString();
    }

    // ================================================================== history

    public List<Map<String, Object>> history(String dateMode, String from, String to, int fromDocNo, int toDocNo,
                                             int supplierId, List<Integer> branchIds) {
        UserAccount u = user();
        need("view", "You do not have the View right for this screen.");
        HistoryFilter f = new HistoryFilter();
        f.canViewAll = rights.has(SCREEN, "viewAll");
        f.financialYearId = financialYear();
        f.dateMode = dateMode == null ? "doc" : dateMode;
        f.from = (from == null || from.isBlank()) ? null : date(from, "From Date");
        f.to = (to == null || to.isBlank()) ? null : date(to, "To Date");
        f.fromDocNo = fromDocNo;
        f.toDocNo = toDocNo;
        f.supplierId = supplierId;
        f.branchIds = branchString(u, branchIds);
        return plain(repo.history(u, f));
    }

    /** ReadById :1723 / HistoryGridDetailBind :2391 - header and the grid rows as the form builds them (:1763). */
    public Map<String, Object> record(int id) {
        UserAccount u = user();
        need("view", "You do not have the View right for this screen.");
        Map<String, Object> h = repo.header(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", plainRow(h));
        List<Map<String, Object>> lines = new ArrayList<>();
        for (var d : repo.details(id)) {
            Map<String, Object> l = new LinkedHashMap<>();
            l.put("Id", intOf(col(d, "Id")));
            l.put("RefDocumentTypeId", intOf(col(d, "RefDocumentTypeId")));
            l.put("RefDocId", intOf(col(d, "RefDocId")));
            l.put("RefDocInvoiceId", intOf(col(d, "RefDocInvoiceId")));
            l.put("RefDocNo", col(d, "RefDocNo"));
            l.put("ItemId", intOf(col(d, "OrderItemId")));
            l.put("ItemCode", col(d, "ItemCode"));
            l.put("Item", col(d, "ItemName"));
            l.put("JobLotId", intOf(col(d, "JobLotId")));
            l.put("JobLot", col(d, "JobLotDescription"));
            l.put("PackUomId", intOf(col(d, "OrderItemUOMId")));
            l.put("PackUom", col(d, "UOMCode"));
            l.put("PackEquivalent", dbl(col(d, "UOMDescription")));                  // :1768 puts UOMDescription (= Equivalent) here
            l.put("ItemQty", dbl(col(d, "OrderItemQty")));
            l.put("TotalQty", dbl(col(d, "NetWeight")));
            l.put("Rate", dbl(col(d, "OrderItemRate")));
            l.put("RateUomId", intOf(col(d, "OrderItemRateUOMId")));
            l.put("RateUom", col(d, "RateUom"));
            l.put("RateEquivalent", dbl(col(d, "EquivalentRate")));
            l.put("Amount", dbl(col(d, "Amount")));
            l.put("Remarks", col(d, "OrderRemarks"));
            l.put("BalQty", dbl(col(d, "DemandBalanceQty")));
            lines.add(l);
        }
        out.put("lines", lines);
        return out;
    }

    // ================================================================== Purchase Demand loader (btnLoader_Click :3366)

    /** frmPendingPurchaseDemand.ComboDbCall :104 - ("141", "Item"). */
    public Map<String, Object> demandLoaderLookups() {
        UserAccount u = user();
        need("view", "You do not have the View right for this screen.");
        List<Map<String, Object>> items = new ArrayList<>();
        for (var r : grn.purchaseDemandDropDowns(u, String.valueOf(DEMAND_DOCUMENT_TYPE_ID), "Item")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", intOf(col(r, "Id")));
            o.put("Name", col(r, "ReferenceName"));
            items.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items);
        return out;
    }

    /** PendingDataDbCall :154 -> PurchaseDemand_PendingDataLoader (branch, year, type 141 sent by the form). */
    public List<Map<String, Object>> pendingDemands(String from, String to, int fromDocNo, int toDocNo, int itemId) {
        UserAccount u = user();
        need("view", "You do not have the View right for this screen.");
        return plain(grn.pendingPurchaseDemands(u, u.getBranchesId(), financialYear(), DEMAND_DOCUMENT_TYPE_ID,
                stamp(from), stamp(to), fromDocNo, toDocNo, itemId));
    }

    private static Timestamp stamp(String v) {
        if (v == null || v.isBlank()) return null;
        return Timestamp.valueOf(date(v, "Date").toLocalDate().atStartOfDay());
    }

    // ================================================================== save

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(Request r) {
        if (r == null || r.Id < 0) throw new IllegalArgumentException("Invalid Purchase Order");
        boolean insert = r.Id == 0;
        UserAccount u = user();
        if (insert) need("save", "You do not have the Save right for this screen.");
        else need("update", "You do not have the Update right for this screen.");
        Map<String, Object> old = insert ? null : repo.header(u, r.Id);

        String status = switch (r.StatusId) { case 1 -> "Open"; case 2 -> "Complete"; case 3 -> "Cancel"; default -> ""; };
        if (insert && ("Complete".equals(status) || "Cancel".equals(status))) {          // btnsave_Click :1696
            throw new IllegalArgumentException("Order Status Should not Be Complete Or Cancel In Save Mode");
        }

        // -------------------------------------------------------- FormValidation :1023
        List<Map<String, Object>> suppliers = repo.suppliers(u);
        if (r.OrderSupCustId == 0 || suppliers.stream().noneMatch(s -> intOf(s.get("Id")) == r.OrderSupCustId)) {
            throw new IllegalArgumentException("Supplier Name Field is Required");
        }
        String deliveryTerm = switch (r.DeliveryTermId) { case 1 -> "Load"; case 2 -> "Ponch"; default -> null; };
        if (deliveryTerm == null) throw new IllegalArgumentException("Delivery Term Field is Required");
        String termText = null;
        for (var t : repo.dueTerms(u)) if (intOf(t.get("Id")) == r.PaymentTermsId) termText = Objects.toString(t.get("TermsDescription"), "");
        if (r.PaymentTermsId == 0 || termText == null) throw new IllegalArgumentException("Payment Term Field is Required");
        int dueDays = intOf(r.OrderDueDays);
        if ("Credit".equals(termText) && dueDays <= 0) throw new IllegalArgumentException("Due Days Field is Required");

        // -------------------------------------------------------- Insert() :1499
        if (r.lines == null || r.lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        LocalDate docDate = date(r.DocDate, "Doc Date").toLocalDate();
        LocalDate dueDate = "Cash".equals(termText) ? docDate                              // combpttrm_TextChanged :1812
                : (r.OrderDueDate == null || r.OrderDueDate.isBlank() ? LocalDate.now().plusDays(dueDays) : date(r.OrderDueDate, "Due Date").toLocalDate());
        if ("Cash".equals(termText)) dueDays = 0;
        LocalDate deliveryStart = r.DeliveryStartDate == null || r.DeliveryStartDate.isBlank() ? LocalDate.now()
                : date(r.DeliveryStartDate, "Delivery Start Date").toLocalDate();

        int amountDigits = amountRoundDigits(u);
        boolean packingNotOnStore = bool(shared.config(u, "PackingMaterialItemsNotIncludeOnStore"));
        Set<Integer> itemIds = new HashSet<>();
        for (var it : repo.items(u, packingNotOnStore ? "17" : "14,17")) itemIds.add(intOf(it.get("Id")));
        Set<Integer> lotIds = new HashSet<>();
        for (var j : repo.jobLots(u)) lotIds.add(intOf(j.get("Id")));
        List<Map<String, Object>> uoms = shared.uoms(u);
        Set<Integer> existingDetailIds = new HashSet<>();
        Map<String, Double> ownQtyByDemand = new HashMap<>();
        Map<String, Double> storedBalance = new HashMap<>();
        if (!insert) {
            for (var d : repo.details(r.Id)) {
                existingDetailIds.add(intOf(col(d, "Id")));
                if (intOf(col(d, "RefDocumentTypeId")) == DEMAND_DOCUMENT_TYPE_ID) {
                    String k = intOf(col(d, "RefDocId")) + ":" + intOf(col(d, "RefDocInvoiceId"));
                    storedBalance.put(k, dbl(col(d, "DemandBalanceQty")));
                    ownQtyByDemand.merge(k, dbl(col(d, "OrderItemQty")), Double::sum);
                }
            }
        }
        Map<String, Double> pendingBalance = null;
        Map<String, Double> usedByDemand = new HashMap<>();

        List<Map<String, Object>> details = new ArrayList<>();
        BigDecimal limit = BigDecimal.ZERO;
        int rowNo = 0;
        for (Line l : r.lines) {
            rowNo++;
            if (l == null) throw new IllegalArgumentException("Grid Record Not Found");
            if (l.ItemId == 0 || !itemIds.contains(l.ItemId)) throw new IllegalArgumentException("Item not found in row#" + rowNo);
            if (l.JobLotId == 0 || !lotIds.contains(l.JobLotId)) throw new IllegalArgumentException("Job Lot not found in row#" + rowNo);
            Map<String, Object> pack = uom(uoms, l.ItemId, l.PackUomId);
            if (l.PackUomId == 0 || pack == null) throw new IllegalArgumentException("Pack Uom not found in row#" + rowNo);
            if (!(l.ItemQty > 0d)) throw new IllegalArgumentException("Qty should be greater than 0 in row#" + rowNo);
            double packEq = dbl(pack.get("Equivalent"));
            double totalQty = (l.ItemQty > 0d && packEq > 0d) ? packEq * l.ItemQty : 0d;       // CalculateWeight :1436
            if (!(totalQty > 0d)) throw new IllegalArgumentException("Total Qty should be greater than 0 in row#" + rowNo);
            if (!(l.Rate > 0d)) throw new IllegalArgumentException("Rate should be greater than 0 in row#" + rowNo);
            Map<String, Object> rateUom = uom(uoms, l.ItemId, l.RateUomId);
            if (l.RateUomId == 0 || rateUom == null) throw new IllegalArgumentException("Rate Uom not found in row#" + rowNo);
            double rateEq = dbl(rateUom.get("Equivalent"));
            double amount = 0d;                                                                   // CalculateAmount :1446
            if (l.ItemQty > 0d && rateEq > 0d && l.Rate > 0d && totalQty > 0d) amount = totalQty / rateEq * l.Rate;
            amount = round(amount, amountDigits);
            if (!(amount > 0d)) throw new IllegalArgumentException("Item Amount should be greater than 0 in row#" + rowNo);

            // a row that came from a Purchase Demand (D3)
            int refType = l.RefDocumentTypeId, refDoc = l.RefDocId, refInv = l.RefDocInvoiceId;
            if (refType != 0 && refType != DEMAND_DOCUMENT_TYPE_ID) throw new IllegalArgumentException("Ref Document Type is not valid in row#" + rowNo);
            if (refType == 0) { refDoc = 0; refInv = 0; }
            if (refType == DEMAND_DOCUMENT_TYPE_ID) {
                if (refDoc <= 0 || refInv <= 0) throw new IllegalArgumentException("Ref Doc is not valid in row#" + rowNo);
                String k = refDoc + ":" + refInv;
                Double max = storedBalance.get(k);
                if (max == null) {
                    if (pendingBalance == null) pendingBalance = pendingBalances(u);
                    max = pendingBalance.get(k);
                    if (max == null) throw new IllegalArgumentException("Ref Doc is not available for this company in row#" + rowNo);
                    max = max + ownQtyByDemand.getOrDefault(k, 0d);
                }
                double used = usedByDemand.merge(k, l.ItemQty, Double::sum);
                if (used > max + 0.0000001) {
                    throw new IllegalArgumentException("Qty exceeds allowed balance for row# " + rowNo + ".\nmax allowed is " + trim(max - (used - l.ItemQty)) + ".");
                }
            }
            if (insert && l.Id > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
            if (!insert && l.Id > 0 && !existingDetailIds.contains(l.Id)) throw new IllegalArgumentException("Detail row does not belong to this order in row#" + rowNo);

            Map<String, Object> d = detailDefaults();
            d.put("Id", insert ? 0 : l.Id);                                                       // vd.Id = RecId > 0 ? cell : 0 :1566
            d.put("RefDocumentTypeId", refType);
            d.put("RefDocId", refDoc);
            d.put("RefDocInvoiceId", refInv);
            d.put("OrderItemId", l.ItemId);
            d.put("JobLotId", l.JobLotId);
            d.put("OrderItemUOMId", l.PackUomId);
            d.put("OrderItemQty", l.ItemQty);
            d.put("NetWeight", totalQty);
            d.put("OrderItemRate", l.Rate);
            d.put("RetailRate", 0d);
            d.put("OrderItemRateUOMId", l.RateUomId);
            d.put("Amount", amount);
            d.put("OrderRemarks", l.Remarks == null ? "" : l.Remarks);
            d.put("TaxNameId", 0);
            d.put("TaxPercent", 0d);
            d.put("TaxAmount", 0d);
            d.put("TotalAmount", amount);                                                         // CalculateAmount :1458 sets TotalAmount = Amount
            details.add(d);
            limit = limit.add(BigDecimal.valueOf(amount));
        }

        // removed rows - grd_ColumnButtonClick :1157
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

        // -------------------------------------------------------- header :1531-1563
        int fy = financialYear();
        Timestamp now = StorePurchaseOrderTradeRepository.now();
        Map<String, Object> po = PurchaseOrderHeaderRepository.blankModel();
        po.put("Id", insert ? 0 : r.Id);
        po.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        po.put("BranchesId", u.getBranchesId());
        po.put("ProjectsId", u.getBranchesId());                                                  // :1537 (BranchesId)
        po.put("DocDate", Timestamp.valueOf(docDate.atStartOfDay()));
        po.put("DocNo", insert ? header.nextDocNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy) : intOf(col(old, "DocNo")));
        po.put("BranchSrNo", insert
                ? header.nextBranchSrNo(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, u.getBranchesId(), fy)
                : intOf(col(old, "BranchSrNo")));
        po.put("OrderCatagoryId", 0);                                                             // combordercat is never bound on this run
        po.put("CatagorySrNo", 0);
        po.put("OrderSupCustId", r.OrderSupCustId);
        po.put("SupplierRefNo", r.SupplierRefNo == null ? "" : r.SupplierRefNo);
        po.put("RemarksHeader", r.RemarksHeader == null ? "" : r.RemarksHeader.trim());
        po.put("PaymentTermsId", r.PaymentTermsId);
        po.put("RefrenenceParty", 0);                                                             // CmbAgreementNo is never bound
        po.put("OrderDueDays", dueDays);
        po.put("OrderDueDate", Timestamp.valueOf(dueDate.atStartOfDay()));
        po.put("OrderExpiryDate", Timestamp.valueOf(LocalDateTime.now().toLocalDate().atStartOfDay()));   // the hidden "Validite Date" picker, today
        po.put("DeliveryTerm", deliveryTerm);
        po.put("DeliveryStartDate", Timestamp.valueOf(deliveryStart.atStartOfDay()));
        po.put("DeliveryDays", intOf(r.DeliveryDays));
        po.put("OrderType", "Whole");                                                             // CmbWholeSale hidden, Rows[1]
        po.put("OrderStatus", status.isEmpty() ? "Open" : status);
        po.put("OrderTaxable", false);
        po.put("BrokerAgentSupCustId", 0);                                                        // combsalesman is never bound
        po.put("OrganizationId", u.getOrganizationId());
        po.put("FinancialYearId", fy);
        po.put("CompanyId", u.getCompanyId());
        po.put("EntryUser", u.getId());
        po.put("ModifyUser", u.getId());
        po.put("EntryDate", now);
        po.put("ModifyDate", now);

        // BLL Save - lock date first
        header.assertNotDateLocked(u.getOrganizationId(), u.getCompanyId(), Date.valueOf(docDate));

        int id;
        try {
            id = header.save(po);
            for (Map<String, Object> d : details) {
                d.put("PurchaseOrderId", id);
                shared.saveDetail(d);
            }
            if (!insert) {
                if (!removed.isEmpty()) {
                    StringBuilder ids = new StringBuilder();
                    for (int x : removed) ids.append(',').append(x);                 // OrderDetailRemoveIds = "," + id ...
                    shared.deleteRemovedDetails(u, id, ids.toString());
                }
                shared.weightValidation(u, id);
            } else {
                repo.approvalDetail(u, id, limit);
            }
        } catch (DataAccessException e) {
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

    private Map<String, Double> pendingBalances(UserAccount u) {
        Map<String, Double> m = new HashMap<>();
        for (var d : grn.pendingPurchaseDemands(u, u.getBranchesId(), financialYear(), DEMAND_DOCUMENT_TYPE_ID, null, null, 0, 0, 0)) {
            m.put(intOf(col(d, "Id")) + ":" + intOf(col(d, "DetailId")), dbl(col(d, "BalanceQty")));
        }
        return m;
    }

    // ================================================================== print (201)

    /** GenerateReport :2831 -> CommonServices.PurchaseOrderSlipReport201 -> Sp_PurchaseOrder_GeneralOrderSlip_Rpt. */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = user();
        need("print", "You do not have the Print right for this screen.");
        repo.header(u, id);
        return plain(grn.purchaseOrderGeneralSlip(u, id));
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

    /** Math.Round(x, n, MidpointRounding.AwayFromZero). */
    private static double round(double v, int digits) {
        return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue();
    }

    private static String trim(double v) {
        BigDecimal b = BigDecimal.valueOf(v).setScale(6, RoundingMode.HALF_UP).stripTrailingZeros();
        return b.toPlainString();
    }

    private static Date date(String v, String name) {
        try { return Date.valueOf(LocalDate.parse(v.trim().substring(0, 10))); }
        catch (Exception e) { throw new IllegalArgumentException(name + " is not a valid date"); }
    }

    private static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : rows) out.add(plainRow(r));
        return out;
    }

    private static Map<String, Object> plainRow(Map<String, Object> r) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (var e : r.entrySet()) {
            Object v = e.getValue();
            if (v instanceof Timestamp t) v = t.toLocalDateTime().toString();
            else if (v instanceof java.util.Date d) v = new Timestamp(d.getTime()).toLocalDateTime().toString();
            else if (v instanceof BigDecimal b) v = b.doubleValue();
            o.put(e.getKey(), v);
        }
        return o;
    }
}
