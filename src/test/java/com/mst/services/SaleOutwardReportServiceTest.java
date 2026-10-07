package com.mst.services;

import com.mst.models.SaleOutwardReportColumns;
import com.mst.models.SaleOutwardReportFilter;
import com.mst.models.UserAccount;
import com.mst.repositories.SaleOutwardReportRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.security.access.AccessDeniedException;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class SaleOutwardReportServiceTest {
    private final SaleOutwardReportRepository repository = mock(SaleOutwardReportRepository.class);
    private final CurrentUserContext context = mock(CurrentUserContext.class);
    private final DesktopReportRights rights = mock(DesktopReportRights.class);
    private final UserAccount user = new UserAccount();
    private SaleOutwardReportService service;

    @BeforeEach void setup() {
        user.setId(42);
        user.setOrganizationId(78);
        user.setCompanyId(78);
        user.setBranchesId(58);
        when(context.requireAccountingUser()).thenReturn(user);
        when(context.currentFinancialYearId()).thenReturn(2026);
        when(repository.screenId()).thenReturn(484);
        when(repository.branches(user)).thenReturn(List.of(Map.of("BranchId", 58, "BranchName", "Seed Branch")));
        when(repository.lookups(user, null)).thenReturn(List.of());
        when(repository.lookups(user, ",58")).thenReturn(List.of());
        when(repository.rows(eq(user), any(), anyString())).thenReturn(List.of());
        service = new SaleOutwardReportService(repository, context, rights);
    }

    @Test void initialAndLookupsUseTheSeededBranchAndDesktopColumnList() {
        when(repository.yearStart(user, 2026)).thenReturn("2026-07-01");

        var initial = service.initial();
        assertEquals(58, initial.get("branchId"));
        assertEquals("2026-07-01", initial.get("yearStart"));
        assertEquals(SaleOutwardReportColumns.names(), initial.get("columns"));
        assertEquals("Seed Branch", ((Map<?, ?>) ((List<?>) initial.get("branches")).get(0)).get("BranchName"));

        assertTrue(service.lookups(null).isEmpty());
        assertTrue(service.lookups(List.of(58)).isEmpty());
        verify(repository).lookups(user, null);
        verify(repository).lookups(user, ",58");
        verify(rights, times(3)).require(user, 484, "View");
    }

    @Test void rowsValidateDatesStatusAndAllocatedBranchesBeforeQuerying() {
        var valid = new SaleOutwardReportFilter(LocalDate.of(2026, 9, 1), LocalDate.of(2026, 10, 6),
                0, 0, 0, "", "Open", List.of(58, 58), false);
        assertTrue(service.rows(valid).isEmpty());
        verify(repository).rows(user, valid, ",58");

        var missingDates = new SaleOutwardReportFilter(null, null, 0, 0, 0, "", "", List.of(58), false);
        assertThrows(IllegalArgumentException.class, () -> service.rows(missingDates));
        var invalidStatus = new SaleOutwardReportFilter(valid.fromDate(), valid.toDate(), 0, 0, 0, "", "Void", List.of(58), false);
        assertThrows(IllegalArgumentException.class, () -> service.rows(invalidStatus));
        var unallocated = new SaleOutwardReportFilter(valid.fromDate(), valid.toDate(), 0, 0, 0, "", "", List.of(59), false);
        assertThrows(AccessDeniedException.class, () -> service.rows(unallocated));
        var missingBranch = new SaleOutwardReportFilter(valid.fromDate(), valid.toDate(), 0, 0, 0, "", "", List.of(), false);
        assertThrows(IllegalArgumentException.class, () -> service.rows(missingBranch));
        verify(repository, times(1)).rows(eq(user), any(), anyString());
    }
}
