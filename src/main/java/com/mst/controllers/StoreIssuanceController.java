package com.mst.controllers;

import com.mst.models.dto.StoreIssuanceDto;
import com.mst.services.StoreIssuanceService;
import com.mst.services.StoreIssuanceService.Screen;
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
import java.util.List;
import java.util.Map;

/**
 * Store Management, ModuleId 24:
 *
 *   322  frmGSIssuance        "Store Issuance"         DocumentTypeId 451   /store/store-issuance
 *   321  StoreIssuanceDirect  "Store Issuance Direct"  DocumentTypeId 452   /store/store-issuance-direct
 *
 * Ids and ScreenNames from migration/user-rights/reconciliation-input.json (the live GoldenAcedb
 * dump), recorded here so DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID has its second record.
 *
 * {screen} is "issuance" (322) or "direct" (321) and nothing else. No organization, company,
 * branch, financial year, document type, user or view-all flag is accepted from the request —
 * every one is fixed by the screen or read from the signed-in user in the service.
 */
@Controller
public class StoreIssuanceController {

    private static final String API = "/api/store/issuance/{screen}";

    private final StoreIssuanceService service;
    public StoreIssuanceController(StoreIssuanceService service) { this.service = service; }

    // ----------------------------------------------------------------------------- pages

    /** 322 Store Issuance. */
    @GetMapping({"/store/store-issuance", "/store/issuance"})
    public String issuancePage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/store_issuance";
    }

    /** 321 Store Issuance Direct. */
    @GetMapping({"/store/store-issuance-direct", "/store/issuance-direct"})
    public String directPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/store_issuance_direct";
    }


    // ------------------------------------------------------------------------------- api

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups(@PathVariable String screen) {
        return run(() -> service.lookups(screen(screen)), "Could not load the screen.");
    }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@PathVariable String screen,
                                   @RequestParam(defaultValue = "0") int recId,
                                   @RequestParam int itemId,
                                   @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int itemConditionId,
                                   @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int rackId) {
        return run(() -> service.stock(screen(screen), recId, itemId, docDate, itemConditionId, warehouseId, rackId),
                "Could not read stock.");
    }

    @GetMapping(API + "/last-rates")
    @ResponseBody
    public ResponseEntity<?> lastRates(@PathVariable String screen, @RequestParam int itemId,
                                       @RequestParam(defaultValue = "0") int itemConditionId) {
        screen(screen);
        return run(() -> service.lastThreeRates(itemId, itemConditionId), "Could not read rates.");
    }

    /** 321 only — txtBarcodeReader. */
    @GetMapping(API + "/barcode")
    @ResponseBody
    public ResponseEntity<?> barcode(@PathVariable String screen, @RequestParam String barcode) {
        if (screen(screen) != Screen.DIRECT) return ResponseEntity.notFound().build();
        return run(() -> java.util.Collections.singletonMap("itemId", service.itemIdByBarcode(barcode)), "Barcode lookup failed.");
    }

    /** 322 only — PendingDoPmForIssuance. */
    @GetMapping(API + "/pending-delivery-orders")
    @ResponseBody
    public ResponseEntity<?> pendingDeliveryOrders(@PathVariable String screen,
                                                   @RequestParam(required = false) String fromDate,
                                                   @RequestParam(required = false) String toDate,
                                                   @RequestParam(defaultValue = "0") int docNoFrom,
                                                   @RequestParam(defaultValue = "0") int docNoTo) {
        if (screen(screen) != Screen.ISSUANCE) return ResponseEntity.notFound().build();
        return run(() -> service.pendingDeliveryOrders(fromDate, toDate, docNoFrom, docNoTo),
                "Could not load pending delivery orders.");
    }

    /** 322 only — LoadDepRequestToConsumableStore pickers. */
    @GetMapping(API + "/department-request-lookups")
    @ResponseBody
    public ResponseEntity<?> departmentRequestLookups(@PathVariable String screen) {
        if (screen(screen) != Screen.ISSUANCE) return ResponseEntity.notFound().build();
        return run(service::departmentRequestLookups, "Could not load the request filters.");
    }

    /** 322 only — LoadDepRequestToConsumableStore grid. */
    @GetMapping(API + "/pending-department-requests")
    @ResponseBody
    public ResponseEntity<?> pendingDepartmentRequests(@PathVariable String screen,
                                                       @RequestParam(required = false) String fromDate,
                                                       @RequestParam(required = false) String toDate,
                                                       @RequestParam(defaultValue = "0") int docNoFrom,
                                                       @RequestParam(defaultValue = "0") int docNoTo,
                                                       @RequestParam(defaultValue = "0") int itemId,
                                                       @RequestParam(defaultValue = "0") int departmentFromId,
                                                       @RequestParam(defaultValue = "0") int departmentToId) {
        if (screen(screen) != Screen.ISSUANCE) return ResponseEntity.notFound().build();
        return run(() -> service.pendingDepartmentRequests(fromDate, toDate, docNoFrom, docNoTo,
                itemId, departmentFromId, departmentToId), "Could not load pending department requests.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@PathVariable String screen,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo) {
        return run(() -> service.history(screen(screen), fromDate, toDate, fromDocNo, toDocNo), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable String screen, @PathVariable int id) {
        try {
            Map<String, Object> m = service.load(screen(screen), id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that document.")));
        }
    }

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable String screen, @PathVariable int id) {
        try {
            List<Map<String, Object>> rows = service.slip(screen(screen), id);
            if (rows == null || rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            return ResponseEntity.ok(rows);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@PathVariable String screen, @RequestBody StoreIssuanceDto dto) {
        return write(() -> service.save(screen(screen), dto), "Save failed.");
    }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable String screen, @PathVariable int id) {
        return write(() -> service.delete(screen(screen), id), "Delete failed.");
    }

    // --------------------------------------------------------------------------- helpers

    private static Screen screen(String s) {
        if ("issuance".equals(s)) return Screen.ISSUANCE;
        if ("direct".equals(s)) return Screen.DIRECT;
        throw new org.springframework.web.server.ResponseStatusException(HttpStatus.NOT_FOUND);
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (org.springframework.web.server.ResponseStatusException e) {
            throw e;
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (org.springframework.web.server.ResponseStatusException e) {
            throw e;
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            /* A RAISERROR from a procedure (USP_InventoryValidation, the save procs) carries the
               desktop's own message; it reaches the operator as the desktop's MessageBox did. */
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), fallback)));
        }
    }

    private static Throwable rootCause(Throwable t) {
        Throwable r = t;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        return r;
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
