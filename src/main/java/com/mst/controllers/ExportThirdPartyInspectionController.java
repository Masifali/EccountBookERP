package com.mst.controllers;

import com.mst.services.ExportThirdPartyInspectionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Export (dbo.App 8), module 136:
 *
 *   /export/third-party-inspection               857  frmThirdPartyInspection             "Third Party Inspection"
 *   /export/lab-against-third-party-inspection   858  frmLabAgainstThirdPartyInspection   "Lab Against Third Party Inspection"
 *
 * Both pages share one API shape under /api/export/{third-party-inspection | lab-against-third-party-inspection}/...;
 * the path segment selects the screen (857 / 858) whose rights, ScreenName and validations apply.
 * No parameter carries tenancy or a user id; the service derives them from the session.
 */
@Controller
public class ExportThirdPartyInspectionController {

    @Autowired private ExportThirdPartyInspectionService svc;

    @GetMapping("/export/third-party-inspection")
    public String tpiPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/third_party_inspection";
    }

    @GetMapping("/export/lab-against-third-party-inspection")
    public String labPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/lab_against_third_party_inspection";
    }

    private static int screen(String seg) {
        if ("third-party-inspection".equals(seg)) return ExportThirdPartyInspectionService.SCREEN_TPI;
        if ("lab-against-third-party-inspection".equals(seg)) return ExportThirdPartyInspectionService.SCREEN_LAB;
        throw new IllegalArgumentException("Unknown screen");
    }

    private static final String API = "/api/export/{seg:third-party-inspection|lab-against-third-party-inspection}";

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup(@PathVariable("seg") String seg) { return call(() -> svc.setup(screen(seg)), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@PathVariable("seg") String seg) { return call(() -> svc.refresh(screen(seg)), "Load failed."); }

    @GetMapping(API + "/tracking-nos") @ResponseBody
    public ResponseEntity<?> trackingNos(@PathVariable("seg") String seg, @RequestParam(value = "status", defaultValue = "not") String status) {
        return call(() -> svc.trackingNos(screen(seg), status), "Load failed.");
    }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos(@PathVariable("seg") String seg) { return call(() -> svc.historyComboRefresh(screen(seg)), "Load failed."); }

    @GetMapping(API + "/contracts") @ResponseBody
    public ResponseEntity<?> contracts(@PathVariable("seg") String seg, @RequestParam("customerId") int customerId,
                                       @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.contracts(screen(seg), customerId, recId), "Load failed.");
    }

    @GetMapping(API + "/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@PathVariable("seg") String seg, @RequestParam("contractId") int contractId) {
        return call(() -> svc.invoices(screen(seg), contractId), "Load failed.");
    }

    @GetMapping(API + "/schedules") @ResponseBody
    public ResponseEntity<?> schedules(@PathVariable("seg") String seg, @RequestParam("contractId") int contractId,
                                       @RequestParam("customerId") int customerId) {
        return call(() -> svc.schedules(screen(seg), contractId, customerId), "Load failed.");
    }

    @GetMapping(API + "/contract-info") @ResponseBody
    public ResponseEntity<?> contractInfo(@PathVariable("seg") String seg, @RequestParam(value = "ids", defaultValue = "") String ids) {
        return call(() -> svc.contractInfo(screen(seg), ids), "Load failed.");
    }

    @GetMapping(API + "/group-parameters") @ResponseBody
    public ResponseEntity<?> groupParameters(@PathVariable("seg") String seg, @RequestParam("groupId") int groupId) {
        return call(() -> svc.groupParameters(screen(seg), groupId), "Load failed.");
    }

    @GetMapping(API + "/third-party-types") @ResponseBody
    public ResponseEntity<?> thirdPartyTypes(@PathVariable("seg") String seg, @RequestParam(value = "activeOnly", defaultValue = "false") boolean activeOnly) {
        return call(() -> svc.thirdPartyTypes(screen(seg), activeOnly), "Load failed.");
    }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@PathVariable("seg") String seg, @RequestParam("id") int id) {
        return call(() -> svc.readById(screen(seg), id), "Load failed.");
    }

    @GetMapping(API + "/history-detail") @ResponseBody
    public ResponseEntity<?> historyDetail(@PathVariable("seg") String seg, @RequestParam("id") int id) {
        return call(() -> svc.historyDetail(screen(seg), id), "Load failed.");
    }

    @GetMapping(API + "/attachments") @ResponseBody
    public ResponseEntity<?> attachments(@PathVariable("seg") String seg, @RequestParam("id") int id) {
        return call(() -> svc.attachments(screen(seg), id), "Load failed.");
    }

    @GetMapping(API + "/print-check") @ResponseBody
    public ResponseEntity<?> printCheck(@PathVariable("seg") String seg, @RequestParam("id") int id) {
        return call(() -> svc.printCheck(screen(seg), id), "No Record Found");
    }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@PathVariable("seg") String seg, @RequestBody Map<String, Object> body) {
        return call(() -> svc.history(screen(seg), body), "Load failed.");
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@PathVariable("seg") String seg, @RequestBody Map<String, Object> body) {
        return call(() -> svc.save(screen(seg), body), "Save failed.");
    }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

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
