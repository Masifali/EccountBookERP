package com.mst.controllers;

import com.mst.services.MonthWiseGraphService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Monthly Purchase (Graph) Report and Monthly Sale (Graph) Report -
 * Architecture.WinApp.Graph\MonthWisePurchaseReport.cs and MonthWiseSaleReport.cs.
 *
 * Two separate routes, because they are two separate DashBoard entries backed by two separate
 * desktop forms and two different procedures. Each path's last segment normalises to its own
 * TargetUrl class name - "monthwisepurchasereport" and "monthwisesalereport" - so each card
 * links to its own page and neither opens the other's.
 *
 * They render through one template because the two forms are laid out identically; the page is
 * told which one it is, and the data, titles and procedures differ throughout.
 */
@Controller
public class MonthWiseGraphController {

    @Autowired
    private MonthWiseGraphService service;

    @GetMapping("/dashboard/month-wise-purchase-report")
    public String purchasePage(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Monthly Purchase (Graph) Report");
        model.addAttribute("graphTitle", "Monthly Purchase Graph");
        model.addAttribute("seriesName", "Purchase");
        model.addAttribute("mode", "purchase");
        return "dashboard/month_wise_graph";
    }

    @GetMapping("/dashboard/month-wise-sale-report")
    public String salePage(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Monthly Sale (Graph) Report");
        model.addAttribute("graphTitle", "Monthly Sale Graph");
        model.addAttribute("seriesName", "Sale");
        model.addAttribute("mode", "sale");
        return "dashboard/month_wise_graph";
    }

    /** CombosAgainstPurchaseInvoice() / its sale twin, :156-213. */
    @GetMapping("/api/dashboard/month-wise/combos")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> combos(
            @RequestParam(defaultValue = "purchase") String mode) {
        return ResponseEntity.ok(service.combos("sale".equalsIgnoreCase(mode)));
    }

    /** ShowReport(), :227-330. */
    @GetMapping("/api/dashboard/month-wise/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam(defaultValue = "purchase") String mode,
            @RequestParam(defaultValue = "0") int month,
            @RequestParam(defaultValue = "0") int categoryId,
            @RequestParam(defaultValue = "0") int classGroupId) {
        return ResponseEntity.ok(service.report(
                "sale".equalsIgnoreCase(mode), month, categoryId, classGroupId));
    }
}
