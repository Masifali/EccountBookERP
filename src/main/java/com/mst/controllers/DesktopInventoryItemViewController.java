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
    @GetMapping("/inventory/items/edit/{id}")
    public String edit(@PathVariable int id){return "redirect:"+service.editRoute(id);}
}
