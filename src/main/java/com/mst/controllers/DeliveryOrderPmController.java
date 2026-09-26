package com.mst.controllers;

import com.mst.models.dto.DeliveryOrderPmDto;
import com.mst.services.DeliveryOrderPmService;
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
import java.util.Map;

/**
 * Packing Material, ModuleId 54 - screen 506 "Delivery Order Packing Material", {@code DeliveryOrderPackingMaterial.cs}
 * (ScreenName "DeliveryOrderPackingMaterial"), DocumentTypeId 85, at /packing-material/delivery-order.
 * Behaviour notes and deviations are on {@link DeliveryOrderPmService}.
 */
@Controller
public class DeliveryOrderPmController {

    private static final String API = "/api/packing-material/delivery-order";

    private final DeliveryOrderPmService service;
    public DeliveryOrderPmController(DeliveryOrderPmService service) { this.service = service; }

    @GetMapping("/packing-material/delivery-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/delivery_order_pm";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not number the document."); }

    @GetMapping(API + "/id-by-doc-no")
    @ResponseBody
    public ResponseEntity<?> idByDocNo(@RequestParam(defaultValue = "0") int docNo) { return run(() -> Collections.singletonMap("id", service.idByDocNo(docNo)), "Could not find the document."); }

    @GetMapping(API + "/orders")
    @ResponseBody
    public ResponseEntity<?> orders(@RequestParam(defaultValue = "0") int customerId) { return run(() -> service.orders(customerId), "Could not load orders."); }

    @GetMapping(API + "/balances")
    @ResponseBody
    public ResponseEntity<?> balances(@RequestParam(defaultValue = "0") int bagTypeId, @RequestParam(defaultValue = "0") int customerId,
                                      @RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String docDate) {
        return run(() -> service.balances(bagTypeId, customerId, itemId, docDate), "Could not read the balances.");
    }

    @GetMapping(API + "/history-customers")
    @ResponseBody
    public ResponseEntity<?> historyCustomers() { return run(service::historyCustomers, "Could not load customers."); }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int customerId) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, customerId), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record not found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Could not open that document.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody DeliveryOrderPmDto dto) { return write(() -> service.save(dto), "Save failed."); }

    // --------------------------------------------------------------------------- helpers

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), fallback)));
        }
    }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            /* RAISERRORs from the save procedures carry the desktop's own message. */
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(rootCause(e), fallback)));
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
