package com.mst.services;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.*;
import com.mst.serviceInterface.IBrandService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.*;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.web.context.support.StaticWebApplicationContext;
import org.thymeleaf.context.WebContext;
import org.thymeleaf.spring5.SpringTemplateEngine;
import org.thymeleaf.spring5.expression.ThymeleafEvaluationContext;
import org.thymeleaf.templateresolver.ClassLoaderTemplateResolver;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.*;
import java.util.regex.Pattern;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class InventoryNavigationTest {
    private final DashboardModuleService menus = spy(new DashboardModuleService());
    private final List<Map<String, Object>> rights = new ArrayList<>();
    private MockMvc mvc;
    private static final Map<Integer, String> VIEWS = Map.ofEntries(
            Map.entry(92, "stocks/opening_stock"), Map.entry(104, "inventory/item_min_max_rate"),
            Map.entry(105, "inventory/consumption_items"), Map.entry(106, "inventory/pos_item"),
            Map.entry(110, "inventory/lots"), Map.entry(111, "inventory/general_item"),
            Map.entry(112, "inventory/item_categories"), Map.entry(113, "inventory/item_types"),
            Map.entry(115, "inventory/item_uom_schedule"), Map.entry(116, "inventory/warehouses"),
            Map.entry(876, "inventory/brands"), Map.entry(287, "stocks/item_ledger"),
            Map.entry(288, "stocks/store_stock_report"), Map.entry(291, "stocks/transaction_vehicle_wise"),
            Map.entry(301, "inventory/stock_report"), Map.entry(580, "inventory/stock_transactions"));

    @BeforeEach void setup() throws Exception {
        JsonNode seed = new ObjectMapper().readTree(Path.of("migration/user-rights/reconciliation-input.json").toFile());
        Map<Integer, String> names = new HashMap<>();
        for (JsonNode grant : seed.get("grants")) names.putIfAbsent(grant.path("ScreenId").asInt(), grant.path("ScreenName").asText());
        for (JsonNode screen : seed.get("standardScreens")) {
            int module = screen.path("ModuleId").asInt();
            if (!Set.of(4, 8, 19).contains(module)) continue;
            int id = screen.path("ScreenId").asInt();
            var row = new LinkedHashMap<String, Object>();
            row.put("ScreenID", id); row.put("ScreenName", names.get(id));
            row.put("ScreenAlias", screen.path("ScreenAlias").asText());
            row.put("ModuleID", module); row.put("ModuleDescription", screen.path("ModuleDescription").asText());
            row.put("AppId", 5); row.put("App", "Inventory"); row.put("Value", true);
            rights.add(row);
        }
        assertEquals(20, rights.size());
        ReflectionTestUtils.setField(menus, "screenRouteIndex", mock(ScreenRouteIndex.class));
        doReturn(rights).when(menus).viewRights();
        var inventory = new InventoryModuleViewController();
        ReflectionTestUtils.setField(inventory, "brandService", mock(IBrandService.class));
        var apps = new AppMenuController();
        ReflectionTestUtils.setField(apps, "dashboardModuleService", menus);
        mvc = MockMvcBuilders.standaloneSetup(apps, inventory, new MainModulesController(),
                new DesktopInventoryItemViewController(mock(DesktopInventoryItemService.class)),
                new DesktopInventoryCategoryController(mock(DesktopInventoryCategoryService.class)),
                new ItemTypeApiController(mock(DesktopInventoryTypeService.class)),
                new InventoryOpeningController(mock(InventoryOpeningService.class)),
                new InventoryPosItemController(mock(InventoryPosItemService.class)),
                new InventoryUomScheduleController(mock(InventoryUomScheduleService.class)),
                new InventoryItemLedgerController(mock(InventoryItemLedgerService.class)),
                new InventoryVehicleTransactionsController(mock(InventoryVehicleTransactionsService.class)),
                new InventoryStoreStockController(mock(InventoryStoreStockService.class)),
                new InventoryStockReportController(mock(InventoryStockReportService.class)),
                new InventoryTransactionsController(mock(InventoryTransactionsService.class))).build();
    }

    @Test void sidebarEntryForwardsToTheExistingDashboardIncludingModuleRequests() throws Exception {
        for (String path : List.of("/app/Inventory", "/app/inventory", "/app/Inventory?module=19")) {
            mvc.perform(get(path)).andExpect(status().isOk()).andExpect(forwardedUrl("/inventory/dashboard"));
        }
        for (String path : List.of("/inventory", "/inventory/", "/inventory/dashboard")) {
            mvc.perform(get(path)).andExpect(status().isOk()).andExpect(view().name("inventory/inventory_dashboard"))
                    .andExpect(model().attribute("appName", "Inventory"));
        }
    }

    @Test void allExistingDashboardLinksStillOpenTheSamePages() throws Exception {
        String source = Files.readString(Path.of("src/main/resources/templates/inventory/inventory_dashboard.html"));
        var links = Pattern.compile("<a href=\"([^\"]+)\" class=\"module-tile\"").matcher(source);
        var paths = new HashSet<String>();
        while (links.find()) {
            String path = links.group(1);
            assertTrue(paths.add(path));
            var result = mvc.perform(get(path)).andExpect(status().isOk()).andReturn();
            String view = Objects.requireNonNull(result.getModelAndView()).getViewName();
            assertNotNull(view);
            assertTrue(Files.exists(Path.of("src/main/resources/templates/" + view + ".html")), path);
        }
        assertEquals(20, paths.size());
    }

    @Test void seededMappingsOpenTheirActualPagesAndUseTheirOwnParents() throws Exception {
        var navigation = new ErpNavigationService(menus);
        for (var entry : VIEWS.entrySet()) {
            String route = menus.webRouteFor(entry.getKey(), "", "");
            assertNotNull(route);
            mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name(entry.getValue()));
            var row = rights.stream().filter(r -> r.get("ScreenID").equals(entry.getKey())).findFirst();
            if (row.isPresent()) {
                assertEquals("/app/Inventory?module=" + row.get().get("ModuleID"),
                        navigation.build(rights, route, Map.of(), null, null).get("parentRoute"));
            }
        }
    }

    @Test void restoredMappingsDoNotInventRightsOrMapUnrelatedReports() {
        var definitions = menus.getModuleCards(5).stream().filter(c -> c.get("moduleId").equals(4)).findFirst().orElseThrow();
        assertEquals(11, definitions.get("built"));
        assertEquals(11, definitions.get("count"));
        for (int id : List.of(171, 292, 293, 297, 299)) assertNull(menus.webRouteFor(id, "", ""));
        // 580 exists, but is not part of this user's seeded menu. A route must not allocate it.
        assertEquals(20, menus.getAppCards().get(0).get("count"));
        assertTrue(((List<?>) menus.getScreenCards(5, 93).get("screens")).isEmpty());
    }

    @Test void backLinksReopenEachInventoryCategoryWithoutAnApplicationLoop() throws Exception {
        var navigation = new ErpNavigationService(menus);
        for (var category : Map.of(4, "def", 8, "invrpt", 93, "invrpt", 19, "stockrpt").entrySet()) {
            mvc.perform(get("/inventory/dashboard").param("module", category.getKey().toString()))
                    .andExpect(model().attribute("inventoryCategory", category.getValue()))
                    .andExpect(model().attribute("selectedModuleId", category.getKey()));
        }
        assertEquals("/apps", navigation.build(rights, "/inventory/dashboard", Map.of(), "Inventory", null).get("parentRoute"));
        assertEquals("/app/Inventory", navigation.build(rights, "/inventory/dashboard",
                Map.of("module", new String[]{"19"}), "Inventory", 19).get("parentRoute"));
        mvc.perform(get("/inventory/dashboard?module=999")).andExpect(model().attributeDoesNotExist("selectedModuleId"));
    }

    @Test void legacyStockLinksReturnToTheAllowedInventoryParent() {
        var navigation = new ErpNavigationService(menus);
        for (String name : List.of("stock_register", "warehouse_summary", "stock_as_on_date", "lot_wise_stock", "brand_wise_stock")) {
            for (String path : Set.of("/stocks/" + name, "/stocks/" + name.replace('_', '-'))) {
                assertEquals("/app/Inventory?module=19", navigation.build(rights, path, Map.of(), null, null).get("parentRoute"));
                assertEquals("/apps", navigation.build(List.of(), path, Map.of(), null, null).get("parentRoute"));
            }
        }
        var definitionsOnly = rights.stream().filter(r -> r.get("ModuleID").equals(4)).toList();
        assertEquals("/app/Inventory", navigation.build(definitionsOnly, "/stocks/stock_register", Map.of(), null, null).get("parentRoute"));
    }

    @Test void actualTemplateKeepsAllTwentyLinksAndInitializesTheSelectedCategory() throws Exception {
        var result = mvc.perform(get("/inventory/dashboard?module=19")).andReturn().getModelAndView();
        assertNotNull(result);
        var servlet = new MockServletContext();
        var request = new MockHttpServletRequest(servlet, "GET", "/inventory/dashboard");
        request.setParameter("module", "19");
        request.setUserPrincipal(() -> "Navigation preview");
        try (var application = new StaticWebApplicationContext()) {
            application.setServletContext(servlet);
            application.getBeanFactory().registerSingleton("erpNavigationService", new ErpNavigationService(menus));
            application.refresh();
            var context = new WebContext(request, new MockHttpServletResponse(), servlet);
            context.setVariables(result.getModel());
            context.setVariable(ThymeleafEvaluationContext.THYMELEAF_EVALUATION_CONTEXT_CONTEXT_VARIABLE_NAME,
                    new ThymeleafEvaluationContext(application, null));
            var resolver = new ClassLoaderTemplateResolver();
            resolver.setPrefix("templates/"); resolver.setSuffix(".html"); resolver.setTemplateMode("HTML");
            var engine = new SpringTemplateEngine(); engine.setTemplateResolver(resolver);
            String html = engine.process(result.getViewName(), context);
            assertEquals(20, Pattern.compile("class=\"module-tile\"").matcher(html).results().count());
            assertTrue(html.contains("var inventoryCategory = \"stockrpt\";"));
            assertTrue(html.contains("data-parent-url=\"/app/Inventory\""));
            Files.writeString(Path.of("target/inventory-navigation-preview.html"), html);
        }
    }
}
