package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.DesktopReceivablesRequest;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.dao.DataAccessResourceFailureException;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
import static org.mockito.ArgumentMatchers.*;

class DesktopReceivablesRepositoryContractTest {
    private UserAccount user() { UserAccount u=new UserAccount();u.setId(7);u.setOrganizationId(2);u.setCompanyId(3);return u; }
    @Test void dueDatesUseDesktopProcedureAndAccountClass() {
        JdbcTemplate jdbc=mock(JdbcTemplate.class);
        
        DesktopReceivablesRequest r=new DesktopReceivablesRequest();r.setToDate(LocalDate.of(2026,9,15));r.setParentId(42);
        assertTrue(new DesktopReceivablesRepository(jdbc).load("receivables-by-due-dates",user(),r).isEmpty());
        verify(jdbc).queryForList(eq("EXEC dbo.Usp_ReceivablesByDueDates @OrganizationId=?, @CompanyId=?, @UserId=?, @ToDate=?, @AccouuntClassId=?, @ActionId=?, @ParentAccountId=?"),eq(2),eq(3),eq(7),eq(java.sql.Date.valueOf(r.getToDate())),eq(2),eq(1),eq(42));
        verifyNoMoreInteractions(jdbc);
    }
    @Test void schedulePreservesOptionalDatesAndSelectedControlAccounts() {
        JdbcTemplate jdbc=mock(JdbcTemplate.class);
        
        DesktopReceivablesRequest r=new DesktopReceivablesRequest();r.setControls("12,14");r.setSaleFrom(LocalDate.of(2026,9,1));
        new DesktopReceivablesRepository(jdbc).load("receivables-receipt-schedule",user(),r);
        verify(jdbc).queryForList(eq("EXEC dbo.usp_ReceivablesAndReceiptsSchedule @OrganizationId=?, @CompanyId=?, @UserId=?, @ParentCategoryId=?, @SaleFromDate=?, @ControlAccountIds=?"),eq(2),eq(3),eq(7),eq(0),eq(java.sql.Date.valueOf(r.getSaleFrom())),eq("12,14"));
    }
    @Test void databaseFailureDoesNotBecomeAnUnrelatedReport() {
        JdbcTemplate jdbc=mock(JdbcTemplate.class);
        when(jdbc.queryForList(anyString(),eq(2),eq(3),eq(7),eq(2),eq(1))).thenThrow(new DataAccessResourceFailureException("offline"));
        assertThrows(DataAccessResourceFailureException.class,()->new DesktopReceivablesRepository(jdbc).load("receivables-by-due-dates",user(),new DesktopReceivablesRequest()));
        verify(jdbc).queryForList(startsWith("EXEC dbo.Usp_ReceivablesByDueDates "),eq(2),eq(3),eq(7),eq(2),eq(1));verifyNoMoreInteractions(jdbc);
    }
}
