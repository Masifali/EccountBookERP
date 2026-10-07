package com.mst.controllers;

import com.mst.models.dto.PurchaseOrderPmRequest;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.PurchaseOrderPmService;
import java.util.LinkedHashMap;
import java.util.Map;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;
import javax.servlet.http.HttpServletRequest;

/**
 * Screen 498 "1002 Purchsae Order" - Packing Material module 54.
 * Desktop {@code Architecture.WinApp.PackingMaterial_Store.PurchsaeOrderPmNew}, DocumentTypeId 700.
 */
@Controller
public class PurchaseOrderPmController {

    private static final String API = "/api/packing-material/purchase-order";
    private static final String API1001 = "/api/packing-material/purchase-order-1001";

    private final PurchaseOrderPmService service;

    public PurchaseOrderPmController(PurchaseOrderPmService service) {
        this.service = service;
    }

    @GetMapping({"/packing-material/purchase-order", "/packing-material/po", "/packing/purchase-order", "/packing-material/purchase-order-1001"})
    public String page(Model model, HttpServletRequest request) {
        model.addAttribute("activeMenu", "packing-material");
        boolean legacy1001 = request.getRequestURI().endsWith("purchase-order-1001");
        model.addAttribute("screenCaption", legacy1001 ? "1001 Purchase Order" : "1002 Purchsae Order");
        model.addAttribute("purchaseOrderApiBase", legacy1001 ? API1001 : API);
        return "packing_material/purchase_order_pm";
    }

    @GetMapping({API + "/lookups", API1001 + "/lookups"})
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping({API + "/numbers", API1001 + "/numbers"})
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not generate the document number."); }

    @GetMapping({API + "/item-defaults", API1001 + "/item-defaults"})
    @ResponseBody
    public ResponseEntity<?> itemDefaults(@RequestParam(defaultValue = "0") int itemId,
                                          @RequestParam(defaultValue = "0") int supplierId,
                                          @RequestParam String docDate) {
        return run(() -> service.itemDefaults(itemId, supplierId, docDate), "Record Not Found");
    }

    @GetMapping({API + "/ref-docs", API1001 + "/ref-docs"})
    @ResponseBody
    public ResponseEntity<?> refDocs(@RequestParam(defaultValue = "0") int refDocumentTypeId,
                                     @RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.refDocs(refDocumentTypeId, recId), "Could not load reference documents.");
    }

    @GetMapping({API + "/exchange-rate", API1001 + "/exchange-rate"})
    @ResponseBody
    public ResponseEntity<?> exchangeRate(@RequestParam(defaultValue = "0") int currencyId) {
        return run(() -> service.exchangeRate(currencyId), "Could not read the exchange rate.");
    }

    @GetMapping({API + "/history", API1001 + "/history"})
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateMode,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierId,
                                     @RequestParam(required = false) String branchIds) {
        return run(() -> service.history(dateMode, fromDate, toDate, fromDocNo, toDocNo, supplierId, branchIds), "History failed.");
    }

    @GetMapping({API + "/{id}", API1001 + "/{id}"})
    @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) { return run(() -> service.record(id), "Could not open that order."); }

    @PostMapping({API + "/save", API1001 + "/save"})
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchaseOrderPmRequest request) { return run(() -> service.save(request), "Save failed."); }

    @GetMapping({API + "/{id}/attachments/{attachmentId}", API1001 + "/{id}/attachments/{attachmentId}"})
    public ResponseEntity<byte[]> attachment(@PathVariable int id, @PathVariable int attachmentId) {
        DesktopInventoryItemFileService.Download d = service.attachment(id, attachmentId);
        HttpHeaders h = new HttpHeaders();
        h.setContentType(MediaType.parseMediaType(d.type()));
        h.setContentDisposition(ContentDisposition.attachment().filename(d.name()).build());
        return new ResponseEntity<>(d.bytes(), h, HttpStatus.OK);
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (ResponseStatusException e) {
            return ResponseEntity.status(e.getStatus()).body(fail(e.getReason()));
        } catch (Exception e) {
            String m = e.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(m == null || m.isBlank() ? fallback : m));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
