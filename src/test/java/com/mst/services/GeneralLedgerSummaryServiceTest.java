package com.mst.services;

import com.mst.models.GeneralLedgerSummaryRequest;
import com.mst.security.CurrentUserContext;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;

import java.math.BigDecimal;
import java.sql.Date;
import java.time.LocalDate;
import java.util.Arrays;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class GeneralLedgerSummaryServiceTest {
    private final CurrentUserContext context = mock(CurrentUserContext.class);
    private final List<Map<String, Object>> rows = List.of(Map.of("RunningBalance", new BigDecimal("123.4500")));
    private String sql;
    private Object[] parameters;
    private final JdbcTemplate jdbc = mock(JdbcTemplate.class, invocation -> {
        if (invocation.getMethod().getName().equals("queryForList")) {
            sql = invocation.getArgument(0);
            parameters = Arrays.copyOfRange(invocation.getArguments(), 1, invocation.getArguments().length);
            return rows;
        }
        return RETURNS_DEFAULTS.answer(invocation);
    });
    private final GeneralLedgerSummaryService service = new GeneralLedgerSummaryService(jdbc, context);
    private final GeneralLedgerSummaryRequest request = new GeneralLedgerSummaryRequest();

    @BeforeEach
    void setUp() {
        when(context.currentFinancialYearId()).thenReturn(58);
        when(context.currentOrganizationId()).thenReturn(78);
        when(context.currentCompanyId()).thenReturn(79);
        request.setAccountId(23967);
        request.setFromDate(LocalDate.of(2026, 8, 1));
        request.setToDate(LocalDate.of(2026, 10, 3));
    }

    @Test
    void usesSessionContextAndSummaryProcedureWithoutChangingRows() {
        assertSame(rows, service.getGeneralLedgerSummary(request));
        assertEquals("EXEC dbo.Sp_VouchersAccountsGeneralLedgerSummery "
                + "@FinancialYearId=?, @OrganizationId=?, @CompanyId=?, "
                + "@AccountId=?, @VoucherDateF=?, @VoucherDateT=?", sql);
        assertArrayEquals(new Object[]{58, 78, 79, 23967, Date.valueOf("2026-08-01"), Date.valueOf("2026-10-03")}, parameters);
    }

    @Test
    void selectedFiltersAndPostedOnlyHaveMatchingSqlParameters() {
        request.setIncludeUnposted(false);
        request.setBranchId(4);
        request.setSubsidiaryAccountId(8);
        request.setSubsidiaryTypeId(2);
        request.setCostCenterId(6);
        request.setLanguageId(1);
        service.getGeneralLedgerSummary(request);
        assertTrue(sql.endsWith(", @IsApproved=?, @BranchesId=?, @SubsidiaryAccountId=?, @SubsidiaryTypeId=?, @CostCenterId=?, @LanguageId=?"));
        assertArrayEquals(new Object[]{58, 78, 79, 23967, Date.valueOf("2026-08-01"), Date.valueOf("2026-10-03"),
                true, 4, 8, 2, 6, 1}, parameters);
    }

    @Test
    void zeroFiltersAndSubsidiaryTypeWithoutAccountAreOmitted() {
        request.setBranchId(0);
        request.setSubsidiaryAccountId(0);
        request.setSubsidiaryTypeId(2);
        request.setCostCenterId(0);
        request.setLanguageId(0);
        service.getGeneralLedgerSummary(request);
        assertEquals(6, parameters.length);
        assertFalse(sql.contains("@SubsidiaryTypeId"));
    }

    @Test
    void missingAccountingSessionCannotQueryReport() {
        when(context.currentFinancialYearId()).thenThrow(new AccessDeniedException("Sign in"));
        assertThrows(AccessDeniedException.class, () -> service.getGeneralLedgerSummary(request));
        verifyNoInteractions(jdbc);
    }
}
