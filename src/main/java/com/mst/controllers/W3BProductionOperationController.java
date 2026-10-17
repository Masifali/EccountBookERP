package com.mst.controllers;

import com.mst.services.W3BMfgMastersService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Mfg Production hub (module 103) - screen 770 "Production Operation"
 * (Architecture.WinApp.Mfg.ProductionOperation). Page /production/mfg/operation, JSON under /production/mfg/operation/api.
 * Behaviour notes: {@link W3BMfgMastersService}.
 */
@Controller
@RequestMapping("/production/mfg/operation")
public class W3BProductionOperationController {

    private final W3BMfgMastersService service;
    public W3BProductionOperationController(W3BMfgMastersService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Operation");
        return "production/mfg/w3b_production_operation";
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history() { return W3BApi.run(service::operationHistory, "Could not load the history."); }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) { return W3BApi.run(() -> service.operationLoad(id), "Could not open that record."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.operationSave(body), "Save failed."); }
}
