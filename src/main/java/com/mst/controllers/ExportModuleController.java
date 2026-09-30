package com.mst.controllers;

import com.mst.services.ExportGdBreakUpService;
import com.mst.services.ExportInvoicePackingListService;
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
 * The Export application (dbo.App Id 8 "Export"). For this company (GOLDEN ACE FOODS, CompanyId 78)
 * the desktop menu shows exactly two screens - the only two rows of AppModules 11 "Export" that
 * CompanyRights activates and tblUserRights grants (GoldenAceDb(0509)t.sql):
 *
 *   /export/gd-break-up-by-invoice   881  frmGdBreakUpByInvoice        "Gd Break Up By Invoice"
 *   /export/invoice-packing-list     882  frmExportInvoicePackingList  "Export Invoice Packing List"
 *
 * The hub itself is /export (AppMenuController.export - the generic App/Module/Screen renderer), which
 * links each screen row through DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID by the row's own Id.
 * The other 28 rows of module 11 (and modules 17, 98-100, 130, 133, 2047) are not enabled for this
 * company and are not built; the hub shows them as not built if a company ever enables them.
 *
 * No parameter carries tenancy or a user id; the services derive them from the session.
 */
@Controller
public class ExportModuleController {

    private static final String GD = "/api/export/gd-break-up";
    private static final String PL = "/api/export/packing-list";

    @Autowired private ExportGdBreakUpService gd;
    @Autowired private ExportInvoicePackingListService pl;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/gd-break-up-by-invoice")
    public String gdBreakUpPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/gd_break_up_by_invoice";
    }

    @GetMapping("/export/invoice-packing-list")
    public String packingListPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/invoice_packing_list";
    }

    // ------------------------------------------------------------------ 881 Gd Break Up By Invoice

    @GetMapping(GD + "/setup") @ResponseBody
    public ResponseEntity<?> gdSetup() { return call(() -> gd.setup(), "Error occurred during database call."); }

    @GetMapping(GD + "/refresh") @ResponseBody
    public ResponseEntity<?> gdRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> gd.refreshGdBreakUp(recId), "Load failed.");
    }

    @GetMapping(GD + "/invoices") @ResponseBody
    public ResponseEntity<?> gdInvoices(@RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> gd.invoices(recId), "Load failed.");
    }

    @GetMapping(GD + "/by-invoice") @ResponseBody
    public ResponseEntity<?> gdByInvoice(@RequestParam("invoiceId") int invoiceId) {
        return call(() -> gd.byInvoice(invoiceId), "Load failed.");
    }

    @GetMapping(GD + "/history") @ResponseBody
    public ResponseEntity<?> gdHistory(@RequestParam(value = "fromDate", required = false) String fromDate,
                                       @RequestParam(value = "toDate", required = false) String toDate,
                                       @RequestParam(value = "bankId", defaultValue = "0") int bankId) {
        return call(() -> gd.gdHistory(fromDate, toDate, bankId), "Load failed.");
    }

    @GetMapping(GD + "/history-banks") @ResponseBody
    public ResponseEntity<?> gdHistoryBanks() { return call(() -> gd.historyBanks(), "Load failed."); }

    @PostMapping(GD + "/save") @ResponseBody
    public ResponseEntity<?> gdSave(@RequestBody Map<String, Object> body) { return call(() -> gd.saveGdBreakUp(body), "Save failed."); }

    @GetMapping(GD + "/advance/setup") @ResponseBody
    public ResponseEntity<?> advSetup() { return call(() -> gd.advanceSetup(), "Load failed."); }

    @GetMapping(GD + "/advance/by-invoice") @ResponseBody
    public ResponseEntity<?> advByInvoice(@RequestParam("invoiceId") int invoiceId,
                                          @RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId) {
        return call(() -> gd.advanceByInvoice(invoiceId, documentTypeId), "Load failed.");
    }

    @GetMapping(GD + "/advance/history") @ResponseBody
    public ResponseEntity<?> advHistory() { return call(() -> gd.advanceHistory(), "Load failed."); }

    @PostMapping(GD + "/advance/save") @ResponseBody
    public ResponseEntity<?> advSave(@RequestBody Map<String, Object> body) { return call(() -> gd.saveAdvanceUtilize(body), "Save failed."); }

    // ------------------------------------------------------------------ 882 Export Invoice Packing List

    @GetMapping(PL + "/setup") @ResponseBody
    public ResponseEntity<?> plSetup() { return call(() -> pl.setup(), "Error occurred during database call."); }

    @GetMapping(PL + "/refresh") @ResponseBody
    public ResponseEntity<?> plRefresh() { return call(() -> pl.refresh(), "Load failed."); }

    @GetMapping(PL + "/history-combos") @ResponseBody
    public ResponseEntity<?> plHistoryCombos() { return call(() -> pl.historyComboRefresh(), "Load failed."); }

    @GetMapping(PL + "/by-id") @ResponseBody
    public ResponseEntity<?> plById(@RequestParam("id") int id,
                                    @RequestParam(value = "history", defaultValue = "false") boolean history) {
        return call(() -> pl.readById(id, history), "Load failed.");
    }

    @GetMapping(PL + "/detail") @ResponseBody
    public ResponseEntity<?> plDetail(@RequestParam("id") int id) { return call(() -> pl.packingListDetail(id), "Load failed."); }

    @GetMapping(PL + "/commodity-remarks") @ResponseBody
    public ResponseEntity<?> plRemarks(@RequestParam("itemId") int itemId,
                                       @RequestParam("detailTypeId") int detailTypeId) {
        return call(() -> pl.commodityRemarks(itemId, detailTypeId), "Load failed.");
    }

    @PostMapping(PL + "/history") @ResponseBody
    public ResponseEntity<?> plHistory(@RequestBody Map<String, Object> body) { return call(() -> pl.history(body), "Load failed."); }

    @PostMapping(PL + "/save") @ResponseBody
    public ResponseEntity<?> plSave(@RequestBody Map<String, Object> body) { return call(() -> pl.save(body), "Save failed."); }

    @PostMapping(PL + "/delete") @ResponseBody
    public ResponseEntity<?> plDelete(@RequestBody Map<String, Object> body) {
        Object v = body.get("recId");
        int recId;
        try { recId = v == null ? 0 : (int) Double.parseDouble(String.valueOf(v)); } catch (NumberFormatException e) { recId = 0; }
        final int id = recId;
        return call(() -> pl.delete(id), "Delete failed.");
    }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    /**
     * 400 for the form's own validation text, 403 for a missing right, 500 with the innermost message
     * otherwise - for an SQL RAISERROR that is the procedure's own text, as MessageBox.Show(ex.Message).
     */
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
