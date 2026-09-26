package com.mst.controllers;

import com.mst.services.WeighBridgeService;
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
 * Screen 411 "Weigh Bridge" — {@code Architecture.WinApp.WeightBridge.frmWeightbridge}.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THE PAGE IS /weighbridge/weight-bridge
 * ---------------------------------------------------------------------------------------------
 * ScreenRouteIndex wires a desktop screen to a web page by exact normalised equality between the
 * screen's name and a registered route's last segment, trying the name with its "frm" prefix
 * removed too. "frmWeightbridge" -> "weightbridge", and "weight-bridge" normalises to exactly
 * that, so the application hub links screen 411 here with no hand-kept mapping. The existing
 * "/weighbridge" and "/weigh-bridge" routes normalise to "weighbridge" (no t) and do not collide.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT IS DELIBERATELY NOT A PARAMETER
 * ---------------------------------------------------------------------------------------------
 * No organisation, company, branch, financial year, user, document type or scale id is accepted
 * on any route. Each is read from the signed-in session or from the scales list inside the
 * service, as the desktop reads them from UserAccount / clsGlobalVariables / dtWbList.
 */
@Controller
public class WeighBridgeController {

    private static final String API = "/api/weighbridge";

    @Autowired
    private WeighBridgeService service;

    @GetMapping("/weighbridge/weight-bridge")
    public String page(Model model) {
        model.addAttribute("activeMenu", "kanta");
        return "weighbridge/weight_bridge";
    }

    /** InitializeComponentMethod():661. */
    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    /** btnRefresh_Click:4629. */
    @GetMapping(API + "/refresh-lists")
    @ResponseBody
    public ResponseEntity<?> refreshLists() { return run(service::refreshLists, "Refresh failed."); }

    /** BtnRefreshHistory_Click:3706. */
    @GetMapping(API + "/refresh-history-lists")
    @ResponseBody
    public ResponseEntity<?> refreshHistoryLists() { return run(service::refreshHistoryLists, "Refresh failed."); }

    /** WeightBridge_Helper.GetTicketNoDbCall. */
    @GetMapping(API + "/ticket-no")
    @ResponseBody
    public ResponseEntity<?> ticketNo() {
        return run(() -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("ticketNo", service.nextTicketNo()); return m; },
                   "Could not generate the ticket number.");
    }

    /** GatePassBending():3816. mode = lab | engr | outward | general | party. */
    @GetMapping(API + "/ref-docs")
    @ResponseBody
    public ResponseEntity<?> refDocs(@RequestParam String mode,
                                     @RequestParam(required = false, defaultValue = "0") int documentTypeId) {
        return run(() -> service.refDocs(mode, documentTypeId), "Could not load the gate passes.");
    }

    /** cmbRefDocNo_Leave():3946. kind = inward | outward | general | party. */
    @GetMapping(API + "/gate-pass")
    @ResponseBody
    public ResponseEntity<?> gatePass(@RequestParam String kind,
                                      @RequestParam(required = false, defaultValue = "0") int documentTypeId,
                                      @RequestParam(required = false, defaultValue = "0") int id,
                                      @RequestParam(required = false, defaultValue = "0") int gpSrNo) {
        return run(() -> service.gatePass(kind, documentTypeId, id, gpSrNo), "Could not read the gate pass.");
    }

    @GetMapping(API + "/lab-items")
    @ResponseBody
    public ResponseEntity<?> labItems(@RequestParam int gpId,
                                      @RequestParam(required = false, defaultValue = "0") int actionId) {
        return run(() -> service.labItems(gpId, actionId), "Could not read the lab items.");
    }

    @GetMapping(API + "/delivery-order-items")
    @ResponseBody
    public ResponseEntity<?> deliveryOrderItems(@RequestParam(required = false, defaultValue = "0") int deliveryOrderId,
                                                @RequestParam(required = false, defaultValue = "0") int gpId) {
        return run(() -> service.deliveryOrderItems(deliveryOrderId, gpId), "Could not read the delivery order items.");
    }

    @GetMapping(API + "/delivery-order-parties-items")
    @ResponseBody
    public ResponseEntity<?> deliveryOrderPartiesAndItems(@RequestParam int gpId,
                                                          @RequestParam(required = false, defaultValue = "0") int recId) {
        return run(() -> service.deliveryOrderPartiesAndItems(gpId, recId), "Could not read the delivery order parties.");
    }

    @GetMapping(API + "/delivery-order-invoices")
    @ResponseBody
    public ResponseEntity<?> deliveryOrderInvoices(@RequestParam int gpId) {
        return run(() -> service.deliveryOrderInvoices(gpId), "Could not read the invoice numbers.");
    }

    @GetMapping(API + "/sale-order-items")
    @ResponseBody
    public ResponseEntity<?> saleOrderItems(@RequestParam int saleOrderId) {
        return run(() -> service.saleOrderItems(saleOrderId), "Could not read the sale order items.");
    }

    /** BindPendingFor1st():1729. A date is absent when its picker checkbox is cleared. */
    @GetMapping(API + "/pending-first")
    @ResponseBody
    public ResponseEntity<?> pendingFirst(@RequestParam(required = false) String fromDate,
                                          @RequestParam(required = false) String toDate,
                                          @RequestParam(required = false, defaultValue = "0") int gpNoFrom,
                                          @RequestParam(required = false, defaultValue = "0") int gpNoTo) {
        return run(() -> service.pendingFirst(fromDate, toDate, gpNoFrom, gpNoTo), "Could not load the pending gate passes.");
    }

    /** bindGridPendingForSecond():1975. */
    @GetMapping(API + "/pending-second")
    @ResponseBody
    public ResponseEntity<?> pendingSecond(@RequestParam(required = false) String fromDate,
                                           @RequestParam(required = false) String toDate,
                                           @RequestParam(required = false, defaultValue = "0") int gpNoFrom,
                                           @RequestParam(required = false, defaultValue = "0") int gpNoTo) {
        return run(() -> service.pendingSecond(fromDate, toDate, gpNoFrom, gpNoTo), "Could not load the pending tickets.");
    }

    /** BindHistoryGrid():3537. */
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

    /** ReadById(ID):2587. */
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

    /** 280 slip; withImages = the 281 slip. */
    @GetMapping(API + "/slip/{id}")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id,
                                  @RequestParam(required = false, defaultValue = "false") boolean withImages) {
        return run(() -> service.slip(id, withImages), "Could not read the slip.");
    }

    /** 257 inward gate pass slip with the lab and weighbridge sub-reports. */
    @GetMapping(API + "/gate-pass-slip/{gatePassId}")
    @ResponseBody
    public ResponseEntity<?> gatePassSlip(@PathVariable int gatePassId) {
        return run(() -> service.inwardGatePassSlip(gatePassId), "Could not read the gate pass slip.");
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody WeighBridgeService.Request request) {
        return write(() -> service.save(request), "Save failed.");
    }

    @PostMapping(API + "/update")
    @ResponseBody
    public ResponseEntity<?> update(@RequestBody WeighBridgeService.Request request) {
        return write(() -> service.update(request), "Update failed.");
    }

    // ------------------------------------------------------------------ plumbing

    private static ResponseEntity<?> run(Callable<?> body, String fallback) {
        try {
            return ResponseEntity.ok(body.call());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    /** The desktop's own validation text reaches the operator unchanged (400); a missing right
     *  is 403; anything the database raised comes back with its own message (500). */
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

    /** The innermost cause's message — a RAISERROR text from a procedure, not a Spring wrapper. */
    private static String msg(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return (m == null || m.trim().isEmpty()) ? fallback : m;
    }
}
