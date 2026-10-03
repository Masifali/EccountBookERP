package com.mst.controllers;

import com.mst.services.ExportChartOfDocumentService;
import com.mst.services.ExportClientCustomGroupService;
import com.mst.services.ExportCustomGroupService;
import com.mst.services.ExportDocDueColorScheduleService;
import com.mst.services.ExportDocumentOfGroupService;
import com.mst.services.ExportDocumentTrackingReportService;
import com.mst.services.ExportShipmentDocScheduleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Module 130 "Export document Tracking" of the Export application (dbo.App 8) - the SDT (shipment
 * document tracking) forms of Architecture.WinApp.SDT / SDT_Reports:
 *
 *   /export/doc-due-color-schedule      619  frmDocDueAlertColorSchedule        "DocDue Color Schedule"
 *   /export/define-chart-of-document    620  DefineChartOfDocument              "Define Chart Of Document"
 *   /export/define-custom-group         621  DefineCustomGroup                  "Define Custom Group"
 *   /export/client-assign-to-group      622  frmClientCustomGroup               "Client Assign To Group"
 *   /export/document-assign-to-group    623  frmDocumentOfGroup                 "Document Assign To Group"
 *   /export/shipment-doc-schedule       624  frmShipmentDocumentSchedule        "Shipment Doc Schedule"
 *   /export/document-tracking-report    625  frmExportDocumentTrackingReport    "Export Document Tracking Report"
 *
 * No parameter carries tenancy or a user id; the services derive them from the session and check the
 * screen's rights through DesktopReportRights with the ScreenDefinition.Id above.
 */
@Controller
public class ExportDocumentTrackingController {

    private static final String CS = "/api/export/doc-due-color-schedule";
    private static final String COD = "/api/export/define-chart-of-document";
    private static final String CG = "/api/export/define-custom-group";
    private static final String CCG = "/api/export/client-assign-to-group";
    private static final String DOG = "/api/export/document-assign-to-group";
    private static final String SDS = "/api/export/shipment-doc-schedule";
    private static final String RPT = "/api/export/document-tracking-report";

    @Autowired private ExportDocDueColorScheduleService cs;
    @Autowired private ExportChartOfDocumentService cod;
    @Autowired private ExportCustomGroupService cg;
    @Autowired private ExportClientCustomGroupService ccg;
    @Autowired private ExportDocumentOfGroupService dog;
    @Autowired private ExportShipmentDocScheduleService sds;
    @Autowired private ExportDocumentTrackingReportService rpt;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/doc-due-color-schedule")
    public String colorSchedulePage(Model model) { model.addAttribute("activeMenu", "export"); return "export/doc_due_color_schedule"; }

    @GetMapping("/export/define-chart-of-document")
    public String chartOfDocumentPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/define_chart_of_document"; }

    @GetMapping("/export/define-custom-group")
    public String customGroupPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/define_custom_group"; }

    @GetMapping("/export/client-assign-to-group")
    public String clientAssignPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/client_assign_to_group"; }

    @GetMapping("/export/document-assign-to-group")
    public String documentAssignPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/document_assign_to_group"; }

    @GetMapping("/export/shipment-doc-schedule")
    public String shipmentDocSchedulePage(Model model) { model.addAttribute("activeMenu", "export"); return "export/shipment_doc_schedule"; }

    @GetMapping("/export/document-tracking-report")
    public String trackingReportPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/document_tracking_report"; }

    // ------------------------------------------------------------------ 619 DocDue Color Schedule

    @GetMapping(CS + "/setup") @ResponseBody
    public ResponseEntity<?> csSetup() { return call(() -> cs.setup(), "Error occurred during database call."); }

    @GetMapping(CS + "/refresh") @ResponseBody
    public ResponseEntity<?> csRefresh() { return call(() -> cs.refresh(), "Load failed."); }

    @GetMapping(CS + "/history-documents") @ResponseBody
    public ResponseEntity<?> csHistoryDocuments() { return call(() -> cs.historyDocuments(), "Load failed."); }

    @PostMapping(CS + "/history") @ResponseBody
    public ResponseEntity<?> csHistory(@RequestBody Map<String, Object> body) { return call(() -> cs.history(body), "Load failed."); }

    @PostMapping(CS + "/save") @ResponseBody
    public ResponseEntity<?> csSave(@RequestBody Map<String, Object> body) { return call(() -> cs.save(body), "Save failed."); }

    // ------------------------------------------------------------------ 620 Define Chart Of Document

    @GetMapping(COD + "/setup") @ResponseBody
    public ResponseEntity<?> codSetup() { return call(() -> cod.setup(), "Error occurred during database call."); }

    @GetMapping(COD + "/combos") @ResponseBody
    public ResponseEntity<?> codCombos() { return call(() -> cod.combos(), "Load failed."); }

    @GetMapping(COD + "/history") @ResponseBody
    public ResponseEntity<?> codHistory() { return call(() -> cod.history(), "Load failed."); }

    @GetMapping(COD + "/by-id") @ResponseBody
    public ResponseEntity<?> codById(@RequestParam("id") int id) { return call(() -> cod.readById(id), "Load failed."); }

    @PostMapping(COD + "/save") @ResponseBody
    public ResponseEntity<?> codSave(@RequestBody Map<String, Object> body) { return call(() -> cod.save(body), "Save failed."); }

    // ------------------------------------------------------------------ 621 Define Custom Group

    @GetMapping(CG + "/setup") @ResponseBody
    public ResponseEntity<?> cgSetup() { return call(() -> cg.setup(), "Error occurred during database call."); }

    @GetMapping(CG + "/history") @ResponseBody
    public ResponseEntity<?> cgHistory() { return call(() -> cg.history(), "Load failed."); }

    @GetMapping(CG + "/by-id") @ResponseBody
    public ResponseEntity<?> cgById(@RequestParam("id") int id) { return call(() -> cg.readById(id), "Load failed."); }

    @PostMapping(CG + "/save") @ResponseBody
    public ResponseEntity<?> cgSave(@RequestBody Map<String, Object> body) { return call(() -> cg.save(body), "Save failed."); }

    // ------------------------------------------------------------------ 622 Client Assign To Group

    @GetMapping(CCG + "/setup") @ResponseBody
    public ResponseEntity<?> ccgSetup() { return call(() -> ccg.setup(), "Error occurred during database call."); }

    @GetMapping(CCG + "/custom-groups") @ResponseBody
    public ResponseEntity<?> ccgCustomGroups() { return call(() -> ccg.customGroups(), "Load failed."); }

    @GetMapping(CCG + "/show") @ResponseBody
    public ResponseEntity<?> ccgShow(@RequestParam("customGroupId") int customGroupId) { return call(() -> ccg.show(customGroupId), "Load failed."); }

    @PostMapping(CCG + "/allocate") @ResponseBody
    public ResponseEntity<?> ccgAllocate(@RequestBody Map<String, Object> body) { return call(() -> ccg.allocate(body, true), "Save failed."); }

    @PostMapping(CCG + "/unallocate") @ResponseBody
    public ResponseEntity<?> ccgUnallocate(@RequestBody Map<String, Object> body) { return call(() -> ccg.allocate(body, false), "Save failed."); }

    // ------------------------------------------------------------------ 623 Document Assign To Group

    @GetMapping(DOG + "/setup") @ResponseBody
    public ResponseEntity<?> dogSetup() { return call(() -> dog.setup(), "Error occurred during database call."); }

    @GetMapping(DOG + "/refresh") @ResponseBody
    public ResponseEntity<?> dogRefresh() { return call(() -> dog.refresh(), "Load failed."); }

    @GetMapping(DOG + "/history-combos") @ResponseBody
    public ResponseEntity<?> dogHistoryCombos() { return call(() -> dog.historyCombos(), "Load failed."); }

    @GetMapping(DOG + "/show") @ResponseBody
    public ResponseEntity<?> dogShow(@RequestParam("customGroupId") int customGroupId,
                                     @RequestParam(value = "withLast", defaultValue = "true") boolean withLast) {
        return call(() -> dog.show(customGroupId, withLast), "Load failed.");
    }

    @PostMapping(DOG + "/save") @ResponseBody
    public ResponseEntity<?> dogSave(@RequestBody Map<String, Object> body) { return call(() -> dog.save(body), "Save failed."); }

    @PostMapping(DOG + "/history") @ResponseBody
    public ResponseEntity<?> dogHistory(@RequestBody Map<String, Object> body) { return call(() -> dog.history(body), "Load failed."); }

    // ------------------------------------------------------------------ 624 Shipment Doc Schedule

    @GetMapping(SDS + "/setup") @ResponseBody
    public ResponseEntity<?> sdsSetup() { return call(() -> sds.setup(), "Error occurred during database call."); }

    @GetMapping(SDS + "/schedules") @ResponseBody
    public ResponseEntity<?> sdsSchedules(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId,
                                          @RequestParam(value = "salesPersonId", defaultValue = "0") int salesPersonId,
                                          @RequestParam(value = "referedInSDS", defaultValue = "true") boolean referedInSDS) {
        return call(() -> sds.schedules(supplierCustomerId, salesPersonId, referedInSDS), "Load failed.");
    }

    @GetMapping(SDS + "/statuses") @ResponseBody
    public ResponseEntity<?> sdsStatuses() { return call(() -> sds.statuses(), "Load failed."); }

    @GetMapping(SDS + "/custom-groups") @ResponseBody
    public ResponseEntity<?> sdsCustomGroups(@RequestParam("supplierCustomerId") int supplierCustomerId) {
        return call(() -> sds.customGroups(supplierCustomerId), "Load failed.");
    }

    @GetMapping(SDS + "/show") @ResponseBody
    public ResponseEntity<?> sdsShow(@RequestParam("refDocId") int refDocId,
                                     @RequestParam("refDocumentTypeId") int refDocumentTypeId,
                                     @RequestParam("customGroupId") int customGroupId) {
        return call(() -> sds.show(refDocId, refDocumentTypeId, customGroupId), "Load failed.");
    }

    @PostMapping(SDS + "/allocate") @ResponseBody
    public ResponseEntity<?> sdsAllocate(@RequestBody Map<String, Object> body) { return call(() -> sds.allocate(body), "Save failed."); }

    @PostMapping(SDS + "/save") @ResponseBody
    public ResponseEntity<?> sdsSave(@RequestBody Map<String, Object> body) { return call(() -> sds.save(body), "Save failed."); }

    @PostMapping(SDS + "/unallocate") @ResponseBody
    public ResponseEntity<?> sdsUnallocate(@RequestBody Map<String, Object> body) { return call(() -> sds.unallocate(body), "Delete failed."); }

    // ------------------------------------------------------------------ 625 Export Document Tracking Report

    @GetMapping(RPT + "/setup") @ResponseBody
    public ResponseEntity<?> rptSetup() { return call(() -> rpt.setup(), "Error occurred during database call."); }

    @GetMapping(RPT + "/refresh") @ResponseBody
    public ResponseEntity<?> rptRefresh() { return call(() -> rpt.refresh(), "Load failed."); }

    @PostMapping(RPT + "/show") @ResponseBody
    public ResponseEntity<?> rptShow(@RequestBody Map<String, Object> body) { return call(() -> rpt.show(body), "Load failed."); }

    @PostMapping(RPT + "/update") @ResponseBody
    public ResponseEntity<?> rptUpdate(@RequestBody Map<String, Object> body) { return call(() -> rpt.update(body), "Update failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    /** 400 for the form's own validation text, 403 for a missing right, 500 with the innermost message otherwise. */
    private static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
