package com.mst.controllers;

import com.mst.models.dto.StoreReturnDto;
import com.mst.services.StoreReturnService;
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

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Management, ModuleId 24 — screen 330 "Store Return", {@code StoreReturn.cs},
 * DocumentTypeId 140, at /store/store-return.
 *
 * The id and ScreenName come from migration/user-rights/reconciliation-input.json (the live
 * GoldenAcedb dump). The page accepts ?issuanceId=N — the desktop's LoadIssuanceByRecId, which
 * 321 and 322 call after a save when their "Store Return" box is ticked.
 */
@Controller
public class StoreReturnController {

    private static final String API = "/api/store/store-return";

    private final StoreReturnService service;
    public StoreReturnController(StoreReturnService service) { this.service = service; }

    @GetMapping({"/store/store-return", "/store/return"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/store_return";
    }


    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/avg-rate")
    @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int recId, @RequestParam int itemId,
                                     @RequestParam(required = false) String docDate,
                                     @RequestParam(defaultValue = "0") int itemConditionId) {
        return run(() -> Collections.singletonMap("avgRate", service.avgRate(recId, itemId, docDate, itemConditionId)),
                "Could not read the rate.");
    }

    @GetMapping(API + "/last-issuance-rate")
    @ResponseBody
    public ResponseEntity<?> lastIssuanceRate(@RequestParam int itemId) {
        return run(() -> Collections.singletonMap("itemRate", service.lastIssuanceRate(itemId)), "Could not read the rate.");
    }

    @GetMapping(API + "/pending-issuances")
    @ResponseBody
    public ResponseEntity<?> pending(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int issuanceId) {
        return run(() -> service.pendingIssuances(fromDate, toDate, issuanceId), "Could not load issuances.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int docNoFrom,
                                     @RequestParam(defaultValue = "0") int docNoTo,
                                     @RequestParam(defaultValue = "0") int warehouseId,
                                     @RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(defaultValue = "0") int departmentId,
                                     @RequestParam(defaultValue = "0") int assetId,
                                     @RequestParam(defaultValue = "0") int accountId) {
        return run(() -> service.history(fromDate, toDate, docNoFrom, docNoTo, warehouseId, itemId, departmentId, assetId, accountId),
                "History failed.");
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
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody StoreReturnDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
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

    private interface Call { Object get() throws Exception; }

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
