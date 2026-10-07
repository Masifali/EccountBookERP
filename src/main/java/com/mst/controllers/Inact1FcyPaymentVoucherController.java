package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.Inact1FcyPaymentVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screen 23 "Fcy Payment Voucher" (frmFcyPaymentVoucher, AcfrmFcyPaymentVoucher.cs).
 *   GET  /accounts/fcy-payment-voucher                                  the form
 *   GET  /api/accounts/fcy-payment-voucher/load | refresh | doc-no     Load, btnRefresh_Click, GenerateDocumentNo
 *   GET  .../suppliers?term= | lc-orders | invoices | invoice-amount | invoice-rate   PaymentTerm(), LcOrder(), InvoiceNo(), InvoiceAmountGet(), GetExchangeRateandCurrencyFromVoucher()
 *   GET  .../read?id= ; POST .../history ; POST .../save ; GET .../print-target
 * The View right of the screen gates every call; Save / Update / Print / CanView AllRecord are checked by the service.
 */
@Controller
public class Inact1FcyPaymentVoucherController {
    private static final String API = "/api/accounts/fcy-payment-voucher";

    @Autowired private Inact1FcyPaymentVoucherService service;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopReportRights rights;

    private void gate() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, Inact1FcyPaymentVoucherService.SCREEN_ID, "View");
    }

    @GetMapping("/accounts/fcy-payment-voucher")
    public String page(Model model) {
        gate();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Fcy Payment Voucher");
        return "accounts/inact1_fcy_payment_voucher";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(() -> { gate(); return service.load(); }); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(() -> { gate(); return service.refresh(); }); }

    @GetMapping(API + "/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> { gate(); return service.generateCode(); }); }

    @GetMapping(API + "/suppliers") @ResponseBody
    public ResponseEntity<?> suppliers(@RequestParam String term) { return run(() -> { gate(); return service.suppliers(term); }); }

    @GetMapping(API + "/lc-orders") @ResponseBody
    public ResponseEntity<?> lcOrders(@RequestParam int supplierId) { return run(() -> { gate(); return service.lcOrders(supplierId); }); }

    @GetMapping(API + "/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@RequestParam int supplierId) { return run(() -> { gate(); return service.invoices(supplierId); }); }

    @GetMapping(API + "/invoice-amount") @ResponseBody
    public ResponseEntity<?> invoiceAmount(@RequestParam int invoiceId) { return run(() -> { gate(); return service.invoiceAmount(invoiceId); }); }

    @GetMapping(API + "/invoice-rate") @ResponseBody
    public ResponseEntity<?> invoiceRate(@RequestParam int invoiceId) { return run(() -> { gate(); return service.invoiceRate(invoiceId); }); }

    @GetMapping(API + "/read") @ResponseBody
    public ResponseEntity<?> read(@RequestParam int id) { return run(() -> { gate(); return service.read(id); }); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) {
        return run(() -> { gate(); return service.history(Boolean.TRUE.equals(body.get("all"))); });
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> { gate(); return service.save(body); }); }

    @GetMapping(API + "/print-target") @ResponseBody
    public ResponseEntity<?> printTarget(@RequestParam int id) { return run(() -> { gate(); return service.printTarget(id); }); }

    private ResponseEntity<?> run(Supplier<Object> work) {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            m.put("success", true);
            m.put("data", work.get());
            return ResponseEntity.ok(m);
        } catch (AccessDeniedException e) {
            m.clear(); m.put("success", false); m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(m);
        } catch (SecurityException e) {
            m.clear(); m.put("success", false); m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(m);
        } catch (Exception e) {
            Throwable c = e;
            while (c.getCause() != null && c.getCause() != c) c = c.getCause();
            m.clear(); m.put("success", false); m.put("message", c.getMessage() == null ? e.getClass().getSimpleName() : c.getMessage());
            return ResponseEntity.ok(m);
        }
    }
}
