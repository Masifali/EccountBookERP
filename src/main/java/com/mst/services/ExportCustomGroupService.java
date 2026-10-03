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
 * 621 DefineCustomGroup "Define Custom Group" (Architecture.WinApp.SDT), ClientSize 479 x 450:
 * Custom Group text + Active check (checked on load), Save (DoHaveSaveRight) / Update (DoHaveUpdateRights,
 * shown after an Edit), the History grid (Edit button column first, frozen) - CustomGroup BLL 0290.
 * The validation text is the desktop's own ("Crop Year Field Required" - copied from the crop-year form).
 */
@Service
public class ExportCustomGroupService {

    public static final int SCREEN_ID = 621;

    @Autowired private ExportDocumentTrackingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    public Map<String, Object> setup() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(rights, u, SCREEN_ID, "Save"));
        perm.put("Update", allowed(rights, u, SCREEN_ID, "Update"));
        out.put("permissions", perm);
        out.put("history", history(u));
        return out;
    }

    /** BindGrid: Id, CustomGroup, Active, EntryUser, EntryDate, ModifyUser, ModifyDate. */
    public List<Map<String, Object>> history(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.customGroupHistory(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "customGroupId")));
            m.put("CustomGroup", text(ci(r, "CustomGroupName")));
            m.put("Active", asBool(ci(r, "IsActive")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> history() { return history(user(ctx, rights, SCREEN_ID, "View")); }

    /** ReadbyId - CustomGroup.ReadById ([0] of the list). */
    public Map<String, Object> readById(int id) {
        user(ctx, rights, SCREEN_ID, "View");
        List<Map<String, Object>> rows = repo.customGroupById(id);
        if (rows.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "customGroupId")));
        m.put("customGroupName", text(ci(r, "customGroupName")));
        m.put("isActive", asBool(ci(r, "isActive")));
        return m;
    }

    /** Insert(): FormValidation, then CustomGroup.save with the 10 model properties. */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(ctx, rights, SCREEN_ID, recId > 0 ? "Update" : "Save");
        String name = text(body.get("customGroupName"));
        if (name.isEmpty()) throw new IllegalArgumentException("Crop Year Field Required");
        Timestamp now = now();
        Map<String, Object> vd = new LinkedHashMap<>();
        vd.put("isActive", asBool(body.get("isActive")));
        vd.put("EntryDate", now);
        vd.put("ModifyDate", now);
        vd.put("CompanyId", u.getCompanyId());
        vd.put("customGroupId", recId);
        vd.put("EntryUserId", u.getId());
        vd.put("ModifyUserId", u.getId());
        vd.put("OrganizationId", u.getOrganizationId());
        vd.put("sortNo", 0);
        vd.put("customGroupName", String.valueOf(body.get("customGroupName") == null ? "" : body.get("customGroupName")));
        int id = repo.saveCustomGroup(vd);
        return ok(recId > 0 ? "Record Update Successfully." : "Record Save Successfully..", id);
    }
}
