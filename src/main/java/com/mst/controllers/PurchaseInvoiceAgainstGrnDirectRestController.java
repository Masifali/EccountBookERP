package com.mst.controllers;

import com.mst.services.PurchaseInvoiceAgainstGrnDirectService;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Purchase Invoice Against GRN Direct (screen 131, DocumentTypeId 138). Organization, company, branch,
 * financial year and user always come from the session inside the service; rights come from tblUserRights
 * for ScreenName "PurchaseInvoiceAgainstGrnDirect".
 */
@RestController
@RequestMapping("/api/purchase-invoice-against-grn-direct")
public class PurchaseInvoiceAgainstGrnDirectRestController {

    private final PurchaseInvoiceAgainstGrnDirectService service;

    public PurchaseInvoiceAgainstGrnDirectRestController(PurchaseInvoiceAgainstGrnDirectService service) { this.service = service; }

    /** Form Load: rights, lookups, configuration and the next document number. */
    @GetMapping("/init")
    public Map<String, Object> init() { return service.init(); }

    /** Toolbar Refresh (btnFrmRefresh_Click). */
    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-code")
    public Map<String, Object> nextCode() { return Map.of("docNo", service.nextDocNo()); }

    @GetMapping("/last-exchange-rate")
    public Map<String, Object> lastExchangeRate(@RequestParam int currencyId) { return service.lastExchangeRate(currencyId); }

    @GetMapping("/history-suppliers")
    public List<Map<String, Object>> historySuppliers() { return service.historySuppliers(); }

    /** Load GRN -> LoadInGridDetail. */
    @PostMapping("/load-grns")
    public Map<String, Object> loadGrns(@RequestBody Map<String, Object> body) { return service.loadGrns(body); }

    @PostMapping("/history")
    public List<Map<String, Object>> history(@RequestBody(required = false) Map<String, Object> body) {
        return service.history(body == null ? Map.of() : body);
    }

    @GetMapping("/{id}/detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable int id) { return service.getById(id); }

    /** Save (id 0) and Update (id &gt; 0). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    @PostMapping("/delete/{id}")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }
}
