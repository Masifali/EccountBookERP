package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportDhlTrackingRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.ExportShipmentFormsSupport.*;

/**
 * Architecture.WinApp.Export.frmDhlTracking "DHL Tracking" (ClientSize 963 x 533, DocumentTypeId 248). No ScreenDefinition
 * row: rights by the form name "frmDhlTracking" (ExportShipmentFormsSupport) - btnSave = Save, btnUpdate = Update.
 *
 * No external call: the desktop form has no DHL web API, URL or credential - it only keeps AWB numbers, dates and a
 * status in dbo.DHLTracking. Everything is ported server-side.
 *
 * Desktop behaviour reproduced, not corrected:
 *  Q1  Insert() never sets DHLTracking.ActionId (and the BLL does not either), so GenericProvider.SetProc sends
 *      @ActionId = 0. USP_DHLTracking_InsertAndUpdate only acts on ActionId 1 / 2 / 3, so neither a new record nor an
 *      update reaches the table, yet the form says "Record Save Successfully" / "Record Update Successfully". The web
 *      page sends the same 0 and shows the same message (the coordinator / user decides whether to change this).
 *  Q2  History always runs with CanViewAllRecord = true (BindGrid sets it), so @EntryUserId is never sent.
 *  Q3  The procedure filters @CurrentStatusId and @AWBBillNumber with "<=" (not "="), as written.
 *  Q4  Grid "X": refused while a record is open ("Please reset form first to delete"); no right is checked.
 * Not ported: attachments (Attachment dialog, "Add-Attachment" grid button) - the DMS store is not available here;
 * the NoOfAttachments column is shown.
 */
@Service
public class ExportDhlTrackingService {

    public static final String SCREEN_NAME = "frmDhlTracking";
    public static final int DOCUMENT_TYPE_ID = 248;

    @Autowired private ExportDhlTrackingRepository repo;
    @Autowired private SaleInvoiceRepository rightsRepo;
    @Autowired private CurrentUserContext ctx;

    /** DateLock_Load: rights, CurrentStatusFill, ComboBindHistory. */
    public Map<String, Object> setup() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights(rightsRepo, ctx, u, SCREEN_NAME));
        combos(u, out);
        return out;
    }

    /** btnRefresh_Click. */
    public Map<String, Object> refresh() {
        Map<String, Object> out = new LinkedHashMap<>();
        combos(ctx.requireAccountingUser(), out);
        return out;
    }

    private void combos(UserAccount u, Map<String, Object> out) {
        put(out, "statuses", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.currentStatuses(u)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("LookUpName", text(ci(r, "LookUpName")));
                rows.add(m);
            }
            return rows;
        });
        put(out, "historyCombos", () -> {
            List<Map<String, Object>> awb = new ArrayList<>(), cs = new ArrayList<>();
            for (Map<String, Object> r : repo.dropDowns(u)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", raw(ci(r, "ReferenceName")));
                String a = text(ci(r, "Activity"));
                if ("AWBBillNumber".equals(a)) awb.add(m);
                else if ("CurrentStatus".equals(a)) cs.add(m);
            }
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("awb", awb);
            m.put("status", cs);
            return m;
        });
    }

    /**
     * BindGrid: the checked pickers, the AWB text, the status. The grid columns are the form's projection; the page keeps
     * the filter it sent so the register print re-runs the same set.
     */
    public List<Map<String, Object>> history(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "CanViewAllRecord", 1);                       // Q2
        if (asDate(body.get("dispatchedFrom")) != null) p.put("DispatchedFromDate", pickerDate(body.get("dispatchedFrom")));
        if (asDate(body.get("dispatchedTo")) != null) p.put("DispatchedToDate", pickerDate(body.get("dispatchedTo")));
        if (asDate(body.get("etaFrom")) != null) p.put("FinalETAFromDate", pickerDate(body.get("etaFrom")));
        if (asDate(body.get("etaTo")) != null) p.put("FinalETAToDate", pickerDate(body.get("etaTo")));
        String awb = text(body.get("awbBillNo"));
        if (!awb.isEmpty()) p.put("AWBBillNumber", awb);
        int status = asInt(body.get("statusId"));
        if (status != 0) p.put("CurrentStatusId", status);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("DHLTrackingId", asLong(ci(r, "DHLTrackingId")));
            m.put("DateOfDispatched", iso(ci(r, "DateOfDispatched")));
            m.put("AWBBillNo", raw(ci(r, "AWBBillNumber")));
            m.put("FinalETA", iso(ci(r, "FinalETADate")));
            m.put("CurrentStatus", raw(ci(r, "CurrentStatus")));
            m.put("Detail", raw(ci(r, "Detail")));
            m.put("EntryUser", raw(ci(r, "EntryUser")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", raw(ci(r, "ModifyUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** grdfrm_DoubleClick: GetByID(RecId) -> the entry boxes (null -> nothing happens). */
    public Map<String, Object> readById(long id) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> r = repo.readById(id);
        if (r.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> d = r.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("DHLTrackingId", asLong(ci(d, "DHLTrackingId")));
        m.put("DateOfDispatched", iso(ci(d, "DateOfDispatched")));
        m.put("AWBBillNumber", raw(ci(d, "AWBBillNumber")));
        m.put("FinalETADate", iso(ci(d, "FinalETADate")));
        m.put("Detail", raw(ci(d, "Detail")));
        m.put("CurrentStatusId", asInt(ci(d, "CurrentStatusId")));
        return m;
    }

    /** Insert(): FormValidation (repeated), the model in its property order, DHLTracking.Save. */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        long recId = asLong(body.get("recId"));
        Map<String, Boolean> r = rights(rightsRepo, ctx, u, SCREEN_NAME);
        if (recId > 0) require(r, "Update", "You do not have the Update right for this screen.");
        else require(r, "Save", "You do not have the Save right for this screen.");
        String awb = raw(body.get("awbBillNo"));
        String detail = text(body.get("detail"));
        int status = asInt(body.get("currentStatusId"));
        if (awb.trim().isEmpty()) throw new IllegalArgumentException("AWB Bill No Field is Required");
        if (detail.isEmpty()) throw new IllegalArgumentException("Detail/Description Field is Required");
        if (status == 0) throw new IllegalArgumentException("Current Status Field is Required");
        Timestamp dispatched = pickerDate(body.get("dateOfDispatched"));
        Timestamp eta = pickerDate(body.get("finalEtaDate"));
        if (asDate(body.get("finalEtaDate")) != null && asDate(body.get("dateOfDispatched")) != null
                && asDate(body.get("finalEtaDate")).isBefore(asDate(body.get("dateOfDispatched"))))
            throw new IllegalArgumentException("ETA date Can't be less than Dispatched date");
        String attachments = "", customAttachments = "";
        if (recId > 0) {
            List<Map<String, Object>> cur = repo.readById(recId);
            if (!cur.isEmpty()) {
                attachments = raw(ci(cur.get(0), "AttachmentsValues"));
                customAttachments = raw(ci(cur.get(0), "CustomAttachmentsValues"));
            }
        }
        Timestamp now = now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("ApprovedDate", now);
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("DateOfDispatched", dispatched);
        m.put("FinalETADate", eta);
        m.put("EntryUserId", u.getId());
        m.put("ModifyUserId", u.getId());
        m.put("ApprovedUserId", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("CompanyId", u.getCompanyId());
        m.put("DHLTrackingId", recId);
        m.put("ActionId", 0);                                                                   // Q1
        m.put("CurrentStatusId", status);
        m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("PendingForView", 0);
        m.put("AWBBillNumber", awb);
        m.put("Detail", detail);
        m.put("AttachmentsValues", attachments);
        m.put("CustomAttachmentsValues", customAttachments);
        long id = repo.save(m);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully", id);
    }

    /** grdfrm_ColumnButtonClick "Delete": the page asks and refuses while a record is open (Q4). */
    public Map<String, Object> delete(long id, long openRecId) {
        UserAccount u = ctx.requireAccountingUser();
        if (openRecId == id || openRecId > 0) throw new IllegalArgumentException("Please reset form first to delete");
        repo.deleteById(id, u.getId());
        return ok("Record deleted successfully!", id);
    }
}
