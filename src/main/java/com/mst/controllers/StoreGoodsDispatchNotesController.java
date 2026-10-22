package com.mst.controllers;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.StoreGoodsDispatchNotesService;
import com.mst.services.sale.engr.SaleEngrAttachments;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 348 GoodsDispatchNotes_Store (Store Management) - page /store/goods-dispatch-notes-store, API /store/goods-dispatch-notes-store/api. */
@Controller
public class StoreGoodsDispatchNotesController extends SaleEngrControllerBase {
    private static final String API = "/store/goods-dispatch-notes-store/api";
    private final StoreGoodsDispatchNotesService service;

    public StoreGoodsDispatchNotesController(StoreGoodsDispatchNotesService service) { this.service = service; }

    @GetMapping("/store/goods-dispatch-notes-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        model.addAttribute("moduleTitle", "Goods Dispatch Notes Store");
        return "store/goods_dispatch_notes_store";
    }

    @GetMapping("/store/goods-dispatch-notes-store/api/initial") @ResponseBody
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/store/goods-dispatch-notes-store/api/refresh") @ResponseBody
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/store/goods-dispatch-notes-store/api/reset-data") @ResponseBody
    public Map<String, Object> resetData() { return service.resetData(); }

    @GetMapping("/store/goods-dispatch-notes-store/api/items") @ResponseBody
    public List<Map<String, Object>> items(@RequestParam(defaultValue = "0") int customerId) { return service.items(customerId); }

    @GetMapping("/store/goods-dispatch-notes-store/api/history-combos") @ResponseBody
    public List<Map<String, Object>> historyCombos() { return service.historyCombos(); }

    @PostMapping("/store/goods-dispatch-notes-store/api/history") @ResponseBody
    public List<Map<String, Object>> history(@RequestBody StoreGoodsDispatchNotesService.HistoryFilter f) { return service.history(f); }

    @GetMapping("/store/goods-dispatch-notes-store/api/{id}") @ResponseBody
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/store/goods-dispatch-notes-store/api/{id}/details") @ResponseBody
    public List<Map<String, Object>> details(@PathVariable int id) { return service.detailOf(id); }

    @PostMapping(API) @ResponseBody
    public Map<String, Object> save(@RequestBody StoreGoodsDispatchNotesService.Request r) { return service.save(r); }

    @GetMapping("/store/goods-dispatch-notes-store/api/{id}/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachmentList(id); }

    @GetMapping("/store/goods-dispatch-notes-store/api/{id}/attachments/{attachmentId}")
    public ResponseEntity<byte[]> download(@PathVariable int id, @PathVariable int attachmentId) {
        SaleEngrAttachments.Download d = service.download(id, attachmentId);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename*=UTF-8''" + java.net.URLEncoder.encode(d.name(), StandardCharsets.UTF_8).replace("+", "%20"))
                .contentType(MediaType.APPLICATION_OCTET_STREAM).body(d.bytes());
    }
}
