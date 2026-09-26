package com.mst.controllers;

import com.mst.services.ProductionDailyPlantHoursService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.sql.SQLException;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Screen 877 "Daily Plant Consumed Hours" - Architecture.WinApp.Production/frmDailyPlantConsumedHours.cs.
 *
 * Desktop entry: frmProductionOutput.btnDailyPlantConsumedHours_Click:3221
 * `new frmDailyPlantConsumedHours(UserAccount).Show()` - a separate, non-modal window, so the web
 * Output tab opens this page in a new window. It also works on its own.
 *
 * No parameter carries tenancy, branch, financial year or user: all are server-derived.
 */
@Controller
public class ProductionDailyPlantHoursController {

    private static final String API = "/api/production/daily-plant-consumed-hours";

    @Autowired private ProductionDailyPlantHoursService service;

    @GetMapping("/production/daily-plant-consumed-hours")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/daily_plant_consumed_hours";
    }

    /** InitializeComponentMethod:159 - job orders + downtime reasons. */
    @GetMapping(API + "/load")
    @ResponseBody
    public ResponseEntity<?> load() {
        try { return ResponseEntity.ok(service.load()); } catch (Exception e) { return error(e); }
    }

    /** JobOrderNoDbCall (Refresh). */
    @GetMapping(API + "/job-orders")
    @ResponseBody
    public ResponseEntity<?> jobOrders() {
        try { return ResponseEntity.ok(service.jobOrders()); } catch (Exception e) { return error(e); }
    }

    /** DownTimeReasonDbCall (Refresh). */
    @GetMapping(API + "/reasons")
    @ResponseBody
    public ResponseEntity<?> reasons() {
        try { return ResponseEntity.ok(service.reasons()); } catch (Exception e) { return error(e); }
    }

    /** PlantBindByJobOrderId (CmbJobOrder_Leave). */
    @GetMapping(API + "/plants")
    @ResponseBody
    public ResponseEntity<?> plants(@RequestParam(defaultValue = "0") int jobOrderId) {
        try { return ResponseEntity.ok(service.plants(jobOrderId)); } catch (Exception e) { return error(e); }
    }

    /** Insert():304 - {result:true, updated} | 400 {message}. */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        try {
            boolean updated = service.save(body);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("result", true);
            m.put("updated", updated);
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /** BindGrid:435. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int jobOrderId,
                                     @RequestParam(defaultValue = "0") int reasonId) {
        try {
            return ResponseEntity.ok(service.history(dateType, fromDate, toDate, jobOrderId, reasonId));
        } catch (Exception e) { return error(e); }
    }

    /** The desktop shows ex.Message; a procedure's RAISERROR text is unwrapped from DataAccessException. */
    private static ResponseEntity<?> error(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        HttpStatus status = HttpStatus.BAD_REQUEST;
        if (e instanceof DataAccessException) {
            Throwable t = e;
            while (t.getCause() != null && !(t instanceof SQLException)) t = t.getCause();
            msg = t.getMessage();
        } else if (!(e instanceof IllegalArgumentException) && !(e instanceof IllegalStateException)) {
            status = HttpStatus.INTERNAL_SERVER_ERROR;
        }
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        return ResponseEntity.status(status).body(m);
    }
}
