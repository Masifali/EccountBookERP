package com.mst.services;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.AccountsModuleViewController;
import com.mst.controllers.AppMenuController;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.ui.ExtendedModelMap;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class ContractorWagesNavigationTest {
    private final DashboardModuleService menus = spy(new DashboardModuleService());
    private final ScreenRouteIndex index = mock(ScreenRouteIndex.class);
    private final List<Map<String, Object>> rights = new ArrayList<>();
    private static final Map<Integer, String> VIEWS = Map.of(
            181, "accounts/vouchers/contractor_wise_wages_schedule",
            182, "accounts/vouchers/contractor_wages_account",
            183, "accounts/vouchers/contractor_wages_schedule",
            184, "accounts/vouchers/labour_wages",
            187, "accounts/vouchers/labour_wages_manual",
            474, "production/reports/evaluation_detail_wages",
            475, "accounts/reports/contractor_wages_history");

    @BeforeEach void loadRealSeededScreenIdsAndNames() throws Exception {
        JsonNode seed = new ObjectMapper().readTree(Path.of("migration/user-rights/reconciliation-input.json").toFile());
        Map<Integer, String> names = new LinkedHashMap<>();
        for (JsonNode grant : seed.get("grants")) {
            names.putIfAbsent(grant.path("ScreenId").asInt(), grant.path("ScreenName").asText());
        }
        for (JsonNode screen : seed.get("standardScreens")) {
            int module = screen.path("ModuleId").asInt();
            if (module != 10 && module != 51) continue;
            int id = screen.path("ScreenId").asInt();
            Map<String, Object> row = new LinkedHashMap<>();
            row.put("ScreenID", id);
            row.put("ScreenName", names.get(id));
            row.put("ScreenAlias", screen.path("ScreenAlias").asText());
            row.put("ModuleID", module);
            row.put("ModuleDescription", screen.path("ModuleDescription").asText());
            row.put("AppId", 1);
            row.put("App", "Contractor Wages");
            row.put("Value", true);
            rights.add(row);
        }
        assertEquals(VIEWS.keySet(), rights.stream().map(r -> (Integer) r.get("ScreenID")).collect(Collectors.toSet()));
        ReflectionTestUtils.setField(menus, "screenRouteIndex", index);
        doReturn(rights).when(menus).viewRights();
    }

    @Test void seededMenuCountsAllFiveTransactionsAndTwoReports() {
        var cards = menus.getModuleCards(1);
        assertEquals(2, cards.size());
        for (var card : cards) {
            int expected = card.get("moduleId").equals(10) ? 5 : 2;
            assertEquals(expected, card.get("count"));
            assertEquals(expected, card.get("built"));
        }
        assertEquals(7, menus.getAppCards().get(0).get("built"));
        verifyNoInteractions(index);
    }

    @Test void everySeededScreenOpensItsOwnExistingPageAndReturnsToItsParent() throws Exception {
        var controller = new AccountsModuleViewController();
        ReflectionTestUtils.setField(controller, "accountsReportService", mock(AccountsReportService.class));
        var mvc = MockMvcBuilders.standaloneSetup(controller).build();
        var navigation = new ErpNavigationService(menus);
        var visited = new HashSet<String>();
        for (var row : rights) {
            int id = (Integer) row.get("ScreenID");
            String route = menus.webRouteFor(id, (String) row.get("ScreenName"), "");
            assertNotNull(route, row.get("ScreenAlias").toString());
            assertTrue(visited.add(route), "Different screens must open different pages");
            mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name(VIEWS.get(id)));
            assertTrue(Files.exists(Path.of("src/main/resources/templates/" + VIEWS.get(id) + ".html")));
            var request = new MockHttpServletRequest("GET", route);
            request.setUserPrincipal(() -> "menu-test");
            assertEquals("/app/Contractor%20Wages?module=" + row.get("ModuleID"),
                    navigation.forRequest(request, null, null).get("parentRoute"));
        }
        assertEquals(7, visited.size());
    }

    @Test void sidebarApplicationAndChildModulesUseTheCompleteSeededMenu() {
        var controller = new AppMenuController();
        ReflectionTestUtils.setField(controller, "dashboardModuleService", menus);
        var model = new ExtendedModelMap();
        assertEquals("dashboard", controller.app("Contractor Wages", null, model));
        assertEquals(7, model.get("screenCount"));
        assertEquals(7, model.get("builtCount"));
        assertEquals(2, model.get("cardCount"));
        assertEquals("/app/Contractor%20Wages", model.get("appRoute"));
        for (int module : List.of(10, 51)) {
            @SuppressWarnings("unchecked")
            var screens = (List<Map<String, Object>>) menus.getScreenCards(1, module).get("screens");
            assertEquals(module == 10 ? 5 : 2, screens.size());
            for (var screen : screens) assertEquals(true, screen.get("built"));
        }
    }

    @Test void countsStillRespectTheUsersAllocatedMenu() {
        doReturn(List.of(rights.get(0))).when(menus).viewRights();
        var card = menus.getModuleCards(1).get(0);
        assertEquals(1, card.get("count"));
        assertEquals(1, card.get("built"));
        assertEquals(1, menus.getAppCards().get(0).get("count"));
    }
}
