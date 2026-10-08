package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels.*;
import com.mst.repositories.SaleInvoiceTradingRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import java.util.stream.Collectors;

import static com.mst.repositories.SaleInvoiceTradingRepository.DOC;
import static com.mst.repositories.SaleInvoiceTradingRepository.SCREEN;
import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Packing Material (ModuleId 54) - screen 507 "Sale Invoice Packing Material".
 *
 *   Desktop form : Architecture.WinApp.* InvfrmInvSaleInvoiceDirectPackingMaterial (ScreenName FrmSaleInvoiceDirectPackingMaterial)
 *   DocumentTypeId 126, BaseDocumentTypeId 2, route /packing-material/sale-invoice, API /api/packing-material/sale-invoice
 *   GDN loader    : frmLoadGdnStoreAndPm (OrderCategoryId 9) - usp_getGdndataForSaleInvoice_Store, one GDN at a time
 *   Save          : Insert() :2037 -> BLL 0580 InvSaleInvoice.Save -> SaleInvoiceFinancial (0612/0613) -> DAL 0433 SetData,
 *                   shared with screen 95 through {@link SaleInvoiceService#persist}. A manual invoice (no GDN line) only
 *                   refreshes the evaluation rows (UpdateInventoryReference) and rebuilds stock (InventoryTransactions);
 *                   a GDN invoice moves the 149 GDN's stock rows to the invoice (GdnReferences_Update).
 *   Prints        : CommonServices.SaleInvoiceStorePMSlip(Id, 126) -> SaleInvoiceDirectItemSlip_294 ("sidpm-294"),
 *                   VoucherReport_118 ("acc-118").
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *  1. FillDetailRow takes the tax percent with Conversion.ToInt of the "#,##.###" text: a fractional percent (17.5) or one of
 *     1,000 or more becomes 0 - the row keeps its Tax Name with 0 % and 0 tax.
 *  2. The detail Remarks column is never copied to the model, so it is not saved.
 *  3. SupplierInvoiceNo is the Due Days; SupplierInvoiceDate is the due date.
 *  4. The sales-tax number is regenerated on every save that has tax, update included.
 *  5. ReadById fills a non-subsidiary freight grid with the FIRST freight row's account on every row.
 *  6. A loaded GDN line with no rack keeps rack 0 (LoadInGridDetail writes the resolved rack to the source row, not the grid
 *     row), and "Rack Name is required" then refuses the save. Every row also needs a Secondary Uom, its weight and rate.
 *  7. Delete goes through InvPurchaseInvoice.RemoveByID (Sp_InvoicesVouchersandStocksDelete) without the sale-invoice
 *     reference check.
 *  8. The GDN loader never sets its DocumentTypeId: branches come from GetBranchesAllocatedToUserFromGdn with no type.
 *
 * DEVIATIONS (web-only)
 *  A. DocNo is the generator value on insert and the stored header's on update.
 *  B. GDN lines are re-read from their source (the pending GDN, or the saved invoice) and must match it; manual lines are
 *     re-priced here (CalculateAmount). All proportions and the bill amount are recomputed here.
 *  C. Grid F1 pop-ups for warehouse / rack / job lot / condition are not ported (a manual row is edited from the entry bar);
 *     the tax pop-up is a per-row button. Attachments, grid layouts and the shortcut pop-up are not ported.
 */
@Service
public class SaleInvoiceTradingService {

    /** frmLoadGdnStoreAndPm.OrderCategoryId for BaseDocumentTypeId 0 (loader.OrderCategoryId = 0, so @OrderCategoryId is not sent). */
    public static final int ORDER_CATEGORY = 0;
    private static final String[] COMM_TYPES = {"Flat", "Percent", "Comm Weight"};
    private static final String[] COMM_UOMS = {"40", "50", "60", "100"};

    private final SaleInvoiceTradingRepository own;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceService shared;
    private final SaleInvoiceDirectPersist persist;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public SaleInvoiceTradingService(SaleInvoiceTradingRepository own, SaleInvoiceRepository repo, SaleInvoiceService shared, SaleInvoiceDirectPersist persist,
                                      StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own; this.repo = repo; this.shared = shared; this.persist = persist; this.rights = rights; this.ctx = ctx;
    }

    private UserAccount user() { return ctx.requireAccountingUser(); }
    private Map<String, Boolean> r() { return rights.of(SCREEN); }

    // ================================================================================= load

    /** InitializeComponentMethod :538. */
    public Map<String, Object> lookups() {
        UserAccount u = user();
        boolean subsidiary = repo.feature(u, 4);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", r());
        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("subsidiary", subsidiary);
        cfg.put("defaultDaysToLessFromHistoryFromDate", i(repo.config(u, "DefaultDaysToLessFromHistoryFromDate").trim()));
        cfg.put("itemSearchByCode", truthy(repo.config(u, "ItemSearchByCode")));
        cfg.put("creditAmountInItemSaleGL", repo.config(u, "CreditAmountInItemSaleGL").trim());
        String dp = repo.config(u, "DefaultNoofDecimalPointsForAmount").trim();
        cfg.put("amountDecimals", dp.isEmpty() ? 0 : i(dp));
        cfg.put("defaultWarehouseForStoreFlow", i(repo.config(u, "DefaultWarehouseForStoreFlow").trim()));
        cfg.put("saleInvoiceBranchWise", truthy(repo.config(u, "SaleInvoiceBranchWise")));
        LocalDateTime start = repo.financialYearStart(u, ctx.currentFinancialYearId());
        cfg.put("yearStart", start == null ? null : start.toLocalDate().toString());
        m.put("configuration", cfg);
        m.put("customers", repo.customers(u, false));
        m.put("accounts", repo.accounts(u, subsidiary, false));
        m.put("taxAccounts", own.taxAccounts(u));
        List<Map<String, Object>> terms = new ArrayList<>();
        for (var t : repo.paymentTerms(u)) terms.add(two(i(col(t, "Id")), s(col(t, "TermsDescription"))));
        m.put("paymentTerms", terms);
        List<Map<String, Object>> ct = new ArrayList<>(), cu = new ArrayList<>();
        for (int k = 0; k < COMM_TYPES.length; k++) ct.add(two(k + 1, COMM_TYPES[k]));
        for (int k = 0; k < COMM_UOMS.length; k++) cu.add(two(k + 1, COMM_UOMS[k]));
        m.put("commissionTypes", ct);
        m.put("commissionUoms", cu);
        m.put("jobLots", own.jobLots(u));
        m.put("items", own.items(u));
        m.put("uoms", own.uoms(u));
        m.put("racks", own.racks(u));
        m.put("conditions", own.conditions());
        m.put("historyCustomers", own.historyCustomers(u));
        m.putAll(numbers());
        return m;
    }

    /** DocumentNoDbCall + GenerateSalesTaxNo. */
    public Map<String, Object> numbers() {
        UserAccount u = user();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(u, ctx.currentFinancialYearId(), DOC));
        m.put("taxNo", own.salesTaxNo(u));
        return m;
    }

    public List<Map<String, Object>> historyCustomers() { return own.historyCustomers(user()); }

    /** TaxTypeDbCall :1011 - {Id, TaxType, TaxPrcnt} from the item's schedule row. */
    public List<Map<String, Object>> taxOptions(int itemId, String docDate) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        var rows = own.taxForItem(u, itemId, day(docDate));
        if (!rows.isEmpty()) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(rows.get(0), "TaxNameId"))); x.put("TaxType", s(col(rows.get(0), "TaxName"))); x.put("TaxPrcnt", d(col(rows.get(0), "TaxPercent")));
            out.add(x);
        }
        return out;
    }

    public List<Map<String, Object>> taxByItems(String itemIds, String docDate) {
        String ids = itemIds == null ? "" : itemIds.replaceAll("[^0-9,]", "");
        return own.taxByItems(user(), ids.startsWith(",") ? ids : "," + ids, day(docDate));
    }

    /** AvailableStockGetByItem :3182 - Math.Round(QtyInHand, 2). */
    public Map<String, Object> stock(int itemId, String docDate, int conditionId, int warehouseId, int rackId) {
        double q = itemId == 0 ? 0 : own.stockInHand(user(), itemId, day(docDate), conditionId, warehouseId, rackId);
        return Map.of("qtyInHand", BigDecimal.valueOf(q).setScale(2, RoundingMode.HALF_EVEN).doubleValue());
    }

    // ================================================================================= GDN loader

    /** frmLoadGRN_Load / BranchesFill. */
    public Map<String, Object> gdnBranches() {
        UserAccount u = user();
        boolean branchWise = truthy(repo.config(u, "SaleInvoiceBranchWise"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", own.gdnBranches(u, branchWise));
        m.put("branchImplemented", branchWise);
        LocalDateTime start = repo.financialYearStart(u, ctx.currentFinancialYearId());
        m.put("fromDate", start == null ? null : start.toLocalDate().toString());
        return m;
    }

    /** PendingGdnLoad - the rows of every pending 149 GDN of order category 9 for the checked branches. */
    public List<Map<String, Object>> pendingGdns(String from, String to, String branchIds) {
        UserAccount u = user();
        boolean branchWise = truthy(repo.config(u, "SaleInvoiceBranchWise"));
        Set<Integer> allowed = own.gdnBranches(u, branchWise).stream().map(x -> i(col(x, "BranchId"))).collect(Collectors.toSet());
        StringBuilder ids = new StringBuilder();
        for (String p : (branchIds == null ? "" : branchIds).split(",")) { int b = i(p); if (b > 0 && allowed.contains(b)) ids.append(',').append(b); }
        if (ids.length() == 0) throw new IllegalArgumentException("Select branch first");
        return own.gdnData(u, ctx.currentFinancialYearId(), dt(from), dt(to), ids.toString(), ORDER_CATEGORY);
    }

    /** The source rows of one pending GDN (BtnLoad): every branch, no dates. */
    private List<Map<String, Object>> gdnSource(UserAccount u, int gdnId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var x : own.gdnData(u, ctx.currentFinancialYearId(), null, null, null, ORDER_CATEGORY)) if (i(col(x, "Id")) == gdnId) out.add(x);
        return out;
    }

    public List<Map<String, Object>> loadGdn(int gdnId) {
        if (gdnId == 0) throw new IllegalArgumentException("Invalid GDN selected");
        var rows = gdnSource(user(), gdnId);
        if (rows.isEmpty()) throw new IllegalArgumentException("No data found for selected GDN");
        return rows;
    }

    // ================================================================================= read / delete / history

    public Map<String, Object> read(int id) {
        Map<String, Object> m = shared.read(id, DOC);
        String screen = s(col(m, "ScreenName")).trim();
        Object base = col(m, "BaseDocumentTypeId");
        if (!screen.isEmpty() && !SCREEN.equals(screen) && base != null && i(base) != 0)
            throw new NoSuchElementException("Sale Invoice not found");
        return m;
    }

    /** btnDelete_Click :1997. */
    @Transactional
    public Map<String, Object> delete(int id) {
        if (!r().get("delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        Map<String, Object> m = read(id);
        if (b(col(m, "IsApproved"))) throw new IllegalArgumentException("Record cannot be  Delete because Record has approved");
        repo.delete(user(), id, DOC);
        return Map.of("message", "Delete Record Successfully");
    }

    public List<Map<String, Object>> history(String dateMode, String from, String to, int fromDocNo, int toDocNo, int customerId) {
        UserAccount u = user();
        return own.history(u, ctx.currentFinancialYearId(), r().get("viewAll"), dateMode, dt(from), dt(to), fromDocNo, toDocNo, customerId);
    }

    // ================================================================================= save (Insert :2037)

    @Transactional
    public Map<String, Object> save(Map<String, Object> req) {
        UserAccount u = user();
        int year = ctx.currentFinancialYearId();
        Map<String, Boolean> rt = r();
        int id = i(req.get("Id"));
        if (!rt.get(id > 0 ? "update" : "save")) throw new IllegalStateException("You do not have the " + (id > 0 ? "Update" : "Save") + " right for this screen.");
        Map<String, Object> existing = null;
        if (id > 0) {
            existing = read(id);
            if (b(col(existing, "IsApproved"))) throw new IllegalArgumentException("Record Not Update because Record has approved");
        }
        boolean subsidiary = repo.feature(u, 4);
        List<Map<String, Object>> customers = repo.customers(u, false);
        List<Map<String, Object>> accounts = repo.accounts(u, subsidiary, false);
        List<Map<String, Object>> grid = rows(req.get("details"));
        List<Map<String, Object>> freightGrid = rows(req.get("freights"));
        List<Map<String, Object>> glGrid = rows(req.get("journals"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Detail Record Not Found");

        int customerId = i(req.get("SupplierCustomerId"));
        Map<String, Object> customer = customers.stream().filter(x -> i(x.get("Id")) == customerId).findFirst().orElse(null);
        if (customer == null) throw new IllegalArgumentException("Customer field is required");
        int customerGl = i(customer.get("GlAccountId"));
        int paymentTermId = i(req.get("PaymentTermId"));
        Map<String, Object> term = repo.paymentTerms(u).stream().filter(t -> i(col(t, "Id")) == paymentTermId).findFirst().orElse(null);
        if (paymentTermId == 0 || term == null) throw new IllegalArgumentException("Payment Term field is required");
        String termText = s(col(term, "TermsDescription"));
        boolean cashOrAdvance = "Cash".equals(termText) || "Advance".equals(termText);
        String dueDaysText = cashOrAdvance ? "" : s(req.get("DueDays")).trim();
        int dueDays = ci(dueDaysText);
        if (paymentTermId == 2 && dueDays == 0) throw new IllegalArgumentException("Due Days Field is Required");
        LocalDateTime docDate = dt(req.get("DocDate"));
        if (docDate == null) throw new IllegalArgumentException("Doc Date is required");
        /* DueDateCalculate :3777 / CmbPaymentTerm_TextChanged :1085. */
        LocalDateTime now = LocalDateTime.now();
        LocalDateTime dueDate = cashOrAdvance ? docDate : !dueDaysText.isEmpty() ? docDate.plusDays(dueDays) : now;

        /* ------------------------------------------------ the grid, each line against its source */
        List<Map<String, Object>> savedLines = existing == null ? List.of() : rows(existing.get("details"));
        Map<Integer, Map<String, Object>> savedById = new HashMap<>();
        for (var x : savedLines) savedById.put(i(col(x, "Id")), x);
        boolean anyGdn = grid.stream().anyMatch(x -> i(x.get("GdnId")) > 0);
        if (anyGdn && grid.stream().anyMatch(x -> i(x.get("GdnId")) == 0)) throw new IllegalArgumentException("A GDN invoice cannot carry manual lines");
        Set<Integer> gdnIds = grid.stream().map(x -> i(x.get("GdnId"))).filter(x -> x > 0).collect(Collectors.toSet());
        if (gdnIds.size() > 1) throw new IllegalArgumentException("Sorry! You can only load a single GDN at a time.");
        Map<Integer, Map<String, Object>> gdnByDetail = new HashMap<>();
        if (anyGdn) {
            int gdnId = gdnIds.iterator().next();
            boolean savedGdn = savedLines.stream().anyMatch(x -> i(col(x, "InvGdnId")) == gdnId);
            if (savedGdn) {
                for (var x : savedLines) gdnByDetail.put(i(col(x, "InvGdnDetailId")), sourceFromSaved(x));
                if (customerId != i(col(existing, "SupplierCustomerId"))) throw new IllegalArgumentException("The customer of a GDN invoice cannot change");
            } else {
                var src = gdnSource(u, gdnId);
                if (src.isEmpty()) throw new IllegalArgumentException("No data found for selected GDN");
                for (var x : src) gdnByDetail.put(i(col(x, "GdnDetailId")), sourceFromLoad(x));
                if (customerId != i(col(src.get(0), "SupplierCustomerId"))) throw new IllegalArgumentException("The customer must be the GDN's customer");
            }
        }
        List<Map<String, Object>> items = own.items(u), uoms = own.uoms(u), racks = own.racks(u), lots = own.jobLots(u), conds = own.conditions();
        Set<Integer> seenGdnDetail = new HashSet<>();
        List<Map<String, Object>> lines = new ArrayList<>();
        int rowNo = 0;
        for (var g : grid) {
            rowNo++;
            int lineId = i(g.get("Id"));
            if (lineId > 0 && (id == 0 || !savedById.containsKey(lineId))) throw new IllegalArgumentException("Detail row " + rowNo + " is not part of this invoice");
            Map<String, Object> L;
            if (anyGdn) {
                int gd = i(g.get("GdnDetailId"));
                Map<String, Object> src = gdnByDetail.get(gd);
                if (src == null || !seenGdnDetail.add(gd)) throw new IllegalArgumentException("GDN line of row " + rowNo + " not found");
                L = new LinkedHashMap<>(src);
                L.put("Id", lineId);
                if (lineId > 0 && i(col(savedById.get(lineId), "InvGdnDetailId")) != gd) throw new IllegalArgumentException("Detail row " + rowNo + " is not part of this invoice");
            } else {
                L = manualLine(g, rowNo, items, uoms, racks, lots, conds);
                L.put("Id", lineId);
                if (lineId > 0 && i(col(savedById.get(lineId), "InvGdnId")) != 0) throw new IllegalArgumentException("Detail row " + rowNo + " is not part of this invoice");
            }
            L.put("BillAmount", d(g.get("BillAmount")));
            applyTax(u, L, g, docDate, lineId > 0 ? savedById.get(lineId) : null, anyGdn, rowNo);
            lines.add(L);
        }

        /* ------------------------------------------------ header commission, freight, GL */
        int commAgent = i(req.get("CommissionAgentId"));
        if (commAgent != 0 && customers.stream().noneMatch(x -> i(x.get("Id")) == commAgent)) throw new IllegalArgumentException("Commission Agent not found");
        String commType = s(req.get("CommissionType")).trim();
        String commRateText = s(req.get("CommRate")).trim();
        String commUomText = s(req.get("CommUom")).trim();
        String commAmountText = totalCommissionAmount(lines, commType, commRateText, commUomText, s(req.get("CommAmount")).trim());
        commissionProportion(lines, commType, commRateText, commAmountText);

        String valueKey = subsidiary ? "SupplierCustomerId" : "Id";
        for (var f : freightGrid) {
            int t = i(f.get("Transporter"));
            if (t != 0 && accounts.stream().noneMatch(a -> i(a.get(valueKey)) == t)) throw new IllegalArgumentException("Freight account not found");
        }
        for (var gl : glGrid) {
            int a = i(gl.get("AccountId"));
            if (a != 0 && accounts.stream().noneMatch(x -> i(x.get(valueKey)) == a)) throw new IllegalArgumentException("Account not found");
        }
        String creditInItemSaleGl = repo.config(u, "CreditAmountInItemSaleGL").trim();
        freightProportion(lines, freightGrid);
        double billAmount = billAmount(lines, freightGrid, glGrid, subsidiary, customerId, customerGl, commAgent, commAmountText);
        billProportion(lines, creditInItemSaleGl);

        boolean hasTax = lines.stream().anyMatch(x -> i(x.get("TaxNameId")) > 0);
        int taxAccountId = i(req.get("TaxAccountId"));
        if (taxAccountId != 0 && own.taxAccounts(u).stream().noneMatch(x -> i(x.get("Id")) == taxAccountId)) throw new IllegalArgumentException("Tax Account not found");
        int taxNo = 0;
        if (hasTax) {
            taxNo = own.salesTaxNo(u);
            if (taxAccountId == 0) throw new IllegalArgumentException("Tax Account field is required");
            if (taxNo == 0) throw new IllegalArgumentException("Tax Invoice No field is required");
        }
        rowNo = 0;
        for (var gl : glGrid) {
            rowNo++;
            if ((d(gl.get("Credit")) > 0 || ci(gl.get("Debit")) > 0) && i(gl.get("AccountId")) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
        }
        for (var f : freightGrid) if (d(f.get("Freight")) > 0 && i(f.get("Transporter")) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
        if (d(commAmountText) > 0 && commAgent == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");

        /* ------------------------------------------------ the model */
        Head h = new Head();
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.BranchesId = n(u.getBranchesId());
        h.ProjectsId = 0;
        h.FinancialYearId = year;
        h.DocumentTypeId = DOC;
        h.BaseDocumentTypeId = 0;
        h.ScreenName = SCREEN;
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.Id = id;
        h.DocNo = id > 0 ? i(col(existing, "DocNo")) : repo.nextDocNo(u, year, DOC);
        if (h.DocNo == 0) throw new IllegalArgumentException("Doc No field is required");
        h.DocDate = docDate;
        h.SupplierCustomerId = customerId;
        h.ManualBillNo = s(req.get("ManualBillNo")).trim();
        h.SalesTaxNo = hasTax ? taxNo : 0;
        h.IsTaxable = hasTax;
        h.ReferencePartyId = taxAccountId;
        h.TaxAccountId = taxAccountId;
        h.SupplierInvoiceDate = dueDate;
        h.PaymentTermId = paymentTermId;
        h.DueDays = dueDays;
        h.DueDate = dueDate;
        h.SupplierInvoiceNo = dueDays;
        h.RemarksHeader = s(req.get("RemarksHeader")).trim();
        h.CommissionAgentId = commAgent;
        h.CommissionType = commType;
        h.UomScheduleIdCmRate = commUomText;
        h.CommRate = d(commRateText);
        h.CommAmount = d(commAmountText);
        h.CommissionRemarks = s(req.get("CommissionRemarks")).trim();
        h.BillAmount = billAmount;
        h.AttachmentsValues = id > 0 ? nullable(col(existing, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = id > 0 ? nullable(col(existing, "CustomAttachmentsValues")) : "";

        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        int line = 1, idx = 0;
        for (var L : lines) {
            Detail v = new Detail();
            v.LineId = line;
            v.Id = id != 0 ? i(L.get("Id")) : 0;
            v.InvGdnId = i(L.get("GdnId"));
            v.InvGdnDetailId = i(L.get("GdnDetailId"));
            v.SaleOrderId = i(L.get("SoId"));
            v.SaleOrderDetailId = i(L.get("SoDetailId"));
            v.ItemId = i(L.get("ItemId"));
            v.ItemCode = s(L.get("ItemCode"));
            v.ItemName = s(L.get("Item"));
            v.WarehouseId = i(L.get("WarehouseId"));
            v.RackId = i(L.get("RackId"));
            v.ItemConditionId = i(L.get("ItemConditionId"));
            v.JobLotId = i(L.get("JobLotId"));
            v.ItemUOMId = i(L.get("PackUOMId"));
            v.ItemQty = d(L.get("ItemQty"));
            v.SecondaryUomId = i(L.get("SecondaryUomId"));
            v.SecondaryUomQty = dec(L.get("SecondaryUomQty"));
            v.AdLsWeight = d(L.get("AddLessWeight"));
            v.NetBillWeight = d(L.get("NetBillWeight"));
            v.PerItemSecondaryUomQty = d(L.get("PerItemWeight"));
            v.GrossWeight = v.NetBillWeight;
            v.NetStockWeight = v.NetBillWeight;
            v.SecondaryUomItemRate = dec(L.get("SecondaryUomItemRate"));
            v.ItemRate = d(L.get("ItemRate"));
            v.UomScheduleIdRate = i(L.get("RateUOMId"));
            v.ItemAmount = d(L.get("ItemAmount"));
            v.TaxNameId = i(L.get("TaxNameId"));
            v.TaxDescriptions = s(L.get("TaxName"));
            v.TaxPercent = d(L.get("TaxPercent"));
            v.TaxAmount = d(L.get("TaxAmount"));
            v.GpDate = L.get("GpDate") instanceof LocalDateTime ? (LocalDateTime) L.get("GpDate") : LocalDateTime.of(1900, 1, 1, 0, 0);
            v.GpNo = i(L.get("GpNo"));
            v.VehicleNo = s(L.get("VehicleNo"));
            v.BillAmount = d(L.get("BillAmount"));
            v.FreightAmount = d(L.get("Freights"));
            v.CommissionAmount = d(L.get("Commission"));
            required(v.ItemId, "Item", idx); required(v.WarehouseId, "WareHouse", idx); required(v.RackId, "Rack Name", idx);
            required(v.ItemConditionId, "Item Condition", idx); required(v.JobLotId, "JobLot", idx); required(v.ItemUOMId, "Pack Uom", idx);
            required(v.ItemQty, "Qty", idx); required(v.SecondaryUomId, "Secondary Uom", idx); required(v.SecondaryUomQty.doubleValue(), "SecondaryUom Qty", idx);
            required(v.NetBillWeight, "Net bill Weight", idx); required(v.SecondaryUomItemRate.doubleValue(), "SecondaryUom ItemRate", idx);
            required(v.ItemRate, "Item Rate", idx); required(v.UomScheduleIdRate, "Rate Uom", idx); required(v.ItemAmount, "Amount", idx);
            if (v.TaxNameId > 0 || v.TaxPercent > 0 || v.TaxAmount > 0) {
                required(v.TaxNameId, "Tax Name", idx); required(v.TaxPercent, "Tax Percent", idx); required(v.TaxAmount, "Tax Amount", idx);
            }
            if (v.BillAmount <= 0) throw new IllegalArgumentException("Item Net Amount can't be less than equal to zero");
            inv.details.add(v);
            line++; idx++;
        }
        for (var f : freightGrid) {
            int t = i(f.get("Transporter"));
            if (t == 0) continue;
            Freight pf = new Freight();
            pf.TansporterId = glOf(accounts, subsidiary, t);
            pf.TransporterSupCustId = subsidiary ? t : 0;
            if (pf.TansporterId == 0) throw new IllegalArgumentException("Transporter Gl Account not found!");
            pf.FreightAmount = toInt(d(f.get("Freight")));
            inv.freights.add(pf);
        }
        for (var gl : glGrid) {
            int a = i(gl.get("AccountId"));
            if (a == 0) continue;
            Journal j = new Journal();
            j.TransporterSupCustId = subsidiary ? a : 0;
            j.ChartofAccountId = glOf(accounts, subsidiary, a);
            if (j.ChartofAccountId == 0) throw new IllegalArgumentException("Gl Account not found!");
            j.JvRemarks = s(gl.get("Remarks"));
            j.JvPrcnt = d(gl.get("Percentage"));
            j.JvQty = d(gl.get("Qty"));
            j.JvRate = d(gl.get("Rate"));
            j.JvDebit = d(gl.get("Debit"));
            j.JvCredit = d(gl.get("Credit"));
            inv.journals.add(j);
        }
        if (commAgent > 0 && d(commAmountText) > 0) {
            Commission c = new Commission();
            c.Id = 0; c.SaleOrderId = 0; c.CommDebitSupCustId = 0;
            c.CommissionAgentId = commAgent;
            if ("Percent".equals(commType)) c.CommType = 2.0;
            if ("Flat".equals(commType)) c.CommType = 1.0;
            if ("Comm Weight".equals(commType)) c.CommType = 3.0;
            c.Rate = d(commRateText);
            c.RateUom = d(commUomText);
            c.CommAmount = d(commAmountText);
            c.Remarks = s(req.get("CommissionRemarks"));
            inv.commissions.add(c);
        }

        /* BLL 0580 Save. */
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(h.OrganizationId, h.CompanyId, DOC, saved));
        out.put("message", (id == 0 ? "Record Saved Successfully [" : "Record Update Successfully [") + h.DocNo + "] ");
        return out;
    }

    // ================================================================================= lines

    /** FillDetailRow :1244 for a line typed on the entry bar; the amount is CalculateAmount :2823. */
    private Map<String, Object> manualLine(Map<String, Object> g, int rowNo, List<Map<String, Object>> items, List<Map<String, Object>> uoms,
                                           List<Map<String, Object>> racks, List<Map<String, Object>> lots, List<Map<String, Object>> conds) {
        int itemId = i(g.get("ItemId"));
        Map<String, Object> item = items.stream().filter(x -> i(x.get("Id")) == itemId).findFirst().orElse(null);
        if (item == null) throw new IllegalArgumentException("Item is required in Detail Grid at row No: " + rowNo);
        Map<String, Object> L = new LinkedHashMap<>();
        L.put("GdnId", 0); L.put("GdnDetailId", 0); L.put("SoId", 0); L.put("SoDetailId", 0);
        L.put("ItemId", itemId); L.put("ItemCode", item.get("ItemCode")); L.put("Item", item.get("ItemName"));
        int wh = i(g.get("WarehouseId")), rack = i(g.get("RackId"));
        if (wh != 0 && racks.stream().noneMatch(x -> i(x.get("ItemId")) == itemId && i(x.get("WarehouseId")) == wh))
            throw new IllegalArgumentException("Warehouse of row " + rowNo + " is not allocated to the item");
        if (rack != 0 && racks.stream().noneMatch(x -> i(x.get("ItemId")) == itemId && i(x.get("Id")) == rack && (wh == 0 || i(x.get("WarehouseId")) == wh)))
            throw new IllegalArgumentException("Rack of row " + rowNo + " is not allocated to the item");
        L.put("WarehouseId", wh); L.put("RackId", rack);
        int cond = i(g.get("ItemConditionId")), lot = i(g.get("JobLotId"));
        if (cond != 0 && conds.stream().noneMatch(x -> i(x.get("Id")) == cond)) throw new IllegalArgumentException("Item Condition not found");
        if (lot != 0 && lots.stream().noneMatch(x -> i(x.get("Id")) == lot)) throw new IllegalArgumentException("Job/Lot not found");
        L.put("ItemConditionId", cond); L.put("JobLotId", lot);
        List<Map<String, Object>> iu = uoms.stream().filter(x -> i(x.get("ItemId")) == itemId).collect(Collectors.toList());
        int pack = i(g.get("PackUOMId")), rateUom = i(g.get("RateUOMId")), sec = i(g.get("SecondaryUomId"));
        for (int x : new int[]{pack, rateUom, sec})
            if (x != 0 && iu.stream().noneMatch(y -> i(y.get("Id")) == x)) throw new IllegalArgumentException("Uom of row " + rowNo + " is not the item's");
        L.put("PackUOMId", pack); L.put("RateUOMId", rateUom); L.put("SecondaryUomId", sec);
        double qty = d(g.get("ItemQty")), rate = d(g.get("ItemRate"));
        double eq = iu.stream().filter(y -> i(y.get("Id")) == rateUom).map(y -> d(y.get("Equivalent"))).findFirst().orElse(0d);
        double amount = qty > 0 && eq > 0 && rate > 0 ? round(qty / eq * rate, 3) : 0;
        L.put("ItemQty", qty); L.put("ItemRate", rate); L.put("ItemAmount", amount);
        L.put("SecondaryUomQty", d(g.get("SecondaryUomQty"))); L.put("AddLessWeight", d(g.get("AddLessWeight")));
        L.put("NetBillWeight", d(g.get("NetBillWeight"))); L.put("PerItemWeight", d(g.get("PerItemWeight")));
        L.put("SecondaryUomItemRate", d(g.get("SecondaryUomItemRate")));
        LocalDateTime gp = dt(g.get("GpDate"));
        L.put("GpDate", gp == null ? LocalDateTime.of(1900, 1, 1, 0, 0) : gp.toLocalDate().atStartOfDay());
        L.put("GpNo", ci(g.get("GpNo")));
        L.put("VehicleNo", s(g.get("VehicleNo")));
        return L;
    }

    /** LoadInGridDetail :4046 - the grid row made from a usp_getGdndataForSaleInvoice_Store row. */
    private static Map<String, Object> sourceFromLoad(Map<String, Object> dr) {
        Map<String, Object> L = new LinkedHashMap<>();
        L.put("GdnId", i(col(dr, "Id"))); L.put("GdnDetailId", i(col(dr, "GdnDetailId")));
        L.put("SoId", i(col(dr, "SaleOrderId"))); L.put("SoDetailId", i(col(dr, "SaleOrderDetailId")));
        int itemId = i(col(dr, "ItemId"));
        L.put("ItemId", itemId); L.put("ItemCode", itemId > 0 ? s(col(dr, "ItemCode")) : ""); L.put("Item", itemId > 0 ? s(col(dr, "ItemName")) : "");
        L.put("WarehouseId", i(col(dr, "WarehouseId"))); L.put("RackId", i(col(dr, "RackId")));
        L.put("ItemConditionId", i(col(dr, "ItemConditionId"))); L.put("JobLotId", i(col(dr, "JobLotId")));
        L.put("PackUOMId", i(col(dr, "ItemUomId"))); L.put("ItemQty", d(col(dr, "ItemQty")));
        L.put("SecondaryUomId", i(col(dr, "SecondaryUomId"))); L.put("SecondaryUomQty", d(col(dr, "SecondaryUomQty")));
        L.put("AddLessWeight", d(col(dr, "AdLsWeight"))); L.put("NetBillWeight", d(col(dr, "NetBillWeight")));
        L.put("PerItemWeight", d(col(dr, "PerItemNetWeight"))); L.put("SecondaryUomItemRate", d(col(dr, "SecondaryUomItemRate")));
        L.put("ItemRate", d(col(dr, "ItemRate"))); L.put("RateUOMId", i(col(dr, "ItemUomId")));
        L.put("ItemAmount", d(col(dr, "Amount")));
        L.put("TaxNameId", i(col(dr, "TaxNameId"))); L.put("TaxName", s(col(dr, "TaxName")));
        L.put("TaxPercent", d(col(dr, "TaxPercent"))); L.put("TaxAmount", d(col(dr, "TaxAmount")));
        LocalDateTime gp = dt(col(dr, "GpDate"));
        L.put("GpDate", gp == null ? LocalDateTime.of(1900, 1, 1, 0, 0) : gp);
        L.put("GpNo", i(col(dr, "GpNo"))); L.put("VehicleNo", s(col(dr, "VehicleNo")));
        return L;
    }

    /** FillDetailFromListCommonForReadById :2448 - a saved line. */
    private static Map<String, Object> sourceFromSaved(Map<String, Object> x) {
        Map<String, Object> L = new LinkedHashMap<>();
        L.put("GdnId", i(col(x, "InvGdnId"))); L.put("GdnDetailId", i(col(x, "InvGdnDetailId")));
        L.put("SoId", i(col(x, "SaleOrderId"))); L.put("SoDetailId", i(col(x, "SaleOrderDetailId")));
        L.put("ItemId", i(col(x, "ItemId"))); L.put("ItemCode", s(col(x, "ItemCode"))); L.put("Item", s(col(x, "ItemName")));
        L.put("WarehouseId", i(col(x, "WarehouseId"))); L.put("RackId", i(col(x, "RackId")));
        L.put("ItemConditionId", i(col(x, "ItemConditionId"))); L.put("JobLotId", i(col(x, "JobLotId")));
        L.put("PackUOMId", i(col(x, "ItemUOMId"))); L.put("ItemQty", d(col(x, "ItemQty")));
        L.put("SecondaryUomId", i(col(x, "SecondaryUomId"))); L.put("SecondaryUomQty", d(col(x, "SecondaryUomQty")));
        L.put("AddLessWeight", d(col(x, "AdLsWeight"))); L.put("NetBillWeight", d(col(x, "NetBillWeight")));
        L.put("PerItemWeight", d(col(x, "PerItemSecondaryUomQty"))); L.put("SecondaryUomItemRate", d(col(x, "SecondaryUomItemRate")));
        L.put("ItemRate", d(col(x, "ItemRate"))); L.put("RateUOMId", i(col(x, "UomScheduleIdRate")));
        L.put("ItemAmount", d(col(x, "ItemAmount")));
        L.put("TaxNameId", i(col(x, "TaxNameId"))); L.put("TaxName", s(col(x, "TaxDescriptions")));
        L.put("TaxPercent", d(col(x, "TaxPercent"))); L.put("TaxAmount", d(col(x, "TaxAmount")));
        LocalDateTime gp = dt(col(x, "GpDate"));
        L.put("GpDate", gp == null ? LocalDateTime.of(1900, 1, 1, 0, 0) : gp);
        L.put("GpNo", i(col(x, "GpNo"))); L.put("VehicleNo", s(col(x, "VehicleNo")));
        return L;
    }

    /**
     * The tax of a line may be: none; its source's (loaded GDN / saved line); or the item's schedule row on the Doc Date
     * (TaxTypeBind, DocDate_Leave, the F1 pop-up), whose percent FillDetailRow truncates for a line typed on the bar.
     * TaxAmount is ItemAmount x percent / 100 (Math.Round(, 3) when the percent was changed on the grid).
     */
    private void applyTax(UserAccount u, Map<String, Object> L, Map<String, Object> g, LocalDateTime docDate, Map<String, Object> saved,
                          boolean gdnLine, int rowNo) {
        int taxId = i(g.get("TaxNameId"));
        double pct = d(g.get("TaxPercent"));
        double amount = d(L.get("ItemAmount"));
        String name = "";
        if (taxId == 0) { pct = 0; }
        else {
            boolean ok = false;
            if (gdnLine && taxId == i(L.get("TaxNameId")) && pct == d(L.get("TaxPercent"))) { ok = true; name = s(L.get("TaxName")); }
            if (!ok && saved != null && taxId == i(col(saved, "TaxNameId")) && pct == d(col(saved, "TaxPercent"))) { ok = true; name = s(col(saved, "TaxDescriptions")); }
            if (!ok) {
                var sch = own.taxForItem(u, i(L.get("ItemId")), docDate.toLocalDate().atStartOfDay());
                if (!sch.isEmpty() && i(col(sch.get(0), "TaxNameId")) == taxId) {
                    double sp = d(col(sch.get(0), "TaxPercent"));
                    if (pct == sp || pct == truncatedPercent(sp)) { ok = true; name = s(col(sch.get(0), "TaxName")); }
                }
            }
            if (!ok) throw new IllegalArgumentException("Tax of row " + rowNo + " is not the item's tax schedule");
        }
        double tax = amount * pct / 100.0;
        double sent = d(g.get("TaxAmount"));
        if (gdnLine && taxId == i(L.get("TaxNameId")) && pct == d(L.get("TaxPercent"))) tax = d(L.get("TaxAmount"));
        else if (Math.abs(sent - round(tax, 3)) < 1e-9) tax = round(tax, 3);
        L.put("TaxNameId", taxId); L.put("TaxName", taxId > 0 ? name : ""); L.put("TaxPercent", pct); L.put("TaxAmount", tax);
    }

    /** Conversion.ToInt(taxPercent.ToString("#,##.###")) - Convert.ToInt32 of the text: 0 when it is not a whole number or has a group separator. */
    static double truncatedPercent(double p) {
        if (p != Math.rint(p) || Math.abs(p) >= 1000) return 0;
        return p;
    }

    // ================================================================================= proportions (:2650-3130)

    /** TotalCommissionAmount :3093. */
    static String totalCommissionAmount(List<Map<String, Object>> lines, String type, String rateText, String uomText, String current) {
        if (rateText.isEmpty()) return "0";
        double rate = d(rateText);
        if ("Flat".equals(type)) return g(rate);
        if ("Percent".equals(type) || "Percentage".equals(type)) return g(round(sum(lines, "ItemAmount") * rate / 100.0, 3));
        if ("Comm Weight".equals(type)) {
            double uom = d(uomText);
            return g(round(uom > 0 ? sum(lines, "NetBillWeight") / uom * rate : 0, 3));
        }
        return current;
    }

    /** CommissionProportion :2791. */
    static void commissionProportion(List<Map<String, Object>> lines, String type, String rateText, String amountText) {
        double total = d(amountText), pct = d(rateText), net = sum(lines, "NetBillWeight");
        for (var r : lines) {
            if ("Percent".equals(type)) r.put("Commission", d(r.get("ItemAmount")) * pct / 100.0);
            else r.put("Commission", total / net * d(r.get("NetBillWeight")));
        }
    }

    /** FreightProportion :2729. */
    static void freightProportion(List<Map<String, Object>> lines, List<Map<String, Object>> freights) {
        double net = sum(lines, "NetBillWeight"), credit = sum(freights, "Freight");
        for (var r : lines) r.put("Freights", credit > 0 ? Math.rint(credit) / net * d(r.get("NetBillWeight")) : 0d);
    }

    /** BillProportion :2762. */
    static void billProportion(List<Map<String, Object>> lines, String creditAmountInItemSaleGl) {
        for (var r : lines) {
            double v;
            if (!creditAmountInItemSaleGl.isEmpty() && !Boolean.parseBoolean(creditAmountInItemSaleGl))
                v = d(r.get("ItemAmount")) - d(r.get("Commission")) + d(r.get("TaxAmount"));
            else if (!creditAmountInItemSaleGl.isEmpty()) v = d(r.get("BillAmount"));      // ConfigKey true: the cell is left as it was
            else v = d(r.get("ItemAmount")) - d(r.get("Freights")) - d(r.get("Commission")) + d(r.get("TaxAmount"));
            r.put("BillAmount", v);
        }
    }

    /** BillAmount :2650 - Math.Round (to even) of item + tax - freight credited to the customer + GL (credit - debit) - own commission. */
    static double billAmount(List<Map<String, Object>> lines, List<Map<String, Object>> freights, List<Map<String, Object>> gl, boolean subsidiary,
                             int customerId, int customerGl, int commAgent, String commAmountText) {
        double jd = 0, jc = 0, tc = 0;
        int rowNo = 0;
        for (var r : gl) {
            rowNo++;
            int a = i(r.get("AccountId"));
            if (a <= 0) continue;
            if (a == (subsidiary ? customerId : customerGl))
                throw new IllegalArgumentException("You cannot select Customer Account in Customer Add Less Grid at row#" + rowNo);
            jd += d(r.get("Debit")); jc += d(r.get("Credit"));
        }
        for (var f : freights) {
            int t = i(f.get("Transporter"));
            if (t <= 0) continue;
            if (t == (subsidiary ? customerId : customerGl)) tc += d(f.get("Freight"));
        }
        double bill = sum(lines, "ItemAmount") + sum(lines, "TaxAmount");
        if (tc > 0) bill -= Math.abs(tc);
        double diff = jc - jd;
        bill = diff < 0 ? bill - Math.abs(diff) : bill + diff;
        if (customerId == commAgent) bill -= d(commAmountText);
        return Math.rint(bill);
    }

    // ================================================================================= helpers

    private static void required(double v, String name, int rowIndex) {
        if (v <= 0) throw new IllegalArgumentException(name + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }
    private static void required(int v, String name, int rowIndex) {
        if (v == 0) throw new IllegalArgumentException(name + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }
    private static int glOf(List<Map<String, Object>> accounts, boolean subsidiary, int value) {
        String key = subsidiary ? "SupplierCustomerId" : "Id";
        return accounts.stream().filter(a -> i(a.get(key)) == value).map(a -> i(a.get("Id"))).findFirst().orElse(0);
    }
    /** Convert.ToInt32(double) - to even. */
    private static int toInt(double v) { return (int) Math.rint(v); }
    private static double sum(List<Map<String, Object>> rows, String key) { double t = 0; for (var r : rows) t += d(r.get(key)); return t; }
    private static double round(double v, int n) { return BigDecimal.valueOf(v).setScale(n, RoundingMode.HALF_EVEN).doubleValue(); }
    /** double.ToString() of a whole or fractional value. */
    private static String g(double v) { return v == Math.rint(v) && Math.abs(v) < 1e15 ? String.valueOf((long) v) : BigDecimal.valueOf(v).stripTrailingZeros().toPlainString(); }
    /** Conversion.ToInt of text: Convert.ToInt32(string) - 0 unless the text is a whole number. */
    private static int ci(Object o) {
        if (o instanceof Number) return (int) Math.rint(((Number) o).doubleValue());
        String t = s(o).trim();
        try { return Integer.parseInt(t); } catch (NumberFormatException e) { return 0; }
    }
    private static Map<String, Object> two(int id, String description) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", id); m.put("Description", description); return m;
    }
    private static LocalDateTime day(String v) {
        LocalDateTime x = dt(v);
        return (x == null ? LocalDate.now().atStartOfDay() : x.toLocalDate().atStartOfDay());
    }
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rows(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object x : (List<Object>) o) if (x instanceof Map) out.add(new LinkedHashMap<>((Map<String, Object>) x));
        return out;
    }
    private static String nullable(Object o) { return o == null ? null : String.valueOf(o); }
}
