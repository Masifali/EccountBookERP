package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleDeliveryOrderMfgEngrService;
import com.mst.services.sale.engr.SaleEngrAttachments;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 808 frmDeliveryOrderEngr (Architecture.WinApp.Mfg, module 134) - API under /sale/engr/mfg/delivery-order/api (page: SaleDeliveryOrderMfgEngrPageController). */
@RestController
@RequestMapping("/sale/engr/mfg/delivery-order/api")
public class SaleDeliveryOrderMfgEngrController extends SaleEngrControllerBase {
    private final SaleDeliveryOrderMfgEngrService service;

    public SaleDeliveryOrderMfgEngrController(SaleDeliveryOrderMfgEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    /** btnRefresh_Click */
    @GetMapping("/lists")
    public Map<String, Object> lists() { return service.lists(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    /** SaleOrderBind */
    @GetMapping("/orders")
    public List<Map<String, Object>> orders(@RequestParam int customerId, @RequestParam(defaultValue = "0") int recId) { return service.orders(customerId, recId); }

    /** ItemBindbyOrderId */
    @GetMapping("/order-items")
    public List<Map<String, Object>> orderItems(@RequestParam int orderId) { return service.orderItems(orderId); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/variants")
    public List<Map<String, Object>> variants(@RequestParam int itemId) { return service.variants(itemId); }

    @GetMapping("/balance")
    public Map<String, Object> balance(@RequestParam int orderId, @RequestParam(defaultValue = "0") int orderDetailId, @RequestParam int itemId) {
        return service.balance(orderId, orderDetailId, itemId);
    }

    @GetMapping("/history-customers")
    public List<Map<String, Object>> historyCustomers() { return service.historyCustomers(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/loader/init")
    public Map<String, Object> loaderInit() { return service.loaderInit(); }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos(@RequestParam(required = false) String branchIds) { return service.loaderCombos(branchIds); }

    @GetMapping("/loader/main")
    public List<Map<String, Object>> loaderMain(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                                @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int itemId,
                                                @RequestParam(required = false) String branchIds) {
        return service.loaderMain(fromDate, toDate, customerId, itemId, branchIds);
    }

    @GetMapping("/loader/detail")
    public List<Map<String, Object>> loaderDetail(@RequestParam(required = false) String ids, @RequestParam(required = false) String orderDetailIds) {
        return service.loaderDetail(ids, orderDetailIds);
    }

    @GetMapping("/loader/second")
    public List<Map<String, Object>> loaderSecond(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                                  @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int itemId,
                                                  @RequestParam(required = false) String branchIds) {
        return service.loaderSecond(fromDate, toDate, customerId, itemId, branchIds);
    }

    @GetMapping("/loader/rows")
    public List<Map<String, Object>> loaderRows(@RequestParam String orderDetailIds) { return service.loaderRows(orderDetailIds); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleDeliveryOrderMfgEngrService.Request request) { return service.save(request); }

    @DeleteMapping("/{id}")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }

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
