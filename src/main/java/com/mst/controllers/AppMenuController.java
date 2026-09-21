package com.mst.controllers;

import com.mst.services.DashboardModuleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestParam;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * The desktop's menu, at the two levels the port was missing.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS EXISTS
 * ---------------------------------------------------------------------------------------------
 * The desktop has three levels (see DASHBOARD-CORRECT-LEVEL-APP-MODULE-SCREEN):
 *
 *   1  frmModules.DynamicallyGenerateCardsForApp()   one card per APPLICATION
 *   2  frmMenue.DynamicallyGenerateCardsForModule()  one card per MODULE, numbered by screen count
 *   3  frmMenue.DynamicallyGenerateCards(ModId)      one card per SCREEN
 *
 * {@code /dashboard} already does levels 2 and 3 — but only for the application literally named
 * "DashBoard", because the name is a constant in that controller. Every other application's
 * landing page was hand-written instead, which is why the Production page showed its screens
 * flattened into two invented sections ("Transactions (Module 18)" / "Reports (Module 21)") where
 * the desktop shows two module cards reading **Production 3** and **Production Reports 5**.
 *
 * A hand-written page also means an application nobody has written a page for is simply
 * unreachable, however complete its screens are. (Admin Panel and System Utilities are NOT such
 * applications - they are a fixed gear menu; see GearMenuController.)
 *
 * So the level-2/3 page is made generic here, driven by the same rights rows the desktop reads,
 * and level 1 is added so every application the user is allocated has a way in.
 *
 * ---------------------------------------------------------------------------------------------
 * COUNTS COME FROM THE RIGHTS ROWS, NOT FROM A LIST IN THE CODE
 * ---------------------------------------------------------------------------------------------
 * A card's number is how many screens that module actually contains for THIS user, exactly as
 * {@code lg.Count()} does on the desktop. Nothing here enumerates screens by hand, so a screen
 * added to a module in the database appears without a code change — and a module the user may not
 * view never appears at all.
 */
@Controller
public class AppMenuController {

    @Autowired
    private DashboardModuleService dashboardModuleService;

    /**
     * Level 1 — every application this user may view.
     */
    @GetMapping({"/apps", "/modules"})
    public String apps(Model model) {
        model.addAttribute("activeMenu", "apps");
        List<Map<String, Object>> cards = dashboardModuleService.getAppCards();

        int screenCount = 0, builtCount = 0;
        for (Map<String, Object> c : cards) {
            screenCount += ((Number) c.get("count")).intValue();
            builtCount  += ((Number) c.get("built")).intValue();
        }
        model.addAttribute("cards", cards);
        model.addAttribute("cardCount", cards.size());
        model.addAttribute("screenCount", screenCount);
        model.addAttribute("builtCount", builtCount);
        model.addAttribute("rightsSource", dashboardModuleService.getRightsSource());
        model.addAttribute("loadError", dashboardModuleService.getLastError());
        return "apps";
    }

    /**
     * Levels 2 and 3 for any application, by name. The name is matched with letters and digits
     * only, so "Admin Panel", "admin-panel" and "AdminPanel" all reach the same application.
     *
     * Renders the same view {@code /dashboard} uses, because it is the same desktop form
     * (frmMenue) showing the same two levels — module cards, and the chosen module's screens
     * underneath without leaving the page.
     */
    @GetMapping("/app/{appName}")
    public String app(@PathVariable String appName,
                      @RequestParam(value = "module", required = false) Integer moduleId,
                      Model model) {
        return renderApp(appName, moduleId, model);
    }

    /* ----------------------------------------------------------------- named entry points */

    /**
     * Production. This replaces the hand-written landing page, whose sections and counts were
     * written by hand and did not match the desktop's two module cards.
     */
    @GetMapping("/production")
    public String production(@RequestParam(value = "module", required = false) Integer moduleId,
                             Model model) {
        return renderApp("Production", moduleId, model);
    }

    /*
     * "Admin Panel" and "System Utilities" USED to be routed from here, as if they were rows in
     * the App table. They are not. They are contextMenuStrip3 on DashboardNew - a fixed menu
     * written into the form, whose items open forms directly and have no ScreenDefinition rows at
     * all. That is why no application named "System Utilities" was ever found. Both now live in
     * GearMenuController, which reproduces the designer's own item list.
     */

    /* ------------------------------------------------------------------------- the renderer */

    private String renderApp(String appName, Integer moduleId, Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("moduleTitle", appName);

        Map<String, Object> app = dashboardModuleService.getAppByName(appName);
        if (app == null) {
            /* The same panel /dashboard shows when its own name does not match: the distinct
               { AppId, App } pairs this user actually has, so the real spelling is visible
               instead of an empty page. */
            model.addAttribute("appMissing", true);
            model.addAttribute("parentDiag", dashboardModuleService.parentDiagnostics(appName));
            model.addAttribute("loadError", dashboardModuleService.getLastError());
            model.addAttribute("cards", new ArrayList<>());
            model.addAttribute("cardCount", 0);
            return "dashboard";
        }

        int appId = ((Number) app.get("appId")).intValue();
        List<Map<String, Object>> cards = dashboardModuleService.getModuleCards(appId);
        String loadError = dashboardModuleService.getLastError();
        if (cards.isEmpty() && loadError == null) {
            model.addAttribute("parentDiag", dashboardModuleService.parentDiagnostics(appName));
        }

        int screenCount = 0, builtCount = 0;
        for (Map<String, Object> c : cards) {
            screenCount += ((Number) c.get("count")).intValue();
            builtCount  += ((Number) c.get("built")).intValue();
        }

        /* frmMenue:233 — with exactly one module the desktop opens it straight away. */
        Integer selected = moduleId;
        if (selected == null && cards.size() == 1) {
            selected = ((Number) cards.get(0).get("moduleId")).intValue();
        }
        if (selected != null) {
            Map<String, Object> d = dashboardModuleService.getScreenCards(appId, selected);
            model.addAttribute("selectedModuleId", selected);
            model.addAttribute("selectedModuleTitle", d.get("moduleTitle"));
            model.addAttribute("screens", d.get("screens"));
        }

        model.addAttribute("appId", appId);
        model.addAttribute("appName", app.get("app"));
        model.addAttribute("appRoute", "/app/" + urlPath(appName));
        model.addAttribute("rightsSource", dashboardModuleService.getRightsSource());
        model.addAttribute("cards", cards);
        model.addAttribute("cardCount", cards.size());
        model.addAttribute("screenCount", screenCount);
        model.addAttribute("builtCount", builtCount);
        model.addAttribute("loadError", loadError);
        model.addAttribute("noScreens", cards.isEmpty() && loadError == null);
        return "dashboard";
    }

    private static String urlPath(String s) {
        try {
            return java.net.URLEncoder.encode(s, "UTF-8").replace("+", "%20");
        } catch (java.io.UnsupportedEncodingException e) {
            return s;
        }
    }
}
