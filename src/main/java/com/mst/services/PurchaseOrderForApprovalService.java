package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseOrderForApprovalRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Purchase Order approval - the Approval Dashboard cards "Purchase Order" (TypeID 20 and 1100).
 *
 * TypeID 20   Architecture.WinApp.ApprovalDashboard.PurchaseOrderForApproval        (2,508 lines)  variant "po"
 * TypeID 1100 Architecture.WinApp.FeedMill.Approval.PurchaseOrderForApproval        (2,394 lines)  variant "fm"
 *
 * The dashboard's switch (ApprovalDashboard.cs:284-295 and :1100-1105) hands the form
 *   case 20    BranchesId = branch combo (0 when none), RequestedFromDate only when the From tick box is set,
 *              RequestedToDate always
 *   case 1100  nothing at all
 * The class name is the same, the namespace, BLL and procedures are not, so the two are separate variants of
 * one page rather than one form with a flag.
 *
 * Tenancy (organization, company, user, financial year) always comes from CurrentUserContext. The desktop form
 * checks no screen rights beyond the Rightsobjects below; the dashboard only lists the card to a user who may
 * see it. The browser never sends an organization, company or user id.
 *
 * Load (variant "po", :154-190)
 *   BranchImplemented = config "PurchaseOrderBranchWise"; when it is on, UserAccount.BranchesId becomes
 *   BranchesId (the dashboard's) when that is above zero, and BranchesIds = that id as text goes to the history
 *   and the combos, otherwise "" (not sent).
 *   formright = SetRightsValueInRightsObject("PurchaseOrderForApproval"): Admin role = every CanChange* right;
 *   any other role = Sp_tblUserRights_GetAllMethod 'GetByUserId' rows for the ScreenName (the form is not a
 *   ScreenDefinition row, so for a non-Admin role the two rights come back false and the Complete / Cancel
 *   buttons never appear - the same as on the desktop).
 *   AllowEditOnVoucherNoinReports = config.
 *
 * Money and rate display formats follow clsGlobalVariables (Default NoofDecimal Points For Amount / Rate).
 */
@Service
public class PurchaseOrderForApprovalService {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseOrderForApprovalService.class);

    public static final String PO = "po";
    public static final String FM = "fm";
    /** base.Name - the ScreenName CommonServices.SetRightsValueInRightsObject looks up. */
    private static final String SCREEN_NAME = "PurchaseOrderForApproval";

    @Autowired private PurchaseOrderForApprovalRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private AccountReportsDesktopService config;

    // ================================================================== filter

    /** The filter strip of the form: fromdate todate txtSrFrom txtSrTo CmbSupplier CmbItem CmbStatus CmbApproved. */
    public static class Filter {
        public String variant = PO;
        public int branchId;
        public String fromDate;
        public String toDate;
        public String fromNo;
        public String toNo;
        public int supplierId;
        public int itemId;
        public String status;
        public String approved;
    }

    private static String variant(String v) {
        return FM.equalsIgnoreCase(v) ? FM : PO;
    }

    private UserAccount user() {
        return ctx.requireAccountingUser();
    }

    // ================================================================== setup (PurchaseOrderHistory_Load)

    public Map<String, Object> setup(String variantIn, int branchId, String reqFrom, String reqTo) {
        String variant = variant(variantIn);
        UserAccount u = user();
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("variant", variant);
        out.put("title", PO.equals(variant) ? "Purchase Order" : "Purchase Order Approval");

        boolean branchImplemented = false;
        int effectiveBranch = 0;
        if (PO.equals(variant)) {
            branchImplemented = safeConfigBool("PurchaseOrderBranchWise");
            if (branchImplemented) effectiveBranch = effectiveBranch(u, branchId);
        }
        out.put("branchImplemented", branchImplemented);
        out.put("branchesId", effectiveBranch);

        Map<String, Object> rights = rights(u);
        out.put("rights", rights);
        out.put("allowEditOnVoucherNoinReports", safeConfigBool("AllowEditOnVoucherNoinReports"));

        // AllDropDownBind / SupplierNameFill + ItemDetailFill - a failure is the desktop's MessageBox, the screen carries on.
        try {
            out.putAll(combos(variant, branchImplemented ? String.valueOf(effectiveBranch) : ""));
        } catch (Exception e) {
            LOG.error("Purchase order approval combos failed", e);
            out.put("suppliers", new ArrayList<>());
            out.put("items", new ArrayList<>());
            out.put("comboError", message(e));
        }

        String yearStart = "";
        try { yearStart = financialYearStart(org, comp); } catch (Exception e) { LOG.error("Financial year start failed", e); }
        String from = PO.equals(variant) ? iso(reqFrom) : null;
        String to = PO.equals(variant) ? iso(reqTo) : null;
        if (from != null && to != null) {
            out.put("fromDate", from);
            out.put("toDate", to);
        } else {
            out.put("fromDate", yearStart);
            out.put("toDate", LocalDate.now().toString());
        }
        out.put("amountDecimals", decimals("Default NoofDecimal Points For Amount", 0));
        out.put("rateDecimals", decimals("Default NoofDecimal Points For Rate", 2));
        return out;
    }

    /** AllDropDownBind (:192-231) / SupplierNameFill + ItemDetailFill (FeedMill :235-299). */
    public Map<String, Object> combos(String variantIn, String branchesIds) {
        String variant = variant(variantIn);
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        List<Map<String, Object>> suppliers = new ArrayList<>();
        List<Map<String, Object>> items = new ArrayList<>();
        if (PO.equals(variant)) {
            List<Map<String, Object>> all = repo.dropDownPo(org, comp, "41", branchesIds);
            if (!all.isEmpty()) {
                for (Map<String, Object> r : all) {
                    Object act = r.get("Activity");
                    String a = act == null ? null : String.valueOf(act);
                    if ("Supplier".equals(a)) suppliers.add(idName(r));
                    else if ("Item".equals(a)) items.add(idName(r));
                }
            }
        } else {
            for (Map<String, Object> r : repo.dropDownFm(org, comp, "Supplier")) suppliers.add(idName(r));
            for (Map<String, Object> r : repo.dropDownFm(org, comp, "Item")) items.add(idName(r));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", suppliers);
        out.put("items", items);
        return out;
    }

    /** Btnrefresh_Click - AllDropDownBind again, with the branch filter of the Load. */
    public Map<String, Object> refreshCombos(String variantIn, int branchId) {
        String variant = variant(variantIn);
        String branches = "";
        if (PO.equals(variant) && safeConfigBool("PurchaseOrderBranchWise")) {
            branches = String.valueOf(effectiveBranch(user(), branchId));
        }
        return combos(variant, branches);
    }

    private static Map<String, Object> idName(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("id", toInt(r.get("Id")));
        Object n = r.get("ReferenceName");
        m.put("name", n == null ? "" : String.valueOf(n));
        return m;
    }

    // ================================================================== rights

    /** CommonServices.SetRightsValueInRightsObject(base.Name) - only the two flags this form reads. */
    public Map<String, Object> rights(UserAccount u) {
        boolean complete = false;
        boolean cancel = false;
        String role = ctx.currentRoleName();
        if ("Admin".equals(role)) {
            complete = true;
            cancel = true;
        } else {
            try {
                for (Map<String, Object> r : repo.rightsByUser(ctx.currentUserId(), SCREEN_NAME, role, ctx.currentCompanyId())) {
                    String name = r.get("RightName") == null ? "" : String.valueOf(r.get("RightName"));
                    boolean value = toBool(r.get("Value"));
                    if ("CanChangeOrderStatusToComplete".equals(name)) complete = value;
                    else if ("CanChangeOrderStatusToCancel".equals(name)) cancel = value;
                }
            } catch (Exception e) {
                LOG.error("Rights lookup for {} failed", SCREEN_NAME, e);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canChangeOrderStatusToComplete", complete);
        out.put("canChangeOrderStatusToCancel", cancel);
        return out;
    }

    private boolean can(String key) {
        Object v = rights(user()).get(key);
        return v instanceof Boolean && (Boolean) v;
    }

    // ================================================================== history (GetDataForApproval)

    /**
     * GetDataForApproval - the "Po_History" grid. rows = the grouped master grid (one row per order);
     * lines = every history row's detail columns, keyed by order Id (grd_SelectionChanged filters them client side).
     */
    public Map<String, Object> history(Filter f) {
        f.variant = variant(f.variant);
        List<Map<String, Object>> raw = rawHistory(f);
        Map<String, Object> out = new LinkedHashMap<>();
        if (raw.isEmpty()) {
            out.put("rows", new ArrayList<>());
            out.put("lines", new ArrayList<>());
            return out;
        }
        if (PO.equals(f.variant)) {
            out.put("rows", masterRowsPo(raw, raw, false));
            out.put("lines", linesPo(raw, true));
        } else {
            out.put("rows", masterRowsFm(raw, false, null));
            out.put("lines", linesFm(raw, true));
        }
        return out;
    }

    /**
     * GetPendingPurchaseOrder - the "Pending Order For View" grid. It is always run right after GetDataForApproval
     * with the same filter strip, and variant "po" sums the Amount column from the history rows of that run
     * (grdlst) rather than from its own rows, so the history is read again here.
     */
    public Map<String, Object> pending(Filter f) {
        f.variant = variant(f.variant);
        Map<String, Object> out = new LinkedHashMap<>();
        if (PO.equals(f.variant)) {
            List<Map<String, Object>> grdlst = rawHistory(f);
            List<Map<String, Object>> pend = rawPending(f);
            if (pend.isEmpty()) {
                out.put("rows", new ArrayList<>());
                out.put("lines", new ArrayList<>());
                return out;
            }
            out.put("rows", masterRowsPo(pend, grdlst, true));
            out.put("lines", linesPo(pend, false));
        } else {
            List<Map<String, Object>> grdlst = rawHistory(f);
            List<Map<String, Object>> pend = rawPending(f);
            if (pend.isEmpty()) {
                out.put("rows", new ArrayList<>());
                out.put("lines", new ArrayList<>());
                return out;
            }
            out.put("rows", masterRowsFm(pend, true, grdlst));
            out.put("lines", linesFm(pend, false));
        }
        return out;
    }

    private List<Map<String, Object>> rawHistory(Filter f) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        Timestamp from = stamp(f.fromDate);
        Timestamp to = stamp(f.toDate);
        int noFrom = convInt(f.fromNo);
        int noTo = convInt(f.toNo);
        String status = status(f.status);
        Boolean approved = approvedFlag(f.approved);
        if (PO.equals(f.variant)) {
            return repo.historyPo(org, comp, 41, from, to, noFrom, noTo, f.supplierId, f.itemId,
                    approved, status, false, branchesIds(f));
        }
        return repo.historyFm(org, comp, 1100, from, to, noFrom, noTo, f.supplierId, f.itemId,
                approved, status == null ? "" : status, 1);
    }

    private List<Map<String, Object>> rawPending(Filter f) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        Timestamp from = stamp(f.fromDate);
        Timestamp to = stamp(f.toDate);
        int noFrom = convInt(f.fromNo);
        int noTo = convInt(f.toNo);
        String status = status(f.status);
        if (PO.equals(f.variant)) {
            return repo.historyPo(org, comp, 41, from, to, noFrom, noTo, f.supplierId, f.itemId,
                    Boolean.FALSE, status, true, branchesIds(f));
        }
        return repo.historyFm(org, comp, 1100, from, to, noFrom, noTo, f.supplierId, f.itemId,
                Boolean.FALSE, status == null ? "" : status, 2);
    }

    /** BranchesIds = BranchImplemented ? UserAccount.BranchesId.ToString() : "". */
    private String branchesIds(Filter f) {
        if (!safeConfigBool("PurchaseOrderBranchWise")) return "";
        return String.valueOf(effectiveBranch(user(), f.branchId));
    }

    /** UserAccount.BranchesId = (BranchesId > 0) ? BranchesId : UserAccount.BranchesId. */
    private static int effectiveBranch(UserAccount u, int branchId) {
        if (branchId > 0) return branchId;
        Integer own = u.getBranchesId();
        return own == null ? 0 : own;
    }

    // ------------------------------------------------------------------ TypeID 20 rows

    /**
     * The grouped dt of GetDataForApproval (:262-306) / GetPendingPurchaseOrder (:~745-800).
     * qty and weight come from {@code src}; the amount comes from {@code amountSrc} - the same list for the
     * history grid, the history rows (grdlst) for the pending grid. AvgRate40Kg = amount / weight * 40 in double
     * arithmetic, so a zero weight is NaN / Infinity exactly as the .NET double is.
     */
    private List<Map<String, Object>> masterRowsPo(List<Map<String, Object>> src, List<Map<String, Object>> amountSrc,
                                                   boolean pending) {
        Map<Integer, double[]> totals = new HashMap<>();
        for (Map<String, Object> r : src) {
            double[] t = totals.computeIfAbsent(toInt(r.get("Id")), k -> new double[3]);
            t[1] += toDouble(r.get("OrderItemQty"));
            t[2] += toDouble(r.get("NetWeight"));
            if (!pending) t[0] += toDouble(r.get("Amount"));
        }
        if (pending) {
            Map<Integer, Double> amounts = new HashMap<>();
            for (Map<String, Object> r : amountSrc) amounts.merge(toInt(r.get("Id")), toDouble(r.get("Amount")), Double::sum);
            for (Map.Entry<Integer, double[]> e : totals.entrySet()) {
                Double a = amounts.get(e.getKey());
                e.getValue()[0] = a == null ? 0.0 : a;
            }
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        java.util.Set<Integer> seen = new java.util.HashSet<>();
        for (Map<String, Object> r : src) {
            int id = toInt(r.get("Id"));
            if (!seen.add(id)) continue;
            double[] t = totals.get(id);
            double amount = t[0], qty = t[1], weight = t[2];
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("DocumentTypeId", val(r.get("DocumentTypeId")));
            m.put("DocNo", val(r.get("DocNo")));
            m.put("DocDate", val(r.get("DocDate")));
            if (!pending) {
                m.put("BranchSrNo", val(r.get("BranchSrNo")));
                m.put("BranchName", val(r.get("BranchName")));
                m.put("OrderSupCustId", val(r.get("OrderSupCustId")));
            } else {
                m.put("SupplierCustomerId", val(r.get("OrderSupCustId")));
            }
            m.put("SupplierName", val(r.get("SupplierName")));
            m.put("CommissionAgent", val(r.get("CompanyNameSpCAgent")));
            m.put("DeliveryTerm", val(r.get("DeliveryTerm")));
            m.put("OrderExpiryDate", val(r.get("OrderExpiryDate")));
            m.put("OrderQty", qty);
            m.put("OrderWeight", weight);
            m.put("AvgRate40Kg", doubleText(amount / weight * 40.0));
            m.put("Amount", amount);
            m.put("EntryDate", val(r.get("EntryDate")));
            m.put("EntryUser", val(r.get("UserNameEusr")));
            m.put("ModifyDate", val(r.get("ModifyDate")));
            m.put("ModifyUser", val(r.get("UserNameMusr")));
            m.put("NoOfAttachments", val(r.get("NoOfAttachments")));
            rows.add(m);
        }
        return rows;
    }

    /** grd_SelectionChanged (:560-600) / grdPendingViewVoucher_SelectionChanged (:1004-1040). */
    private List<Map<String, Object>> linesPo(List<Map<String, Object>> src, boolean withLab) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("PurchaseGLAC", val(r.get("PurchaseGLAC")));
            m.put("ItemId", val(r.get("OrderItemId")));
            m.put("ItemName", val(r.get("ItemName")));
            m.put("CropYear", val(r.get("Crop")));
            m.put("PackUom", val(r.get("UOMCodeItm")));
            m.put("ItemQty", toDouble(r.get("OrderItemQty")));
            m.put("NetWeight", toDouble(r.get("NetWeight")));
            m.put("ItemRate", toDouble(r.get("OrderItemRate")));
            m.put("RateUom", val(r.get("UOMCodeRate")));
            m.put("ItemAmount", toDouble(r.get("Amount")));
            m.put("PPBagPrice", plain(r.get("PPBagRate")));
            m.put("JuteBagPrice", plain(r.get("JuteBagRate")));
            m.put("PPBagWeight", plain(r.get("PPBagWeight")));
            m.put("JuteBagWeight", plain(r.get("JuteBagWeight")));
            m.put("JobLot", val(r.get("JobLotDescription")));
            if (withLab) {
                m.put("LabSampleId", val(r.get("InvLabSampleAnalysisHeaderId")));
                m.put("LabSampleNo", val(r.get("LabSampleNo")));
            }
            m.put("CityArea", val(r.get("CityArea")));
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------ TypeID 1100 rows

    /**
     * FeedMill GetDataForApproval (:359-440) / GetPendingForReviewPurchaseOrder (:537-612). Amounts are decimal, so a
     * zero weight is the desktop's "Attempted to divide by zero." and nothing is filled. The pending grid's
     * quantity is summed over the pending rows whose Id equals the Id of grdlst row i (:568, the desktop indexes
     * grdlst there) - reproduced, including the "There is no row at position i." that follows when grdlst is shorter.
     */
    private List<Map<String, Object>> masterRowsFm(List<Map<String, Object>> src, boolean pending,
                                                   List<Map<String, Object>> grdlst) {
        List<Map<String, Object>> rows = new ArrayList<>();
        java.util.Set<Integer> seen = new java.util.HashSet<>();
        for (int i = 0; i < src.size(); i++) {
            Map<String, Object> r = src.get(i);
            int id = toInt(r.get("Id"));
            BigDecimal totalAmount = BigDecimal.ZERO;
            BigDecimal totalWeight = BigDecimal.ZERO;
            BigDecimal totalQty = BigDecimal.ZERO;
            int qtyId = id;
            if (pending) {
                if (i >= grdlst.size()) throw new IllegalStateException("There is no row at position " + i + ".");
                qtyId = toInt(grdlst.get(i).get("Id"));
            }
            for (Map<String, Object> x : src) {
                int xid = toInt(x.get("Id"));
                if (xid == id) {
                    totalAmount = totalAmount.add(toDecimal(x.get("Amount")));
                    totalWeight = totalWeight.add(toDecimal(x.get("NetWeight")));
                }
                if (xid == qtyId) totalQty = totalQty.add(toDecimal(x.get("ItemQty")));
            }
            if (totalWeight.signum() == 0) throw new IllegalStateException("Attempted to divide by zero.");
            BigDecimal avg = totalAmount.divide(totalWeight, MathContext.DECIMAL128).multiply(BigDecimal.valueOf(40));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("DocumentTypeId", val(r.get("DocumentTypeId")));
            m.put("DocNo", val(r.get("DocNo")));
            m.put("DocDate", val(r.get("DocDate")));
            m.put(pending ? "SupplierCustomerId" : "OrderSupCustId", val(r.get("OrderSupCustId")));
            m.put("SupplierName", val(r.get("SupplierName")));
            m.put("CommissionAgent", val(r.get("CompanyNameSpCAgent")));
            m.put("DeliveryTerm", val(r.get("DeliveryTerm")));
            m.put("OrderExpiryDate", val(r.get("OrderExpiryDate")));
            m.put("OrderQty", totalQty.doubleValue());
            m.put("OrderWeight", totalWeight.doubleValue());
            m.put("AvgRate40Kg", avg.doubleValue());
            m.put("Amount", totalAmount.doubleValue());
            m.put("EntryDate", val(r.get("EntryDate")));
            m.put("EntryUser", val(r.get("UserNameEusr")));
            m.put("ModifyDate", val(r.get("ModifyDate")));
            m.put("ModifyUser", val(r.get("UserNameMusr")));
            m.put("NoOfAttachments", val(r.get("NoOfAttachments")));
            rows.add(m);
        }
        return rows;
    }

    /** FeedMill grd_SelectionChanged (:803-846) / grdPendingViewVoucher_SelectionChanged (:1357-1398). */
    private List<Map<String, Object>> linesFm(List<Map<String, Object>> src, boolean withLab) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("PurchaseGLAC", val(r.get("PurchaseGLAC")));
            m.put("ItemId", val(r.get("ItemId")));
            m.put("ItemName", val(r.get("ItemName")));
            m.put("CropYear", val(r.get("CropYear")));
            m.put("PackUom", val(r.get("UOMCodeItm")));
            m.put("ItemQty", toDouble(r.get("ItemQty")));
            m.put("NetWeight", toDouble(r.get("NetWeight")));
            m.put("ItemRate", toDouble(r.get("ItemRate")));
            m.put("RateUom", val(r.get("UOMCodeRate")));
            m.put("ItemAmount", toDouble(r.get("Amount")));
            m.put("PPBagPrice", toDouble(r.get("PPBagRate")));
            m.put("JuteBagPrice", toDouble(r.get("JuteBagRate")));
            m.put("PPBagWeight", toDouble(r.get("PPBagWeight")));
            m.put("JuteBagWeight", toDouble(r.get("JuteBagWeight")));
            m.put("JobLot", val(r.get("JobLotDescription")));
            if (withLab) {
                m.put("LabSampleId", val(r.get("InvLabSampleAnalysisHeaderId")));
                m.put("LabSampleNo", val(r.get("LabSampleNo")));
            }
            m.put("CityArea", val(r.get("CityName")));
            out.add(m);
        }
        return out;
    }

    // ================================================================== approve / status / cancel

    /**
     * ApproveOrder (:1327-1360), btnApprovedOrder_Click (:1362-1403), BtnCancel_Click (:1405-1448),
     * btnPendingForView_Click (:1450-1497) of variant "po"; ApproveOrder (:938-971), btnApprovedOrder_Click (:1174-1215),
     * btnPendingForView_Click (:1217-1259) of variant "fm".
     *
     * source
     *   row          a row button of the grid: reqType Approve | Status | Cancel | ApproveAndCancel (variant fm: AP)
     *   approve      the toolbar "Approved" button (reqType Approve / AP)
     *   cancel       the toolbar "Cancel" button (reqType Cancel, variant po only)
     *   pending      the toolbar "Pending For View" button (reqType PendingForView)
     * approved / status are the CmbApproved / CmbStatus texts at the click: the desktop only draws the buttons, and
     * only runs the toolbar handlers, for the matching combination, so the same conditions are enforced here.
     */
    public Map<String, Object> update(String variantIn, String source, String reqTypeIn, List<Integer> ids,
                                      String approved, String status) {
        String variant = variant(variantIn);
        user();
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        int userId = ctx.currentUserId();
        String ap = approved == null ? "" : approved.trim();
        String st = status == null ? "" : status.trim();
        String src = source == null ? "" : source;
        List<Integer> clean = new ArrayList<>();
        if (ids != null) for (Integer i : ids) if (i != null && i != 0) clean.add(i);

        String reqType;
        String okMessage;
        if ("row".equals(src)) {
            if (clean.size() != 1) throw new IllegalArgumentException("Record Not Found");
            if (PO.equals(variant)) {
                String r = reqTypeIn == null ? "" : reqTypeIn;
                if ("Approve".equals(r) || "ApproveAndCancel".equals(r)) {
                    if (!("Not Approved".equals(ap) && "Open".equals(st)))
                        throw new IllegalArgumentException("This action is not available for the selected filters");
                } else if ("Status".equals(r) || "Cancel".equals(r)) {
                    if (!("Approved".equals(ap) && "Open".equals(st)))
                        throw new IllegalArgumentException("This action is not available for the selected filters");
                    if (!can("Status".equals(r) ? "canChangeOrderStatusToComplete" : "canChangeOrderStatusToCancel"))
                        throw new AccessDeniedException("The user does not have rights for this order action");
                } else {
                    throw new IllegalArgumentException("Unknown order action");
                }
                reqType = r;
            } else {
                reqType = "AP";
            }
            okMessage = "Record " + (PO.equals(variant) ? reqType : "Approve") + " Successfully";
        } else if ("approve".equals(src)) {
            if (clean.isEmpty()) throw new IllegalArgumentException("Please select check box first");
            if (!"Not Approved".equals(ap)) throw new IllegalArgumentException("Please Select Not Approved status first");
            reqType = PO.equals(variant) ? "Approve" : "AP";
            okMessage = "Record Approve Successfully";
        } else if ("cancel".equals(src) && PO.equals(variant)) {
            if (clean.isEmpty()) throw new IllegalArgumentException("Please select check box first");
            if (!("Not Approved".equals(ap) && "Open".equals(st))) {
                Map<String, Object> none = new LinkedHashMap<>();
                none.put("ok", true);
                none.put("count", 0);
                none.put("message", "");
                return none;
            }
            reqType = "Cancel";
            okMessage = "Record Approve & Cancel Successfully";
        } else if ("pending".equals(src)) {
            if (clean.isEmpty()) throw new IllegalArgumentException("Please select check box first");
            if (!"Not Approved".equals(ap)) throw new IllegalArgumentException("Please Select Not Approved status first");
            reqType = "PendingForView";
            okMessage = "Record Updated Successfully";
        } else {
            throw new IllegalArgumentException("Unknown order action");
        }

        // One call per order, in grid order, no transaction - UpdateStatusandIsApprovedbyOrderId / PurchaseOrderandVoucherApprove.
        int done = 0;
        try {
            for (Integer id : clean) {
                if (PO.equals(variant)) {
                    repo.updatePo(org, comp, userId, id, reqType);
                } else {
                    if (!repo.fmOrderBelongsTo(org, comp, id)) throw new IllegalArgumentException("Record Not Found");
                    repo.updateFm(userId, id, reqType);
                }
                done++;
            }
        } catch (RuntimeException e) {
            if (done > 0) {
                throw new IllegalStateException(message(e) + " (" + done + " of " + clean.size()
                        + " orders were updated before this failed. Press Show to see the current state.)", e);
            }
            throw e;
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("ok", true);
        out.put("count", done);
        out.put("message", okMessage);
        return out;
    }

    // ================================================================== links

    /** CommonServices.GoToCustomerLedgerFromLinkedEvent - the supplier's GL account id for the ledger report. */
    public Map<String, Object> glAccount(int supplierCustomerId) {
        List<Map<String, Object>> rows = repo.glAccountOfParty(ctx.currentOrganizationId(), ctx.currentCompanyId(), supplierCustomerId);
        Map<String, Object> out = new LinkedHashMap<>();
        int id = 0;
        if (!rows.isEmpty() && !rows.get(0).isEmpty()) id = toInt(rows.get(0).values().iterator().next());
        out.put("accountId", id);
        return out;
    }

    // ================================================================== helpers

    private String financialYearStart(int org, int comp) {
        int year = ctx.currentFinancialYearId();
        for (Map<String, Object> r : repo.activeYears(org, comp)) {
            if (toInt(r.get("Id")) == year) {
                Object s = r.get("Start_Period");
                String t = s == null ? null : iso(String.valueOf(s));
                return t == null ? "" : t;
            }
        }
        return "";
    }

    private boolean safeConfigBool(String key) {
        try { return config.configBool(key); }
        catch (Exception e) { LOG.error("Configuration {} lookup failed", key, e); return false; }
    }

    private int decimals(String key, int dflt) {
        try {
            String v = config.config(key);
            int n = (int) Double.parseDouble(v.trim());
            return n < 0 ? dflt : n;
        } catch (Exception e) {
            return dflt;
        }
    }

    /** CmbStatus.Text.Trim(): one of the three fixed rows; anything else is not sent. */
    private static String status(String s) {
        String t = s == null ? "" : s.trim();
        return Arrays.asList("Open", "Complete", "Cancel").contains(t) ? t : null;
    }

    /** Approved -> true, Not Approved -> false, All -> not sent (ApprovedFilter "All"); an empty combo leaves IsApproved false. */
    private static Boolean approvedFlag(String s) {
        String t = s == null ? "" : s.trim();
        if ("Approved".equals(t)) return Boolean.TRUE;
        if ("All".equals(t)) return null;
        return Boolean.FALSE;
    }

    /** Conversion.ToInt(text): anything that is not a whole number is 0. */
    private static int convInt(String s) {
        String t = s == null ? "" : s.trim();
        if (!t.matches("-?\\d{1,9}")) return 0;
        return Integer.parseInt(t);
    }

    private static Timestamp stamp(String s) {
        String t = iso(s);
        return t == null ? null : Timestamp.valueOf(LocalDate.parse(t).atStartOfDay());
    }

    private static String iso(String s) {
        if (s == null) return null;
        String t = s.trim();
        if (t.length() < 10) return null;
        try { return LocalDate.parse(t.substring(0, 10)).toString(); } catch (Exception e) { return null; }
    }

    private static String message(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() != null ? t.getMessage() : e.getMessage();
        return m == null ? e.getClass().getSimpleName() : m;
    }

    private static Object val(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toString();
        if (v instanceof LocalDateTime || v instanceof LocalDate) return v.toString();
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        return v;
    }

    /** A decimal(18,2) bound to an untyped DataTable column is shown as its text ("0.00"). */
    private static Object plain(Object v) {
        if (v == null) return null;
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        return String.valueOf(v);
    }

    private static String doubleText(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        return String.valueOf(d);
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double toDouble(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0.0; }
    }

    private static BigDecimal toDecimal(Object o) {
        if (o == null) return BigDecimal.ZERO;
        if (o instanceof BigDecimal) return (BigDecimal) o;
        try { return new BigDecimal(String.valueOf(o).trim()); } catch (Exception e) { return BigDecimal.ZERO; }
    }

    private static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
