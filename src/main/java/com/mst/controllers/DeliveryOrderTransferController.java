package com.mst.controllers;

import com.mst.models.dto.DeliveryOrderTransferDto;
import com.mst.services.DeliveryOrderTransferService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Management, ModuleId 24 — screen 331 "Delivery Order (Stock Transfer)": Architecture.WinApp.Sale.DeliveryOrder
 * (DeliveryOrder.cs) opened with ScreenName / Tag "DeliveryOrderTransfer", DocumentTypeId 84, DO Type 2 "StockTransfer",
 * at /store/delivery-order-transfer. See {@link DeliveryOrderTransferService} for the desktop behaviour reproduced,
 * the deviations and what is not ported.
 */
@Controller
public class DeliveryOrderTransferController {

    private static final String API = "/api/store/delivery-order-transfer";

    private final DeliveryOrderTransferService service;
    public DeliveryOrderTransferController(DeliveryOrderTransferService service) { this.service = service; }

    @GetMapping("/store/delivery-order-transfer")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/delivery_order_transfer";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(service::docNo, "Could not generate the document number."); }

    @GetMapping(API + "/history-customers")
    @ResponseBody
    public ResponseEntity<?> historyCustomers() { return run(service::historyCustomers, "Could not load the customers."); }

    @GetMapping(API + "/available-stock")
    @ResponseBody
    public ResponseEntity<?> availableStock(@RequestParam(defaultValue = "0") int itemId,
                                            @RequestParam(defaultValue = "0") int warehouseId,
                                            @RequestParam(defaultValue = "0") int jobLotId,
                                            @RequestParam(required = false) String cropYear,
                                            @RequestParam(required = false) String docDate,
                                            @RequestParam(defaultValue = "0") int packingTypeId,
                                            @RequestParam(defaultValue = "0") int uomId) {
        return run(() -> service.availableStock(itemId, warehouseId, jobLotId, cropYear, docDate, packingTypeId, uomId),
                "Could not read the available stock.");
    }

    @GetMapping(API + "/current-stock")
    @ResponseBody
    public ResponseEntity<?> currentStock(@RequestParam(defaultValue = "0") int itemId,
                                          @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(required = false) String cropYear,
                                          @RequestParam(required = false) String docDate,
                                          @RequestParam(defaultValue = "0") int packingTypeId,
                                          @RequestParam(defaultValue = "0") int uomId) {
        return run(() -> service.currentStock(itemId, warehouseId, jobLotId, cropYear, docDate, packingTypeId, uomId),
                "Could not read the current stock.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateMode,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int docNoFrom,
                                     @RequestParam(defaultValue = "0") int docNoTo,
                                     @RequestParam(defaultValue = "0") int supplierCustomerId) {
        return run(() -> service.history(dateMode, fromDate, toDate, docNoFrom, docNoTo, supplierCustomerId), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that document.")));
        }
    }

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) { return slipOf(id, false); }

    @GetMapping(API + "/{id}/slip264")
    @ResponseBody
    public ResponseEntity<?> slip264(@PathVariable int id) { return slipOf(id, true); }

    private ResponseEntity<?> slipOf(int id, boolean challan264) {
        try {
            List<Map<String, Object>> rows = service.slip(id, challan264);
            if (rows == null || rows.isEmpty()) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(challan264 ? "Record Not Found For Display" : "No Record Found For Display"));
            }
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody DeliveryOrderTransferDto dto) {
        return write(() -> service.save(dto), "Save failed.");
    }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return write(() -> service.delete(id), "Delete failed.");
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (DeliveryOrderTransferService.RightsException e) {           // only a missing right is 403
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (IllegalArgumentException | IllegalStateException e) {       // validation, DesktopProc, no saved id
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(r, fallback)));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
