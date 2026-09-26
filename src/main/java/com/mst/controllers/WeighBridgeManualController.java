package com.mst.controllers;

import com.mst.services.WeighBridgeManualService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
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
import java.util.concurrent.Callable;

/**
 * Screen 410 "Weigh Bridge Manual" — {@code Architecture.WinApp.WeightBridge.WeightbridgeMannual}.
 *
 * The page route's last segment, "weightbridge-mannual", normalises to "weightbridgemannual" —
 * exactly the desktop ScreenName "WeightbridgeMannual", spelling included — so ScreenRouteIndex
 * links screen 410 here on its own. It does not collide with 411's "weight-bridge".
 *
 * The lab items, delivery-order items / parties / invoices, sale-order items and the 257 slip are
 * the same BLL calls screen 411 makes, so the page uses /api/weighbridge/* for those. Everything
 * that differs on this form is here. No organisation, company, branch, year, user or document
 * type is accepted from the caller.
 */
@Controller
public class WeighBridgeManualController {

    private static final String API = "/api/weighbridge-manual";

    @Autowired
    private WeighBridgeManualService service;

    @GetMapping("/weighbridge/weightbridge-mannual")
    public String page(Model model) {
        model.addAttribute("activeMenu", "kanta");
        return "weighbridge/weightbridge_mannual";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/refresh-lists")
    @ResponseBody
    public ResponseEntity<?> refreshLists() { return run(service::refreshLists, "Refresh failed."); }

    @GetMapping(API + "/refresh-history-lists")
    @ResponseBody
    public ResponseEntity<?> refreshHistoryLists() { return run(service::refreshHistoryLists, "Refresh failed."); }

    @GetMapping(API + "/ticket-no")
    @ResponseBody
    public ResponseEntity<?> ticketNo() {
        return run(() -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("ticketNo", service.nextTicketNo()); return m; },
                   "Could not generate the ticket number.");
    }

    /** GatePassBending():1183. mode = outward | steel | lab | engr | general | party. */
    @GetMapping(API + "/ref-docs")
    @ResponseBody
    public ResponseEntity<?> refDocs(@RequestParam String mode,
                                     @RequestParam(required = false, defaultValue = "0") int documentTypeId,
                                     @RequestParam(required = false, defaultValue = "0") int recId) {
        return run(() -> service.refDocs(mode, documentTypeId, recId), "Could not load the gate passes.");
    }

    /** BindingAgainstGatePass():1305. */
    @GetMapping(API + "/gate-pass")
    @ResponseBody
    public ResponseEntity<?> gatePass(@RequestParam String kind,
                                      @RequestParam(required = false, defaultValue = "0") int documentTypeId,
                                      @RequestParam(required = false, defaultValue = "0") int id,
                                      @RequestParam(required = false, defaultValue = "0") int gpSrNo) {
        return run(() -> service.gatePass(kind, documentTypeId, id, gpSrNo), "Could not read the gate pass.");
    }

    @GetMapping(API + "/saved-vehicles")
    @ResponseBody
    public ResponseEntity<?> savedVehicles() { return run(service::savedVehicles, "Could not read the saved vehicles."); }

    @GetMapping(API + "/pending-first")
    @ResponseBody
    public ResponseEntity<?> pendingFirst() { return run(service::pendingFirst, "Could not load the pending gate passes."); }

    @GetMapping(API + "/pending-second")
    @ResponseBody
    public ResponseEntity<?> pendingSecond() { return run(service::pendingSecond, "Could not load the pending tickets."); }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false, defaultValue = "doc") String dateMode,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false, defaultValue = "0") int ticketNoFrom,
                                     @RequestParam(required = false, defaultValue = "0") int ticketNoTo,
                                     @RequestParam(required = false, defaultValue = "0") int gpNoFrom,
                                     @RequestParam(required = false, defaultValue = "0") int gpNoTo,
                                     @RequestParam(required = false) String vehicleNo,
                                     @RequestParam(required = false, defaultValue = "0") int refDocumentTypeId,
                                     @RequestParam(required = false, defaultValue = "0") int wbTypeId,
                                     @RequestParam(required = false) String wbTypeText) {
        return run(() -> service.history(dateMode, fromDate, toDate, ticketNoFrom, ticketNoTo, gpNoFrom, gpNoTo,
                                         vehicleNo, refDocumentTypeId, wbTypeId, wbTypeText),
                   "Could not load the history.");
    }

    @GetMapping(API + "/slip/{id}")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) { return run(() -> service.slip(id), "Could not read the slip."); }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that ticket.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody WeighBridgeManualService.Request request) {
        return write(() -> service.save(request), "Save failed.");
    }

    @PostMapping(API + "/update")
    @ResponseBody
    public ResponseEntity<?> update(@RequestBody WeighBridgeManualService.Request request) {
        return write(() -> service.update(request), "Update failed.");
    }

    private static ResponseEntity<?> run(Callable<?> body, String fallback) {
        try {
            return ResponseEntity.ok(body.call());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static ResponseEntity<?> write(Callable<?> body, String fallback) {
        try {
            return ResponseEntity.ok(body.call());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }

    private static String msg(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return (m == null || m.trim().isEmpty()) ? fallback : m;
    }
}
