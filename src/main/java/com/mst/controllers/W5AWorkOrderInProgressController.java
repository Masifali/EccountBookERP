package com.mst.controllers;

import com.mst.services.W5AWorkOrderInProgressService;
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
 * Mfg Production hub (module 103) - screen 594 "Work Order In Progress" (Architecture.WinApp.Mfg.frmWorkOrderInProgress, DocumentTypeId 1620).
 * Page /production/mfg/work-order-in-progress, JSON under /production/mfg/work-order-in-progress/api. Notes: {@link W5AWorkOrderInProgressService}.
 */
@Controller
@RequestMapping("/production/mfg/work-order-in-progress")
public class W5AWorkOrderInProgressController {

    private final W5AWorkOrderInProgressService service;
    public W5AWorkOrderInProgressController(W5AWorkOrderInProgressService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Work Order In Progress");
        return "production/mfg/w5a_work_order_in_progress";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/main") @ResponseBody
    public ResponseEntity<?> main() { return W3BApi.run(service::mainGrid, "Could not load the Work Order In Progress list."); }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.load(id), "Could not open that record."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }
}
