package com.mst.controllers.quality;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * dbo.App 4 "Quality Control" through the generic App / Module / Screen renderer (AppMenuController),
 * like the Accounts, Purchase, Production, HRM and Taxation hubs: the module cards (AppModules 7 "Lab",
 * 1011 "Lab Report") and their counts come from the user's CompanyRights + ScreenRights rows, as the
 * desktop frmMenue builds them.
 *
 * The old quality_control/dashboard.html was hand-written: typed counts "10" and "6", two tiles for
 * screens that do not exist on the desktop ("Lab Master", "Lab Report") and its own look. For company 78
 * the seeder enables 8 screens in module 7 (155, 156, 157, 158, 159, 160, 162, 163) and 6 reports in
 * module 1011 (626, 629, 630, 631, 632, 633).
 *
 * Every screen route that used to live here (all mock-ups whose Save returned a typed "success") now
 * belongs to its own controller in com.mst.controllers.lab, ported from the desktop form (2026-09-30).
 */
@Controller
@RequestMapping({"/quality", "/quality_control", "/quality-control"})
public class QualityControlViewController {

    @GetMapping({"", "/", "/dashboard"})
    public String dashboard() {
        return "forward:/app/QualityControl";
    }

    @GetMapping("/setup")
    public String labModule() {
        return "forward:/app/QualityControl?module=7";
    }

    @GetMapping("/reports")
    public String labReports() {
        return "forward:/app/QualityControl?module=1011";
    }

    /** Screen 156 is served by LabModuleViewController; the old URL is kept for existing links. */
    @GetMapping("/item-analysis-parameter")
    public String itemAnalysisParameter() {
        return "redirect:/lab/item-analysis-parameter";
    }

    /** "Lab Master" and "Lab Report" were invented tiles - no such screens in ScreenDefinition. */
    @GetMapping({"/lab", "/lab-report"})
    public String inventedScreens() {
        return "redirect:/quality";
    }
}
