package com.mst.services;

import com.mst.controllers.AppMenuController;
import com.mst.controllers.CompanyProfileController;
import com.mst.controllers.ConfigurationViewController;
import com.mst.controllers.DesktopUserRightsController;
import com.mst.controllers.UserDefineController;
import com.mst.serviceInterface.IConfigurationService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.ui.ExtendedModelMap;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class AdminPanelNavigationTest {
    private final DashboardModuleService menus = spy(new DashboardModuleService());
    private final ScreenRouteIndex index = mock(ScreenRouteIndex.class);

    // ScreenDefinition/AppModules/App metadata read from the desktop database, 2026-10-03.
    private static final List<Map<String, Object>> SCREENS = List.of(
            screen(313, 22, "frmDefineUser", "Define User", "Configurations.frmUserRights"),
            screen(317, 22, "frmSystemConfiguration", "System Configurations", "Configurations.Configuration"),
            screen(396, 43, "frmSystemConfiguration", "System Configuration", "Configurations.Configuration"),
            screen(401, 43, "frmCompanyProfile", "Company Profile", "Configurations.frmCompanyProfile"),
            screen(409, 43, "UserRightsByCompany", "User Rights By Company", "Configurations.UserRightsByCompany"));

    private static final Map<Integer, String> VIEWS = Map.of(
            313, "userAccounts/user_define",
            317, "configurations/configuration",
            396, "configurations/configuration",
            401, "configurations/company_profile",
            409, "userAccounts/desktop_rights");

    @BeforeEach void setup() {
        ReflectionTestUtils.setField(menus, "screenRouteIndex", index);
        doReturn(SCREENS).when(menus).viewRights();
    }

    @Test void existingAdminPagesAreCountedAtBothMenuLevels() {
        var card = menus.getAppCards().get(0);
        assertEquals("Admin Panel", card.get("title"));
        assertEquals(5, card.get("count"));
        assertEquals(5, card.get("built"));
        for (var module : menus.getModuleCards(18)) {
            assertEquals(module.get("count"), module.get("built"));
        }
        verifyNoInteractions(index);
    }

    @Test void allocatedSystemAdminScreensShowThreeOfThreeAndTheirOwnLinks() {
        doReturn(SCREENS.subList(2, 5)).when(menus).viewRights();
        var controller = new AppMenuController();
        ReflectionTestUtils.setField(controller, "dashboardModuleService", menus);
        var model = new ExtendedModelMap();
        assertEquals("dashboard", controller.app("Admin Panel", null, model));
        assertEquals(1, model.get("cardCount"));
        assertEquals(3, model.get("screenCount"));
        assertEquals(3, model.get("builtCount"));
        assertEquals(1, menus.getAppCards().get(0).get("moduleCount"));
        @SuppressWarnings("unchecked")
        var screens = (List<Map<String, Object>>) menus.getScreenCards(18, 43).get("screens");
        assertEquals(List.of("/configurations", "/configurations/company-profile", "/user-management/rights"),
                screens.stream().map(s -> s.get("route")).collect(java.util.stream.Collectors.toList()));
    }

    @Test void everyMappedFormHasARegisteredPageAndBackToItsAllocatedParent() throws Exception {
        var users = new UserDefineController();
        var userService = mock(UserDefineService.class);
        ReflectionTestUtils.setField(users, "service", userService);
        var configuration = new ConfigurationViewController();
        ReflectionTestUtils.setField(configuration, "configurationService", mock(IConfigurationService.class));
        var rightsService = mock(DesktopUserRightsService.class);
        var companyService = mock(CompanyProfileService.class);
        var mvc = MockMvcBuilders.standaloneSetup(users, configuration,
                new DesktopUserRightsController(rightsService), new CompanyProfileController(companyService)).build();
        var navigation = new ErpNavigationService(menus);
        for (var row : SCREENS) {
            int id = (Integer) row.get("ScreenID");
            String route = menus.webRouteFor(id, row.get("ScreenName").toString(), row.get("TargetUrl").toString());
            assertNotNull(route);
            mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name(VIEWS.get(id)));
            assertTrue(Files.exists(Path.of("src/main/resources/templates/" + VIEWS.get(id) + ".html")));
            // The same Configuration form belongs to either module depending on allocated rights.
            doReturn(List.of(row)).when(menus).viewRights();
            var request = new MockHttpServletRequest("GET", route);
            request.setUserPrincipal(() -> "navigation-test");
            assertEquals("/app/Admin%20Panel?module=" + row.get("ModuleID"),
                    navigation.forRequest(request, null, null).get("parentRoute"));
        }
        verify(userService).admin();
        verify(rightsService).admin();
        verify(companyService).canOpen();
    }

    @Test void separateDesktopRightsEditingFormIsNotMislabelledAsTheCompanyRightsEditor() {
        var legacy = screen(318, 22, "frmUserRightsManagement", "User Right Management",
                "UserRightsManagement.UserRightsEditing");
        doReturn(List.of(SCREENS.get(0), SCREENS.get(1), legacy)).when(menus).viewRights();
        var card = menus.getAppCards().get(0);
        assertEquals(3, card.get("count"));
        assertEquals(2, card.get("built"));
        assertNull(menus.webRouteFor(318, legacy.get("ScreenName").toString(), legacy.get("TargetUrl").toString()));
        assertNull(menus.webRouteFor(397, legacy.get("ScreenName").toString(), legacy.get("TargetUrl").toString()));
    }

    @Test void mappingDoesNotAddScreensOutsideTheUsersAllocatedMenu() {
        doReturn(List.of(SCREENS.get(0))).when(menus).viewRights();
        assertEquals(1, menus.getAppCards().get(0).get("count"));
        assertEquals(1, menus.getAppCards().get(0).get("built"));
        assertTrue(((List<?>) menus.getScreenCards(18, 43).get("screens")).isEmpty());
    }

    private static Map<String, Object> screen(int id, int module, String name, String alias, String form) {
        return Map.of("ScreenID", id, "ScreenName", name, "ScreenAlias", alias,
                "TargetUrl", "Architecture.WinApp." + form, "ModuleID", module,
                "ModuleDescription", module == 22 ? "Admin Panel" : "System Admin",
                "AppId", 18, "App", "Admin Panel", "Value", true);
    }
}
