package com.mst.controllers;

import com.mst.models.dto.GrnPmRequest;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.GrnPmService;
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

/**
 * Screen 500 "Goods Receipt Notes PM" - Packing Material module 54.
 * Desktop {@code Architecture.WinApp.PackingMaterial_Store.GrnPackingMaterial}, DocumentTypeId 701.
 */
@Controller
public class GrnPmController {

    private static final String API = "/api/packing-material/grn";

    private final GrnPmService service;

    public GrnPmController(GrnPmService service) {
        this.service = service;
    }

    @GetMapping({"/packing-material/grn", "/packing-material/store-grn", "/packing/grn"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/grn_pm";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Error occurred during database call."); }

    @GetMapping(API + "/fresh")
    @ResponseBody
    public ResponseEntity<?> fresh(@RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.fresh(recId), "Could not reset the form.");
    }

    @GetMapping(API + "/order-lines")
    @ResponseBody
    public ResponseEntity<?> orderLines(@RequestParam(defaultValue = "0") int orderId,
                                        @RequestParam(defaultValue = "0") int gpId) {
        return run(() -> service.orderLines(orderId, gpId), "Could not load the order.");
    }

    @GetMapping(API + "/order-loader")
    @ResponseBody
    public ResponseEntity<?> orderLoader(@RequestParam(defaultValue = "0") int supplierId,
                                         @RequestParam(required = false) String fromDate,
                                         @RequestParam(required = false) String toDate) {
        return run(() -> service.orderLoader(supplierId, fromDate, toDate), "Could not load purchase orders.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateMode,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierId,
                                     @RequestParam(defaultValue = "all") String referred) {
        return run(() -> service.history(dateMode, fromDate, toDate, fromDocNo, toDocNo, supplierId, referred), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) { return run(() -> service.record(id), "Record Not Found"); }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody GrnPmRequest request) { return run(() -> service.save(request), "Save failed."); }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return run(() -> service.delete(id), "Delete failed."); }

    @GetMapping(API + "/{id}/attachments/{attachmentId}")
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
