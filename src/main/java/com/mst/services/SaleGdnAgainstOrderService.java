package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleGdnAgainstOrderRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.util.*;

/**
 * Screen 142 - Architecture.WinApp.Sale.frmGdnAgainstSaleOrder (DocumentTypeId 170).
 *   Load :341  FormValidation :967  FormDetailValidation :1004  Insert :1262  ReadById :1496  btnDelete :1544
 *   HistoryGridFill :1818  AvailableStock :2370  btnLoadSaleOrder :2555  LoadInGridDetail :2576
 * Save = BLL InvGdn.Save -> DAL 0428 SetData (the generic GDN chain in SaleGdnPurchaseReturnRepository), one transaction.
 * The messages and their order are the form's: Grid Record Not Found; the four header checks; the freight and transporter checks;
 * then the per-row checks. DocNo on insert is the generator value (web deviation: two users cannot collide on a typed number).
 */
@Service
public class SaleGdnAgainstOrderService {
    private final SaleGdnAgainstOrderRepository repo;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final SaleInvoiceRepository si;

    public SaleGdnAgainstOrderService(SaleGdnAgainstOrderRepository repo, CurrentUserContext ctx, StoreScreenRights rights, SaleInvoiceRepository si) {
        this.repo = repo; this.ctx = ctx; this.rights = rights; this.si = si;
    }

    private UserAccount user() { return ctx.requireAccountingUser(); }
    private boolean viewAll(UserAccount u) { return repo.canViewAllRecords(u, ctx.currentRoleName()); }

    public Map<String, Object> lookups() {
        UserAccount u = user();
        Map<String, Object> m = repo.initial(u, ctx.currentFinancialYearId());
        m.put("rights", rights.of(SaleGdnAgainstOrderRepository.SCREEN));
        m.put("historyCustomers", repo.historyCustomers(u));
        m.put("today", LocalDate.now().toString());
        java.time.LocalDateTime start = si.financialYearStart(u, ctx.currentFinancialYearId());   /* clsGlobalVariables.ActiveYr.Start_Period */
        m.put("yearStart", start == null ? null : start.toLocalDate().toString());
        return m;
    }
    public Object nextNo() { return repo.nextNo(user(), ctx.currentFinancialYearId()); }
    public List<Map<String, Object>> uoms(int itemId) {
        if (itemId <= 0) throw new IllegalArgumentException("Item Field is Required");
        return repo.uoms(user(), itemId);
    }
    public Map<String, Object> stock(int itemId, String cropYear, String docDate, int jobLotId, int warehouseId) {
        Map<String, Object> m = new LinkedHashMap<>();
        double v = repo.stock(user(), itemId, cropYear, docDate == null || docDate.isBlank() ? LocalDate.now().toString() : docDate, jobLotId, warehouseId);
        m.put("stock", v > 0 ? v : 0d);
        return m;
    }
    public Map<String, Object> record(int id) { return repo.record(user(), id); }
    public List<Map<String, Object>> history(String mode, String from, String to, int fromNo, int toNo, int customerId) {
        UserAccount u = user();
        return repo.historyFiltered(u, ctx.currentFinancialYearId(), viewAll(u), mode, from, to, fromNo, toNo, customerId);
    }

    // ---------------------------------------------------------------- frmLoadSaleOrder
    public Map<String, Object> loaderSetup() {
        UserAccount u = user();
        boolean bw = "true".equalsIgnoreCase(repo.configValue(u, "SaleOrderBranchWise")) || "1".equals(repo.configValue(u, "SaleOrderBranchWise"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branchWise", bw);
        m.put("branches", repo.loaderBranches(u, bw));
        m.put("userBranchId", u.getBranchesId());
        return m;
    }
    public Map<String, Object> loaderCombos(String branchIds) {
        UserAccount u = user();
        boolean bw = "true".equalsIgnoreCase(repo.configValue(u, "SaleOrderBranchWise")) || "1".equals(repo.configValue(u, "SaleOrderBranchWise"));
        return repo.loaderCombos(u, branchIds, bw);
    }
    public List<Map<String, Object>> loaderSearch(String branchIds, int customerId, int itemId, String from, String to) {
        if (branchIds == null || branchIds.isBlank()) throw new IllegalArgumentException("Select branch first");
        return repo.loaderPending(user(), branchIds, customerId, itemId, from, to);
    }
    /** btnLoadOnInvoice_Click_1 (rows of one order only) + LoadInGridDetail. */
    public List<Map<String, Object>> loaderLoad(List<Number> orderIds) {
        if (orderIds == null || orderIds.isEmpty()) throw new IllegalArgumentException("No Row is Selected");
        int first = 0;
        for (Number n : orderIds) {
            int id = n == null ? 0 : n.intValue();
            if (first == 0) first = id;
            if (first != id) throw new IllegalArgumentException("Sorry! Select Same OrderNo Rows");
        }
        if (first == 0) return List.of();
        return repo.orderForGdn(user(), first);
    }

    // ---------------------------------------------------------------- save / delete
    @Transactional
    public Map<String, Object> save(Map<String, Object> r) {
        UserAccount u = user();
        validate(u, r);
        int id = repo.save(u, ctx.currentFinancialYearId(), r);
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> rec = repo.record(u, id);
        out.put("id", id);
        out.put("docNo", rec.get("DocNo"));
        out.put("record", rec);
        out.put("message", (num(r.get("id")) > 0 ? "Record Update Successfully " : "Record Save Successfully ") + rec.get("DocNo"));
        return out;
    }
    @Transactional
    public void delete(int id) {
        if (id <= 0) throw new IllegalArgumentException("Record not found...");
        repo.remove(user(), id);
    }

    @SuppressWarnings("unchecked")
    private void validate(UserAccount u, Map<String, Object> r) {
        List<Map<String, Object>> rows = (List<Map<String, Object>>) r.getOrDefault("details", List.of());
        List<Map<String, Object>> active = new ArrayList<>();
        for (Map<String, Object> x : rows) if (num(x.get("ActionTypeId")) != 3) active.add(x);
        if (active.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        boolean insert = num(r.get("id")) == 0;
        if (insert) r.put("DocNo", num(repo.nextNo(u, ctx.currentFinancialYearId())));
        if (num(r.get("DocNo")) == 0) throw new IllegalArgumentException("DocNo Field is Required");
        if (num(r.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer Field is Required");
        if (text(r.get("DeliveryTerm")).isEmpty()) throw new IllegalArgumentException("Delivery Term Field is Required");
        if (text(r.get("GpNo")).isEmpty() || num(r.get("GpNo")) == 0) throw new IllegalArgumentException("GPNo Field is Required");
        double freight = dec(r.get("CarriageAmount"));
        int transporter = num(r.get("TransporterId"));
        if (freight > 0) {
            if (transporter == 0) throw new IllegalArgumentException("Transporter Account field required");
        } else {
            r.put("CarriageAmount", 0d);
            if (transporter > 0) throw new IllegalArgumentException("Frieght field required");
        }
        for (Map<String, Object> x : active) {
            if (num(x.get("WarehouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Required");
            if (num(x.get("ItemId")) == 0) throw new IllegalArgumentException("ItemName Field Required");
            if (text(x.get("CropYear")).isEmpty()) throw new IllegalArgumentException("CropYear Field Required");
            /* the desktop reads a "JobId" cell that does not exist; the intent (the row's JobLotId) is checked */
            if (num(x.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot Field Required");
            if (num(x.get("PackingTypeId")) == 0) throw new IllegalArgumentException("Packing Type Field Required");
            if (num(x.get("ItemUomId")) == 0) throw new IllegalArgumentException("PackUom Field Required");
            if (dec(x.get("ItemQty")) == 0) throw new IllegalArgumentException("Item Qty Field Required");
            if (dec(x.get("GrossWeight")) == 0) throw new IllegalArgumentException("GrossWight Field Required");
            if (dec(x.get("NetBillWeight")) == 0) throw new IllegalArgumentException("NetWeight Field Required");
            if (dec(x.get("StockWeight")) == 0) throw new IllegalArgumentException("StockWeight Field Required");
            if (text(x.get("AreaCity")).isEmpty() || num(x.get("CityId")) == 0) throw new IllegalArgumentException("City Field Required");
        }
    }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }
    private static double dec(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (Exception e) { return 0; }
    }
    private static String text(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
}
