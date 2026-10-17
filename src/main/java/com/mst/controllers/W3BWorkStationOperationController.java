package com.mst.controllers;

import com.mst.services.W3BMfgMastersService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Mfg Production hub (module 103) - screen 595 "Work Station Operation"
 * (Architecture.WinApp.Mfg.WorkStationOperation). Page /production/mfg/work-station-operation,
 * JSON under /production/mfg/work-station-operation/api. Behaviour notes: {@link W3BMfgMastersService}.
 */
@Controller
@RequestMapping("/production/mfg/work-station-operation")
public class W3BWorkStationOperationController {

    private final W3BMfgMastersService service;
    public W3BWorkStationOperationController(W3BMfgMastersService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Work Station Operation");
        return "production/mfg/w3b_work_station_operation";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::wsoLookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::wsoRefresh, "Refresh failed."); }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history() { return W3BApi.run(service::wsoHistory, "Could not load the history."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.wsoSave(body), "Save failed."); }
}
