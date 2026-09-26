package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLedgerRequest;
import com.mst.services.InventoryItemLedgerService;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class InventoryItemLedgerContractTest {
    @Test void usesExactDesktopParametersAndEvaluationItems(){
        JdbcTemplate jdbc=mock(JdbcTemplate.class);var repository=new InventoryItemLedgerRepository(jdbc);var user=new UserAccount();user.setOrganizationId(78);user.setCompanyId(78);user.setId(85);user.setBranchesId(90);
        var request=new InventoryItemLedgerRequest();request.setItemId(7);request.setFromDate(LocalDate.of(2026,9,1));request.setToDate(LocalDate.of(2026,9,16));
        repository.items(user);repository.load(user,request);
        verify(jdbc).queryForList("EXEC dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId=?, @CompanyId=?, @ActivityType=?",78,78,"Items");
        verify(jdbc).queryForList("EXEC dbo.usp_ItemLedgerFromStockEvaluations @OrganizationId=?, @CompanyId=?, @ItemId=?, @FromDate=?, @ToDate=?",78,78,7,java.sql.Date.valueOf(request.getFromDate()),java.sql.Date.valueOf(request.getToDate()));
        verifyNoMoreInteractions(jdbc);
    }
    @Test void openingAndClosingUseEndpointRowsWithoutRecalculatingSqlBalances(){
        Map<String,Object> opening=new LinkedHashMap<>(Map.of("BalQty","100","BalWeight","1000","BalAmount","123456789012345.125","AvgRate","12.3456","TrancDate","1900-01-01","RateUom","KG","PackTypeDesc","Bag"));
        Map<String,Object> closing=new LinkedHashMap<>(Map.of("BalQty","87","BalWeight","870","BalAmount","123456789012000.125","AvgRate","12.5","TrancDate","2026-09-15","RateUom","KG"));
        Map<String,Object> result=InventoryItemLedgerService.project(List.of(opening,closing));
        assertEquals("100",((Map<?,?>)result.get("opening")).get("BalQty"));assertEquals("87",((Map<?,?>)result.get("closing")).get("BalQty"));
        assertEquals("123456789012345.125",((Map<?,?>)result.get("opening")).get("BalAmount"));
        var row=(Map<?,?>)((List<?>)result.get("rows")).get(0);assertEquals("1900-01-01",row.get("TranDate"));assertEquals("KG",row.get("RateUomIn"));assertEquals("KG",row.get("RateUomOut"));assertEquals("Bag",row.get("PackingType"));assertFalse(opening.containsKey("TranDate"));
    }
    @Test void emptyReportReturnsZeroSummary(){var result=InventoryItemLedgerService.project(List.of());assertTrue(((List<?>)result.get("rows")).isEmpty());assertEquals("0",((Map<?,?>)result.get("closing")).get("BalAmount"));}
}
