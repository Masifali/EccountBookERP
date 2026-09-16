package com.mst.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

@Controller
public class MainModulesController {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @GetMapping({"/modules", "/main-hub", "/modules-hub"})
    public String mainModulesHub(Model model) {
        model.addAttribute("activeMenu", "modules");
        return "modules";
    }

    @GetMapping("/quality")
    public String qualityControl(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Quality Control Master");
        model.addAttribute("production", new HashMap<>());
        model.addAttribute("companies", new ArrayList<>());
        model.addAttribute("branches", new ArrayList<>());
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
        model.addAttribute("production", new HashMap<>());
        model.addAttribute("companies", new ArrayList<>());
        model.addAttribute("branches", new ArrayList<>());
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

    private void populateStockOpeningModel(Model model) {
        model.addAttribute("activeMenu", "inventory");

        if (!model.containsAttribute("itemCategory")) {
            model.addAttribute("itemCategory", new HashMap<>());
        }

        try {
            model.addAttribute("itemCategories", jdbcTemplate.queryForList("SELECT ID as id, CategoryCode as formattedCode, CategoryDescription as name FROM ItemCategory"));
        } catch (Exception e) {
            model.addAttribute("itemCategories", new ArrayList<>());
        }

        try {
            model.addAttribute("itemSubCategories", jdbcTemplate.queryForList("SELECT ID as id, TypeCode as formattedCode, TypeDescription as name FROM ItemType"));
        } catch (Exception e) {
            model.addAttribute("itemSubCategories", new ArrayList<>());
        }

        try {
            model.addAttribute("itemDefs", jdbcTemplate.queryForList("SELECT ID as id, ItemCode as formattedCode, ItemName as name, CostPrice as standardRate, RetailPrice as saleRate, 1.0 as unitValue FROM Item"));
        } catch (Exception e) {
            model.addAttribute("itemDefs", new ArrayList<>());
        }
    }

    // Stocks & Inventory Sub-modules

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

    private void populateStockProfitabilityModel(Model model) {
        model.addAttribute("activeMenu", "inventory");

        if (!model.containsAttribute("stockLedgerReportRequest")) {
            Map<String, Object> req = new HashMap<>();
            req.put("companyId", 0);
            req.put("branchId", 0);
            req.put("itemCategoryId", 0);
            req.put("itemDefId", 0);
            req.put("fromDate", LocalDate.now().toString());
            req.put("toDate", LocalDate.now().toString());
            req.put("summary", "0");
            model.addAttribute("stockLedgerReportRequest", req);
        }

        try {
            model.addAttribute("companies", jdbcTemplate.queryForList("SELECT ID as id, CompName as name FROM Company"));
        } catch (Exception e) {
            model.addAttribute("companies", new ArrayList<>());
        }

        try {
            model.addAttribute("companyBranches", jdbcTemplate.queryForList("SELECT ID as id, BranchName as name FROM Branches"));
        } catch (Exception e) {
            model.addAttribute("companyBranches", new ArrayList<>());
        }

        try {
            model.addAttribute("itemCategories", jdbcTemplate.queryForList("SELECT ID as id, CategoryDescription as name FROM ItemCategory"));
        } catch (Exception e) {
            model.addAttribute("itemCategories", new ArrayList<>());
        }

        try {
            model.addAttribute("itemDefs", jdbcTemplate.queryForList("SELECT ID as id, ItemCode as formattedCode, ItemName as name FROM Item"));
        } catch (Exception e) {
            model.addAttribute("itemDefs", new ArrayList<>());
        }
    }

    @GetMapping({"/stocks/inventory_profitability_report", "/stocks/inventory-profitability-report"})
    public String inventoryProfitabilityReport(Model model) {
        populateStockProfitabilityModel(model);
        return "stocks/inventory_profitability_report";
    }

    // 13. Stock Valuation Report
    @GetMapping({"/stocks/stock_valuation", "/stocks/stock-valuation"})
    public String stockValuation(Model model) {
        populateStockOpeningModel(model);
        model.addAttribute("moduleTitle", "Stock Valuation Report");
        return "stocks/stock_opening_form";
    }

    // 15. Stock Register Report
    @GetMapping({"/stocks/stock_register", "/stocks/stock-register"})
    public String stockRegister(Model model) {
        populateStockOpeningModel(model);
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
        populateStockOpeningModel(model);
        model.addAttribute("moduleTitle", "Item Stock As On Date");
        return "stocks/stock_opening_form";
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
