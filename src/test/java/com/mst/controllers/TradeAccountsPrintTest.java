package com.mst.controllers;

import com.mst.controllers.ERPPrint.AccountsPrintController;
import com.mst.models.Company;
import com.mst.models.UserAccount;
import com.mst.repositories.ICompanyRepository;
import com.mst.repositories.TradeReportRepository;
import com.mst.reports.jasper.ReportTemplateService;
import com.mst.security.CurrentUserContext;
import com.mst.services.GeneralLedgerSummaryService;
import com.mst.services.TradeReportService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import java.math.BigDecimal;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.*;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class TradeAccountsPrintTest {
    @TempDir Path templates;
    private final CurrentUserContext context = mock(CurrentUserContext.class);
    private final Map<String,Object> sqlParams = new LinkedHashMap<>();
    private String sql;
    private List<Map<String,Object>> rows = List.of(
            Map.of("AccountTitle","Sample Karachi Account","CityName","Karachi","Closing",new BigDecimal("2500.50")),
            Map.of("AccountTitle","Sample Faisalabad Account","CityName","Faisalabad","Closing",new BigDecimal("1200.25")));
    private JdbcTemplate jdbc;
    private MockMvc mvc;

    @BeforeEach void setup() throws Exception {
        UserAccount user = new UserAccount();
        user.setId(12); user.setOrganizationId(78); user.setCompanyId(79); user.setAppId(2);
        when(context.currentUserId()).thenReturn(12);
        when(context.currentCompanyId()).thenReturn(79);
        when(context.requireAccountingUser()).thenReturn(user);
        jdbc = mock(JdbcTemplate.class, invocation -> {
            if (!invocation.getMethod().getName().equals("queryForList")) return RETURNS_DEFAULTS.answer(invocation);
            sql = invocation.getArgument(0);
            var matcher = java.util.regex.Pattern.compile("@(\\w+)=\\?").matcher(sql);
            int index = 1;
            while (matcher.find()) sqlParams.put(matcher.group(1), invocation.getArguments()[index++]);
            return rows.stream().map(LinkedHashMap::new).collect(java.util.stream.Collectors.toList());
        });
        var companies = mock(ICompanyRepository.class);
        Company company = new Company(); company.setCompName("Test Company"); company.setCompAddress("Sample Address");
        when(companies.findById(79)).thenReturn(Optional.of(company));
        var controller = new AccountsPrintController(mock(GeneralLedgerSummaryService.class));
        ReflectionTestUtils.setField(controller,"currentUserContext",context);
        ReflectionTestUtils.setField(controller,"tradeReportService",new TradeReportService(new TradeReportRepository(jdbc),context));
        ReflectionTestUtils.setField(controller,"companyRepository",companies);
        ReflectionTestUtils.setField(controller,"reportTemplates",new ReportTemplateService(templates.toString()));
        mvc = MockMvcBuilders.standaloneSetup(controller).build();
    }

    @Test void bothPrintsUseTradeProcedureAndLoadedFiltersAndProducePdf() throws Exception {
        for (boolean cityWise : List.of(false,true)) {
            byte[] pdf = mvc.perform(get("/reports/print/trade-accounts")
                    .param("fromDate","2026-08-01").param("toDate","2026-10-04")
                    .param("cityWise",String.valueOf(cityWise)).param("accountClass","2")
                    .param("controlAccounts","41,42").param("accountId","123").param("branches","58,59")
                    .param("credit","false").param("debit","true").param("approved","true")
                    .param("balanceFrom","123.45").param("cityId","7").param("costCenterId","8")
                    .param("status","227 records").param("companyId","999").param("organizationId","999"))
                    .andExpect(status().isOk()).andExpect(content().contentType("application/pdf"))
                    .andExpect(header().string("Content-Disposition",org.hamcrest.Matchers.containsString(cityWise?"122A_CityWise":"122A_AcRpt")))
                    .andReturn().getResponse().getContentAsByteArray();
            assertTrue(sql.startsWith("EXEC dbo.SpAccounts_TradeDebtorsAndCreditors_Report "));
            assertEquals("41,42",sqlParams.get("ParentAccountCode"));
            assertFalse(sqlParams.containsValue("227 records"));
            assertEquals(78,sqlParams.get("OrganizationId")); assertEquals(79,sqlParams.get("CompanyId"));
            assertEquals(2,sqlParams.get("AccouuntClassId")); assertEquals(123,sqlParams.get("CoaDetailAccountId"));
            assertEquals("58,59",sqlParams.get("BranchesIds")); assertEquals(2,sqlParams.get("ActionId"));
            assertEquals(true,sqlParams.get("IsApproved")); assertEquals(new BigDecimal("123.45"),sqlParams.get("BalanceFrom"));
            assertEquals(7,sqlParams.get("CityId")); assertEquals(8,sqlParams.get("CostCenterId"));
            var reader = new com.itextpdf.text.pdf.PdfReader(pdf);
            StringBuilder text = new StringBuilder();
            try { for(int page=1;page<=reader.getNumberOfPages();page++) text.append(com.itextpdf.text.pdf.parser.PdfTextExtractor.getTextFromPage(reader,page)); }
            finally { reader.close(); }
            assertTrue(text.toString().contains("Test Company"));
            assertTrue(text.toString().contains("Sample Karachi Account"));
            assertTrue(text.toString().contains("Sample Faisalabad Account"));
            if(cityWise) assertTrue(text.indexOf("Sample Faisalabad Account") < text.indexOf("Sample Karachi Account"));
            Files.createDirectories(Path.of("target/ui-verification"));
            Files.write(Path.of("target/ui-verification/trade-print-"+(cityWise?"city":"standard")+"-sample.pdf"),pdf);
        }
    }

    @Test void emptyOrInvalidReportsDoNotProducePdf() throws Exception {
        rows = List.of();
        mvc.perform(get("/reports/print/trade-accounts").param("fromDate","2026-08-01").param("toDate","2026-10-04"))
                .andExpect(status().isNotFound()).andExpect(content().string("No Record Found For Display"));
        clearInvocations(jdbc);
        mvc.perform(get("/reports/print/trade-accounts").param("fromDate","2026-10-05").param("toDate","2026-10-04"))
                .andExpect(status().isBadRequest());
        verifyNoInteractions(jdbc);
        when(context.currentUserId()).thenThrow(new AccessDeniedException("Sign in"));
        mvc.perform(get("/reports/print/trade-accounts")).andExpect(status().isForbidden());
        verifyNoInteractions(jdbc);
    }
}
