package com.mst.controllers;

import com.mst.services.ProductionEvaluationWagesService;
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
 * "Wages Register" - Architecture.WinApp.Inventory_Reports/frmEvaulationDetailWagesReports.cs.
 *
 * Desktop entry used by screen 280: frmProductionSettlement.grdOverHeadSettlement_LinkClicked:972,
 * DocumentTypeId 80 or 112:
 *     obj2 = new frmEvaulationDetailWagesReports(UserAccount); obj2.Show();
 *     obj2.CmbJobOrderNo.Value = Id;  (the settlement's job order)   obj2.GridFill();
 * The web link opens this page in a new window with ?jobOrderId=Id, and the page replays those
 * two statements after its Load. It also works on its own.
 *
 * No parameter carries tenancy or user: all are server-derived.
 */
@Controller
public class ProductionEvaluationWagesController {

    private static final String API = "/api/production/reports/evaluation-detail-wages";

    @Autowired private ProductionEvaluationWagesService service;

    @GetMapping("/production/reports/evaluation-detail-wages")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/reports/evaluation_detail_wages";
    }

    /** frmEvaulationDetailSalesReports_Load:151 (features, BranchesFill, Start_Period, formats). */
    @GetMapping(API + "/load")
    @ResponseBody
    public ResponseEntity<?> load() {
        try { return ResponseEntity.ok(service.load()); } catch (Exception e) { return error(e); }
    }

    /** BranchesFill (Refresh). */
    @GetMapping(API + "/branches")
    @ResponseBody
    public ResponseEntity<?> branches() {
        try { return ResponseEntity.ok(service.branches()); } catch (Exception e) { return error(e); }
    }

    /** ComboFill:280. */
    @PostMapping(API + "/combos")
    @ResponseBody
    public ResponseEntity<?> combos(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.combos(str(body.get("branchText")))); } catch (Exception e) { return error(e); }
    }

    /** JobOrderFill:363. */
    @PostMapping(API + "/job-orders")
    @ResponseBody
    public ResponseEntity<?> jobOrders(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.jobOrders(str(body.get("branchText")))); } catch (Exception e) { return error(e); }
    }

    /** GridFill:482. */
    @PostMapping(API + "/grid")
    @ResponseBody
    public ResponseEntity<?> grid(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.grid(body)); } catch (Exception e) { return error(e); }
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

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
