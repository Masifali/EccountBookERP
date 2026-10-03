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
 * 622 frmClientCustomGroup "Client Assign To Group" (Architecture.WinApp.SDT), ClientSize 802 x 619:
 * Custom Group combo (GetCustomGroupForCombo) + Show; left "Pending  For Allocation (InActive)" grid with a
 * header selector and the Allocate button (DoHaveSaveRight); right "Allocated (Active)" grid with the
 * UnAllocate button (DoHaveUpdateRights). Both buttons run Insert(grd, Active) - every checked row through
 * ClientCustomGroup.SaveAndUpdate with isActive = true / false.
 */
@Service
public class ExportClientCustomGroupService {

    public static final int SCREEN_ID = 622;

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
        out.put("customGroups", customGroups(u));
        return out;
    }

    /** CustomGroupBind / btnRefresh_Click: customGroupId / customGroupName. */
    public List<Map<String, Object>> customGroups(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.customGroupsForCombo(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "customGroupId")));
            m.put("name", text(ci(r, "customGroupName")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> customGroups() { return customGroups(user(ctx, rights, SCREEN_ID, "View")); }

    /** BtnShow_Click: both grids (Id, SupplierCustomerId, Suppliercustomer). */
    public Map<String, Object> show(int customGroupId) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        if (customGroupId <= 0) throw new IllegalArgumentException("Select Custom Group First");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("unallocated", gridRows(repo.clientUnallocated(u, customGroupId)));
        out.put("allocated", gridRows(repo.clientAllocated(u, customGroupId)));
        return out;
    }

    private static List<Map<String, Object>> gridRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("SupplierCustomerId", asInt(ci(r, "SuppliercustomerId")));
            m.put("Suppliercustomer", text(ci(r, "Suppliercustomer")));
            out.add(m);
        }
        return out;
    }

    /**
     * BtnAllocateItems_Click (active = true, Save right) / btnDeAllocate_Click (active = false, Update
     * right): custom group required, at least one checked row, then Insert(grd, Active).
     */
    public Map<String, Object> allocate(Map<String, Object> body, boolean active) {
        UserAccount u = user(ctx, rights, SCREEN_ID, active ? "Save" : "Update");
        int customGroupId = asInt(body.get("customGroupId"));
        if (customGroupId == 0) throw new IllegalArgumentException("Select Custom Group First");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException(active ? "Check Row's first To Allocate" : "Check Row's first To UnAllocate");
        Timestamp now = now();
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            /* Model ClientCustomGroup - the 10 non-virtual properties in declaration order. */
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("isActive", active);
            vd.put("EntryDate", now);
            vd.put("ModifyDate", now);
            vd.put("companyId", u.getCompanyId());
            vd.put("CustomGroupId", customGroupId);
            vd.put("EntryUserId", u.getId());
            vd.put("Id", asInt(r.get("Id")));
            vd.put("ModifyUserId", u.getId());
            vd.put("organizationId", u.getOrganizationId());
            vd.put("supplierCustomerId", asInt(r.get("SupplierCustomerId")));
            items.add(vd);
        }
        int id = repo.saveClientCustomGroup(items);
        return ok(active ? "Selected Record Allocated Successfully" : "Selected Record UnAllocated Successfully", id);
    }
}
