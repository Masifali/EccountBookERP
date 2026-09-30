package com.mst.repositories;

import com.mst.models.*;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="sale.live",matches="true")
class SaleGdnReportLiveTest {
    @Test void everyReturnedReportCellMatchesTheDesktopHistoryProcedure() throws Exception {
        var config=new Properties();try(var input=Files.newInputStream(Path.of("src/main/resources/application.properties"))){config.load(input);}
        var jdbc=new JdbcTemplate(new DriverManagerDataSource(config.getProperty("spring.datasource.url"),config.getProperty("spring.datasource.username"),config.getProperty("spring.datasource.password")));jdbc.setQueryTimeout(120);
        var account=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId FROM UserAccount WHERE UserName='numan'");
        var u=new UserAccount();u.setId(n(account.get("ID")));u.setOrganizationId(n(account.get("OrganizationId")));u.setCompanyId(n(account.get("CompanyId")));u.setBranchesId(n(account.get("BranchesId")));
        var repo=new SaleGdnReportRepository(jdbc);assertTrue(repo.branches(u).stream().anyMatch(b->n(b.get("BranchId"))==u.getBranchesId()));
        assertFalse(repo.lookups(u,","+u.getBranchesId()).isEmpty(),"dropdowns use original GDN data, not empty placeholders");
        var date=LocalDate.of(2026,9,17);var f=new SaleGdnReportFilter(date,date,List.of(u.getBranchesId()),0,0,0,0,0,0,0,0,0,0,0,0,0);
        var actual=repo.history(u,f,","+u.getBranchesId());
        var expected=jdbc.queryForList("EXEC dbo.Sp_InvGdn_History @OrganizationId=?,@CompanyId=?,@GrnDateF=?,@GrnDateT=?,@BranchesIds=?",u.getOrganizationId(),u.getCompanyId(),date.toString(),date.toString(),","+u.getBranchesId());
        assertFalse(expected.isEmpty());assertEquals(expected.size(),actual.size());
        var aliases=Map.ofEntries(Map.entry("PartyName","CompanyName"),Map.entry("PartyReference","SupplierReference"),Map.entry("GPDate","GpDate"),Map.entry("OrderNo","SoOrderNo"),Map.entry("OrderDate","SoDate"),Map.entry("PackingType","PackTypeDesc"),Map.entry("JobLot","JobLotDescription"),Map.entry("PackUom","UOMCode"),Map.entry("LabReportReference","LabReportRef"),Map.entry("FreightAmount","NetPaid"),Map.entry("CityName","AreaCity"),Map.entry("EntryUser","UserNameEusr"),Map.entry("RemarksDetail","CommentsDetail"));
        int cells=0;for(int i=0;i<actual.size();i++)for(var cell:actual.get(i).entrySet()){assertEquals(expected.get(i).get(aliases.getOrDefault(cell.getKey(),cell.getKey())),cell.getValue(),"row "+i+" "+cell.getKey());cells++;}
        Files.writeString(Path.of("migration/sale/evidence/gdn-report-read-parity.txt"),"Read-only GoldenAcedb comparison: "+actual.size()+" rows and "+cells+" cells match the original Sp_InvGdn_History procedure for Numan, organization/company 78, branch 58, 17-Sep-2026. Branch allocation and original lookup procedure returned data.\n");
    }
    private static int n(Object value){return value instanceof Number n?n.intValue():0;}
}
