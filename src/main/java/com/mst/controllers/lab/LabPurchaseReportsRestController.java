package com.mst.controllers.lab;

import com.mst.services.lab.LabPurchaseReportsService;
import java.util.Map;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

/**
 * Lab Report (module 1011) purchase-analysis reports API. Desktop messages come back as HTTP 400
 * (IllegalArgumentException -> ApiExceptionAdvice), a missing View grant as 403 (AccessDeniedException).
 *
 *   /purchase-by-vehicle/*   626 LabDataVehicleWiseByParent   "Purchase Analysis By Vehicle"
 *   /purchase-register/*     633 InvLabPurchaseRegister       "Purchase Analylsis Report"
 *   /purchase-periodic/*     630 LabPurchaseAnalyticPeriodic  "Lab Purchase Analysis Periodic Report"
 */
@RestController
@RequestMapping("/api/lab/reports")
public class LabPurchaseReportsRestController {

    private final LabPurchaseReportsService service;

    public LabPurchaseReportsRestController(LabPurchaseReportsService service) { this.service = service; }

    // ---------------------------------------------------------------- 626 Purchase Analysis By Vehicle
    @GetMapping("/purchase-by-vehicle/init")
    public Map<String, Object> vehicleInit() { return service.vehicleInit(); }

    @GetMapping("/purchase-by-vehicle/lookups")
    public Map<String, Object> vehicleLookups(@RequestParam(defaultValue = "0") int parentCategoryId) { return service.vehicleLookups(parentCategoryId); }

    @PostMapping("/purchase-by-vehicle/rows")
    public Map<String, Object> vehicleRows(@RequestBody Map<String, Object> body) { return service.vehicleRows(body); }

    // ---------------------------------------------------------------- 633 Purchase Analylsis Report
    @GetMapping("/purchase-register/init")
    public Map<String, Object> registerInit() { return service.registerInit(); }

    @GetMapping("/purchase-register/lookups")
    public Map<String, Object> registerLookups() { return service.registerLookups(); }

    @PostMapping("/purchase-register/rows")
    public Map<String, Object> registerRows(@RequestBody Map<String, Object> body) { return service.registerRows(body); }

    // ---------------------------------------------------------------- 630 Lab Purchase Analysis Periodic Report
    @GetMapping("/purchase-periodic/init")
    public Map<String, Object> periodicInit() { return service.periodicInit(); }

    @GetMapping("/purchase-periodic/season")
    public Map<String, Object> periodicSeason() { return service.periodicSeason(); }

    @PostMapping("/purchase-periodic/cards")
    public Map<String, Object> periodicCards(@RequestBody Map<String, Object> body) { return service.periodicCards(body); }

    @PostMapping("/purchase-periodic/item-wise")
    public Map<String, Object> periodicItemWise(@RequestBody Map<String, Object> body) { return service.periodicItemWise(body); }

    @PostMapping("/purchase-periodic/party-wise")
    public Map<String, Object> periodicPartyWise(@RequestBody Map<String, Object> body) { return service.periodicPartyWise(body); }
}
