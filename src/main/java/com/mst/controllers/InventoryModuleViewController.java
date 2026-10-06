package com.mst.controllers;

import com.mst.models.*;
import com.mst.serviceInterface.*;
import com.mst.services.ChartofAccountService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;

@Controller
@RequestMapping("/inventory")
public class InventoryModuleViewController {

    @Autowired
    private IItemService itemService;
    @Autowired
    private IItemCategoryService itemCategoryService;
    @Autowired
    private IItemTypeService itemTypeService;
    @Autowired
    private IItemGroupService itemGroupService;
    @Autowired
    private IBrandService brandService;
    @Autowired
    private IWarehouseService warehouseService;
    @Autowired
    private IRackService rackService;
    @Autowired
    private IProductTypeService productTypeService;
    @Autowired
    private ChartofAccountService chartofAccountService;

    // 1. DASHBOARD
    /* The desktop opens the Inventory APPLICATION through frmMenue: module cards and the chosen module's
       screen cards, both built from USP_GetUserRightsForViewbyUserId - so a user sees exactly the screens
       allocated to them (Stock Reports: Stock Report Store, Transaction Report Vehicle Wise, Stock Evaluation
       Report Vehicle Wise, Transaction Report With/Without Value, Stock Report With/Without Values, Item Ledger).
       The hand-written inventory_dashboard.html tiles (fixed counts 11/1/8, invented screens such as
       Warehouse Stock Summary / Lot Wise / Brand Wise) are no longer used; the generic renderer draws the
       same page as every other application. The module query (?module=19) is carried by the forward. */
    @GetMapping({"", "/", "/dashboard"})
    public String inventoryDashboard(@RequestParam(value = "module", required = false) Integer moduleId) {
        return "forward:/app/Inventory";
    }

    // DefineItemGroup stores GroupId and ItemGroupName through its original procedures.
    @GetMapping({"/item_groups", "/item-groups"})
    public String viewItemGroups(Model model) { return "inventory/item_uom_groups"; }

    @GetMapping("/item_groups/edit/{id}")
    public String editItemGroup(@PathVariable("id") int id, Model model) {
        return "redirect:/inventory/item-groups?id=" + id;
    }
    // 6. WAREHOUSES: original InvWareHouse data is loaded by the scoped desktop API.
    @GetMapping("/warehouses")
    public String viewWarehouses(Model model) { return "inventory/warehouses"; }

    @GetMapping("/warehouse-racks")
    public String warehouseRacks() { return "inventory/warehouse_racks"; }

    @GetMapping("/warehouse-rack-items")
    public String warehouseRackItems() { return "inventory/warehouse_rack_items"; }

    @GetMapping("/warehouses/edit/{id}")
    public String editWarehouse(@PathVariable("id") int id, Model model) {
        return "redirect:/inventory/warehouses?id=" + id;
    }
    // 7. BRANDS
    @GetMapping("/brands")
    public String viewBrands(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("brands", brandService.getAll());
        model.addAttribute("brand", new Brand());
        return "inventory/brands";
    }

    @GetMapping("/brands/edit/{id}")
    public String editBrand(@PathVariable("id") int id, Model model) {
        Brand brand = brandService.getById(id);
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("brands", brandService.getAll());
        model.addAttribute("brand", brand != null ? brand : new Brand());
        return "inventory/brands";
    }

    @PostMapping("/brands/save")
    public String saveBrand(@ModelAttribute("brand") Brand brand) {
        brandService.addOrUpdate(brand);
        return "redirect:/inventory/brands";
    }

    @PostMapping("/brands/delete/{id}")
    public String deleteBrand(@PathVariable("id") int id) {
        brandService.delete(id);
        return "redirect:/inventory/brands";
    }

    // 8. PRODUCT TYPES
    @GetMapping({"/product_types", "/product-types"})
    public String viewProductTypes(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("productTypes", productTypeService.getAll());
        model.addAttribute("productType", new ProductType());
        return "inventory/product_types";
    }

    @GetMapping("/product_types/edit/{id}")
    public String editProductType(@PathVariable("id") int id, Model model) {
        ProductType productType = productTypeService.getById(id);
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("productTypes", productTypeService.getAll());
        model.addAttribute("productType", productType != null ? productType : new ProductType());
        return "inventory/product_types";
    }

    @PostMapping("/product_types/save")
    public String saveProductType(@ModelAttribute("productType") ProductType productType) {
        productTypeService.addOrUpdate(productType);
        return "redirect:/inventory/product_types";
    }

    @GetMapping("/product_types/delete/{id}")
    public String deleteProductType(@PathVariable("id") int id) {
        productTypeService.delete(id);
        return "redirect:/inventory/product_types";
    }

    // 9. RACKS
    @GetMapping("/racks")
    public String viewRacks(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("racks", rackService.getAll());
        model.addAttribute("rack", new Rack());
        return "inventory/racks";
    }

    @GetMapping("/racks/edit/{id}")
    public String editRack(@PathVariable("id") int id, Model model) {
        Rack rack = rackService.getById(id);
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("racks", rackService.getAll());
        model.addAttribute("rack", rack != null ? rack : new Rack());
        return "inventory/racks";
    }

    @PostMapping("/racks/save")
    public String saveRack(@ModelAttribute("rack") Rack rack) {
        rackService.addOrUpdate(rack);
        return "redirect:/inventory/racks";
    }

    @GetMapping("/racks/delete/{id}")
    public String deleteRack(@PathVariable("id") int id) {
        rackService.delete(id);
        return "redirect:/inventory/racks";
    }

    // 10. LOTS
    @GetMapping({"/lots", "/define_lots", "/define-lots"})
    public String viewLots(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "inventory/lots";
    }

    // 11. ITEM MIN MAX RATE
    @GetMapping({"/item_min_max_rate", "/item-min-max-rate"})
    public String viewItemMinMaxRate(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "inventory/item_min_max_rate";
    }

    // 12. CONSUMPTION ITEMS
    @GetMapping({"/consumption_items", "/consumption-items"})
    public String viewConsumptionItems(Model model) {
        model.addAttribute("activeMenu", "inventory");
        return "inventory/consumption_items";
    }


}


