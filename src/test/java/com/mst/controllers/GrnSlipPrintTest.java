package com.mst.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.itextpdf.text.pdf.PdfReader;
import com.itextpdf.text.pdf.parser.PdfTextExtractor;
import com.mst.controllers.ERPPrint.CommonPrintController;
import com.mst.controllers.ERPPrint.PurchasePrintController;
import com.mst.models.Company;
import com.mst.repositories.ICompanyRepository;
import com.mst.repositories.ReportContractRepository;
import com.mst.reports.ReportDataService;
import com.mst.reports.ReportRegistry;
import com.mst.reports.jasper.ReportTemplateService;
import com.mst.reports.prints.ReportPdfService;
import com.mst.security.CurrentUserContext;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import org.springframework.beans.factory.ObjectProvider;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.*;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class GrnSlipPrintTest {
    private static final String TEMPLATE = "211-InvRptGoodsReceiptsNotesRiceSlip.rpt";
    private static final String URL = "/reports/print/211-goods-receipts-notes-rice-slip";
    @TempDir Path emptyTemplateFolder;
    private final JdbcTemplate jdbc = new JdbcTemplate() {
        @Override public List<Map<String, Object>> queryForList(String sql, Object... args) {
            queries.add(sql);
            bindings.add(Arrays.asList(args));
            if (sql.startsWith("EXEC Sp_InvGrn_RiceSlip_Rpt ")) return List.of(mainRow());
            if (sql.equals("EXEC Sp_InvGrnDetailEmptyBagsSubReport @GrnId=?")) return List.of(
                    Map.of("ItemName", "BAGS211", "TypeDescription", "Party", "Bags_Condition", "Good",
                            "ReceivedQty", 100, "PurchaseQty", 5));
            if (sql.startsWith("EXEC Sp_InvContractorWagesBillHeader_SlipandRegister ")) return List.of(
                    Map.of("ContractorName", "CONTRACTOR211", "WagesAccountName", "Loading",
                            "ItemName", "Rice", "Qty", 105, "PackSize", 50, "WagesTypeId", 1));
            throw new AssertionError("Unexpected report query: " + sql);
        }
    };
    private final CurrentUserContext user = mock(CurrentUserContext.class);
    private final List<String> queries = new ArrayList<>();
    private final List<List<Object>> bindings = new ArrayList<>();
    private ReportRegistry registry;
    private ReportDataService data;
    private ReportPdfService pdfs;
    private MockMvc mvc;

    @BeforeEach
    @SuppressWarnings("unchecked")
    void setup() throws Exception {
        registry = new ReportRegistry();
        // Exercise seed loading and key selection too: the seed must not replace the subreports.
        var repository = new ReportContractRepository();
        ReflectionTestUtils.setField(repository, "jdbcTemplate", mock(JdbcTemplate.class));
        ObjectProvider<ReportContractRepository> provider = mock(ObjectProvider.class);
        when(provider.getIfAvailable()).thenReturn(repository);
        ReflectionTestUtils.setField(registry, "contractRepository", provider);
        ReflectionTestUtils.invokeMethod(registry, "loadSeededContracts");
        when(user.currentUserId()).thenReturn(12);
        when(user.currentOrganizationId()).thenReturn(78);
        when(user.currentCompanyId()).thenReturn(79);
        var company = new Company();
        company.setCompName("GRN TEST COMPANY");
        var companies = mock(ICompanyRepository.class);
        when(companies.findById(79)).thenReturn(Optional.of(company));
        data = new ReportDataService();
        ReflectionTestUtils.setField(data, "jdbcTemplate", jdbc);
        ReflectionTestUtils.setField(data, "currentUserContext", user);
        ReflectionTestUtils.setField(data, "registry", registry);
        ReflectionTestUtils.setField(data, "companies", companies);
        pdfs = new ReportPdfService(registry, data, null, user);
        var purchase = new PurchasePrintController();
        var common = new CommonPrintController();
        for (Object controller : List.of(purchase, common)) {
            ReflectionTestUtils.setField(controller, "reportDataService", data);
            ReflectionTestUtils.setField(controller, "reportRegistry", registry);
            ReflectionTestUtils.setField(controller, "reportPdfService", pdfs);
            ReflectionTestUtils.setField(controller, "currentUserContext", user);
            ReflectionTestUtils.setField(controller, "objectMapper", new ObjectMapper());
            ReflectionTestUtils.setField(controller, "reportTemplates", new ReportTemplateService(emptyTemplateFolder.toString()));
        }
        mvc = MockMvcBuilders.standaloneSetup(purchase, common).build();
    }

    private Map<String, Object> mainRow() {
        var row = new LinkedHashMap<String, Object>();
        row.put("ItemName", "RICE211");
        row.put("CompanyName", "GRN SUPPLIER");
        row.put("DocNo", 20);
        row.put("DocDate", "2026-08-06");
        row.put("GpDate", "2026-08-06");
        row.put("GpNo", 21);
        row.put("ItemQty", 105);
        row.put("GrossWeight", 6126.25);
        row.put("EBWTotal", 105);
        row.put("NetBillWeight", 6021.25);
        row.put("VehicleNo", "TEST211");
        row.put("WareHouseName", "TEST WAREHOUSE");
        return row;
    }

    @Test
    void namedRowPrintProducesPdfWithBothPackagedSubreportsAndCompany() throws Exception {
        var response = mvc.perform(post(URL).contentType("application/json")
                        .content("{\"id\":20,\"documentTypeId\":46,\"companyId\":999,\"organizationId\":999}"))
                .andExpect(status().isOk()).andExpect(content().contentType("application/pdf"))
                .andReturn().getResponse();
        byte[] pdf = response.getContentAsByteArray();
        var reader = new PdfReader(pdf);
        StringBuilder text = new StringBuilder();
        try {
            for (int page = 1; page <= reader.getNumberOfPages(); page++) text.append(PdfTextExtractor.getTextFromPage(reader, page));
        } finally { reader.close(); }
        for (String expected : List.of("RICE211", "BAGS211", "CONTRACTOR211", "GRN TEST COMPANY")) {
            assertTrue(text.toString().contains(expected), "Missing from PDF: " + expected);
        }
        assertEquals(List.of(78, 79, 20, 46), bindings.get(0));
        assertEquals(List.of(20), bindings.get(1));
        assertEquals(List.of(78, 79, "101", 46, 20, "0"), bindings.get(2));
        Path output = Path.of("target/grn-211-qa/sample-report.pdf");
        Files.createDirectories(output.getParent());
        Files.write(output, pdf);
    }

    @Test
    void templateAliasKeepsTheCompleteContractAfterSeeding() throws Exception {
        assertEquals("211-invrptgoodsreceiptsnotesriceslip", pdfs.keyOf(TEMPLATE));
        assertTrue(registry.isHandTraced(pdfs.keyOf(TEMPLATE)));
        assertEquals(2, registry.get(pdfs.keyOf(TEMPLATE)).subReports.size());
        mvc.perform(post("/reports/print/by-template/" + TEMPLATE + "/pdf")
                        .contentType("application/json").content("{\"id\":20,\"documentTypeId\":217}"))
                .andExpect(status().isOk()).andExpect(content().contentType("application/pdf"));
        assertEquals(List.of(78, 79, 20, 217), bindings.get(0));
        assertEquals(List.of(78, 79, "101", 217, 20, "0"), bindings.get(2));
    }

    @Test
    void legacyKeyDefaultsUnsetDocumentTypeTo46ForBothMainAndWages() {
        for (Map<String, Object> args : List.of(Map.<String, Object>of("id",20), Map.<String, Object>of("id",20,"documentTypeId",0))) {
            queries.clear(); bindings.clear();
            data.run("grn-211", args);
            assertEquals(3, queries.size());
            assertEquals(List.of(78,79,20,46), bindings.get(0));
            assertEquals(List.of(20), bindings.get(1));
            assertEquals(List.of(78,79,"101",46,20,"0"), bindings.get(2));
        }
    }

    @Test
    void missingRowIdDoesNotExecuteAReportQuery() throws Exception {
        mvc.perform(post(URL).contentType("application/json").content("{}"))
                .andExpect(status().isBadRequest());
        assertTrue(queries.isEmpty());
    }
}
