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

    @GetMapping({"/wages", "/wages/dashboard", "/contractor-wages"})
    public String contractorWages(Model model) {
        model.addAttribute("activeMenu", "wages");
        return "accounts/vouchers/contractor_wages_dashboard";
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

    @GetMapping({"/kanta", "/weighbridge", "/weigh-bridge", "/weighbridge/dashboard"})
    public String weighBridge(Model model) {
        model.addAttribute("activeMenu", "kanta");
        return "weighbridge/weigh_bridge_dashboard";
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

    // 13. Stock Valuation Report
    @GetMapping({"/stocks/stock_valuation", "/stocks/stock-valuation"})
    public String stockValuation(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Valuation Report");
        return "stocks/stock_opening_form";
    }

    // 14. Stock Movement Report
    @GetMapping({"/stocks/stock_movement", "/stocks/stock-movement"})
    public String stockMovement(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Movement Report");
        return "stocks/inventory_profitability_report";
    }

    // 15. Stock Register Report
    @GetMapping({"/stocks/stock_register", "/stocks/stock-register"})
    public String stockRegister(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Register Report");
        return "stocks/stock_opening_form";
    }

    // 16. Warehouse Stock Summary
    @GetMapping({"/stocks/warehouse_summary", "/stocks/warehouse-summary"})
    public String warehouseSummary(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Warehouse Stock Summary");
        return "inventory/warehouses";
    }

    // 17. Item Stock As On Date
    @GetMapping({"/stocks/stock_as_on_date", "/stocks/stock-as-on-date"})
    public String stockAsOnDate(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Item Stock As On Date");
        return "stocks/stock_opening_form";
    }

    // 18. Item Ledger Report
    @GetMapping({"/stocks/item_ledger", "/stocks/item-ledger"})
    public String itemLedger(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Item Ledger Report");
        return "inventory/items";
    }

    // 19. Lot Wise Stock Report
    @GetMapping({"/stocks/lot_wise_stock", "/stocks/lot-wise-stock"})
    public String lotWiseStock(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Lot Wise Stock Report");
        return "inventory/lots";
    }

    // 20. Brand Wise Stock Report
    @GetMapping({"/stocks/brand_wise_stock", "/stocks/brand-wise-stock"})
    public String brandWiseStock(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Brand Wise Stock Report");
        return "inventory/brands";
    }
}
