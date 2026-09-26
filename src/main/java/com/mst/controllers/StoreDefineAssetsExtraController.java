package com.mst.controllers;

import com.mst.models.dto.StoreDefineAssetsExtraDto;
import com.mst.services.StoreDefineAssetsExtraService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * JSON API of the two forms the Define Asset dialog opens with its "+" buttons:
 * frmItemCatagoryStore FormTypeId 3 ("Item Category (Fix Assets)", ScreenName "frmItemCatagoryStore")
 * at /api/store/define/asset-category and frmDefineAssets ("Add Fixed Assest Item", ScreenName
 * "frmDefineAssets", ScreenDefinition 356) at /api/store/define/fixed-asset-item.
 * No page of their own — countx_store_define_lookups.js (StoreDefine.openAssetCategory /
 * openFixedAssetItem) builds the dialogs on the host page. Every call carries {@code host} = the
 * ScreenName of the page that opened the chain (rights). Errors → 400 {success:false, message}.
 * Behaviour notes and deviations: {@link StoreDefineAssetsExtraService}.
 */
@RestController
public class StoreDefineAssetsExtraController {

    private static final String CAT = "/api/store/define/asset-category";
    private static final String FAI = "/api/store/define/fixed-asset-item";

    private final StoreDefineAssetsExtraService service;
    public StoreDefineAssetsExtraController(StoreDefineAssetsExtraService service) { this.service = service; }

    // ------------------------------------------------------------------ frmItemCatagoryStore (FormTypeId 3)

    @GetMapping(CAT + "/lookups")
    public ResponseEntity<?> categoryLookups(@RequestParam(required = false) String host) {
        return run(() -> service.categoryLookups(host), "Could not load the screen.");
    }

    @GetMapping(CAT + "/history")
    public ResponseEntity<?> categoryHistory(@RequestParam(required = false) String host) {
        return run(() -> service.categoryHistory(host), "History failed.");
    }

    @GetMapping(CAT + "/attributes")
    public ResponseEntity<?> categoryAttributes(@RequestParam(defaultValue = "0") int categoryId,
                                                @RequestParam(required = false) String host) {
        return run(() -> service.categoryAttributes(categoryId, host), "Could not load the attributes.");
    }

    @GetMapping(CAT + "/{id}")
    public ResponseEntity<?> category(@PathVariable int id, @RequestParam(required = false) String host) {
        return run(() -> service.category(id, host), "Could not open the category.");
    }

    @PostMapping(CAT + "/save")
    public ResponseEntity<?> categorySave(@RequestBody StoreDefineAssetsExtraDto.AssetCategory body) {
        return run(() -> service.saveCategory(body), "Save failed.");
    }

    // ------------------------------------------------------------------ frmDefineAssets

    @GetMapping(FAI + "/lookups")
    public ResponseEntity<?> itemLookups(@RequestParam(required = false) String host) {
        return run(() -> service.assetItemLookups(host), "Could not load the screen.");
    }

    @GetMapping(FAI + "/categories")
    public ResponseEntity<?> itemCategories(@RequestParam(required = false) String host) {
        return run(() -> service.assetItemCategories(host), "Could not load the categories.");
    }

    @GetMapping(FAI + "/category-leave")
    public ResponseEntity<?> itemCategoryLeave(@RequestParam(defaultValue = "0") int categoryId,
                                               @RequestParam(required = false) String host) {
        return run(() -> service.assetItemCategoryLeave(categoryId, host), "Record Not Found");
    }

    @GetMapping(FAI + "/history")
    public ResponseEntity<?> itemHistory(@RequestParam(defaultValue = "0") int noOfRecords,
                                         @RequestParam(required = false) String host) {
        return run(() -> service.assetItemHistory(noOfRecords, host), "History failed.");
    }

    @GetMapping(FAI + "/{id}")
    public ResponseEntity<?> item(@PathVariable int id, @RequestParam(required = false) String host) {
        return run(() -> service.assetItem(id, host), "Could not open the asset item.");
    }

    @PostMapping(FAI + "/save")
    public ResponseEntity<?> itemSave(@RequestBody StoreDefineAssetsExtraDto.FixedAssetItem body) {
        return run(() -> service.saveAssetItem(body), "Save failed.");
    }

    // ------------------------------------------------------------------ plumbing

    @FunctionalInterface
    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            String m = (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
            Map<String, Object> body = new LinkedHashMap<>();
            body.put("success", false);
            body.put("message", m);
            return ResponseEntity.badRequest().body(body);
        }
    }
}
