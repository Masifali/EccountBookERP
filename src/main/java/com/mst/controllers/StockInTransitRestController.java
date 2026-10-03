package com.mst.controllers;

import com.mst.models.dto.StockInTransitDto;
import com.mst.services.StockInTransitService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * JSON for screen 868 "Stock In Transit" (frmSupplierDispatchPreBill.cs, DocumentTypeId 251).
 * No parameter carries tenancy: organization, company, branch, financial year and user come from
 * the session inside {@link StockInTransitService}.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice);
 * rights are AccessDeniedException (403). A procedure's own RAISERROR (the financial-year and
 * server-date checks of USP_SupplierDispatch_InsertAndUpdate, "Record cannot be update because record
 * has been referred in Purchase Order n", the approved check of DeleteById) is what the desktop shows
 * in its "Database Error" box, so it is answered here as a 400 with that text.
 */
@RestController
@RequestMapping("/api/purchase/stock-in-transit")
public class StockInTransitRestController {

    private static final Logger LOG = LoggerFactory.getLogger(StockInTransitRestController.class);

    private final StockInTransitService service;
    public StockInTransitRestController(StockInTransitService service) { this.service = service; }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.formRefresh(); }

    @GetMapping("/numbers")
    public Map<String, Object> numbers() { return service.numbers(); }

    @GetMapping("/history-refresh")
    public Map<String, Object> historyRefresh() { return service.historyRefresh(); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam(defaultValue = "0") int itemId) { return service.uoms(itemId); }

    @GetMapping("/last-saved")
    public Map<String, Object> lastSaved(@RequestParam(defaultValue = "0") int supplierId) { return service.lastSaved(supplierId); }

    @GetMapping("/order-loader")
    public List<Map<String, Object>> orderLoader(@RequestParam(defaultValue = "0") int supplierId,
                                                 @RequestParam(required = false) String fromDate,
                                                 @RequestParam(required = false) String toDate,
                                                 @RequestParam(required = false) String fromDocNo,
                                                 @RequestParam(required = false) String toDocNo) {
        return service.pendingOrders(supplierId, fromDate, toDate, fromDocNo, toDocNo);
    }

    @GetMapping("/order-expenses")
    public List<Map<String, Object>> orderExpenses(@RequestParam(required = false) String orderIds) {
        return service.orderExpenses(orderIds);
    }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(required = false) String branchIds,
                                             @RequestParam(defaultValue = "doc") String dateType,
                                             @RequestParam(defaultValue = "true") boolean fromChecked,
                                             @RequestParam(required = false) String fromDate,
                                             @RequestParam(defaultValue = "true") boolean toChecked,
                                             @RequestParam(required = false) String toDate,
                                             @RequestParam(required = false) String fromDocNo,
                                             @RequestParam(required = false) String toDocNo) {
        return service.history(branchIds, dateType, fromChecked, fromDate, toChecked, toDate, fromDocNo, toDocNo);
    }

    @PostMapping("/status")
    public Map<String, Object> status(@RequestBody List<Map<String, Object>> rows) { return service.updateStatus(rows); }

    @GetMapping("/{id}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    @GetMapping("/{id}/print-check")
    public Map<String, Object> printCheck(@PathVariable int id) { return service.checkPrint(id); }

    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody StockInTransitDto dto) { return service.save(dto); }

    @PostMapping("/{id}/delete")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }

    /** A procedure's RAISERROR text — the message the desktop's "Database Error" box shows. */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Stock In Transit database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
