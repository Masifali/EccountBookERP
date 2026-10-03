package com.mst.controllers;

import com.mst.services.DashboardModuleService;
import com.mst.services.ErpNavigationService;
import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.mock.web.MockServletContext;
import org.springframework.web.context.support.StaticWebApplicationContext;
import org.thymeleaf.context.WebContext;
import org.thymeleaf.spring5.SpringTemplateEngine;
import org.thymeleaf.spring5.expression.ThymeleafEvaluationContext;
import org.thymeleaf.templateresolver.ClassLoaderTemplateResolver;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

class ErpNavigationTemplateTest {
    @Test void sharedFragmentRendersEscapedUserAndContextAwareParentLinks() throws Exception {
        var menus = mock(DashboardModuleService.class);
        when(menus.viewRights()).thenReturn(List.of(
                Map.of("AppId", 2, "App", "Accounts", "ModuleID", 3, "ModuleDescription", "Account Reports",
                        "ScreenID", 79, "ScreenName", "General Ledger", "ScreenAlias", "General Ledger", "Value", true),
                Map.of("AppId", 4, "App", "Purchase", "ModuleID", 5, "ModuleDescription", "Supplier Purchases",
                        "ScreenID", 20, "ScreenName", "Purchase Order", "ScreenAlias", "Purchase Order", "Value", true),
                Map.of("AppId", 5, "App", "Sale", "ModuleID", 6, "ModuleDescription", "Customer Sales",
                        "ScreenID", 21, "ScreenName", "Sale Order", "ScreenAlias", "Sale Order", "Value", true)));
        when(menus.webRouteFor(eq(79), anyString(), anyString())).thenReturn("/accounts/reports/general-ledger");
        var servlet = new MockServletContext();
        var request = new MockHttpServletRequest(servlet, "GET", "/erp/accounts/reports/general-ledger");
        request.setContextPath("/erp");
        request.setUserPrincipal(() -> "Asif <Admin>");
        try (var application = new StaticWebApplicationContext()) {
            application.setServletContext(servlet);
            application.getBeanFactory().registerSingleton("erpNavigationService", new ErpNavigationService(menus));
            application.refresh();
            var context = new WebContext(request, new MockHttpServletResponse(), servlet);
            context.setVariable(ThymeleafEvaluationContext.THYMELEAF_EVALUATION_CONTEXT_CONTEXT_VARIABLE_NAME,
                    new ThymeleafEvaluationContext(application, null));
            var resolver = new ClassLoaderTemplateResolver();
            resolver.setPrefix("templates/");
            resolver.setSuffix(".html");
            resolver.setTemplateMode("HTML");
            var engine = new SpringTemplateEngine();
            engine.setTemplateResolver(resolver);
            String html = engine.process("fragments/erp_navigation", context);
            assertTrue(html.contains("/erp/app/Accounts?module=3"));
            assertTrue(html.contains("href=\"/erp/apps\""));
            assertTrue(html.contains("Asif &lt;Admin&gt;"));
            assertFalse(html.contains("Asif <Admin>"));
            assertTrue(html.contains("id=\"erp-parent-back\""));
            assertTrue(html.contains("/erp/app/Purchase"));
            Files.writeString(Path.of("target/navigation-fragment-preview.html"), html);
            String ledger = engine.process("accounts/reports/general_ledger_desktop", context);
            assertEquals(1, ledger.split("id=\"erp-navigation\"", -1).length - 1);
            assertTrue(ledger.contains("Single Account Ledger"));
            Files.writeString(Path.of("target/navigation-preview.html"), ledger);
        }
    }
}
