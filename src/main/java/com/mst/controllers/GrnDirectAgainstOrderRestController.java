package com.mst.controllers;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.GrnDirectAgainstOrderService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * JSON API of GRN Direct Against Order - Architecture.WinApp.Purchase.GRNDirectAgainstOrder (ScreenDefinition 133,
 * DocumentTypeId 169). The page is GrnDirectAgainstOrderViewController (/purchase/grn-direct-against-order); every rule lives in
 * GrnDirectAgainstOrderService. Tenancy is the session's everywhere - no request parameter carries organization / company /
 * branch / year / user. (Replaces the earlier fabricated /api/grn-direct-against-order endpoints: GenerategpCode numbering, a raw
 * SupplierCustomer query, gate-pass history and a gate-pass delete - none of them the form's.)
 */
@RestController
@RequestMapping("/api/purchase/grn-direct-against-order")
public class GrnDirectAgainstOrderRestController {

    private final GrnDirectAgainstOrderService service;

    public GrnDirectAgainstOrderRestController(GrnDirectAgainstOrderService service) { this.service = service; }

    /** InvFrmGRN_Load :1582 - rights, combos, doc no. */
    @GetMapping("/setup")
    public ResponseEntity<?> setup() { return HrmApi.run(service::setup); }

    /** toolStripButton1_Click (Refresh) :2210 */
    @GetMapping("/refresh")
    public ResponseEntity<?> refresh(@RequestParam(value = "orderId", defaultValue = "0") int orderId) {
        return HrmApi.run(() -> service.refresh(orderId));
    }

    /** GenerateCode :931 (reset) */
    @GetMapping("/code")
    public ResponseEntity<?> code() { return HrmApi.run(service::code); }

    /** PackUOMFillWithoutOrder :1123 (combitem ValueChanged / Leave) */
    @GetMapping("/uoms")
    public ResponseEntity<?> uoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> service.uoms(itemId)); }

    /** ItemNameFill :1064 */
    @GetMapping("/order-items")
    public ResponseEntity<?> orderItems(@RequestParam(value = "orderId", defaultValue = "0") int orderId) {
        return HrmApi.run(() -> service.orderItems(orderId));
    }

    /** cmbOrderNo_Leave :2680 (orderNo = the order's DocNo, as the form sends cmbOrderNo.Text) */
    @GetMapping("/order-leave")
    public ResponseEntity<?> orderLeave(@RequestParam("orderNo") int orderNo) { return HrmApi.run(() -> service.orderLeave(orderNo)); }

    /** BtnLoadOrder_Click :2499 - the LoadPurchaseOrder list (DocumentTypeId 41). */
    @GetMapping("/order-loader")
    public ResponseEntity<?> orderLoader(@RequestParam(value = "supplierId", defaultValue = "0") int supplierId,
                                         @RequestParam(value = "fromDate", required = false) String fromDate,
                                         @RequestParam(value = "toDate", required = false) String toDate,
                                         @RequestParam(value = "fromDocNo", required = false) String fromDocNo,
                                         @RequestParam(value = "toDocNo", required = false) String toDocNo) {
        return HrmApi.run(() -> service.pendingOrders(supplierId, fromDate, toDate, fromDocNo, toDocNo));
    }

    /** LoadInGridDetail :2515 - body {ids: [purchase order ids in the picked order]}. */
    @PostMapping("/load-orders")
    public ResponseEntity<?> loadOrders(@RequestBody Map<String, Object> body) {
        List<Integer> ids = new ArrayList<>();
        Object v = body == null ? null : body.get("ids");
        if (v instanceof List) for (Object o : (List<?>) v) {
            try { ids.add(o instanceof Number ? ((Number) o).intValue() : Integer.parseInt(String.valueOf(o).trim())); }
            catch (NumberFormatException e) { /* not an id - skipped */ }
        }
        return HrmApi.run(() -> service.loadOrders(ids));
    }

    /** HistoryGridFill(NoOfRecords) :1743 - 50 on the tab change, 0 (all) on LoadAll. */
    @GetMapping("/history")
    public ResponseEntity<?> history(@RequestParam(value = "noOfRecords", defaultValue = "0") int noOfRecords) {
        return HrmApi.run(() -> service.history(noOfRecords));
    }

    /** ReadById :637 / the history Detail button :2004 */
    @GetMapping("/by-id")
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return HrmApi.run(() -> service.byId(id)); }

    /** btnSave_Click :608 / btnUpdate_Click :621 -> Insert() :359 */
    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> service.save(body)); }
}
