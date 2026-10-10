package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleSoMfgEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 779 frmSaleOrderEngr - API of /sale/engr/mfg/sale-order (page: SaleSoMfgEngrPageController). */
@RestController
@RequestMapping("/sale/engr/mfg/sale-order/api")
public class SaleSoMfgEngrController extends SaleEngrControllerBase {
    private final SaleSoMfgEngrService service;

    public SaleSoMfgEngrController(SaleSoMfgEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/lists")
    public Map<String, Object> lists() { return service.lists(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/variants")
    public List<Map<String, Object>> variants(@RequestParam int itemId) { return service.variants(itemId); }

    @GetMapping("/item-tax")
    public List<Map<String, Object>> itemTax(@RequestParam int itemId, @RequestParam(required = false) String date) { return service.itemTax(itemId, date); }

    @GetMapping("/tax-by-items")
    public List<Map<String, Object>> taxByItems(@RequestParam String itemIds, @RequestParam(required = false) String date) { return service.taxByItems(itemIds, date); }

    @GetMapping("/last-rate")
    public List<Map<String, Object>> lastRate(@RequestParam int currencyId) { return service.lastRate(currencyId); }

    @GetMapping("/history-branches")
    public Map<String, Object> historyBranches() { return service.historyBranches(); }

    @GetMapping("/history-customers")
    public List<Map<String, Object>> historyCustomers(@RequestParam(defaultValue = "") String branchIds, @RequestParam(defaultValue = "false") boolean validate) {
        return service.historyCustomers(branchIds, validate);
    }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "") String branchIds) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId, branchIds);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/{id}/history-detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @GetMapping("/{id}/slip-check")
    public Map<String, Object> slipCheck(@PathVariable int id) { return service.slipRows(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleSoMfgEngrService.Request request) { return service.save(request); }

    @GetMapping("/{id}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachmentList(id); }

    @GetMapping("/{id}/attachments/{attachmentId}")
    public ResponseEntity<byte[]> download(@PathVariable int id, @PathVariable int attachmentId) {
        SaleEngrAttachments.Download d = service.download(id, attachmentId);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename*=UTF-8''" + java.net.URLEncoder.encode(d.name(), StandardCharsets.UTF_8).replace("+", "%20"))
                .contentType(MediaType.APPLICATION_OCTET_STREAM).body(d.bytes());
    }
}
