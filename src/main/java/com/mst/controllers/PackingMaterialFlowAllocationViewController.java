package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class PackingMaterialFlowAllocationViewController {
    @GetMapping("/packing-material/allocate-items-to-flow")
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Packing Material Items Allocate To Transaction Flow");
        return "packing_material/allocate_items_to_flow";
    }
}
