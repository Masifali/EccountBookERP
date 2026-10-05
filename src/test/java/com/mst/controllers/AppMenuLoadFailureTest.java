package com.mst.controllers;

import com.mst.services.DashboardModuleService;
import com.mst.services.ErpNavigationService;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.mock.web.MockServletContext;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.ui.ExtendedModelMap;
import org.springframework.web.context.support.StaticWebApplicationContext;
import org.thymeleaf.context.WebContext;
import org.thymeleaf.spring5.SpringTemplateEngine;
import org.thymeleaf.spring5.expression.ThymeleafEvaluationContext;
import org.thymeleaf.templateresolver.ClassLoaderTemplateResolver;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class AppMenuLoadFailureTest {
    @Test void exportLoadFailureIsNotReportedAsAMissingApplication() {
        var menus = mock(DashboardModuleService.class);
        when(menus.getAppByName("Export")).thenReturn(null);
        when(menus.getLastError()).thenReturn("Connection unavailable");
        var controller = new AppMenuController();
        ReflectionTestUtils.setField(controller, "dashboardModuleService", menus);
        var model = new ExtendedModelMap();
        assertEquals("dashboard", controller.export(null, model));
        assertEquals(false, model.get("appMissing"));
        assertEquals("Connection unavailable", model.get("loadError"));
        assertEquals("Export", model.get("appName"));
        assertEquals("/app/Export", model.get("appRoute"));
        verify(menus, never()).parentDiagnostics(anyString());
    }

    @Test void unavailableExportKeepsItsNameAndDoesNotRepeatTheRightsQuery() {
        var menus = mock(DashboardModuleService.class);
        when(menus.getAppByName("Export")).thenReturn(null);
        var controller = new AppMenuController();
        ReflectionTestUtils.setField(controller, "dashboardModuleService", menus);
        var model = new ExtendedModelMap();
        controller.export(null, model);
        assertEquals(true, model.get("appMissing"));
        assertNull(model.get("loadError"));
        assertEquals("Export", model.get("appName"));
        verify(menus, never()).parentDiagnostics(anyString());
    }

    @Test void dashboardPreservesItsInitialLoadError() {
        var menus = mock(DashboardModuleService.class);
        when(menus.getAppByName("DashBoard")).thenReturn(null);
        when(menus.getLastError()).thenReturn("Connection unavailable");
        var controller = new DashboardController();
        ReflectionTestUtils.setField(controller, "dashboardModuleService", menus);
        var model = new ExtendedModelMap();
        controller.dashboardHub(null, model);
        assertEquals(false, model.get("appMissing"));
        assertEquals("Connection unavailable", model.get("loadError"));
        verify(menus, never()).parentDiagnostics(anyString());
    }

    @Test void errorPageNamesExportOffersRetryAndHidesInternalDetails() {
        var servlet = new MockServletContext();
        try (var application = new StaticWebApplicationContext()) {
            application.setServletContext(servlet);
            var navigation = mock(ErpNavigationService.class);
            when(navigation.forRequest(any(), any(), any())).thenReturn(null);
            application.getBeanFactory().registerSingleton("erpNavigationService", navigation);
            application.getBeanFactory().registerSingleton("gearMenuController", mock(GearMenuController.class));
            application.refresh();
            var request = new MockHttpServletRequest(servlet, "GET", "/export");
            var context = new WebContext(request, new MockHttpServletResponse(), servlet);
            context.setVariable(ThymeleafEvaluationContext.THYMELEAF_EVALUATION_CONTEXT_CONTEXT_VARIABLE_NAME,
                    new ThymeleafEvaluationContext(application, null));
            context.setVariable("appName", "Export");
            context.setVariable("moduleTitle", "Export");
            context.setVariable("appRoute", "/app/Export");
            context.setVariable("cardCount", 0);
            context.setVariable("appMissing", false);
            context.setVariable("loadError", "Internal database details");
            var resolver = new ClassLoaderTemplateResolver();
            resolver.setPrefix("templates/"); resolver.setSuffix(".html"); resolver.setTemplateMode("HTML");
            var engine = new SpringTemplateEngine();
            engine.setTemplateResolver(resolver);
            String html = engine.process("dashboard", context);
            assertTrue(html.contains("<title>Export</title>"));
            assertTrue(html.contains("Unable to load Export modules."));
            assertTrue(html.contains("href=\"/app/Export\">Try again"));
            assertFalse(html.contains("Internal database details"));
            assertFalse(html.contains("No AppModules"));
            context.setVariable("loadError", null);
            context.setVariable("appMissing", true);
            html = engine.process("dashboard", context);
            assertTrue(html.contains("No Export modules are available for this login."));
            assertFalse(html.contains("Unable to load Export modules."));
        }
    }
}
