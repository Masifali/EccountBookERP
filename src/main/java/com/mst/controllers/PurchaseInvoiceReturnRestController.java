package com.mst.controllers;

import com.mst.services.PurchaseInvoiceReturnService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Purchase Invoice Return (InvfrmInvPurchaseInvoiceReturn, screen 125, DocumentTypeId 59) JSON API.
 * Business refusals come back as 400 with the desktop's message. There is no DELETE: the desktop form's Delete button is
 * hidden and has no handler (InvfrmInvPurchaseInvoiceReturn.cs designer, btnDelete.Visible = false).
 */
@RestController
@RequestMapping("/api/purchase/purchase-invoice-return")
public class PurchaseInvoiceReturnRestController {

    private final PurchaseInvoiceReturnService service;

    public PurchaseInvoiceReturnRestController(PurchaseInvoiceReturnService service) { this.service = service; }

    /** Load event: rights, configuration flags, every lookup, document numbers, history branches. */
    @GetMapping("/init")
    public ResponseEntity<?> init() { return run(service::init); }

    @GetMapping("/next-code")
    public ResponseEntity<?> numbers() { return run(service::numbers); }

    /** comItem_Leave -> UomFromGlobalBind. */
    @GetMapping("/uoms")
    public ResponseEntity<?> uoms(@RequestParam("itemId") int itemId) { return run(() -> service.uoms(itemId)); }

    /** AvailableStockGetByItem -> lblBalance. */
    @GetMapping("/stock")
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int itemId, @RequestParam(defaultValue = "0") int jobLotId,
                                   @RequestParam(required = false) String cropYear, @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int packingTypeId, @RequestParam(defaultValue = "0") int packUomId) {
        return run(() -> Map.of("balance", service.currentStock(warehouseId, itemId, jobLotId, cropYear, docDate, packingTypeId, packUomId)));
    }

    @GetMapping("/history-branches")
    public ResponseEntity<?> historyBranches() { return run(service::historyBranches); }

    /** HistoryComboFill: suppliers of the ticked branches (leading-comma list, as the desktop builds it). */
    @GetMapping("/history-suppliers")
    public ResponseEntity<?> historySuppliers(@RequestParam(value = "branchesIds", required = false) String branchesIds) { return run(() -> service.historySuppliers(branchesIds)); }

    @GetMapping("/history")
    public ResponseEntity<?> history(@RequestParam(value = "dateMode", required = false) String dateMode,
                                     @RequestParam(value = "fromDate", required = false) String fromDate,
                                     @RequestParam(value = "toDate", required = false) String toDate,
                                     @RequestParam(value = "fromDocNo", defaultValue = "0") int fromDocNo,
                                     @RequestParam(value = "toDocNo", defaultValue = "0") int toDocNo,
                                     @RequestParam(value = "supplierId", defaultValue = "0") int supplierId) {
        return run(() -> service.history(dateMode, fromDate, toDate, fromDocNo, toDocNo, supplierId));
    }

    @GetMapping("/{id:[0-9]+}")
    public ResponseEntity<?> read(@PathVariable("id") int id) { return run(() -> service.read(id)); }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> payload) { return run(() -> service.save(payload)); }

    // ------------------------------------------------------------------ Load Purchase Invoice dialog
    @GetMapping("/loader/init")
    public ResponseEntity<?> loaderInit() { return run(service::loaderInit); }

    @GetMapping("/loader/pending")
    public ResponseEntity<?> pending(@RequestParam("fromDate") String fromDate, @RequestParam("toDate") String toDate,
                                     @RequestParam(value = "branchesIds", required = false) String branchesIds) {
        return run(() -> service.pending(fromDate, toDate, branchesIds));
    }

    @SuppressWarnings("unchecked")
    @PostMapping("/loader/load")
    public ResponseEntity<?> load(@RequestBody Map<String, Object> body) {
        return run(() -> {
            List<Map<String, Object>> checked = new ArrayList<>();
            if (body.get("checked") instanceof List<?> l) for (Object o : l) if (o instanceof Map<?, ?> m) checked.add((Map<String, Object>) m);
            return service.load(checked, String.valueOf(body.get("fromDate")), String.valueOf(body.get("toDate")), body.get("branchesIds") == null ? "" : String.valueOf(body.get("branchesIds")));
        });
    }

    private interface Work { Object get(); }
    private static ResponseEntity<?> run(Work work) {
        try { return ResponseEntity.ok(work.get()); }
        catch (IllegalArgumentException e) { return fail(HttpStatus.BAD_REQUEST, e.getMessage()); }
        catch (org.springframework.dao.DataAccessException e) {
            // a procedure's RAISERROR (USP_InventoryValidation, USP_VoucherBalanceCheck, ...) reaches the user in its own words, as the desktop MessageBox
            Throwable cause = e.getMostSpecificCause();
            if (cause instanceof java.sql.SQLException sql && sql.getErrorCode() >= 50000) return fail(HttpStatus.BAD_REQUEST, sql.getMessage());
            org.slf4j.LoggerFactory.getLogger(PurchaseInvoiceReturnRestController.class).error("Purchase Invoice Return database operation failed", e);
            return fail(HttpStatus.INTERNAL_SERVER_ERROR, "The database request failed. No partial changes were saved.");
        }
        catch (IllegalStateException e) { return fail(HttpStatus.INTERNAL_SERVER_ERROR, e.getMessage()); }
    }
    private static ResponseEntity<?> fail(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", message == null || message.isBlank() ? "Request failed." : message);
        return ResponseEntity.status(status).body(m);
    }
}
