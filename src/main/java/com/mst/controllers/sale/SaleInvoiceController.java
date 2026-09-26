package com.mst.controllers.sale;

import com.mst.services.SaleInvoiceService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Sale Invoice (InvfrmSaleInvoice, 95). Every endpoint maps to one desktop event or BLL call; see
 * SaleInvoiceService / SaleInvoiceRepository for the source line of each. Tenancy, user, branch and
 * financial year always come from the signed-in session.
 */
@RestController
@RequestMapping("/sale/sale-invoice/api")
public class SaleInvoiceController {
    private static final Logger LOG = LoggerFactory.getLogger(SaleInvoiceController.class);
    private final SaleInvoiceService service;

    public SaleInvoiceController(SaleInvoiceService service) { this.service = service; }

    /** InvfrmSaleInvoice InitializeComponentMethod / btnRefresh_Click. */
    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    /** Reset(): GenerateDocumentNo + GenerateBranchSrNo. */
    @GetMapping("/next-codes")
    public Map<String, Object> nextCodes() { return service.nextCodes(); }

    /** cmbsuppliername_ValueChanged - ledger balance. */
    @GetMapping("/ledger")
    public Map<String, Object> ledger(@RequestParam("customerId") int customerId, @RequestParam(name = "docDate", required = false) String docDate) {
        return service.ledger(customerId, docDate);
    }

    /** cmbCurrency_Leave - last exchange rate of the currency on Sale Invoices. */
    @GetMapping("/exchange-rate/{currencyId}")
    public Map<String, Object> exchangeRate(@PathVariable("currencyId") int currencyId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rate", service.lastExchangeRate(currencyId));
        return m;
    }

    /** frmLoadGDN - branches + pending GDNs (branchIds omitted = only the branch list). */
    @GetMapping("/pending-gdns")
    public Map<String, Object> pendingGdns(@RequestParam(name = "from", required = false) String from,
                                           @RequestParam(name = "to", required = false) String to,
                                           @RequestParam(name = "branchIds", required = false) String branchIds) {
        return service.pendingGdns(from, to, branchIds);
    }

    /** frmLoadGDN.grd_SelectionChanged - detail of the highlighted GDN. */
    @GetMapping("/gdn-detail/{id}")
    public List<Map<String, Object>> gdnDetail(@PathVariable("id") int id) { return service.gdnDetail(id); }

    /** toolStripButton3_Click_1 -> LoadInGridDetail. */
    @PostMapping("/load-gdns")
    @SuppressWarnings("unchecked")
    public Map<String, Object> loadGdns(@RequestBody Map<String, Object> body) {
        List<Integer> ids = new ArrayList<>();
        Object raw = body.get("gdnIds");
        if (raw instanceof List) for (Object o : (List<Object>) raw) ids.add(com.mst.repositories.SaleInvoiceRepository.i(o));
        Object customer = body.get("customerId");
        return service.loadGdns(ids, customer == null ? null : com.mst.repositories.SaleInvoiceRepository.i(customer),
                body.get("reserveStatus") == null ? null : String.valueOf(body.get("reserveStatus")));
    }

    /** LoadPaymentDetail - SaleOrderPaymentTermDetailBySoIds. */
    @PostMapping("/so-payment-terms")
    @SuppressWarnings("unchecked")
    public List<Map<String, Object>> soPaymentTerms(@RequestBody Map<String, Object> body) {
        List<Integer> ids = new ArrayList<>();
        Object raw = body.get("orderIds");
        if (raw instanceof List) for (Object o : (List<Object>) raw) ids.add(com.mst.repositories.SaleInvoiceRepository.i(o));
        return service.saleOrderPaymentTerms(ids);
    }

    /** CommissionAmountCalculateInCaseofPolicy - policy rates for the customer (1) or agent (2). */
    @GetMapping("/commission-policy")
    public List<Map<String, Object>> commissionPolicy(@RequestParam("customerId") int customerId,
                                                      @RequestParam(name = "docDate", required = false) String docDate,
                                                      @RequestParam(name = "itemIds", required = false) String itemIds,
                                                      @RequestParam(name = "policyTypeId", defaultValue = "0") int policyTypeId) {
        return service.commissionPolicy(customerId, docDate, itemIds, policyTypeId);
    }

    /** FillHistoryGrid. */
    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(name = "dateMode", required = false) String dateMode,
                                             @RequestParam(name = "from", required = false) String from,
                                             @RequestParam(name = "to", required = false) String to,
                                             @RequestParam(name = "fromDocNo", required = false) Integer fromDocNo,
                                             @RequestParam(name = "toDocNo", required = false) Integer toDocNo,
                                             @RequestParam(name = "customerId", required = false) Integer customerId,
                                             @RequestParam(name = "branchIds", required = false) String branchIds) {
        return service.history(dateMode, from, to, fromDocNo, toDocNo, customerId, branchIds);
    }

    /** ReadById. */
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
        m.put("message", "Delete Record Successfully");
        return m;
    }

    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class, UnsupportedOperationException.class})
    public ResponseEntity<Map<String, Object>> badRequest(RuntimeException e) { return error(HttpStatus.BAD_REQUEST, e.getMessage()); }

    @ExceptionHandler(NoSuchElementException.class)
    public ResponseEntity<Map<String, Object>> notFound(RuntimeException e) { return error(HttpStatus.NOT_FOUND, e.getMessage()); }

    @ExceptionHandler(SecurityException.class)
    public ResponseEntity<Map<String, Object>> forbidden(RuntimeException e) { return error(HttpStatus.FORBIDDEN, e.getMessage()); }

    /** A procedure's RAISERROR (e.g. USP_VoucherBalanceCheck) reaches the user with its own words, as the desktop's MessageBox did. */
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(org.springframework.dao.DataAccessException e) {
        LOG.warn("Sale Invoice database error", e);
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
