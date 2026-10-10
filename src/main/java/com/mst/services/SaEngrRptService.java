package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaEngrRptRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

/**
 * Group E2 - Sale Engr reports 555 / 554 / 556. Tenancy (Organization / Company / User / Branch / Financial Year) always comes from
 * CurrentUserContext, rights are the desktop's ScreenId "View" right (DesktopReportRights). Nothing here edits a shared service.
 */
@Service
public class SaEngrRptService {
    /** dbo.ScreenDefinition ids, keyed by the route segment. */
    public static final Map<String, Integer> SCREENS;
    static {
        Map<String, Integer> m = new LinkedHashMap<>();
        m.put("gdn-history", 555);
        m.put("order-history", 554);
        m.put("sales-activities", 556);
        m.put("mfg-order-history", 848);
        SCREENS = Collections.unmodifiableMap(m);
    }
    /** The form's own screen name (base.Name), the key of its tblUserRights rows (CanChangeOrder* rights). */
    public static final String ORDER_SCREEN_NAME = "frmSaleOrderHistory_Engr";

    private final SaEngrRptRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final DesktopAttachmentStore store;

    public SaEngrRptService(SaEngrRptRepository repository, CurrentUserContext context, DesktopReportRights rights, DesktopAttachmentStore store) {
        this.repository = repository;
        this.context = context;
        this.rights = rights;
        this.store = store;
    }

    private UserAccount user(String key) {
        Integer screen = SCREENS.get(key);
        if (screen == null) throw new IllegalArgumentException("Unknown report " + key);
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    private int appId(UserAccount u) {
        Integer a = u.getAppId();
        return a != null && a > 0 ? a : context.currentAppId();
    }

    public UserAccount currentUser(String key) { return user(key); }

    // ------------------------------------------------------------------ lookups (the form's Load / Refresh)
    public Map<String, Object> lookups(String key) { return lookups(key, null); }

    /** branchIds: 848 only - the comma separated ids of the checked branches (AllDropDownBind builds BranchesIds from them). */
    public Map<String, Object> lookups(String key, String branchIds) {
        UserAccount u = user(key);
        int fy = context.currentFinancialYearId();
        switch (key) {
            case "gdn-history": return repository.gdnLookups(u, fy);
            case "sales-activities": return repository.evaluationLookups(u, fy, appId(u));
            case "order-history": {
                Map<String, Object> d = repository.orderLookups(u, fy, appId(u));
                d.put("rights", orderRights(u));
                return d;
            }
            case "mfg-order-history": {
                boolean first = branchIds == null;                                   // Load: BranchesFill then AllDropDownBind with the user's own branch
                List<Map<String, Object>> branches = first ? repository.mfgBranches(u) : null;
                String ids = first ? ownBranch(u, branches) : allowedBranches(u, branchIds);
                Map<String, Object> d = repository.mfgOrderLookups(u, fy, appId(u), ids, false);
                if (first) d.put("branches", branches);
                d.put("rights", orderRights(u));
                return d;
            }
            default: throw new IllegalArgumentException("Unknown report " + key);
        }
    }

    /** The branch text of cmbBranchName after BranchesFill is the user's branch name: ",{BranchId}" when that branch is in the allocated list. */
    private String ownBranch(UserAccount u, List<Map<String, Object>> branches) {
        Integer own = u.getBranchesId();
        if (own == null || branches == null) return "";
        for (Map<String, Object> b : branches) {
            Object id = b.get("BranchId") != null ? b.get("BranchId") : b.get("BranchID");
            if (SaEngrRptRepository.i(id) == own) return "," + own;
        }
        return "";
    }

    /** The checked branch ids, restricted to the branches allocated to the user (the desktop only offers those); ",id,id" like the desktop string, "" when none. */
    private String allowedBranches(UserAccount u, String requested) {
        if (requested == null || requested.trim().isEmpty()) return "";
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> b : repository.mfgBranches(u)) allowed.add(SaEngrRptRepository.i(b.get("BranchId") != null ? b.get("BranchId") : b.get("BranchID")));
        StringBuilder out = new StringBuilder();
        for (String part : requested.split(",")) {
            String t = part.trim();
            if (!t.matches("\\d{1,9}")) continue;
            if (allowed.contains(Integer.parseInt(t))) out.append(',').append(t);
        }
        return out.toString();
    }

    /**
     * CommonServices.SetRightsValueInRightsObject(base.Name) for the CanChangeOrder* rights: Admin role -> all true, otherwise each flag comes
     * from the user's rights rows of the form's own screen (a flag is only ever set by a row, as in SaleEngrSupport.rights).
     */
    public Map<String, Boolean> orderRights(UserAccount u) {
        String role = context.currentRoleName();
        boolean admin = "Admin".equals(role);
        Map<String, Boolean> r = new LinkedHashMap<>();
        r.put("CanChangeOrderStatusToComplete", admin);
        r.put("CanChangeOrderStatusToCancel", admin);
        r.put("CanChangeOrderExpiryDate", admin);
        if (admin) return r;
        for (Map<String, Object> row : repository.userRights(u, ORDER_SCREEN_NAME, role)) {
            String name = String.valueOf(row.get("RightName")).trim();
            if (!r.containsKey(name)) continue;
            Object v = row.get("Value");
            r.put(name, Boolean.TRUE.equals(v) || "1".equals(String.valueOf(v)) || "true".equalsIgnoreCase(String.valueOf(v)));
        }
        return r;
    }

    // ------------------------------------------------------------------ rows (the Show buttons)
    public List<Map<String, Object>> rows(String key, Map<String, String> q) {
        UserAccount u = user(key);
        switch (key) {
            case "gdn-history": return repository.gdnRegisterEng(u, context.currentFinancialYearId(), q);
            case "sales-activities": return repository.salesActivity(u, q);
            case "order-history": return repository.orderDetail(u, q);
            case "mfg-order-history": return repository.mfgOrderDetail(u, q);
            default: throw new IllegalArgumentException("Unknown report " + key);
        }
    }

    /** 554 GridSummaryFill (the Summary tab). */
    public List<Map<String, Object>> orderSummaryRows(Map<String, String> q) {
        UserAccount u = user("order-history");
        return repository.orderSummary(u, appId(u), q);
    }

    /** 848 GridSummaryFill (the Summary tab of the Mfg form). */
    public List<Map<String, Object>> mfgOrderSummaryRows(Map<String, String> q) {
        UserAccount u = user("mfg-order-history");
        return repository.mfgOrderSummary(u, appId(u), q);
    }

    /**
     * 554 CompleteStatus / CancelStatus / UpdateExpiryDate. The desktop confirms in the form, then calls
     * SaleOrder.UpdateStatusandIsApprovedbyOrderId with one ApprovalList item (ReqType Approve | Status | Cancel | UpdateExpiryDate).
     * The desktop applies the CanChangeOrder* rights only to which button columns the Brief grid (GrdMain) shows; the Detail grid (grdsaleorder) adds
     * Complete / Cancel / Update for every user whenever the Approved Status is "Approve", so the form itself enforces no further right and neither does this call
     * (the View right of the screen is required, as for opening the form). The web page applies the same button rules.
     * ApproveRecord is not reachable on the desktop either: its IsApproved button column is created hidden (Visible = false), so no web
     * endpoint is offered for it.
     */
    public Map<String, Object> orderAction(String action, int id, LocalDate expiry) { return orderAction("order-history", action, id, expiry); }

    /** key: order-history (554) or mfg-order-history (848); both forms call the same SaleOrder.UpdateStatusandIsApprovedbyOrderId. */
    public Map<String, Object> orderAction(String key, String action, int id, LocalDate expiry) {
        if (!"order-history".equals(key) && !"mfg-order-history".equals(key)) throw new IllegalArgumentException("Unknown report " + key);
        UserAccount u = user(key);
        String reqType;
                switch (action == null ? "" : action) {
            case "Complete": reqType = "Status"; break;
            case "Cancel": reqType = "Cancel"; break;
            case "UpdateExpiryDate": reqType = "UpdateExpiryDate"; break;
            default: throw new IllegalArgumentException("Unknown order action");
        }
        if (id <= 0) throw new IllegalArgumentException("Select a record first");
        if ("UpdateExpiryDate".equals(action) && expiry == null) throw new IllegalArgumentException("Select the expiry date");
        repository.updateOrderStatus(u, id, reqType, expiry);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("ok", true);
        return out;
    }

    /**
     * Approval Detail button (frmApprovalCommentory with RefDocumentTypeId / RefDocNoId). USp_ApprovalCommentory has no tenant parameter, so the
     * document is only served when the same report (same filters) returns it for this user: idColumn / typeColumn name its identity columns.
     */
    public List<Map<String, Object>> approvalHistory(String key, int id, int documentTypeId, Map<String, String> q, String idColumn, String typeColumn) {
        List<Map<String, Object>> rows = "order-history".equals(key) && "summary".equals(q.get("kind")) ? orderSummaryRows(q) : rows(key, q);
        boolean present = id > 0 && documentTypeId > 0;
        if (present) {
            present = false;
            for (Map<String, Object> r : rows) {
                if (SaEngrRptRepository.i(ci(r, idColumn)) == id && SaEngrRptRepository.i(ci(r, typeColumn)) == documentTypeId) { present = true; break; }
            }
        }
        if (!present) throw new org.springframework.security.access.AccessDeniedException("This document is not available in the selected report");
        return repository.approvalCommentory(documentTypeId, id);
    }

    private static Object ci(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    // ------------------------------------------------------------------ links
    public int glAccountOfParty(String key, int supplierCustomerId) {
        return repository.glAccountIdOfParty(user(key), supplierCustomerId);
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId): the attachments of the document (name, custom name, entry date). */
    public List<Map<String, Object>> attachments(String key, int id, int documentTypeId) {
        UserAccount u = user(key);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repository.attachmentsByRefDocumentType(u, id, documentTypeId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id") != null ? r.get("Id") : r.get("id"));
            m.put("AttachmentName", r.get("Attachment"));
            m.put("CustomName", r.get("UploadedFileCustomName"));
            m.put("EntryDate", r.get("EntryDate"));
            out.add(m);
        }
        return out;
    }

    public static final class Download {
        public final String name;
        public final byte[] bytes;
        Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
    }

    /** The stored file of one listed attachment (the attachment must be in the document's own list). */
    public Download download(String key, int id, int documentTypeId, int attachmentId) {
        UserAccount u = user(key);
        for (Map<String, Object> r : repository.attachmentsByRefDocumentType(u, id, documentTypeId)) {
            Object rid = r.get("Id") != null ? r.get("Id") : r.get("id");
            if (SaEngrRptRepository.i(rid) != attachmentId) continue;
            String stored = SaEngrRptRepository.str(r.get("UploadedFileCustomName"));
            if (stored.trim().isEmpty()) stored = SaEngrRptRepository.str(r.get("Attachment"));
            String name = baseName(SaEngrRptRepository.str(r.get("Attachment")).isEmpty() ? stored : SaEngrRptRepository.str(r.get("Attachment")));
            return new Download(name, store.read(u, baseName(stored)));
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    private static String baseName(String value) {
        String name = value.replace('\\', '/');
        name = name.substring(name.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(name);
        return name;
    }
}
