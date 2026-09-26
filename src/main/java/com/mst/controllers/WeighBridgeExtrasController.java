package com.mst.controllers;

import com.mst.services.WeighBridgeLookupsService;
import com.mst.services.WeighBridgeReportsService;
import com.mst.services.WeighBridgeWeightUpdateService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * The weigh bridge satellite screens. Route last segments normalise to the desktop ScreenNames
 * (with "frm" stripped) so ScreenRouteIndex links them on its own:
 *
 *   /weighbridge/reports/weight-bridge-history              → frmWeightBridgeHistory (360)
 *   /weighbridge/reports/weigh-bridge-rejected-ticket-nos   → frmWeighBridgeRejectedTicketNos (359)
 *   /weighbridge/weigh-bridge-general-lookups               → WeighBridgeGeneralLookups (728)
 *   /weighbridge/vehicle-weight-lookup                      → VehicleWeightLookUp (432)
 *   /weighbridge/weighbridge-weights-update                 → WeighBridge_WeightUpdate (Admin Panel, Admin only)
 *
 * No organisation, company, branch, year or user is accepted from the caller.
 */
@Controller
public class WeighBridgeExtrasController {

    private static final String API = "/api/weighbridge-extras";

    @Autowired private WeighBridgeLookupsService lookups;
    @Autowired private WeighBridgeReportsService reports;
    @Autowired private WeighBridgeWeightUpdateService weightUpdate;

    // ------------------------------------------------------------------------------ pages

    @GetMapping("/weighbridge/weigh-bridge-general-lookups")
    public String generalLookupsPage(Model model) { model.addAttribute("activeMenu", "kanta"); return "weighbridge/weighbridge_general_lookups"; }

    @GetMapping("/weighbridge/vehicle-weight-lookup")
    public String vehicleWeightPage(Model model) { model.addAttribute("activeMenu", "kanta"); return "weighbridge/vehicle_weight_lookup"; }

    @GetMapping("/weighbridge/reports/weigh-bridge-rejected-ticket-nos")
    public String rejectedPage(Model model) { model.addAttribute("activeMenu", "kanta"); return "weighbridge/weighbridge_rejected_ticket_nos"; }

    @GetMapping("/weighbridge/reports/weight-bridge-history")
    public String historyPage(Model model) { model.addAttribute("activeMenu", "kanta"); return "weighbridge/weightbridge_history"; }

    // ------------------------------------------------------------ WeighBridgeGeneralLookups

    @GetMapping(API + "/general-lookups")
    @ResponseBody
    public ResponseEntity<?> generalLookups() { return run(lookups::generalLookups, "Could not load the lookups."); }

    @PostMapping(API + "/general-lookups/save")
    @ResponseBody
    public ResponseEntity<?> saveGeneral(@RequestBody WeighBridgeLookupsService.GeneralRequest r) {
        return run(() -> lookups.saveGeneral(r), "Save failed.");
    }

    // ------------------------------------------------------------------ VehicleWeightLookUp

    @GetMapping(API + "/vehicle-weights")
    @ResponseBody
    public ResponseEntity<?> vehicleWeights() { return run(lookups::vehicleWeights, "Could not load the vehicle weights."); }

    @GetMapping(API + "/vehicle-types")
    @ResponseBody
    public ResponseEntity<?> vehicleTypes() { return run(lookups::vehicleTypes, "Could not load the vehicle types."); }

    @PostMapping(API + "/vehicle-weights/save")
    @ResponseBody
    public ResponseEntity<?> saveVehicle(@RequestBody WeighBridgeLookupsService.VehicleRequest r) {
        return run(() -> lookups.saveVehicle(r), "Save failed.");
    }

    // ------------------------------------------------------------------------------ 359

    @GetMapping(API + "/rejected-ticket-nos")
    @ResponseBody
    public ResponseEntity<?> rejectable() { return run(reports::rejectableTickets, "Could not load the tickets."); }

    @PostMapping(API + "/rejected-ticket-nos/{id}/reject")
    @ResponseBody
    public ResponseEntity<?> reject(@PathVariable int id) { return run(() -> reports.reject(id), "Reject failed."); }

    // ------------------------------------------------------------------------------ 360

    @GetMapping(API + "/history/lookups")
    @ResponseBody
    public ResponseEntity<?> historyLookups() { return run(reports::historyLookups, "Could not load the screen."); }

    @GetMapping(API + "/history/refresh")
    @ResponseBody
    public ResponseEntity<?> historyRefresh() { return run(reports::historyRefresh, "Refresh failed."); }

    @PostMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestBody WeighBridgeReportsService.HistoryRequest r) {
        return run(() -> reports.history(r), "Could not load the history.");
    }

    @GetMapping(API + "/history/slip/{id}")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id, @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> reports.slip(id, documentTypeId), "Could not read the slip.");
    }

    // ------------------------------------------- WeighBridge_WeightUpdate (Gear → Admin Panel)

    @GetMapping("/weighbridge/weighbridge-weights-update")
    public String weightUpdatePage(Model model) { model.addAttribute("activeMenu", "apps"); return "weighbridge/weighbridge_weights_update"; }

    @GetMapping(API + "/weight-update/lookups")
    @ResponseBody
    public ResponseEntity<?> weightUpdateLookups() { return run(weightUpdate::lookups, "Could not load the screen."); }

    @GetMapping(API + "/weight-update/branches")
    @ResponseBody
    public ResponseEntity<?> weightUpdateBranches() { return run(weightUpdate::branches, "Refresh failed."); }

    @PostMapping(API + "/weight-update/search")
    @ResponseBody
    public ResponseEntity<?> weightUpdateSearch(@RequestBody WeighBridgeWeightUpdateService.SearchRequest r) {
        return run(() -> weightUpdate.search(r), "Could not load the tickets.");
    }

    @PostMapping(API + "/weight-update/update")
    @ResponseBody
    public ResponseEntity<?> weightUpdateSave(@RequestBody WeighBridgeWeightUpdateService.UpdateRequest r) {
        return run(() -> weightUpdate.update(r), "Update failed.");
    }

    // ------------------------------------------------------------------------------ plumbing

    private static ResponseEntity<?> run(Callable<?> body, String fallback) {
        try {
            return ResponseEntity.ok(body.call());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }

    private static String msg(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return (m == null || m.trim().isEmpty()) ? fallback : m;
    }
}
