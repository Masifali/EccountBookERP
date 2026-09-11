package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

import java.util.ArrayList;

@Controller
public class MainModulesController {

    @GetMapping({"/modules", "/main-hub", "/modules-hub"})
    public String mainModulesHub(Model model) {
        model.addAttribute("activeMenu", "modules");
        return "modules";
    }

    @GetMapping("/quality")
    public String qualityControl(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Quality Control Master");
        return "production/production_form";
    }

    @GetMapping("/wages")
    public String contractorWages(Model model) {
        model.addAttribute("activeMenu", "wages");
        model.addAttribute("moduleTitle", "Contractor Wages Account");
        return "accounts/vouchers/contractor_wages";
    }

    @GetMapping("/production")
    public String production(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Entry & Yield");
        return "production/production_form";
    }

    @GetMapping("/store")
    public String storeManagement(Model model) {
        model.addAttribute("activeMenu", "store");
        return "redirect:/inventory/warehouses";
    }

    @GetMapping("/taxation")
    public String taxation(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "redirect:/accounts/reports/payables-report";
    }

    @GetMapping("/kanta")
    public String weighBridge(Model model) {
        model.addAttribute("activeMenu", "kanta");
        return "redirect:/purchase/inward-gate-pass";
    }

    @GetMapping({"/lookups/reasons", "/lookups/reasons/list"})
    public String masterDataReasons(Model model) {
        model.addAttribute("activeMenu", "lookups");
        model.addAttribute("reasons", new ArrayList<>());
        return "lookups/reasons";
    }

    @GetMapping("/commission")
    public String commissionTrading(Model model) {
        model.addAttribute("activeMenu", "commission");
        return "redirect:/purchase/purchase-order";
    }

    @GetMapping("/packing")
    public String packingMaterial(Model model) {
        model.addAttribute("activeMenu", "packing");
        return "redirect:/inventory/item-categories";
    }

    // Stocks & Inventory Sub-modules
    @GetMapping({"/stocks/stock_opening_form", "/stocks/stock-opening-form"})
    public String stockOpeningForm(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "stocks/stock_opening_form";
    }

    @GetMapping({"/stocks/item_recipe_form", "/stocks/item-recipe-form"})
    public String itemRecipeForm(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "stocks/item_recipe_form";
    }

    @GetMapping("/stocks/mill_rates")
    public String millRates(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "stocks/mill_rates";
    }

    @GetMapping("/stocks/mill_rates_view")
    public String millRatesView(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "stocks/mill_rates_view";
    }

    @GetMapping("/stocks/inventory_profitability_report")
    public String inventoryProfitabilityReport(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "stocks/inventory_profitability_report";
    }
}
