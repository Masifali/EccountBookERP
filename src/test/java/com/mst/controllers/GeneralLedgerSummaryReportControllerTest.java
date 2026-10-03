package com.mst.controllers;

import com.mst.models.GeneralLedgerSummaryRequest;
import com.mst.controllers.ERPPrint.AccountsPrintController;
import com.mst.services.GeneralLedgerSummaryService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import java.math.BigDecimal;
import java.nio.charset.StandardCharsets;
import java.sql.Date;
import java.time.LocalDate;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

class GeneralLedgerSummaryReportControllerTest {
    private final GeneralLedgerSummaryService service = mock(GeneralLedgerSummaryService.class);
    private MockMvc mvc;

    @BeforeEach
    void setUp() {
        mvc = MockMvcBuilders.standaloneSetup(new AccountsPrintController(service)).build();
    }

    @Test
    void browserRequestCompilesExistingTemplateAndExportsPdf() throws Exception {
        Map<String, Object> row = new HashMap<>();
        row.put("CompName", "Ledger Test Company");
        row.put("CompAddress", "Test Address");
        row.put("VoucherDate", Date.valueOf("2026-08-01"));
        row.put("VoucherCode", "JV-001");
        row.put("DebitAmount", new BigDecimal("123.45"));
        row.put("CreditAmount", BigDecimal.ZERO);
        row.put("RunningBalance", new BigDecimal("123.45"));
        when(service.getGeneralLedgerSummary(any())).thenReturn(List.of(row));

        byte[] pdf = mvc.perform(get("/reports/general-ledger-summary")
                        .param("accountId", "23967").param("fromDate", "2026-08-01").param("toDate", "2026-10-03"))
                .andExpect(status().isOk())
                .andExpect(content().contentType("application/pdf"))
                .andExpect(header().string("Content-Disposition", "inline; filename=\"108-GeneralLedgerSummary.pdf\""))
                .andReturn().getResponse().getContentAsByteArray();
        assertTrue(pdf.length > 1000);
        assertEquals("%PDF-", new String(pdf, 0, 5, StandardCharsets.US_ASCII));
        var request = ArgumentCaptor.forClass(GeneralLedgerSummaryRequest.class);
        verify(service).getGeneralLedgerSummary(request.capture());
        assertEquals(23967, request.getValue().getAccountId());
        assertEquals(LocalDate.of(2026, 8, 1), request.getValue().getFromDate());
        assertTrue(request.getValue().isIncludeUnposted());
    }

    @Test
    void postBindsExplicitFiltersAndReturnsNotFoundForEmptyReport() throws Exception {
        when(service.getGeneralLedgerSummary(any())).thenReturn(List.of());
        mvc.perform(post("/reports/general-ledger-summary").contentType(MediaType.APPLICATION_JSON).content("""
                {"accountId":23967,"fromDate":"2026-08-01","toDate":"2026-10-03",
                 "includeUnposted":false,"branchId":4,"subsidiaryAccountId":8,"subsidiaryTypeId":2,
                 "costCenterId":6,"languageId":1,"companyId":999,"organizationId":999}
                """))
                .andExpect(status().isNotFound());
        var request = ArgumentCaptor.forClass(GeneralLedgerSummaryRequest.class);
        verify(service).getGeneralLedgerSummary(request.capture());
        assertFalse(request.getValue().isIncludeUnposted());
        assertEquals(4, request.getValue().getBranchId());
        assertEquals(8, request.getValue().getSubsidiaryAccountId());
        assertEquals(2, request.getValue().getSubsidiaryTypeId());
        assertEquals(6, request.getValue().getCostCenterId());
        assertEquals(1, request.getValue().getLanguageId());
    }

    @Test
    void invalidAccountDatesAndFiltersDoNotLoadData() throws Exception {
        for (String query : List.of(
                "fromDate=2026-08-01&toDate=2026-10-03",
                "accountId=0&fromDate=2026-08-01&toDate=2026-10-03",
                "accountId=23967&fromDate=2026-10-03&toDate=2026-08-01",
                "accountId=23967&fromDate=not-a-date&toDate=2026-10-03",
                "accountId=23967&fromDate=2026-08-01",
                "accountId=23967&fromDate=2026-08-01&toDate=2026-10-03&branchId=-1")) {
            mvc.perform(get("/reports/general-ledger-summary?" + query)).andExpect(status().isBadRequest());
        }
        mvc.perform(post("/reports/general-ledger-summary").contentType(MediaType.APPLICATION_JSON).content("""
                {"accountId":23967,"fromDate":"2026-10-03","toDate":"2026-08-01"}
                """)).andExpect(status().isBadRequest());
        verifyNoInteractions(service);
    }
}
