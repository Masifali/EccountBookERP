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
 * unreachable, however complete its screens are. Admin Panel also has seeded application rows
 * (App 18), independently of the fixed desktop gear menu served by GearMenuController.
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
    /*
     * "/apps" only.
     *
     * This method also claimed "/modules", which MainModulesController.mainModulesHub already
     * owns. Two handlers on one identical pattern are not resolved at startup - Spring throws
     * IllegalStateException: Ambiguous handler methods mapped for '/modules' on the FIRST
     * request, which is why the application started normally and then answered 500.
     *
     * The two pages are different things and must stay on different URLs:
     *   /apps    -> apps.html    "Applications"  - level 1, one card per application
     *   /modules -> modules.html "Modules Hub"   - linked from index.html and the stock pages
     */
    @GetMapping("/apps")
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
    public String app(@PathVariable("appName") String appName,
                      @RequestParam(value = "module", required = false) Integer moduleId,
                      Model model) {
        /* Inventory is drawn like every other application (frmMenue from the user's rights rows).
           /inventory/dashboard forwards here, so child-page Back links (?module=19) keep working. */
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

    /**
     * Taxation - dbo.App Id 13 "Taxation", whose one module (ModuleId 9) holds screens 172-180. This
     * replaces MainModulesController's redirect of /taxation to the Payables report.
     */
    /**
     * Export - dbo.App Id 8 "Export". For this company (78) CompanyRights activates exactly two screens
     * of AppModules 11 "Export": 881 frmGdBreakUpByInvoice and 882 frmExportInvoicePackingList, both
     * built on 2026-09-29 (ExportModuleController). Rendered through the generic renderer so the module
     * cards and counts come from the rights rows, as everywhere else.
     */
    @GetMapping("/export")
    public String export(@RequestParam(value = "module", required = false) Integer moduleId,
                         Model model) {
        return renderApp("Export", moduleId, model);
    }

    @GetMapping("/taxation")
    public String taxation(@RequestParam(value = "module", required = false) Integer moduleId,
                           Model model) {
        return renderApp("Taxation", moduleId, model);
    }

    /**
     * Master Data Definition - dbo.App Id 19, whose one module (AppModules 2039 "System_Level") holds
     * screens 742 and 749-756. This replaces DesktopInventoryLookupDefinitionController's alias of
     * /master-data-definition onto the Inventory "Lookup Definitions" page, an unrelated screen.
     */
    @GetMapping("/master-data-definition")
    public String masterDataDefinition(@RequestParam(value = "module", required = false) Integer moduleId,
                                       Model model) {
        return renderApp("Master Data Definition", moduleId, model);
    }

    /**
     * HRM - dbo.App Id 12, modules 2018-2027 (Profile, Policy, Employee, Device, Attendance, Leave, Loan,
     * Over Time, Approval, Payroll), 47 (CPL Leave) and 29 (HRM Reports). Every screen row of those modules
     * is mapped in DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID to its own /hrm/... page.
     */
    @GetMapping("/hrm")
    public String hrm(@RequestParam(value = "module", required = false) Integer moduleId,
                      Model model) {
        return renderApp("HRM", moduleId, model);
    }

    /**
     * The five desktop applications that had no Java port before 30-Sep-2026 (dbo.App rows):
     *   9 Fixed Assets (AppModules 2032), 23 Upload documents, 14 Logistics Management,
     *   15 Import, 16 Party Processing.
     * Each is rendered by the generic App/Module/Screen renderer, so the cards come from the
     * signed-in user's rights rows exactly as the desktop's frmMenue builds them. Every screen row
     * is mapped in DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID to its own page.
     */
    @GetMapping("/fixed-assets")
    public String fixedAssets(@RequestParam(value = "module", required = false) Integer moduleId, Model model) {
        return renderApp("Fixed Assets", moduleId, model);
    }

    @GetMapping("/upload-documents")
    public String uploadDocuments(@RequestParam(value = "module", required = false) Integer moduleId, Model model) {
        return renderApp("Upload documents", moduleId, model);
    }

    @GetMapping("/logistics")
    public String logistics(@RequestParam(value = "module", required = false) Integer moduleId, Model model) {
        return renderApp("Logistics Management", moduleId, model);
    }

    @GetMapping("/import")
    public String importApp(@RequestParam(value = "module", required = false) Integer moduleId, Model model) {
        return renderApp("Import", moduleId, model);
    }

    @GetMapping("/party-processing")
    public String partyProcessing(@RequestParam(value = "module", required = false) Integer moduleId, Model model) {
        return renderApp("Party Processing", moduleId, model);
    }

    /*
     * /admin-panel and /system-utilities are the fixed desktop gear menus served by
     * GearMenuController. The separately seeded Admin Panel application (App 18) remains
     * available at /app/Admin%20Panel with this user's allocated modules and screens.
     */

    /* ------------------------------------------------------------------------- the renderer */

    private String renderApp(String appName, Integer moduleId, Model model) {
        model.addAttribute("activeMenu", "apps");
        model.addAttribute("moduleTitle", appName);
        model.addAttribute("appName", appName);
        model.addAttribute("appRoute", "/app/" + urlPath(appName));

        Map<String, Object> app = dashboardModuleService.getAppByName(appName);
        if (app == null) {
            // A failed read must stay a load error; another diagnostic query can clear it.
            String loadError = dashboardModuleService.getLastError();
            model.addAttribute("appMissing", loadError == null);
            model.addAttribute("loadError", loadError);
            model.addAttribute("cards", new ArrayList<>());
            model.addAttribute("cardCount", 0);
            return "dashboard";
        }

        int appId = ((Number) app.get("appId")).intValue();
        List<Map<String, Object>> cards = dashboardModuleService.getModuleCards(appId);
        String loadError = dashboardModuleService.getLastError();

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
