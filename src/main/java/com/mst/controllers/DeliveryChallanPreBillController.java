package com.mst.controllers;

import com.mst.models.dto.DeliveryChallanPreBillDto;
import com.mst.services.DeliveryChallanPreBillService;
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
 * Store Management, ModuleId 24 — ScreenDefinition 960 "Delivery Challan Against PreBill",
 * ScreenName {@code frmDeliveryChallanAgainstPurchasePreBill}, DocumentTypeId 148,
 * page /store/delivery-challan-prebill, API /api/store/delivery-challan-prebill.
 *
 * Business rules, desktop-behaviour notes and deviations: see {@link DeliveryChallanPreBillService}.
 * Errors: IllegalArgumentException (validation, desktop messages) → 400, IllegalStateException
 * (missing right) → 403, anything else (a procedure's RAISERROR included) → 400 with its message.
 */
@Controller
public class DeliveryChallanPreBillController {

    private static final String API = "/api/store/delivery-challan-prebill";

    private final DeliveryChallanPreBillService service;
    public DeliveryChallanPreBillController(DeliveryChallanPreBillService service) { this.service = service; }

    @GetMapping("/store/delivery-challan-prebill")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/delivery_challan_prebill";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not read the Doc No."); }

    @GetMapping(API + "/loader/combos")
    @ResponseBody
    public ResponseEntity<?> loaderCombos() { return run(service::loaderCombos, "Could not load the filters."); }

    @GetMapping(API + "/loader")
    @ResponseBody
    public ResponseEntity<?> loader(@RequestParam(required = false) String fromDate,
                                    @RequestParam(required = false) String toDate,
                                    @RequestParam(required = false) String fromDocNo,
                                    @RequestParam(required = false) String toDocNo,
                                    @RequestParam(defaultValue = "0") int itemId,
                                    @RequestParam(defaultValue = "0") int billToPartyId) {
        return run(() -> service.pending(fromDate, toDate, fromDocNo, toDocNo, itemId, billToPartyId), "Could not load pending Pre-Bills.");
    }

    @GetMapping(API + "/loader/expenses")
    @ResponseBody
    public ResponseEntity<?> loaderExpenses(@RequestParam(required = false) String headerIds) {
        return run(() -> service.loaderExpenses(headerIds), "Could not load the Pre-Bill expenses.");
    }

    @GetMapping(API + "/prebill/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> preBillSlip(@PathVariable int id) {
        return run(() -> service.preBillSlip(id), "Print failed.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(defaultValue = "false") boolean fromChecked,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(defaultValue = "false") boolean toChecked,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String fromDocNo,
                                     @RequestParam(required = false) String toDocNo,
                                     @RequestParam(defaultValue = "0") int cityId,
                                     @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.history(dateType, fromChecked, fromDate, toChecked, toDate, fromDocNo, toDocNo, cityId, itemId),
                "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return error(e, "Could not open that document.");
        }
    }

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) {
        return run(() -> service.slip(id), "Print failed.");
    }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return run(() -> service.delete(id), "Delete failed.");
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody DeliveryChallanPreBillDto dto) {
        return run(() -> service.save(dto), "Save failed.");
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return error(e, fallback);
        }
    }

    private static ResponseEntity<?> error(Exception e, String fallback) {
        if (e instanceof IllegalStateException) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(msg(e, fallback)));
        if (e instanceof IllegalArgumentException) return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();                   // the procedure's RAISERROR text
        return ResponseEntity.badRequest().body(fail(msg(r, fallback)));
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
