package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportDocumentTrackingRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportSdtSupport.*;

/**
 * 624 frmShipmentDocumentSchedule "Shipment Doc Schedule" (Architecture.WinApp.SDT), ClientSize 1184 x 561,
 * ScreenName frmShipmentDocumentSchedule.
 *
 * Header: Customer / Sales Person combos (GetDataForDropDownFromExportContract), Scheduled / Not Scheduled
 * radios (ReferedInSDS), Schedule / Invoice No combo (GetContactScheduleNoExistsInShipmentDocumentSchedule;
 * its hidden columns carry DocumentTypeId, SupplierCustomerId, LockScheduleForSDS, CustomGroupId), Custom
 * Group combo (GetCustomGroupAllocatedToCustomerBySupplierCustomerId, filled on the schedule's Leave), Lock
 * Schedule check, Show. Left: "Un-Allocated Document" (GetChartOfDocumentForShipmentScheduleByRefDocument,
 * mandatory rows pre-checked) with the Allocate button (DoHaveSaveRight -> AutoSaveByRefDocumentType of the
 * checked ids). Right: "Allocated Document" (ReadByRefDocument; DueDate = BaseCriteriaDate -/+ before/after
 * days; editable ReadyDate, DocStatus (SpStaticColumnNames), Remarks, DocumentProviderName) with the UnAllocate
 * button (DoHaveSaveRight -> DeleteByIds of the checked rows) and the toolbar "UpdateLockStatus" (Insert():
 * UpdateRefDocument + every row through USP_ShipmentDocumentSchedule_Insert).
 *
 * Desktop facts kept: the History tab is removed on Load (tabControl1.TabPages.Remove(tabPage3)) and
 * ComboBindForHistory is empty, so there is no history here; ReadyDate blank is bound as 1900-01-01
 * (Conversion.ToDateTime); the attachment popups (AttachmentForDetailRows / GetSaveAndDeleteAttachmentsForRecord)
 * are file dialogs of the desktop and are not ported - the AddAttachments count column is read from
 * DMSAttachments.GetByID and shown read-only.
 */
@Service
public class ExportShipmentDocScheduleService {

    public static final int SCREEN_ID = 624;
    public static final String SCREEN_NAME = "frmShipmentDocumentSchedule";

    @Autowired private ExportDocumentTrackingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    /** frmShipmentDocumentSchedule_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(rights, u, SCREEN_ID, "Save"));
        perm.put("Update", allowed(rights, u, SCREEN_ID, "Update"));
        out.put("permissions", perm);
        out.putAll(contractCombos(u));
        out.put("schedules", schedules(u, 0, 0, true));
        out.put("statuses", statuses());
        return out;
    }

    /** CombosFromContractBind: SalePerson / Customer rows of the export-contract drop-down. */
    private Map<String, Object> contractCombos(UserAccount u) {
        List<Map<String, Object>> rows = repo.exportContractDropDown(u);
        List<Map<String, Object>> sp = new ArrayList<>(), cu = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String t = text(ci(r, "ActivityType"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "name")));
            if ("SalePerson".equals(t)) sp.add(m);
            else if ("Customer".equals(t)) cu.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("salesPersons", sp);
        out.put("customers", cu);
        return out;
    }

    /** ScheduleNoComboDbCall / CmbCustomerContract_Leave / RadReferedRefDoc_CheckedChanged / Refresh. */
    public List<Map<String, Object>> schedules(int supplierCustomerId, int salesPersonId, boolean referedInSDS) {
        return schedules(user(ctx, rights, SCREEN_ID, "View"), supplierCustomerId, salesPersonId, referedInSDS);
    }

    private List<Map<String, Object>> schedules(UserAccount u, int supplierCustomerId, int salesPersonId, boolean referedInSDS) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.contractScheduleNos(u, supplierCustomerId, salesPersonId, referedInSDS)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ScheduleCode", text(ci(r, "ScheduleCode")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("LockScheduleForSDS", asBool(ci(r, "LockScheduleForSDS")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("LoadingDate", iso(ci(r, "LoadingDate")));
            m.put("CustomGroupId", asInt(ci(r, "CustomGroupId")));
            m.put("NoOfContainer", asDouble(ci(r, "NoOfContainer")));
            m.put("MTon", asDouble(ci(r, "MTon")));
            m.put("InvoiceId", asInt(ci(r, "InvoiceId")));
            m.put("InvoiceDocumentTypeId", asInt(ci(r, "InvoiceDocumentTypeId")));
            m.put("ExImLcOrderId", asInt(ci(r, "ExImLcOrderId")));
            m.put("ScheduleNo", text(ci(r, "ScheduleNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("InvoiceMTon", asDouble(ci(r, "InvoiceMTon")));
            m.put("IsForInvoiced", asBool(ci(r, "IsForInvoiced")));
            out.add(m);
        }
        return out;
    }

    /** StatusFill - Id / type. */
    public List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.scheduleStatuses()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("type", text(ci(r, "type")));
            out.add(m);
        }
        return out;
    }

    /** CustomGroupCombo(SupplierCustomerId, ..) on CmbScheduleNo_Leave. */
    public List<Map<String, Object>> customGroups(int supplierCustomerId) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.customGroupsForCustomer(u, supplierCustomerId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "CustomGroupId")));
            m.put("name", text(ci(r, "CustomGroup")));
            out.add(m);
        }
        return out;
    }

    /** ShowData: FillUnAssignData + ReadByRefDocumentTypeId (with the attachment counts). */
    public Map<String, Object> show(int refDocId, int refDocumentTypeId, int customGroupId) {
        user(ctx, rights, SCREEN_ID, "View");
        if (refDocId == 0) throw new IllegalArgumentException("Please Select Schedule First");
        if (customGroupId == 0) throw new IllegalArgumentException("Please Select CustomGroup First");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> un = new ArrayList<>();
        for (Map<String, Object> r : repo.sdsChartOfDocumentsForRefDocument(refDocId, refDocumentTypeId, customGroupId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("DocumentId", asInt(ci(r, "chartOfDocumentId")));
            m.put("Document", text(ci(r, "documentName")));
            m.put("RequiredLevelId", asInt(ci(r, "requiredLevelId")));
            m.put("RequiredLevel", text(ci(r, "RequiredLevel")));
            un.add(m);
        }
        out.put("unassigned", un);
        List<Map<String, Object>> rows = repo.sdsReadByRefDocument(refDocId, refDocumentTypeId, customGroupId);
        Map<Integer, Integer> counts = new LinkedHashMap<>();
        if (!rows.isEmpty()) {
            try {
                for (Map<String, Object> a : repo.dmsAttachmentsById(refDocId, SCREEN_NAME)) {
                    if (!asBool(ci(a, "DetailWiseAttachment"))) continue;
                    int line = asInt(ci(a, "LineId"));
                    counts.put(line, counts.getOrDefault(line, 0) + 1);
                }
            } catch (RuntimeException ignored) { /* the attachment store is optional here; the count stays 0 */ }
        }
        List<Map<String, Object>> as = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            LocalDate base = asDate(ci(r, "BaseCriteriaDate"));
            LocalDate due = base == null ? LocalDate.of(1900, 1, 1) : base;
            int before = asInt(ci(r, "beforeDays")), after = asInt(ci(r, "afterDays"));
            if (before > 0) due = due.minusDays(before);
            else if (after > 0) due = due.plusDays(after);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "shipmentDocumentScheduleId")));
            m.put("CustomGroupId", asInt(ci(r, "RefDocCustomGroupId")));
            m.put("CustomGroup", text(ci(r, "customGroupName")));
            m.put("DocumentId", asInt(ci(r, "chartOfDocumentId")));
            m.put("Document", text(ci(r, "documentName")));
            m.put("Original", asInt(ci(r, "original")));
            m.put("Duplicate", asInt(ci(r, "duplicate")));
            m.put("BaseCriteriaDate", iso(ci(r, "BaseCriteriaDate")));
            m.put("BeforeDays", before);
            m.put("AfterDays", after);
            m.put("DueDate", due.toString());
            m.put("ReadyDate", isNullDate(ci(r, "ReadyDate")) ? "" : iso(ci(r, "ReadyDate")));
            m.put("DocStatus", asInt(ci(r, "DocStatus")));
            m.put("PrioritySequence", asInt(ci(r, "prioritySeqNo")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("DocumentProviderName", text(ci(r, "DocumentProviderName")));
            m.put("AddAttachments", counts.getOrDefault(asInt(ci(r, "chartOfDocumentId")), 0));
            m.put("RefDocLockStatus", asBool(ci(r, "RefDocLockStatus")));
            as.add(m);
        }
        out.put("assigned", as);
        return out;
    }

    /** InsertAuto (BtnTransferToAssign / the Allocate toolbar button). */
    public Map<String, Object> allocate(Map<String, Object> body) {
        user(ctx, rights, SCREEN_ID, "Save");
        int refDocId = asInt(body.get("refDocId"));
        if (refDocId == 0) throw new IllegalArgumentException("ScheduleNo Field Required...");
        if (!asBool(body.get("hasRows"))) throw new IllegalArgumentException("Grid has no Record");
        int refDocumentTypeId = refDocId > 0 ? asInt(body.get("refDocumentTypeId")) : 0;
        List<Map<String, Object>> checked = list(body.get("rows"));
        if (checked.isEmpty()) throw new IllegalArgumentException("Please select at least one record.");
        List<String> ids = new ArrayList<>();
        for (Map<String, Object> r : checked) { String v = text(r.get("DocumentId")); if (!v.isEmpty()) ids.add(v); }
        if (ids.isEmpty()) throw new IllegalArgumentException("No valid IDs to Insert.");
        repo.sdsAutoInsert(refDocumentTypeId, refDocId, asInt(body.get("customGroupId")), String.join(",", ids));
        return ok("Save SuccessFully ", null);
    }

    /** Insert() - BtnUpdate "UpdateLockStatus": the desktop's validations, then ShipmentDocumentSchedule.Save. */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "Save");
        int refDocId = asInt(body.get("refDocId"));
        if (refDocId == 0) throw new IllegalArgumentException("ScheduleNo Field Required...");
        int customGroupId = asInt(body.get("customGroupId"));
        if (customGroupId == 0) throw new IllegalArgumentException("CustomGroup Field Required...");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found...");
        boolean update = false;
        for (Map<String, Object> r : rows) if (asInt(r.get("Id")) > 0) { update = true; break; }
        int refDocumentTypeId = asInt(body.get("refDocumentTypeId"));
        boolean lock = asBool(body.get("lockSchedule"));
        Timestamp now = now();
        List<Map<String, Object>> items = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : rows) {
            int n = i + 1;
            if (asInt(r.get("DocumentId")) == 0) throw new IllegalArgumentException("Chart Of Document Required in Row No:" + n);
            if (asInt(r.get("Original")) == 0) throw new IllegalArgumentException("Original Required in Row No:" + n);
            if (asInt(r.get("Duplicate")) == 0) throw new IllegalArgumentException("Duplicate Required in Row No:" + n);
            int before = asInt(r.get("BeforeDays")), after = asInt(r.get("AfterDays"));
            if (before == 0 && after == 0) throw new IllegalArgumentException("BeforeDays or AfterDays Required in Row No:" + n);
            if (before > 0 && after > 0) throw new IllegalArgumentException("BeforeDays or AfterDays Can Be Added,not both.please check in Row No:" + n);
            if (asInt(r.get("PrioritySequence")) == 0) throw new IllegalArgumentException("PrioritySequence Required in Row No:" + n);
            if (asInt(r.get("DocStatus")) == 0) throw new IllegalArgumentException("DocStatus Required in Row No:" + n);
            /* Model ShipmentDocumentSchedule - the 25 non-virtual properties in declaration order. */
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("IsApproved", false);
            vd.put("RefDocLockStatus", lock);
            vd.put("ApprovedDate", now);
            vd.put("BaseCriteriaDate", tsOrNullDate(r.get("BaseCriteriaDate")));
            vd.put("EntryDate", now);
            vd.put("ModifyDate", now);
            vd.put("DueDate", tsOrNullDate(r.get("DueDate")));
            vd.put("ReadyDate", tsOrNullDate(r.get("ReadyDate")));
            vd.put("DocStatus", asInt(r.get("DocStatus")));
            vd.put("afterDays", after);
            vd.put("ApprovedUserId", u.getId());
            vd.put("beforeDays", before);
            vd.put("chartOfDocumentId", asInt(r.get("DocumentId")));
            vd.put("CompanyId", u.getCompanyId());
            vd.put("duplicate", asInt(r.get("Duplicate")));
            vd.put("EntryUserId", u.getId());
            vd.put("ModifyUserId", u.getId());
            vd.put("OrganizationId", u.getOrganizationId());
            vd.put("original", asInt(r.get("Original")));
            vd.put("prioritySeqNo", asInt(r.get("PrioritySequence")));
            vd.put("refDocumentId", refDocId);
            vd.put("referenceDocumentTypeId", refDocumentTypeId);
            vd.put("shipmentDocumentScheduleId", asInt(r.get("Id")));
            vd.put("documentPath", "");
            vd.put("Remarks", text(r.get("Remarks")));
            items.add(vd);
            i++;
        }
        int id = repo.sdsSave(items, refDocumentTypeId, refDocId, customGroupId, lock);
        return ok(update ? "Update SuccessFully" : "Save SuccessFully ", id);
    }

    /** BtnDelete_Click (UnAllocate): the checked rows' Ids and DocumentIds through DeleteByIds. */
    public Map<String, Object> unallocate(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "Save");
        if (asInt(body.get("refDocId")) == 0) throw new IllegalArgumentException("ScheduleNo Field Required...");
        if (asInt(body.get("customGroupId")) == 0) throw new IllegalArgumentException("CustomGroup Field Required...");
        if (!asBool(body.get("hasRows"))) throw new IllegalArgumentException("Grid has no records.");
        List<Map<String, Object>> checked = list(body.get("rows"));
        if (checked.isEmpty()) throw new IllegalArgumentException("Please select at least one record.");
        List<String> ids = new ArrayList<>(), cods = new ArrayList<>();
        for (Map<String, Object> r : checked) {
            String v = text(r.get("Id")); if (!v.isEmpty()) ids.add(v);
            String c = text(r.get("DocumentId")); if (!c.isEmpty()) cods.add(c);
        }
        if (ids.isEmpty()) throw new IllegalArgumentException("No valid IDs to delete.");
        Integer entryUser = u.getEntryUserId() == null ? 0 : u.getEntryUserId();
        repo.sdsDeleteByIds(String.join(",", ids), String.join(",", cods), asInt(body.get("refDocumentTypeId")), entryUser);
        return ok("Delete Successfully", null);
    }
}
