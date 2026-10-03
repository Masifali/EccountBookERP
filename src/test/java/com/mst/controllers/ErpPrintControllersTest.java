package com.mst.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.controllers.ERPPrint.*;
import org.springframework.context.annotation.ClassPathScanningCandidateComponentProvider;
import org.springframework.core.type.filter.AnnotationTypeFilter;
import org.springframework.stereotype.Controller;
import com.mst.reports.ReportDataService;
import com.mst.reports.ReportDefinition;
import com.mst.reports.ReportRegistry;
import com.mst.reports.jasper.ReportTemplateService;
import com.mst.reports.prints.ReportPdfService;
import com.mst.security.CurrentUserContext;
import com.mst.services.GeneralLedgerSummaryService;
import com.mst.services.InventoryUomGridOutputService;
import com.mst.services.banking.ChequePrintingDesktopService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import org.mockito.ArgumentCaptor;
import org.springframework.http.MediaType;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class ErpPrintControllersTest {
    @TempDir Path templates;
    private final ReportDataService data = mock(ReportDataService.class);
    private final ReportRegistry registry = mock(ReportRegistry.class);
    private final ReportPdfService pdfs = mock(ReportPdfService.class);
    private final CurrentUserContext user = mock(CurrentUserContext.class);
    private final ObjectMapper json = new ObjectMapper().findAndRegisterModules();
    private CommonPrintController controller;
    private final java.util.List<Object> controllers = new java.util.ArrayList<>();
    private MockMvc mvc;

    @BeforeEach
    void setup() throws Exception {
        var scanner = new ClassPathScanningCandidateComponentProvider(false);
        scanner.addIncludeFilter(new AnnotationTypeFilter(Controller.class));
        var templateService = new ReportTemplateService(templates.toString());
        for (var bean : scanner.findCandidateComponents("com.mst.controllers.ERPPrint")) {
            Class<?> type = Class.forName(bean.getBeanClassName());
            Object instance = type == AccountsPrintController.class
                    ? new AccountsPrintController(mock(GeneralLedgerSummaryService.class))
                    : type.getDeclaredConstructor().newInstance();
            ReflectionTestUtils.setField(instance, "reportDataService", data);
            ReflectionTestUtils.setField(instance, "reportRegistry", registry);
            ReflectionTestUtils.setField(instance, "reportPdfService", pdfs);
            ReflectionTestUtils.setField(instance, "currentUserContext", user);
            ReflectionTestUtils.setField(instance, "objectMapper", json);
            ReflectionTestUtils.setField(instance, "reportTemplates", templateService);
            controllers.add(instance);
        }
        controller = controller(CommonPrintController.class);
        when(user.currentUserId()).thenReturn(12);
        when(user.currentCompanyId()).thenReturn(79);
        when(user.currentOrganizationId()).thenReturn(78);
        when(pdfs.keyOf(anyString())).thenReturn("sample");
        when(registry.get(anyString())).thenReturn(definition("sample.rpt"));
        when(data.run(anyString(), anyMap())).thenReturn(Map.of("rows", List.of()));
        mvc = MockMvcBuilders.standaloneSetup(controllers.toArray()).build();
    }

    private <T> T controller(Class<T> type) {
        return controllers.stream().filter(type::isInstance).map(type::cast).findFirst().orElseThrow();
    }

    private ReportDefinition definition(String template) {
        return new ReportDefinition("sample", template, "Sp_Test", "Test", List.of(), List.of());
    }

    @Test
    @SuppressWarnings("unchecked")
    void everyNamedPrintRouteSupportsBothGetAndPostInItsModule() throws Exception {
        Map<String, String> routes;
        try (var stream = getClass().getResourceAsStream("/reports/print-endpoints.json")) {
            routes = json.readValue(stream, Map.class);
        }
        java.util.Set<String> templates = new java.util.LinkedHashSet<>();
        for (String resource : List.of("print-contracts.json", "print-contracts-bll.json")) {
            try (var stream = getClass().getResourceAsStream("/reports/" + resource)) {
                for (var report : json.readTree(stream).get("prints")) {
                    templates.add(report.get("template").asText().toLowerCase(java.util.Locale.ROOT));
                }
            }
        }
        assertEquals(templates, routes.keySet(), "Every seeded/bundled report needs a named Java print action");
        assertEquals(routes.size(), new java.util.HashSet<>(routes.values()).size(), "Print routes must be unique");
        Map<String, Map<String, String>> modules;
        try (var stream = getClass().getResourceAsStream("/reports/print-modules.json")) {
            modules = json.readValue(stream, Map.class);
        }
        assertEquals(routes.keySet(), modules.keySet());
        for (var entry : routes.entrySet()) {
            String route = entry.getValue();
            Class<?> moduleController = Class.forName("com.mst.controllers.ERPPrint." + modules.get(entry.getKey()).get("controller"));
            mvc.perform(get(route).param("id", "123")).andExpect(handler().handlerType(moduleController))
                    .andExpect(status().isNotFound());
            mvc.perform(post(route).contentType(MediaType.APPLICATION_JSON).content("{\"id\":123}"))
                    .andExpect(handler().handlerType(moduleController)).andExpect(status().isNotFound());
        }
        verify(data, times(routes.size() * 2)).run(eq("sample"), anyMap());
    }

    @Test
    @SuppressWarnings("unchecked")
    void templateAndKeyAliasesUseSessionTenancyAndSameDataFlow() throws Exception {
        for (String route : List.of("/reports/print/by-template/example.rpt/pdf", "/api/print/by-template/example.rpt/pdf",
                "/reports/print/by-key/sample", "/api/print/sample/pdf", "/api/reports/sample/print.pdf", "/reports/jasper/sample")) {
            mvc.perform(post(route).contentType(MediaType.APPLICATION_JSON)
                    .content("{\"id\":123,\"companyId\":999,\"ORGANIZATIONID\":999,\"clsGlobalVariables\":999}"))
                    .andExpect(handler().handlerType(CommonPrintController.class)).andExpect(status().isNotFound());
        }
        var args = ArgumentCaptor.forClass(Map.class);
        verify(data, times(6)).run(eq("sample"), args.capture());
        for (Map<String, Object> request : args.getAllValues()) {
            assertEquals(79, request.get("companyId"));
            assertEquals(78, request.get("organizationId"));
            assertEquals(12, request.get("clsGlobalVariables"));
            assertEquals(123, request.get("id"));
            assertFalse(request.containsKey("ORGANIZATIONID"));
        }
    }

    @Test
    void deniedSessionNeverLoadsReportRows() throws Exception {
        when(user.currentUserId()).thenThrow(new AccessDeniedException("Sign in"));
        mvc.perform(get("/reports/print/by-key/sample")).andExpect(status().isForbidden());
        verifyNoInteractions(data);
    }

    @Test
    void migratedRoutesDoNotConflictWithRemainingScreenAndMetadataControllers() throws Exception {
        var all = new java.util.ArrayList<>(controllers);
        all.addAll(List.of(mock(com.mst.reports.PrintController.class), mock(ReportPrintController.class),
                mock(AccountDefinitionModulesController.class), mock(PackingMaterialReportsController.class),
                mock(InventoryUomGridOutputController.class), mock(ChequePrintingDesktopController.class)));
        MockMvc combined = MockMvcBuilders.standaloneSetup(all.toArray()).build();
        combined.perform(get("/api/print/by-template/example.rpt/pdf"))
                .andExpect(handler().handlerType(CommonPrintController.class)).andExpect(status().isNotFound());
        combined.perform(get("/reports/print/by-key/sample"))
                .andExpect(handler().handlerType(CommonPrintController.class)).andExpect(status().isNotFound());
    }

    @Test
    void compilesMainAndSubreportAndPreservesCompanyParameters() throws Exception {
        Files.writeString(templates.resolve("sample.manifest.json"), """
                {"main":"sample.jrxml","subreports":[{"name":"expenses.rpt","key":"expenses","file":"expenses.jrxml"}]}
                """);
        Files.writeString(templates.resolve("sample.jrxml"), """
                <?xml version="1.0" encoding="UTF-8"?>
                <jasperReport xmlns="http://jasperreports.sourceforge.net/jasperreports" name="sample" pageWidth="400" pageHeight="300" columnWidth="360" leftMargin="20" rightMargin="20" topMargin="20" bottomMargin="20">
                <parameter name="CompanyName" class="java.lang.String"/>
                <parameter name="SUBREPORT_expenses" class="net.sf.jasperreports.engine.JasperReport"/>
                <parameter name="SUBDATA_expenses" class="java.util.Collection"/>
                <title><band height="24"><textField><reportElement x="0" y="0" width="350" height="20"/><textFieldExpression><![CDATA[$P{CompanyName}]]></textFieldExpression></textField></band></title>
                <detail><band height="30"><subreport><reportElement x="0" y="0" width="350" height="25"/><dataSourceExpression><![CDATA[new net.sf.jasperreports.engine.data.JRMapCollectionDataSource($P{SUBDATA_expenses})]]></dataSourceExpression><subreportExpression><![CDATA[$P{SUBREPORT_expenses}]]></subreportExpression></subreport></band></detail>
                </jasperReport>
                """);
        Files.writeString(templates.resolve("expenses.jrxml"), """
                <?xml version="1.0" encoding="UTF-8"?>
                <jasperReport xmlns="http://jasperreports.sourceforge.net/jasperreports" name="expenses" pageWidth="350" pageHeight="200" columnWidth="350" leftMargin="0" rightMargin="0" topMargin="0" bottomMargin="0">
                <field name="Label" class="java.lang.String"/>
                <detail><band height="20"><textField><reportElement x="0" y="0" width="350" height="20"/><textFieldExpression><![CDATA[$F{Label}]]></textFieldExpression></textField></band></detail>
                </jasperReport>
                """);
        when(data.run(anyString(), anyMap())).thenReturn(Map.of("rows", List.of(Map.of("Id", 1)),
                "reportParameters", Map.of("@CompanyName", "Company from session"),
                "subReports", List.of(Map.of("template", "EXPENSES.RPT", "rows", List.of(Map.of("Label", "Expense included"))))));
        byte[] bytes = mvc.perform(get("/reports/print/by-key/sample"))
                .andExpect(status().isOk()).andExpect(content().contentType("application/pdf"))
                .andReturn().getResponse().getContentAsByteArray();
        var reader = new com.itextpdf.text.pdf.PdfReader(bytes);
        try {
            String text = com.itextpdf.text.pdf.parser.PdfTextExtractor.getTextFromPage(reader, 1);
            assertTrue(text.contains("Company from session"));
            assertTrue(text.contains("Expense included"));
        } finally { reader.close(); }
    }

    @Test
    void generatedLayoutStillWorksWhenThereIsNoConvertedTemplate() throws Exception {
        when(data.run(anyString(), anyMap())).thenReturn(Map.of("rows", List.of(Map.of("Description", "Sample row", "Amount", 10)),
                "reportParameters", Map.of("@CompanyName", "Company from session")));
        byte[] bytes = mvc.perform(get("/reports/print/by-key/sample"))
                .andExpect(status().isOk()).andExpect(content().contentType("application/pdf"))
                .andReturn().getResponse().getContentAsByteArray();
        assertTrue(bytes.length > 1000);
    }

    @Test
    void templateNameWithoutRptExtensionStillResolves() throws Exception {
        mvc.perform(get("/reports/print/by-template/example/pdf")).andExpect(status().isNotFound());
        verify(pdfs).keyOf("example.rpt");
        verify(data).run(eq("sample"), anyMap());
    }

    @Test
    void emptyReportsReturnAReadableMessageInsteadOfAnHtmlErrorPage() throws Exception {
        mvc.perform(get("/reports/print/by-key/sample")).andExpect(status().isNotFound())
                .andExpect(content().contentTypeCompatibleWith(MediaType.TEXT_PLAIN))
                .andExpect(content().string("No Record Found For Display"));
    }

    @Test
    void screenPrintCombinesEveryGridInOrderAndSkipsEmptyGrids() throws Exception {
        when(pdfs.gridPdf(eq("Invoices"), any(), anyList())).thenReturn(page("Invoice rows"));
        when(pdfs.gridPdf(eq("Receipts"), any(), anyList())).thenReturn(page("Receipt rows"));
        byte[] bytes = mvc.perform(post("/reports/print/screen").contentType(MediaType.APPLICATION_JSON).content("""
                [{"rpt":"Invoices","rows":[{"Amount":"1,000.00"}]},
                 {"rpt":"Empty","rows":[]}, {"rpt":"Receipts","rows":[{"Amount":"50.00"}]}]
                """)).andExpect(status().isOk()).andExpect(content().contentType("application/pdf"))
                .andReturn().getResponse().getContentAsByteArray();
        var reader = new com.itextpdf.text.pdf.PdfReader(bytes);
        try {
            assertEquals(2, reader.getNumberOfPages());
            assertTrue(com.itextpdf.text.pdf.parser.PdfTextExtractor.getTextFromPage(reader, 1).contains("Invoice rows"));
            assertTrue(com.itextpdf.text.pdf.parser.PdfTextExtractor.getTextFromPage(reader, 2).contains("Receipt rows"));
        } finally { reader.close(); }
        verify(pdfs, never()).gridPdf(eq("Empty"), any(), anyList());
        verifyNoInteractions(data);
        mvc.perform(post("/reports/print/screen").contentType(MediaType.APPLICATION_JSON).content("[]"))
                .andExpect(status().isNotFound());
    }

    @Test
    void barcodePrintUsesTheAuthorizedBarsAndCreatesOneLabelPage() throws Exception {
        var items = mock(com.mst.services.InventoryPosItemService.class);
        ReflectionTestUtils.setField(controller(PosPrintController.class), "posItemService", items);
        var bars = java.util.stream.IntStream.range(0, 31).mapToObj(i -> 2).toList();
        when(items.barcode("123456")).thenReturn(Map.of("code", "123456", "bars", bars));
        byte[] bytes = mvc.perform(post("/reports/print/item-barcode").contentType(MediaType.APPLICATION_JSON)
                .content("{\"code\":\"123456\"}"))
                .andExpect(status().isOk()).andExpect(content().contentType("application/pdf"))
                .andReturn().getResponse().getContentAsByteArray();
        var reader = new com.itextpdf.text.pdf.PdfReader(bytes);
        try {
            assertEquals(1, reader.getNumberOfPages());
            assertEquals(82f, reader.getPageSize(1).getWidth());
            assertEquals(70f, reader.getPageSize(1).getHeight());
            assertTrue(reader.getPageContent(1).length > 0);
        } finally { reader.close(); }
        verify(items).barcode("123456");
        when(items.barcode("denied")).thenThrow(new AccessDeniedException("View permission required"));
        mvc.perform(post("/reports/print/item-barcode").contentType(MediaType.APPLICATION_JSON)
                .content("{\"code\":\"denied\"}")).andExpect(status().isForbidden());
    }

    private byte[] page(String text) throws Exception {
        var output = new java.io.ByteArrayOutputStream();
        var document = new com.itextpdf.text.Document();
        com.itextpdf.text.pdf.PdfWriter.getInstance(document, output);
        document.open();
        document.add(new com.itextpdf.text.Paragraph(text));
        document.close();
        return output.toByteArray();
    }

    @Test
    void gridPrintKeepsTheSubmittedRowsAndNeverRerunsADatabaseReport() throws Exception {
        mvc.perform(post("/reports/print/grid").contentType(MediaType.APPLICATION_JSON)
                .content("{\"rpt\":\"Selected rows\",\"title\":\"Current grid\",\"rows\":[{\"ID\":7},{\"ID\":2}]}"))
                .andExpect(status().isOk()).andExpect(handler().handlerType(CommonPrintController.class));
        verify(pdfs).writeGridPdf(any(), eq("Selected rows"), eq("Current grid"), eq(List.of(Map.of("ID", 7), Map.of("ID", 2))));
        verifyNoInteractions(data);
    }

    @Test
    void specializedPrintsKeepTheirExistingServicesAndRefusalStatus() throws Exception {
        var uom = mock(InventoryUomGridOutputService.class);
        ReflectionTestUtils.setField(controller(InventoryPrintController.class), "uomGridService", uom);
        when(uom.print(any())).thenThrow(new IllegalStateException("History changed"));
        mvc.perform(post("/api/inventory/uom-schedules/grid-print").contentType(MediaType.APPLICATION_JSON)
                .content("{\"ids\":[1],\"columns\":[\"ItemName\"]}"))
                .andExpect(handler().handlerType(InventoryPrintController.class)).andExpect(status().isConflict());
        var cheques = mock(ChequePrintingDesktopService.class);
        ReflectionTestUtils.setField(controller(BankingPrintController.class), "chequePrintingService", cheques);
        when(cheques.print(anyInt(), anyList())).thenThrow(new IllegalArgumentException("Select cheques"));
        mvc.perform(post("/accounts/api/banking/cheque-printing/print").contentType(MediaType.APPLICATION_JSON)
                .content("{\"bankId\":3,\"ids\":[376,377]}"))
                .andExpect(handler().handlerType(BankingPrintController.class)).andExpect(status().isBadRequest());
        verify(cheques).print(3, List.of(376, 377));
    }

    @Test
    void packingAndUomKeepTheirJsonValidationMessages() throws Exception {
        var packing = mock(com.mst.services.PackingMaterialReportsService.class);
        ReflectionTestUtils.setField(controller(PackingMaterialPrintController.class), "packingReportsService", packing);
        when(packing.print(anyString(), any(), anyString())).thenThrow(new IllegalArgumentException("Choose a date"));
        mvc.perform(post("/api/packing-material/reports/purchase-order-register/print").contentType(MediaType.APPLICATION_JSON)
                        .accept(MediaType.APPLICATION_JSON).content("{}"))
                .andExpect(status().isBadRequest()).andExpect(jsonPath("$.message").value("Choose a date"));
        var uom = mock(InventoryUomGridOutputService.class);
        ReflectionTestUtils.setField(controller(InventoryPrintController.class), "uomGridService", uom);
        when(uom.print(any())).thenThrow(new AccessDeniedException("Print permission required"));
        mvc.perform(post("/api/inventory/uom-schedules/grid-print").contentType(MediaType.APPLICATION_JSON)
                        .accept(MediaType.APPLICATION_JSON).content("{}"))
                .andExpect(status().isForbidden()).andExpect(jsonPath("$.message").value("Print permission required"));
    }
}
