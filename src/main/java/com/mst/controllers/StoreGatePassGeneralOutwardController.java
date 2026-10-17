package com.mst.controllers;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.StoreGatePassGeneralOutwardService;
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

/** Screen 343 GatePassGeneralOutward (Store Sale) - page /store/gate-pass-general-outward, API /store/gate-pass-general-outward/api. */
@Controller
public class StoreGatePassGeneralOutwardController extends SaleEngrControllerBase {
    private static final String API = "/store/gate-pass-general-outward/api";
    private final StoreGatePassGeneralOutwardService service;

    public StoreGatePassGeneralOutwardController(StoreGatePassGeneralOutwardService service) { this.service = service; }

    @GetMapping("/store/gate-pass-general-outward")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        model.addAttribute("moduleTitle", "Gate Pass General Outward");
        return "store/gate_pass_general_outward";
    }

    @GetMapping(API + "/initial") @ResponseBody
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping(API + "/next-no") @ResponseBody
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping(API + "/warehouses") @ResponseBody
    public List<Map<String, Object>> warehouses() { return service.warehouses(); }

    @GetMapping(API + "/vehicle-types") @ResponseBody
    public List<Map<String, Object>> vehicleTypes() { return service.vehicleTypes(); }

    @GetMapping(API + "/lookups") @ResponseBody
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping(API + "/delivery-order") @ResponseBody
    public Map<String, Object> deliveryOrder(@RequestParam(defaultValue = "0") int docNo) { return service.deliveryOrderLookup(docNo); }

    @PostMapping(API + "/history") @ResponseBody
    public List<Map<String, Object>> history(@RequestBody StoreGatePassGeneralOutwardService.HistoryFilter f) { return service.history(f); }

    @GetMapping(API + "/{id}") @ResponseBody
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping(API) @ResponseBody
    public Map<String, Object> save(@RequestBody StoreGatePassGeneralOutwardService.Request r) { return service.save(r); }

    @GetMapping(API + "/{id}/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachmentList(id); }

    @GetMapping(API + "/{id}/attachments/{attachmentId}")
    public ResponseEntity<byte[]> download(@PathVariable int id, @PathVariable int attachmentId) {
        SaleEngrAttachments.Download d = service.download(id, attachmentId);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename*=UTF-8''" + java.net.URLEncoder.encode(d.name(), StandardCharsets.UTF_8).replace("+", "%20"))
                .contentType(MediaType.APPLICATION_OCTET_STREAM).body(d.bytes());
    }
}
