package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.PreparedStatement;
import java.time.LocalDate;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import static org.mockito.Mockito.*;
import static org.mockito.ArgumentMatchers.*;

class PayablesInvoiceWiseContractTest {
    @Test void uncheckedFiltersAreOmittedAsInDesktopBll() throws Exception {
        JdbcTemplate jdbc=mock(JdbcTemplate.class);
        PreparedStatement stmt=mock(PreparedStatement.class);
        when(stmt.getUpdateCount()).thenReturn(-1);
        when(jdbc.execute(anyString(), any(PreparedStatementCallback.class))).thenAnswer(i ->
            ((PreparedStatementCallback<?>) i.getArgument(1)).doInPreparedStatement(stmt));
        UserAccount user=new UserAccount(); user.setOrganizationId(78); user.setCompanyId(79);
        new PayablesReportRepository(jdbc).invoiceWise(user,null,null,0,0,0,0,0);
        verify(jdbc).execute(eq("EXEC dbo.usp_PayablesReportInvoiceWise @OrganizationId=?, @CompanyId=?"), any(PreparedStatementCallback.class));
        verify(stmt).setObject(1,78); verify(stmt).setObject(2,79);
        verify(stmt,never()).setObject(eq(3),any());
    }
    @Test void selectedFiltersKeepDesktopParameterNamesAndOrder() throws Exception {
        JdbcTemplate jdbc=mock(JdbcTemplate.class);
        PreparedStatement stmt=mock(PreparedStatement.class);
        when(stmt.getUpdateCount()).thenReturn(-1);
        when(jdbc.execute(anyString(), any(PreparedStatementCallback.class))).thenAnswer(i ->
            ((PreparedStatementCallback<?>) i.getArgument(1)).doInPreparedStatement(stmt));
        UserAccount user=new UserAccount();user.setOrganizationId(78);user.setCompanyId(79);
        new PayablesReportRepository(jdbc).invoiceWise(user,LocalDate.of(2026,9,1),LocalDate.of(2026,9,26),5,6,7,8,2);
        verify(jdbc).execute(eq("EXEC dbo.usp_PayablesReportInvoiceWise @OrganizationId=?, @CompanyId=?, @FromDate=?, @ToDate=?, @SupplierCustomerId=?, @CommissionAgentId=?, @ParentAccountId=?, @CustomGruopId=?, @ActionId=?"), any(PreparedStatementCallback.class));
        verify(stmt).setObject(3,java.sql.Date.valueOf("2026-09-01"));
        verify(stmt).setObject(5,5);verify(stmt).setObject(8,8);verify(stmt).setObject(9,2);
    }
}
