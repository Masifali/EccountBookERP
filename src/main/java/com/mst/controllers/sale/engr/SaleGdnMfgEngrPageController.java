package com.mst.controllers.sale.engr;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 844 frmGoodsDispatchNotesEngr (Architecture.WinApp.Mfg.frmGoodsDispatchNotesEngr, Sale Engr module 134). */
@Controller
public class SaleGdnMfgEngrPageController {

    @GetMapping("/sale/engr/mfg/gdn")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Goods Dispatch Notes");
        return "sale/engr/mfg/gdn_mfg_engr";
    }
}
