package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportDocumentTrackingRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.ExportSdtSupport.*;

/**
 * 623 frmDocumentOfGroup "Document Assign To Group" (Architecture.WinApp.SDT), ClientSize 1084 x 561,
 * DocumentTypeId 243 (the procedure's default; the model has no DocumentTypeId property, so it is never sent).
 *
 * Form tab: Custom Group combo + Show -> "Un-Assigned Document" grid (GetChartOfDocumentByCustomGroupId,
 * grouped by RequiredLevel, mandatory rows (RequiredLevelId 1) pre-checked, header selector) and "Assigned
 * Document" grid (ReadByCustomGroupId; editable Original / Duplicate / CriteriaDateTypeId (combo) /
 * DocumentProviderId (combo) / BeforeDays / AfterDays / PrioritySequence / Remarks / Active; X button on
 * new rows only). The transfer button moves the checked rows with the defaults 1 / 0 / LastCriteriaDateTypeId
 * / 0 / 1 / 1 / "" / true. Save/Update = Insert(). History tab: Entry / Modify / Approved date radios, Custom
 * Group / Document / Required Level / Document Provider combos, Show; Edit reopens the custom group.
 *
 * Desktop quirks kept: DataGridHistoryFill sets ReportsParameters.CustomerGroupId while the BLL reads
 * CustomGroupId, so the history Custom Group filter never reaches the procedure; LstRemoveRecord is never
 * filled (deleting a saved grid row throws "You Can't Delete Saved Record..."); the grid Delete of a new
 * row puts the document back in the un-assigned grid with the provider's display text.
 */
@Service
public class ExportDocumentOfGroupService {

    public static final int SCREEN_ID = 623;
    public static final int DOCUMENT_TYPE_ID = 243;

    @Autowired private ExportDocumentTrackingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    /** frmDocumentOfGroup_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(rights, u, SCREEN_ID, "Save"));
        out.put("permissions", perm);
        out.putAll(gridCombos());
        out.put("customGroups", customGroups(u));
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        out.putAll(historyCombos(u));
        return out;
    }

    /** BtnRefreshUnAssigned_Click: CustomGroupBind, FillgrdCombo, grdAssignCombo. */
    public Map<String, Object> refresh() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customGroups", customGroups(u));
        out.putAll(gridCombos());
        return out;
    }

    /** FillgrdCombo: dtCType (CriteriaDateTypeForCombo) and dtPType (DocumentProviderTypeForCombo). */
    private Map<String, Object> gridCombos() {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> c = new ArrayList<>();
        for (Map<String, Object> r : repo.criteriaDateTypes()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "CriteriaDateTypeId")));
            m.put("name", text(ci(r, "CriteriaDateType")));
            c.add(m);
        }
        out.put("criteriaDateTypes", c);
        List<Map<String, Object>> p = new ArrayList<>();
        for (Map<String, Object> r : repo.documentProviders()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "DocumentProviderId")));
            m.put("name", text(ci(r, "DocumentProvider")));
            p.add(m);
        }
        out.put("documentProviders", p);
        return out;
    }

    private List<Map<String, Object>> customGroups(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.customGroupsForCombo(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "customGroupId")));
            m.put("name", text(ci(r, "customGroupName")));
            out.add(m);
        }
        return out;
    }

    /** ComboBindForHistory / btnRefreshHistoryCombos_Click - the four Id/Name tables by Activity. */
    public Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> rows = repo.docOfGroupDropDown(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyCustomGroups", pick(rows, "customGroup"));
        out.put("historyDocuments", pick(rows, "chartOfDocument"));
        out.put("historyRequiredLevels", pick(rows, "RequiredLevel"));
        out.put("historyProviders", pick(rows, "documentProvider"));
        return out;
    }

    public Map<String, Object> historyCombos() { return historyCombos(user(ctx, rights, SCREEN_ID, "View")); }

    /**
     * btnShowByCustomGroup_Click / ShowData: FillUnAssignData + ReadByCustomGroupId, then
     * LastRecordBycustomGroupId (the CriteriaDateTypeId new rows default to).
     */
    public Map<String, Object> show(int customGroupId, boolean withLast) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        if (customGroupId <= 0) throw new IllegalArgumentException("Please Select custom Group First");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> un = new ArrayList<>();
        for (Map<String, Object> r : repo.chartOfDocumentByCustomGroupId(u, customGroupId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("DocumentId", asInt(ci(r, "chartOfDocumentId")));
            m.put("Document", text(ci(r, "documentName")));
            m.put("RequiredLevelId", asInt(ci(r, "requiredLevelId")));
            m.put("RequiredLevel", text(ci(r, "RequiredLevel")));
            m.put("DocumentProviderId", asInt(ci(r, "DocumentProviderId")));
            m.put("DocProviderName", text(ci(r, "DocProviderName")));
            un.add(m);
        }
        out.put("unassigned", un);
        List<Map<String, Object>> as = new ArrayList<>();
        for (Map<String, Object> r : repo.docOfGroupByCustomGroup(customGroupId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "documentOfGroupId")));
            m.put("DocumentId", asInt(ci(r, "chartOfDocumentId")));
            m.put("Document", text(ci(r, "documentName")));
            m.put("RequiredLevelId", asInt(ci(r, "RequiredLevelId")));
            m.put("RequiredLevel", text(ci(r, "requiredLevelDesc")));
            m.put("DocumentProviderId", asInt(ci(r, "documentProviderId")));
            m.put("Original", asInt(ci(r, "original")));
            m.put("Duplicate", asInt(ci(r, "duplicate")));
            m.put("CriteriaDateTypeId", asInt(ci(r, "CriteriaDateTypeId")));
            m.put("BeforeDays", asInt(ci(r, "beforeDays")));
            m.put("AfterDays", asInt(ci(r, "afterDays")));
            m.put("PrioritySequence", asInt(ci(r, "prioritySeqNo")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("Active", asBool(ci(r, "isActive")));
            as.add(m);
        }
        out.put("assigned", as);
        if (withLast) {
            List<Map<String, Object>> last = repo.docOfGroupLastRecord(u, customGroupId);
            out.put("lastCriteriaDateTypeId", last.isEmpty() ? 0 : asInt(ci(last.get(0), "CriteriaDateTypeId")));
        }
        return out;
    }

    /**
     * Insert(): Custom Group required, grid rows required, per-row rules in the desktop's order (row number
     * = grid RowIndex + 1), at least one mandatory (RequiredLevelId 1) row, then DocumentOfGroup.Save with
     * the removed rows first (none on the desktop) and every grid row (ActionId 1 for Id <= 0, else 2).
     */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "Save");
        int customGroupId = asInt(body.get("customGroupId"));
        if (customGroupId == 0) throw new IllegalArgumentException("Custom Group Field Required...");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found...");
        boolean update = false;
        for (Map<String, Object> r : rows) if (asInt(r.get("Id")) > 0) { update = true; break; }
        Timestamp now = now();
        boolean mandatoryExist = false;
        List<Map<String, Object>> items = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : rows) {
            int n = i + 1;
            if (asInt(r.get("DocumentId")) == 0) throw new IllegalArgumentException("Chart Of Document Required in Row No:" + n);
            if (asInt(r.get("RequiredLevelId")) == 0) throw new IllegalArgumentException("RequiredLevel Required in Row No:" + n);
            if (asInt(r.get("DocumentProviderId")) == 0) throw new IllegalArgumentException("DocumentProvider Required in Row No:" + n);
            if (asInt(r.get("Original")) == 0) throw new IllegalArgumentException("Original Required in Row No:" + n);
            if (asInt(r.get("CriteriaDateTypeId")) == 0) throw new IllegalArgumentException("CriteriaDateType Required in Row No:" + n);
            int before = asInt(r.get("BeforeDays")), after = asInt(r.get("AfterDays"));
            if (before == 0 && after == 0) throw new IllegalArgumentException("BeforeDays or AfterDays Required in Row No:" + n);
            if (before > 0 && after > 0) throw new IllegalArgumentException("BeforeDays or AfterDays Can Be Added,not both.please check in Row No:" + n);
            if (asInt(r.get("PrioritySequence")) == 0) throw new IllegalArgumentException("PrioritySequence Required in Row No:" + n);
            if (!mandatoryExist) mandatoryExist = asInt(r.get("RequiredLevelId")) == 1;
            int id = asInt(r.get("Id"));
            /* Model DocumentOfGroup - the 23 non-virtual properties in declaration order. */
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("IsApproved", false);
            vd.put("isActive", asBool(r.get("Active")));
            vd.put("ApprovedDate", now);
            vd.put("EntryDate", now);
            vd.put("ModifyDate", now);
            vd.put("ActionId", id <= 0 ? 1 : 2);
            vd.put("afterDays", after);
            vd.put("ApprovedUserId", u.getId());
            vd.put("beforeDays", before);
            vd.put("chartOfDocumentId", asInt(r.get("DocumentId")));
            vd.put("RequiredLevelId", asInt(r.get("RequiredLevelId")));
            vd.put("CompanyId", u.getCompanyId());
            vd.put("CriteriaDateTypeId", asInt(r.get("CriteriaDateTypeId")));
            vd.put("customGroupId", customGroupId);
            vd.put("documentOfGroupId", id);
            vd.put("documentProviderId", asInt(r.get("DocumentProviderId")));
            vd.put("duplicate", asInt(r.get("Duplicate")));
            vd.put("EntryUserId", u.getId());
            vd.put("ModifyUserId", u.getId());
            vd.put("OrganizationId", u.getOrganizationId());
            vd.put("original", asInt(r.get("Original")));
            vd.put("prioritySeqNo", asInt(r.get("PrioritySequence")));
            vd.put("Remarks", r.get("Remarks") == null ? "" : String.valueOf(r.get("Remarks")));
            items.add(vd);
            i++;
        }
        if (!mandatoryExist) throw new IllegalArgumentException("Grid Should have at least one Record With Mandatory Status...");
        int id = repo.saveDocumentOfGroup(items);
        return ok(update ? "Update SuccessFully" : "Save SuccessFully ", id);
    }

    /** DataGridHistoryFill - the BLL's parameter list; the desktop grid columns in their order. */
    public List<Map<String, Object>> history(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        boolean viewAll = canViewAll(ctx, rights, u, SCREEN_ID);
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUserId", u.getId());
        /* CustomerGroupId is what the form sets; the BLL reads CustomGroupId (never set) -> no @CustomGroupId. */
        String dateType = text(body.get("dateType"));
        java.sql.Date from = asBool(body.get("fromChecked")) ? sqlDate(body.get("fromDate")) : null;
        java.sql.Date to = asBool(body.get("toChecked")) ? sqlDate(body.get("toDate")) : null;
        if ("entry".equals(dateType)) { if (from != null) p.put("EntryFromDate", from); if (to != null) p.put("EntryToDate", to); }
        else if ("modify".equals(dateType)) { if (from != null) p.put("ModifyFromDate", from); if (to != null) p.put("ModifyToDate", to); }
        else if ("approved".equals(dateType)) { if (from != null) p.put("ApprovedFromDate", from); if (to != null) p.put("ApprovedToDate", to); }
        int provider = asInt(body.get("documentProviderId")), level = asInt(body.get("requiredLevelId")), cod = asInt(body.get("chartOfDocumentId"));
        if (provider != 0) p.put("DocumentProviderId", provider);
        if (level != 0) p.put("RequiredLevelId", level);
        if (cod != 0) p.put("chartOfDocumentId", cod);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.docOfGroupHistory(p)) out.add(historyRow(r));
        return out;
    }

    static Map<String, Object> historyRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "documentOfGroupId")));
        m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
        m.put("CustomGroupId", asInt(ci(r, "customGroupId")));
        m.put("CustomGroup", text(ci(r, "customGroupName")));
        m.put("Document", text(ci(r, "documentName")));
        m.put("RequiredLevel", text(ci(r, "requiredLevelDesc")));
        m.put("DocumentProvider", text(ci(r, "DocProviderName")));
        m.put("Original", asInt(ci(r, "original")));
        m.put("Duplicate", asInt(ci(r, "duplicate")));
        m.put("CriteriaDateType", text(ci(r, "criteriaDateTypeDesc")));
        m.put("BeforeDays", asInt(ci(r, "beforeDays")));
        m.put("AfterDays", asInt(ci(r, "afterDays")));
        m.put("PrioritySequence", asInt(ci(r, "prioritySeqNo")));
        m.put("Remarks", text(ci(r, "Remarks")));
        m.put("Active", asBool(ci(r, "isActive")));
        m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
        m.put("EntryUser", text(ci(r, "EntryUserName")));
        m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
        m.put("ModifyUser", text(ci(r, "ModifyUserName")));
        m.put("ApprovedDate", isoDateTime(ci(r, "ApprovedDate")));
        m.put("ApprovedUser", text(ci(r, "ApprovedUserName")));
        return m;
    }
}
