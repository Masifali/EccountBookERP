package com.mst.controllers.cmagt;

import com.mst.services.CmtrTransactionService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Collections;
import java.util.Map;

/** Web screens for the 1702 direct loading/delivery and 1704 Commission Bill forms. */
@Controller
@RequestMapping("/commission/trading")
public class CmtrTransactionController {
    private final CmtrTransactionService service;

    public CmtrTransactionController(CmtrTransactionService service) { this.service = service; }

    @GetMapping("/loading-delivery-direct")
    public String loadingDeliveryDirect(Model model) {
        service.requireView(CmtrTransactionService.LOADING_DIRECT);
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("transactionType", CmtrTransactionService.LOADING_DIRECT);
        model.addAttribute("screenId", 518);
        model.addAttribute("documentTypeId", 1702);
        model.addAttribute("pageTitle", "Comm Trade Loading Delivery Direct");
        return "cmagt/commission/cmtr_transaction";
    }

    @GetMapping("/commission-bill")
    public String commissionBill(Model model) {
        service.requireView(CmtrTransactionService.COMMISSION_BILL);
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("transactionType", CmtrTransactionService.COMMISSION_BILL);
        model.addAttribute("screenId", 519);
        model.addAttribute("documentTypeId", 1704);
        model.addAttribute("pageTitle", "Commision Bill");
        return "cmagt/commission/cmtr_transaction";
    }

    @GetMapping("/api/transaction/{type}/init")
    @ResponseBody public ResponseEntity<?> initialize(@PathVariable String type) {
        return run(() -> service.initialize(type));
    }

    @GetMapping("/api/transaction/{type}/history")
    @ResponseBody public ResponseEntity<?> history(@PathVariable String type, @RequestParam Map<String,String> filters) {
        return run(() -> service.history(type, filters));
    }

    @GetMapping("/api/transaction/{type}/record")
    @ResponseBody public ResponseEntity<?> record(@PathVariable String type, @RequestParam int id) {
        return run(() -> service.record(type, id));
    }

    @GetMapping("/api/transaction/{type}/pending-loading")
    @ResponseBody public ResponseEntity<?> pendingLoading(@PathVariable String type, @RequestParam Map<String,String> filters) {
        return run(() -> service.pendingLoading(type, filters));
    }

    @PostMapping("/api/transaction/{type}/save")
    @ResponseBody public ResponseEntity<?> save(@PathVariable String type, @RequestBody Map<String,Object> body) {
        return run(() -> service.save(type, body));
    }

    @PostMapping("/api/transaction/{type}/delete")
    @ResponseBody public ResponseEntity<?> delete(@PathVariable String type, @RequestBody Map<String,Object> body) {
        return run(() -> service.delete(type, body.get("id") instanceof Number
                ? ((Number) body.get("id")).intValue() : Integer.parseInt(String.valueOf(body.get("id")))));
    }

    private static ResponseEntity<?> run(Call call) {
        try { return ResponseEntity.ok(call.get()); }
        catch (IllegalArgumentException ex) { return ResponseEntity.badRequest().body(Collections.singletonMap("message", ex.getMessage())); }
        catch (org.springframework.security.access.AccessDeniedException ex) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Collections.singletonMap("message", ex.getMessage())); }
        catch (Exception ex) {
            String message = ex.getMessage() == null || ex.getMessage().isBlank() ? "Unable to process this transaction." : ex.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(Collections.singletonMap("message", message));
        }
    }

    @FunctionalInterface private interface Call { Object get() throws Exception; }
}
