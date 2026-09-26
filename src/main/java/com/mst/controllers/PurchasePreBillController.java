package com.mst.controllers;

import com.mst.models.dto.PurchasePreBillDto;
import com.mst.services.PurchasePreBillService;
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

/**
 * Store Management (ModuleId 24) — screen 961 "Store Purchase Pre Bill", {@code frmPurchasePreBill.cs}
 * (ScreenName "frmPurchasePreBill"), DocumentTypeId 147, at /store/purchase-pre-bill; JSON under
 * /api/store/purchase-pre-bill. The grid is filled only by the Load-Demand dialog
 * {@code frmPendingPurchaseDemand.cs} (IsForPreBill, Purchase Demand DocumentTypeId 141), served by
 * the /loader endpoints.
 *
 * Business rules, desktop-behaviour notes and deviations: see {@link PurchasePreBillService}.
 * Errors answer 400 with {message} (403 for a missing right), as the other Store controllers do.
 */
@Controller
public class PurchasePreBillController {

    private static final String API = "/api/store/purchase-pre-bill";

    private final PurchasePreBillService service;
    public PurchasePreBillController(PurchasePreBillService service) { this.service = service; }

    @GetMapping("/store/purchase-pre-bill")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/purchase_pre_bill";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups); }

    @GetMapping(API + "/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::formRefresh); }

    @GetMapping(API + "/history-refresh")
    @ResponseBody
    public ResponseEntity<?> historyRefresh() { return run(service::historyRefresh); }

    @GetMapping(API + "/loader/lookups")
    @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(service::loaderLookups); }

    @GetMapping(API + "/loader/pending")
    @ResponseBody
    public ResponseEntity<?> loaderPending(@RequestParam(required = false) String fromDate,
                                           @RequestParam(required = false) String toDate,
                                           @RequestParam(required = false) String docNoFrom,
                                           @RequestParam(required = false) String docNoTo,
                                           @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.pendingDemands(fromDate, toDate, docNoFrom, docNoTo, itemId));
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(defaultValue = "true") boolean fromChecked,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(defaultValue = "true") boolean toChecked,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String fromDocNo,
                                     @RequestParam(required = false) String toDocNo,
                                     @RequestParam(defaultValue = "0") int billToPartyId,
                                     @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.history(dateType, fromChecked, fromDate, toChecked, toDate, fromDocNo, toDocNo,
                billToPartyId, itemId));
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id, @RequestParam(defaultValue = "false") boolean edit) {
        try {
            Map<String, Object> m = service.load(id, edit);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found"));
            return ResponseEntity.ok(m);
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(msg(e)));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e)));
        }
    }

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) { return run(() -> service.slip(id)); }

    @GetMapping(API + "/{id}/slip-sub")
    @ResponseBody
    public ResponseEntity<?> slipSub(@PathVariable int id) { return run(() -> service.slipSubReport(id)); }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchasePreBillDto dto) { return run(() -> service.save(dto)); }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return run(() -> service.delete(id)); }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(msg(e)));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e)));
        }
    }

    private static String msg(Throwable e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String m = r.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? "Request failed." : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
