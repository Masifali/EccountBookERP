package com.mst.repositories;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import org.junit.jupiter.api.Test;
import org.springframework.dao.DataAccessResourceFailureException;
import org.springframework.jdbc.core.JdbcTemplate;
import com.mst.repositories.GeneralLedgerPrintRepository.Format;

class GeneralLedgerPrintRepositoryTest {
    private final LocalDate from = LocalDate.of(2026, 8, 1);
    private final LocalDate to = LocalDate.of(2026, 8, 31);

    @Test void allFiveActionsUseDesktopProcedureAndDateParameters() {
        List<String> statements = new ArrayList<>();
        List<Map<String, Object>> rows = List.of(Map.of("DebitAmount", new BigDecimal("123.4500")));
        JdbcTemplate jdbc = mock(JdbcTemplate.class, invocation -> {
            if (invocation.getMethod().getName().equals("queryForList")) {
                statements.add(invocation.getArgument(0));
                return rows;
            }
            return RETURNS_DEFAULTS.answer(invocation);
        });
        GeneralLedgerPrintRepository repository = new GeneralLedgerPrintRepository(jdbc);
        for (Format format : Format.values()) {
            assertSame(rows, repository.load(format, 78, 78, 58, 77, 22400, from, to, false, null));
        }
        for (int i = 0; i < 3; i++) {
            assertTrue(statements.get(i).startsWith("EXEC dbo.Sp_Accounts_GeneralLedger_Rpt "));
            assertTrue(statements.get(i).contains("@EndDate=?"));
            assertTrue(statements.get(i).contains("@EntryUser=?"));
        }
        assertTrue(statements.get(3).startsWith("EXEC dbo.Sp_VouchersAccountsGeneralLedgerSummery "));
        assertTrue(statements.get(3).contains("@VoucherDateF=?, @VoucherDateT=?"));
        assertFalse(statements.get(3).contains("@EntryUser"));
        assertTrue(statements.get(4).startsWith("EXEC dbo.SpAccounts_GeneralLedger2Format_Rpt "));
        assertTrue(statements.get(4).contains("@FromDate=?, @ToDate=?"));
        assertFalse(statements.get(4).contains("@FinancialYearId"));
    }

    @Test void includeUnpostedOmitsApprovalAndFailureNeverFallsBack() {
        List<String> calls = new ArrayList<>();
        JdbcTemplate jdbc = mock(JdbcTemplate.class, invocation -> {
            calls.add(invocation.getArgument(0));
            throw new DataAccessResourceFailureException("Database unavailable");
        });
        GeneralLedgerPrintRepository repository = new GeneralLedgerPrintRepository(jdbc);
        assertThrows(DataAccessResourceFailureException.class,
                () -> repository.load(Format.STANDARD, 78, 78, 58, 77, 22400, from, to, true, null));
        assertEquals(1, calls.size());
        assertFalse(calls.get(0).contains("@IsApproved"));
    }

    @Test void invalidContextAndDatesCannotExecuteSql() {
        JdbcTemplate jdbc = mock(JdbcTemplate.class);
        GeneralLedgerPrintRepository repository = new GeneralLedgerPrintRepository(jdbc);
        assertThrows(IllegalArgumentException.class,
                () -> repository.load(Format.STANDARD, 0, 78, 58, 77, 22400, from, to, false, null));
        assertThrows(IllegalArgumentException.class,
                () -> repository.load(Format.STANDARD, 78, 78, 58, 77, 22400, to, from, false, null));
        assertThrows(IllegalArgumentException.class, () -> Format.fromCode("105;DROP"));
        verifyNoInteractions(jdbc);
    }
}
