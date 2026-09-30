package com.mst.controllers;

import com.mst.services.PurchaseReportService;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;
import org.springframework.http.CacheControl;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

/**
 * Purchase Reports (module 52) API. Business refusals are IllegalArgumentException (HTTP 400 with the
 * desktop's message, ApiExceptionAdvice); missing grants are AccessDeniedException (403).
 *
 *   /grn/*              477 frmGRNHistory
 *   /gate-pass/*        478 frmGatePassReport
 *   /purchase-order/*   479 PurchaseOrderHistory
 *   /purchase-register/* 480 PurchaseRegisterNew
 *   /stock-in-transit/* 869 frmSupplierDispatchPreBillReport
 */
@RestController
@RequestMapping("/purchase/api/reports")
public class PurchaseReportRestController {

    private final PurchaseReportService service;

    public PurchaseReportRestController(PurchaseReportService service) { this.service = service; }

    // ---------------------------------------------------------------- 477 Grn Report
    @GetMapping("/grn/init") public Map<String, Object> grnInit() { return service.grnInit(); }
    @GetMapping("/grn/lookups") public List<Map<String, Object>> grnLookups(@RequestParam List<Integer> branchIds) { return service.grnLookups(branchIds); }
    @PostMapping("/grn/rows") public Map<String, Object> grnRows(@RequestBody Map<String, Object> body) { return service.grnRows(body); }
    @PostMapping("/grn/print-args") public Map<String, Object> grnPrintArgs(@RequestBody Map<String, Object> body) { return service.grnPrintArgs(body); }

    // ---------------------------------------------------------------- 478 Gate Pass Report
    @GetMapping("/gate-pass/init") public Map<String, Object> gatePassInit() { return service.gatePassInit(); }
    @GetMapping("/gate-pass/lookups") public List<Map<String, Object>> gatePassLookups(@RequestParam List<Integer> branchIds) { return service.gatePassLookups(branchIds); }
    @PostMapping("/gate-pass/rows") public Map<String, Object> gatePassRows(@RequestBody Map<String, Object> body) { return service.gatePassRows(body); }

    // ---------------------------------------------------------------- 479 Purchase Order Report
    @GetMapping("/purchase-order/init") public Map<String, Object> poInit() { return service.purchaseOrderInit(); }
    @GetMapping("/purchase-order/lookups") public List<Map<String, Object>> poLookups(@RequestParam List<Integer> branchIds) { return service.purchaseOrderLookups(branchIds); }
    @GetMapping("/purchase-order/status") public List<Double> poStatus() { return service.purchaseOrderStatus(); }
    @PostMapping("/purchase-order/detail") public Map<String, Object> poDetail(@RequestBody Map<String, Object> body) { return service.purchaseOrderDetail(body); }
    @PostMapping("/purchase-order/summary") public Map<String, Object> poSummary(@RequestBody Map<String, Object> body) { return service.purchaseOrderSummary(body); }
    @PostMapping("/purchase-order/action") public Map<String, Object> poAction(@RequestBody Map<String, Object> body) { return service.purchaseOrderAction(body); }
    @PostMapping("/purchase-order/approval") public Map<String, Object> poApproval(@RequestBody Map<String, Object> body) { return service.purchaseOrderApproval(body); }

    // ---------------------------------------------------------------- 480 Purchase Report (With Activites)
    @GetMapping("/purchase-register/init") public Map<String, Object> registerInit() { return service.registerInit(); }
    @GetMapping("/purchase-register/detail-lookups") public List<Map<String, Object>> registerDetailLookups(@RequestParam List<Integer> branchIds) { return service.registerDetailLookups(branchIds); }
    @GetMapping("/purchase-register/summary-lookups") public List<Map<String, Object>> registerSummaryLookups(@RequestParam List<Integer> branchIds) { return service.registerSummaryLookups(branchIds); }
    @PostMapping("/purchase-register/detail") public Map<String, Object> registerDetail(@RequestBody Map<String, Object> body) { return service.registerDetail(body); }
    @PostMapping("/purchase-register/summary") public Map<String, Object> registerSummary(@RequestBody Map<String, Object> body) { return service.registerSummary(body); }
    @PostMapping("/purchase-register/approval") public Map<String, Object> registerApproval(@RequestBody Map<String, Object> body) { return service.registerApproval(body); }

    // ---------------------------------------------------------------- 869 Stock In Transit Report
    @GetMapping("/stock-in-transit/init") public Map<String, Object> transitInit() { return service.transitInit(); }
    @GetMapping("/stock-in-transit/lookups") public List<Map<String, Object>> transitLookups(@RequestParam List<Integer> branchIds) { return service.transitLookups(branchIds); }
    @PostMapping("/stock-in-transit/rows") public Map<String, Object> transitRows(@RequestBody Map<String, Object> body) { return service.transitRows(body); }

    // ---------------------------------------------------------------- shared links (screen = 477/478/479/480/869)
    @GetMapping("/{screen:[0-9]+}/gl-account")
    public Map<String, Object> glAccount(@PathVariable int screen, @RequestParam int supplierCustomerId) {
        return service.glAccount(screen, supplierCustomerId);
    }

    @GetMapping("/{screen:[0-9]+}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable int screen, @RequestParam int id, @RequestParam int documentTypeId) {
        return service.attachments(screen, id, documentTypeId);
    }

    @GetMapping("/{screen:[0-9]+}/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> attachment(@PathVariable int screen, @PathVariable int attachmentId,
                                             @RequestParam int id, @RequestParam int documentTypeId) {
        var file = service.attachment(screen, id, documentTypeId, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes());
    }
}
