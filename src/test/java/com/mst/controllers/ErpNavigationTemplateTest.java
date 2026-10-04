package com.mst.controllers;

import com.mst.services.DashboardModuleService;
import com.mst.services.ErpNavigationService;
import com.mst.services.DesktopUserRightsService;
import com.mst.services.AccountsReportService;
import com.mst.security.CurrentUserContext;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.util.LinkedCaseInsensitiveMap;
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
            var rights = mock(DesktopUserRightsService.class);
            when(rights.canManage()).thenReturn(true);
            application.getBeanFactory().registerSingleton("desktopUserRightsService", rights);
            application.getBeanFactory().registerSingleton("gearMenuController", mock(GearMenuController.class));
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
            assertTrue(html.contains("id=\"erp-favorites\""));
            assertTrue(html.contains("id=\"erp-recent\""));
            assertTrue(html.contains("/erp/user-management/rights?tab=rights&amp;screenId=79"));
            when(rights.canManage()).thenReturn(false);
            assertFalse(engine.process("fragments/erp_navigation", context).contains("title=\"View Rights Report\""));
            when(rights.canManage()).thenReturn(true);
            Files.writeString(Path.of("target/navigation-fragment-preview.html"), html);
            context.setVariable("accountClass", 2);
            request.setContextPath("");
            Files.createDirectories(Path.of("target/ui-verification"));
            Files.writeString(Path.of("target/ui-verification/trade-tools-rendered.html"), engine.process("accounts/reports/trade_accounts", context));
            request.setContextPath("/erp");
            String ledger = engine.process("accounts/reports/general_ledger_desktop", context);
            assertEquals(1, ledger.split("id=\"erp-navigation\"", -1).length - 1);
            assertTrue(ledger.contains("Single Account Ledger"));
            Files.writeString(Path.of("target/navigation-preview.html"), ledger);
            request.setContextPath("");
            request.setRequestURI("/accounts/reports/general-ledger-multi");
            List<Map<String, Object>> accountRows = List.of(
                    Map.of("Id", 1, "AccountTitle", "Sample Rice Traders", "AccountCode", "250404993", "ClassName", "Liabilities", "AccountType", "Receivables & Payables", "ParentAccountTitle", "TRADE SUPPLIERS"),
                    Map.of("Id", 2, "AccountTitle", "Sample Flour Mills", "AccountCode", "150404994", "ClassName", "Assets", "AccountType", "Receivables & Payables", "ParentAccountTitle", "TRADE DEBTORS"),
                    Map.of("Id", 3, "AccountTitle", "Sample Bank", "AccountCode", "150104001", "ClassName", "Assets", "AccountType", "Bank", "ParentAccountTitle", "BANK ACCOUNTS"));
            // Use the real service and JDBC map semantics: aliases change serialized key casing.
            List<Map<String, Object>> jdbcRows = new java.util.ArrayList<>();
            for (var row : accountRows) {
                Map<String, Object> jdbcRow = new LinkedCaseInsensitiveMap<>();
                jdbcRow.putAll(row);
                jdbcRows.add(jdbcRow);
            }
            var jdbc = mock(JdbcTemplate.class);
            when(jdbc.queryForList(anyString(), eq(0), eq(0), eq(0), eq(0), eq("DetailAccount"))).thenReturn(jdbcRows);
            var reports = new AccountsReportService();
            ReflectionTestUtils.setField(reports, "jdbcTemplate", jdbc);
            ReflectionTestUtils.setField(reports, "currentUserContext", mock(CurrentUserContext.class));
            var accounts = reports.getAllDetailAccounts();
            assertEquals(3, accounts.size());
            String accountJson = new ObjectMapper().writeValueAsString(accounts);
            assertTrue(accountJson.contains("\"accountTitle\""));
            assertTrue(accountJson.contains("\"ID\""));
            Files.writeString(Path.of("target/ui-verification/ledger-account-lookup.json"), accountJson);
            context.setVariable("accountsList", accounts);
            context.setVariable("dateTypesList", List.of());
            String multiLedger = engine.process("accounts/reports/general_ledger", context);
            assertTrue(multiLedger.contains("general_ledger_ui.js"));
            assertFalse(multiLedger.contains("select2.full.min.js"));
            assertTrue(multiLedger.contains("ParentAccountTitle"));
            Files.writeString(Path.of("target/ui-verification/multi-ledger-rendered.html"), multiLedger);
            request.setContextPath("/erp");
            request.setRequestURI("/erp/accounts/reports/general-ledger");

            // The same child form must fill its iframe without a second rail or
            // another menu lookup, while retaining navigation when opened alone.
            clearInvocations(menus);
            request.addHeader("Sec-Fetch-Dest", "iframe");
            String embedded = engine.process("production/p280_input", context);
            assertFalse(embedded.contains("id=\"erp-navigation\""));
            assertFalse(embedded.contains("/build/js/erp_navigation.js"));
            assertTrue(embedded.contains("id=\"viewForm\""));
            verifyNoInteractions(menus);

            request.removeHeader("Sec-Fetch-Dest");
            request.addHeader("Sec-Fetch-Dest", "document");
            String standalone = engine.process("production/p280_input", context);
            assertEquals(1, standalone.split("id=\"erp-navigation\"", -1).length - 1);
            verify(menus).viewRights();
        }
    }
}
