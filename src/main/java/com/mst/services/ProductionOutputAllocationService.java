package com.mst.services;

import com.mst.repositories.ProductionOutputAllocationRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpSession;
import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ProductionOutputAllocationRepository.col;
import static com.mst.repositories.ProductionOutputAllocationRepository.toInt;

/**
 * ProductionOutputAllocationWithExportInvoice.cs. The page keeps the grid (as the desktop grid
 * does its own arithmetic); every database step, and every refusal Insert() makes before its
 * write, runs here again in the desktop's order and wording. Tenancy, branch, financial year and
 * user always come from {@link CurrentUserContext}.
 *
 * Two places where the procedures carry no tenancy are fenced in here, without changing a call:
 *  - ReadById (GetByID sends only @JobOrderId): the rows returned are kept to this company's.
 *  - the InsertAndUpdate procedure's UPDATE / "delete" (ActionId 2 / 3) are `WHERE Id = @Id`:
 *    an Id is accepted only when this session's ReadById of that job order returned it - the
 *    only way the desktop form can ever hold one.
 */
@Service
public class ProductionOutputAllocationService {

    private static final String SESSION_KEY = "poa.readById.ids";

    @Autowired private ProductionOutputAllocationRepository repo;
    @Autowired private CurrentUserContext ctx;

    /* ======================================================================== InitializeComponentMethod */

    /** InitializeComponentMethod:158 - rights, job orders, history combo, export invoices. */
    public Map<String, Object> load() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenName", ProductionOutputAllocationRepository.DESKTOP_SCREEN_NAME);
        out.put("rights", rights());
        out.put("jobOrders", jobOrders());
        out.put("historyJobOrders", historyJobOrders());
        out.put("invoices", invoices());
        return out;
    }

    /**
     * CommonServices.SetRightsValueInRightsObject(ScreenName) (CommonServices.cs:14738), the members
     * this form reads. Only the role spelled exactly "Admin" is special; FieldChooser, SaveLayout,
     * GroupCollapse and GroupExpand start true for everyone; Grid Print / Grid Export come only
     * from grant rows.
     */
    public Map<String, Boolean> rights() {
        Map<String, Boolean> r = new LinkedHashMap<>();
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        r.put("save", admin);
        r.put("update", admin);
        r.put("canViewAllRecord", admin);
        r.put("fieldChooser", true);
        r.put("saveLayout", true);
        r.put("gridPrint", false);
        r.put("gridExport", false);
        r.put("groupCollapse", true);
        r.put("groupExpand", true);
        try {
            for (Map<String, Object> row : repo.userRightsForScreen(ctx.currentUserId(), role, ctx.currentCompanyId())) {
                String name = str(col(row, "RightName"));
                boolean v = toBool(col(row, "Value"));
                switch (name) {
                    case "Save":              r.put("save", admin || v); break;
                    case "Update":            r.put("update", admin || v); break;
                    case "CanView AllRecord": r.put("canViewAllRecord", admin || v); break;
                    case "Grid Print":        r.put("gridPrint", v); break;
                    case "Grid Export":       r.put("gridExport", v); break;
                    default: break;
                }
            }
        } catch (Exception ignored) { /* defaults stay */ }
        return r;
    }

    /** GetJobOrderNoData:206 - GetJobOrderAll(FinancialYearId = ActiveYr.Id, ActionId = 1). */
    public List<Map<String, Object>> jobOrders() {
        return repo.jobOrdersAll(ctx.currentOrganizationId(), ctx.currentCompanyId(), ctx.currentFinancialYearId(), 1);
    }

    /** bindHistoryCombo:258 - rows whose Activity == "JobOrder"; Id / ReferenceName (Activity hidden). */
    public List<Map<String, Object>> historyJobOrders() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDown(ctx.currentOrganizationId(), ctx.currentCompanyId())) {
            if (!"JobOrder".equals(str(col(r, "Activity")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("ReferenceName", col(r, "ReferenceName"));
            m.put("Activity", col(r, "Activity"));
            out.add(m);
        }
        return out;
    }

    /** CommonServices.GetExportInvoiceNo() - dtInvoice, bound as the grid's value list (Id / InvoiceNo). */
    public List<Map<String, Object>> invoices() {
        return repo.exportInvoiceNos(ctx.currentOrganizationId(), ctx.currentCompanyId());
    }

    /** GetDataAgainstJobOrderFromProduction:638 - ItemId, ItemName, Qty, Weight per output item. */
    public List<Map<String, Object>> outputItems(int jobOrderId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.outputItems(ctx.currentOrganizationId(), ctx.currentCompanyId(), jobOrderId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", col(r, "ItemId"));
            m.put("ItemName", col(r, "ItemName"));
            m.put("Qty", col(r, "Qty"));
            m.put("Weight", col(r, "Weight"));
            out.add(m);
        }
        return out;
    }

    /* ================================================================================== ReadById */

    /** ReadById:948 - GetByID(JobOrderId); the page fills dtdetail from the list. */
    public List<Map<String, Object>> readById(int jobOrderId, HttpSession session) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> ids = new HashSet<>();
        for (Map<String, Object> r : repo.readById(jobOrderId)) {
            if (toInt(col(r, "OrganizationId")) != org || toInt(col(r, "CompanyId")) != comp) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("InvProductionJobOrderId", col(r, "InvProductionJobOrderId"));
            m.put("ItemId", col(r, "ItemId"));
            m.put("ItemName", col(r, "ItemName"));
            m.put("OutPutQty", col(r, "OutPutQty"));
            m.put("OutPutWeight", col(r, "OutPutWeight"));
            m.put("Qty", col(r, "Qty"));
            m.put("NetWeight", col(r, "NetWeight"));
            m.put("ExImInvoiceId", col(r, "ExImInvoiceId"));
            out.add(m);
            ids.add(toInt(col(r, "Id")));
        }
        if (session != null) {
            @SuppressWarnings("unchecked")
            Map<String, Set<Integer>> all = (Map<String, Set<Integer>>) session.getAttribute(SESSION_KEY);
            if (all == null) all = new HashMap<>();
            all.put(org + ":" + comp + ":" + jobOrderId, ids);
            session.setAttribute(SESSION_KEY, all);
        }
        return out;
    }

    /* ==================================================================================== Insert */

    /**
     * Insert():304, rule for rule, over the rows the grid shows (grd.GetRows(), in display order).
     *
     * @param recId    RecId: 0 from btnsave_Click (and Ctrl+S, even in edit mode), the read job order's
     *                 id after ReadById for btnUpdate_Click / Ctrl+U.
     * @param rows     the grid rows (Id, ItemId, ItemName, OutPutQty, OutPutWeight, Qty, NetWeightKg,
     *                 ExportInvoiceId), dtdetail order.
     * @param removed  lstRemoveRecord - rows with Id > 0 deleted with the "X" button.
     * @return true when RecId > 0 (the update message).
     */
    public boolean save(int recId, int jobOrderId, List<Map<String, Object>> rows,
                        List<Map<String, Object>> removed, HttpSession session) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        int branch = ctx.currentBranchId();
        int fy = ctx.currentFinancialYearId();
        int user = ctx.currentUserId();

        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        /* FormValidation:528 - CmbJobOrder.ActiveRow == null: the value must be a row of its list. */
        boolean inList = false;
        for (Map<String, Object> j : jobOrders()) if (toInt(col(j, "Id")) == jobOrderId) { inList = true; break; }
        if (jobOrderId == 0 || !inList) throw new IllegalArgumentException("Job Order Field Required");

        Set<Integer> invoiceIds = new HashSet<>();
        for (Map<String, Object> i : invoices()) invoiceIds.add(toInt(col(i, "Id")));
        Set<Integer> readIds = readIds(session, org, comp, jobOrderId);

        List<LinkedHashMap<String, Object>> items = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {
            Map<String, Object> item = rows.get(idx);
            int itemId = toInt(item.get("ItemId"));
            double sumOfQty = 0, sumOfWeight = 0;       /* dtdetail.Compute("Sum(..)", "ItemId = 'x'") */
            for (Map<String, Object> o : rows) {
                if (toInt(o.get("ItemId")) == itemId) {
                    sumOfQty += dbl(o.get("Qty"));
                    sumOfWeight += dbl(o.get("NetWeightKg"));
                }
            }
            double outputQty = dbl(item.get("OutPutQty"));
            double outputWeight = dbl(item.get("OutPutWeight"));
            String itemName = str(item.get("ItemName"));
            if (sumOfQty > outputQty) {
                throw new IllegalArgumentException("Total Qty of Item:" + itemName
                        + " can't be greater than output qty which is " + net(outputQty));
            }
            if (sumOfWeight > outputWeight) {
                throw new IllegalArgumentException("Total Weigh of Item:" + itemName
                        + " can't be greater than output weight which is " + net(outputWeight));
            }
            int actionId = recId == 0 ? 1 : 2;
            int id = toInt(item.get("Id"));
            if (actionId == 2 && id > 0 && !readIds.contains(id)) throw new IllegalArgumentException("Record not found.");
            LinkedHashMap<String, Object> d = model(actionId, org, comp, branch, fy, user, jobOrderId);
            d.put("@Id", id);
            d.put("@ItemId", itemId);
            int invoiceId = toInt(item.get("ExportInvoiceId"));
            if (!invoiceIds.contains(invoiceId)) invoiceId = 0;     /* LimitToList: only a listed invoice */
            d.put("@ExImInvoiceId", invoiceId);
            if (invoiceId == 0) throw new IllegalArgumentException("Invoice No found in row#" + (idx + 1));
            double qty = dbl(item.get("Qty"));
            d.put("@Qty", qty);
            if (qty == 0.0) throw new IllegalArgumentException("Qty should be greater than 0 in row#" + (idx + 1));
            double netWeight = dbl(item.get("NetWeightKg"));
            d.put("@NetWeight", netWeight);
            if (netWeight == 0.0) throw new IllegalArgumentException("Net weight should be greater than 0 in row#" + (idx + 1));
            items.add(d);
        }
        if (removed != null) {
            for (Map<String, Object> r : removed) {
                int id = toInt(r.get("Id"));
                if (id <= 0 || !readIds.contains(id)) throw new IllegalArgumentException("Record not found.");
                /* DeleteRow:727 - ActionId 3 and the row's values at the time of the X click. */
                LinkedHashMap<String, Object> d = model(3, org, comp, branch, fy, user, jobOrderId);
                d.put("@Id", id);
                d.put("@ItemId", toInt(r.get("ItemId")));
                d.put("@Qty", dbl(r.get("Qty")));
                d.put("@NetWeight", dbl(r.get("NetWeightKg")));
                d.put("@ExImInvoiceId", toInt(r.get("ExportInvoiceId")));
                items.add(d);
            }
        }
        repo.save(items);
        return recId > 0;
    }

    /** One list item: every model property the procedure declares, CLR defaults where unset. */
    private static LinkedHashMap<String, Object> model(int actionId, int org, int comp, int branch, int fy,
                                                       int user, int jobOrderId) {
        LinkedHashMap<String, Object> d = new LinkedHashMap<>();
        d.put("@Id", 0);
        d.put("@InvProductionJobOrderId", jobOrderId);
        d.put("@ItemId", 0);
        d.put("@Qty", 0d);
        d.put("@NetWeight", 0d);
        d.put("@ExImInvoiceId", 0);
        d.put("@OrganizationId", org);
        d.put("@CompanyId", comp);
        d.put("@FinancialYearId", fy);
        d.put("@BranchesId", branch);
        d.put("@ActionId", actionId);
        d.put("@EntryDate", null);          /* set to DateTime.Now by the DAL loop */
        d.put("@EntryUserId", user);
        d.put("@ModifyDate", null);
        d.put("@ModifyUserId", user);
        return d;
    }

    private Set<Integer> readIds(HttpSession session, int org, int comp, int jobOrderId) {
        if (session == null) return new HashSet<>();
        @SuppressWarnings("unchecked")
        Map<String, Set<Integer>> all = (Map<String, Set<Integer>>) session.getAttribute(SESSION_KEY);
        if (all == null) return new HashSet<>();
        Set<Integer> s = all.get(org + ":" + comp + ":" + jobOrderId);
        return s == null ? new HashSet<>() : s;
    }

    /* ================================================================================ History */

    /**
     * GetHistoryData:768. When the user lacks "CanView AllRecord" the BLL adds @EntryUser, which
     * USP_ProductionOutputAllocationWithExportInvoice_GetAllMethod does not declare (it declares
     * @EntryUserId), so on the desktop SQL Server refuses the call and the form shows that error.
     * The same message is raised here without sending an undeclared parameter.
     */
    public List<Map<String, Object>> history(String fromDate, String toDate, int jobOrderId) {
        boolean canViewAll = Boolean.TRUE.equals(rights().get("canViewAllRecord"));
        if (!canViewAll) {
            throw new IllegalStateException("@EntryUser is not a parameter for procedure "
                    + ProductionOutputAllocationRepository.GET_ALL_PROC + ".");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                ctx.currentBranchId(), ctx.currentFinancialYearId(), true, ts(fromDate), ts(toDate), jobOrderId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("JobOrderId", col(r, "InvProductionJobOrderId"));
            m.put("JobOrderNo", col(r, "JobOrderNo"));
            m.put("ItemId", col(r, "ItemId"));
            m.put("ItemName", col(r, "ItemName"));
            m.put("Qty", col(r, "Qty"));
            m.put("NetWeight", col(r, "NetWeight"));
            m.put("InvoiceId", col(r, "ExImInvoiceId"));
            m.put("InvoiceNo", col(r, "InvoiceNo"));
            m.put("InvoiceDate", col(r, "InvoiceDate"));
            m.put("EntryDate", col(r, "EntryDate"));
            m.put("EntryUser", col(r, "EntryUser"));
            m.put("ModifyDate", col(r, "ModifyDate"));
            m.put("ModifyUser", col(r, "ModifyUser"));
            out.add(m);
        }
        return out;
    }

    /** Slip:1018 - the rows behind Print-627, so the page can say "Record Not Found For DisPlay". */
    public int slipRowCount(int jobOrderId) {
        if (jobOrderId <= 0) return 0;
        return repo.slipAndRegister(jobOrderId).size();
    }

    /* ================================================================================ helpers */

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o);
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    private static double dbl(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    /** double.ToString() - 15 significant digits, no trailing zeros. */
    private static String net(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        if (d == 0d) return "0";
        return new BigDecimal(d).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static Timestamp ts(String s) {
        String t = s == null ? "" : s.trim();
        if (t.isEmpty()) return null;
        try {
            if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atStartOfDay());
            return Timestamp.valueOf(LocalDateTime.parse(t.length() > 19 ? t.substring(0, 19) : t));
        } catch (Exception e) {
            return null;
        }
    }
}
