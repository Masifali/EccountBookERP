package com.mst.services;

import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

class ErpNavigationServiceTest {
    private final DashboardModuleService menus = mock(DashboardModuleService.class);
    private final ErpNavigationService navigation = new ErpNavigationService(menus);

    private Map<String, Object> row(int app, String name, int module, int screen, String title) {
        return new LinkedHashMap<>(Map.of("AppId", app, "App", name, "ModuleID", module,
                "ModuleDescription", module == 3 ? "Account Reports" : "Transactions", "ScreenID", screen,
                "ScreenName", title, "ScreenAlias", title, "TargetUrl", "Desktop." + title, "Value", true));
    }

    @Test void rightsControlTheSidebarAndScreenParent() {
        var allowed = row(2, "Accounts", 3, 79, "General Ledger");
        var denied = row(4, "Purchase", 5, 12, "Purchase Order");
        denied.put("Value", false);
        when(menus.webRouteFor(eq(79), anyString(), anyString())).thenReturn("/accounts/reports/general-ledger");
        var result = navigation.build(List.of(allowed, allowed, denied), "/accounts/reports/general-ledger", Map.of(), null, null);
        assertEquals("/app/Accounts?module=3", result.get("parentRoute"));
        assertEquals("Account Reports", result.get("parentTitle"));
        assertEquals("General Ledger", result.get("currentTitle"));
        assertEquals(1, ((List<?>) result.get("apps")).size());
        var app = (Map<?, ?>) ((List<?>) result.get("apps")).get(0);
        assertEquals(1, ((List<?>) app.get("modules")).size());
        assertEquals(true, app.get("active"));
        verify(menus, never()).webRouteFor(eq(12), anyString(), anyString());
    }

    @Test void backMovesFromModuleToApplicationThenMainPages() {
        var rows = List.of(row(2, "Accounts", 3, 79, "General Ledger"));
        var module = navigation.build(rows, "/app/Accounts", Map.of("module", new String[]{"3"}), "Accounts", 3);
        assertEquals("/app/Accounts", module.get("parentRoute"));
        var app = navigation.build(rows, "/app/Accounts", Map.of(), "Accounts", null);
        assertEquals("/apps", app.get("parentRoute"));
        assertNull(navigation.build(rows, "/apps", Map.of(), null, null).get("parentRoute"));
    }

    @Test void singleModuleAutoSelectionDoesNotCreateABackLoop() {
        var result = navigation.build(List.of(row(2, "Accounts", 3, 79, "General Ledger")),
                "/app/Accounts", Map.of(), "Accounts", 3);
        assertEquals("/apps", result.get("parentRoute"));
    }

    @Test void queryParametersChooseTheCorrectParentForSharedScreenPaths() {
        when(menus.webRouteFor(eq(692), anyString(), anyString())).thenReturn("/party-processing/reports/grn-register?mode=grn");
        when(menus.webRouteFor(eq(693), anyString(), anyString())).thenReturn("/party-processing/reports/grn-register?mode=gdn");
        var result = navigation.build(List.of(row(16, "Party Processing", 50, 692, "GRN"), row(16, "Party Processing", 51, 693, "GDN")),
                "/party-processing/reports/grn-register", Map.of("mode", new String[]{"gdn"}, "id", new String[]{"123"}), null, null);
        assertEquals("/app/Party%20Processing?module=51", result.get("parentRoute"));
        assertEquals("GDN", result.get("currentTitle"));
    }

    @Test void screenPlaceholderReturnsToItsOwnModule() {
        var result = navigation.build(List.of(row(4, "Purchase", 5, 9000, "UnportedForm")),
                "/dashboard/screen", Map.of("name", new String[]{"UnportedForm"}), null, null);
        assertEquals("/app/Purchase?module=5", result.get("parentRoute"));
    }

    @Test void encodedAppNamesAndInvalidModuleIdsStayWithinAllowedParents() {
        var rows = List.of(row(4, "Store Management", 8, 10, "Item"));
        var result = navigation.build(rows, "/app/Store%20Management", Map.of("module", new String[]{"999"}), null, null);
        assertEquals("/apps", result.get("parentRoute"));
        assertEquals("Store Management", result.get("currentTitle"));
    }

    @Test void anonymousRequestsDoNotLoadAnyMenuRows() {
        assertNull(navigation.forRequest(new MockHttpServletRequest(), null, null));
        verifyNoInteractions(menus);
    }

    @Test void usernamesAndRightsAreResolvedForEachRequest() {
        when(menus.viewRights()).thenReturn(List.of(row(2, "Accounts", 3, 79, "Ledger")), List.of());
        var request = new MockHttpServletRequest("GET", "/erp/apps");
        request.setContextPath("/erp");
        request.setUserPrincipal(() -> "first");
        assertEquals("first", navigation.forRequest(request, null, null).get("username"));
        request.setUserPrincipal(() -> "second");
        var second = navigation.forRequest(request, null, null);
        assertEquals("second", second.get("username"));
        assertTrue(((List<?>) second.get("apps")).isEmpty());
    }

    @Test void menuFailureKeepsThePageAndSafeBackLinkAvailable() {
        when(menus.viewRights()).thenThrow(new IllegalStateException("Database unavailable"));
        var request = new MockHttpServletRequest("GET", "/accounts/report");
        request.setUserPrincipal(() -> "user");
        var result = navigation.forRequest(request, null, null);
        assertEquals(true, result.get("unavailable"));
        assertEquals("/apps", result.get("parentRoute"));
        assertFalse(result.toString().contains("Database unavailable"));
    }
}
