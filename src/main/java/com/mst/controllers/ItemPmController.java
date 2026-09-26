package com.mst.controllers;

import com.mst.models.dto.ItemPmRequest;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.ItemPmService;
import java.util.LinkedHashMap;
import java.util.List;
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
 * Screen 496 "Item PM" - Packing Material module (54). Desktop form
 * {@code Architecture.WinApp.StoreManagement.AddItemPM}.
 *
 * The route normalises to nothing else on purpose; it is linked from the dashboard by screen id
 * (DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID) and from the sidebar catalog.
 */
@Controller
public class ItemPmController {

    private static final String API = "/api/packing-material/item-pm";

    private final ItemPmService service;

    public ItemPmController(ItemPmService service) {
        this.service = service;
    }

    @GetMapping({"/packing-material/item-pm", "/packing-material/items", "/packing-material/item", "/packing/items", "/packing/item-pm"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/item_pm";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() {
        return run(service::lookups, "Could not load the screen.");
    }

    /** cmbItemCategory_Leave / cmbItemType_Leave. */
    @GetMapping(API + "/defaults")
    @ResponseBody
    public ResponseEntity<?> defaults(@RequestParam(defaultValue = "0") int categoryId,
                                      @RequestParam(defaultValue = "0") int typeId,
                                      @RequestParam(defaultValue = "false") boolean withAccounts) {
        return run(() -> service.defaults(categoryId, typeId, withAccounts), "Record Not Found");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int categoryId,
                                     @RequestParam(defaultValue = "0") int typeId,
                                     @RequestParam(defaultValue = "0") int masterItemId) {
        return run(() -> service.history(categoryId, typeId, masterItemId), "History failed.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) {
        return run(() -> service.record(id), "Could not open that item.");
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody ItemPmRequest request) {
        return run(() -> service.save(request), "Save failed.");
    }

    @PostMapping(API + "/master-items")
    @ResponseBody
    public ResponseEntity<?> masterItems(@RequestBody List<ItemPmRequest.MasterItemRow> rows) {
        return run(() -> service.updateMasterItems(rows), "Update failed.");
    }

    @GetMapping(API + "/{id}/images/{imageId}")
    public ResponseEntity<byte[]> image(@PathVariable int id, @PathVariable int imageId) {
        return file(service.image(id, imageId), true);
    }

    @GetMapping(API + "/{id}/attachments/{attachmentId}")
    public ResponseEntity<byte[]> attachment(@PathVariable int id, @PathVariable int attachmentId) {
        return file(service.attachment(id, attachmentId), false);
    }

    // ------------------------------------------------------------------ plumbing

    private static ResponseEntity<byte[]> file(DesktopInventoryItemFileService.Download d, boolean inline) {
        HttpHeaders h = new HttpHeaders();
        h.setContentType(MediaType.parseMediaType(d.type()));
        h.setContentDisposition((inline ? ContentDisposition.inline() : ContentDisposition.attachment()).filename(d.name()).build());
        return new ResponseEntity<>(d.bytes(), h, HttpStatus.OK);
    }

    private interface Call { Object get() throws Exception; }

    /** The desktop's own validation text reaches the operator unchanged. */
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
