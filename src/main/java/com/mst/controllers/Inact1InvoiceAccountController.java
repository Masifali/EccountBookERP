package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.Inact1InvoiceAccountService;
import com.mst.services.Inact1InvoiceAccountService.Kind;
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
 * Screens 36 "Payment By Invoice" (PaymentByInvoiceAccount) and 35 "Receipt By Invoice" (ReceiptByInvoiceAccount): one template, two kinds.
 *   GET  /accounts/payment-by-invoice-account, /accounts/receipt-by-invoice-account    the form
 *   GET  /api/accounts/invoice-account/{kind}/load | refresh | generate-code            Load, btnRefresh_Click, VoucherNofill
 *   GET  .../balance?accountId=                                                          AccountCurrentBalance / CustomerCurrentBalance
 *   GET  .../invoices?supplierId=                                                        cmbsupcust_Leave grid
 *   GET  .../tax-schedule, .../cheques, .../pdc, .../advance-balance                     CmbTranType_Leave, cmbcashbankac_Leave, CmbCheqNo_Leave
 *   GET  .../by-doc-no?docNo=                                                            txtdocno_Leave (GetIdByDocNo + ReadById)
 *   POST .../history, .../save ; GET .../print-target                                    HistoryGridFill, Insert, print_Click
 * The View right of the screen gates every call (the form's own Save / Update / Print rights are checked by the service).
 */
@Controller
public class Inact1InvoiceAccountController {
    private static final String API = "/api/accounts/invoice-account/{kind}";

    @Autowired private Inact1InvoiceAccountService service;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopReportRights rights;

    private void gate(Kind k) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, k.screenId, "View");
    }

    @GetMapping("/accounts/payment-by-invoice-account")
    public String paymentPage(Model model) { return page(model, Kind.PAYMENT); }

    @GetMapping("/accounts/receipt-by-invoice-account")
    public String receiptPage(Model model) { return page(model, Kind.RECEIPT); }

    private String page(Model model, Kind k) {
        gate(k);
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", k == Kind.PAYMENT ? "Payment By Invoice" : "Receipt By Invoice");
        model.addAttribute("invoiceKind", k == Kind.PAYMENT ? "payment" : "receipt");
        return "accounts/inact1_invoice_account";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load(@PathVariable String kind) { Kind k = Kind.of(kind); return run(() -> { gate(k); return service.load(k); }); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@PathVariable String kind) { Kind k = Kind.of(kind); return run(() -> { gate(k); return service.refresh(k); }); }

    @GetMapping(API + "/generate-code") @ResponseBody
    public ResponseEntity<?> generateCode(@PathVariable String kind) { Kind k = Kind.of(kind); return run(() -> { gate(k); return service.generateCode(k); }); }

    @GetMapping(API + "/balance") @ResponseBody
    public ResponseEntity<?> balance(@PathVariable String kind, @RequestParam int accountId) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.balance(accountId); });
    }

    @GetMapping(API + "/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@PathVariable String kind, @RequestParam int supplierId) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.invoices(supplierId); });
    }

    @GetMapping(API + "/tax-schedule") @ResponseBody
    public ResponseEntity<?> taxSchedule(@PathVariable String kind, @RequestParam int taxTypeId, @RequestParam(required = false) String date) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.taxSchedule(taxTypeId, date); });
    }

    @GetMapping(API + "/cheques") @ResponseBody
    public ResponseEntity<?> cheques(@PathVariable String kind, @RequestParam int bankId) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.cheques(bankId); });
    }

    @GetMapping(API + "/pdc") @ResponseBody
    public ResponseEntity<?> pdc(@PathVariable String kind, @RequestParam int accountId, @RequestParam(defaultValue = "0") int cheqId) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.pdc(accountId, cheqId); });
    }

    @GetMapping(API + "/advance-balance") @ResponseBody
    public ResponseEntity<?> advanceBalance(@PathVariable String kind, @RequestParam int accountId) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.advanceBalance(k, accountId); });
    }

    @GetMapping(API + "/by-doc-no") @ResponseBody
    public ResponseEntity<?> byDocNo(@PathVariable String kind, @RequestParam int docNo) {
        Kind k = Kind.of(kind);
        return run(() -> { gate(k); int id = service.idByDocNo(k, docNo); return id > 0 ? service.read(k, id) : null; });
    }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@PathVariable String kind, @RequestBody Map<String, Object> body) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.history(k, body); });
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@PathVariable String kind, @RequestBody Map<String, Object> body) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.save(k, body); });
    }

    @GetMapping(API + "/print-target") @ResponseBody
    public ResponseEntity<?> printTarget(@PathVariable String kind, @RequestParam int id) {
        Kind k = Kind.of(kind); return run(() -> { gate(k); return service.printTarget(k, id); });
    }

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
            boolean bad = e instanceof IllegalArgumentException || e instanceof IllegalStateException;
            return ResponseEntity.status(bad ? HttpStatus.OK : HttpStatus.INTERNAL_SERVER_ERROR).body(m);
        }
    }
}
