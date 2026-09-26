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

class PayablesDesktopProcedureContractTest {
    private UserAccount user(){UserAccount u=new UserAccount();u.setId(7);u.setOrganizationId(2);u.setCompanyId(3);u.setAppId(1);return u;}
    @Test void emptySupplierAgingDoesNotSwitchToReceivablesOrInventBuckets(){
        JdbcTemplate jdbc=mock(JdbcTemplate.class);List<List<Map<String,Object>>> sets=List.of(List.of(),List.of(),List.of(),List.of(),List.of());
        doReturn(sets).when(jdbc).execute(anyString(),any(PreparedStatementCallback.class));
        assertSame(sets,new PayablesAgingRepository(jdbc).load(user(),LocalDate.of(2026,9,15),30,0,0,0,0,0,""));
        verify(jdbc).execute(eq("EXEC dbo.UPS_PayablesAging_New @OrganizationId=?, @CompanyId=?, @AsOnDate=?, @AgingDays=?, @UserId=?, @AccouuntClassId=?, @Activity=?"),any(PreparedStatementCallback.class));verifyNoMoreInteractions(jdbc);
    }
    @Test void paymentScheduleUsesDesktopProcedureAndOptionalDateParameters(){
        JdbcTemplate jdbc=mock(JdbcTemplate.class);doReturn(List.of()).when(jdbc).execute(anyString(),any(PreparedStatementCallback.class));
        new PayablesPaymentScheduleRepository(jdbc).load(user(),null,null,LocalDate.of(2026,9,1),null,null,null,30,30,0,0,0,0,0,0,"12,14","");
        verify(jdbc).execute(eq("EXEC dbo.usp_PayablesAndPaymentSchedule @OrganizationId=?, @CompanyId=?, @UserId=?, @ParentCategoryId=?, @DueDateFrom=?, @AgingDays=?, @ControlAccountIds=?"),any(PreparedStatementCallback.class));verifyNoMoreInteractions(jdbc);
    }
    @Test void supplierFailurePropagates(){
        JdbcTemplate jdbc=mock(JdbcTemplate.class);doThrow(new DataAccessResourceFailureException("offline")).when(jdbc).execute(anyString(),any(PreparedStatementCallback.class));
        assertThrows(DataAccessResourceFailureException.class,()->new PayablesAgingRepository(jdbc).load(user(),LocalDate.of(2026,9,15),30,0,0,0,0,0,""));
        verify(jdbc).execute(anyString(),any(PreparedStatementCallback.class));verifyNoMoreInteractions(jdbc);
    }
}
