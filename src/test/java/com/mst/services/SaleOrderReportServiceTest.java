package com.mst.services;

import com.mst.models.*;
import com.mst.repositories.SaleOrderReportRepository;
import com.mst.security.*;
import org.junit.jupiter.api.*;
import org.springframework.security.access.AccessDeniedException;
import java.time.LocalDate;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class SaleOrderReportServiceTest {
    final SaleOrderReportRepository repo=mock(SaleOrderReportRepository.class);
    final CurrentUserContext context=mock(CurrentUserContext.class);
    final DesktopReportRights rights=mock(DesktopReportRights.class);
    final UserAccount user=new UserAccount();
    final SaleOrderReportService service=new SaleOrderReportService(repo,context,rights);
    @BeforeEach void setup(){user.setId(78);user.setAppId(3);user.setCompanyId(58);user.setOrganizationId(58);when(context.requireAccountingUser()).thenReturn(user);when(repo.screenId()).thenReturn(7);when(repo.branches(user)).thenReturn(List.of(Map.of("BranchId",58)));}
    SaleOrderReportFilter filter(List<Integer> branches,LocalDate from,LocalDate to,int cost){return new SaleOrderReportFilter(from,to,0,0,0,0,0,0,0,0,0,0,cost,"Open","Approve",false,branches,0,null,0,0,0,"Order Register",false);}
    @Test void reportAndPrintDataShareBranchAndViewChecks(){var f=filter(List.of(58,58),LocalDate.of(2026,9,1),LocalDate.of(2026,10,4),0);when(repo.detail(user,f,",58")).thenReturn(List.of());assertTrue(service.detailRows(f).isEmpty());verify(rights).require(user,7,"View");verify(repo).detail(user,f,",58");}
    @Test void rejectsUnallocatedBranchBeforeDataRead(){assertThrows(AccessDeniedException.class,()->service.detailRows(filter(List.of(59),null,LocalDate.now(),0)));verify(repo,never()).detail(any(),any(),anyString());}
    @Test void rejectsUnavailableCostCenter(){assertThrows(AccessDeniedException.class,()->service.detailRows(filter(List.of(58),null,LocalDate.now(),9)));verify(repo,never()).detail(any(),any(),anyString());}
    @Test void rejectsReversedDates(){assertThrows(IllegalArgumentException.class,()->service.detailRows(filter(List.of(58),LocalDate.of(2026,10,5),LocalDate.of(2026,10,4),0)));verify(repo,never()).detail(any(),any(),anyString());}
    @Test void summaryEnforcesTheSameScopeAndUsesTheChosenPrintLabel(){
        var f=filter(List.of(58),LocalDate.of(2026,9,1),LocalDate.of(2026,10,4),0);when(repo.summary(user,f,",58")).thenReturn(List.of());
        var result=service.summary(f);assertEquals("347-OrderRegister",result.get("printLabel"));verify(rights).require(user,7,"View");verify(repo).summary(user,f,",58");
        assertThrows(AccessDeniedException.class,()->service.summaryRows(filter(List.of(59),null,LocalDate.now(),0)));
        verify(repo,times(1)).summary(any(),any(),anyString());
    }
    @Test void groupsByOrderWithoutMergingLinesOrMultiplyingCommission(){
        var f=filter(List.of(58),null,LocalDate.now(),0);var rows=new ArrayList<Map<String,Object>>();
        for(int i=0;i<3;i++){Map<String,Object> row=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);for(String spec:List.of(SaleOrderReportColumns.DETAIL,SaleOrderReportColumns.HEADER,SaleOrderReportColumns.LINES))for(String pair:spec.split(","))row.put(pair.split(":")[1],null);row.put("Id",i<2?1:2);row.put("Amount",i+0.25);row.put("BalWeight",i+5.5);row.put("CommAmount",100.0);row.put("OrderItemId",10+i);rows.add(row);}
        when(repo.detail(user,f,",58")).thenReturn(rows);var result=service.detail(f);
        @SuppressWarnings("unchecked") var headers=(List<Map<String,Object>>)result.get("headers");
        @SuppressWarnings("unchecked") var lines=(Map<Integer,List<Map<String,Object>>>)result.get("lines");
        assertEquals(2,headers.size());assertEquals(2,lines.get(1).size());assertEquals(1.5,headers.get(0).get("OrderAmount"));assertEquals(12.0,headers.get(0).get("BalWeight"));assertEquals(100.0,headers.get(0).get("CommAmount"));assertEquals(11,lines.get(1).get(1).get("ItemId"));
    }
}
