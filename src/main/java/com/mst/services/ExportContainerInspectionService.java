package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportContainerInspectionRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportFormSupport.*;

/**
 * BLL side of 205 GatePassInspection "Container Inspection" / "Vehicle Cargo Inspection Report"
 * (Architecture.WinApp.Export), ClientSize 959 x 661. tabControl1 "Form" | "History".
 * Rights: ScreenDefinition 205 - View, Save, Update; the 244 print has no right check on the desktop.
 *
 * One gate pass (DocumentTypeId 91, GatepassType 'Export') carries up to two ExImVCITransaction rows
 * (DocumentTypeId 180): "Container 01" (Container1No / Seal1No) and "Container 02" (Container2No / Seal2No).
 * Quirks kept: Save forces RecId = RecId2 = 0 (a loaded gate pass is inserted again, never updated by Save);
 * InsertDocSecond writes Container1No / Seal1No = the second container too; the Update button does not
 * confirm the second document; ApprovedUserId is never set (0).
 */
@Service
public class ExportContainerInspectionService {

    public static final int SCREEN_ID = 205;
    public static final int DOCUMENT_TYPE_ID = 180;

    @Autowired private ExportContainerInspectionRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** GatePassInspection_Load: rights, Gatepassfill(0), grdDocFirstfill / grdDocSecondfill (the parameter list). */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        out.put("permissions", perm);
        put(out, "gatePasses", () -> gatePasses(u, 0));
        put(out, "parameters", () -> parameterRows(u));
        return out;
    }

    /** Gatepassfill(Id): Id, GpSrNo, GpDate, VehicleNo, BiltyNo, Status (DocumentTypeId hidden). */
    public List<Map<String, Object>> gatePasses(UserAccount u, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gatePasses(u, currentUserContext.currentFinancialYearId(), id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("GpSrNo", text(ci(r, "GpSrNo")));
            m.put("GpDate", iso(ci(r, "GpDate")));
            m.put("VehicleNo", text(ci(r, "VehicleNo")));
            m.put("BiltyNo", text(ci(r, "BiltyNo")));
            m.put("Status", text(ci(r, "Status")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> gatePasses(int id) { return gatePasses(user("View"), id); }

    /** grdDocFirstfill / grdDocSecondfill rows: Id 0, VCIParameterId, CategoryId, Category, InspectionPoint, Status false, Remarks "". */
    private List<Map<String, Object>> parameterRows(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.parameters(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("VCIParameterId", asInt(ci(r, "VCIParameterId")));
            m.put("CategoryId", asInt(ci(r, "ExImVCICartegoryId")));
            m.put("Category", text(ci(r, "VciCategoryDescription")));
            m.put("InspectionPoint", text(ci(r, "VciParameterDescription")));
            m.put("Status", false);
            m.put("Remarks", "");
            out.add(m);
        }
        return out;
    }

    /**
     * cmbgatepass_Leave -> ReadById(GatePassId): the (up to two) ExImVCITransaction rows of the gate pass with
     * their detail grids (GetByID -> ReadDetailByHeaderId); when none exist, the parameter list and the gate
     * pass's Container / Container1 (GetcontainserNoFromGp).
     */
    public Map<String, Object> readByGatePass(int gatePassId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> list = repo.byGatePassId(u, gatePassId);
        out.put("found", !list.isEmpty());
        List<Map<String, Object>> params = parameterRows(u);
        if (!list.isEmpty()) {
            out.put("doc1", doc(list.get(0), 1, params));
            if (list.size() > 1) out.put("doc2", doc(list.get(1), 2, params));
            else out.put("doc2", emptyDoc(params));
        } else {
            Map<String, Object> d1 = emptyDoc(params), d2 = emptyDoc(params);
            if (gatePassId > 0) {
                List<Map<String, Object>> gp = repo.gatePassById(gatePassId);
                if (!gp.isEmpty()) {
                    d1.put("ContainerNo", text(ci(gp.get(0), "Container")));
                    d2.put("ContainerNo", text(ci(gp.get(0), "Container1")));
                }
            }
            out.put("doc1", d1);
            out.put("doc2", d2);
        }
        return out;
    }

    private Map<String, Object> emptyDoc(List<Map<String, Object>> params) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("RecId", 0);
        m.put("rows", copy(params));
        return m;
    }

    private Map<String, Object> doc(Map<String, Object> r, int which, List<Map<String, Object>> params) {
        Map<String, Object> m = new LinkedHashMap<>();
        int id = asInt(ci(r, "Id"));
        m.put("RecId", id);
        m.put("ContainerNo", text(ci(r, which == 1 ? "Container1No" : "Container2No")));
        m.put("SealNo", text(ci(r, which == 1 ? "Seal1No" : "Seal2No")));
        m.put("InspectedBy", text(ci(r, "InspectedBy")));
        m.put("VerifiedBy", text(ci(r, "VerifiedBy")));
        m.put("PictureTakenBy", text(ci(r, "PictureTakenBy")));
        m.put("InspectorRemarks", text(ci(r, "InspectorRemarks")));
        m.put("IsContainerAccepted", asBool(ci(r, "IsContainerAccepted")));
        List<Map<String, Object>> rows = new ArrayList<>();
        if (id != 0) {
            for (Map<String, Object> d : repo.detailsByHeaderId(id)) rows.add(detailRow(d));
        }
        m.put("rows", rows.isEmpty() && id == 0 ? copy(params) : rows);
        return m;
    }

    private static Map<String, Object> detailRow(Map<String, Object> d) {
        Map<String, Object> x = new LinkedHashMap<>();
        x.put("Id", asInt(ci(d, "Id")));
        x.put("VCIParameterId", asInt(ci(d, "VCIParameter")));
        x.put("CategoryId", asInt(ci(d, "VCICategoryId")));
        x.put("Category", text(ci(d, "VciCategoryDescription")));
        x.put("InspectionPoint", text(ci(d, "VciParameterDescription")));
        x.put("Status", asInt(ci(d, "InspectionStatus")) == 1);
        x.put("Remarks", text(ci(d, "Remarks")));
        return x;
    }

    /** FillHistoryDetailByGpId: the same read, only rows with InspectionStatus == 1 in the grids. */
    public Map<String, Object> historyDetail(int gatePassId) {
        Map<String, Object> out = readByGatePass(gatePassId);
        for (String k : new String[] { "doc1", "doc2" }) {
            @SuppressWarnings("unchecked") Map<String, Object> d = (Map<String, Object>) out.get(k);
            if (d == null) continue;
            @SuppressWarnings("unchecked") List<Map<String, Object>> rows = (List<Map<String, Object>>) d.get("rows");
            List<Map<String, Object>> kept = new ArrayList<>();
            if (asInt(d.get("RecId")) != 0) for (Map<String, Object> r : rows) if (flag(r.get("Status"))) kept.add(r);
            d.put("rows", kept);
        }
        if (!flag(out.get("found"))) { out.put("doc1", emptyDoc(new ArrayList<>())); out.put("doc2", emptyDoc(new ArrayList<>())); }
        return out;
    }

    /** FormHistory: GatePassInformationForHistory with the ticked dates and the GP number range. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user("View");
        java.sql.Date from = flag(b.get("fromChecked")) ? sqlDate(asDate(b.get("fromDate"))) : null;
        java.sql.Date to = flag(b.get("toChecked")) ? sqlDate(asDate(b.get("toDate"))) : null;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, from, to, asInt(b.get("fromDocNo")), asInt(b.get("toDocNo")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("GatePassOutwardId", asInt(ci(r, "GatePassOutwardId")));
            m.put("GpDate", iso(ci(r, "GpDate")));
            m.put("GpSrNo", asInt(ci(r, "GpSrNo")));
            m.put("VehicleNo", text(ci(r, "VehicleNo")));
            m.put("FactoryWeight", asDouble(ci(r, "FactoryWeight")));
            m.put("Container1No", text(ci(r, "Container1No")));
            m.put("Container2No", text(ci(r, "Container2No")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            out.add(m);
        }
        return out;
    }

    /**
     * btnsave_Click / btnUpdate_Click: "Gate Pass No Field is Required"; each container with a number goes
     * through FormValidationDoc (Inspected By / Verified By / Picture Taken By, at least one checked row when
     * the grid has rows); the validated documents are saved (Save with Id 0, Update with the loaded ids);
     * nothing validated -> "Container No field is Required..." / "Container No fields is Required...".
     */
    public Map<String, Object> save(Map<String, Object> b) {
        boolean update = flag(b.get("update"));
        UserAccount u = user(update ? "Update" : "Save");
        int gatePassId = asInt(b.get("gatePassId"));
        if (gatePassId == 0) throw new IllegalArgumentException("Gate Pass No Field is Required");
        @SuppressWarnings("unchecked") Map<String, Object> d1 = b.get("doc1") instanceof Map ? (Map<String, Object>) b.get("doc1") : new LinkedHashMap<>();
        @SuppressWarnings("unchecked") Map<String, Object> d2 = b.get("doc2") instanceof Map ? (Map<String, Object>) b.get("doc2") : new LinkedHashMap<>();
        boolean v1 = false, v2 = false;
        if (!str(d1.get("ContainerNo")).isEmpty()) { validateDoc(d1, "01"); v1 = true; }
        if (!str(d2.get("ContainerNo")).isEmpty()) { validateDoc(d2, "02"); v2 = true; }
        int saved = 0;
        if (v1) { repo.save(header(u, gatePassId, d1, 1, update ? asInt(d1.get("RecId")) : 0), details(d1)); saved++; }
        if (v2) { repo.save(header(u, gatePassId, d2, 2, update ? asInt(d2.get("RecId")) : 0), details(d2)); saved++; }
        if (saved == 0) throw new IllegalArgumentException(update ? "Container No fields is Required..." : "Container No field is Required...");
        return saved(saved, update ? "Record Updated Successfully..." : "Record Save Successfully...");
    }

    private static void validateDoc(Map<String, Object> d, String no) {
        if (str(d.get("InspectedBy")).isEmpty()) throw new IllegalArgumentException("Inspected By Field is Required");
        if (str(d.get("VerifiedBy")).isEmpty()) throw new IllegalArgumentException("Verified By Field is Required");
        if (str(d.get("PictureTakenBy")).isEmpty()) throw new IllegalArgumentException("Picture Taken By Field is Required");
        List<Map<String, Object>> rows = list(d.get("rows"));
        if (!rows.isEmpty()) {
            boolean any = false;
            for (Map<String, Object> r : rows) if (flag(r.get("Status"))) any = true;
            if (!any) throw new IllegalArgumentException("AtLeast one Row Should be Checked in Grid " + no);
        }
    }

    /** InsertDocFirst / InsertDocSecond - the ExImVCITransaction model in SetProc order. */
    private static Map<String, Object> header(UserAccount u, int gatePassId, Map<String, Object> d, int which, int id) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("IsContainerAccepted", flag(d.get("IsContainerAccepted")));
        m.put("ApprovedDate", now);
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("ApprovedUserId", 0);
        m.put("BranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        m.put("CompanyId", u.getCompanyId());
        m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("EntryUserId", u.getId());
        m.put("GatePassOutwardId", gatePassId);
        m.put("Id", id);
        m.put("ModifyUserId", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        String c = str(d.get("ContainerNo")), s = str(d.get("SealNo"));
        m.put("Container1No", c);                              // doc 2 writes its container into Container1No too (desktop)
        m.put("Container2No", which == 2 ? c : null);
        m.put("InspectedBy", str(d.get("InspectedBy")));
        m.put("InspectorRemarks", str(d.get("InspectorRemarks")));
        m.put("PictureTakenBy", str(d.get("PictureTakenBy")));
        m.put("Seal1No", s);
        m.put("Seal2No", which == 2 ? s : null);
        m.put("VerifiedBy", str(d.get("VerifiedBy")));
        return m;
    }

    private static List<Map<String, Object>> details(Map<String, Object> d) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : list(d.get("rows"))) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("ExImVCITransactionId", 0);
            x.put("Id", asInt(r.get("Id")));
            x.put("InspectionStatus", flag(r.get("Status")) ? 1 : 0);
            x.put("VCICategoryId", asInt(r.get("CategoryId")));
            x.put("VCIParameter", asInt(r.get("VCIParameterId")));
            x.put("Remarks", text(r.get("Remarks")));
            out.add(x);
        }
        return out;
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
