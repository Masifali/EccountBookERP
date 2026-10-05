package com.mst.controllers;

import com.mst.services.ErpNavigationService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.mock.web.MockServletContext;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.web.context.support.StaticWebApplicationContext;
import org.thymeleaf.context.WebContext;
import org.thymeleaf.spring5.SpringTemplateEngine;
import org.thymeleaf.spring5.expression.ThymeleafEvaluationContext;
import org.thymeleaf.templateresolver.ClassLoaderTemplateResolver;
import org.thymeleaf.templateresolver.StringTemplateResolver;

import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

class VoucherPrintTemplateTest {
    @ParameterizedTest
    @CsvSource({"bank-receipt,4", "bank-payment,2", "cash-receipt,3", "cash-payment,1"})
    void paymentAndReceiptPagesRenderTheirPrintScripts(String voucher, int documentType) throws Exception {
        var mvc = MockMvcBuilders.standaloneSetup(new AccountsModuleViewController()).build();
        var result = mvc.perform(get("/accounts/vouchers/" + voucher)).andExpect(status().isOk())
                .andReturn().getModelAndView();
        assertNotNull(result);
        var servlet = new MockServletContext();
        var request = new MockHttpServletRequest(servlet, "GET", "/erp/accounts/vouchers/" + voucher);
        request.setContextPath("/erp");
        request.setUserPrincipal(() -> "Voucher test");
        try (var application = new StaticWebApplicationContext()) {
            application.setServletContext(servlet);
            var navigation = mock(ErpNavigationService.class);
            when(navigation.forRequest(any(), nullable(String.class), nullable(Integer.class))).thenReturn(Map.of(
                    "parentRoute", "/app/Accounts?module=2", "parentTitle", "Accounts Transaction",
                    "currentTitle", voucher, "username", "Voucher test", "apps", List.of(), "unavailable", false));
            application.getBeanFactory().registerSingleton("erpNavigationService", navigation);
            application.refresh();
            var context = new WebContext(request, new MockHttpServletResponse(), servlet);
            context.setVariables(result.getModel());
            context.setVariable(ThymeleafEvaluationContext.THYMELEAF_EVALUATION_CONTEXT_CONTEXT_VARIABLE_NAME,
                    new ThymeleafEvaluationContext(application, null));
            var resolver = new ClassLoaderTemplateResolver();
            resolver.setPrefix("templates/"); resolver.setSuffix(".html"); resolver.setTemplateMode("HTML");
            var engine = new SpringTemplateEngine(); engine.setTemplateResolver(resolver);
            String html = engine.process(result.getViewName(), context);
            assertTrue(html.contains("src=\"/erp/build/js/countx_crystal_print.js?v=20261003erp\""));
            assertTrue(html.contains("id=\"prvForm\""));
            assertTrue(html.contains("id=\"erp-parent-back\""));
            assertTrue(html.contains("PRV.init({ doc: " + documentType + " });"));
            assertFalse(html.contains("th:src="));
        }
    }

    @Test void everyThymeleafPrintScriptLinkParsesAndKeepsTheContextPath() throws Exception {
        var servlet = new MockServletContext();
        var request = new MockHttpServletRequest(servlet, "GET", "/erp/test");
        request.setContextPath("/erp");
        var context = new WebContext(request, new MockHttpServletResponse(), servlet);
        var resolver = new StringTemplateResolver(); resolver.setTemplateMode("HTML");
        var engine = new SpringTemplateEngine(); engine.setTemplateResolver(resolver);
        var scriptTags = Pattern.compile("<script\\b[^>]*\\b(?:th:src|data-th-src)\\s*=[^>]*>", Pattern.CASE_INSENSITIVE);
        int checked = 0;
        try (var paths = Files.walk(Path.of("src/main/resources/templates"))) {
            for (var path : paths.filter(p -> p.toString().endsWith(".html")).toList()) {
                // Only ASCII script tags are inspected; legacy page encodings need no conversion.
                String source = new String(Files.readAllBytes(path), StandardCharsets.UTF_8);
                var tags = scriptTags.matcher(source);
                while (tags.find()) {
                    String tag = tags.group();
                    String helper = tag.contains("/build/js/countx_crystal_print.js") ? "/build/js/countx_crystal_print.js"
                            : tag.contains("/js/print-rpt.js") ? "/js/print-rpt.js" : null;
                    if (helper == null) continue;
                    String html = assertDoesNotThrow(() -> engine.process(tag + "</script>", context), path.toString());
                    assertTrue(html.contains("src=\"/erp" + helper + "?v="), path.toString());
                    assertFalse(html.contains("th:src="), path.toString());
                    checked++;
                }
            }
        }
        assertTrue(checked >= 119, "All previously damaged print-helper references must be checked");
    }
}
