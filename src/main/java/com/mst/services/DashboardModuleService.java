package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DashBoard module.
 *
 * The desktop's menu is NOT hard-coded - DashboardNew.cs builds every menu item from the database
 * and resolves the form by the row's own TargetUrl:
 *
 *   clsGlobalVariables.ScreenViewReights = GetViewRightsByUserId();            (DashboardNew.cs :1198)
 *      -> tblUserRights.GetUserRightsForViewbyUserId(...)                      (BLL :276-313)
 *      -> USP_GetUserRightsForViewbyUserId  @UserId, @CompanyId [, @AppId, @AppModuleId]
 *
 *   MenuItem_Click  looks the clicked item up by ScreenID and opens Type.GetType(TargetUrl)  (:1553)
 *   ScreenBindInNavigation  keeps only rows where Value is true and TargetUrl is not empty   (:1595)
 *
 * So the DashBoard hub's cards are whatever that procedure returns for this user and company, in
 * the module the row names - the same source, the same per-user rights. Nothing is invented here.
 *
 * Related procedures, kept for reference and used by getModules():
 *   usp_getModulesByCompanyId          @CompanyId, @ModuleTypeId, @UserId
 *   usp_getScreensByModuleId           @ModuleId, @ModuleTypeId, @UserId
 *   usp_getScreenRightsByModuleAndUserId  @ScreenId, @UserId, @AppId
 */
@Service
public class DashboardModuleService {

    private static final Logger LOG = LoggerFactory.getLogger(DashboardModuleService.class);

    /** Not thread-shared state that matters: the hub reads it immediately after the call. */
    private volatile String lastError;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ScreenRouteIndex screenRouteIndex;

    /**
     * The desktop reaches this through USP_GetUserRightsForViewbyUserId, but that procedure appears
     * nowhere else in this project and could not be confirmed against the database from here. The
     * same rows are available through the join DesktopUserRightsRepository.filterScreens already
     * uses successfully (tblUserRights -> ScreenDefinition -> AppModules -> CompanyRights), so that
     * proven path is used and extended with TargetUrl and the View right:
     *
     *   ScreenRights holds one row per right per screen; the View right is the one
     *   DashboardNew.cs's ScreenViewReights list is built from, and tblUserRights.Value is the
     *   per-user flag for it. ScreenBindInNavigation (:1595) also drops rows with no TargetUrl.
     */
    private static final String SQL_VIEW_RIGHTS =
            "SELECT DISTINCT S.Id AS ScreenID, S.ScreenName, S.ScreenAlias, S.TargetUrl, "
          + "S.SortNo, AP.ModuleDescription "
          + "FROM tblUserRights UR "
          + "JOIN ScreenDefinition S ON UR.ScreenId = S.Id AND S.IsActive = 1 "
          + "JOIN AppModules AP ON S.ModuleId = AP.Id "
          /* The column is RightId, not RightsID: the C# model property is tblUserRights.RightsID but
             the table column is RightId - resources/sql/user-rights-report.sql, which runs against
             this database, joins ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID. */
          + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
          + "AND SR.RightName = 'View' "
          + "JOIN CompanyRights CR ON CR.ScreenId = S.Id AND CR.CompanyId = ? "
          + "WHERE UR.CompanyId = ? AND UR.UserId = ? "
          + "AND ISNULL(CR.IsActive, 0) = 1 "
          + "AND ISNULL(UR.Value, 0) = 1 "
          + "AND ISNULL(S.TargetUrl, '') <> '' "
          + "AND AP.ModuleDescription = ? "
          + "ORDER BY S.SortNo, S.ScreenAlias";

    /* The BLL's own module call is usp_getModulesByCompanyId, but DesktopUserRightsRepository
       already uses USP_GetAppModulesByCompany against this database successfully, so the proven
       one is used here rather than a second, unverified procedure name. */
    private static final String SQL_MODULES =
            "EXEC dbo.USP_GetAppModulesByCompany @CompanyId=?, @UserId=?";

    /**
     * Where a ported screen lives on the web, keyed by the desktop TargetUrl's class name (the part
     * after the last dot). A screen that is not in this map is reported as not built, naming its
     * desktop form - the card still appears, because the desktop shows it, but it opens an honest
     * placeholder instead of an unrelated page.
     *
     * The previous hub had six invented cards ("Executive DashBoards", "Followups & Pending Works
     * DashBoards" ...) whose links went to receivables, purchase and quality report pages - screens
     * from other modules entirely.
     */
    private static final Map<String, String> WEB_ROUTES = new LinkedHashMap<>();
    static {
        /* Deliberately empty.
         *
         * Four entries used to live here and all four were wrong:
         *
         *   AcFrmDashboard          -> /accounts/dashboard
         *        No controller maps that path. No @GetMapping("/dashboard") exists under the
         *        @RequestMapping("/accounts") controllers, and requesting it is refused. Several
         *        templates link to it too (index.html :323, modules.html :207,
         *        account/custom_group.html :270) - those links are broken for the same reason.
         *        "Accounts Current Postition (Dashboard)" was showing as built because of it.
         *
         *   SalesAnalytics          -> /business-dashboard
         *   SalesAnalyticsDashBoard -> /business-dashboard
         *   frmSalesAnalytics       -> /business-dashboard
         *        /business-dashboard renders index.html, a generic KPI page written for this web
         *        app. It is not a port of SalesAnalytics.cs (523 lines,
         *        Architecture.WinApp.Dashboard), so sending that screen there breaks the rule that
         *        a screen must never open an unrelated page.
         *
         * A screen is linked only when ScreenRouteIndex finds a REGISTERED route whose name
         * matches it exactly. Add an entry here only for a page that genuinely ports that desktop
         * form and whose name does not match by itself. */
    }

    /** The desktop forms behind the DashBoard menu, for the placeholder to name (DashboardNew.cs :5161-5196). */
    private static final Map<String, String> DESKTOP_FORMS = new LinkedHashMap<>();
    static {
        DESKTOP_FORMS.put("PendingVouchersForApproval",   "PendingVouchersForApproval.cs");
        DESKTOP_FORMS.put("frmPendingWorksRpt",           "frmPendingWorksRpt.cs");
        DESKTOP_FORMS.put("frmStockDashboard",            "frmStockDashboard.cs");
        DESKTOP_FORMS.put("btnStocksDashBoard",           "frmStockDashboard.cs");
        DESKTOP_FORMS.put("UnApprovedVouchersDashBoard",  "UnApprovedInvoicesAndVouchers.cs");
        DESKTOP_FORMS.put("UnApprovedInvoicesAndVouchers","UnApprovedInvoicesAndVouchers.cs");
        DESKTOP_FORMS.put("OrderManagementDashboard",     "OrderManagementDashboard.cs");
        DESKTOP_FORMS.put("frmPaddyPurchaseAnalytics",    "PurchaseAnalytiicsDashBoard.cs");
        DESKTOP_FORMS.put("frmRicePurchaseAnalytics",     "PurchaseAnalytiicsDashBoard.cs");
    }

    /**
     * Cards for one module, straight from the database.
     *
     * @param moduleName matched case-insensitively against the row's ModuleDescription
     */
    public List<Map<String, Object>> getModuleScreens(String moduleName) {
        lastError = null;
        List<Map<String, Object>> cards = new ArrayList<>();
        try {
            int compId = currentUserContext.currentCompanyId();
            int userId = currentUserContext.currentUserId();

            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    SQL_VIEW_RIGHTS, compId, compId, userId, moduleName)) {

                String targetUrl = str(col(r, "TargetUrl"));
                if (targetUrl.isEmpty()) continue;          // belt and braces; the SQL filters it too

                String cls = targetUrl.contains(".")
                        ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1)
                        : targetUrl;

                Map<String, Object> card = new LinkedHashMap<>();
                card.put("screenId", col(r, "ScreenID"));
                card.put("screenName", str(col(r, "ScreenName")));
                // ScreenAlias is the caption the desktop puts on the menu item and the form
                card.put("title", str(col(r, "ScreenAlias")).isEmpty()
                        ? str(col(r, "ScreenName")) : str(col(r, "ScreenAlias")));
                card.put("module", str(col(r, "ModuleDescription")));
                card.put("targetUrl", targetUrl);
                card.put("iconUrl", "");
                card.put("isFavorite", false);

                String route = WEB_ROUTES.get(cls);
                card.put("built", route != null);
                card.put("route", route != null
                        ? route
                        : "/dashboard/screen?name=" + urlEncode(cls));
                card.put("desktopForm", DESKTOP_FORMS.getOrDefault(cls, cls + ".cs"));
                cards.add(card);
            }
        } catch (Exception e) {
            /* An empty list and a failed query are NOT the same thing: the first means the screens
               are not allocated, the second means the page is broken. The hub must be able to tell
               the user which, so the message is kept rather than swallowed. */
            LOG.error("Dashboard module screens failed for module '{}'", moduleName, e);
            lastError = e.getMessage();
            return Collections.emptyList();
        }
        return cards;
    }

    /**
     * Why is the list empty?
     *
     * The main query joins five tables and applies five predicates, so an empty result has a dozen
     * possible causes and the page cannot guess. This walks the same joins one step at a time and
     * reports where the rows disappear, plus the module names that ARE available - the usual cause
     * being that AppModules.ModuleDescription does not read exactly "DashBoard".
     *
     * Runs only when the main query returned nothing, and never throws: a diagnostic that fails
     * must not replace the answer it is diagnosing.
     */
    public Map<String, Object> diagnose(String moduleName) {
        Map<String, Object> d = new LinkedHashMap<>();
        int compId, userId;
        try {
            compId = currentUserContext.currentCompanyId();
            userId = currentUserContext.currentUserId();
        } catch (Exception e) {
            d.put("error", "No signed-in user/company context: " + e.getMessage());
            return d;
        }
        d.put("companyId", compId);
        d.put("userId", userId);
        d.put("moduleAskedFor", moduleName == null ? "(all modules)" : moduleName);

        String base = "FROM tblUserRights UR "
                    + "JOIN ScreenDefinition S ON UR.ScreenId = S.Id AND S.IsActive = 1 "
                    + "JOIN AppModules AP ON S.ModuleId = AP.Id "
                    + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
                    + "AND SR.RightName = 'View' "
                    + "JOIN CompanyRights CR ON CR.ScreenId = S.Id AND CR.CompanyId = ? ";

        count(d, "1. rights rows for this user+company",
              "SELECT COUNT(*) FROM tblUserRights WHERE CompanyId = ? AND UserId = ?", compId, userId);
        count(d, "2. + active screen",
              "SELECT COUNT(*) FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId = S.Id "
            + "AND S.IsActive = 1 WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, userId);
        count(d, "3. + module join",
              "SELECT COUNT(*) FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId = S.Id "
            + "AND S.IsActive = 1 JOIN AppModules AP ON S.ModuleId = AP.Id "
            + "WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, userId);
        count(d, "4. + View right row",
              "SELECT COUNT(*) FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId = S.Id "
            + "AND S.IsActive = 1 JOIN AppModules AP ON S.ModuleId = AP.Id "
            + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
            + "AND SR.RightName = 'View' WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, userId);
        count(d, "5. + company allocation", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, compId, userId);
        count(d, "6. + CompanyRights.IsActive = 1", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1",
              compId, compId, userId);
        count(d, "7. + View right granted (UR.Value = 1)", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
            + "AND ISNULL(UR.Value, 0) = 1", compId, compId, userId);
        count(d, "8. + TargetUrl present", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
            + "AND ISNULL(UR.Value, 0) = 1 AND ISNULL(S.TargetUrl, '') <> ''", compId, compId, userId);
        /* Step 9 only applies when a single module was asked for. The hub no longer filters by
           module - there is no "DashBoard" module on the desktop either - so it passes null. */
        if (moduleName != null && !moduleName.isEmpty()) {
            count(d, "9. + module = '" + moduleName + "'", "SELECT COUNT(*) " + base
                + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
                + "AND ISNULL(UR.Value, 0) = 1 AND ISNULL(S.TargetUrl, '') <> '' "
                + "AND AP.ModuleDescription = ?", compId, compId, userId, moduleName);
        }

        /* The decisive one: what module names does this user actually have screens under? */
        List<String> modules = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "SELECT AP.ModuleDescription, COUNT(*) AS Screens " + base
                  + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
                  + "AND ISNULL(UR.Value, 0) = 1 AND ISNULL(S.TargetUrl, '') <> '' "
                  + "GROUP BY AP.ModuleDescription ORDER BY AP.ModuleDescription",
                    compId, compId, userId)) {
                modules.add(str(col(r, "ModuleDescription")) + " (" + str(col(r, "Screens")) + ")");
            }
        } catch (Exception e) {
            modules.add("could not be listed: " + e.getMessage());
        }
        d.put("modulesAvailableToYou", modules);
        return d;
    }

    private void count(Map<String, Object> into, String label, String sql, Object... args) {
        try {
            Integer n = jdbcTemplate.queryForObject(sql, Integer.class, args);
            into.put(label, n == null ? 0 : n);
        } catch (Exception e) {
            into.put(label, "failed: " + e.getMessage());
        }
    }


    /**
     * The whole menu this user can see, exactly as DashboardNew.InitializeMenu() builds it (:1445).
     *
     * The desktop does NOT have a module called "DashBoard" - the btnDashBoard tree in the
     * designer (:5159) is dead code, because InitializeMenu() starts by clearing both context
     * menus and rebuilds every item from the rights rows:
     *
     *     ModuleTypeId == 1  ->  ContextMenuStrip1   (the Screens menu)
     *     ModuleTypeId == 2  ->  contextMenuStrip2   (the Reports menu)
     *
     * grouped by { ModuleID, ModuleDescription }, each group's children being the rows with that
     * ModuleID and Value = true, captioned by ScreenAlias and opened by ScreenID.
     *
     * So the web hub shows the same two groups of modules, and a screen's link is resolved by
     * ScreenRouteIndex against the routes this application really registered - never invented.
     *
     * @return one entry per module: moduleTypeId, moduleId, moduleDescription, sortNo, screens
     */
    public List<Map<String, Object>> getMenu() {
        lastError = null;
        List<Map<String, Object>> modules = new ArrayList<>();
        try {
            int compId = currentUserContext.currentCompanyId();
            int userId = currentUserContext.currentUserId();

            Map<String, Map<String, Object>> byModule = new LinkedHashMap<>();

            for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_MENU, compId, compId, userId)) {
                String targetUrl = str(col(r, "TargetUrl"));
                if (targetUrl.isEmpty()) continue;

                String moduleId = str(col(r, "ModuleId"));
                Map<String, Object> mod = byModule.get(moduleId);
                if (mod == null) {
                    mod = new LinkedHashMap<>();
                    mod.put("moduleId", col(r, "ModuleId"));
                    mod.put("moduleTypeId", asInt(col(r, "ModuleTypeId")));
                    mod.put("moduleDescription", str(col(r, "ModuleDescription")));
                    mod.put("sortNo", asInt(col(r, "ModuleSortNo")));
                    mod.put("screens", new ArrayList<Map<String, Object>>());
                    byModule.put(moduleId, mod);
                    modules.add(mod);
                }

                String screenName = str(col(r, "ScreenName"));
                String alias = str(col(r, "ScreenAlias"));
                String route = screenRouteIndex.routeFor(screenName, targetUrl);

                String cls = targetUrl.contains(".")
                        ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1) : targetUrl;
                /* An explicit mapping always wins over the name match. */
                String explicit = WEB_ROUTES.get(cls);
                if (explicit != null) route = explicit;

                Map<String, Object> sc = new LinkedHashMap<>();
                sc.put("screenId", col(r, "ScreenID"));
                sc.put("screenName", screenName);
                sc.put("title", alias.isEmpty() ? screenName : alias);
                sc.put("targetUrl", targetUrl);
                sc.put("built", route != null);
                sc.put("route", route != null
                        ? route
                        : "/dashboard/screen?name=" + urlEncode(cls));
                sc.put("desktopForm", DESKTOP_FORMS.getOrDefault(cls, cls + ".cs"));

                @SuppressWarnings("unchecked")
                List<Map<String, Object>> screens = (List<Map<String, Object>>) mod.get("screens");
                screens.add(sc);
            }
        } catch (Exception e) {
            LOG.error("Dashboard menu read failed", e);
            lastError = e.getMessage();
            return Collections.emptyList();
        }
        return modules;
    }

    /**
     * Same joins as SQL_VIEW_RIGHTS, minus the single-module filter and carrying the module's own
     * type and sort order, because InitializeMenu() groups by ModuleTypeId then ModuleID.
     * AppModules.ModuleTypeId / SortNo are columns of that table - Architecture.Model.AppModules
     * declares AppId, Id, ModuleTypeId, SortNo, IconUrl, ModelMenuControllName, ModuleDescription,
     * and GenericProvider maps that model one property per column.
     */
    private static final String SQL_MENU =
            "SELECT DISTINCT S.Id AS ScreenID, S.ScreenName, S.ScreenAlias, S.TargetUrl, "
          + "S.SortNo, AP.Id AS ModuleId, AP.ModuleDescription, ISNULL(AP.AppId, 0) AS AppId, "
          + "ISNULL(AP.ModuleTypeId, 0) AS ModuleTypeId, ISNULL(AP.SortNo, 0) AS ModuleSortNo "
          + "FROM tblUserRights UR "
          + "JOIN ScreenDefinition S ON UR.ScreenId = S.Id AND S.IsActive = 1 "
          + "JOIN AppModules AP ON S.ModuleId = AP.Id "
          + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
          + "AND SR.RightName = 'View' "
          + "JOIN CompanyRights CR ON CR.ScreenId = S.Id AND CR.CompanyId = ? "
          + "WHERE UR.CompanyId = ? AND UR.UserId = ? "
          + "AND ISNULL(CR.IsActive, 0) = 1 "
          + "AND ISNULL(UR.Value, 0) = 1 "
          + "AND ISNULL(S.TargetUrl, '') <> '' "
          + "ORDER BY ISNULL(AP.ModuleTypeId, 0), ISNULL(AP.SortNo, 0), AP.ModuleDescription, "
          + "S.SortNo, S.ScreenAlias";

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    /** How many page routes this application registered - shown when the hub has nothing to show. */
    public int routeCount() { return screenRouteIndex.size(); }


    // ================================================================ the DashBoard screen itself

    /**
     * The rights rows the desktop menu is built from - its own procedure, not a rebuilt join.
     *
     *   DashboardNew.GetViewRightsByUserId()            (:1791-1812)
     *     -> tblUserRights.GetUserRightsForViewbyUserId (BLL :276-313)
     *        USP_GetUserRightsForViewbyUserId @UserId, @CompanyId
     *            [, @AppId]        only when non-zero
     *            [, @AppModuleId]  only when non-zero
     *
     * The rows come back as Architecture.Model.tblUserRights, which GenericProvider maps one
     * property per column, so every column the menu needs is on them: AppId, App, ModuleID,
     * ModuleDescription, ModuleTypeId, IconUrl, ScreenID, ScreenName, ScreenAlias, TargetUrl,
     * Value, IsFavorite.
     *
     * THIS is why the desktop works where the web did not. Earlier versions of this class rebuilt
     * the join by hand (tblUserRights -> ScreenDefinition -> AppModules -> ScreenRights ->
     * CompanyRights) and took AppId from AppModules, which is not the same AppId the rights rows
     * carry. Same user, same company, same database - different column. The procedure is the
     * desktop's own and is demonstrably working, so it is the source here too; the hand-built
     * join stays only as a fallback if the procedure fails, and the page says which was used.
     */
    public List<Map<String, Object>> viewRights() {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@UserId");    args.add(currentUserContext.currentUserId());
        names.add("@CompanyId"); args.add(currentUserContext.currentCompanyId());
        /* BLL :291-297 appends @AppId only when it is non-zero; DashboardNew passes
           UserAccount.AppId, or 0 when it is 3. Omitting a parameter is not the same as
           passing NULL, so it is left off entirely when it would be zero. */
        int appId = 0;
        try { appId = currentUserContext.currentAppId(); } catch (Exception ignored) { }
        if (appId != 0 && appId != 3) { names.add("@AppId"); args.add(appId); }

        StringBuilder sql = new StringBuilder("EXEC USP_GetUserRightsForViewbyUserId ");
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append(names.get(i)).append("=?");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
            /* ScreenBindInNavigation (:1595) keeps only granted rows that have a TargetUrl. */
            if (!truthy(col(r, "Value"))) continue;
            if (str(col(r, "TargetUrl")).isEmpty()) continue;
            out.add(r);
        }
        return out;
    }

    /** The rights rows, from the procedure when it works and from the proven join when it does not. */
    private List<Map<String, Object>> rights() {
        lastError = null;
        rightsSource = "USP_GetUserRightsForViewbyUserId";
        try {
            return viewRights();
        } catch (Exception e) {
            LOG.error("USP_GetUserRightsForViewbyUserId failed; falling back to the rebuilt join", e);
            rightsSource = "rebuilt join (procedure failed: " + e.getMessage() + ")";
        }
        try {
            return jdbcTemplate.queryForList(SQL_MENU,
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId());
        } catch (Exception e) {
            LOG.error("Rebuilt join failed too", e);
            lastError = e.getMessage();
            return Collections.emptyList();
        }
    }

    private volatile String rightsSource = "";

    /** Which source the last read used - shown on the page so the two can never be confused. */
    public String getRightsSource() { return rightsSource; }

    /**
     * frmModules groups the rights rows by { AppId, App } (:69-98). "DashBoard" is one of those
     * App values, carried on the rows themselves.
     */
    public Map<String, Object> getAppByName(String appName) {
        for (Map<String, Object> r : rights()) {
            if (squash(str(col(r, "App"))).equals(squash(appName))) {
                Map<String, Object> out = new LinkedHashMap<>();
                out.put("appId", asInt(col(r, "AppId")));
                out.put("app", str(col(r, "App")));
                return out;
            }
        }
        return null;
    }

    /**
     * When no App matches, print what the rows actually carry - the distinct { AppId, App } pairs
     * and the modules inside each - so the right name is visible instead of an empty page.
     */
    public Map<String, Object> parentDiagnostics(String appName) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("lookedFor", appName);
        d.put("source", rightsSource);
        try {
            List<Map<String, Object>> rows = rights();
            d.put("rowsReturned", rows.size());
            Map<String, List<String>> byApp = new LinkedHashMap<>();
            for (Map<String, Object> r : rows) {
                String key = str(col(r, "App")) + " [AppId " + asInt(col(r, "AppId")) + "]";
                String m = str(col(r, "ModuleDescription"));
                List<String> l = byApp.computeIfAbsent(key, k -> new ArrayList<>());
                if (!l.contains(m)) l.add(m);
            }
            List<String> groups = new ArrayList<>();
            for (Map.Entry<String, List<String>> e : byApp.entrySet()) {
                groups.add(e.getKey() + " (" + e.getValue().size() + " modules) -> "
                         + String.join(", ", e.getValue()));
            }
            d.put("appGroups", groups);
            if (!rows.isEmpty()) d.put("columnsReturned", new ArrayList<>(rows.get(0).keySet()));
        } catch (Exception e) {
            d.put("appGroups", Collections.singletonList("could not be read: " + e.getMessage()));
        }
        return d;
    }

    /**
     * frmMenue.DynamicallyGenerateCardsForModule() (:201-244) - one card per module under the
     * application clicked, captioned by ModuleDescription, numbered by its screen count.
     */
    /**
     * frmModules.DynamicallyGenerateCardsForApp() (:69-98) — the desktop's LEVEL 1: one card per
     * application this user may view, captioned by the App name and numbered by how many screens
     * it contains.
     *
     * This level was missing from the port entirely. Without it there is no way to reach an
     * application that has no hand-written landing page of its own — Admin Panel and System
     * Utilities among them — because every module page was written one at a time by hand.
     */
    public List<Map<String, Object>> getAppCards() {
        List<Map<String, Object>> cards = new ArrayList<>();
        Map<String, Map<String, Object>> byApp = new LinkedHashMap<>();
        for (Map<String, Object> r : rights()) {
            int appId = asInt(col(r, "AppId"));
            String key = String.valueOf(appId);
            Map<String, Object> a = byApp.get(key);
            if (a == null) {
                a = new LinkedHashMap<>();
                a.put("appId", appId);
                a.put("title", str(col(r, "App")));
                a.put("count", 0);
                a.put("built", 0);
                a.put("modules", new java.util.LinkedHashSet<Integer>());
                byApp.put(key, a);
                cards.add(a);
            }
            a.put("count", asInt(a.get("count")) + 1);
            if (routeFor(r) != null) a.put("built", asInt(a.get("built")) + 1);
            @SuppressWarnings("unchecked")
            java.util.Set<Integer> mods = (java.util.Set<Integer>) a.get("modules");
            mods.add(asInt(moduleIdOf(r)));
        }
        for (Map<String, Object> a : cards) {
            @SuppressWarnings("unchecked")
            java.util.Set<Integer> mods = (java.util.Set<Integer>) a.remove("modules");
            a.put("moduleCount", mods.size());
        }
        return cards;
    }

    public List<Map<String, Object>> getModuleCards(int appId) {
        List<Map<String, Object>> cards = new ArrayList<>();
        Map<String, Map<String, Object>> byModule = new LinkedHashMap<>();
        for (Map<String, Object> r : rights()) {
            if (asInt(col(r, "AppId")) != appId) continue;
            String moduleId = String.valueOf(asInt(moduleIdOf(r)));
            Map<String, Object> m = byModule.get(moduleId);
            if (m == null) {
                m = new LinkedHashMap<>();
                m.put("moduleId", asInt(moduleIdOf(r)));
                m.put("moduleTypeId", asInt(col(r, "ModuleTypeId")));
                m.put("title", str(col(r, "ModuleDescription")));
                m.put("count", 0);
                m.put("built", 0);
                byModule.put(moduleId, m);
                cards.add(m);
            }
            m.put("count", asInt(m.get("count")) + 1);
            if (routeFor(r) != null) m.put("built", asInt(m.get("built")) + 1);
        }
        return cards;
    }

    /**
     * frmMenue.DynamicallyGenerateCards(ModId) (:84-113) - one card per screen of one module,
     * captioned by ScreenAlias. The module is checked against the application too, so a
     * hand-edited id cannot reach screens the user was not allocated.
     */
    public Map<String, Object> getScreenCards(int appId, int moduleId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> screens = new ArrayList<>();
        String moduleTitle = "";
        for (Map<String, Object> r : rights()) {
            if (asInt(col(r, "AppId")) != appId) continue;
            if (asInt(moduleIdOf(r)) != moduleId) continue;
            moduleTitle = str(col(r, "ModuleDescription"));
            screens.add(screenCard(r));
        }
        out.put("moduleTitle", moduleTitle);
        out.put("screens", screens);
        return out;
    }

    /** The procedure spells it ModuleID; the rebuilt join aliases it ModuleId. Accept either. */
    private static Object moduleIdOf(Map<String, Object> r) {
        Object v = col(r, "ModuleID");
        return v != null ? v : col(r, "ModuleId");
    }

    /** One screen card, with its web route resolved or the placeholder that names its desktop form. */
    private Map<String, Object> screenCard(Map<String, Object> r) {
        String targetUrl = str(col(r, "TargetUrl"));
        String cls = targetUrl.contains(".")
                ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1) : targetUrl;
        String alias = str(col(r, "ScreenAlias"));
        String route = routeFor(r);

        Map<String, Object> sc = new LinkedHashMap<>();
        sc.put("screenId", col(r, "ScreenID"));
        sc.put("screenName", str(col(r, "ScreenName")));
        sc.put("title", alias.isEmpty() ? str(col(r, "ScreenName")) : alias);
        sc.put("targetUrl", targetUrl);
        sc.put("built", route != null);
        sc.put("route", route != null ? route : "/dashboard/screen?name=" + urlEncode(cls));
        sc.put("desktopForm", DESKTOP_FORMS.getOrDefault(cls, cls + ".cs"));
        return sc;
    }

    /** The explicit map wins; otherwise the registered-route index, which never guesses a URL. */
    private String routeFor(Map<String, Object> r) {
        String targetUrl = str(col(r, "TargetUrl"));
        if (targetUrl.isEmpty()) return null;
        String cls = targetUrl.contains(".")
                ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1) : targetUrl;
        String explicit = WEB_ROUTES.get(cls);
        if (explicit != null) return explicit;
        return screenRouteIndex.routeFor(str(col(r, "ScreenName")), targetUrl);
    }

    /** Lower-case, letters and digits only - "DashBoard", "Dash Board" and "dashboard" all match. */
    private static String squash(String s) {
        if (s == null) return "";
        StringBuilder b = new StringBuilder();
        for (int i = 0; i < s.length(); i++) {
            char c = s.charAt(i);
            if (Character.isLetterOrDigit(c)) b.append(Character.toLowerCase(c));
        }
        return b.toString();
    }

    /** Message from the last getModuleScreens call that threw, or null if it did not. */
    public String getLastError() { return lastError; }

    /** usp_getModulesByCompanyId - the module list the desktop's own hub is built from. */
    public List<Map<String, Object>> getModules() {
        try {
            return jdbcTemplate.queryForList(SQL_MODULES,
                    currentUserContext.currentCompanyId(), currentUserContext.currentUserId());
        } catch (Exception e) {
            LOG.error("Module list failed", e);
            return Collections.emptyList();
        }
    }

    /** The desktop form a not-yet-ported screen belongs to, for the placeholder page. */
    public String desktopFormFor(String className) {
        if (className == null || className.isEmpty()) return null;
        return DESKTOP_FORMS.getOrDefault(className, className + ".cs");
    }

    // ---------------------------------------------------------------- helpers

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static boolean truthy(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static String urlEncode(String s) {
        try { return java.net.URLEncoder.encode(s, "UTF-8"); }
        catch (Exception e) { return s; }
    }
}
