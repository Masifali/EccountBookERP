package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLedgerRequest;
import com.mst.services.InventoryItemLedgerService;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import java.math.BigDecimal;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

/** Opt-in, read-only comparison against the same desktop stored procedures. No Spring startup/seeding. */
@EnabledIfSystemProperty(named="inventory.live",matches="true")
class InventoryItemLedgerLiveTest {
    @Test void compareDesktopLookupAndLedgerWithJavaProjection() throws Exception {
        Properties props=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){props.load(in);}
        JdbcTemplate jdbc=new JdbcTemplate(new DriverManagerDataSource(props.getProperty("spring.datasource.url"),props.getProperty("spring.datasource.username"),props.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        Map<String,Object> account=jdbc.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName=?","numan");
        UserAccount user=new UserAccount();user.setId(((Number)account.get("ID")).intValue());user.setOrganizationId(((Number)account.get("OrganizationId")).intValue());user.setCompanyId(((Number)account.get("CompanyId")).intValue());
        var repository=new InventoryItemLedgerRepository(jdbc);
        var items=repository.items(user);
        var desktopItems=jdbc.queryForList("EXEC dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId=?, @CompanyId=?, @ActivityType=?",user.getOrganizationId(),user.getCompanyId(),"Items");
        assertEquals(new HashSet<>(desktopItems),new HashSet<>(items));assertFalse(items.isEmpty());
        int item=((Number)items.stream().filter(r->Integer.valueOf(3).equals(r.get("Id"))).findFirst().orElse(items.get(0)).get("Id")).intValue();
        java.sql.Date latest=jdbc.queryForObject("SELECT MAX(CAST(DocDate AS date)) FROM dbo.InventoryStockEvalautionDetail WHERE OrganizationId=? AND CompanyId=? AND ItemId=?",java.sql.Date.class,user.getOrganizationId(),user.getCompanyId(),item);
        assertNotNull(latest);var request=new InventoryItemLedgerRequest();request.setItemId(item);request.setToDate(latest.toLocalDate());request.setFromDate(request.getToDate().minusDays(7));
        var direct=jdbc.queryForList("EXEC dbo.usp_ItemLedgerFromStockEvaluations @OrganizationId=?, @CompanyId=?, @ItemId=?, @FromDate=?, @ToDate=?",user.getOrganizationId(),user.getCompanyId(),item,java.sql.Date.valueOf(request.getFromDate()),java.sql.Date.valueOf(request.getToDate()));
        var ported=repository.load(user,request);assertFalse(direct.isEmpty());assertEquals(direct.size(),ported.size());
        for(int i=0;i<direct.size();i++)for(var entry:direct.get(i).entrySet())assertEquals(entry.getValue() instanceof BigDecimal?((BigDecimal)entry.getValue()).toPlainString():entry.getValue(),ported.get(i).get(entry.getKey()),"Row "+i+", column "+entry.getKey());
        var output=InventoryItemLedgerService.project(ported);
        assertEquals(ported.get(0).get("BalAmount"),((Map<?,?>)output.get("opening")).get("BalAmount"));assertEquals(ported.get(ported.size()-1).get("BalAmount"),((Map<?,?>)output.get("closing")).get("BalAmount"));
        String evidence="Read-only live comparison\nUser="+user.getId()+" Organization="+user.getOrganizationId()+" Company="+user.getCompanyId()+"\nItems="+items.size()+"\nItem="+item+" From="+request.getFromDate()+" To="+request.getToDate()+"\nLedger rows="+ported.size()+"\nEvery returned column matched the desktop procedure. Opening/closing endpoint values matched.\nDesktop GUI comparison remains blocked by the Windows capture helper.\nNo database writes.\n";
        Files.createDirectories(Path.of("migration/inventory/evidence"));Files.writeString(Path.of("migration/inventory/evidence/item-ledger-live.txt"),evidence);
    }
}
