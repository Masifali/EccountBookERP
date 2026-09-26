package com.mst.controllers;

import com.mst.models.dto.PurchaseDemandDto;
import com.mst.services.PurchaseDemandService;
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
 * Store Purchase, ModuleId 67 — screen 340 "Purchase Demand", {@code frmPurchaseDemand.cs}
 * (ScreenName "frmPurchaseDemand", the Store path only), DocumentTypeId 141, at
 * /store/purchase-demand. Behaviour notes and deviations: {@link PurchaseDemandService}.
 */
@Controller
public class PurchaseDemandController {

    private static final String API = "/api/store/purchase-demand";

    private final PurchaseDemandService service;
    public PurchaseDemandController(PurchaseDemandService service) { this.service = service; }

    @GetMapping("/store/purchase-demand")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/purchase_demand";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/last-purchase")
    @ResponseBody
    public ResponseEntity<?> lastPurchase(@RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.lastPurchase(itemId), "Could not read the last purchase.");
    }

    @GetMapping(API + "/outstanding")
    @ResponseBody
    public ResponseEntity<?> outstanding(@RequestParam(defaultValue = "0") int itemId,
                                         @RequestParam(required = false) String docDate,
                                         @RequestParam(defaultValue = "0") int itemConditionId,
                                         @RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.outstanding(itemId, docDate, itemConditionId, recId), "Could not read the outstanding quantity.");
    }

    @GetMapping(API + "/barcode")
    @ResponseBody
    public ResponseEntity<?> barcode(@RequestParam(required = false) String no) {
        return run(() -> Collections.singletonMap("itemId", service.itemIdByBarcode(no)), "Could not read the barcode.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo), "History failed.");
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
            if (rows == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            /* CommonServices.cs:8172 — the desktop's own text, spelling included, when the report has no rows. */
            if (rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found For Dispaly"));
            return ResponseEntity.ok(rows);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchaseDemandDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.badRequest().body(fail(msg(r, "Save failed.")));
        }
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
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
