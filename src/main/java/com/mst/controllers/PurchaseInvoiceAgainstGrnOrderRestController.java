package com.mst.controllers;

import com.mst.services.PurchaseInvoiceAgainstGrnOrderService;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Purchase Invoice Against GRN Order (screen 132, DocumentTypeId 172). Organization, company, branch, financial year and
 * user always come from the session inside the service; rights come from tblUserRights for ScreenName
 * "PurchaseInvoiceAgainstGrnOrder". The desktop form has no Delete action (btnDelete is hidden and its handler is empty),
 * so there is no delete endpoint.
 */
@RestController
@RequestMapping("/api/purchase-invoice-against-grn-order")
public class PurchaseInvoiceAgainstGrnOrderRestController {

    private final PurchaseInvoiceAgainstGrnOrderService service;

    public PurchaseInvoiceAgainstGrnOrderRestController(PurchaseInvoiceAgainstGrnOrderService service) { this.service = service; }

    /** Form Load: rights, lookups, configuration and the next document number. */
    @GetMapping("/init")
    public Map<String, Object> init() { return service.init(); }

    /** Toolbar Refresh (btnFrmRefresh_Click). */
    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-code")
    public Map<String, Object> nextCode() { return Map.of("docNo", service.nextDocNo()); }

    /** Load GRN (loader selection, body {grnIds}) or Grn No leave (body {grnNo}) -> LoadInGridDetail. */
    @PostMapping("/load-grns")
    public Map<String, Object> loadGrns(@RequestBody Map<String, Object> body) { return service.loadGrns(body); }

    /** History tab: 50 records on tab select, all with LoadAll (noOfRecords 0). */
    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "50") int noOfRecords) { return service.history(noOfRecords); }

    @GetMapping("/{id}/detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable int id) { return service.getById(id); }

    /** Save (id 0) and Update (id &gt; 0). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }
}
