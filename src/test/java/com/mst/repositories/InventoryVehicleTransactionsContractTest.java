package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryVehicleTransactionsRequest;
import com.mst.services.InventoryVehicleTransactionsService;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class InventoryVehicleTransactionsContractTest {
    @Test void optionalDesktopFiltersAreNullAndDoNotAddInventedBranchParameters(){
        var jdbc=mock(JdbcTemplate.class);var repository=new InventoryVehicleTransactionsRepository(jdbc);var u=new UserAccount();u.setOrganizationId(78);u.setCompanyId(78);u.setBranchesId(90);
        var r=new InventoryVehicleTransactionsRequest();r.setFromDate(LocalDate.of(2026,9,1));r.setToDate(LocalDate.of(2026,9,8));r.setItemId(3);r.setWarehouseId(0);r.setDocumentTypeIds("5");r.setCropYear("");
        repository.load(u,r);
        verify(jdbc).queryForList("EXEC dbo.USP_GetStockEvalautionDetailByRefRefIds @OrganizationId=?, @CompanyId=?, @DateFrom=?, @DateTo=?, @ItemId=?, @ReferenceDocumentTypeId=?",78,78,java.sql.Date.valueOf(r.getFromDate()),java.sql.Date.valueOf(r.getToDate()),3,5);
        verifyNoMoreInteractions(jdbc);
    }
    @Test void documentsPreserveTheRecoveredScalarConversionIncludingItsKnownMultipleCheckDefect(){
        assertEquals(5,InventoryVehicleTransactionsRepository.desktopDocumentType("5"));assertNull(InventoryVehicleTransactionsRepository.desktopDocumentType("5,8"));assertNull(InventoryVehicleTransactionsRepository.desktopDocumentType(""));
    }
    @Test void projectionUsesWarehouseAndLotCodesAndPreservesProcedureValues(){
        var raw=Map.<String,Object>of("WareHouseCode","WH-7","JobLotCode","LOT-3","AVgRate","12.345","AmountIn","987654321098765.123");
        var row=InventoryVehicleTransactionsService.project(List.of(raw)).get(0);assertEquals("WH-7",row.get("Warehouse"));assertEquals("LOT-3",row.get("JobLot"));assertEquals("12.345",row.get("AvgRate"));assertEquals(raw.get("AmountIn"),row.get("AmountIn"));assertFalse(raw.containsKey("Warehouse"));
    }
    @Test void aDeniedUserCannotReadAnyLookup(){
        var repository=mock(InventoryVehicleTransactionsRepository.class);var context=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var user=new UserAccount();when(context.requireAccountingUser()).thenReturn(user);doThrow(new AccessDeniedException("Denied")).when(rights).require(user,291,"View");
        assertThrows(AccessDeniedException.class,()->new InventoryVehicleTransactionsService(repository,context,rights).lookups());verifyNoInteractions(repository);
    }
}
