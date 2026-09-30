package com.mst.controllers;

import com.mst.services.ProductionMasterDataService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * The three Production master-data definitions (Architecture.WinApp.Production):
 *
 *   /production/define-production-type       DefineProductionType.cs      "Define Production Type"
 *   /production/define-production-plan-type  DefineProductionPlanType.cs  "Define Plan Type"
 *   /production/define-production-plant      DefineProductionPlant.cs     "Define Plant"
 *
 * Each route normalises (letters and digits, lower-case) to the desktop class name, which is what
 * ScreenRouteIndex matches a dbo.ScreenDefinition row against - so a screen row of that name opens
 * the page from the menu without a hand-kept map.
 *
 * No parameter carries tenancy or a user id; the service derives them from the session.
 */
@Controller
public class ProductionMasterDataController {

    private static final String API = "/api/production/master-data";

    @Autowired private ProductionMasterDataService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/production/define-production-type")
    public String productionTypePage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/define_production_type";
    }

    @GetMapping("/production/define-production-plan-type")
    public String planTypePage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/define_production_plan_type";
    }

    @GetMapping("/production/define-production-plant")
    public String plantPage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/define_production_plant";
    }

    // ------------------------------------------------------------------ Production Type

    @GetMapping(API + "/production-type/list")
    @ResponseBody
    public ResponseEntity<?> productionTypes() {
        try { return ResponseEntity.ok(service.productionTypes()); } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/production-type/by-id")
    @ResponseBody
    public ResponseEntity<?> productionType(@RequestParam("id") int id) {
        try { return ResponseEntity.ok(service.productionType(id)); }
        catch (IllegalArgumentException e) { return ResponseEntity.badRequest().body(fail(root(e, "Record not found."))); }
        catch (Exception e) { return error(e, "Load failed."); }
    }

    @PostMapping(API + "/production-type/save")
    @ResponseBody
    public ResponseEntity<?> saveProductionType(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.saveProductionType(body)); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, "Save failed."))); }
        catch (Exception e) { return error(e, "Save failed."); }
    }

    // ------------------------------------------------------------------ Plan Type

    @GetMapping(API + "/plan-type/list")
    @ResponseBody
    public ResponseEntity<?> planTypes() {
        try { return ResponseEntity.ok(service.planTypes()); } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/plan-type/by-id")
    @ResponseBody
    public ResponseEntity<?> planType(@RequestParam("id") int id) {
        try { return ResponseEntity.ok(service.planType(id)); }
        catch (IllegalArgumentException e) { return ResponseEntity.badRequest().body(fail(root(e, "Record not found."))); }
        catch (Exception e) { return error(e, "Load failed."); }
    }

    @PostMapping(API + "/plan-type/save")
    @ResponseBody
    public ResponseEntity<?> savePlanType(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.savePlanType(body)); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, "Save failed."))); }
        catch (Exception e) { return error(e, "Save failed."); }
    }

    // ------------------------------------------------------------------ Plant

    /** DefineCountry_Load - the plant list and the branch combo in one call. */
    @GetMapping(API + "/plant/setup")
    @ResponseBody
    public ResponseEntity<?> plantSetup() {
        try { return ResponseEntity.ok(service.plantSetup()); } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/plant/list")
    @ResponseBody
    public ResponseEntity<?> plants() {
        try { return ResponseEntity.ok(service.plants()); } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/plant/by-id")
    @ResponseBody
    public ResponseEntity<?> plant(@RequestParam("id") int id) {
        try { return ResponseEntity.ok(service.plant(id)); }
        catch (IllegalArgumentException e) { return ResponseEntity.badRequest().body(fail(root(e, "Record not found."))); }
        catch (Exception e) { return error(e, "Load failed."); }
    }

    @PostMapping(API + "/plant/save")
    @ResponseBody
    public ResponseEntity<?> savePlant(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.savePlant(body)); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, "Save failed."))); }
        catch (Exception e) { return error(e, "Save failed."); }
    }

    /** btnWarehouseAllocation_Click - the rights check for frmWarehousesAllocationToPlant. */
    @GetMapping(API + "/plant/warehouse-allocation-right")
    @ResponseBody
    public ResponseEntity<?> warehouseAllocationRight() {
        try { return ResponseEntity.ok(service.warehouseAllocationRight()); } catch (Exception e) { return error(e, "Rights check failed."); }
    }

    // ------------------------------------------------------------------ plumbing

    /** The innermost message - for an SQL RAISERROR that is the procedure's own text, as MessageBox.Show(ex.Message). */
    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static ResponseEntity<Map<String, Object>> error(Exception e, String fallback) {
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
