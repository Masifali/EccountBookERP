package com.mst.services;
import com.mst.models.*;
import com.mst.repositories.SaleActivitiesReportRepository;
import com.mst.security.*;
import org.junit.jupiter.api.*;
import org.springframework.security.access.AccessDeniedException;
import java.time.LocalDate;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class SaleActivitiesReportServiceTest {
    final SaleActivitiesReportRepository repo=mock(SaleActivitiesReportRepository.class);
    final CurrentUserContext context=mock(CurrentUserContext.class);final DesktopReportRights rights=mock(DesktopReportRights.class);
    final UserAccount user=new UserAccount();final SaleActivitiesReportService service=new SaleActivitiesReportService(repo,context,rights);
    @BeforeEach void setup(){user.setId(78);user.setCompanyId(58);when(context.requireAccountingUser()).thenReturn(user);when(repo.screenId()).thenReturn(481);when(repo.branches(user)).thenReturn(List.of(Map.of("BranchId",58)));}
    SaleActivitiesReportFilter filter(String activity,List<Integer> branches,List<Integer> groups,LocalDate from){return new SaleActivitiesReportFilter(from,LocalDate.of(2026,10,4),0,0,0,0,0,0,0,null,0,0,0,0,0,0,0,0,0,0,0,activity,branches,groups);}
    @Test void showAndPrintRowsRequireViewAndTheUsersBranch(){var f=filter("Sales Register",List.of(58,58),List.of(),LocalDate.of(2026,9,1));when(repo.rows(user,f,",58","")).thenReturn(List.of());assertTrue(service.reportRows(f).isEmpty());verify(rights).require(user,481,"View");verify(repo).rows(user,f,",58","");}
    @Test void rejectsOtherBranchesAndInvalidDatesBeforeQuery(){assertThrows(AccessDeniedException.class,()->service.reportRows(filter("Sales Register",List.of(59),List.of(),LocalDate.of(2026,9,1))));assertThrows(IllegalArgumentException.class,()->service.reportRows(filter("Sales Register",List.of(58),List.of(),LocalDate.of(2026,10,5))));verify(repo,never()).rows(any(),any(),any(),any());}
    @Test void validatesCustomGroupsAndDeduplicatesTheirIds(){when(repo.lookups(user,",58")).thenReturn(List.of(Map.of("Activity","GetCustomGroups","Id",7)));var f=filter("Sales Summary By Item",List.of(58),List.of(7,7),LocalDate.of(2026,9,1));when(repo.rows(user,f,",58",",7")).thenReturn(List.of());assertEquals("316-SalesRegisterSummaryByItemWithoutPacking",service.rows(f).get("printLabel"));verify(repo).rows(user,f,",58",",7");assertThrows(IllegalArgumentException.class,()->service.reportRows(filter("Sales Register",List.of(58),List.of(8),LocalDate.of(2026,9,1))));}
    @Test void rejectsUnknownActivities(){assertThrows(IllegalArgumentException.class,()->service.reportRows(filter("arbitrary.rpt",List.of(58),List.of(),LocalDate.of(2026,9,1))));verify(repo,never()).rows(any(),any(),any(),any());}
    @Test void registerNetRateSubtractsTheCut(){var row=SaleActivitiesReportColumns.project(Map.of("ItemRate",5200.0,"RateCut",50.0),"NetRate:$NetRate");assertEquals(5150.0,row.get("NetRate"));}
}
