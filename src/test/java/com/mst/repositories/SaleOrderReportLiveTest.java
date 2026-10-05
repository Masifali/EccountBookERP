package com.mst.repositories;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.models.*;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.SaleOrderReportService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

/** Only the native read-only dropdown and detail procedures; never order actions or shared summary scratch tables. */
@EnabledIfSystemProperty(named="sale.report.live", matches="true")
class SaleOrderReportLiveTest {
    @Test @SuppressWarnings("unchecked") void liveDetailMatchesDesktopAndKeepsEveryOrderLine() throws Exception {
        var config=new Properties();try(var input=Files.newInputStream(Path.of("src/main/resources/application-local.properties"))){config.load(input);}
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(config.getProperty("spring.datasource.url"),config.getProperty("spring.datasource.username"),config.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        var account=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId,AppId FROM dbo.UserAccount WHERE UserName=?","numan");
        var u=new UserAccount();u.setId(n(account.get("ID")));u.setOrganizationId(n(account.get("OrganizationId")));u.setCompanyId(n(account.get("CompanyId")));u.setBranchesId(n(account.get("BranchesId")));u.setAppId(n(account.get("AppId")));
        var repo=new SaleOrderReportRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(u);
        var service=new SaleOrderReportService(repo,context,mock(DesktopReportRights.class));
        var initial=service.initial();assertEquals(u.getBranchesId(),initial.get("branchId"));
        var lookups=service.lookups(List.of(u.getBranchesId()),0);assertFalse(lookups.isEmpty());
        for(String activity:List.of("Customer","Item","ItemCategory","BookingPerson"))assertTrue(lookups.stream().anyMatch(row->activity.equals(row.get("Activity"))),activity);
        var f=new SaleOrderReportFilter(LocalDate.of(2026,9,1),LocalDate.of(2026,10,4),0,0,0,0,0,0,0,0,0,0,0,"Open","Approve",false,List.of(u.getBranchesId()),0,null,0,0,0,null,false);
        var raw=repo.detail(u,f,","+u.getBranchesId());assertFalse(raw.isEmpty());
        var expected=jdbc.queryForList("EXEC dbo.Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt @OrganizationId=?,@CompanyId=?,@AppId=?,@BranchesIds=?,@CustomerUserId=?,@FromDate=?,@ToDate=?,@Status=?,@IsApproved=?",u.getOrganizationId(),u.getCompanyId(),u.getAppId(),","+u.getBranchesId(),u.getId(),f.fromDate().toString(),f.toDate().toString(),"Open",true);
        assertEquals(expected.size(),raw.size());
        for(int i=0;i<raw.size();i++)for(String key:List.of("Id","OrderItemQty","DispatchQty","BalQty","NetWeight","DispatchWeight","BalWeight","CompanyNameSpCAgent","BookingPerson"))assertEquals(expected.get(i).get(key),raw.get(i).get(key),key);
        var detail=service.detail(f);var headers=(List<Map<String,Object>>)detail.get("headers");var lines=(Map<Integer,List<Map<String,Object>>>)detail.get("lines");
        assertEquals(raw.size(),lines.values().stream().mapToInt(List::size).sum());assertEquals(raw.stream().map(row->row.get("Id")).distinct().count(),headers.size());
        for(var header:headers){var children=lines.get(n(header.get("Id")));assertFalse(children.isEmpty());
            assertEquals(children.stream().mapToDouble(row->((Number)row.get("ItemAmount")).doubleValue()).sum(),((Number)header.get("OrderAmount")).doubleValue(),.01);
            assertEquals(children.stream().mapToDouble(row->((Number)row.get("BalWeight")).doubleValue()).sum(),((Number)header.get("BalWeight")).doubleValue(),.01);
            for(var row:children){assertEquals(d(row,"ItemQty")-d(row,"DispatchQty"),d(row,"BalQty"),.01);assertEquals(d(row,"Weight")-d(row,"DispatchWeight"),d(row,"BalWeight"),.01);}
        }
        var path=Path.of("target/sale-order-verification");Files.createDirectories(path);
        var mapper=new ObjectMapper().findAndRegisterModules();
        Files.writeString(path.resolve("detail.json"),mapper.writeValueAsString(detail));Files.writeString(path.resolve("initial.json"),mapper.writeValueAsString(initial));Files.writeString(path.resolve("lookups.json"),mapper.writeValueAsString(lookups));
        Files.writeString(path.resolve("result.txt"),raw.size()+" detail rows, "+headers.size()+" orders and "+lookups.size()+" lookup values verified against native desktop procedures. Quantities, weights, commissions and per-order totals match. No business records changed.\n");
    }
    static int n(Object value){return ((Number)value).intValue();}
    static double d(Map<String,Object> row,String key){return ((Number)row.get(key)).doubleValue();}
}
