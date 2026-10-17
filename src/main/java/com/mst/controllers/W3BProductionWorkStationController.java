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
 * Mfg Production hub (module 103) - screen 771 "Production Work Station"
 * (Architecture.WinApp.Mfg.ProductionWorkStation). Page /production/mfg/work-station, JSON under /production/mfg/work-station/api.
 * Behaviour notes: {@link W3BMfgMastersService}.
 */
@Controller
@RequestMapping("/production/mfg/work-station")
public class W3BProductionWorkStationController {

    private final W3BMfgMastersService service;
    public W3BProductionWorkStationController(W3BMfgMastersService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Work Station");
        return "production/mfg/w3b_production_work_station";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::workStationLookups, "Could not load the screen."); }

    @GetMapping("/api/departments") @ResponseBody
    public ResponseEntity<?> departments() { return W3BApi.run(service::departments, "Refresh failed."); }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history() { return W3BApi.run(service::workStationHistory, "Could not load the history."); }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) { return W3BApi.run(() -> service.workStationLoad(id), "Could not open that record."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.workStationSave(body), "Save failed."); }
}
