package com.mst.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.itextpdf.text.pdf.PdfReader;
import com.itextpdf.text.pdf.parser.PdfTextExtractor;
import com.mst.controllers.ERPPrint.LabPrintController;
import com.mst.models.Company;
import com.mst.repositories.ICompanyRepository;
import com.mst.reports.jasper.ReportTemplateService;
import com.mst.security.CurrentUserContext;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class LabVehicleReportPrintTest {
    private static final String URL = "/reports/lab/purchase-analysis-by-vehicle";
    @TempDir Path emptyTemplateFolder;
    private final CurrentUserContext context = mock(CurrentUserContext.class);
    private final ObjectMapper json = new ObjectMapper();
    private MockMvc mvc;

    @BeforeEach
    void setup() throws Exception {
        when(context.currentUserId()).thenReturn(78);
        when(context.currentCompanyId()).thenReturn(58);
        var companies = mock(ICompanyRepository.class);
        var company = new Company();
        company.setCompName("PRINT TEST COMPANY");
        company.setCompAddress("Synthetic report verification");
        when(companies.findById(58)).thenReturn(Optional.of(company));
        var controller = new LabPrintController();
        ReflectionTestUtils.setField(controller, "currentUserContext", context);
        ReflectionTestUtils.setField(controller, "companies", companies);
        // Force packaged resources: a developer's external template folder must not hide missing subreports.
        ReflectionTestUtils.setField(controller, "reportTemplates", new ReportTemplateService(emptyTemplateFolder.toString()));
        mvc = MockMvcBuilders.standaloneSetup(controller).build();
    }

    private Map<String, Object> detail() {
        var row = new LinkedHashMap<String, Object>();
        row.put("ItemName", "DETAIL665");
        row.put("PurchaseType", "Gate Purchase");
        row.put("Supplier", "Sample Supplier");
        row.put("VehicleNo", "TEST-665");
        row.put("LabDate", "2026-10-05");
        row.put("AnalysisQty", 105);
        row.put("NetWeight", 6021.25);
        row.put("ItemRate", 4000);
        row.put("NetRate", 3900);
        row.put("Moisture", 29.5);
        row.put("WeightCut", 1.5);
        row.put("QtyForWtCut", 90);
        row.put("Dust/Stone", 0);
        row.put("Empty Shell / Trash", 0);
        return row;
    }

    private Map<String, Object> summary() {
        return Map.of("ItemName", "SUB665", "TotalAnalysisQty", 105,
                "TotalNetWeight", 6021.25, "AvgBroken", 3, "WeightedAvgBroken", 2.5);
    }

    @Test
    void packaged665MainAndSummaryRenderTogetherThroughThePrintButtonEndpoint() throws Exception {
        var response = mvc.perform(post(URL).contentType("application/json").content(json.writeValueAsString(
                Map.of("detail", List.of(detail()), "summary", List.of(summary())))))
                .andExpect(status().isOk()).andExpect(content().contentType("application/pdf"))
                .andReturn().getResponse();
        byte[] pdf = response.getContentAsByteArray();
        assertEquals("%PDF", new String(pdf, 0, 4));
        var reader = new PdfReader(pdf);
        try {
            StringBuilder text = new StringBuilder();
            for (int page = 1; page <= reader.getNumberOfPages(); page++) {
                text.append(PdfTextExtractor.getTextFromPage(reader, page));
            }
            assertTrue(text.toString().contains("DETAIL665"), "Main detail rows must appear");
            assertTrue(text.toString().contains("SUB665"), "The summary must receive its own rows");
            assertTrue(text.toString().contains("PRINT TEST COMPANY"), "Company must come from the current session");
        } finally { reader.close(); }
        Path output = Path.of("target/lab-665-qa/sample-report.pdf");
        Files.createDirectories(output.getParent());
        Files.write(output, pdf);
    }

    @Test
    void emptyDetailOrSummaryDoesNotProduceAnIncompletePdf() throws Exception {
        for (Map<String, Object> body : List.<Map<String, Object>>of(
                Map.of("detail", List.of(detail()), "summary", List.of()),
                Map.of("detail", List.of(), "summary", List.of(summary())))) {
            mvc.perform(post(URL).contentType("application/json").content(json.writeValueAsString(body)))
                    .andExpect(status().isNotFound()).andExpect(content().string("No Record Found For Display"));
        }
    }

    @Test
    void signedOutRequestsCannotPrint() throws Exception {
        when(context.currentUserId()).thenThrow(new AccessDeniedException("Sign in to print reports."));
        mvc.perform(post(URL).contentType("application/json").content(json.writeValueAsString(
                Map.of("detail", List.of(detail()), "summary", List.of(summary())))))
                .andExpect(status().isForbidden());
    }
}
