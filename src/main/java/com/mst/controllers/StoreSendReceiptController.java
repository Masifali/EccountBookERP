package com.mst.controllers;

import com.mst.models.dto.StoreSendReceiptRequest;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.StoreSendReceiptService;
import java.util.*;
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
 * Screen 504 "Store Send Receipt" - Packing Material module 54.
 * Desktop {@code Architecture.WinApp.PackingMaterial_Store.StoreSendReceipt}, DocumentTypeId 177.
 */
@Controller
public class StoreSendReceiptController {

    private static final String API = "/api/packing-material/store-send-receipt";

    private final StoreSendReceiptService service;

    public StoreSendReceiptController(StoreSendReceiptService service) {
        this.service = service;
    }

    @GetMapping("/packing-material/store-send-receipt")
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/store_send_receipt";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "DocNo Not Found"); }

    @GetMapping(API + "/pending")
    @ResponseBody
    public ResponseEntity<?> pending() { return run(service::pending, "Could not load the loader."); }

    @GetMapping(API + "/avg-rate")
    @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int itemId, @RequestParam(defaultValue = "0") int conditionId,
                                     @RequestParam String docDate, @RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.avgRate(itemId, conditionId, docDate, recId), "Could not read the average rate.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int senderId, @RequestParam(defaultValue = "0") int receiverId) {
        return run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, senderId, receiverId), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) { return run(() -> service.record(id), "Record Not Found"); }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody StoreSendReceiptRequest request) { return run(() -> service.save(request), "Save failed."); }

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
