package com.mst.controllers;

import com.mst.models.dto.ItemStoreDto;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.ItemStoreService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.server.ResponseStatusException;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Store Management, ModuleId 24 - ScreenDefinition 329 "Item Store", ScreenName
 * {@code AddItemStore}, desktop {@code AddItemStore.cs}, at /store/item-store. A master
 * (item definition) screen: no DocumentTypeId, no print. See {@link ItemStoreService} for the
 * desktop-behaviour notes, deviations and what is not ported.
 */
@Controller
public class ItemStoreController {

    private static final String API = "/api/store/item-store";

    private final ItemStoreService service;
    public ItemStoreController(ItemStoreService service) { this.service = service; }

    @GetMapping("/store/item-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/item_store";
    }

    /** InvDefrmAddItem_Load :307 / toolStripButton1_Click :1216. */
    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    /** cmbItemCategory_Leave :536 / cmbItemType_Leave :1690. */
    @GetMapping(API + "/defaults")
    @ResponseBody
    public ResponseEntity<?> defaults(@RequestParam(defaultValue = "0") int categoryId,
                                      @RequestParam(defaultValue = "0") int typeId,
                                      @RequestParam(defaultValue = "false") boolean withAccounts) {
        return run(() -> service.defaults(categoryId, typeId, withAccounts), "Record Not Found");
    }

    /** grdfrmfill :1254 - noOfRecords 50 on the tab switch, 0 on Search / LoadAll. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int noOfRecords,
                                     @RequestParam(defaultValue = "0") int categoryId,
                                     @RequestParam(defaultValue = "0") int typeId) {
        return run(() -> service.history(noOfRecords, categoryId, typeId), "History failed.");
    }

    /** grdhistory_DoubleClick :1010. */
    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            return ResponseEntity.ok(service.record(id));
        } catch (ResponseStatusException e) {
            return ResponseEntity.status(e.getStatus()).body(fail(e.getReason() == null ? "Record Not Found" : e.getReason()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that item.")));
        }
    }

    @GetMapping(API + "/{id}/images/{imageId}")
    public ResponseEntity<?> image(@PathVariable int id, @PathVariable int imageId) {
        try {
            DesktopInventoryItemFileService.Download d = service.image(id, imageId);
            return ResponseEntity.ok()
                    .header(HttpHeaders.CONTENT_DISPOSITION, "inline")
                    .contentType(MediaType.parseMediaType(d.type()))
                    .body(d.bytes());
        } catch (ResponseStatusException e) {
            return ResponseEntity.status(e.getStatus()).build();
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
    }

    /** BtnSave_Click :759 (Id 0) and btnupdate_Click :896 (Id = RecId). */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody ItemStoreDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (ResponseStatusException e) {
            return ResponseEntity.status(e.getStatus()).body(fail(e.getReason() == null ? "Record Not Found" : e.getReason()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(r, "Save failed.")));
        }
    }

    private interface Call { Object get() throws Exception; }

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
