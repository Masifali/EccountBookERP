package com.mst.services;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.*;
import com.mst.security.SidebarScreenCatalog;
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
            Map.entry(171, "inventory/item_list_report"), Map.entry(301, "inventory/stock_report"),
            Map.entry(580, "inventory/stock_transactions"));

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
                new InventoryItemListController(mock(InventoryItemListService.class)),
                new InventoryTransactionsController(mock(InventoryTransactionsService.class))).build();
    }

    @Test void sidebarAndInventoryAliasesUseTheRightsDrivenApplicationDashboard() throws Exception {
        for (String path : List.of("/app/Inventory", "/app/inventory")) {
            mvc.perform(get(path)).andExpect(status().isOk()).andExpect(view().name("dashboard"))
                    .andExpect(model().attribute("appName", "Inventory"));
        }
        mvc.perform(get("/app/Inventory").param("module", "19"))
                .andExpect(status().isOk()).andExpect(view().name("dashboard"))
                .andExpect(model().attribute("selectedModuleId", 19));
        for (String path : List.of("/inventory", "/inventory/", "/inventory/dashboard")) {
            mvc.perform(get(path)).andExpect(status().isOk()).andExpect(view().name("forward:/app/Inventory"));
        }
    }

    @Test void seededInventoryModulesShowTheirRightsDrivenScreenLists() throws Exception {
        var modules = menus.getModuleCards(5);
        assertEquals(3, modules.size());
        for (var module : modules) {
            int moduleId = ((Number) module.get("moduleId")).intValue();
            var result = mvc.perform(get("/app/Inventory").param("module", Integer.toString(moduleId)))
                    .andExpect(status().isOk()).andExpect(view().name("dashboard"))
                    .andExpect(model().attribute("selectedModuleId", moduleId)).andReturn();
            var screens = (List<?>) Objects.requireNonNull(result.getModelAndView()).getModel().get("screens");
            assertEquals(((Number) module.get("count")).intValue(), screens.size());
        }
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
        assertEquals("/inventory/reports/item-list", menus.webRouteFor(171, "", ""));
        var itemList = menus.getScreenCards(5, 8).get("screens");
        assertEquals(1, ((List<?>) itemList).size());
        @SuppressWarnings("unchecked") Map<String, Object> itemListCard = (Map<String, Object>) ((List<?>) itemList).get(0);
        assertEquals(171, itemListCard.get("screenId"));
        assertEquals("Item List", itemListCard.get("title"));
        assertEquals("/inventory/reports/item-list", itemListCard.get("route"));
        var catalogEntry = SidebarScreenCatalog.all().stream()
                .filter(entry -> "INVENTORY_ITEM_LIST".equals(entry.authorityCode)).findFirst().orElseThrow();
        assertEquals("Item List", catalogEntry.screenName);
        assertEquals("/inventory/reports/item-list", catalogEntry.targetUrl);
        assertEquals(171, catalogEntry.realScreenDefinitionId);
        assertEquals(8, catalogEntry.realModuleId);
        assertEquals("/stocks/stock-evaluation-vehicle-wise", menus.webRouteFor(292, "", ""));
        assertEquals("/inventory/stock-transactions-with-value", menus.webRouteFor(293, "", ""));
        assertNull(menus.webRouteFor(297, "", ""));
        assertEquals("/inventory/stock-report-with-values", menus.webRouteFor(299, "", ""));
        // 580 exists, but is not part of this user's seeded menu. A route must not allocate it.
        assertEquals(20, menus.getAppCards().get(0).get("count"));
        assertTrue(((List<?>) menus.getScreenCards(5, 93).get("screens")).isEmpty());
    }

    @Test void backLinksReopenEachInventoryCategoryWithoutAnApplicationLoop() throws Exception {
        var navigation = new ErpNavigationService(menus);
        for (int moduleId : List.of(4, 8, 19)) {
            mvc.perform(get("/app/Inventory").param("module", Integer.toString(moduleId)))
                    .andExpect(status().isOk()).andExpect(view().name("dashboard"))
                    .andExpect(model().attribute("selectedModuleId", moduleId));
            mvc.perform(get("/inventory/dashboard").param("module", Integer.toString(moduleId)))
                    .andExpect(status().isOk()).andExpect(view().name("forward:/app/Inventory"));
        }
        assertEquals("/apps", navigation.build(rights, "/inventory/dashboard", Map.of(), "Inventory", null).get("parentRoute"));
        assertEquals("/app/Inventory", navigation.build(rights, "/inventory/dashboard",
                Map.of("module", new String[]{"19"}), "Inventory", 19).get("parentRoute"));
        mvc.perform(get("/app/Inventory")).andExpect(model().attributeDoesNotExist("selectedModuleId"));
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

    @Test void actualTemplateRendersSeededModulesAndSelectedScreenCards() throws Exception {
        var result = mvc.perform(get("/app/Inventory").param("module", "19")).andReturn().getModelAndView();
        assertNotNull(result);
        var servlet = new MockServletContext();
        var request = new MockHttpServletRequest(servlet, "GET", "/app/Inventory");
        request.setParameter("module", "19");
        request.setUserPrincipal(() -> "Navigation preview");
        try (var application = new StaticWebApplicationContext()) {
            application.setServletContext(servlet);
            application.getBeanFactory().registerSingleton("erpNavigationService", new ErpNavigationService(menus));
            var userRights = mock(DesktopUserRightsService.class);
            when(userRights.canManage()).thenReturn(false);
            application.getBeanFactory().registerSingleton("desktopUserRightsService", userRights);
            var gearMenu = mock(GearMenuController.class);
            when(gearMenu.adminItemsForView()).thenReturn(List.of());
            application.getBeanFactory().registerSingleton("gearMenuController", gearMenu);
            application.refresh();
            var context = new WebContext(request, new MockHttpServletResponse(), servlet);
            context.setVariables(result.getModel());
            context.setVariable(ThymeleafEvaluationContext.THYMELEAF_EVALUATION_CONTEXT_CONTEXT_VARIABLE_NAME,
                    new ThymeleafEvaluationContext(application, null));
            var resolver = new ClassLoaderTemplateResolver();
            resolver.setPrefix("templates/"); resolver.setSuffix(".html"); resolver.setTemplateMode("HTML");
            var engine = new SpringTemplateEngine(); engine.setTemplateResolver(resolver);
            String html = engine.process(result.getViewName(), context);
            assertEquals(3, Pattern.compile("class=\"dbm-card").matcher(html).results().count());
            assertEquals(8, Pattern.compile("class=\"dbs-card").matcher(html).results().count());
            assertTrue(html.contains("/inventory/stock-transactions-with-value"));
            Files.writeString(Path.of("target/inventory-navigation-preview.html"), html);
        }
    }
}
