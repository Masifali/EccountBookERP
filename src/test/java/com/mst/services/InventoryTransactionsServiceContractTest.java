package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryTransactionsRequest;
import com.mst.repositories.InventoryTransactionsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import org.junit.jupiter.api.Test;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.server.ResponseStatusException;
import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;

class InventoryTransactionsServiceContractTest {
    @Test void missingDesktopCompanyPermissionNeverQueriesReport(){
        var repository=mock(InventoryTransactionsRepository.class);var context=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var user=new UserAccount();
        when(context.requireAccountingUser()).thenReturn(user);doThrow(new AccessDeniedException("Not enabled")).when(rights).require(user,580,"View");
        var service=new InventoryTransactionsService(repository,context,rights);
        assertThrows(AccessDeniedException.class,service::lookups);
        assertThrows(AccessDeniedException.class,()->service.load(new InventoryTransactionsRequest()));
        verifyNoInteractions(repository);
    }
    @Test void reversedDatesAndInvalidSelectionsNeverQueryReport(){
        var repository=mock(InventoryTransactionsRepository.class);var context=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var user=new UserAccount();
        when(context.requireAccountingUser()).thenReturn(user);var service=new InventoryTransactionsService(repository,context,rights);
        var request=new InventoryTransactionsRequest();request.setFromDate(LocalDate.of(2026,9,15));request.setToDate(LocalDate.of(2026,9,1));
        assertThrows(ResponseStatusException.class,()->service.load(request));
        request.setToDate(LocalDate.of(2026,9,15));request.setWarehouseIds("1,invalid");
        assertThrows(ResponseStatusException.class,()->service.load(request));verifyNoInteractions(repository);
    }
}
