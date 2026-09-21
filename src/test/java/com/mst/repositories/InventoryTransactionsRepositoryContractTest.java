package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryTransactionsRequest;
import java.time.LocalDate;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.dao.DataAccessResourceFailureException;
import static org.mockito.Mockito.*;
import static org.mockito.ArgumentMatchers.*;
import static org.junit.jupiter.api.Assertions.*;

class InventoryTransactionsRepositoryContractTest {
    private UserAccount user(){UserAccount u=new UserAccount();u.setOrganizationId(2);u.setCompanyId(3);return u;}
    @Test void usesDesktopProcedureDatesAndMultipleSelections(){
        JdbcTemplate jdbc=mock(JdbcTemplate.class);
        InventoryTransactionsRequest r=new InventoryTransactionsRequest();r.setFromDate(LocalDate.of(2026,9,1));r.setToDate(LocalDate.of(2026,9,15));r.setItemTypeIds("4,6");r.setWarehouseIds("7,9");
        new InventoryTransactionsRepository(jdbc).load(user(),r);
        verify(jdbc).queryForList("EXEC [pcc].[USP-InventoryStockTransactionsReport] @OrganizationId=?, @CompanyId=?, @DateFrom=?, @DateTo=?, @ItemTypeIds=?, @WarehouseIds=?",2,3,java.sql.Date.valueOf(r.getFromDate()),java.sql.Date.valueOf(r.getToDate()),"4,6","7,9");
        verifyNoMoreInteractions(jdbc);
    }
    @Test void optionalZeroFiltersAreOmittedAndDatabaseFailureIsVisible(){
        JdbcTemplate jdbc=mock(JdbcTemplate.class);
        when(jdbc.queryForList(anyString(),eq(2),eq(3))).thenThrow(new DataAccessResourceFailureException("offline"));
        InventoryTransactionsRequest r=new InventoryTransactionsRequest();r.setItemId(0);r.setWarehouseIds("");
        assertThrows(DataAccessResourceFailureException.class,()->new InventoryTransactionsRepository(jdbc).load(user(),r));
        verify(jdbc).queryForList("EXEC [pcc].[USP-InventoryStockTransactionsReport] @OrganizationId=?, @CompanyId=?",2,3);verifyNoMoreInteractions(jdbc);
    }
}
