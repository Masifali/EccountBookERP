package com.mst.controllers;

import com.mst.services.DashboardModuleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * DashBoard hub.
 *
 * The desktop builds its whole menu from the database - DashboardNew.InitializeMenu() (:1445)
 * clears both context menus and rebuilds them from the rights rows for this user and company,
 * grouped by AppModules.ModuleTypeId (1 = the Screens menu, 2 = the Reports menu) and then by
 * module. The btnDashBoard tree left in the designer (:5159) never shows: Items.Clear() runs
 * first. So there is no module called "DashBoard", and the earlier version of this page, which
 * filtered on ModuleDescription = 'DashBoard', could only ever return nothing.
 *
 * This hub is that same tree, with each screen linked to a page of this application only when
 * ScreenRouteIndex matched it to a route that is actually registered.
 */
@Controller
public class DashboardController {

    /** The module name as the rights rows spell it in ModuleDescription. */
    private static final String MODULE = "DashBoard";

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private DashboardModuleService dashboardModuleService;

    /**
     * The DashBoard screen - frmMenue at its module level, scoped to the "DashBoard" APPLICATION.
     * Cards are modules, captioned by ModuleDescription and numbered by their screen count, which
     * is exactly what the desktop window shows.
     */
    @GetMapping({"/", "/dashboard"})
    public String dashboardHub(@RequestParam(value = "module", required = false) Integer moduleId,
                               Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", MODULE);

        Map<String, Object> app = dashboardModuleService.getAppByName(MODULE);
        if (app == null) {
            /* No AppModules row is named "DashBoard". Show what the table does contain - the
               parent/child tree and this user's modules by AppId - rather than an empty page. */
            model.addAttribute("appMissing", true);
            model.addAttribute("parentDiag", dashboardModuleService.parentDiagnostics(MODULE));
            model.addAttribute("loadError", dashboardModuleService.getLastError());
            model.addAttribute("cards", new ArrayList<>());
            model.addAttribute("cardCount", 0);
            return "dashboard";
        }

        int appId = (Integer) app.get("appId");
        List<Map<String, Object>> cards = dashboardModuleService.getModuleCards(appId);
        String loadError = dashboardModuleService.getLastError();
        /* Found the row but it has no children this user may view - same panel, so the page can
           still say why instead of showing nothing. */
        if (cards.isEmpty() && loadError == null) {
            model.addAttribute("parentDiag", dashboardModuleService.parentDiagnostics(MODULE));
        }

        int screenCount = 0, builtCount = 0;
        for (Map<String, Object> c : cards) {
            screenCount += (Integer) c.get("count");
            builtCount  += (Integer) c.get("built");
        }

        /* The desktop shows both levels on ONE form: the module cards in accountsFlowLayout and
           the chosen module's screens in ScreensFlowlayout underneath (frmMenue). Same here, so
           a click does not leave the page. When only one module exists the desktop opens it
           straight away (:233), which is reproduced too. */
        Integer selected = moduleId;
        if (selected == null && cards.size() == 1) {
            selected = (Integer) cards.get(0).get("moduleId");
        }
        if (selected != null) {
            Map<String, Object> d = dashboardModuleService.getScreenCards(appId, selected);
            model.addAttribute("selectedModuleId", selected);
            model.addAttribute("selectedModuleTitle", d.get("moduleTitle"));
            model.addAttribute("screens", d.get("screens"));
        }

        model.addAttribute("appId", appId);
        model.addAttribute("appName", app.get("app"));
        model.addAttribute("rightsSource", dashboardModuleService.getRightsSource());
        model.addAttribute("cards", cards);
        model.addAttribute("cardCount", cards.size());
        model.addAttribute("screenCount", screenCount);
        model.addAttribute("builtCount", builtCount);
        model.addAttribute("loadError", loadError);
        model.addAttribute("noScreens", cards.isEmpty() && loadError == null);
        if (cards.isEmpty() && loadError == null) {
            model.addAttribute("diagnostics", dashboardModuleService.diagnose(null));
        }
        return "dashboard";
    }

    /**
     * One DashBoard module's screens - frmMenue.DynamicallyGenerateCards(ModId), reached by
     * clicking a module card. The module is checked against the application, so a hand-edited id
     * cannot reach screens from an application this user was not allocated.
     */
    @GetMapping("/dashboard/module")
    public String dashboardModule(@RequestParam("id") int moduleId, Model model) {
        model.addAttribute("activeMenu", "dashboard");

        Map<String, Object> app = dashboardModuleService.getAppByName(MODULE);
        if (app == null) return "redirect:/dashboard";

        int appId = (Integer) app.get("appId");
        Map<String, Object> d = dashboardModuleService.getScreenCards(appId, moduleId);

        @SuppressWarnings("unchecked")
        List<Map<String, Object>> screens = (List<Map<String, Object>>) d.get("screens");
        int built = 0;
        for (Map<String, Object> sc : screens) if (Boolean.TRUE.equals(sc.get("built"))) built++;

        model.addAttribute("moduleTitle", d.get("moduleTitle"));
        model.addAttribute("screens", screens);
        model.addAttribute("screenCount", screens.size());
        model.addAttribute("builtCount", built);
        model.addAttribute("loadError", dashboardModuleService.getLastError());
        return "dashboard_module";
    }

    /**
     * Every module this user can reach, across all applications - frmModules' own view, kept
     * because it is useful during the migration, but it is NOT the DashBoard screen.
     */
    @GetMapping("/dashboard/all")
    public String allModules(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "All Modules");

        List<Map<String, Object>> modules = dashboardModuleService.getMenu();
        List<Map<String, Object>> screenModules = new ArrayList<>();
        List<Map<String, Object>> reportModules = new ArrayList<>();
        int screenCount = 0, builtCount = 0;
        for (Map<String, Object> m : modules) {
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> screens = (List<Map<String, Object>>) m.get("screens");
            screenCount += screens.size();
            for (Map<String, Object> sc : screens) {
                if (Boolean.TRUE.equals(sc.get("built"))) builtCount++;
            }
            if (Integer.valueOf(2).equals(m.get("moduleTypeId"))) reportModules.add(m);
            else screenModules.add(m);
        }
        model.addAttribute("screenModules", screenModules);
        model.addAttribute("reportModules", reportModules);
        model.addAttribute("moduleCount", modules.size());
        model.addAttribute("cardCount", screenCount);
        model.addAttribute("builtCount", builtCount);
        model.addAttribute("routeCount", dashboardModuleService.routeCount());
        model.addAttribute("loadError", dashboardModuleService.getLastError());
        model.addAttribute("noScreens", modules.isEmpty());
        return "dashboard_all";
    }

    /**
     * A DashBoard screen that has not been ported yet. The card still appears on the hub because
     * the desktop shows it; this page names the desktop form rather than opening something else.
     */
    @GetMapping("/dashboard/screen")
    public String dashboardScreen(@RequestParam(value = "name", required = false) String name,
                                  Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleName", MODULE);
        model.addAttribute("moduleHome", "/dashboard");
        model.addAttribute("moduleTitle", name == null || name.isEmpty() ? "DashBoard Screen" : name);
        model.addAttribute("desktopForm", dashboardModuleService.desktopFormFor(name));
        /* Only claim an assembly when this really is a DashBoard-menu form. This route is the
           placeholder for every module's unbuilt screens, and naming the wrong project is a
           false statement about where the code lives. */
        model.addAttribute("desktopProject",
                dashboardModuleService.isDashboardOwnForm(name) ? "Architecture.WinApp.Dashboard" : null);
        return "screen_not_built";
    }

    /** Executive Business Analytics & KPI Board with charts. */
    @GetMapping({"/business-dashboard", "/analytics"})
    public String businessAnalyticsDashboard(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Business Analytics Dashboard");
        model.addAttribute("currentDate", LocalDate.now().toString());

        try {
            model.addAttribute("companies", jdbcTemplate.queryForList(
                    "SELECT ID as id, CompName as compName FROM Company ORDER BY CompName ASC"));
        } catch (Exception e) {
            model.addAttribute("companies", new ArrayList<>());
        }

        try {
            model.addAttribute("branches", jdbcTemplate.queryForList(
                    "SELECT ID as id, BranchName as branchName FROM Branches ORDER BY BranchName ASC"));
        } catch (Exception e) {
            model.addAttribute("branches", new ArrayList<>());
        }

        return "index";
    }
}
