package com.mst.controllers.sale;

import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.SaleInvoiceGdnNoWbService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Sale Invoice Against GDN Without WB (frmSaleInvoiceAgainstGdnWithoutWb, 171). Every endpoint is one
 * desktop event or BLL call; tenancy, user, branch and financial year come from the signed-in session.
 */
@RestController
@RequestMapping("/sale/sale-invoice-gdn-no-wb/api")
public class SaleInvoiceGdnNoWbController {
    private static final Logger LOG = LoggerFactory.getLogger(SaleInvoiceGdnNoWbController.class);
    private final SaleInvoiceGdnNoWbService service;

    public SaleInvoiceGdnNoWbController(SaleInvoiceGdnNoWbService service) { this.service = service; }

    /** Load (refresh=false) or btnRefresh_Click (refresh=true). */
    @GetMapping("/lookups")
    public Map<String, Object> lookups(@RequestParam(name = "refresh", defaultValue = "false") boolean refresh) { return service.lookups(refresh); }

    /** Reset -> DocumentNo. */
    @GetMapping("/next-codes")
    public Map<String, Object> nextCodes() { return service.nextCodes(); }

    /** cmbCurrency_Leave - last exchange rate on DocumentTypeId 171. */
    @GetMapping("/exchange-rate/{currencyId}")
    public Map<String, Object> exchangeRate(@PathVariable("currencyId") int currencyId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rate", service.lastExchangeRate(currencyId));
        return m;
    }

    /** frmLoadGDN with DocumentTypeIds "170,221". */
    @GetMapping("/pending-gdns")
    public Map<String, Object> pendingGdns(@RequestParam(name = "from", required = false) String from,
                                           @RequestParam(name = "to", required = false) String to,
                                           @RequestParam(name = "branchIds", required = false) String branchIds) {
        return service.pendingGdns(from, to, branchIds);
    }

    @GetMapping("/gdn-detail/{id}")
    public List<Map<String, Object>> gdnDetail(@PathVariable("id") int id) { return service.gdnDetail(id); }

    /** BtnLoadGdn_Click -> LoadInGridDetail. */
    @PostMapping("/load-gdns")
    @SuppressWarnings("unchecked")
    public Map<String, Object> loadGdns(@RequestBody Map<String, Object> body) {
        List<Integer> ids = new ArrayList<>();
        Object raw = body.get("gdnIds");
        if (raw instanceof List) for (Object o : (List<Object>) raw) ids.add(SaleInvoiceRepository.i(o));
        return service.loadGdns(ids);
    }

    /** GetAll. */
    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(name = "dateMode", required = false) String dateMode,
                                             @RequestParam(name = "from", required = false) String from,
                                             @RequestParam(name = "to", required = false) String to,
                                             @RequestParam(name = "fromDocNo", required = false) Integer fromDocNo,
                                             @RequestParam(name = "toDocNo", required = false) Integer toDocNo,
                                             @RequestParam(name = "customerId", required = false) Integer customerId) {
        return service.history(dateMode, from, to, fromDocNo, toDocNo, customerId);
    }

    /** ReadById / GetDetailGrdByHeadId. */
    @GetMapping("/{id}")
    public Map<String, Object> read(@PathVariable("id") int id) { return service.read(id); }

    /** btnSave_Click / btnUpdate_Click -> Insert(). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    /** btnDelete_Click. */
    @DeleteMapping("/{id}")
    public Map<String, Object> delete(@PathVariable("id") int id) {
        service.delete(id);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("message", "Delete Record Seccessfully");
        return m;
    }

    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class, UnsupportedOperationException.class})
    public ResponseEntity<Map<String, Object>> badRequest(RuntimeException e) { return error(HttpStatus.BAD_REQUEST, e.getMessage()); }

    @ExceptionHandler(NoSuchElementException.class)
    public ResponseEntity<Map<String, Object>> notFound(RuntimeException e) { return error(HttpStatus.NOT_FOUND, e.getMessage()); }

    @ExceptionHandler(SecurityException.class)
    public ResponseEntity<Map<String, Object>> forbidden(RuntimeException e) { return error(HttpStatus.FORBIDDEN, e.getMessage()); }

    /** A procedure's RAISERROR (USP_InventoryValidation, USP_VoucherBalanceCheck) reaches the user in its own words. */
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(org.springframework.dao.DataAccessException e) {
        LOG.warn("Sale Invoice (171) database error", e);
        Throwable root = e.getMostSpecificCause();
        return error(HttpStatus.BAD_REQUEST, root == null ? e.getMessage() : root.getMessage());
    }

    private static ResponseEntity<Map<String, Object>> error(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message == null ? "The request could not be completed" : message);
        return ResponseEntity.status(status).body(m);
    }
}
