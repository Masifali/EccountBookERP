package com.mst.services;

import com.mst.security.SidebarScreenCatalog;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.authentication.AnonymousAuthenticationToken;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpServletRequest;
import java.net.URI;
import java.net.URLDecoder;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.security.Principal;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/** Shared application navigation from the desktop's seeded, user-filtered menu rows. */
@Service
public class ErpNavigationService {
    private static final Logger LOG = LoggerFactory.getLogger(ErpNavigationService.class);
    private static final String VIEW_RIGHTS_SESSION_KEY = ErpNavigationService.class.getName() + ".viewRights";
    // Existing Inventory dashboard links without a corresponding seeded desktop screen.
    // These identify the navigation parent only; they never mark a desktop report as built.
    private static final Map<String, String> INVENTORY_DASHBOARD_STOCK_PAGES = Map.ofEntries(
            Map.entry("/stocks/stock_register", "Stock Register Report"),
            Map.entry("/stocks/stock-register", "Stock Register Report"),
            Map.entry("/stocks/warehouse_summary", "Warehouse Stock Summary"),
            Map.entry("/stocks/warehouse-summary", "Warehouse Stock Summary"),
            Map.entry("/stocks/stock_as_on_date", "Item Stock As On Date"),
            Map.entry("/stocks/stock-as-on-date", "Item Stock As On Date"),
            Map.entry("/stocks/lot_wise_stock", "Lot Wise Stock Report"),
            Map.entry("/stocks/lot-wise-stock", "Lot Wise Stock Report"),
            Map.entry("/stocks/brand_wise_stock", "Brand Wise Stock Report"),
            Map.entry("/stocks/brand-wise-stock", "Brand Wise Stock Report"));
    private final DashboardModuleService menus;

    public ErpNavigationService(DashboardModuleService menus) {
        this.menus = menus;
    }

    /** Called only by the navigation fragment, never by report exports or JSON endpoints. */
    public Map<String, Object> forRequest(HttpServletRequest request, String appName, Integer selectedModuleId) {
        Principal user = request.getUserPrincipal();
        if (user == null || user instanceof AnonymousAuthenticationToken) return null;
        String path = request.getRequestURI().substring(request.getContextPath().length());
        List<Map<String, Object>> rows;
        boolean unavailable = false;
        try {
            rows = viewRightsForSession(request, user.getName());
        } catch (RuntimeException exception) {
            LOG.warn("Could not load the navigation menu", exception);
            rows = List.of();
            unavailable = true;
        }
        Map<String, Object> navigation = build(rows, path, request.getParameterMap(), appName, selectedModuleId);
        navigation.put("username", user.getName());
        navigation.put("unavailable", unavailable);
        addQuickLinks(request, navigation, user.getName());
        return navigation;
    }

    /**
     * DashboardNew keeps ScreenViewReights for the lifetime of the signed-in desktop session.
     * Keep the exact stored-procedure result in the web session too, so every page render does not
     * execute the same potentially slow rights query again. A completed LoginContext changes the
     * cache key when the user switches company or application.
     */
    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> viewRightsForSession(HttpServletRequest request, String username) {
        com.mst.security.LoginContext context = com.mst.security.LoginContext.of(request, username);
        String scope = context != null && context.isComplete()
                ? username + ":" + context.getCompanyId() + ":" + context.getAppId()
                : username + ":default";
        String key = VIEW_RIGHTS_SESSION_KEY + ":" + scope;
        var session = request.getSession();
        synchronized (session) {
            Object cached = session.getAttribute(key);
            if (cached instanceof List<?>) return (List<Map<String, Object>>) cached;
            List<Map<String, Object>> rows = menus.viewRights();
            session.setAttribute(key, rows);
            return rows;
        }
    }

    @SuppressWarnings("unchecked")
    private void addQuickLinks(HttpServletRequest request, Map<String, Object> navigation, String username) {
        Map<Integer, Map<String, Object>> screens = (Map<Integer, Map<String, Object>>) navigation.remove("screenLinks");
        var favorites = menus.favoriteScreenNames();
        navigation.put("favoriteScreens", screens.values().stream()
                .filter(screen -> favorites.contains(string(screen.get("name")).toLowerCase(Locale.ROOT))).toList());
        // Keep only screen identifiers in this signed-in browser session. Re-resolve every
        // title/route against current view rights; removed rights never survive in Recent.
        var session = request.getSession();
        String key = "erp.recentScreens." + username;
        synchronized (session) {
            List<Integer> recent = new ArrayList<>();
            if (session.getAttribute(key) instanceof List<?> previous) {
                for (Object id : previous) if (id instanceof Integer && screens.containsKey(id)) recent.add((Integer) id);
            }
            Integer current = integer(navigation.get("currentScreenId"));
            if (current != null && screens.containsKey(current)) { recent.remove(current); recent.add(0, current); }
            if (recent.size() > 20) recent = new ArrayList<>(recent.subList(0, 20));
            session.setAttribute(key, recent);
            navigation.put("recentScreens", recent.stream().map(screens::get).toList());
        }
    }

    Map<String, Object> build(List<Map<String, Object>> rows, String path, Map<String, String[]> query,
                              String appName, Integer selectedModuleId) {
        Map<Integer, Map<String, Object>> apps = new LinkedHashMap<>();
        Map<Integer, Map<String, Object>> screens = new LinkedHashMap<>();
        Integer currentScreenId = null;
        int currentModuleType = 0;
        Map<String, Object> activeApp = null, activeModule = null;
        String currentTitle = "Main pages";
        int bestMatch = -1;
        for (Map<String, Object> row : rows) {
            if (!granted(value(row, "Value"))) continue;
            Integer appId = integer(value(row, "AppId")), moduleId = integer(value(row, "ModuleID"));
            if (appId == null || moduleId == null) continue;
            String title = string(value(row, "App"));
            if (title.isBlank()) continue;
            Map<String, Object> app = apps.computeIfAbsent(appId, id -> {
                Map<String, Object> item = new LinkedHashMap<>();
                item.put("id", id);
                item.put("title", title);
                item.put("route", "/app/" + encode(title));
                item.put("icon", icon(title));
                item.put("active", false);
                item.put("modules", new LinkedHashMap<Integer, Map<String, Object>>());
                return item;
            });
            Map<Integer, Map<String, Object>> modules = moduleMap(app);
            Map<String, Object> module = modules.computeIfAbsent(moduleId, id -> {
                Map<String, Object> item = new LinkedHashMap<>();
                item.put("id", id);
                item.put("title", string(value(row, "ModuleDescription")));
                item.put("route", app.get("route") + "?module=" + id);
                item.put("active", false);
                return item;
            });
            Integer screenId = integer(value(row, "ScreenID"));
            String target = string(value(row, "TargetUrl"));
            String route = menus.webRouteFor(screenId, string(value(row, "ScreenName")), target);
            if (route == null && !target.isBlank()) {
                route = "/dashboard/screen?name=" + encode(target.substring(target.lastIndexOf('.') + 1));
            }
            String screenTitle = string(value(row, "ScreenAlias"));
            if (screenTitle.isBlank()) screenTitle = string(value(row, "ScreenName"));
            if (screenId != null && route != null && route.startsWith("/") && !route.startsWith("//")) {
                screens.putIfAbsent(screenId, Map.of("id", screenId, "title", screenTitle, "route", route,
                        "name", string(value(row, "ScreenName"))));
            }
            int match = match(route, path, query);
            if (screenId != null && match < 0) {
                for (SidebarScreenCatalog.Entry entry : SidebarScreenCatalog.all()) {
                    if (screenId.equals(entry.realScreenDefinitionId)) match = Math.max(match, match(entry.targetUrl, path, query));
                }
            }
            if (match > bestMatch) {
                bestMatch = match;
                activeApp = app;
                activeModule = module;
                currentTitle = string(value(row, "ScreenAlias"));
                if (currentTitle.isBlank()) currentTitle = string(value(row, "ScreenName"));
                currentScreenId = screenId;
                Integer type = integer(value(row, "ModuleTypeId"));
                currentModuleType = type == null ? 0 : type;
            }
        }

        // Menu pages can arrive through /app/name or one of the existing friendly entry points.
        String requestedApp = appName;
        if (requestedApp == null && path.startsWith("/app/")) requestedApp = decode(path.substring(5));
        if (requestedApp == null && (path.equals("/") || path.equals("/dashboard") || path.equals("/dashboard/module"))) requestedApp = "DashBoard";
        if (requestedApp != null) {
            for (Map<String, Object> app : apps.values()) {
                if (!normal(requestedApp).equals(normal(string(app.get("title"))))) continue;
                activeApp = app;
                activeModule = null;
                Integer moduleId = integer(first(query, "module"));
                if (moduleId == null && path.equals("/dashboard/module")) moduleId = integer(first(query, "id"));
                // An automatically opened single module still belongs to the application page.
                if (moduleId != null) activeModule = moduleMap(app).get(moduleId);
                if (selectedModuleId != null && moduleMap(app).containsKey(selectedModuleId)) {
                    moduleMap(app).get(selectedModuleId).put("active", true);
                }
                currentTitle = string((activeModule == null ? app : activeModule).get("title"));
                bestMatch = -1;
                break;
            }
        }
        // The existing Inventory dashboard also links to web-only stock pages. Return these
        // to its Stock Reports category when that module is present in this user's menu.
        if (activeApp == null && INVENTORY_DASHBOARD_STOCK_PAGES.containsKey(path)) {
            for (Map<String, Object> app : apps.values()) {
                if (!"inventory".equals(normal(string(app.get("title"))))) continue;
                activeApp = app;
                activeModule = moduleMap(app).get(19);
                currentTitle = INVENTORY_DASHBOARD_STOCK_PAGES.get(path);
                bestMatch = 0;
                break;
            }
        }
        // A screen with no exact registered alias can still return to its allowed application.
        if (activeApp == null && !path.equals("/apps")) {
            String prefix = path.split("/").length > 1 ? normal(path.split("/")[1]) : "";
            for (Map<String, Object> app : apps.values()) {
                if (!prefix.isEmpty() && normal(string(app.get("title"))).equals(prefix)) {
                    activeApp = app;
                    break;
                }
            }
        }

        String parentRoute = "/apps", parentTitle = "Main pages";
        if (activeApp != null) {
            activeApp.put("active", true);
            if (activeModule != null) {
                activeModule.put("active", true);
                Map<String, Object> parent = bestMatch >= 0 ? activeModule : activeApp;
                parentRoute = string(parent.get("route"));
                parentTitle = string(parent.get("title"));
            } else if (requestedApp == null) {
                parentRoute = string(activeApp.get("route"));
                parentTitle = string(activeApp.get("title"));
            }
        }
        if (path.equals("/apps")) { parentRoute = null; currentTitle = "Main pages"; }
        for (Map<String, Object> app : apps.values()) app.put("modules", new ArrayList<>(moduleMap(app).values()));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("apps", new ArrayList<>(apps.values()));
        out.put("parentRoute", parentRoute);
        out.put("parentTitle", parentTitle);
        out.put("currentTitle", currentTitle);
        out.put("screenLinks", screens);
        out.put("currentScreenId", currentScreenId);
        out.put("currentModuleType", currentModuleType);
        return out;
    }

    @SuppressWarnings("unchecked")
    private static Map<Integer, Map<String, Object>> moduleMap(Map<String, Object> app) {
        return (Map<Integer, Map<String, Object>>) app.get("modules");
    }

    /** Route query values distinguish screens sharing a URL, such as GRN and GDN. */
    private static int match(String route, String path, Map<String, String[]> query) {
        if (route == null || !route.startsWith("/") || route.startsWith("//")) return -1;
        URI uri;
        try { uri = URI.create(route); } catch (IllegalArgumentException invalidRoute) { return -1; }
        if (!uri.getPath().equals(path)) return -1;
        int specificity = 0;
        if (uri.getRawQuery() != null) {
            for (String part : uri.getRawQuery().split("&")) {
                String[] pair = part.split("=", 2);
                if (!decode(pair.length == 2 ? pair[1] : "").equals(first(query, decode(pair[0])))) return -1;
                specificity++;
            }
        }
        return specificity;
    }

    private static Object value(Map<String, Object> row, String name) {
        return row.entrySet().stream().filter(e -> e.getKey().equalsIgnoreCase(name)).map(Map.Entry::getValue).filter(java.util.Objects::nonNull).findFirst().orElse(null);
    }
    private static String first(Map<String, String[]> query, String name) { String[] values = query.get(name); return values == null || values.length == 0 ? null : values[0]; }
    private static String string(Object value) { return value == null ? "" : value.toString().trim(); }
    private static Integer integer(Object value) { try { return value == null ? null : Integer.valueOf(value.toString()); } catch (NumberFormatException ignored) { return null; } }
    private static boolean granted(Object value) { return "true".equalsIgnoreCase(string(value)) || "1".equals(string(value)); }
    private static String encode(String value) { return URLEncoder.encode(value, StandardCharsets.UTF_8).replace("+", "%20"); }
    private static String decode(String value) { return URLDecoder.decode(value, StandardCharsets.UTF_8); }
    private static String normal(String value) { return value.toLowerCase(Locale.ROOT).replaceAll("[^a-z0-9]", ""); }
    private static String icon(String title) {
        String name = normal(title);
        if (name.contains("account") || name.contains("bank")) return "fa-book";
        if (name.contains("purchase")) return "fa-shopping-cart";
        if (name.contains("sale")) return "fa-briefcase";
        if (name.contains("inventory") || name.contains("stock") || name.contains("store")) return "fa-cubes";
        if (name.contains("dashboard")) return "fa-bar-chart";
        if (name.contains("hrm")) return "fa-users";
        if (name.contains("production")) return "fa-industry";
        if (name.contains("export") || name.contains("import")) return "fa-globe";
        if (name.contains("logistic") || name.contains("weigh")) return "fa-truck";
        return "fa-th-large";
    }
}
