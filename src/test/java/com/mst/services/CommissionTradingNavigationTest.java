package com.mst.services;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.AppMenuController;
import com.mst.controllers.GearMenuController;
import com.mst.controllers.cmagt.CmagtModuleViewController;
import com.mst.controllers.cmagt.SaleOrderCmagtController;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.mock.web.MockServletContext;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.ui.ExtendedModelMap;
import org.springframework.web.context.support.StaticWebApplicationContext;
import org.thymeleaf.context.WebContext;
import org.thymeleaf.spring5.SpringTemplateEngine;
import org.thymeleaf.spring5.expression.ThymeleafEvaluationContext;
import org.thymeleaf.templateresolver.ClassLoaderTemplateResolver;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class CommissionTradingNavigationTest {
    private final DashboardModuleService menus = spy(new DashboardModuleService());
    private final ScreenRouteIndex index = mock(ScreenRouteIndex.class);
    private final List<Map<String, Object>> rights = new ArrayList<>();
    private static final Map<Integer, String> VIEWS = Map.ofEntries(
            Map.entry(971, "cmagt/buyer_inquiry_booking"), Map.entry(987, "cmagt/supplier_offer_cmagt"),
            Map.entry(972, "cmagt/purchase_order_cmagt"), Map.entry(973, "cmagt/sale_order_cmagt"),
            Map.entry(976, "cmagt/grn_loading_challan_cmagt"), Map.entry(977, "cmagt/goods_dispatching_note_cmagt"),
            Map.entry(979, "cmagt/trade_bill_against_gdn"), Map.entry(981, "cmagt/reports/agent_trade_bill_register"),
            Map.entry(982, "cmagt/reports/gdn_buyer_dispatch_report"), Map.entry(983, "cmagt/reports/grn_supplier_loading_report"),
            Map.entry(984, "cmagt/reports/purchase_order_report"), Map.entry(985, "cmagt/reports/sale_order_report"));

    @BeforeEach void loadRealSeededScreenIdsAndNames() throws Exception {
        JsonNode seed = new ObjectMapper().readTree(Path.of("migration/user-rights/reconciliation-input.json").toFile());
        Map<Integer, String> names = new LinkedHashMap<>();
        for (JsonNode grant : seed.get("grants")) names.putIfAbsent(grant.path("ScreenId").asInt(), grant.path("ScreenName").asText());
        for (JsonNode screen : seed.get("standardScreens")) {
            int module = screen.path("ModuleId").asInt();
            if (module != 56 && module != 57) continue;
            int id = screen.path("ScreenId").asInt();
            Map<String, Object> row = new LinkedHashMap<>();
            row.put("ScreenID", id);
            row.put("ScreenName", names.get(id));
            row.put("ScreenAlias", screen.path("ScreenAlias").asText());
            row.put("ModuleID", module);
            row.put("ModuleDescription", screen.path("ModuleDescription").asText());
            row.put("AppId", 1);
            row.put("App", "Commission Trading");
            row.put("Value", true);
            rights.add(row);
        }
        assertEquals(VIEWS.keySet(), rights.stream().map(r -> (Integer) r.get("ScreenID")).collect(java.util.stream.Collectors.toSet()));
        ReflectionTestUtils.setField(menus, "screenRouteIndex", index);
        doReturn(rights).when(menus).viewRights();
    }

    @Test void seededMenuCountsAllSevenTransactionsAndFiveReports() {
        var cards = menus.getModuleCards(1);
        assertEquals(2, cards.size());
        for (var card : cards) {
            int expected = card.get("moduleId").equals(56) ? 7 : 5;
            assertEquals(expected, card.get("count"));
            assertEquals(expected, card.get("built"));
        }
        assertEquals(12, menus.getAppCards().get(0).get("built"));
        verifyNoInteractions(index);
    }

    @Test void everySeededScreenOpensItsOwnExistingPage() throws Exception {
        var mvc = MockMvcBuilders.standaloneSetup(new CmagtModuleViewController(), new SaleOrderCmagtController()).build();
        Set<String> visited = new java.util.HashSet<>();
        for (var row : rights) {
            int id = (Integer) row.get("ScreenID");
            String route = menus.webRouteFor(id, (String) row.get("ScreenName"), "");
            assertNotNull(route, row.get("ScreenAlias").toString());
            assertTrue(visited.add(route), "Two different screens must not point to one page");
            mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name(VIEWS.get(id)));
            assertTrue(Files.exists(Path.of("src/main/resources/templates/" + VIEWS.get(id) + ".html")));
        }
        assertEquals(12, visited.size());
    }

    @Test void newSidebarEntryGetsTheSameCompleteMenu() {
        var controller = new AppMenuController();
        ReflectionTestUtils.setField(controller, "dashboardModuleService", menus);
        var model = new ExtendedModelMap();
        assertEquals("dashboard", controller.app("Commission Trading", null, model));
        assertEquals(12, model.get("screenCount"));
        assertEquals(12, model.get("builtCount"));
        assertEquals(2, model.get("cardCount"));
        assertEquals("/app/Commission%20Trading", model.get("appRoute"));
    }

    @Test void builtScreenCountsStillRespectTheUsersAllocatedMenu() {
        doReturn(List.of(rights.get(0))).when(menus).viewRights();
        var card = menus.getModuleCards(1).get(0);
        assertEquals(1, card.get("count"));
        assertEquals(1, card.get("built"));
        assertEquals(1, menus.getAppCards().get(0).get("count"));
    }

    @Test void renderedApplicationShowsCompleteCountsAndCorrectBreadcrumb() throws Exception {
        var controller = new AppMenuController();
        ReflectionTestUtils.setField(controller, "dashboardModuleService", menus);
        var model = new ExtendedModelMap();
        controller.app("Commission Trading", null, model);
        var servlet = new MockServletContext();
        var request = new MockHttpServletRequest(servlet, "GET", "/app/Commission%20Trading");
        request.setUserPrincipal(() -> "Navigation preview");
        try (var application = new StaticWebApplicationContext()) {
            application.setServletContext(servlet);
            application.getBeanFactory().registerSingleton("erpNavigationService", new ErpNavigationService(menus));
            var gear = mock(GearMenuController.class);
            when(gear.adminItemsForView()).thenReturn(List.of());
            when(gear.utilityItemsForView()).thenReturn(List.of());
            application.getBeanFactory().registerSingleton("gearMenuController", gear);
            var desktopUserRights = mock(DesktopUserRightsService.class);
            when(desktopUserRights.canManage()).thenReturn(false);
            application.getBeanFactory().registerSingleton("desktopUserRightsService", desktopUserRights);
            application.refresh();
            var context = new WebContext(request, new MockHttpServletResponse(), servlet);
            context.setVariables(model);
            context.setVariable(ThymeleafEvaluationContext.THYMELEAF_EVALUATION_CONTEXT_CONTEXT_VARIABLE_NAME,
                    new ThymeleafEvaluationContext(application, null));
            var resolver = new ClassLoaderTemplateResolver();
            resolver.setPrefix("templates/"); resolver.setSuffix(".html"); resolver.setTemplateMode("HTML");
            var engine = new SpringTemplateEngine();
            engine.setTemplateResolver(resolver);
            String html = engine.process("dashboard", context);
            String compact = html.replaceAll("\\s+", " ");
            assertTrue(compact.contains("<b>7</b> of <span>7</span> built"));
            assertTrue(compact.contains("<b>5</b> of <span>5</span> built"));
            assertTrue(html.contains("Screens &gt; Commission Trading"));
            Files.writeString(Path.of("target/commission-navigation-preview.html"), html);
        }
    }

    @Test void eachScreenReturnsToItsCorrectSeededParentModule() {
        var navigation = new ErpNavigationService(menus);
        for (var row : rights) {
            String route = menus.webRouteFor((Integer) row.get("ScreenID"), (String) row.get("ScreenName"), "");
            var request = new MockHttpServletRequest("GET", route);
            request.setUserPrincipal(() -> "menu-test");
            var nav = navigation.forRequest(request, null, null);
            assertEquals("/app/Commission%20Trading?module=" + row.get("ModuleID"), nav.get("parentRoute"));
        }
    }
}
