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

    /*
     * "/quality" is owned by QualityControlViewController, which is annotated
     * @RequestMapping("/quality") with @GetMapping({"", "/", "/dashboard"}) and serves the real
     * Quality Control (Lab) dashboard plus its sixteen screens.
     *
     * The handler that used to sit here also claimed GET /quality and returned
     * "production/production_form" - the Production Entry form, an unrelated page. Two handlers
     * on the same (method, path) is an Ambiguous mapping, so Spring refused to start the
     * application at all; and had it started, /quality would have shown the Production screen
     * while every link on the Quality Control dashboard 404'd.
     *
     * Do not re-add a /quality mapping here. Run `python3 mapcheck.py src/main/java` before a
     * build to catch a collision like this one.
     */

    @GetMapping({"/wages", "/wages/dashboard", "/contractor-wages"})
    public String contractorWages(Model model) {
        model.addAttribute("activeMenu", "wages");
        return "accounts/vouchers/contractor_wages_dashboard";
    }

    /*
     * This used to return "production/production_form" with a HashMap under the model name
     * "production". That template opens with th:object="${production}" and then binds fields with
     * th:field="${production.company}" / "${production.id}" / "${production.productionCode}".
     * Spring resolves a th:field through a BeanWrapper, and a java.util.HashMap has no readable
     * "company" property, so every request to /production ended in
     * NotReadablePropertyException -> HTTP 500. The page could never have rendered.
     *
     * The template itself is misfiled scaffolding, not a Production screen: its heading says
     * "SALE ORDER FORM", it posts to /receivables/add_or_update_sale_order, its element ids are
     * purchaseOrderForm / purchaseOrderId, and it also reads ${accounts}, ${financialYears},
     * ${itemSubCategories} and ${millKhate}, none of which any handler supplies. It is the same
     * template that once made /quality show a Production page (see the note above).
     *
     * /production now serves the Production module's own landing page - the screens this port has
     * actually built, each gated on the authority its sidebar entry uses, so the page can never
     * offer a screen the signed-in user's rights do not allow. No model attribute is invented for
     * it; the rights already reach the template as Spring authorities.
     */
    /* /production moved to AppMenuController, which renders the desktop's own two levels for
       the Production APPLICATION — two module cards ("Production 3", "Production Reports 5")
       and the chosen module's screens underneath. The hand-written landing page this used to
       return invented its own sections and counts; production_module.html is kept on disk but
       nothing routes to it. */

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

    /*
     * "/commission" is now owned by CmagtModuleViewController, which serves the real
     * Commission Trading screen board (Commission Agent Portal + Commission Trading Reports).
     *
     * It previously redirected to "/purchase/purchase-order" - an unrelated module - so the
     * Commission Trading tile in modules.html opened the Purchase Order screen and the
     * Commission Trading pages were unreachable from the menu. The mapping is removed here
     * rather than duplicated, because two @GetMapping("/commission") handlers would fail
     * startup with an ambiguous-mapping error.
     */

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
        return "stocks/stock_register";
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
        return "stocks/stock_as_on_date";
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
        return "stocks/brand_wise_stock";
    }
}
