package com.mst.controllers;

import com.mst.services.StorePurchaseOrderTradeService;
import com.mst.services.StorePurchaseOrderTradeService.Request;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
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
import org.springframework.web.server.ResponseStatusException;

/**
 * Store Purchase, ModuleId 67 - screen 328 "Purchase Order Store",
 * {@code Architecture.WinApp.PurchaseTrading.PurchsaeOrderTrade} (ScreenName "PurchsaeOrderTrade", DocumentTypeId 42),
 * page /store/purchase-order-store, JSON under /api/store/purchase-order-store.
 * See {@link StorePurchaseOrderTradeService} for the desktop behaviour reproduced and the web deviations.
 */
@Controller
public class StorePurchaseOrderTradeController {


    private final StorePurchaseOrderTradeService service;

    public StorePurchaseOrderTradeController(StorePurchaseOrderTradeService service) { this.service = service; }

    @GetMapping("/store/purchase-order-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/purchase_order_trade";
    }

    @GetMapping("/api/store/purchase-order-store/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/store/purchase-order-store/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not generate the Doc No."); }

    @GetMapping("/api/store/purchase-order-store/history-suppliers")
    @ResponseBody
    public ResponseEntity<?> historySuppliers(@RequestParam(defaultValue = "") String branchIds) {
        return run(() -> service.historySuppliers(ids(branchIds)), "Could not refresh the filters.");
    }

    @GetMapping("/api/store/purchase-order-store/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierId,
                                     @RequestParam(defaultValue = "") String branchIds) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, supplierId, ids(branchIds)),
                "Could not load the history.");
    }

    @GetMapping("/api/store/purchase-order-store/loader/demand/lookups")
    @ResponseBody
    public ResponseEntity<?> demandLookups() { return run(service::demandLoaderLookups, "Could not load the loader."); }

    @GetMapping("/api/store/purchase-order-store/loader/demand")
    @ResponseBody
    public ResponseEntity<?> demandPending(@RequestParam(required = false) String fromDate,
                                           @RequestParam(required = false) String toDate,
                                           @RequestParam(defaultValue = "0") int fromDocNo,
                                           @RequestParam(defaultValue = "0") int toDocNo,
                                           @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.pendingDemands(fromDate, toDate, fromDocNo, toDocNo, itemId), "Could not load pending demands.");
    }

    @GetMapping("/api/store/purchase-order-store/{id:[0-9]+}")
    @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) { return run(() -> service.record(id), "Could not load the order."); }

    @GetMapping("/api/store/purchase-order-store/{id:[0-9]+}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) { return run(() -> service.slip(id), "Print failed."); }

    @PostMapping("/api/store/purchase-order-store/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Request request) { return run(() -> service.save(request), "Save failed."); }

    // ------------------------------------------------------------------------------ plumbing

    private static List<Integer> ids(String csv) {
        List<Integer> out = new ArrayList<>();
        for (String s : csv.split(",")) {
            s = s.trim();
            if (s.isEmpty()) continue;
            try { out.add(Integer.parseInt(s)); } catch (NumberFormatException e) { /* not an id */ }
        }
        return out;
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(msg(e, fallback)));
        } catch (ResponseStatusException e) {
            return ResponseEntity.status(e.getStatus()).body(fail(msg(e, fallback)));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), fallback)));
        }
    }

    private static Throwable root(Throwable e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        return r;
    }

    private static String msg(Throwable e, String fallback) {
        String m = e instanceof ResponseStatusException rse ? rse.getReason() : e.getMessage();
        return (m == null || m.trim().isEmpty()) ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
