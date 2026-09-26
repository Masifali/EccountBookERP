package com.mst.controllers;

import com.mst.models.dto.PurchaseInvoiceReturnStoreDto;
import com.mst.services.PurchaseInvoiceReturnStoreService;
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

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Purchase (ModuleId 67) — screen 346 "Purchase Invoice Return Store",
 * {@code PurchaseInvoiceReturn_Store.cs}, DocumentTypeId 145, at /store/purchase-invoice-return-store.
 * The business rules live in {@link PurchaseInvoiceReturnStoreService}; this class only routes and maps
 * errors (a validation message → 400 {message}, a missing right → 403, anything else → 500).
 */
@Controller
public class PurchaseInvoiceReturnStoreController {

    private static final String API = "/api/store/purchase-invoice-return-store";

    private final PurchaseInvoiceReturnStoreService service;
    public PurchaseInvoiceReturnStoreController(PurchaseInvoiceReturnStoreService service) { this.service = service; }

    @GetMapping("/store/purchase-invoice-return-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/purchase_invoice_return_store";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(service::nextDocNo, "Could not generate the document number."); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping(API + "/reasons")
    @ResponseBody
    public ResponseEntity<?> reasons() { return run(service::reasons, "Could not read the reasons."); }

    @GetMapping(API + "/history-customers")
    @ResponseBody
    public ResponseEntity<?> historyCustomers() { return run(service::historyCustomers, "Could not read the customers."); }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int recId,
                                   @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int jobLotId,
                                   @RequestParam(defaultValue = "0") int uomId) {
        return run(() -> service.stock(recId, docDate, itemId, warehouseId, jobLotId, uomId), "Could not read the stock.");
    }

    @GetMapping(API + "/last-exchange-rate")
    @ResponseBody
    public ResponseEntity<?> lastExchangeRate(@RequestParam int currencyId) {
        return run(() -> service.lastExchangeRate(currencyId), "Could not read the exchange rate.");
    }

    @GetMapping(API + "/loader")
    @ResponseBody
    public ResponseEntity<?> loaderInit() { return run(service::loaderInit, "Could not open the loader."); }

    @GetMapping(API + "/pending-invoices")
    @ResponseBody
    public ResponseEntity<?> pending(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String branchIds) {
        try {
            List<Integer> ids = new ArrayList<>();
            if (branchIds != null) {
                for (String s : branchIds.split(",")) {
                    s = s.trim();
                    if (!s.isEmpty()) ids.add(Integer.valueOf(s));
                }
            }
            return ResponseEntity.ok(service.pendingInvoices(fromDate, toDate, ids));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(msg(e, "Select branch first")));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not load the invoices.")));
        }
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateMode,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierCustomerId) {
        return run(() -> service.history(dateMode, fromDate, toDate, fromDocNo, toDocNo, supplierCustomerId), "History failed.");
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

    /** 145-Print and 145A-Print (both read the same procedure). */
    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) {
        return rows(() -> service.slip(id));
    }

    @GetMapping(API + "/{id}/slip-subreport")
    @ResponseBody
    public ResponseEntity<?> slipSubReport(@PathVariable int id) {
        return rows(() -> service.slipSubReport(id));
    }

    /** 103-Voucher. */
    @GetMapping(API + "/voucher-slip/{voucherHeadId}")
    @ResponseBody
    public ResponseEntity<?> voucherSlip(@PathVariable int voucherHeadId) {
        return rows(() -> service.voucherSlip(voucherHeadId));
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchaseInvoiceReturnStoreDto dto) {
        return write(() -> service.save(dto), "Save failed.");
    }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return write(() -> service.delete(id), "Delete failed.");
    }

    // ------------------------------------------------------------------ frmDefineReasons

    @GetMapping(API + "/define-reasons")
    @ResponseBody
    public ResponseEntity<?> reasonsDialog() { return run(service::reasonsDialog, "Could not open Define Reasons."); }

    @GetMapping(API + "/define-reasons/history")
    @ResponseBody
    public ResponseEntity<?> reasonsHistory() { return run(service::reasonsHistory, "Could not read the reasons."); }

    @GetMapping(API + "/define-reasons/{id}")
    @ResponseBody
    public ResponseEntity<?> reason(@PathVariable int id) {
        try {
            Map<String, Object> m = service.reason(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not read the reason.")));
        }
    }

    @PostMapping(API + "/define-reasons/save")
    @ResponseBody
    public ResponseEntity<?> saveReason(@RequestBody PurchaseInvoiceReturnStoreDto.Reason dto) {
        return write(() -> service.saveReason(dto), "Save failed.");
    }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static ResponseEntity<?> rows(Call c) {
        try {
            Object r = c.get();
            if (r == null || (r instanceof List && ((List<?>) r).isEmpty())) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            }
            return ResponseEntity.ok(r);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            boolean right = e.getMessage() != null && e.getMessage().startsWith("You do not have");
            return ResponseEntity.status(right ? HttpStatus.FORBIDDEN : HttpStatus.BAD_REQUEST).body(fail(msg(e, fallback)));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.badRequest().body(fail(msg(r, fallback)));
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
