package com.mst.repositories;

import com.mst.models.UserAccount;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.dao.DataAccessResourceFailureException;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.mockito.ArgumentMatchers.*;

class ReceivablesAgingRepositoryContractTest {
    private UserAccount user() {
        UserAccount user = new UserAccount();
        user.setId(7); user.setOrganizationId(2); user.setCompanyId(3); user.setAppId(1);
        return user;
    }

    @Test void emptyDesktopResultsAreNotReplacedByOtherReports() {
        JdbcTemplate jdbc = mock(JdbcTemplate.class);
        List<List<Map<String,Object>>> empty = Arrays.asList(List.of(),List.of(),List.of(),List.of(),List.of());
        doReturn(empty).when(jdbc).execute(anyString(), any(PreparedStatementCallback.class));
        assertSame(empty, new ReceivablesAgingRepository(jdbc).load(user(),LocalDate.of(2026,9,15),30,1,0,0,0,0,""));
        verify(jdbc).execute(startsWith("EXEC dbo.UPS_ReceivablesAging_New "),any(PreparedStatementCallback.class));
        verifyNoMoreInteractions(jdbc);
    }

    @Test void databaseFailureIsReportedInsteadOfInventingBalances() {
        JdbcTemplate jdbc = mock(JdbcTemplate.class);
        doThrow(new DataAccessResourceFailureException("unavailable")).when(jdbc).execute(anyString(),any(PreparedStatementCallback.class));
        assertThrows(DataAccessResourceFailureException.class,()->new ReceivablesAgingRepository(jdbc).load(user(),LocalDate.of(2026,9,15),30,1,0,0,0,0,""));
        verify(jdbc).execute(startsWith("EXEC dbo.UPS_ReceivablesAging_New "),any(PreparedStatementCallback.class));
        verifyNoMoreInteractions(jdbc);
    }
}
