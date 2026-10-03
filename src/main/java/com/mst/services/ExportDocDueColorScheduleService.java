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
 * 619 frmDocDueAlertColorSchedule "DocDue Color Schedule" (Architecture.WinApp.SDT), ClientSize 1153 x 461.
 *
 * Left: grdChartOfDocument (ChartOfDocument.FormHistory); selecting a row builds GridUnAllocatedTemplateII in
 * memory from AlertLevelColor.FormHistory: for every colour row a range of Interval = LeadTime / 3 days,
 * BeforeRangeFrom = levelIndex * Interval + 1, BeforeRangeTo = (levelIndex + 1) * Interval, levelIndex
 * cycling 0..2. The Update button (DoHaveUpdateRights) saves the CHECKED rows of one colour group
 * (Insert(grd)) through DocDueAlertColorSchedule.Save. Right: the History grid with From/To (Entry Date /
 * Modify Date radios - neither is checked on the desktop, so no date reaches the procedure unless a radio is
 * picked), the Chart Of Document combo (DropDownFillFromDocDueAlertColorSchedule 'chartOfDocument') and Show.
 */
@Service
public class ExportDocDueColorScheduleService {

    public static final int SCREEN_ID = 619;

    @Autowired private ExportDocumentTrackingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    /** frmDocDueAlertColorSchedule_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Update", allowed(rights, u, SCREEN_ID, "Update"));
        out.put("permissions", perm);
        out.put("alertLevelColors", alertColors());
        out.put("chartOfDocuments", codRows(u));
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        out.put("historyDocuments", historyDocuments(u));
        return out;
    }

    /** btnRefresh_Click: AlertLevelColor.FormHistory + CODGridFill. */
    public Map<String, Object> refresh() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("alertLevelColors", alertColors());
        out.put("chartOfDocuments", codRows(u));
        return out;
    }

    private List<Map<String, Object>> alertColors() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.alertLevelColors()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AlertLevelColorId", asInt(ci(r, "AlertLevelColorId")));
            m.put("ColorGroupId", asInt(ci(r, "ColorGroupId")));
            m.put("ColorGroup", text(ci(r, "ColorGroup")));
            m.put("LevelNameId", asInt(ci(r, "LevelNameId")));
            m.put("LevelName", text(ci(r, "levelName")));
            m.put("ColorCode", text(ci(r, "ColorCode")));
            m.put("ColorHex", colorNameToHex(text(ci(r, "ColorCode"))));
            out.add(m);
        }
        return out;
    }

    /** CODGridFill: Id, DocumentCode, DocumentName, RequiredLevel (hidden), LeadTime. */
    private List<Map<String, Object>> codRows(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.chartOfDocumentHistory(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "chartOfDocumentId")));
            m.put("DocumentCode", text(ci(r, "documentCode")));
            m.put("DocumentName", text(ci(r, "documentName")));
            m.put("RequiredLevel", text(ci(r, "RequiredLevel")));
            m.put("LeadTime", asInt(ci(r, "leadTime")));
            out.add(m);
        }
        return out;
    }

    /** HistoryComboFill / btnRefreshHistory_Click. */
    public List<Map<String, Object>> historyDocuments(UserAccount u) {
        return pick(repo.docDueDropDown(u, "chartOfDocument"), "chartOfDocument");
    }

    public List<Map<String, Object>> historyDocuments() { return historyDocuments(user(ctx, rights, SCREEN_ID, "View")); }

    /**
     * HistoryFill: CanViewAllRecord from the rights; when the user cannot view all, only @EntryUserId is
     * added (the desktop's else-if chain skips the dates entirely); else the Entry or Modify dates of the
     * ticked pickers by the checked radio; @chartOfDocumentId when a document is chosen.
     */
    public List<Map<String, Object>> history(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        boolean viewAll = canViewAll(ctx, rights, u, SCREEN_ID);
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "CanViewAllRecord", viewAll);
        String dateType = text(body.get("dateType"));
        java.sql.Date from = asBool(body.get("fromChecked")) ? sqlDate(body.get("fromDate")) : null;
        java.sql.Date to = asBool(body.get("toChecked")) ? sqlDate(body.get("toDate")) : null;
        if (!viewAll) {
            p.put("EntryUserId", u.getId());
        } else if ("entry".equals(dateType)) {
            if (from != null) p.put("EntryFromDate", from);
            if (to != null) p.put("EntryToDate", to);
        } else if ("modify".equals(dateType)) {
            if (from != null) p.put("ModifyFromDate", from);
            if (to != null) p.put("ModifyToDate", to);
        }
        int cod = asInt(body.get("chartOfDocumentId"));
        if (cod != 0) p.put("chartOfDocumentId", cod);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.docDueHistory(p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asLong(ci(r, "DocDueAlertColorScheduleId")));
            m.put("chartOfDocumentId", asInt(ci(r, "chartOfDocumentId")));
            m.put("DocumentName", text(ci(r, "documentName")));
            m.put("ColorGroupId", asInt(ci(r, "ColorGroupId")));
            m.put("ColorGroup", text(ci(r, "ColorGroup")));
            m.put("LevelNameId", asInt(ci(r, "LevelNameId")));
            m.put("LevelName", text(ci(r, "levelName")));
            m.put("ColorCode", text(ci(r, "ColorCode")));
            m.put("ColorHex", colorNameToHex(text(ci(r, "ColorCode"))));
            m.put("BeforeRangeFrom", asInt(ci(r, "BeforeRangeFrom")));
            m.put("BeforeRangeTo", asInt(ci(r, "BeforeRangeTo")));
            m.put("SeqNo", asInt(ci(r, "SeqNo")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            out.add(m);
        }
        return out;
    }

    /**
     * Insert(GridUnAllocatedTemplateII): the page sends the checked rows in grid order, each with its grid
     * RowIndex (after UpdateGridBasedOnGroupId has checked every row of the first checked row's group).
     * Rules in the desktop's order: none checked -> "Check Row's of ColorGroup You want To Update"; a row of
     * another group -> "Please Select Same Group Rows..."; ranges must chain (from == previousTo + 1) and
     * from <= to. SeqNo = RowIndex, ColorCode = ColorNameToHexCode(name).
     */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "Update");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Check Row's of ColorGroup You want To Update");
        int groupId = 0, previousTo = 0;
        Timestamp now = now();
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (groupId == 0) groupId = asInt(r.get("ColorGroupId"));
            if (groupId != asInt(r.get("ColorGroupId"))) throw new IllegalArgumentException("Please Select Same Group Rows...");
            int rowIndex = asInt(r.get("RowIndex"));
            int rangeFrom = asInt(r.get("BeforeRangeFrom")), rangeTo = asInt(r.get("BeforeRangeTo"));
            if (previousTo > 0 && rangeFrom != previousTo + 1)
                throw new IllegalArgumentException("Invalid Range: 'BeforeRangeFrom' for row " + (rowIndex + 1) + " should be " + (previousTo + 1));
            if (rangeFrom > rangeTo)
                throw new IllegalArgumentException("Invalid Range: 'BeforeRangeFrom' should be less than 'BeforeRangeTo' for row " + (rowIndex + 1));
            previousTo = rangeTo;
            /* Model DocDueAlertColorSchedule - every non-virtual property, in declaration order. */
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("EntryDate", now);
            vd.put("ModifyDate", now);
            vd.put("BeforeRangeFrom", rangeFrom);
            vd.put("BeforeRangeTo", rangeTo);
            vd.put("ChartOfDocumentId", asInt(r.get("chartOfDocumentId")));
            vd.put("ColorGroupId", asInt(r.get("ColorGroupId")));
            vd.put("CompanyId", u.getCompanyId());
            vd.put("EntryUserId", u.getId());
            vd.put("LevelNameId", asInt(r.get("LevelNameId")));
            vd.put("ModifyUserId", u.getId());
            vd.put("OrganizationId", u.getOrganizationId());
            vd.put("SeqNo", rowIndex);
            vd.put("DocDueAlertColorScheduleId", 0L);
            vd.put("ColorCode", colorNameToHex(text(r.get("ColorCode"))));
            items.add(vd);
        }
        int id = repo.saveDocDueSchedule(items);
        return ok("Row's Update Successfully", id);
    }
}
