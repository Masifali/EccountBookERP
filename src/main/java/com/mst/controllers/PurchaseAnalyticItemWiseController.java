package com.mst.controllers;

import com.mst.services.PurchaseAnalyticItemWiseService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.List;
import java.util.Map;

/**
 * Purchase Analytic Periodic ItemWise -
 * Architecture.WinApp.AnalyticDashboard\PurchaseAnalyticPeriodicItemWise.cs.
 *
 * The path's last segment normalises to "purchaseanalyticperiodicitemwise", the TargetUrl class
 * name, so the DashBoard card links here on its own. It is also where the Purchase Analytics
 * (Periodic) card caption now goes.
 */
@Controller
public class PurchaseAnalyticItemWiseController {

    @Autowired
    private PurchaseAnalyticItemWiseService service;

    @GetMapping("/dashboard/purchase-analytic-periodic-item-wise")
    public String page(@RequestParam(required = false) Integer parentCategoryId, Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Purchase Analytic Periodic ItemWise");
        /* RequestedByOtherDocument (form :41, :105-118): when the periodic screen opens this one
           it passes the parent category, fixes the combo to it and runs the report at once. */
        model.addAttribute("parentCategoryId", parentCategoryId);
        return "dashboard/purchase_analytic_item_wise";
    }

    /** GetSeasonScheduleDates(), form :154-187. */
    @GetMapping("/api/analytics/purchase-item-wise/season")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> season() {
        return ResponseEntity.ok(service.seasonSchedule());
    }

    /** ParentCategoryComboFill(), form :126-152. */
    @GetMapping("/api/analytics/purchase-item-wise/parent-categories")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> parentCategories() {
        return ResponseEntity.ok(service.parentCategories());
    }

    /** btnshow_Click, form :238-341 - @Activity = 'Product_Wise'. */
    @GetMapping("/api/analytics/purchase-item-wise/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam String fromDate,
            @RequestParam String toDate,
            @RequestParam String seasonStart,
            @RequestParam String seasonEnd,
            @RequestParam(required = false) String ids) {
        return ResponseEntity.ok(service.itemWise(fromDate, toDate, seasonStart, seasonEnd, ids));
    }
}
