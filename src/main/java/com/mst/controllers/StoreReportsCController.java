package com.mst.controllers;

import com.mst.services.StoreReportsCService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Store Management Reports (module 46), group C — two report pages, both read-only:
 *
 *   458  StoreIssuenceHistory  "Store Issuance Report"       IssuanceHistory.cs     /store/reports/store-issuance-history
 *   460  frmGatePassGeneral    "General Gate Pass  Report"   frmGatePassGeneral.cs  /store/reports/general-gate-pass
 *
 * frmGatePassGeneral was checked for an entry form behind the name: it has no save, update or
 * delete — it is a register over GatePassGeneral with a Slip button — so it lives here with 458.
 * See StoreReportsCService for the desktop-behaviour notes and deviations.
 *
 * No organization, company, user or financial year is accepted from the request.
 */
@Controller
public class StoreReportsCController {

    private static final String API_458 = "/api/store/reports/store-issuance-history";
    private static final String API_460 = "/api/store/reports/general-gate-pass";

    private final StoreReportsCService service;

    public StoreReportsCController(StoreReportsCService service) { this.service = service; }

    // ============================================================================= 458

    @GetMapping("/store/reports/store-issuance-history")
    public String issuancePage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/store_issuance_history";
    }

    @GetMapping(API_458 + "/lookups")
    @ResponseBody
    public ResponseEntity<?> issuanceLookups() {
        return run(service::issuanceLookups, "Could not load the screen.");
    }

    /** btnRefresh — FillAllDropDowns again. */
    @GetMapping(API_458 + "/refresh")
    @ResponseBody
    public ResponseEntity<?> issuanceRefresh() {
        return run(service::issuanceRefresh, "Could not refresh the lists.");
    }

    @GetMapping(API_458 + "/search")
    @ResponseBody
    public ResponseEntity<?> issuanceSearch(@RequestParam(required = false) String fromDate,
                                            @RequestParam(required = false) String toDate,
                                            @RequestParam(defaultValue = "0") int departmentId,
                                            @RequestParam(defaultValue = "0") int itemId,
                                            @RequestParam(defaultValue = "0") int assetId,
                                            @RequestParam(defaultValue = "0") int warehouseId,
                                            @RequestParam(defaultValue = "0") int accountId,
                                            @RequestParam(defaultValue = "0") int itemConditionId,
                                            @RequestParam(defaultValue = "0") int fromDocNo,
                                            @RequestParam(defaultValue = "0") int toDocNo) {
        return run(() -> service.issuanceSearch(fromDate, toDate, departmentId, itemId, assetId, warehouseId,
                accountId, itemConditionId, fromDocNo, toDocNo), "Search failed.");
    }

    @GetMapping(API_458 + "/slip")
    @ResponseBody
    public ResponseEntity<?> issuanceSlip(@RequestParam(defaultValue = "0") int id) {
        return run(() -> service.issuanceSlip(id), "Print failed.");
    }

    @GetMapping(API_458 + "/voucher")
    @ResponseBody
    public ResponseEntity<?> issuanceVoucher(@RequestParam(defaultValue = "0") int voucherHeadId,
                                             @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> service.voucher118(voucherHeadId, documentTypeId), "Voucher print failed.");
    }

    // ============================================================================= 460

    @GetMapping("/store/reports/general-gate-pass")
    public String gatePassPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/general_gate_pass";
    }

    @GetMapping(API_460 + "/lookups")
    @ResponseBody
    public ResponseEntity<?> gatePassLookups() {
        return run(service::gatePassLookups, "Could not load the screen.");
    }

    @GetMapping(API_460 + "/search")
    @ResponseBody
    public ResponseEntity<?> gatePassSearch(@RequestParam(required = false) String fromDate,
                                            @RequestParam(required = false) String toDate,
                                            @RequestParam(defaultValue = "0") int gpNoFrom,
                                            @RequestParam(defaultValue = "0") int gpNoTo,
                                            @RequestParam(required = false) String status,
                                            @RequestParam(defaultValue = "0") int documentTypeId,
                                            @RequestParam(required = false) String weightStatus,
                                            @RequestParam(defaultValue = "false") boolean onlyPending) {
        return run(() -> service.gatePassSearch(fromDate, toDate, gpNoFrom, gpNoTo, status, documentTypeId,
                weightStatus, onlyPending), "Search failed.");
    }

    @GetMapping(API_460 + "/slip-254")
    @ResponseBody
    public ResponseEntity<?> gatePassSlip254(@RequestParam(defaultValue = "0") int id,
                                             @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> service.gatePassSlip254(id, documentTypeId), "Print failed.");
    }

    @GetMapping(API_460 + "/slip-290")
    @ResponseBody
    public ResponseEntity<?> gatePassSlip290(@RequestParam(defaultValue = "0") int id) {
        return run(() -> service.gatePassSlip290(id), "Print failed.");
    }

    // ========================================================================= helpers

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(msg(e, fallback)));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static String msg(Exception e, String fallback) {
        String m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
