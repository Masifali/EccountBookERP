package com.mst.controllers;

import com.mst.models.dto.GrnStoreDto;
import com.mst.services.GrnStoreService;
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
 * Store Purchase, ModuleId 67 — screen 324 "Grn Store", {@code GrnStore.cs} (ScreenName "GrnStore"),
 * DocumentTypeId 48, at /store/grn-store; JSON under /api/store/grn-store.
 *
 * The page accepts ?fromDate=&toDate=&onlyPending=1 — the desktop's public FromDate / ToDate /
 * OnlyPending fields, which open the form on its Register tab (InitializeComponentMethod:431).
 *
 * See {@link GrnStoreService} for the desktop behaviour reproduced and the web deviations.
 */
@Controller
public class GrnStoreController {

    private static final String API = "/api/store/grn-store";

    private final GrnStoreService service;
    public GrnStoreController(GrnStoreService service) { this.service = service; }

    @GetMapping("/store/grn-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/grn_store";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() {
        return run(() -> Collections.singletonMap("docNo", service.nextDocNo()), "Could not generate the Doc No.");
    }

    @GetMapping(API + "/history-filters")
    @ResponseBody
    public ResponseEntity<?> historyFilters() { return run(service::historyFilters, "Could not refresh the filters."); }

    /** Body: {docDate, recId, rows:[{ItemId, ItemConditionId, WarehouseId, RackId}]} → [QtyInHand...]. */
    @PostMapping(API + "/stock")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> stock(@RequestBody Map<String, Object> body) {
        return run(() -> {
            Object recId = body.get("recId");
            int rid = recId instanceof Number ? ((Number) recId).intValue() : 0;
            Object rows = body.get("rows");
            return service.availableStock(body.get("docDate") == null ? null : String.valueOf(body.get("docDate")), rid,
                    rows instanceof List ? (List<Map<String, Object>>) rows : null);
        }, "Could not read the stock.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierId) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, supplierId), "History failed.");
    }

    @GetMapping(API + "/register")
    @ResponseBody
    public ResponseEntity<?> register(@RequestParam(required = false) String fromDate,
                                      @RequestParam(required = false) String toDate,
                                      @RequestParam(defaultValue = "0") int fromDocNo,
                                      @RequestParam(defaultValue = "0") int toDocNo,
                                      @RequestParam(defaultValue = "0") int supplierId,
                                      @RequestParam(defaultValue = "0") int warehouseId,
                                      @RequestParam(defaultValue = "0") int itemId,
                                      @RequestParam(defaultValue = "false") boolean onlyPending) {
        return run(() -> service.register(fromDate, toDate, fromDocNo, toDocNo, supplierId, warehouseId, itemId, onlyPending),
                "Register failed.");
    }

    @GetMapping(API + "/po-slip/{orderId}")
    @ResponseBody
    public ResponseEntity<?> poSlip(@PathVariable int orderId) {
        try {
            return ResponseEntity.ok(service.purchaseOrderSlip(orderId));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    // ------------------------------------------------------------------------------ loaders

    /** Loader Doc No links: PO slip 201, Purchase Demand slip 454, Delivery Challan slip 148 (session org/company). */
    @GetMapping(API + "/loader/{kind}/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> loaderSlip(@PathVariable String kind, @PathVariable int id) {
        try {
            List<Map<String, Object>> rows;
            switch (kind) {
                case "po":     rows = service.loaderPurchaseOrderSlip(id); break;
                case "demand": rows = service.loaderPurchaseDemandSlip(id); break;
                case "dc":     rows = service.loaderDeliveryChallanSlip(id); break;
                default: return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            }
            if (rows == null || rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), "Print failed.")));
        }
    }

    @GetMapping(API + "/loader/po/lookups")
    @ResponseBody
    public ResponseEntity<?> poLookups(@RequestParam(defaultValue = "false") boolean refresh) {
        return run(() -> service.purchaseOrderLoaderLookups(refresh), "Could not load the loader.");
    }

    @GetMapping(API + "/loader/po")
    @ResponseBody
    public ResponseEntity<?> poPending(@RequestParam(required = false) String fromDate,
                                       @RequestParam(required = false) String toDate,
                                       @RequestParam(defaultValue = "0") int fromDocNo,
                                       @RequestParam(defaultValue = "0") int toDocNo,
                                       @RequestParam(defaultValue = "0") int supplierId) {
        return run(() -> service.pendingPurchaseOrders(fromDate, toDate, fromDocNo, toDocNo, supplierId), "Could not load pending orders.");
    }

    @GetMapping(API + "/loader/demand/lookups")
    @ResponseBody
    public ResponseEntity<?> demandLookups() { return run(service::purchaseDemandLoaderLookups, "Could not load the loader."); }

    @GetMapping(API + "/loader/demand")
    @ResponseBody
    public ResponseEntity<?> demandPending(@RequestParam(required = false) String fromDate,
                                           @RequestParam(required = false) String toDate,
                                           @RequestParam(defaultValue = "0") int fromDocNo,
                                           @RequestParam(defaultValue = "0") int toDocNo,
                                           @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.pendingPurchaseDemands(fromDate, toDate, fromDocNo, toDocNo, itemId), "Could not load pending demands.");
    }

    @GetMapping(API + "/loader/dc/lookups")
    @ResponseBody
    public ResponseEntity<?> dcLookups() { return run(service::deliveryChallanLoaderLookups, "Could not load the loader."); }

    @GetMapping(API + "/loader/dc")
    @ResponseBody
    public ResponseEntity<?> dcPending(@RequestParam(required = false) String fromDate,
                                       @RequestParam(required = false) String toDate,
                                       @RequestParam(defaultValue = "0") int fromDocNo,
                                       @RequestParam(defaultValue = "0") int toDocNo,
                                       @RequestParam(defaultValue = "0") int itemId,
                                       @RequestParam(defaultValue = "0") int billToPartyId) {
        return run(() -> service.pendingDeliveryChallans(fromDate, toDate, fromDocNo, toDocNo, itemId, billToPartyId),
                "Could not load pending delivery challans.");
    }

    // ------------------------------------------------------------------------ one document

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found"));
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
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody GrnStoreDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), "Save failed.")));
        }
    }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        try {
            return ResponseEntity.ok(service.delete(id));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), "Delete failed.")));
        }
    }

    // ------------------------------------------------------------------------------ plumbing

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
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
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
