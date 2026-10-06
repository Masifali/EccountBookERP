package com.mst.services;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.AppMenuController;
import com.mst.controllers.GearMenuController;
import com.mst.controllers.sale.SaleInvoiceReturnController;
import com.mst.controllers.sale.SaleModuleViewController;
import com.mst.controllers.sale.SaleOrderController;
import com.mst.repositories.SaleInvoiceReturnRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.sale.SaleOrderService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.mock.web.MockServletContext;
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
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class SaleNavigationTest {
    private final DashboardModuleService menus = spy(new DashboardModuleService());
    private final ScreenRouteIndex index = mock(ScreenRouteIndex.class);
    private final List<Map<String, Object>> rights = new ArrayList<>();
    private MockMvc mvc;
    private static final Map<Integer, String> VIEWS = Map.ofEntries(
            Map.entry(147, "sale/delivery_order"), Map.entry(148, "sale/driver_bio"),
            Map.entry(149, "sale/sale_order"), Map.entry(152, "sale/outward_gate_pass"),
            Map.entry(150, "sale/gdn"), Map.entry(146, "sale/gdn_direct"),
            Map.entry(867, "sale/gdn_purchase_return"), Map.entry(151, "sale/sale_invoice"),
            Map.entry(141, "sale/sale_invoice_gdn_no_wb"), Map.entry(154, "sale/sale_invoice_direct"),
            Map.entry(153, "sale/sale_invoice_return"), Map.entry(856, "sale/reports/gatepass_vehicle_time_analysis"),
            Map.entry(482, "sale/reports/sale_order_report"), Map.entry(484, "sale/reports/outward_gate_pass_report"),
            Map.entry(483, "sale/reports/gdn_report"), Map.entry(481, "sale/reports/sale_invoice_report_with_activities"));

    @BeforeEach void loadSeededScreensAndRealPageControllers() throws Exception {
        JsonNode seed = new ObjectMapper().readTree(Path.of("migration/user-rights/reconciliation-input.json").toFile());
        Map<Integer, String> names = new LinkedHashMap<>();
        for (JsonNode grant : seed.get("grants")) {
            names.putIfAbsent(grant.path("ScreenId").asInt(), grant.path("ScreenName").asText());
        }
        for (JsonNode screen : seed.get("standardScreens")) {
            int module = screen.path("ModuleId").asInt();
            if (module != 6 && module != 53) continue;
            int id = screen.path("ScreenId").asInt();
            rights.add(Map.of("ScreenID", id, "ScreenName", names.get(id),
                    "ScreenAlias", screen.path("ScreenAlias").asText(), "ModuleID", module,
                    "ModuleDescription", screen.path("ModuleDescription").asText(),
                    // AppModules.AppId=6; the grant export's AppId=1 identifies DesktopApp instead.
                    "AppId", 6, "App", "Sale", "Value", true));
        }
        assertEquals(VIEWS.keySet(), rights.stream().map(r -> (Integer) r.get("ScreenID")).collect(Collectors.toSet()));
        ReflectionTestUtils.setField(menus, "screenRouteIndex", index);
        doReturn(rights).when(menus).viewRights();

        var app = new AppMenuController();
        ReflectionTestUtils.setField(app, "dashboardModuleService", menus);
        var pages = new SaleModuleViewController();
        ReflectionTestUtils.setField(pages, "purchaseService", mock(PurchaseService.class));
        var order = new SaleOrderController();
        ReflectionTestUtils.setField(order, "saleOrderService", mock(SaleOrderService.class));
        ReflectionTestUtils.setField(order, "currentUserContext", mock(CurrentUserContext.class));
        mvc = MockMvcBuilders.standaloneSetup(app, pages, order,
                new SaleInvoiceReturnController(mock(SaleInvoiceReturnService.class), mock(SaleInvoiceReturnRepository.class))).build();
    }

    @Test void mainPageAndBothModulesCountEveryExistingSaleScreen() throws Exception {
        var card = menus.getAppCards().get(0);
        assertEquals("Sale", card.get("title"));
        assertEquals(16, card.get("count"));
        assertEquals(16, card.get("built"));
        assertEquals(2, card.get("moduleCount"));
        for (var module : menus.getModuleCards(6)) {
            int expected = module.get("moduleId").equals(6) ? 11 : 5;
            assertEquals(expected, module.get("count"));
            assertEquals(expected, module.get("built"));
        }
        mvc.perform(get("/app/Sale")).andExpect(status().isOk()).andExpect(view().name("dashboard"))
                .andExpect(model().attribute("builtCount", 16)).andExpect(model().attribute("screenCount", 16));
        verifyNoInteractions(index);
    }

    @Test void seededModulesLinkToExactlyTheExistingSaleMenuPages() throws Exception {
        for (int module : List.of(6, 53)) {
            String template = module == 6 ? "customer_sales_menu" : "sales_reports_menu";
            String html = Files.readString(Path.of("src/main/resources/templates/sale/" + template + ".html"));
            Set<String> existing = Pattern.compile("class=\"screen\" href=\"([^\"]+)\"").matcher(html)
                    .results().map(m -> m.group(1)).collect(Collectors.toSet());
            @SuppressWarnings("unchecked")
            var cards = (List<Map<String, Object>>) menus.getScreenCards(6, module).get("screens");
            assertEquals(module == 6 ? 11 : 5, existing.size());
            assertEquals(existing, cards.stream().map(c -> c.get("route")).collect(Collectors.toSet()));
            assertTrue(cards.stream().allMatch(c -> Boolean.TRUE.equals(c.get("built"))));
        }
    }

    @Test void allSixteenLinksReachTheirDistinctExistingMvcViews() throws Exception {
        var visited = new HashSet<String>();
        for (var row : rights) {
            int id = (Integer) row.get("ScreenID");
            String route = route(row);
            assertNotNull(route, row.get("ScreenAlias").toString());
            assertTrue(visited.add(route), "Different screens must retain distinct pages");
            mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name(VIEWS.get(id)));
            assertTrue(Files.exists(Path.of("src/main/resources/templates/" + VIEWS.get(id) + ".html")));
        }
        assertEquals(16, visited.size());
    }

    @Test void everyChildReturnsToItsOwnSaleModule() {
        var navigation = new ErpNavigationService(menus);
        for (var row : rights) {
            var request = new MockHttpServletRequest("GET", route(row));
            request.setUserPrincipal(() -> "navigation-test");
            assertEquals("/app/Sale?module=" + row.get("ModuleID"),
                    navigation.forRequest(request, null, null).get("parentRoute"));
        }
    }

    @Test void menuRenderingShowsElevenAndFiveBuiltAndAllScreenLinks() throws Exception {
        var servlet = new MockServletContext();
        try (var application = new StaticWebApplicationContext()) {
            application.setServletContext(servlet);
            application.getBeanFactory().registerSingleton("erpNavigationService", new ErpNavigationService(menus));
            application.getBeanFactory().registerSingleton("gearMenuController", mock(GearMenuController.class));
            application.getBeanFactory().registerSingleton("desktopUserRightsService", mock(DesktopUserRightsService.class));
            application.refresh();
            var resolver = new ClassLoaderTemplateResolver();
            resolver.setPrefix("templates/"); resolver.setSuffix(".html"); resolver.setTemplateMode("HTML");
            var engine = new SpringTemplateEngine(); engine.setTemplateResolver(resolver);
            for (int module : List.of(6, 53)) {
                var result = mvc.perform(get("/app/Sale").param("module", String.valueOf(module)))
                        .andExpect(status().isOk()).andReturn().getModelAndView();
                assertNotNull(result);
                var request = new MockHttpServletRequest(servlet, "GET", "/app/Sale");
                request.setParameter("module", String.valueOf(module));
                request.setUserPrincipal(() -> "Navigation preview");
                var context = new WebContext(request, new MockHttpServletResponse(), servlet);
                context.setVariables(result.getModel());
                context.setVariable(ThymeleafEvaluationContext.THYMELEAF_EVALUATION_CONTEXT_CONTEXT_VARIABLE_NAME,
                        new ThymeleafEvaluationContext(application, null));
                String html = engine.process(result.getViewName(), context);
                assertTrue(html.contains("<b>11</b> of <span>11</span> built"));
                assertTrue(html.contains("<b>5</b> of <span>5</span> built"));
                assertTrue(html.contains("data-parent-url=\"/app/Sale\""));
                assertFalse(html.contains("not on the web yet"));
                for (var row : rights) if (row.get("ModuleID").equals(module)) {
                    assertTrue(html.contains("href=\"" + route(row) + "\""));
                }
                Files.writeString(Path.of("target/sale-navigation-" + module + "-preview.html"), html);
            }
        }
    }

    @Test void theMenuStillContainsOnlyTheUsersAllocatedScreens() {
        doReturn(List.of(rights.get(0))).when(menus).viewRights();
        assertEquals(1, menus.getAppCards().get(0).get("count"));
        assertEquals(1, menus.getAppCards().get(0).get("built"));
        assertTrue(((List<?>) menus.getScreenCards(6, 53).get("screens")).isEmpty());
    }

    private String route(Map<String, Object> row) {
        return menus.webRouteFor((Integer) row.get("ScreenID"), row.get("ScreenName").toString(), "");
    }
}
