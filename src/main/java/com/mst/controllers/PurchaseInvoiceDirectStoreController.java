package com.mst.controllers;

import com.mst.models.dto.PurchaseInvoiceDirectStoreDto;
import com.mst.services.PurchaseInvoiceDirectStoreService;
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
import java.util.List;
import java.util.Map;

/**
 * Store Purchase, ModuleId 67 — screen 334 "Purchase Invoice Direct Store",
 * {@code frmPurchaseInvoiceDirectStore.cs}, DocumentTypeId 61, at /store/purchase-invoice-direct-store.
 * See {@link PurchaseInvoiceDirectStoreService} for the desktop trace, reproduced behaviour and deviations.
 */
@Controller
public class PurchaseInvoiceDirectStoreController {

    private static final String API = "/api/store/purchase-invoice-direct-store";

    private final PurchaseInvoiceDirectStoreService service;
    public PurchaseInvoiceDirectStoreController(PurchaseInvoiceDirectStoreService service) { this.service = service; }

    @GetMapping("/store/purchase-invoice-direct-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/purchase_invoice_direct_store";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not generate the Doc No."); }

    @GetMapping(API + "/history-branches")
    @ResponseBody
    public ResponseEntity<?> historyBranches() { return run(service::historyBranchList, "Could not load branches."); }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int itemConditionId,
                                   @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int rackId) {
        return run(() -> service.stock(itemId, docDate, itemConditionId, warehouseId, rackId), "Could not read the stock.");
    }

    @GetMapping(API + "/history-suppliers")
    @ResponseBody
    public ResponseEntity<?> historySuppliers(@RequestParam(required = false) String branchIds,
                                              @RequestParam(defaultValue = "false") boolean validate) {
        return run(() -> service.historySuppliers(branchIds, validate), "Could not load suppliers.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierCustomerId,
                                     @RequestParam(required = false) String branchIds) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, supplierCustomerId, branchIds),
                "History failed.");
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
    public ResponseEntity<?> slip(@PathVariable int id) { return report(() -> service.slip(id)); }

    @GetMapping(API + "/{id}/voucher")
    @ResponseBody
    public ResponseEntity<?> voucher(@PathVariable int id) { return report(() -> service.voucher118(id)); }

    @GetMapping(API + "/{id}/voucher-slip")
    @ResponseBody
    public ResponseEntity<?> voucherSlip(@PathVariable int id) { return report(() -> service.voucher103(id)); }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchaseInvoiceDirectStoreDto dto) { return write(() -> service.save(dto)); }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return write(() -> service.delete(id)); }

    @PostMapping(API + "/{id}/detail/{detailId}/check-delete")
    @ResponseBody
    public ResponseEntity<?> checkDetailDelete(@PathVariable int id, @PathVariable int detailId) {
        return write(() -> service.checkDetailDelete(id, detailId));
    }

    // ------------------------------------------------------------------------------ plumbing

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> report(Call c) {
        try {
            Object rows = c.get();
            if (rows == null || (rows instanceof List && ((List<?>) rows).isEmpty())) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            }
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), "Print failed.")));
        }
    }

    private static ResponseEntity<?> write(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(root(e), "Save failed.")));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
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
