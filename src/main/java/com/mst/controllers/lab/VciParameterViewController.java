package com.mst.controllers.lab;

import com.mst.services.lab.VciParameterService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (module 7) — screen 809 "VCI Parameter", desktop form Architecture.WinApp.Lookups.ExImVCIParameter
 * (ScreenName "ExImVCIParameter"). The page loads everything through {@link VciParameterRestController}
 * (/api/lab/vci-parameter).
 */
@Controller
public class VciParameterViewController {

    /**
     * btnVCICategoryDefine_Click (:412) opens the desktop form ExImVCICategory. That form has no Java page
     * known to this port, so the URL is empty and the toolbar button is rendered disabled. Set it to the
     * page's route when ExImVCICategory is ported.
     */
    public static final String VCI_CATEGORY_URL = "";

    @GetMapping("/quality/vci-parameter")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "VCI Parameter");
        model.addAttribute("screenId", VciParameterService.SCREEN_ID);
        model.addAttribute("vciCategoryUrl", VCI_CATEGORY_URL);
        return "lab/vci_parameter";
    }
}
