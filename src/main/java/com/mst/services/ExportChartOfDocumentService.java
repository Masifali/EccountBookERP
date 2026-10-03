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

import static com.mst.services.ExportSdtSupport.*;

/**
 * 620 DefineChartOfDocument "Define Chart Of Document" (Architecture.WinApp.SDT), ClientSize 685 x 405.
 *
 * Desktop quirk kept: DateLock_Load disables btnSave, btnAdd and btnupdate unconditionally (no rights are
 * consulted) and the procedure's insert branch RAISERRORs "New ChartOfDocument Definition Is Closed For
 * Users ,Please Contact EccountBook ERP Support". The page therefore shows the same disabled buttons; the save
 * endpoint exists for parity with Insert() (Save right for a new row, Update right for RecId > 0) and reaches
 * the same procedure, which refuses an insert.
 */
@Service
public class ExportChartOfDocumentService {

    public static final int SCREEN_ID = 620;

    @Autowired private ExportDocumentTrackingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    /** DateLock_Load: RequiredLevelFill, DocumentProviderFill, BindGrid. */
    public Map<String, Object> setup() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.putAll(combos());
        out.put("history", history(u));
        return out;
    }

    /** refreshToolStripMenuItem_Click. */
    public Map<String, Object> combos() {
        user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rl = new ArrayList<>();
        for (Map<String, Object> r : repo.requiredLevels()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "requiredLevelId")));
            m.put("name", text(ci(r, "requiredLevelDesc")));
            rl.add(m);
        }
        out.put("requiredLevels", rl);
        List<Map<String, Object>> dp = new ArrayList<>();
        for (Map<String, Object> r : repo.documentProviders()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "DocumentProviderId")));
            m.put("name", text(ci(r, "DocumentProvider")));
            dp.add(m);
        }
        out.put("documentProviders", dp);
        return out;
    }

    /** BindGrid - the dt columns of the desktop, in its order. */
    public List<Map<String, Object>> history(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.chartOfDocumentHistory(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "chartOfDocumentId")));
            m.put("DocumentCode", text(ci(r, "documentCode")));
            m.put("DocumentName", text(ci(r, "documentName")));
            m.put("SequenceNo", asInt(ci(r, "seqNo")));
            m.put("RequiredLevel", text(ci(r, "RequiredLevel")));
            m.put("LeadTime", asInt(ci(r, "leadTime")));
            m.put("ProviderName", text(ci(r, "DocProviderName")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> history() { return history(user(ctx, rights, SCREEN_ID, "View")); }

    /** ReadById(Id) - ChartOfDocument.ReadById; [0] of an empty list throws on the desktop. */
    public Map<String, Object> readById(int id) {
        user(ctx, rights, SCREEN_ID, "View");
        List<Map<String, Object>> rows = repo.chartOfDocumentById(id);
        if (rows.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "chartOfDocumentId")));
        m.put("documentCode", text(ci(r, "documentCode")));
        m.put("documentName", text(ci(r, "documentName")));
        m.put("requiredLevelId", asInt(ci(r, "requiredLevelId")));
        m.put("DocumentProviderId", asInt(ci(r, "DocumentProviderId")));
        m.put("seqNo", asInt(ci(r, "seqNo")));
        m.put("leadTime", asInt(ci(r, "leadTime")));
        return m;
    }

    /** Insert(): the desktop's validations in order, then ChartOfDocument.save (all 14 model properties). */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(ctx, rights, SCREEN_ID, recId > 0 ? "Update" : "Save");
        if (text(body.get("documentCode")).isEmpty()) throw new IllegalArgumentException("ChartOfDocument Code Field is Required");
        if (text(body.get("documentName")).isEmpty()) throw new IllegalArgumentException("ChartOfDocument Name Field is Required");
        if (asInt(body.get("requiredLevelId")) == 0) throw new IllegalArgumentException("Type Field is Required");
        if (asInt(body.get("seqNo")) == 0) throw new IllegalArgumentException("Sequence No Field is Required");
        if (asInt(body.get("leadTime")) == 0) throw new IllegalArgumentException("LeadTime No Field is Required");
        Timestamp now = now();
        Map<String, Object> vd = new LinkedHashMap<>();
        vd.put("EntryDate", now);
        vd.put("ModifyDate", now);
        vd.put("chartOfDocumentId", recId);
        vd.put("CompanyId", u.getCompanyId());
        vd.put("leadTime", asInt(body.get("leadTime")));
        vd.put("EntryUserId", u.getId());
        vd.put("ModifyUserId", u.getId());
        vd.put("OrganizationId", u.getOrganizationId());
        vd.put("recSortNo", 0);
        vd.put("requiredLevelId", asInt(body.get("requiredLevelId")));
        vd.put("DocumentProviderId", asInt(body.get("DocumentProviderId")));
        vd.put("seqNo", asInt(body.get("seqNo")));
        vd.put("documentCode", text(body.get("documentCode")));
        vd.put("documentName", text(body.get("documentName")));
        int id = repo.saveChartOfDocument(vd);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully", id);
    }
}
