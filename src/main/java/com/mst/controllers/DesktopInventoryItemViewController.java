package com.mst.controllers;

import com.mst.services.DesktopInventoryItemService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class DesktopInventoryItemViewController {
    private final DesktopInventoryItemService service;
    public DesktopInventoryItemViewController(DesktopInventoryItemService service){this.service=service;}
    @GetMapping({"/inventory/items","/inventory/items/list","/inventory/items/add"})
    public String page(){return "inventory/general_item";}
    /** Screen 180 DefineTaxItem (Architecture.WinApp.Tax_Definition, Taxation module 9): the same form in taxable mode. */
    @GetMapping("/taxation/define-tax-item")
    public String taxItemPage(org.springframework.ui.Model model){model.addAttribute("taxable",true);return "inventory/general_item";}
    @GetMapping("/inventory/items/edit/{id}")
    public String edit(@PathVariable int id){return "redirect:"+service.editRoute(id);}
}
