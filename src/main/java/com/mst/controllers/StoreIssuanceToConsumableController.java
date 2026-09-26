package com.mst.controllers;

import com.mst.models.dto.StoreIssuanceToConsumableDto;
import com.mst.services.StoreIssuanceToConsumableService;
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
 * Store Management (AppModules 24) — screen 347 "Store Issuance To Consumable Store",
 * ScreenName / base.Name {@code frmStoreIssuanceToCosumableStore} (TargetUrl
 * StoreManagement.frmStoreIssuanceToCosumableStore), DocumentTypeId 1616, at
 * /store/issuance-to-consumable-store, API /api/store/issuance-to-consumable-store.
 *
 * BLL/DAL: BLL 0252 / DAL 0221 InvGsStoreIssuanceHeader, BLL 0056, BLL 0058, BLL 0078 — see
 * {@link StoreIssuanceToConsumableService} for the numbered DESKTOP BEHAVIOUR REPRODUCED and
 * DEVIATIONS lists.
 *
 * Errors: validation → 400 {message}; missing right → 403 {message}; anything else → 500 {message}.
 */
@Controller
public class StoreIssuanceToConsumableController {

    private static final String API = "/api/store/issuance-to-consumable-store";

    private final StoreIssuanceToConsumableService service;
    public StoreIssuanceToConsumableController(StoreIssuanceToConsumableService service) { this.service = service; }

    @GetMapping("/store/issuance-to-consumable-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/store_issuance_to_consumable";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() {
        return run(() -> java.util.Collections.singletonMap("docNo", service.docNo()), "Could not generate the Doc No.");
    }

    @GetMapping(API + "/loader/lookups")
    @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(service::loaderLookups, "Could not load the filters."); }

    @GetMapping(API + "/loader/pending")
    @ResponseBody
    public ResponseEntity<?> pending(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int docNoFrom,
                                     @RequestParam(defaultValue = "0") int docNoTo,
                                     @RequestParam(defaultValue = "0") int departmentFromId,
                                     @RequestParam(defaultValue = "0") int departmentToId,
                                     @RequestParam(defaultValue = "0") int workStationFromId,
                                     @RequestParam(defaultValue = "0") int workStationToId,
                                     @RequestParam(defaultValue = "0") int workOrderId,
                                     @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.pendingRequests(fromDate, toDate, docNoFrom, docNoTo, departmentFromId, departmentToId,
                workStationFromId, workStationToId, workOrderId, itemId), "Could not load pending requests.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int noOfRecords) {
        return run(() -> service.history(noOfRecords), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that document.")));
        }
    }

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) {
        try {
            List<Map<String, Object>> rows = service.slip(id);
            if (rows == null || rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/precheck")
    @ResponseBody
    public ResponseEntity<?> precheck(@RequestBody StoreIssuanceToConsumableDto dto) {
        return write(() -> service.precheck(dto));
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody StoreIssuanceToConsumableDto dto) {
        return write(() -> service.save(dto));
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> write(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(r, "Save failed.")));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
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
