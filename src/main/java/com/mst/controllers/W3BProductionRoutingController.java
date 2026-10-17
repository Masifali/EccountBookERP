package com.mst.controllers;

import com.mst.services.W3BProductionRoutingService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Mfg Production hub (module 103) - screen 773 "Production Routing" (Architecture.WinApp.Mfg.frmProductionRouting).
 * Page /production/mfg/routing, JSON under /production/mfg/routing/api. Notes: {@link W3BProductionRoutingService}.
 */
@Controller
@RequestMapping("/production/mfg/routing")
public class W3BProductionRoutingController {

    private final W3BProductionRoutingService service;
    public W3BProductionRoutingController(W3BProductionRoutingService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Routing");
        return "production/mfg/w3b_production_routing";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int routingNameId) {
        return W3BApi.run(() -> service.history(routingNameId), "History failed.");
    }

    @GetMapping("/api/read/{routingNameId:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int routingNameId) {
        return W3BApi.run(() -> service.read(routingNameId), "Could not open that routing.");
    }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int routingNameId) { return W3BApi.run(() -> service.delete(routingNameId), "Delete failed."); }
}
