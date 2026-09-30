package com.mst.controllers;

import com.mst.services.PurchaseInvoiceReturnService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * frmLoadPurchaseInvoiceForReturn (the Purchase Invoice Return loader) under its older route. The page uses
 * /api/purchase/purchase-invoice-return/loader/*; this route delegates to the same service so both answer identically
 * (the previous implementation built SQL by string concatenation and fell back to raw table reads on failure).
 */
@RestController
@RequestMapping("/api/purchase-invoice-return-loader")
public class PurchaseInvoiceReturnLoaderRestController {

    private final PurchaseInvoiceReturnService service;

    public PurchaseInvoiceReturnLoaderRestController(PurchaseInvoiceReturnService service) { this.service = service; }

    @GetMapping("/branches")
    public ResponseEntity<?> branches() {
        try { return ResponseEntity.ok(service.loaderInit().get("branches")); } catch (IllegalArgumentException e) { return fail(e); }
    }

    @GetMapping("/pending")
    public ResponseEntity<?> pending(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String branchIds) {
        try { return ResponseEntity.ok(service.pending(fromDate, toDate, branchIds)); } catch (IllegalArgumentException e) { return fail(e); }
    }

    private static ResponseEntity<?> fail(IllegalArgumentException e) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", e.getMessage());
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(m);
    }
}
