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
 * 625 frmExportDocumentTrackingReport "Export Document Tracking Report" (Architecture.WinApp.SDT_Reports),
 * ClientSize 1234 x 561.
 *
 * Filters: Date Type (CommonServices.DateType, fixed), From / To (ticked pickers), Ready / Due radio, Customer
 * Name / Invoice / Document / Provider / Sales Person combos (DropDownFillFromShipmentDocumentSchedule),
 * All / Ready Docs / Not Ready Docs, All Invoice / Invoice Done / Invoice Pending, the Due Date Alert level
 * checks, "Show Records". Tabs: Detail (USP_ExportDocumentTrackingReport) and Group Wise (View I =
 * ...ReportI, View II = ...ReportII), both group-wise grids editable in ReadyDate / DocumentStatus with a
 * Save right and an Update button each (Insert(grd) -> UpdateDocStatusAndReadyDate of the changed rows), plus
 * the "Values To Update In Grid" box (Save right) that fills the visible grid client-side.
 *
 * Desktop facts kept: the 841 / 841A print buttons are hidden and their handlers are empty (no print);
 * tabControl1 opens on Group Wise; the level check boxes' text and colour come from the first row of each
 * level; the counts count every row of the raw result, the grid keeps only the checked levels (none checked
 * = all).
 */
@Service
public class ExportDocumentTrackingReportService {

    public static final int SCREEN_ID = 625;

    @Autowired private ExportDocumentTrackingRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    /** frmExportDocumentTrackingReport_Load: rights, ParameterFill, ComboFill, StatusFill. */
    public Map<String, Object> setup() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(rights, u, SCREEN_ID, "Save"));
        out.put("permissions", perm);
        out.put("dateTypes", dateTypes());
        out.put("financialYearStart", iso(repo.financialYearStart(u, ctx.currentFinancialYearId())));
        out.putAll(combos(u));
        out.put("statuses", statuses());
        return out;
    }

    /** btnRefresh_Click: ComboFill + StatusFill. */
    public Map<String, Object> refresh() {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.putAll(combos(u));
        out.put("statuses", statuses());
        return out;
    }

    private Map<String, Object> combos(UserAccount u) {
        List<Map<String, Object>> rows = repo.sdsDropDown(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", pick(rows, "Customer"));
        out.put("invoices", pick(rows, "InvoiceNo"));
        out.put("documents", pick(rows, "chartOfDocument"));
        out.put("providers", pick(rows, "documentProvider"));
        out.put("salesPersons", pick(rows, "SalesPerson"));
        return out;
    }

    private List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.scheduleStatuses()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("type", text(ci(r, "type")));
            out.add(m);
        }
        return out;
    }

    /**
     * FillGrids: the ReportsParameters of the desktop -> the BLL's parameter list (a value is sent only when
     * the form set it). which = "detail" (tab Detail) or "group" (both group-wise grids).
     */
    public Map<String, Object> show(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "View");
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        boolean due = "due".equals(text(body.get("dateMode")));
        java.sql.Date from = asBool(body.get("fromChecked")) ? sqlDate(body.get("fromDate")) : null;
        java.sql.Date to = asBool(body.get("toChecked")) ? sqlDate(body.get("toDate")) : null;
        if (from != null) p.put(due ? "DueFrom" : "ReadyDateFrom", from);
        if (to != null) p.put(due ? "DueTo" : "ReadyDateTo", to);
        int provider = asInt(body.get("documentProviderId")), cod = asInt(body.get("chartOfDocumentId")),
            cust = asInt(body.get("supplierCustomerId")), sales = asInt(body.get("salesPersonId")), inv = asInt(body.get("exImInvoiceId")),
            skip = asInt(body.get("readyDocsOnly")), pending = asInt(body.get("pendingInvoice"));
        if (provider != 0) p.put("DocumentProviderId", provider);
        if (cod != 0) p.put("chartOfDocumentId", cod);
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (sales != 0) p.put("SalesPersonId", sales);
        if (inv != 0) p.put("ExImInvoiceId", inv);
        if (skip != 0) p.put("ReadyDocsOnly", skip);
        if (pending != 0) p.put("PendingInvoice", pending);
        Map<String, Object> out = new LinkedHashMap<>();
        if ("detail".equals(text(body.get("which")))) {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.trackingReport("", p)) rows.add(detailRow(r));
            out.put("detail", rows);
        } else {
            List<Map<String, Object>> g1 = new ArrayList<>();
            for (Map<String, Object> r : repo.trackingReport("I", new LinkedHashMap<>(p))) g1.add(groupRowI(r));
            out.put("gridI", g1);
            List<Map<String, Object>> g2 = new ArrayList<>();
            for (Map<String, Object> r : repo.trackingReport("II", new LinkedHashMap<>(p))) g2.add(groupRowII(r));
            out.put("gridII", g2);
        }
        return out;
    }

    private static void level(Map<String, Object> m, Map<String, Object> r) {
        m.put("LevelId", asInt(ci(r, "LevelId")));
        m.put("LevelName", text(ci(r, "LevelName")));
        m.put("ColorCode", text(ci(r, "ColorCode")));
        m.put("DueDateDiffDays", asInt(ci(r, "DueDateDiffDays")));
    }

    private static String shortOrBlank(Object v) { return isNullDate(v) ? "" : iso(v); }

    /** GridTemplateIFill's dtcol (View II on the desktop - grd). */
    private static Map<String, Object> groupRowI(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("ShipmentDocumentScheduleId", asInt(ci(r, "shipmentDocumentScheduleId")));
        m.put("Description", text(ci(r, "Description")));
        m.put("ChartOfDocumentId", asInt(ci(r, "chartOfDocumentId")));
        m.put("DocumentName", text(ci(r, "documentName")));
        m.put("ProviderName", text(ci(r, "DocProviderName")));
        m.put("DueDate", shortOrBlank(ci(r, "dueDate")));
        m.put("ReadyDate", shortOrBlank(ci(r, "ReadyDate")));
        m.put("DocumentStatus", asInt(ci(r, "DocStatus")));
        m.put("Remarks", text(ci(r, "Remarks")));
        m.put("PreviousReadyDate", shortOrBlank(ci(r, "ReadyDate")));
        m.put("PreviousDocumentStatus", asInt(ci(r, "DocStatus")));
        level(m, r);
        return m;
    }

    /** GridTemplateIIFill's dtcol (View I on the desktop - grdII). */
    private static Map<String, Object> groupRowII(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("ShipmentDocumentScheduleId", asInt(ci(r, "shipmentDocumentScheduleId")));
        m.put("Description", text(ci(r, "Description")));
        m.put("CustomerName", text(ci(r, "CustomerName")));
        m.put("ScheduleCode", text(ci(r, "ScheduleCode")));
        m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
        m.put("InvoiceDate", shortOrBlank(ci(r, "InvoiceDate")));
        m.put("DueDate", shortOrBlank(ci(r, "dueDate")));
        m.put("ReadyDate", shortOrBlank(ci(r, "ReadyDate")));
        m.put("DocumentStatus", asInt(ci(r, "DocStatus")));
        m.put("Remarks", text(ci(r, "Remarks")));
        m.put("PreviousReadyDate", shortOrBlank(ci(r, "ReadyDate")));
        m.put("PreviousDocumentStatus", asInt(ci(r, "DocStatus")));
        level(m, r);
        return m;
    }

    /** GridDetailFill's dtcol. */
    private static Map<String, Object> detailRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("ShipmentDocumentScheduleId", asInt(ci(r, "shipmentDocumentScheduleId")));
        m.put("referenceDocumentTypeId", asInt(ci(r, "referenceDocumentTypeId")));
        m.put("CustomerName", text(ci(r, "CustomerName")));
        m.put("refDocumentId", asInt(ci(r, "refDocumentId")));
        m.put("ScheduleCode", text(ci(r, "ScheduleCode")));
        m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
        m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
        m.put("InvoiceDate", shortOrBlank(ci(r, "InvoiceDate")));
        m.put("chartOfDocumentId", asInt(ci(r, "chartOfDocumentId")));
        m.put("DocumentName", text(ci(r, "documentName")));
        m.put("ProviderName", text(ci(r, "DocProviderName")));
        m.put("DueDate", shortOrBlank(ci(r, "dueDate")));
        m.put("ReadyDate", shortOrBlank(ci(r, "ReadyDate")));
        m.put("DocumentStatus", text(ci(r, "DocStatus")));
        m.put("Remarks", text(ci(r, "Remarks")));
        m.put("Original", asInt(ci(r, "original")));
        m.put("Duplicate", asInt(ci(r, "duplicate")));
        m.put("CriteriaDate", shortOrBlank(ci(r, "criteriaDate")));
        m.put("BeforeDays", asInt(ci(r, "beforeDays")));
        m.put("AfterDays", asInt(ci(r, "afterDays")));
        m.put("Contract", text(ci(r, "LC_Contract")));
        m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
        level(m, r);
        return m;
    }

    /**
     * Insert(grd): the page sends the rows whose DocumentStatus or ReadyDate differ from their Previous*
     * values (the same comparison is repeated here); none -> "No record Found For Updation"; ReadyDate is
     * sent only when not the null date (else the model default 0001-01-01 is what AddWithValue binds - here
     * the 1900-01-01 the DATE parameter accepts, since a DateTime.MinValue overflows SQL DATETIME on the
     * desktop too and the procedure's parameter is DATE).
     */
    public Map<String, Object> update(Map<String, Object> body) {
        UserAccount u = user(ctx, rights, SCREEN_ID, "Save");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found...");
        List<Map<String, Object>> items = new ArrayList<>();
        Timestamp now = now();
        for (Map<String, Object> r : rows) {
            boolean statusChanged = asInt(r.get("DocumentStatus")) != asInt(r.get("PreviousDocumentStatus"));
            boolean dateChanged = !tsOrNullDate(r.get("ReadyDate")).equals(tsOrNullDate(r.get("PreviousReadyDate")));
            if (!statusChanged && !dateChanged) continue;
            int id = asInt(r.get("ShipmentDocumentScheduleId"));
            if (id <= 0) continue;
            Map<String, Object> it = new LinkedHashMap<>();
            it.put("Id", id);
            it.put("DocStatus", asInt(r.get("DocumentStatus")));
            it.put("ReadyDate", isNullDate(r.get("ReadyDate")) ? java.sql.Date.valueOf("1900-01-01") : java.sql.Date.valueOf(asDate(r.get("ReadyDate"))));
            it.put("ModifyDate", now);
            it.put("ModifyUserId", u.getId());
            items.add(it);
        }
        if (items.isEmpty()) throw new IllegalArgumentException("No record Found For Updation");
        repo.sdsUpdateDocStatusAndReadyDate(items);
        return ok("Update SuccessFully", null);
    }
}
