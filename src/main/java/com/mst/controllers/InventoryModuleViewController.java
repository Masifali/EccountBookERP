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
    @GetMapping({"", "/", "/dashboard"})
    public String inventoryDashboard(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("definitionCount", 11);
        model.addAttribute("reportsCount", 1);
        model.addAttribute("stockReportsCount", 8);
        return "inventory/inventory_dashboard";
    }

    // 2. ITEMS
    @GetMapping({"/items", "/items/list"})
    public String viewItems(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("items", itemService.getAll());
        return "inventory/items";
    }

    @GetMapping("/items/add")
    public String addItemForm(Model model) {
        model.addAttribute("activeMenu", "inventory");
        Item item = new Item();
        item.setItemCategory(new ItemCategory());
        item.setItemType(new ItemType());
        item.setRack(new Rack());
        model.addAttribute("item", item);
        model.addAttribute("itemCategories", itemCategoryService.getAllItemCategories());
        model.addAttribute("categories", itemCategoryService.getAllItemCategories());
        model.addAttribute("itemTypes", itemTypeService.getAll());
        model.addAttribute("types", itemTypeService.getAll());
        model.addAttribute("racks", rackService.getAll());
        model.addAttribute("accounts", chartofAccountService.getAllAccounts());
        return "inventory/item_form";
    }

    @GetMapping("/items/edit/{id}")
    public String editItemForm(@PathVariable("id") int id, Model model) {
        Item item = itemService.getById(id);
        if (item == null) {
            return "redirect:/inventory/items";
        }
        if (item.getItemCategory() == null) item.setItemCategory(new ItemCategory());
        if (item.getItemType() == null) item.setItemType(new ItemType());
        if (item.getRack() == null) item.setRack(new Rack());
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("item", item);
        model.addAttribute("itemCategories", itemCategoryService.getAllItemCategories());
        model.addAttribute("categories", itemCategoryService.getAllItemCategories());
        model.addAttribute("itemTypes", itemTypeService.getAll());
        model.addAttribute("types", itemTypeService.getAll());
        model.addAttribute("racks", rackService.getAll());
        model.addAttribute("accounts", chartofAccountService.getAllAccounts());
        return "inventory/item_form";
    }

    @PostMapping("/items/save")
    public String saveItem(@ModelAttribute("item") Item item) {
        itemService.addOrUpdate(item);
        return "redirect:/inventory/items";
    }

    @GetMapping("/items/delete/{id}")
    public String deleteItem(@PathVariable("id") int id) {
        itemService.delete(id);
        return "redirect:/inventory/items";
    }

    // 3. ITEM CATEGORIES
    @GetMapping({"/item_categories", "/item-categories"})
    public String viewItemCategories(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("itemCategories", itemCategoryService.getAllItemCategories());
        model.addAttribute("itemCategory", new ItemCategory());
        model.addAttribute("accounts", chartofAccountService.getAllAccounts());
        return "inventory/item_categories";
    }

    @GetMapping("/item_categories/edit/{id}")
    public String editItemCategory(@PathVariable("id") int id, Model model) {
        ItemCategory category = itemCategoryService.getItemCategoryById(id);
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("itemCategories", itemCategoryService.getAllItemCategories());
        model.addAttribute("itemCategory", category != null ? category : new ItemCategory());
        model.addAttribute("accounts", chartofAccountService.getAllAccounts());
        return "inventory/item_categories";
    }

    @PostMapping("/item_categories/save")
    public String saveItemCategory(@ModelAttribute("itemCategory") ItemCategory itemCategory) {
        itemCategoryService.save(itemCategory);
        return "redirect:/inventory/item_categories";
    }

    @GetMapping("/item_categories/delete/{id}")
    public String deleteItemCategory(@PathVariable("id") int id) {
        itemCategoryService.delete(id);
        return "redirect:/inventory/item_categories";
    }

    // 4. ITEM TYPES
    @GetMapping({"/item_types", "/item-types"})
    public String viewItemTypes(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("itemTypes", itemTypeService.getAll());
        model.addAttribute("itemType", new ItemType());
        model.addAttribute("itemCategories", itemCategoryService.getAllItemCategories());
        return "inventory/item_types";
    }

    @GetMapping("/item_types/edit/{id}")
    public String editItemType(@PathVariable("id") int id, Model model) {
        ItemType itemType = itemTypeService.getById(id);
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("itemTypes", itemTypeService.getAll());
        model.addAttribute("itemType", itemType != null ? itemType : new ItemType());
        model.addAttribute("itemCategories", itemCategoryService.getAllItemCategories());
        return "inventory/item_types";
    }

    @PostMapping("/item_types/save")
    public String saveItemType(@ModelAttribute("itemType") ItemType itemType) {
        itemTypeService.addOrUpdate(itemType);
        return "redirect:/inventory/item_types";
    }

    @GetMapping("/item_types/delete/{id}")
    public String deleteItemType(@PathVariable("id") int id) {
        itemTypeService.delete(id);
        return "redirect:/inventory/item_types";
    }

    // 5. ITEM GROUPS
    @GetMapping({"/item_groups", "/item-groups"})
    public String viewItemGroups(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("itemGroups", itemGroupService.getAll());
        model.addAttribute("itemGroup", new ItemGroup());
        return "inventory/item_groups";
    }

    @GetMapping("/item_groups/edit/{id}")
    public String editItemGroup(@PathVariable("id") int id, Model model) {
        ItemGroup group = itemGroupService.getById(id);
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("itemGroups", itemGroupService.getAll());
        model.addAttribute("itemGroup", group != null ? group : new ItemGroup());
        return "inventory/item_groups";
    }

    @PostMapping("/item_groups/save")
    public String saveItemGroup(@ModelAttribute("itemGroup") ItemGroup itemGroup) {
        itemGroupService.addOrUpdate(itemGroup);
        return "redirect:/inventory/item_groups";
    }

    @GetMapping("/item_groups/delete/{id}")
    public String deleteItemGroup(@PathVariable("id") int id) {
        itemGroupService.delete(id);
        return "redirect:/inventory/item_groups";
    }

    // 6. WAREHOUSES
    @GetMapping("/warehouses")
    public String viewWarehouses(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("warehouses", warehouseService.getAll());
        model.addAttribute("warehouse", new Warehouse());
        return "inventory/warehouses";
    }

    @GetMapping("/warehouses/edit/{id}")
    public String editWarehouse(@PathVariable("id") int id, Model model) {
        Warehouse warehouse = warehouseService.getById(id);
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("warehouses", warehouseService.getAll());
        model.addAttribute("warehouse", warehouse != null ? warehouse : new Warehouse());
        return "inventory/warehouses";
    }

    @PostMapping("/warehouses/save")
    public String saveWarehouse(@ModelAttribute("warehouse") Warehouse warehouse) {
        warehouseService.addOrUpdate(warehouse);
        return "redirect:/inventory/warehouses";
    }

    @GetMapping("/warehouses/delete/{id}")
    public String deleteWarehouse(@PathVariable("id") int id) {
        warehouseService.delete(id);
        return "redirect:/inventory/warehouses";
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

    @GetMapping("/brands/delete/{id}")
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

    // 13. POS DEFINE ITEM
    @GetMapping({"/pos_define_item", "/pos-define-item"})
    public String posDefineItem(Model model) {
        model.addAttribute("activeMenu", "inventory");
        Item item = new Item();
        item.setItemCategory(new ItemCategory());
        item.setItemType(new ItemType());
        item.setRack(new Rack());
        model.addAttribute("item", item);
        model.addAttribute("itemCategories", itemCategoryService.getAllItemCategories());
        model.addAttribute("categories", itemCategoryService.getAllItemCategories());
        model.addAttribute("itemTypes", itemTypeService.getAll());
        model.addAttribute("types", itemTypeService.getAll());
        model.addAttribute("racks", rackService.getAll());
        model.addAttribute("accounts", chartofAccountService.getAllAccounts());
        return "inventory/item_form";
    }
}
